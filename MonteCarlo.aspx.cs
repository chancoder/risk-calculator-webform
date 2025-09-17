using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SystemWebAdapters;
using RiskCalculatorWebForm.Controls;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;


namespace RiskCalculatorWebForm
{
    public partial class MonteCarlo : Controller {
        private RequestDelegate _next = null;
        private ITempDataDictionary _tempData;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Request.Method != "POST")
            {
                // Initialize TempData for simulation statistics
                TempData["SimulationStatistics"] = new Dictionary<string, object>();
                TempData["SimulationTimes"] = new List<long>();

                // Initialize SessionState for simulation tracking
                if (HttpContext.Session.GetInt32("TotalSimulations") == null)
                {
                    HttpContext.Session.SetInt32("TotalSimulations", 0);
                }
                if (HttpContext.Session.GetString("SimulationHistory") == null)
                {
                    HttpContext.Session.SetString("SimulationHistory", JsonConvert.SerializeObject(new List<MonteCarloSimulationResult>()));
                }

                UpdateStatistics();
            }
            else
            {
                UpdateStatistics();
            }
        }

        protected void MonteCarloControl_SimulationCompleted(object sender, MonteCarloEventArgs e)
        {
            // Handle simulation completed event from the control
            var result = e.Result;

            // Update session statistics
            int totalSimulations = HttpContext.Session.GetInt32("TotalSimulations") ?? 0;
            HttpContext.Session.SetInt32("TotalSimulations", totalSimulations + 1);
            HttpContext.Session.SetString("LastSimulationTime", DateTime.Now.ToString("o"));
            HttpContext.Session.SetString("LastSimulationResult", JsonConvert.SerializeObject(result));

            // Update simulation history in SessionState
            var historyJson = HttpContext.Session.GetString("SimulationHistory");
            var history = string.IsNullOrEmpty(historyJson)
                ? new List<MonteCarloSimulationResult>()
                : JsonConvert.DeserializeObject<List<MonteCarloSimulationResult>>(historyJson);

            if (history == null)
            {
                history = new List<MonteCarloSimulationResult>();
            }
            history.Insert(0, result);

            // Keep only last 20 simulations in session history
            if (history.Count > 20)
            {
                history = history.Take(20).ToList();
            }
            HttpContext.Session.SetString("SimulationHistory", JsonConvert.SerializeObject(history));

            // Update TempData with simulation times
            var simulationTimes = TempData["SimulationTimes"] as List<long>;
            if (simulationTimes == null)
            {
                simulationTimes = new List<long>();
            }
            simulationTimes.Add(result.ExecutionTimeMs);
            TempData["SimulationTimes"] = simulationTimes;

            // Update statistics display
            UpdateStatistics();
        }

        private void UpdateStatistics()
        {
            // Update total simulations
            int totalSimulations = HttpContext.Session.GetInt32("TotalSimulations") ?? 0;
            TempData["TotalSimulations"] = totalSimulations.ToString();

            // Update last simulation time
            var lastSimTimeStr = HttpContext.Session.GetString("LastSimulationTime");
            if (!string.IsNullOrEmpty(lastSimTimeStr))
            {
                DateTime lastSimTime = DateTime.Parse(lastSimTimeStr);
                TempData["LastSimulationTime"] = lastSimTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                TempData["LastSimulationTime"] = "No simulations yet";
            }

            // Calculate and display simulation time statistics
            var simulationTimes = TempData["SimulationTimes"] as List<long>;
            if (simulationTimes != null && simulationTimes.Count > 0)
            {
                double averageTime = simulationTimes.Average();
                long fastestTime = simulationTimes.Min();

                TempData["AverageSimulationTime"] = $"{averageTime:F0} ms";
                TempData["FastestSimulation"] = $"{fastestTime} ms";
            }
            else
            {
                TempData["AverageSimulationTime"] = "N/A";
                TempData["FastestSimulation"] = "N/A";
            }
        }
        public MonteCarlo(RequestDelegate next, ITempDataDictionaryFactory tempDataFactory, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _tempData = tempDataFactory.GetTempData(httpContextAccessor.HttpContext);
        }
    }
}
