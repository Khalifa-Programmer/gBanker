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
    public class HalfYearlySavingInterestController :  BaseController
    {
        private readonly IDayEndService dayEndService;
        private readonly IOfficeService officeService;
        private readonly IDayInitialService dayInitialService;
        private readonly IWeeklyReportService weeklyReportService;
        public HalfYearlySavingInterestController(IDayEndService dayEndService, IOfficeService officeService, IDayInitialService dayInitialService, IWeeklyReportService weeklyReportService)
          {
              this.dayEndService = dayEndService;
              this.officeService = officeService;
              this.dayInitialService = dayInitialService;
              this.weeklyReportService = weeklyReportService;
          }

        //
        // GET: /HalfYearlySavingInterest/
        public ActionResult Index()
        {
            var model = new HalfYearlySavingInterestViewModel();
            if (IsDayInitiated)
                model.BusinessDate = TransactionDate;
                MapDropDownList(model);
                return View(model);
        }
        private void MapDropDownList(HalfYearlySavingInterestViewModel model)
        {

            var alloffice = officeService.GetAll().Where(d => d.OfficeID == LoginUserOfficeID && d.OrgID == LoggedInOrganizationID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;

        }
        public JsonResult HalyYearlySavingInterest(string officeId, string businessDate, string checkInfo)
        {
            try
            {
                //if (!IsDayInitiated)
                //{
                //    return GetErrorMessageResult("Please run the start work process");
                //}

                DateTime vdate = Convert.ToDateTime(businessDate);
                if (checkInfo == "1")
                {
                    var param = new { OfficeID = 0000, YearEndDate = businessDate, OrgID = SessionHelper.LoginUserOrganizationID, YearStartDate = vdate, OfficeIDTo = 99999 };
                    weeklyReportService.HalYearlySavingInterest(param);
                }

                else
                {
                    var param1 = new { OfficeID = LoginUserOfficeID, YearEndDate = businessDate, OrgID = SessionHelper.LoginUserOrganizationID, YearStartDate = vdate, OfficeIDTo = LoginUserOfficeID };
                    weeklyReportService.HalYearlySavingInterest(param1);
                }
              

                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
                //return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //
        // GET: /HalfYearlySavingInterest/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /HalfYearlySavingInterest/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /HalfYearlySavingInterest/Create
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

        //
        // GET: /HalfYearlySavingInterest/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /HalfYearlySavingInterest/Edit/5
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

        //
        // GET: /HalfYearlySavingInterest/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /HalfYearlySavingInterest/Delete/5
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
