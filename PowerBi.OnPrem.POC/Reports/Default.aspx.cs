using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PowerBi.OnPrem.POC.Reports
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("/Account/Login");
            }
            var reportPath = Request.QueryString["ReportPath"];
            IFrame.Src = $"http://pwcsubk/Reports/powerbi{reportPath}?rs:Embed=true";
            //ReportViewer1.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportViewer_Server_Url"]);
            //if (!string.IsNullOrEmpty(reportPath))
            //{
            //    ReportViewer1.ServerReport.ReportPath = reportPath;
            //}
        }
    }
}