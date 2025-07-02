#region Usings

using gBanker.Core.Filters;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Mvc;

#endregion

namespace gBanker.Web.Controllers
{
    public class SMSReportController : BaseController
    {
        #region Private Variabls

        private readonly ICenterService centerService;
        private readonly ILoanCollectionService loanCollectionService;
        private readonly ILoanSummaryService loansummaryService;
        private readonly IMemberService memberService;
        private readonly IOfficeService officeService;
        private readonly IProductService productService;
        private readonly ILoanCollectionReportService loanCollectionReportService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly ISmsLogService smsLogService;
        private readonly ISmsConfigService smsConfigService;
        private readonly ISMSSenderService smsSenderService;
        private static DataSet SMSList;
        private static List<SMSViewModel> List_ViewModel;
        private static string DataSearchType;

        #endregion

        #region Variables        

        public SMSReportController(
            IOfficeService officeService, IProductService productService,           
            ISMSSenderService smsSenderService,
            IUltimateReportService ultimateReportService, ISmsLogService smsLogService)
        {           
            this.officeService = officeService;
            this.productService = productService;           
            this.ultimateReportService = ultimateReportService;
            this.smsLogService = smsLogService;
            this.smsSenderService = smsSenderService;
        }
        
        #endregion

        #region Index

        public ActionResult Index()
        {           
            return View();
        }

        #endregion

        #region Sent Log SMS Summary

        public ActionResult SentLogSMSSummary()
        {
            var currentDate = DateTime.Now;
            var startDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            var fromDate = startDate.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);
            var toDate = startDate.AddMonths(1).AddDays(-1).ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);

            var filter = new BaseSearchFilter 
            {
                StartDateInString = fromDate,
                EndDateInString = toDate,
                OfficeList = new List<SelectListItem> { new SelectListItem { Text = "Nothing Seleted" } }
            };
            return View(filter);
        }

        public async Task<JsonResult> GetSentLogSMSSummary(List<int> officeIds, string DateFromValue, string DateToValue, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                int pageNumber = jtStartIndex + 1;
                var newOfficeIDs = String.Join("_", officeIds);
                var filter = new BaseSearchFilter
                {
                    OfficeIds = newOfficeIDs,
                    StartDateInString = DateFromValue,
                    EndDateInString = DateToValue,
                    PageSize=jtPageSize,
                    PageNumber= pageNumber
                };

                var sentLogSMSSummary = await smsSenderService.GetSentLogSMSSummaryByFilter(filter);
               
                return Json(new { Result = "OK", Records = sentLogSMSSummary, TotalRecordCount = filter.TotalCount, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        #region Private Methods

        #endregion
    }
}