using gBanker.Service;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class PreClosingController : BaseController
    {
        private readonly IDayEndService dayEndService;
        private readonly IOfficeService officeService;
        private readonly IDayInitialService dayInitialService;
        public PreClosingController(IDayEndService dayEndService, IOfficeService officeService, IDayInitialService dayInitialService)
          {
              this.dayEndService = dayEndService;
              this.officeService = officeService;
              this.dayInitialService = dayInitialService;
          }
        // GET: PreClosing
        public ActionResult Index()
        {
            var model = new DayEndViewModel();
            if (IsDayInitiated)
                model.BusinessDate = TransactionDate;
                MapDropDownList(model);
                return View(model);
        }
        public JsonResult PreYearClosing(string officeId, string businessDate)
        {
            try
            {
                //if (!IsDayInitiated)
                //{
                //    return GetErrorMessageResult("Please run the start work process");
                //}
                businessDate = "30 Jun 2016";
                dayEndService.PreYearClosingProcess(SessionHelper.LoginUserOfficeID.Value, Convert.ToDateTime(businessDate));


                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        private void MapDropDownList(DayEndViewModel model)
        {

            var alloffice = officeService.GetAll().Where(d => d.OfficeID == LoginUserOfficeID && d.OrgID == LoggedInOrganizationID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;

        }
        // GET: PreClosing/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PreClosing/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PreClosing/Create
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

        // GET: PreClosing/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PreClosing/Edit/5
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

        // GET: PreClosing/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PreClosing/Delete/5
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
