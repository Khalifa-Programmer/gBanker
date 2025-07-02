using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gBanker.Web.Controllers
{
    public class WeeklyCollectionSheetController : BaseController
     {
           #region Variables
        private readonly IWeeklyReportService weeklyReportService;
        private readonly IDailyReportService dailyReportService;
        private readonly IGroupwiseReportService groupwiseReportService;
        private readonly IUltimateReportService unlimitedReportService;



        public WeeklyCollectionSheetController(IWeeklyReportService weeklyReportService, IDailyReportService dailyReportService, IGroupwiseReportService groupwiseReportService, IUltimateReportService unlimitedReportService)
        {
            this.weeklyReportService = weeklyReportService;
            this.dailyReportService = dailyReportService;
            this.groupwiseReportService = groupwiseReportService;
            this.unlimitedReportService = unlimitedReportService;
        }
        #endregion

        #region Methods
        //public ActionResult GenerateWeeklyCollectionSheetReport(string Date, string DateTo)
        public ActionResult GenerateWeeklyCollectionSheetReport(string Date)
        {
            try
            {

                var param = new { Office = SessionHelper.LoginUserOfficeID, Date = Date, Org = SessionHelper.LoginUserOrganizationID };
                var CollectionSheets = weeklyReportService.GetDataWeeklyCollectionSheetReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);

               // reportParam.Add("UptoDate", Date);

               // ReportHelper.PrintReport("rptWeeklyCollectionSheetDeclineMethod.rpt", CollectionSheets.Tables[0], new Dictionary<string, object>());
                ReportHelper.PrintReport("rptWeeklyCollectionSheetDeclineMethod.rpt", CollectionSheets.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateCenterwiseTransactionReport(string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Proc_GetTodaySummary");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo); 
                ReportHelper.PrintReport("rptTodaySummaryCenterWise.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateCenterwiseTransactionReportExport(string DateFrom, string DateTo)
        {
            var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo };
            groupwiseReportService.GetDataUltimateReleaseReport(param, "Proc_GetTodaySummary");

            GridView gv = new GridView();
            //var allRepaymentSchedule = creditscoreService.GetAll();
            var allRepaymentSchedule = groupwiseReportService.GetDataUltimateReleaseReport(param, "Proc_GetTodaySummary");
            var detail = allRepaymentSchedule.Tables[0];
            gv.DataSource = detail;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=rptTodaySummaryCenterWise.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("CenterwiseTodaySummary");
        }
        public ActionResult GenerateCenterwiseTransactionReport_WithoutDaterange(string DateTo)
        {
            try
            {

                //DateTo = TransactionDate;
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = TransactionDate };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Proc_GetTodaySummary_WithoutDaterange");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptTodaySummaryCenterWise_WithoutDaterange.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateCenterwiseTransactionReport_WithoutDaterange_Aday(string DateTo)
        {
            try
            {

                //DateTo = TransactionDate;
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = TransactionDate, EmpID=LoggedInEmployeeID };
                var alldata = groupwiseReportService.GetDataWithoutDaterange_AdayEmpWise(param, "Proc_GetTodaySummary_WithoutDaterange_AdayEmpWise");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptTodaySummaryCenterWise_WithoutDaterange_Aday.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateCenterwiseTransactionReportExport_WithoutDaterange(string DateTo)
        {
            var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = TransactionDate };
            groupwiseReportService.GetDataUltimateReleaseReport(param, "Proc_GetTodaySummary_WithoutDaterange");

            GridView gv = new GridView();
            //var allRepaymentSchedule = creditscoreService.GetAll();
            var allRepaymentSchedule = groupwiseReportService.GetDataUltimateReleaseReport(param, "Proc_GetTodaySummary_WithoutDaterange");
            var detail = allRepaymentSchedule.Tables[0];
            gv.DataSource = detail;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=rptTodaySummaryCenterWise_WithoutDaterange.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("CenterwiseTodaySummary_WithoutDaterange");
        }
        public ActionResult GenerateCenterwiseTransactionReportExport_WithoutDaterange_Aday(string DateTo)
        {
            var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = TransactionDate };
            groupwiseReportService.GetDataUltimateReleaseReport(param, "Proc_GetTodaySummary_WithoutDaterange_Aday");

            GridView gv = new GridView();
            //var allRepaymentSchedule = creditscoreService.GetAll();
            var allRepaymentSchedule = groupwiseReportService.GetDataUltimateReleaseReport(param, "Proc_GetTodaySummary_WithoutDaterange_Aday");
            var detail = allRepaymentSchedule.Tables[0];
            gv.DataSource = detail;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=rptTodaySummaryCenterWise_WithoutDaterange_Aday.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("CenterwiseTodaySummary_WithoutDaterange_Aday");
        }
        public ActionResult GenerateRecoveryRegisterReport(string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_RecoveryRegister_Report");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptRecoveryRegister.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateRecoveryRegisterReport_Aday(string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataWithoutDaterange_AdayEmpWise(param, "Rpt_RecoveryRegister_Report_AdaySheet");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptRecoveryRegisterLoanFinal.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
                //return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        
        

        #endregion

        //
        // GET: /WeeklyCollectionSheet/
        public ActionResult WeeklyCollectionSheet()
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
        public ActionResult CenterwiseTodaySummary()
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
            ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult CenterwiseTodaySummary_WithoutDaterange()
        {
            return View();
        }
        public ActionResult CenterwiseTodaySummary_WithoutDaterange_Aday()
        {
            return View();
        }
        public ActionResult RecoveryRegisterReport()
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
        public ActionResult RecoveryRegisterReport_Aday()
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
        // GET: /WeeklyCollectionSheet/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /WeeklyCollectionSheet/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /WeeklyCollectionSheet/Create
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
        // GET: /WeeklyCollectionSheet/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /WeeklyCollectionSheet/Edit/5
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
        // GET: /WeeklyCollectionSheet/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /WeeklyCollectionSheet/Delete/5
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
