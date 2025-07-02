using gBanker.Data;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace gBanker.Web.ApiControllers
{
    public class LoanProposalAPIController : ApiController
    {
        // GET api/<controller>
        private readonly IUltimateReportService ultimateReportService;
        private readonly IOfficeService officeService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmployeeOfficeMappingService employeeOfficeService;
        private readonly ILogger loggger;
        public LoanProposalAPIController(IOfficeService officeService, IUltimateReportService ultimateReportService, UserManager<ApplicationUser> userManager, IEmployeeOfficeMappingService employeeOfficeService, ILogger loggger)
        {
            this.officeService = officeService;
            this.ultimateReportService = ultimateReportService;
            this.userManager = userManager;
            this.employeeOfficeService = employeeOfficeService;
            this.loggger = loggger;
        }
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [System.Web.Mvc.Route("api/LoanProposalAPI/GetValidateLogin")]
        public bool GetValidateLogin(string userName, string password)
        {
            LogRequest();
            var user = userManager.Find(userName, password);
            return user != null;
        }
        [System.Web.Mvc.Route("api/LoanProposalAPI/GetLoggedInuserOfficeList")]
        public List<SelectListItem> GetLoggedInuserOfficeList(string employeeCode)
        {
            LogRequest();
            var officeList = employeeOfficeService.GetEmployeeOfficeMappings(employeeCode).Select(s => new SelectListItem() { Value = s.OfficeID.ToString(), Text = string.Format("{0} - {1}", s.Office.OfficeCode, s.Office.OfficeName) }).ToList();
            return officeList;
        }
        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        [System.Web.Mvc.Route("api/LoanProposalAPI/GetCreateLoanProposalStatus")]
        public string GetCreateLoanProposalStatus(string memberCode, decimal amount, int officeID, int centerID, int productID, int purposeID)
        {
            var logtext = string.Format("Loan Proposal: Member:{0}, amount:{1}, office:{2}, Center: {3}, Product:{4}, Purpose:{5}", memberCode, amount, officeID, centerID, productID, purposeID);
            LogToFile(logtext);
            LogRequest();
            try
            {
                var param = new { @OfficeID = officeID, @MemberCode = memberCode, @CenterID = centerID, @amount = amount, @productID = productID, @purposeID = purposeID };
                ultimateReportService.Set_LoanSummaryProposalAPI(param);
            }
            catch (Exception ex)
            {
                LogToFile(ex.Message);
                return "Error";
            }
            return "Success";
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        private void LogRequest()
        {
            try
            {
                var logObject = Logger.GetLogObject();
                loggger.LogRequest(logObject);
            }
            catch (Exception ex)
            {

            }
        }
        private void LogToFile(string msg)
        {
            var orgName = ConfigurationManager.AppSettings["MobileAPIOrgName"].ToString();
            var fileName = string.Format(@"C:\APILog\{0}", orgName);
            if (!Directory.Exists(fileName))
                Directory.CreateDirectory(fileName);
            fileName = string.Format(@"{0}\Proposal_{1}.txt", fileName, DateTime.Now.ToString("dd_MM_yyyy")); ;
            using (StreamWriter sr = File.AppendText(fileName))
            {
                sr.WriteLine(DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt - ") + msg);
            }
        }
    }
}