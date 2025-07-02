using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.Reports;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class AccWeeklyCashFlowController : BaseController
    {
        #region Variables
        private readonly IAccTrxMasterService accTrxMasterService;
        private readonly IAccTrxDetailService accTrxDetailService;
        private readonly IAccChartService accChartService;
        private readonly IAccLastVoucherService accLastVoucherService;
        private readonly IProcessInfoService processInfoService;
        private readonly IAccReportService accReportService;
        private readonly IOfficeService officeService;
        private readonly IWeekNoService weekNoService;

        public AccWeeklyCashFlowController(IAccTrxMasterService accTrxMasterService, IAccTrxDetailService accTrxDetailService, IAccChartService accChartService, IAccLastVoucherService accLastVoucherService, IProcessInfoService processInfoService, IAccReportService accReportService, IOfficeService officeService, IWeekNoService weekNoService)
        {
            this.accTrxMasterService = accTrxMasterService;
            this.accTrxDetailService = accTrxDetailService;
            this.accChartService = accChartService;
            this.accLastVoucherService = accLastVoucherService;
            this.processInfoService = processInfoService;
            this.accReportService = accReportService;
            this.officeService = officeService;
            this.weekNoService = weekNoService;
        }
        #endregion

        #region Methods
        public ActionResult GenerateWeeklyCashFlowReport(string office_id, string from_date, string to_date, string acc_level, string week_no)
        {
            try
            {
                var param = new { office_id = office_id, from_date = from_date.Trim(), to_date = to_date.Trim(), AccLevel = acc_level };
                var allVouchers = accReportService.GetWeeklyCashFlowReport(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("Param_OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("FromDate", from_date.Trim());
                reportParam.Add("ToDate", to_date.Trim());
                reportParam.Add("WeekNo", week_no);
                ReportHelper.PrintReport("rpt_acc_weekly_cashflow.rpt", allVouchers.Tables[0], reportParam);                
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;
            ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");


            var allWeekService = weekNoService.GetAll().Where(w => w.OrgID == LoggedInOrganizationID).OrderBy(o => o.WeekNoSl).ToList();
            var viewWeekService = allWeekService.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.WeekNoID.ToString(),
                Text = string.Format("Week {0} : ({1}) : ({2})", x.WeekNoSl.ToString(), Convert.ToDateTime(x.StartDate).ToString("dd-MMM-yyyy"), Convert.ToDateTime(x.EndDate).ToString("dd-MMM-yyyy"))
            });
            var week_items = new List<SelectListItem>();
            week_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            week_items.AddRange(viewWeekService);
            ViewData["WeekList"] = week_items;

            return View();
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
