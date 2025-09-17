using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace RiskCalculatorWebForm.Controls
{
    public partial class VarCalculationControl : ViewComponent
    {
        // Events for parent pages to handle
        public event EventHandler<VaRCalculationEventArgs> VaRCalculated;
        public event EventHandler CalculationReset;

        // Use TempData to store values between requests
        public ITempDataDictionary TempData { get; set; }

        // Access to HTTP context and session
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        private ISession Session => HttpContextAccessor?.HttpContext?.Session;

        // Initialize state dictionary if needed
        private void EnsureStateInitialized()
        {
            if (TempData["VarCalculationState"] == null)
            {
                TempData["VarCalculationState"] = new System.Collections.Generic.Dictionary<string, object>();
            }
        }

        // Helper methods to get/set state
        private void SetState(string key, object value)
        {
            EnsureStateInitialized();
            var state = TempData["VarCalculationState"] as System.Collections.Generic.Dictionary<string, object>;
            state[key] = value;
            TempData["VarCalculationState"] = state;
        }

        private T GetState<T>(string key, T defaultValue = default)
        {
            EnsureStateInitialized();
            var state = TempData["VarCalculationState"] as System.Collections.Generic.Dictionary<string, object>;
            if (state.TryGetValue(key, out object value))
            {
                return (T)value;
            }
            return defaultValue;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContextAccessor?.HttpContext?.Request?.Method != "POST")
            {
                // Initialize state values
                SetState("LastSymbol", string.Empty);
                SetState("LastAmount", 0m);
                SetState("CalculationHistory", new System.Collections.Generic.List<VaRCalculationResult>());

                // Set default values from SessionState if available
                if (Session != null)
                {
                    // Store values in state instead of directly in control properties
                    // which don't exist in .NET Core
                    if (Session.GetString("DefaultSymbol") != null)
                    {
                        SetState("Symbol", Session.GetString("DefaultSymbol"));
                    }
                    if (Session.GetString("DefaultAmount") != null)
                    {
                        SetState("Amount", Session.GetString("DefaultAmount"));
                    }
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            // Store current values in TempData for persistence
            var symbol = GetState<string>("Symbol", string.Empty);
            if (!string.IsNullOrEmpty(symbol))
            {
                SetState("LastSymbol", symbol);
            }

            var amountStr = GetState<string>("Amount", string.Empty);
            if (!string.IsNullOrEmpty(amountStr))
            {
                if (decimal.TryParse(amountStr, out decimal amount))
                {
                    SetState("LastAmount", amount);
                }
            }
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            // Removed Page.IsValid check as Page is not available in ASP.NET Core
            {
                try
                {
                    string symbol = GetState<string>("Symbol", string.Empty).Trim().ToUpper();
                    decimal amount = decimal.Parse(GetState<string>("Amount", "0"));

                    // Store in SessionState for cross-page persistence
                    if (Session != null)
                    {
                        Session.SetString("LastCalculatedSymbol", symbol);
                        Session.SetString("LastCalculatedAmount", amount.ToString());
                    }

                    // Calculate VaR using business logic
                    var riskCalculator = new RiskCalculator();
                    decimal var = riskCalculator.CalculateVaR(symbol, amount);
                    decimal creditRisk = riskCalculator.CalculateCreditRisk(symbol);
                    string riskLevel = riskCalculator.GetRiskLevel(var / amount);

                    // Create result object
                    var result = new VaRCalculationResult
                    {
                        Symbol = symbol,
                        Amount = amount,
                        VaR = var,
                        CreditRisk = creditRisk,
                        RiskLevel = riskLevel,
                        CalculatedAt = DateTime.Now
                    };

                    // Store in TempData for this control's history
                    var history = GetState<System.Collections.Generic.List<VaRCalculationResult>>("CalculationHistory");
                    if (history == null)
                    {
                        history = new System.Collections.Generic.List<VaRCalculationResult>();
                    }
                    history.Add(result);
                    SetState("CalculationHistory", history);

                    // Update calculation count in state
                    int count = GetState<int>("CalculationCount", 0);
                    count++;
                    SetState("CalculationCount", count);

                    // Display results
                    DisplayResults(result);

                    // Store last calculation in state
                    SetState("LastCalculation", $"{symbol}|{amount}|{var}|{creditRisk}|{riskLevel}");

                    // Raise event for parent page
                    VaRCalculated?.Invoke(this, new VaRCalculationEventArgs(result));

                    // Check for high risk and add alert
                    if (var / amount > 0.05m)
                    {
                        var alertService = new RiskAlertService();
                        alertService.AddAlert($"HIGH RISK: {symbol} VaR exceeds 5% threshold at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    }
                }
                catch (Exception ex)
                {
                    ShowError($"An error occurred during calculation: {ex.Message}");
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
            CalculationReset?.Invoke(this, EventArgs.Empty);
        }

        protected void btnNewCalculation_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void DisplayResults(VaRCalculationResult result)
        {
            // Store results in state instead of directly in control properties
            SetState("DisplayedSymbol", result.Symbol);
            SetState("DisplayedAmount", string.Format("${0:N2}", result.Amount));
            SetState("DisplayedVaR", string.Format("${0:N2}", result.VaR));
            SetState("DisplayedCreditRisk", string.Format("{0:F1}/10", result.CreditRisk));
            SetState("DisplayedRiskLevel", result.RiskLevel);
            SetState("DisplayedRiskLevelClass", "risk-level " + result.RiskLevel.ToLower());

            SetState("FormVisible", false);
            SetState("ResultsVisible", true);
            SetState("ErrorVisible", false);
        }

        private void ResetForm()
        {
            SetState("FormVisible", true);
            SetState("ResultsVisible", false);
            SetState("ErrorVisible", false);

            // Restore from SessionState if available
            if (Session != null && Session.GetString("DefaultSymbol") != null)
            {
                SetState("Symbol", Session.GetString("DefaultSymbol"));
            }
            else
            {
                SetState("Symbol", "AAPL");
            }

            if (Session != null && Session.GetString("DefaultAmount") != null)
            {
                SetState("Amount", Session.GetString("DefaultAmount"));
            }
            else
            {
                SetState("Amount", "100000");
            }
        }

        private void ShowError(string message)
        {
            SetState("ErrorMessage", message);
            SetState("ErrorVisible", true);
            SetState("FormVisible", true);
            SetState("ResultsVisible", false);
        }

        // Public properties for parent pages to access
        public string Symbol
        {
            get { return GetState<string>("Symbol", string.Empty); }
            set { SetState("Symbol", value); }
        }

        public decimal Amount
        {
            get
            {
                var amountStr = GetState<string>("Amount", "0");
                return decimal.TryParse(amountStr, out decimal amount) ? amount : 0;
            }
            set { SetState("Amount", value.ToString("F2")); }
        }

        public int CalculationCount
        {
            get { return GetState<int>("CalculationCount", 0); }
        }

        public System.Collections.Generic.List<VaRCalculationResult> CalculationHistory
        {
            get { return GetState<System.Collections.Generic.List<VaRCalculationResult>>("CalculationHistory") ?? new System.Collections.Generic.List<VaRCalculationResult>(); }
        }
    }

    // Event arguments class
    public class VaRCalculationEventArgs : EventArgs
    {
        public VaRCalculationResult Result { get; }

        public VaRCalculationEventArgs(VaRCalculationResult result)
        {
            Result = result;
        }
    }

    // Result class for calculations
    public class VaRCalculationResult
    {
        public string Symbol { get; set; }
        public decimal Amount { get; set; }
        public decimal VaR { get; set; }
        public decimal CreditRisk { get; set; }
        public string RiskLevel { get; set; }
        public DateTime CalculatedAt { get; set; }
    }
}
