using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using gBanker.Web.Reports.DataSets;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.IO;
using gBanker.Web.Models;
namespace gBanker.Web.Controllers
{
     
    public class ReportController : Controller
    {
        // GET: ReportD:\gBanker\Source\gBanker.Web\Controllers\ReportController.cs
        public ActionResult Index()
        {
            return View();
        }

        private DataTable Ledger_Report(string from_date, string to_date, string sql)
        {
            DataTable dt = new DataTable();
            SqlDataReader oDbDataReader = null;
            try
            {

                SqlCommand cmd = new SqlCommand(sql, clsConnection.Con);
                cmd.CommandType = CommandType.Text;
                //cmd.Parameters.Add("@from_date", SqlDbType.Date).Value = from_date;
                //cmd.Parameters.Add("@to_date", SqlDbType.Date).Value = to_date;
                //cmd.Parameters.Add("@account_code", SqlDbType.Int).Value = account_code;
                //cmd.Parameters.Add("@office_id", SqlDbType.Int).Value = OfficeId;
                //cmd.Parameters.Add("@Order_Condition", SqlDbType.NVarChar).Value = DBNull.Value;
                clsConnection.Con.Open();
                oDbDataReader = cmd.ExecuteReader();
                dt.Load(oDbDataReader);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oDbDataReader.Close();
                oDbDataReader.Dispose();
                clsConnection.Con.Close();
            }

        }
        [AcceptVerbs(HttpVerbs.Get)]
        
        public ActionResult Print(string reportId,  string reportparams)
        {
            string sql="";
            DataTable dt;
            var ds = new LoanSummary();
            var dsPr = new ProductInfo();
            string  startDate, endDate;
            string[] parameters;
            switch (reportId)
            {
                case "LoanSummary":
                        parameters = reportparams.Split(',');
                        startDate = parameters[0];
                        endDate = parameters[1];
                        PrintReport("TestReport.rpt", ds._LoanSummary, new Dictionary<string, object>());
                        break;
                case "Product":
                        parameters = reportparams.Split(',');
                        startDate = parameters[0];
                        endDate = parameters[1];
                        sql = "select p.ProductID, p.ProductCode, p.ProductName, p.ProductFullNameEng, p.ProductShortNameBng, p.ProductFullNameBng, p.InvestorID,i.InvestorCode, " +
                        " p.InterestRate, p.Duration, p.MainProductCode, p.LoanInstallment, p.InterestInstallment, p.SavingsInstallment, p.MinLimit, p.MaxLimit, p.InterestCalculationMethod, " +
                        " p.PaymentFrequency, p.InsuranceItemCode, p.InsuranceItemRate from Product p inner join Investor i on p.InvestorID=i.InvestorID  where p.IsActive=1";
                        dt = Ledger_Report(startDate, endDate, sql);
                        PrintReport("rptProductInfo.rpt", dt, new Dictionary<string, object>());
                        break;
                case "Employee":
                        parameters = reportparams.Split(',');
                        startDate ="";
                        endDate = "";
                        sql = "exec getEmployeeInfo";
                        dt = Ledger_Report(startDate, endDate, sql);
                        PrintReport("rptEmployeeInfo.rpt", dt, new Dictionary<string, object>());
                        break;
                        default:
                        break;

            }
            return Content(string.Empty);
        }
        /// <summary>
        /// Displays the report based on reportname, datasource and report parameters.
        /// </summary>
        /// <param name="reportName">Name of crystal report to display</param>
        /// <param name="dataSource"> Data source of the report, may be DataSet, DataTable, List etc.</param>
        /// <param name="parameters">Report parameters passed as anonymous type object or any other object.</param>
        protected void PrintReport<T>(string reportName, T dataSource, Dictionary<string, object> parameters)
        {
            try
            {

                ReportDocument crDocument = new ReportDocument();

                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                string strFName;
                //All CR file assumed that it resides in the reports folder....
                string strReportPathAbsolute = Server.MapPath("~/Reports/" + reportName);
                crDocument.Load(strReportPathAbsolute);
                crDocument.SetDataSource(dataSource);

                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    crDocument.SetParameterValue(kvp.Key, kvp.Value);

                }
                strFName = Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = crDocument.ExportOptions;
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                crDocument.Export();
                //Response.ClearContent();
                // Response.ClearHeaders();
                // Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", strFName));
                Response.ContentType = "application/pdf";
                Response.WriteFile(strFName);
                Response.End();
                // Response.Close();
                System.IO.File.Delete(strFName);

            }
            catch (Exception ex)
            {


            }

        }
    }
}