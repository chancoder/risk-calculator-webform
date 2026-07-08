using System;
using System.Collections.Generic;
using System.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

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
            var json = _httpContextAccessor.HttpContext.Session.GetString(ALERTS_SESSION_KEY);
            var alerts = json != null ? JsonSerializer.Deserialize<List<string>>(json) : null;
            if (alerts == null)
            {
                alerts = new List<string>();
                SetAlerts(alerts);
            }
            return alerts;
        }

        public void ClearAlerts()
        {
            _httpContextAccessor.HttpContext.Session.SetString(ALERTS_SESSION_KEY, JsonSerializer.Serialize(new List<string>()));
        }

        private void SetAlerts(List<string> alerts)
        {
            _httpContextAccessor.HttpContext.Session.SetString(ALERTS_SESSION_KEY, JsonSerializer.Serialize(alerts));
        }
    }
}
