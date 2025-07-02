using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;

using gBanker.Web.Models;

namespace gShakti.UI.Controllers.Accounts.Reports
{
    public class ReportViewerController : Controller
    {
        //
        // GET: /ReportViewer/

        public ActionResult Index()
        {
            //showRPT();
            //return View();
            return File(clsConnection.oStream, "application/pdf"); 
        }

        public void showRPT()
        {
            try
            {
                bool isValid = true;

                string strReportName = System.Web.HttpContext.Current.Session["rptName"].ToString();
                string strempCode = System.Web.HttpContext.Current.Session["empCode"].ToString();
                var rptSource = System.Web.HttpContext.Current.Session["rptSource"];

                if (string.IsNullOrEmpty(strReportName))
                {
                    isValid = false;
                }

                if (isValid)
                {
                    ReportDocument rd = new ReportDocument();
                    //string strPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Accounts//Reports//CrystalReport//" + strReportName;
                    string strPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "Accounts\\Reports\\CrystalReport\\" + strReportName;
                    rd.Load(strPath);

                    //if (rptSource != null && rptSource.GetType().ToString() != "System.String")
                    //    rd.SetDataSource(rptSource);
                    //if (!string.IsNullOrEmpty(strempCode))
                    //    rd.SetParameterValue("empCode", strempCode);


                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");

                    // Clear all sessions value
                    Session["rptName"] = null;
                    Session["empCode"] = null;
                    Session["rptSource"] = null;
                }
                else
                {
                    Response.Write("<H2>Nothing Found; No Report name found</H2>");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }

    }
}
