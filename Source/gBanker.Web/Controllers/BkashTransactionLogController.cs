using AutoMapper;
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
using System.Globalization;
using gBanker.Data.CodeFirstMigration;

namespace gBanker.Web.Controllers
{
    public class BkashTransactionLogController : BaseController
    {
        private readonly IMemberCategoryService memberCategoryService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IOfficeService officeService;
        public BkashTransactionLogController(IMemberCategoryService memberCategoryService, IUltimateReportService ultimateReportService, IOfficeService officeService)
        {
            this.memberCategoryService = memberCategoryService;
            this.ultimateReportService = ultimateReportService;
            this.officeService = officeService;
        }

        // GET: BkashTransactionLog
        public ActionResult Index()
        {
            return View();
        }
        private static double total;
        public JsonResult GetBkashTransactionLog(string FromDate, string ToDate, string ProdType, int jtPageSize, string jtSorting, string filterColumn, string filterValue, string TypeFilterColumn)
        {
            try
            {
                long TotCount;

                // @OfficeId int, @TrxDate	Date, 	@AccCode varchar(10)
                var officeId = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
                DateTime dt = DateTime.Now;

                var param1 = new { @DateFrom = FromDate, @DateTo = ToDate, @OfficeID = officeId, @ProdType = ProdType };
                var BkashTransactionLog = ultimateReportService.GetBkashTransactionLog(param1);
                // AccCodeViewModel
                List<LoanSummaryViewModel> List_ViewModel = new List<LoanSummaryViewModel>();
                List_ViewModel = BkashTransactionLog.Tables[0].AsEnumerable()
                .Select(row => new LoanSummaryViewModel
                {
                    OfficeCode  = row.Field<string>("OfficeCode"),
                    CenterCode  = row.Field<string>("CenterCode"),
                    MemberCode  = row.Field<string>("MemberCode"),
                    MemberName  = row.Field<string>("MemberName"),
                    ProductCode = row.Field<string>("ProductCode"),
                    PartialAmount = row.Field<decimal>("Amount"),
                    LoanCloseDate = row.Field<DateTime>("BillMonth"),
                    InActiveDate = row.Field<DateTime>("CreateDate")

                }).ToList();
                total = 0.0;
                if (List_ViewModel.Count() > 0)
                {
                    foreach (var v in List_ViewModel)
                    {
                        total = total+ Convert.ToDouble( v.PartialAmount);
                    }

                }
                
                // var memberDetail = memberService.GetMemberDetail(Convert.ToInt16(LoggedInOrganizationID),SessionHelper.LoginUserOfficeID, filterColumn, filterValue, TypeFilterColumn, jtStartIndex, jtSorting, jtPageSize, out TotCount);
                var detail = List_ViewModel.ToList();
                TotCount = detail.Count();
                var currentPageRecords = detail.ToList();
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public JsonResult GetTotal()
        {
            try
            {
                return Json(total,JsonRequestBehavior.AllowGet );
            }
            catch (Exception ex)
            {
                return Json("ERROR", JsonRequestBehavior.AllowGet);
            }

        }// End Function


    }// END CLASS
}// END Namespace