using System;
using System.Web.UI;

namespace RiskCalculatorWebForm
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblCurrentTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }
}
