using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class WeeklyMonitoringReportController : BaseController
    {
        #region Variables
        private readonly IMISReportService misReportService;

        public WeeklyMonitoringReportController(IMISReportService misReportService)
        {
           
            this.misReportService = misReportService;
 
        }
        #endregion

        #region Methods
        public ActionResult GenerateWeeklyMonitoringReport(string from_date, string to_date)
        {
            try
            {
                var ReportProcess = new { FromDate = from_date, ToDate = to_date, qType = 1 };
                misReportService.GetDataWeeklyMonitoringReport(ReportProcess);

                var ReportShow = new { FromDate = from_date, ToDate = to_date, qType = 2 };
                var mis = misReportService.GetDataWeeklyMonitoringReport(ReportShow);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("FromDate", from_date);
                reportParam.Add("ToDate", to_date);

                ReportHelper.PrintReport("rptWeeklyMonitoringReport.rpt", mis.Tables[0], reportParam);
                
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        #region Events
 
        // GET: WeeklyMonitoringReport
        public ActionResult Index()
        {
            return View();
        }

        // GET: WeeklyMonitoringReport/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WeeklyMonitoringReport/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WeeklyMonitoringReport/Create
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

        // GET: WeeklyMonitoringReport/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WeeklyMonitoringReport/Edit/5
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

        // GET: WeeklyMonitoringReport/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WeeklyMonitoringReport/Delete/5
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
        #endregion
    }
       
}
