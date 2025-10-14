using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;


namespace risk_calculator_webapp
{
    public class RiskAlertService
    {
        private const string ALERTS_SESSION_KEY = "RiskAlerts";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RiskAlertService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddAlert(string alertMessage)
        {
            var alerts = GetAlerts();
            alerts.Add(alertMessage);
            SetAlerts(alerts);
        }

        public List<string> GetAlerts()
        {
            var alertsJson = _httpContextAccessor.HttpContext.Session.GetString(ALERTS_SESSION_KEY);
            List<string> alerts = null;
            if (!string.IsNullOrEmpty(alertsJson))
            {
                alerts = JsonConvert.DeserializeObject<List<string>>(alertsJson);
            }
            if (alerts == null)
            {
                alerts = new List<string>();
                SetAlerts(alerts);
            }
            return alerts;
        }

        public void ClearAlerts()
        {
            _httpContextAccessor.HttpContext.Session.SetString(ALERTS_SESSION_KEY, JsonConvert.SerializeObject(new List<string>()));
        }

        private void SetAlerts(List<string> alerts)
        {
            _httpContextAccessor.HttpContext.Session.SetString(ALERTS_SESSION_KEY, JsonConvert.SerializeObject(alerts));
        }
    }
}
