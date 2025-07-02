using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace gBanker.Web.Controllers
{
    public class MRAMISController : BaseController
    {
        #region Variables
        private readonly IMRAReportService mraReportService;

        public MRAMISController(IMRAReportService mraReportService)
        {
            this.mraReportService = mraReportService;

        }
        #endregion
        public ActionResult GenerateMraMIS3AReport(string Date)
        {
            try
            {

                var param = new { Office = SessionHelper.LoginUserOfficeID, Date = Date, Org = SessionHelper.LoginUserOrganizationID };
                var MraMIS3As = mraReportService.GetDataMraMISReport3A(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("UptoDate", Date);
                // ReportHelper.PrintReport("rptTodaysSummary.rpt", TodaysSummarys.Tables[0], reportParam);

                //ReportHelper.PrintReport("MRA-MIS-03A.rpt", MraMIS3As.Tables[0], new Dictionary<string, object>());
                ReportHelper.PrintReport("MRA-MIS-03A.rpt", MraMIS3As.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateMraMISReport(string Date)
        {
            try
            {

                var param = new { Office = SessionHelper.LoginUserOfficeID, Date = Date, Org = SessionHelper.LoginUserOrganizationID };

                var MraMIS3B1s = mraReportService.GetDataMraMISReport3B1(param);
                var MraMIS3B2s = mraReportService.GetDataMraMISReport3B2(param);
                var MraMIS3B3s = mraReportService.GetDataMraMISReport3B3(param);
                var MraMIS3B4s = mraReportService.GetDataMraMISReport3B4(param);

                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("MRA-MIS-03B_Part02.rpt", MraMIS3B2s.Tables[0]);
                subReportDB.Add("MRA-MIS-03B_Part03.rpt", MraMIS3B3s.Tables[0]);
                subReportDB.Add("MRA-MIS-03B_Part04.rpt", MraMIS3B4s.Tables[0]);


                //ReportHelper.PrintWithSubReport("MRA-MIS-03B.rpt", MraMIS3B1s.Tables[0], new Dictionary<string, object>(), subReportDB, new MRA_MIS_03B());
                ReportHelper.PrintWithSubReport("MRAMIS3B_new.rpt", MraMIS3B1s.Tables[0], new Dictionary<string, object>(), subReportDB, new MRAMIS3B_new());



                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateMraMIS4AReport(string Date)
        {
            try
            {

                var param = new { Office = SessionHelper.LoginUserOfficeID, Date = Date, Org = SessionHelper.LoginUserOrganizationID };
                var MraMIS4As = mraReportService.GetDataMraMISReport4A(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("UptoDate", Date);
                // ReportHelper.PrintReport("rptTodaysSummary.rpt", TodaysSummarys.Tables[0], reportParam);

                //ReportHelper.PrintReport("MRA-MIS-04A.rpt", MraMIS4As.Tables[0], new Dictionary<string, object>());
                ReportHelper.PrintReport("MRA-MIS-04A.rpt", MraMIS4As.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateMraMIS4BReport(string Date)
        {
            try
            {
                var param = new { Office = SessionHelper.LoginUserOfficeID, Date = Date, Org = SessionHelper.LoginUserOrganizationID };

                var MraMIS4B1s = mraReportService.GetDataMraMISReport4B1(param);
                var MraMIS4B2s = mraReportService.GetDataMraMISReport4B2(param);

                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("MRA-MIS-04B_Part02.rpt", MraMIS4B2s.Tables[0]);

                ReportHelper.PrintWithSubReport("MRA-MIS-04B.rpt", MraMIS4B1s.Tables[0], new Dictionary<string, object>(), subReportDB, new MRA_MIS_04B());


                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //
        // GET: /MRAMIS/
        public ActionResult MRAMIS3A()
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
            //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult MRAMIS3B()
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
            //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult MRAMIS4A()
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
            //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult MRAMIS4B()
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
            //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }
        //
        // GET: /MRAMIS/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        //
        // GET: /MRAMIS/Create
        public ActionResult Create()
        {
            return View();
        }
        //
        // POST: /MRAMIS/Create
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
        // GET: /MRAMIS/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        //
        // POST: /MRAMIS/Edit/5
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
        // GET: /MRAMIS/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        //
        // POST: /MRAMIS/Delete/5
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
