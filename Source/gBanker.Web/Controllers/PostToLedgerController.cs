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
    public class PostToLedgerController : BaseController
    {
        private readonly IDayEndService dayEndService;
        private readonly IOfficeService officeService;
        private readonly IDayInitialService dayInitialService;

        public PostToLedgerController(IDayEndService dayEndService, IOfficeService officeService, IDayInitialService dayInitialService)
          {
              this.dayEndService = dayEndService;
              this.officeService = officeService;
              this.dayInitialService = dayInitialService;
          }

        // GET: PostToLedger
        public ActionResult Index()
        {
            var model = new DayEndViewModel();
            if (IsDayInitiated)
                model.BusinessDate = TransactionDate;
            else
                model.BusinessDate = System.DateTime.Now;
            MapDropDownList(model);

            return View(model);
        }
        private void MapDropDownList(DayEndViewModel model)
        {

            var alloffice = officeService.GetAll().Where(d => d.OfficeID == LoginUserOfficeID && d.OrgID == LoggedInOrganizationID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;

        }

        public JsonResult PostToLedgerProcess(string officeId, string businessDate)
        {
            try
            {
                try
                {
                    dayEndService.PostToLedgerProcess(SessionHelper.LoginUserOfficeID.Value, Convert.ToDateTime(businessDate), LoggedInOrganizationID);
                }
                catch (Exception ex)
                {
                   
                    return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        // GET: PostToLedger/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PostToLedger/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PostToLedger/Create
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

        // GET: PostToLedger/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PostToLedger/Edit/5
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

        // GET: PostToLedger/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PostToLedger/Delete/5
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
