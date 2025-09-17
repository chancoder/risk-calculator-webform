using System;
using System.Web;
using Microsoft.AspNetCore.Http;
using RiskCalculatorWebForm.Controls;
using Microsoft.AspNetCore.SystemWebAdapters;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Extensions;


namespace RiskCalculatorWebForm
{
    public partial class CalculateVar : Controller {
        RequestDelegate _next = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Request.Method != "POST")
            {
                // Initialize SessionState tracking
                HttpContext.Session.SetString("SessionStartTime", DateTime.Now.ToString("o"));
                HttpContext.Session.SetInt32("TotalCalculations", 0);

                UpdateStatistics();
            }
            else
            {
                UpdateStatistics();
            }
        }

        protected void VarCalcControl_VaRCalculated(object sender, VaRCalculationEventArgs e)
        {
            // Handle VaR calculation event from the control
            var result = e.Result;

            // Update session statistics
            int totalCalculations = HttpContext.Session.GetInt32("TotalCalculations") ?? 0;
            HttpContext.Session.SetInt32("TotalCalculations", totalCalculations + 1);
            HttpContext.Session.SetString("LastCalculationTime", DateTime.Now.ToString("o"));
            HttpContext.Session.SetString("LastCalculationResult", result.ToString());

            // Update statistics display
            UpdateStatistics();
        }

        protected void VarCalcControl_CalculationReset(object sender, EventArgs e)
        {
            // Handle calculation reset event
            UpdateStatistics();
        }

        private void UpdateStatistics()
        {
            // Update total calculations
            int totalCalculations = HttpContext.Session.GetInt32("TotalCalculations") ?? 0;
            ViewData["TotalCalculations"] = totalCalculations.ToString();

            // Update last calculation time
            var lastCalcTimeStr = HttpContext.Session.GetString("LastCalculationTime");
            if (!string.IsNullOrEmpty(lastCalcTimeStr))
            {
                DateTime lastCalcTime = DateTime.Parse(lastCalcTimeStr);
                ViewData["LastCalculationTime"] = lastCalcTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                ViewData["LastCalculationTime"] = "No calculations yet";
            }

            // Update session duration
            var sessionStartStr = HttpContext.Session.GetString("SessionStartTime");
            if (!string.IsNullOrEmpty(sessionStartStr))
            {
                DateTime sessionStart = DateTime.Parse(sessionStartStr);
                TimeSpan duration = DateTime.Now - sessionStart;
                ViewData["SessionDuration"] = $"{duration.Hours:D2}:{duration.Minutes:D2}:{duration.Seconds:D2}";
            }
        }

        public CalculateVar(RequestDelegate next)
        {
            _next = next;
        }
    }
}
