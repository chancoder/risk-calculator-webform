using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;

// Define necessary types that were originally in System.Web.UI.WebControls
namespace System.Web.UI.WebControls
{
    public class GridView
    {
        public object DataSource { get; set; }
        public void DataBind() { }
    }

    public class Label
    {
        public string Text { get; set; }
    }

    public class GridViewRow
    {
        public DataControlRowType RowType { get; set; }
        public object DataItem { get; set; }
    }

    public enum DataControlRowType
    {
        DataRow,
        Header,
        Footer,
        Separator,
        Pager
    }

    public class GridViewRowEventArgs : EventArgs
    {
        public GridViewRow Row { get; set; }
    }
}

namespace RiskCalculatorWebForm.Controls
{
    public partial class PortfolioGridControl : System.Object
    {
        // Events for parent pages to handle
        public event EventHandler<PortfolioDataEventArgs> PortfolioDataLoaded;

        // ViewState replacement for ASP.NET Core
        private Dictionary<string, object> ViewState { get; } = new Dictionary<string, object>();

        // Session accessor
        private IHttpContextAccessor _httpContextAccessor;

        // UI Controls
        private System.Web.UI.WebControls.GridView gvPortfolio { get; set; } = new System.Web.UI.WebControls.GridView();
        private System.Web.UI.WebControls.Label lblTotalValue { get; set; } = new System.Web.UI.WebControls.Label();
        private System.Web.UI.WebControls.Label lblTotalVaR { get; set; } = new System.Web.UI.WebControls.Label();
        private System.Web.UI.WebControls.Label lblRiskRatio { get; set; } = new System.Web.UI.WebControls.Label();
        private System.Web.UI.WebControls.Label lblLastUpdated { get; set; } = new System.Web.UI.WebControls.Label();

        public PortfolioGridControl(IHttpContextAccessor httpContextAccessor = null)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // In ASP.NET Core, we need to manage page state differently
            bool isFirstLoad = true; // Default to first load

            if (isFirstLoad)
            {
                // Initialize ViewState
                ViewState["PortfolioData"] = new Dictionary<string, decimal>();
                ViewState["LastCalculationTime"] = DateTime.Now;

                LoadPortfolioData();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            // Store portfolio data in hidden field for client-side access if needed
            if (ViewState["PortfolioData"] != null)
            {
                var portfolioData = ViewState["PortfolioData"] as Dictionary<string, decimal>;
                // Store serialized data in TempData instead of hidden field
                if (_httpContextAccessor?.HttpContext != null)
                {
                    _httpContextAccessor.HttpContext.Items["PortfolioDataSerialized"] = SerializePortfolioData(portfolioData);
                }
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPortfolioData();
        }

        protected void gvPortfolio_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
            {
                // Add any row-specific logic here
                var dataRow = e.Row.DataItem as DataRowView;
                if (dataRow != null)
                {
                    // You can access row data here if needed
                }
            }
        }

        private void LoadPortfolioData()
        {
            try
            {
                var riskCalculator = new RiskCalculator();
                var portfolioData = riskCalculator.GetPortfolioData();

                // Store in ViewState for persistence
                ViewState["PortfolioData"] = portfolioData;
                ViewState["LastCalculationTime"] = DateTime.Now;

                // Store in SessionState for cross-page access
                if (_httpContextAccessor?.HttpContext?.Session != null)
                {
                    var serializedData = JsonConvert.SerializeObject(portfolioData);
                    _httpContextAccessor.HttpContext.Items["PortfolioData"] = serializedData;
                    _httpContextAccessor.HttpContext.Items["PortfolioLastUpdated"] = DateTime.Now;
                }

                var dt = new DataTable();
                dt.Columns.Add("Symbol", typeof(string));
                dt.Columns.Add("Value", typeof(decimal));
                dt.Columns.Add("DailyVaR", typeof(decimal));
                dt.Columns.Add("RiskPercentage", typeof(decimal));
                dt.Columns.Add("RiskLevel", typeof(string));

                decimal totalValue = 0;
                decimal totalVaR = 0;

                foreach (var item in portfolioData)
                {
                    decimal var = riskCalculator.CalculateVaR(item.Key, item.Value);
                    decimal riskPct = (var / item.Value) * 100;
                    string riskLevel = riskCalculator.GetRiskLevel(var / item.Value);

                    dt.Rows.Add(item.Key, item.Value, var, riskPct, riskLevel);

                    totalValue += item.Value;
                    totalVaR += var;
                }

                gvPortfolio.DataSource = dt;
                gvPortfolio.DataBind();

                // Display summary
                decimal totalRiskPct = totalValue > 0 ? (totalVaR / totalValue) * 100 : 0;
                lblTotalValue.Text = string.Format("${0:N2}", totalValue);
                lblTotalVaR.Text = string.Format("${0:N2}", totalVaR);
                lblRiskRatio.Text = string.Format("{0:F2}%", totalRiskPct);
                lblLastUpdated.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                // Store in context items instead of hidden fields
                if (_httpContextAccessor?.HttpContext != null)
                {
                    _httpContextAccessor.HttpContext.Items["LastRefreshTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }

                // Create event args and raise event
                var eventArgs = new PortfolioDataEventArgs
                {
                    TotalValue = totalValue,
                    TotalVaR = totalVaR,
                    RiskRatio = totalRiskPct,
                    Holdings = portfolioData,
                    LastUpdated = DateTime.Now
                };

                PortfolioDataLoaded?.Invoke(this, eventArgs);
            }
            catch (Exception ex)
            {
                // Handle error - you might want to show a message to the user
                System.Diagnostics.Debug.WriteLine($"Error loading portfolio data: {ex.Message}");
            }
        }

        private string SerializePortfolioData(Dictionary<string, decimal> portfolioData)
        {
            // Simple serialization for hidden field storage
            var items = new List<string>();
            foreach (var item in portfolioData)
            {
                items.Add($"{item.Key}:{item.Value}");
            }
            return string.Join("|", items);
        }

        // Public properties for parent pages to access
        public Dictionary<string, decimal> PortfolioData
        {
            get
            {
                if (ViewState.TryGetValue("PortfolioData", out var data))
                {
                    return data as Dictionary<string, decimal> ?? new Dictionary<string, decimal>();
                }
                return new Dictionary<string, decimal>();
            }
        }

        public DateTime LastCalculationTime
        {
            get
            {
                if (ViewState.TryGetValue("LastCalculationTime", out var time) && time != null)
                    return (DateTime)time;
                return DateTime.MinValue;
            }
        }

        public decimal TotalValue
        {
            get
            {
                var portfolioData = PortfolioData;
                return portfolioData.Values.Sum();
            }
        }

        public decimal TotalVaR
        {
            get
            {
                var riskCalculator = new RiskCalculator();
                var portfolioData = PortfolioData;
                decimal totalVaR = 0;

                foreach (var item in portfolioData)
                {
                    totalVaR += riskCalculator.CalculateVaR(item.Key, item.Value);
                }

                return totalVaR;
            }
        }
    }

    // Event arguments class
    public class PortfolioDataEventArgs : EventArgs
    {
        public decimal TotalValue { get; set; }
        public decimal TotalVaR { get; set; }
        public decimal RiskRatio { get; set; }
        public Dictionary<string, decimal> Holdings { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
