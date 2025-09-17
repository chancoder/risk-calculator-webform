using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNetCore.Http;

using System.Threading.Tasks;


namespace RiskCalculatorWebForm
{
    public partial class RiskAlerts     {
        // UI Controls
        protected dynamic pnlNoAlerts { get; set; }
        protected dynamic pnlAlertsList { get; set; }
        protected dynamic rptAlerts { get; set; }
        protected dynamic lblAlertCount { get; set; }
        protected dynamic lblLastUpdated { get; set; }
        protected dynamic btnClearAlerts { get; set; }
RequestDelegate _next = null;        private RiskAlertService _alertService;

        protected void Page_Load(object sender, EventArgs e)
        {
            _alertService = new RiskAlertService();

            // In ASP.NET Core, simply load the alerts each time
            LoadAlerts();
        }

        private void LoadAlerts()
        {
            var alerts = _alertService.GetAlerts();

            if (alerts.Count == 0)
            {
                pnlNoAlerts.Visible = true;
                pnlAlertsList.Visible = false;
            }
            else
            {
                pnlNoAlerts.Visible = false;
                pnlAlertsList.Visible = true;

                // Create alert objects with timestamps for display
                var alertObjects = new List<object>();
                foreach (var alert in alerts)
                {
                    alertObjects.Add(new { Message = alert, Time = DateTime.Now.ToString("HH:mm:ss") });
                }

                rptAlerts.DataSource = alertObjects;
                rptAlerts.DataBind();
            }

            lblAlertCount.Text = alerts.Count.ToString();
            lblLastUpdated.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        protected void btnClearAlerts_Click(object sender, EventArgs e)
        {
            _alertService.ClearAlerts();
            LoadAlerts();
        }

        public RiskAlerts(RequestDelegate next)
        {
            _next = next;
        }
    }
}
