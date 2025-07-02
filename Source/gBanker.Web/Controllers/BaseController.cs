using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using gBanker.Web.Helpers;
using gBanker.Web.Filters;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using gBanker.Core.Common;
using System.Configuration;
using gBanker.Service.ReportExecutionService;
using gBanker.Service.Helpers;
using gBanker.Core.Utility;

namespace gBanker.Web.Controllers
{
    [Authorize]
    [SessionExpireFilter]
    [DisableCache]
    public class BaseController : Controller
    {      
        public JsonResult GetSuccessMessageResult(string message = "")
        {
            return Json(new { Result = "OK", Message = message.Length == 0 ? "Data saved successfully." : message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSuccessMessageResult(string UserData, string message = "")
        {
            return Json(new { Result = "OK", UserData = UserData, Message = message.Length == 0 ? "Data saved successfully." : message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetErrorMessageResult(string message = "")
        {
            return Json(new { Result = "Error", Message = message.Length == 0 ? "Failed to save data. Please verify all required fields" : message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetErrorMessageResult(Exception ex)
        {
            var msg = ex.Message;
            if (ex.InnerException != null)
                msg = string.Format("Exception: {0}. \n Exception Detail: {1}. \n Source: {2}", msg, ex.InnerException.Message, ex.Source);
            return Json(new { Result = "Error", Message = msg }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDuplicateErrorMessageResult(string message = "")
        {
            return Json(new { Result = "Error", Message = message }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetErrorMessageResult(IEnumerable<ValidationResult> validationResults)
        {
            var msg = "";
            foreach (var validationResult in validationResults)
            {
                string key = validationResult.MemberName ?? string.Empty;
                msg = string.Format("{0}</br>", validationResult.Message);
            }
            return Json(new { Result = "Error", Message = "Please correct the following data. </br>" + msg }, JsonRequestBehavior.AllowGet);
        }
        protected bool IsAuthenticated
        {
            get { return SessionHelper.IsAuthenticated; }
        }
        protected EmployeeViewModel LoggedInEmployee
        {
            get { return SessionHelper.LoggedInEmployee; }
        }
        protected int? LoggedInEmployeeID { get { return SessionHelper.LoggedInEmployeeID; } }
        protected int? LoggedInOrganizationID { get { return SessionHelper.LoginUserOrganizationID; } }

        protected string LoggedInOrganizationCode { get { return SessionHelper.LoggedInOrganizationCode; } }

        protected string UserFullName
        {
            get { return SessionHelper.UserFullName; }
        }
        protected int? LoginUserOfficeID { get { return SessionHelper.LoginUserOfficeID; } }
        protected DateTime TransactionDate
        {
            get { return SessionHelper.TransactionDate; }
        }
        protected string TransactionDay
        {
            get { return SessionHelper.TransactionDay; }
        }
        protected string OrganizationName
        {
            get { return SessionHelper.OrganizationName; }
        }
        protected string ProcessType
        {
            get { return SessionHelper.ProcessType; }
        }
        protected bool IsDayInitiated
        {
            get { return SessionHelper.IsDayInitiated; }
        }  
        
        #region SMS Port
        public string getModemPortNumber()
        {
            string PortNumber = "";
            ConnectionStringSettings PortCon = ConfigurationManager.ConnectionStrings["PortNumber"];
            if (PortCon == null || string.IsNullOrEmpty(PortCon.ConnectionString))
            {
                // string msg ="Please Add Modem Port Number.";
                PortNumber = "COM4";
            }

            PortNumber = PortCon.ToString();

            return PortNumber;
        }

        #endregion

        #region SSRS Report
        protected void PrintSSRSReport(string reportName, ParameterValue[] parameters, string connectionStringName = "gBankerReport")
        {
            var result = "";
            var binaryObj = SSRSReportProcessHelper.RenderReport(reportName, parameters, ref result, connectionStringName);
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(binaryObj);
            Response.End();
        }

        //protected void PrintSSRSReport(string reportName, ParameterValue[] parameters, string connectionStringName = "gBankerReport", string reportFormat = ReportTypeConstants.PDF)
        //{
        //    var result = "";
        //    var contentType = reportFormat == ReportTypeConstants.EXCEL ? "application/excel" : "application/pdf";
        //    var binaryObj = SSRSReportProcessHelper.RenderReport(reportName, reportFormat, parameters, ref result, connectionStringName);
        //    Response.ContentType = contentType;

        //    if (reportFormat == ReportTypeConstants.EXCEL)
        //    {
        //        var fileName = $"{reportName}.xls";
        //        Response.AddHeader("content-disposition", "attachment; filename=" + fileName);

        //        Response.OutputStream.Write(binaryObj, 0, binaryObj.Length);
        //        Response.OutputStream.Flush();
        //        Response.OutputStream.Close();
        //        Response.Flush();
        //        Response.Close();
        //    }
        //    else
        //        Response.BinaryWrite(binaryObj);
        //    Response.End();
        //}
        #endregion

    }
}