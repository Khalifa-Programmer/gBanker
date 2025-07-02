using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;

namespace gBanker.Web.CrystalReport
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportDocument report = (ReportDocument)Session["ReportDocument"];
                Console.WriteLine(report);
                if (report != null)
                {
                    CrystalReportViewer1.ReportSource = report;
                }
            }
        }
    }
}