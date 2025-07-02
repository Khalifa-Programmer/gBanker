using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class MraCdbController : BaseController
    {
        #region Variables
        private readonly IUltimateReportService ultimateReportService;
        public MraCdbController(IUltimateReportService ultimateReportService)
        {
            this.ultimateReportService = ultimateReportService;           
        }
        #endregion

        #region Methods
        public ActionResult GenerateMRACDB03Report(string from_date, string period)
        {
            try
            {

                var param = new { Org = LoggedInOrganizationID, OfficeFrom = LoginUserOfficeID, OfficeTo = LoginUserOfficeID, Date = from_date, MonthType = period };
                var mracdb = ultimateReportService.GetDataMRACDB03Report(param);

                var reportParam = new Dictionary<string, object>();
                //reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("from_date", from_date);
                
                ReportHelper.PrintReport("MRA-CDB-03B.rpt", mracdb.Tables[0], reportParam);                

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateMRACDBReport(string from_date, string to_date, string qType)
        {
            try
            {

                var param = new { Org = LoggedInOrganizationID, Office = LoginUserOfficeID, DateFrom = from_date, DateTo = to_date, Qtype = qType };
                var mracdb = ultimateReportService.GetDataMRACDBReport(param);

                var reportParam = new Dictionary<string, object>();
                //reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("from_date", from_date);

                if (qType == "1")
                {
                    ReportHelper.PrintReport("MRACDB_01.rpt", mracdb.Tables[0], reportParam);
                }
                else if (qType == "2")
                {
                    ReportHelper.PrintReport("MRACDB02_B.rpt", mracdb.Tables[0], reportParam);
                }

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion

        #region Events
        public ActionResult mracdb03()
        {
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            }
            //DateTime VDate;
            //VDate = System.DateTime.Now;
            //ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult mra()
        {
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            }
            //DateTime VDate;
            //VDate = System.DateTime.Now;
            //ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
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
        #endregion
    }
}
