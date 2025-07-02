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
    public class AccAutoVoucherCollectionController :  BaseController
    {
        private readonly IWeeklyReportService weeklyReportService;
        private readonly IAccAutoVoucherCollectionService accAutoVoucherCollectionService;
        private readonly IOfficeService officeService;
        private readonly IUltimateReportService ultimateReportService;
        public AccAutoVoucherCollectionController(IAccAutoVoucherCollectionService accAutoVoucherCollectionService, IOfficeService officeService, IWeeklyReportService weeklyReportService, IUltimateReportService ultimateReportService)
          {
              this.accAutoVoucherCollectionService = accAutoVoucherCollectionService;
              this.officeService = officeService;
              this.weeklyReportService = weeklyReportService;
              this.ultimateReportService = ultimateReportService;
          }
        // GET: AccAutoVoucherCollection
        public ActionResult Index()
        {
            var model = new AccAutoVoucherCollectionViewModel() ;

            if (IsDayInitiated)
                model.BusinessDate = TransactionDate;
            MapDropDownList(model);

            return View(model);
        }
        public JsonResult AutoVoucherCollectionProcess(string officeId, string businessDate)
        {
            try
            {
                if (!IsDayInitiated)
                {
                    return GetErrorMessageResult("Please run the start work process");
                }

                    var param = new { OfficeID = LoginUserOfficeID, BusinessDate = businessDate, OrgID = SessionHelper.LoginUserOrganizationID };
                    weeklyReportService.AutoVoucherCollectionProcess(param);
                    return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }


        }
        private void MapDropDownList(AccAutoVoucherCollectionViewModel model)
        {

            var alloffice = officeService.GetAll().Where(a => a.OfficeID == LoginUserOfficeID && a.OrgID==LoggedInOrganizationID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;

        }
        // GET: AccAutoVoucherCollection/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccAutoVoucherCollection/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccAutoVoucherCollection/Create
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

        // GET: AccAutoVoucherCollection/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccAutoVoucherCollection/Edit/5
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

        // GET: AccAutoVoucherCollection/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccAutoVoucherCollection/Delete/5
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

        public JsonResult GetMessage(int OfficeID)
        {
            string result = string.Empty;
            try
            {
                var filter = new { OfficeId = SessionHelper.LoginUserOfficeID };
                var filteredList = ultimateReportService.GetDataWithParameter(filter, "getAutoVoucherProcessCheck");

                var message = filteredList.Tables[0].Rows[0]["status"].ToString();

                result = message;
            }
            catch (Exception ex)
            {
                return Json(ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }// End of Message


    }
}
