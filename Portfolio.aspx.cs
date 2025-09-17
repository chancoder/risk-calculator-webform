using System;
using System.Collections.Generic;
using System.Linq;
using RiskCalculatorWebForm.Controls;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;


namespace RiskCalculatorWebForm
{
    public partial class Portfolio     {
        RequestDelegate _next = null;
        private HttpContext _httpContext;
        private Dictionary<string, object> _viewState = new Dictionary<string, object>();

        // Properties to replace WebForm controls
        public string LastUpdatedText { get; private set; }
        public string HoldingsCountText { get; private set; }
        public string HighestRiskText { get; private set; }
        public string LowestRiskText { get; private set; }

        public Portfolio(RequestDelegate next)
        {
            _next = next;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _httpContext = (HttpContext)sender;
            if (_httpContext.Request.Method != "POST")
            {
                // Initialize _viewState for portfolio insights
                _viewState["PortfolioInsights"] = new Dictionary<string, object>();
                _viewState["LastAnalysisTime"] = DateTime.Now;

                // Initialize SessionState for portfolio tracking
                if (_httpContext.Session.GetInt32("PortfolioViewCount") == null)
                {
                    _httpContext.Session.SetInt32("PortfolioViewCount", 0);
                }

                UpdatePortfolioInsights();
            }
            else
            {
                UpdatePortfolioInsights();
            }
        }

        protected void PortfolioGridControl_PortfolioDataLoaded(object sender, PortfolioDataEventArgs e)
        {
            // Handle portfolio data loaded event from the control
            var portfolioData = e.Holdings;

            // Update session statistics
            int viewCount = _httpContext.Session.GetInt32("PortfolioViewCount") ?? 0;
            _httpContext.Session.SetInt32("PortfolioViewCount", viewCount + 1);
            _httpContext.Items["LastPortfolioView"] = DateTime.Now;
            _httpContext.Items["LastPortfolioData"] = portfolioData;

            // Store insights in _viewState
            var insights = new Dictionary<string, object>
            {
                ["TotalValue"] = e.TotalValue,
                ["TotalVaR"] = e.TotalVaR,
                ["RiskRatio"] = e.RiskRatio,
                ["HoldingsCount"] = portfolioData.Count,
                ["LastUpdated"] = e.LastUpdated
            };

            // Find highest and lowest risk holdings
            var riskCalculator = new RiskCalculator();
            var riskAnalysis = new List<(string Symbol, decimal RiskRatio)>();

            foreach (var holding in portfolioData)
            {
                decimal var = riskCalculator.CalculateVaR(holding.Key, holding.Value);
                decimal riskRatio = (var / holding.Value) * 100;
                riskAnalysis.Add((holding.Key, riskRatio));
            }

            if (riskAnalysis.Any())
            {
                var highestRisk = riskAnalysis.OrderByDescending(x => x.RiskRatio).First();
                var lowestRisk = riskAnalysis.OrderBy(x => x.RiskRatio).First();

                insights["HighestRiskHolding"] = $"{highestRisk.Symbol} ({highestRisk.RiskRatio:F2}%)";
                insights["LowestRiskHolding"] = $"{lowestRisk.Symbol} ({lowestRisk.RiskRatio:F2}%)";
            }

            _viewState["PortfolioInsights"] = insights;
            _viewState["LastAnalysisTime"] = DateTime.Now;

            // Update display
            UpdatePortfolioInsights();
        }

        private void UpdatePortfolioInsights()
        {
            var insights = _viewState.ContainsKey("PortfolioInsights") ?
                _viewState["PortfolioInsights"] as Dictionary<string, object> : null;

            if (insights != null && insights.Count > 0)
            {
                LastUpdatedText = insights.ContainsKey("LastUpdated") ?
                    ((DateTime)insights["LastUpdated"]).ToString("yyyy-MM-dd HH:mm:ss") :
                    "Never";

                HoldingsCountText = insights.ContainsKey("HoldingsCount") ?
                    insights["HoldingsCount"].ToString() : "0";

                HighestRiskText = insights.ContainsKey("HighestRiskHolding") ?
                    insights["HighestRiskHolding"].ToString() : "N/A";

                LowestRiskText = insights.ContainsKey("LowestRiskHolding") ?
                    insights["LowestRiskHolding"].ToString() : "N/A";
            }
            else
            {
                LastUpdatedText = "No data available";
                HoldingsCountText = "0";
                HighestRiskText = "N/A";
                LowestRiskText = "N/A";
            }
        }
    }
}
