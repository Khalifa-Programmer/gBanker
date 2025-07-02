using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace gBanker.Web.Controllers
{
    public class InsuranceSlotController : BaseController
    {
        #region Variables
        private readonly ILoanTrxService loantrxService;
        private readonly IGroupwiseReportService groupwiseReportService;
        private readonly IMemberService memberService;
        private readonly IOfficeService officeService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly ILoanSummaryService loanSummaryService;

        public InsuranceSlotController(ILoanTrxService loantrxService, IMemberService memberService, IOfficeService officeService, IUltimateReportService ultimateReportService, IGroupwiseReportService groupwiseReportService, ILoanSummaryService loanSummaryService)
        {
            this.loantrxService = loantrxService;
            this.memberService = memberService;
            this.officeService = officeService;
            this.ultimateReportService = ultimateReportService;
            this.groupwiseReportService = groupwiseReportService;
            this.loanSummaryService = loanSummaryService;

        }
        #endregion Variables

        #region Events
        // GET: InsuranceSlot
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GenerateInsuranceSlotList(int jtStartIndex, int jtPageSize, string jtSorting, string Duration, string PaymentFrequency)
        {
            try
            {
                
                var param1 = new { @OrgID = LoggedInOrganizationID, @Office = LoginUserOfficeID, @Duration = Duration, @PaymentFrequency = PaymentFrequency };
                
                List<InsuranceSlot> List_InsuranceSlotViewModel = new List<InsuranceSlot>();
                var empList = ultimateReportService.GetInsuranceSlot(param1);

                List_InsuranceSlotViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new InsuranceSlot
                {
                    InsuranceSlotID = row.Field<int>("InsuranceSlotID"),
                    Duration = row.Field<int>("Duration"),
                    MinAmount = row.Field<decimal>("MinAmount"),
                    MaxAmount = row.Field<decimal>("MaxAmount"),
                    InsuranceRate = row.Field<decimal>("InsuranceRate"),
                    AmountBy = row.Field<decimal>("AmountBy"),
                    InsuarnceDate = row.Field<DateTime>("InsuarnceDate"),
                    IsRunning = row.Field<bool>("IsRunning"),
                    ProductID = row.Field<int>("ProductID"),
                    PaymentFrequency = row.Field<string>("PaymentFrequency"),
                   

                }).ToList();
                 
                var currentPageRecords = List_InsuranceSlotViewModel.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_InsuranceSlotViewModel.LongCount() });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }


        public JsonResult CreateUpdate(

           int      hdnInsuranceSlotID
         , string   txtDuration
         , string   txtMaxAmount
         , string   txtMinAmount
         , string   txtInsuranceRate
 
            )
        {
            string result = "OK";
            try
            {
                var param1 = new {

                    @hdnInsuranceSlotID = hdnInsuranceSlotID,
                    @Office             = LoginUserOfficeID,
                    @Duration           = txtDuration,
                    @MaxAmount          = txtMaxAmount,
                    @MinAmount          = txtMinAmount,
                    @InsuranceRate      = txtInsuranceRate,
                    @InsuarnceDate      = SessionHelper.TransactionDate

                };
                var empList = ultimateReportService.AccessInsuranceSlot(param1);

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        } // END FUNCTION







        #endregion

    }// End of Class
}// End of Namespace