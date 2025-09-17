using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SystemWebAdapters;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace RiskCalculatorWebForm
{
    public partial class Default : Controller
    {
        RequestDelegate _next = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Request.Method != "POST")
            {
                ViewData["CurrentTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ViewData["SessionId"] = HttpContext.Session.Id;

                // Initialize SessionState with default values
                if (HttpContext.Session.GetString("DefaultSymbol") == null)
                {
                    HttpContext.Session.SetString("DefaultSymbol", "AAPL");
                }
                if (HttpContext.Session.GetString("DefaultAmount") == null)
                {
                    HttpContext.Session.SetString("DefaultAmount", "100000");
                }
                if (HttpContext.Session.GetString("DefaultSimulationCount") == null)
                {
                    HttpContext.Session.SetString("DefaultSimulationCount", "1000");
                }

                // Store page visit information in TempData
                TempData["PageVisits"] = 1;
                TempData["FirstVisitTime"] = DateTime.Now;
            }
            else
            {
                // Increment page visits in TempData
                int visits = TempData.ContainsKey("PageVisits") ? (int)TempData["PageVisits"] : 0;
                TempData["PageVisits"] = visits + 1;

                // Update current time
                ViewData["CurrentTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        public Default(RequestDelegate next)
        {
            _next = next;
        }
    }
}
