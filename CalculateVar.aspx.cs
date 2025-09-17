using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RiskCalculatorWebForm
{
    public partial class CalculateVar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Initialize form
                pnlForm.Visible = true;
                pnlResults.Visible = false;
                lblError.Visible = false;
            }
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                string symbol = txtSymbol.Text.Trim().ToUpper();
                decimal amount;
                
                if (!decimal.TryParse(txtAmount.Text, out amount))
                {
                    ShowError("Please enter a valid amount.");
                    return;
                }
                
                if (amount <= 0)
                {
                    ShowError("Amount must be greater than zero.");
                    return;
                }
                
                if (string.IsNullOrEmpty(symbol))
                {
                    ShowError("Please enter a stock symbol.");
                    return;
                }
                
                // Calculate VaR using business logic
                var riskCalculator = new RiskCalculator();
                decimal var = riskCalculator.CalculateVaR(symbol, amount);
                decimal creditRisk = riskCalculator.CalculateCreditRisk(symbol);
                string riskLevel = riskCalculator.GetRiskLevel(var / amount);
                
                // Display results
                lblSymbol.Text = symbol;
                lblAmount.Text = string.Format("${0:N2}", amount);
                lblVaR.Text = string.Format("${0:N2}", var);
                lblCreditRisk.Text = string.Format("{0:F1}/10", creditRisk);
                lblRiskLevel.Text = riskLevel;
                lblRiskLevel.CssClass = "risk-level " + riskLevel.ToLower();
                
                // Check for high risk and add alert
                if (var / amount > 0.05m)
                {
                    var alertService = new RiskAlertService();
                    alertService.AddAlert(string.Format("HIGH RISK: {0} VaR exceeds 5% threshold at {1}", 
                        symbol, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                }
                
                pnlForm.Visible = false;
                pnlResults.Visible = true;
                lblError.Visible = false;
            }
            catch (Exception ex)
            {
                ShowError("An error occurred during calculation: " + ex.Message);
            }
        }
        
        protected void btnNewCalculation_Click(object sender, EventArgs e)
        {
            pnlForm.Visible = true;
            pnlResults.Visible = false;
            lblError.Visible = false;
            txtSymbol.Text = "AAPL";
            txtAmount.Text = "100000";
        }
        
        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
            pnlForm.Visible = true;
            pnlResults.Visible = false;
        }
    }
}
