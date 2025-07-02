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
    public class TodaysSummaryController : BaseController
    {
         private readonly ITodaysSummaryService todaysSummaryService;
         private readonly IOfficeService officeService;
         private readonly ITodaysSummaryReportService todaysSummaryReportService;
         public TodaysSummaryController(ITodaysSummaryService todaysSummaryService, IOfficeService officeService, ITodaysSummaryReportService todaysSummaryReportService)
        {
            this.todaysSummaryService = todaysSummaryService;
            this.officeService = officeService;
            this.todaysSummaryReportService = todaysSummaryReportService;
        }
        //
        // GET: /TodaysSummary/
         private void MapDropDownList(TodaysSummaryViewModel model)
         {

             var alloffice = officeService.GetAll().Where(t => t.OfficeID == LoginUserOfficeID.Value);

             var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

             model.officeListItems = viewOffice;

         }
         public ActionResult GenerateTodaysSummaryReport(string officeId, string businessDate)
         {
             try
             {
                 DateTime vdate=System.DateTime.Now;
                 if (IsDayInitiated)
                      vdate=TransactionDate;

                 todaysSummaryService.GetTodaysSummary(1, LoginUserOfficeID, vdate);
                 var param = new { QType = 2, OfficeId = LoginUserOfficeID, TransactionDate = Convert.ToDateTime(businessDate) };
                 var allproducts = todaysSummaryReportService.GetDataProductInfo(param);
                
                 ReportHelper.PrintReport("rptTodaysSummary.rpt", allproducts.Tables[0], new Dictionary<string, object>());
                 return Content(string.Empty); 

             }
             catch (Exception ex)
             {
                 return Json(new { Result = "ERROR", Message = ex.Message });
             }
         }
       
        public ActionResult Index()
        {
            var model = new TodaysSummaryViewModel();


            MapDropDownList(model);

            return View(model);
        }

        //
        // GET: /TodaysSummary/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /TodaysSummary/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /TodaysSummary/Create
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
        // GET: /TodaysSummary/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /TodaysSummary/Edit/5
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
        // GET: /TodaysSummary/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /TodaysSummary/Delete/5
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
