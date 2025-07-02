using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class MonthlyProjectStatementReportController : Controller
    {
        #region Variables
        private readonly IMISReportService misReportService;

        public MonthlyProjectStatementReportController(IMISReportService misReportService)
        {
           
            this.misReportService = misReportService;
 
        }
        public class YearList
        {
            public int YearCode { get; set; }
            public string YearName { get; set; }
        }
        #endregion

         #region Methods
        public JsonResult GenerateMonthlyProjectStatement(string pYear, string pMonth, string qType)
        {
            var ReportProcess1 = new { ProcessYear = pYear, ProcessMonth = pMonth, OfficeID = SessionHelper.LoginUserOfficeID, qType = qType, CurrencyRate = 80 };
            misReportService.GetDataMonthlyProjectStatementReportProcess(ReportProcess1);

            return Json(new { data = qType }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerateMonthlyProjectStatementReport(string pYear, string pMonth, string OfficeID)
        {
            try
            {
                //Report Call
                var ReportShow = new { ProcessYear = pYear, ProcessMonth = pMonth, OfficeID = SessionHelper.LoginUserOfficeID, qType = 21, CurrencyRate = 80 };
                var mis = misReportService.GetDataMonthlyProjectStatementReportProcess(ReportShow);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("Cunnency", "USD");


                ReportHelper.PrintReport("rptMonthlyProjectStatementReportBW.rpt", mis.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult GetYearList()
        {

            var dt = new DataTable();

            dt.Columns.Add("text", typeof(string));
            dt.Columns.Add("value", typeof(string));

            int cur_yr = DateTime.Now.Year;

            for (int i = 0; i <= 10; i++)
            {
                dt.Rows.Add((cur_yr - i).ToString(), (cur_yr - i).ToString());
            }

            List<YearList> YearList = new List<YearList>();

            //DataTable dt = dc.GetYearList();

            YearList blnkYearList = new YearList()
            {
                YearCode = 0,
                YearName = "Please Select"
            };
            YearList.Add(blnkYearList);

            foreach (DataRow row in dt.Rows)
            {

                YearList oYearList = new YearList()
                {
                    YearCode = Convert.ToInt32(row["value"]),
                    YearName = row["text"].ToString()
                };

                YearList.Add(oYearList);
            }

            IEnumerable<SelectListItem> items = new SelectList(YearList, "YearCode", "YearName");
            return Json(items.ToList(), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Events
        // GET: MonthlyProjectStatementReport
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["YearList"] = items;
            return View();
        }

        // GET: MonthlyProjectStatementReport/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MonthlyProjectStatementReport/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MonthlyProjectStatementReport/Create
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

        // GET: MonthlyProjectStatementReport/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MonthlyProjectStatementReport/Edit/5
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

        // GET: MonthlyProjectStatementReport/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MonthlyProjectStatementReport/Delete/5
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
