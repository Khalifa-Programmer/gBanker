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
    public class LTSPenaltySlotController : BaseController
    {
        #region Variables
        private readonly ILoanTrxService loantrxService;
        private readonly IGroupwiseReportService groupwiseReportService;
        private readonly IMemberService memberService;
        private readonly IOfficeService officeService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly ILoanSummaryService loanSummaryService;

        public LTSPenaltySlotController(ILoanTrxService loantrxService, IMemberService memberService, IOfficeService officeService, IUltimateReportService ultimateReportService, IGroupwiseReportService groupwiseReportService, ILoanSummaryService loanSummaryService)
        {
            this.loantrxService = loantrxService;
            this.memberService = memberService;
            this.officeService = officeService;
            this.ultimateReportService = ultimateReportService;
            this.groupwiseReportService = groupwiseReportService;
            this.loanSummaryService = loanSummaryService;

        }
        #endregion Variables

        // GET: LTSPenaltySlot
        public ActionResult Index()
        {
            return View();
        }



        public JsonResult GenerateLTSPenaltySlotList(int jtStartIndex, int jtPageSize, string jtSorting, double Duration = 0.00)
        {
            try
            {

                var param1 = new { @Duration = Duration };

                List<LTSPenaltySlotViewModel> List_InsuranceRateViewModel = new List<LTSPenaltySlotViewModel>();
                var empList = ultimateReportService.GetLTSPenaltySlot(param1);

                List_InsuranceRateViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new LTSPenaltySlotViewModel
                {
                    LTSPenaltySlotID = row.Field<int>("LTSPenaltySlotID"),
                    Duration = row.Field<decimal>("Duration"),
                    PaymentFrequency = row.Field<string>("PaymentFrequency"),
                    TermDeposit = row.Field<decimal>("TermDeposit"),
                    CreateDate = row.Field<DateTime>("CreateDate"),
                    Penalty = row.Field<decimal>("Penalty") 

                }).ToList();

                var currentPageRecords = List_InsuranceRateViewModel.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_InsuranceRateViewModel.LongCount() });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }


        public JsonResult CreateUpdate(

           string hdnLTSPenaltySlotID
         , string txtPaymentFrequency
         , string txtDuration
         , string txtTermDeposit
         , string txtPenalty

            )
        {
            string result = "OK";
            try
            {
                var param1 = new
                {
                    @hdnLTSPenaltySlotID = hdnLTSPenaltySlotID,
                    @PaymentFrequency = txtPaymentFrequency,
                    @Duration = txtDuration,
                    @TermDeposit = txtTermDeposit,
                    @Penalty = txtPenalty
                };
                var empList = ultimateReportService.AccessLTSPenaltySlot(param1);

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        } // END FUNCTION


    }// End Class
}// ENd Namespace