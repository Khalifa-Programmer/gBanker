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
    public class InterestRateController : BaseController
    {

        #region Variables
        private readonly ILoanTrxService loantrxService;
        private readonly IGroupwiseReportService groupwiseReportService;
        private readonly IMemberService memberService;
        private readonly IOfficeService officeService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly ILoanSummaryService loanSummaryService;

        public InterestRateController(ILoanTrxService loantrxService, IMemberService memberService, IOfficeService officeService, IUltimateReportService ultimateReportService, IGroupwiseReportService groupwiseReportService, ILoanSummaryService loanSummaryService)
        {
            this.loantrxService = loantrxService;
            this.memberService = memberService;
            this.officeService = officeService;
            this.ultimateReportService = ultimateReportService;
            this.groupwiseReportService = groupwiseReportService;
            this.loanSummaryService = loanSummaryService;

        }
        #endregion Variables




        // GET: InsuranceRate
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GenerateInsuranceRateList(int jtStartIndex, int jtPageSize, string jtSorting, double Duration = 0.00, double InterestRate = 0.00, string EffectDate = " ", double EffextYear = 0.00)
        {
            try
            {

                var param1 = new { @Duration = Duration, @InterestRate = InterestRate, @EffectDate = EffectDate, @EffextYear = EffextYear };

                List<InterestRateViewModel> List_InsuranceRateViewModel = new List<InterestRateViewModel>();
                var empList = ultimateReportService.GetInsuranceRate(param1);

                List_InsuranceRateViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new InterestRateViewModel
                {
                    InterestRateID = row.Field<int>("InterestRateID"),
                    Duration = row.Field<decimal>("Duration"),
                    InterestRates = row.Field<decimal>("InterestRate"),
                    EffectDate = row.Field<DateTime>("EffectDate"),
                    EffectDateMsg = row.Field<string>("EffectDateMsg"),
                    EffextYear = row.Field<decimal>("EffextYear"),
                    EStatus = row.Field<int>("EStatus")
                  
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

           string hdnInterestRateID
         , string txtDuration
         , string txtInterestRate
         , string txtEffectDate
         , string txtEffextYear

            )
        {
            string result = "OK";
            try
            {
                var param1 = new
                {

                    hdnInterestRateID = hdnInterestRateID,
                    @Duration           = txtDuration             ,
                    @InterestRate       = txtInterestRate         ,
                    @EffectDate         = txtEffectDate           ,
                    @EffextYear         = txtEffextYear           

                };
                var empList = ultimateReportService.AccessInsuranceRate(param1);

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        } // END FUNCTION




    }// End Class
}// End Namespace