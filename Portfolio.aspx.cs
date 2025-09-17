using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiskCalculatorWebForm
{
    public partial class Portfolio : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPortfolioData();
            }
        }
        
        private void LoadPortfolioData()
        {
            var riskCalculator = new RiskCalculator();
            var portfolioData = riskCalculator.GetPortfolioData();
            
            var dt = new DataTable();
            dt.Columns.Add("Symbol", typeof(string));
            dt.Columns.Add("Value", typeof(decimal));
            dt.Columns.Add("DailyVaR", typeof(decimal));
            dt.Columns.Add("RiskPercentage", typeof(decimal));
            
            decimal totalValue = 0;
            decimal totalVaR = 0;
            
            foreach (var item in portfolioData)
            {
                decimal var = riskCalculator.CalculateVaR(item.Key, item.Value);
                decimal riskPct = (var / item.Value) * 100;
                
                dt.Rows.Add(item.Key, item.Value, var, riskPct);
                
                totalValue += item.Value;
                totalVaR += var;
            }
            
            gvPortfolio.DataSource = dt;
            gvPortfolio.DataBind();
            
            // Display summary
            decimal totalRiskPct = (totalVaR / totalValue) * 100;
            lblTotalValue.Text = string.Format("${0:N2}", totalValue);
            lblTotalVaR.Text = string.Format("${0:N2}", totalVaR);
            lblRiskRatio.Text = string.Format("{0:F2}%", totalRiskPct);
        }
    }
}
