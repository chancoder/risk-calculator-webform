using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace RiskCalculatorWebForm
{
    public partial class MonteCarlo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                pnlResults.Visible = false;
                lblError.Visible = false;
            }
        }
        
        protected void btnRunSimulation_Click(object sender, EventArgs e)
        {
            try
            {
                int simulations;
                if (!int.TryParse(txtSimulations.Text, out simulations) || simulations <= 0)
                {
                    ShowError("Please enter a valid number of simulations.");
                    return;
                }
                
                if (simulations > 10000)
                {
                    ShowError("Maximum 10,000 simulations allowed for performance reasons.");
                    return;
                }
                
                // Run Monte Carlo simulation
                var monteCarloService = new MonteCarloService();
                var results = monteCarloService.RunSimulation(simulations);
                
                // Display results
                lblSimulationCount.Text = simulations.ToString();
                lblSimulationsRun.Text = simulations.ToString();
                lblVaR95.Text = string.Format("{0:F4}%", results.VaR95 * 100);
                lblVaR99.Text = string.Format("{0:F4}%", results.VaR99 * 100);
                lblExpectedReturn.Text = string.Format("{0:F4}%", results.ExpectedReturn * 100);
                
                pnlResults.Visible = true;
                lblError.Visible = false;
            }
            catch (Exception ex)
            {
                ShowError("An error occurred during simulation: " + ex.Message);
            }
        }
        
        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
            pnlResults.Visible = false;
        }
    }
}
