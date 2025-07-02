using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class YearClosingController : BaseController
    {
        private readonly IDayEndService dayEndService;
        private readonly IOfficeService officeService;
        private readonly IDayInitialService dayInitialService;
        private readonly IWeeklyReportService weeklyReportService;
        public YearClosingController(IDayEndService dayEndService, IOfficeService officeService, IDayInitialService dayInitialService,IWeeklyReportService weeklyReportService)
          {
              this.dayEndService = dayEndService;
              this.officeService = officeService;
              this.dayInitialService = dayInitialService;
              this.weeklyReportService = weeklyReportService;
          }
        // GET: YearClosing
        public ActionResult Index()
        {
            var model = new YearClosingViewModel();
            if (IsDayInitiated)
                model.BusinessDate = TransactionDate;
            MapDropDownList(model);

            return View(model);
        }

        public ActionResult AddHalfYearly()
        {

            var model = new YearClosingViewModel();
            if (IsDayInitiated)
                model.BusinessDate = TransactionDate;
            MapDropDownList(model);

            return View(model);
        }
        public JsonResult YearClosingProcess(string officeId, string businessDate)
        {
            try
            {
               

                businessDate = Convert.ToString(TransactionDate);
               // dayEndService.PortFOlioYearClosingProcess(SessionHelper.LoginUserOfficeID.Value, Convert.ToDateTime(businessDate), LoggedInOrganizationID);
                var param = new { OfficeID = LoginUserOfficeID, YearEndDate = Convert.ToDateTime(businessDate), OrgID = SessionHelper.LoginUserOrganizationID };
                weeklyReportService.PortFOlioYearClosingProcess(param);
               
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
                //return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult HalfYearlyProcess(string officeId, string businessDate)
        {
            try
            {


                businessDate = Convert.ToString(SessionHelper.TransactionDate);
                // dayEndService.PortFOlioYearClosingProcess(SessionHelper.LoginUserOfficeID.Value, Convert.ToDateTime(businessDate), LoggedInOrganizationID);
                var param = new { OfficeID = LoginUserOfficeID, YearEndDate = Convert.ToDateTime(businessDate), OrgID = SessionHelper.LoginUserOrganizationID, YearStartDate = Convert.ToDateTime(businessDate), OfficeIDTo = LoginUserOfficeID };
                weeklyReportService.HalyYearlyProcess(param);

                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
                //return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        private void MapDropDownList(YearClosingViewModel model)
        {

            var alloffice = officeService.GetAll().Where(d => d.OfficeID == LoginUserOfficeID && d.OrgID == LoggedInOrganizationID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;

        }
        // GET: YearClosing/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: YearClosing/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: YearClosing/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: YearClosing/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: YearClosing/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: YearClosing/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: YearClosing/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
