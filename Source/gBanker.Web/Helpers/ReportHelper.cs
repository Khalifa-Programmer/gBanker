using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace gBanker.Web.Helpers
{
    public class ReportHelper
    {
        public static void ExportExcelWithSubReportDataOnly(string reportName, DataTable dataSource, Dictionary<string, object> parameters, Dictionary<string, DataTable> subReportDatasources)
        {
            try
            {
                ReportDocument crDocument = new ReportDocument();

                // Load the report
                string strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                crDocument.Load(strReportPathAbsolute);

                // Set the main report data source
                crDocument.SetDataSource(dataSource);

                // Set main report parameters
                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    crDocument.SetParameterValue(kvp.Key, kvp.Value);
                }

                // Set subreport data sources
                if (subReportDatasources != null)
                {
                    foreach (KeyValuePair<string, DataTable> kvp in subReportDatasources)
                    {
                        crDocument.OpenSubreport(kvp.Key).SetDataSource(kvp.Value);
                    }
                }

                // Configure export options
                ExportOptions crExportOptions = crDocument.ExportOptions;
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                crDiskFileDestination.DiskFileName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.xls", Guid.NewGuid());
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.Excel;

                // Export the report
                crDocument.Export();

                // Export data-only to HttpResponse
                ExcelFormatOptions excelFormatOpts = new ExcelFormatOptions();
                excelFormatOpts.ExcelUseConstantColumnWidth = true;
                excelFormatOpts.ShowGridLines = true;
                excelFormatOpts.ExcelTabHasColumnHeadings = true;
                excelFormatOpts.ExcelAreaGroupNumber = 1;
                excelFormatOpts.UsePageRange = true;
                crDocument.ExportToHttpResponse(ExportFormatType.Excel, HttpContext.Current.Response, true, reportName.Replace(".", "").Replace("RPT", "").Replace("rpt", "") + DateTime.Now.ToString("dd_MMM_yyyy_hhmmsszzz"));

                // Clean up resources
                crDocument.Dispose();
                crDocument.Close();
                HttpContext.Current.Response.End();
                System.IO.File.Delete(crDiskFileDestination.DiskFileName);
            }
            catch (Exception ex)
            {
                // Handle exceptions (log, display, etc.)
            }
        }


        public static void ExportExcelDataOnly(string reportName, DataTable dataSource, Dictionary<string, object> parameters)
        {
            try
            {
                ReportDocument crDocument = new ReportDocument();

                // Load the report
                string strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                crDocument.Load(strReportPathAbsolute);

                // Set the main report data source
                crDocument.SetDataSource(dataSource);

                // Set main report parameters
                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    crDocument.SetParameterValue(kvp.Key, kvp.Value);
                }

                // Configure export options
                ExportOptions crExportOptions = crDocument.ExportOptions;
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                crDiskFileDestination.DiskFileName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.xls", Guid.NewGuid());
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.Excel;

                // Export the report
                crDocument.Export();

                // Export data-only to HttpResponse
                ExcelFormatOptions excelFormatOpts = new ExcelFormatOptions();
                excelFormatOpts.ExcelUseConstantColumnWidth = true;
                excelFormatOpts.ShowGridLines = true;
                excelFormatOpts.ExcelTabHasColumnHeadings = true;
                excelFormatOpts.ExcelAreaGroupNumber = 1;
                excelFormatOpts.UsePageRange = true;
                crDocument.ExportToHttpResponse(ExportFormatType.Excel, HttpContext.Current.Response, true, reportName.Replace(".", "").Replace("RPT", "").Replace("rpt", "") + DateTime.Now.ToString("dd_MMM_yyyy_hhmmsszzz"));

                // Clean up resources
                crDocument.Dispose();
                crDocument.Close();
                HttpContext.Current.Response.End();
                System.IO.File.Delete(crDiskFileDestination.DiskFileName);
            }
            catch (Exception ex)
            {
                // Handle exceptions (log, display, etc.)
            }
        }
        public static void ExportExcelWithSubReport(string reportName, DataTable dataSource, Dictionary<string, object> parameters, Dictionary<string, DataTable> subReportDatasources, ReportClass reportClass)
        {
            try
            {


                ReportDocument crDocument = new ReportDocument();

                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                ExcelFormatOptions excelFormatOpts = new ExcelFormatOptions();
                string strFName;
                //All CR file assumed that it resides in the reports folder....
                string strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                //reportClass.Load(strReportPathAbsolute);
                //reportClass.SetDataSource(dataSource);
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
                        crDocument.OpenSubreport(kvp.Key).SetDataSource(kvp.Value);
                    }
                }

                //strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
                //crDiskFileDestination.DiskFileName = strFName;
                //crExportOptions = reportClass.ExportOptions;
                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.xlsx", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = crDocument.ExportOptions;

                /* new */
                excelFormatOpts.ExcelUseConstantColumnWidth = true;
                excelFormatOpts.ShowGridLines = true;
                excelFormatOpts.ExcelTabHasColumnHeadings = true;
                excelFormatOpts.ExcelAreaGroupNumber = 1;
                excelFormatOpts.UsePageRange = true;
                /*end new */


                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.ExcelWorkbook;

                //reportClass.ExportToHttpResponse(ExportFormatType.Excel, HttpContext.Current.Response, true, strFName);
                //reportClass.Dispose();
                //reportClass.Close();
                //HttpContext.Current.Response.End();
                //System.IO.File.Delete(strFName);
                crDocument.Export();
                crDocument.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, HttpContext.Current.Response, true, reportName.Replace(".", "").Replace("RPT", "").Replace("rpt", "") + DateTime.Now.ToString("dd_MMM_yyyy_hhmmsszzz"));
                crDocument.Dispose();
                crDocument.Close();
                HttpContext.Current.Response.End();
                System.IO.File.Delete(strFName);

            }
            catch (Exception ex)
            {


            }

        }

        public static void ExportExcelReport(string reportName, DataTable dataSource, Dictionary<string, object> parameters)
        {
            try
            {

                ReportDocument crDocument = new ReportDocument();

                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                ExcelFormatOptions excelFormatOpts = new ExcelFormatOptions();
                string strFName;
                //All CR file assumed that it resides in the reports folder....
                string strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                crDocument.Load(strReportPathAbsolute);
                crDocument.SetDataSource(dataSource);

                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    crDocument.SetParameterValue(kvp.Key, kvp.Value);
                }

                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.xlsx", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = crDocument.ExportOptions;
                /* new */
                excelFormatOpts.ExcelUseConstantColumnWidth = true;
                excelFormatOpts.ShowGridLines = true;
                excelFormatOpts.ExcelTabHasColumnHeadings = true;
                excelFormatOpts.ExcelAreaGroupNumber = 1;
                excelFormatOpts.UsePageRange = true;
                /*end new */
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.ExcelWorkbook;

                crDocument.Export();
                crDocument.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, HttpContext.Current.Response, true, reportName.Replace(".", "").Replace("RPT", "").Replace("rpt", "") + DateTime.Now.ToString("dd_MMM_yyyy_hhmmsszzz"));
                crDocument.Dispose();
                crDocument.Close();
                HttpContext.Current.Response.End();
                System.IO.File.Delete(strFName);
            }
            catch (Exception ex)
            {


            }

        }
        public static void ExportExcelReportFormated(string reportName, DataTable dataSource, Dictionary<string, object> parameters, string reportTitle)
        {
            try
            {

                ReportDocument crDocument = new ReportDocument();

                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                ExcelFormatOptions excelFormatOpts = new ExcelFormatOptions();
                string strFName;
                //All CR file assumed that it resides in the reports folder....
                string strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                crDocument.Load(strReportPathAbsolute);
                crDocument.SetDataSource(dataSource);

                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    crDocument.SetParameterValue(kvp.Key, kvp.Value);
                }

                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.xlsx", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = crDocument.ExportOptions;
                /* new */
                excelFormatOpts.ExcelUseConstantColumnWidth = true;
                excelFormatOpts.ShowGridLines = true;
                excelFormatOpts.ExcelTabHasColumnHeadings = true;
                excelFormatOpts.ExcelAreaGroupNumber = 1;
                excelFormatOpts.UsePageRange = true;
                /*end new */
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.Excel;

                crDocument.Export();
                crDocument.ExportToHttpResponse(ExportFormatType.Excel, HttpContext.Current.Response, true, reportTitle);
                crDocument.Dispose();
                crDocument.Close();
              
                HttpContext.Current.Response.End();
                System.IO.File.Delete(strFName);
            }
            catch (Exception ex)
            {

            }

        }
        public static void PrintReport(string reportName, DataTable dataSource, Dictionary<string, object> parameters)
        {
            string strReportPathAbsolute = "";
            try
            {
                ReportDocument crDocument = new ReportDocument();

                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                string strFName;
                //All CR file assumed that it resides in the reports folder....
                strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                crDocument.Load(strReportPathAbsolute);
                crDocument.SetDataSource(dataSource);

                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    crDocument.SetParameterValue(kvp.Key, kvp.Value);

                }
                ///strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.xls", Guid.NewGuid());
                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = crDocument.ExportOptions;

                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                //crExportOptions.ExportFormatType = ExportFormatType.Excel;
                crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                crDocument.Export();
                crDocument.Dispose();
                crDocument.Close();
                //Response.ClearContent();
                //Response.ClearHeaders();
                //Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", strFName));
                ///HttpContext.Current.Response.ContentType = "application/xls";
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.WriteFile(strFName);
                HttpContext.Current.Response.End();
                // Response.Close();
                System.IO.File.Delete(strFName);
                crDocument.Close();
                crDocument.Dispose();
                //GC.Collect();

            }
            catch (Exception ex)
            {
                StringBuilder s = new StringBuilder();
                while (ex != null)
                {
                    s.AppendLine("Exception type: " + ex.GetType().FullName);
                    s.AppendLine("Report Path: " + strReportPathAbsolute);
                    s.AppendLine("Message       : " + ex.Message);
                    s.AppendLine("Stacktrace:");
                    s.AppendLine(ex.StackTrace);
                    s.AppendLine();
                    ex = ex.InnerException;
                }
                throw new Exception(s.ToString());
            }
        }
        public static void PrintPdfReport(string reportName, DataTable dataSource, Dictionary<string, object> parameters)
        {
            string strReportPathAbsolute = "";
            try
            {
                ReportDocument crDocument = new ReportDocument();

                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                string strFName;
                //All CR file assumed that it resides in the reports folder....
                strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                crDocument.Load(strReportPathAbsolute);
                crDocument.SetDataSource(dataSource);

                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    crDocument.SetParameterValue(kvp.Key, kvp.Value);
                }
                Random rnd = new Random();
                string fileName = Guid.NewGuid().ToString() + rnd.Next().ToString() + rnd.Next().ToString();
                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.pdf", fileName);
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = crDocument.ExportOptions;
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;

                crDocument.Export();
                crDocument.Dispose();
                crDocument.Close();

                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.WriteFile(strFName);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
                System.IO.File.Delete(strFName);
                crDocument.Close();
                crDocument.Dispose();
            }
            catch (Exception ex)
            {
                StringBuilder s = new StringBuilder();
                while (ex != null)
                {
                    s.AppendLine("Exception type: " + ex.GetType().FullName);
                    s.AppendLine("Report Path: " + strReportPathAbsolute);
                    s.AppendLine("Message       : " + ex.Message);
                    s.AppendLine("Stacktrace:");
                    s.AppendLine(ex.StackTrace);
                    s.AppendLine();
                    ex = ex.InnerException;
                }
                throw new Exception(s.ToString());
            }
        }
        public static void PrintExcelReport(string reportName, DataTable dataSource, Dictionary<string, object> parameters)
        {
            try
            {
                ReportDocument crDocument = new ReportDocument();
                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                ExcelFormatOptions excelFormatOpts = new ExcelFormatOptions();
                string strFName;
                //All CR file assumed that it resides in the reports folder....
                string strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                crDocument.Load(strReportPathAbsolute);
                crDocument.SetDataSource(dataSource);

                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    crDocument.SetParameterValue(kvp.Key, kvp.Value);
                }

                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.xlsx", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = crDocument.ExportOptions;
                
                excelFormatOpts.ExcelUseConstantColumnWidth = true;
                excelFormatOpts.ShowGridLines = true;
                excelFormatOpts.ExcelTabHasColumnHeadings = true;
                excelFormatOpts.ExcelAreaGroupNumber = 1;
                excelFormatOpts.UsePageRange = true;
                
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.ExcelWorkbook;
                
                crDocument.Export();
                crDocument.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, HttpContext.Current.Response, true, reportName.Replace(".", "").Replace("RPT", "").Replace("rpt", "") + DateTime.Now.ToString("dd_MMM_yyyy_hhmmsszzz"));
                crDocument.Dispose();
                crDocument.Close();
                HttpContext.Current.Response.End();
                crDocument.Close();
                crDocument.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void MyPrintReport(string reportName, DataTable dataSource, DataTable subDataSource, Dictionary<string, object> parameters)
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
                crDocument.OpenSubreport("rpt_acc_cashbook_bank.rpt").SetDataSource(subDataSource);
                ///strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.xls", Guid.NewGuid());
                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = crDocument.ExportOptions;
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                ///crExportOptions.ExportFormatType = ExportFormatType.Excel;
                crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                crDocument.Export();
                crDocument.Dispose();
                crDocument.Close();
                //Response.ClearContent();
                // Response.ClearHeaders();
                // Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", strFName));

                ///HttpContext.Current.Response.ContentType = "application/xls";
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.WriteFile(strFName);
                HttpContext.Current.Response.End();
                // Response.Close();
                System.IO.File.Delete(strFName);
                crDocument.Close();
                crDocument.Dispose();
                

            }
            catch (Exception ex)
            {


            }

        }
        public static void PrintWithSubReport(string reportName, DataTable dataSource, Dictionary<string, object> parameters, Dictionary<string, DataTable> subReportDatasources)
        {
            //public static void PrintWithSubReport(string reportName, DataTable dataSource, Dictionary<string, object> parameters, Dictionary<string, DataTable> subReportDatasources, ReportClass reportClass)
            //-----------------------  ///-------------------test
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

                ///strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.xls", Guid.NewGuid());
                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = crDocument.ExportOptions;
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                ///crExportOptions.ExportFormatType = ExportFormatType.Excel;
                crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                crDocument.Export();
                crDocument.Dispose();
                crDocument.Close();
                //Response.ClearContent();
                // Response.ClearHeaders();
                // Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", strFName));
                //HttpContext.Current.Response.ContentType = "application/xls";
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.WriteFile(strFName);
                HttpContext.Current.Response.End();
                // Response.Close();
                System.IO.File.Delete(strFName);
                crDocument.Close();
                crDocument.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static void PrintWithSubReport(string reportName, DataTable dataSource, Dictionary<string, object> parameters, Dictionary<string, DataTable> subReportDatasources, ReportClass reportClass)
        {
            try
            {


                // ReportDocument crDocument = new ReportDocument();

                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                string strFName;
                //All CR file assumed that it resides in the reports folder....
                string strReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
                reportClass.Load(strReportPathAbsolute);
                reportClass.SetDataSource(dataSource);

                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    reportClass.SetParameterValue(kvp.Key, kvp.Value);

                }
                if (subReportDatasources != null)
                {
                    foreach (KeyValuePair<string, DataTable> kvp in subReportDatasources)
                    {
                        //string strSReportPathAbsolute = HttpContext.Current.Server.MapPath("~/Reports/" + kvp.Key);
                        //crDocument.OpenSubreport(strSReportPathAbsolute).SetDataSource(kvp.Value);
                        reportClass.OpenSubreport(kvp.Key).SetDataSource(kvp.Value);
                    }
                }

                ///strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.xls", Guid.NewGuid());
                strFName = HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = reportClass.ExportOptions;
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                ///crExportOptions.ExportFormatType = ExportFormatType.Excel;
                crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                reportClass.Export();
                reportClass.Dispose();
                reportClass.Close();
                //Response.ClearContent();
                // Response.ClearHeaders();
                // Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", strFName));
                ///HttpContext.Current.Response.ContentType = "application/xls";
                //mahi
                HttpContext.Current.Response.ContentType = "application/pdf";
                HttpContext.Current.Response.WriteFile(strFName);
                HttpContext.Current.Response.End();
                // Response.Close();
                System.IO.File.Delete(strFName);
                reportClass.Close();
                reportClass.Dispose();

            }
            catch (Exception ex)
            {
                StringBuilder s = new StringBuilder();
                while (ex != null)
                {
                    s.AppendLine("Exception type: " + ex.GetType().FullName);
                    s.AppendLine("Message       : " + ex.Message);
                    s.AppendLine("Stacktrace:");
                    s.AppendLine(ex.StackTrace);
                    s.AppendLine();
                    ex = ex.InnerException;
                }
            }
        }
        
        //private static string ToMessageAndCompleteStacktrace(this Exception exception)
        //{
        //    Exception e = exception;
        //    StringBuilder s = new StringBuilder();
        //    while (e != null)
        //    {
        //        s.AppendLine("Exception type: " + e.GetType().FullName);
        //        s.AppendLine("Message       : " + e.Message);
        //        s.AppendLine("Stacktrace:");
        //        s.AppendLine(e.StackTrace);
        //        s.AppendLine();
        //        e = e.InnerException;
        //    }
        //    return s.ToString();
        //}

    }
}