using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gBanker.Web.Utility
{
    public class ExcelReortHelper
    {
        //////////////KHALID Print With SubReport TO Excel

        public static void ExcelPrintWithSubReport(string reportName, DataTable dataSource, Dictionary<string, object> parameters, Dictionary<string, DataTable> subReportDatasources)
        {
            try
            {

                ReportDocument crDocument = new ReportDocument();

                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                string strFName;
                //All CR file assumed that it resides in the reports folder....
                string strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                crDocument.Load(strReportPathAbsolute);
                crDocument.SetDataSource(dataSource);

                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    crDocument.SetParameterValue(kvp.Key, kvp.Value);

                }
                if (subReportDatasources != null)
                {
                    foreach (KeyValuePair<string, DataTable> kvp in subReportDatasources)
                    {
                        //string strSReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + kvp.Key);
                        //crDocument.OpenSubreport(strSReportPathAbsolute).SetDataSource(kvp.Value);
                        crDocument.OpenSubreport(kvp.Key).SetDataSource(kvp.Value);
                    }
                }

                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.xls", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = crDocument.ExportOptions;
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.ExcelRecord;
                crDocument.Export();
                crDocument.Dispose();
                crDocument.Close();
                //Response.ClearContent();
                // Response.ClearHeaders();
                // Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", strFName));
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                HttpContext.Current.Response.WriteFile(strFName);
                HttpContext.Current.Response.End();
                // Response.Close();
                System.IO.File.Delete(strFName);

            }
            catch (Exception ex)
            {


            }

        }

        public static void ExcelPrintWithSubReport(string SpName, DataTable dataSource)
        {

            GridView gv = new GridView();
            gv.DataSource = dataSource;
            gv.DataBind();

            SpName = SpName + ".xls";

            HttpContext context = System.Web.HttpContext.Current;
            context.Response.ClearContent();
            context.Response.Buffer = true;
            context.Response.AddHeader("content-disposition", "attachment; filename= CheckData.xls");
            context.Response.ContentType = "application/ms-excel";
            context.Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            context.Response.Output.Write(sw.ToString());
            context.Response.Flush();
            htw.Close();
            sw.Close();
            context.Response.End();

        }// End of Method






        /////// END KHALID Print With SuReport TO Excel


    }// END Class
}// END Namespace