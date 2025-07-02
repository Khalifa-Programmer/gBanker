using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
namespace gBanker.Web.Controllers
{
    public class WeeklySamityWiseReportController : BaseController
    {
        #region Variables
        private readonly IWeeklyReportService weeklyReportService;
        private readonly IDailyReportService dailyReportService;
        private readonly IGroupwiseReportService groupwiseReportService;
        public WeeklySamityWiseReportController(IWeeklyReportService weeklyReportService, IDailyReportService dailyReportService, IGroupwiseReportService groupwiseReportService)
        {
            this.weeklyReportService = weeklyReportService;
            this.dailyReportService = dailyReportService;
            this.groupwiseReportService = groupwiseReportService;
        }

        #endregion

        #region Methods
        public ActionResult GenerateSamityWiseWeeklyReport(string from_date, string to_date, string Qtype )
        {
            try
            {

                var param = new { BranchCode = SessionHelper.LoginUserOfficeID, DateFrom = from_date, DateTo = to_date, Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID };
                DataSet Weeklys;
                if (LoggedInOrganizationID==5)
                {
                    Weeklys = weeklyReportService.GetDataSamityWiseWeeklyReportJCF(param);
                }
                else
                 Weeklys = weeklyReportService.GetDataSamityWiseWeeklyReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("from_date", from_date);
                reportParam.Add("to_date", to_date);
                // ReportHelper.PrintReport("rptTodaysSummary.rpt", TodaysSummarys.Tables[0], reportParam);
                

                if (Qtype == "02")
                {

                    // ReportHelper.PrintReport("rptWeeklySamityWise_Part01.rpt", Weeklys.Tables[0], reportParam);
                    ReportHelper.PrintReport("rptWeeklySamityWisePart01.rpt", Weeklys.Tables[0], reportParam);
                }
                else if (Qtype == "04")
                {
                   
                    //ReportHelper.PrintReport("rptWeeklySamityWise_Part02.rpt", Weeklys.Tables[0], new Dictionary<string, object>());   
                    ReportHelper.PrintReport("rptWeeklySamityWise_Part02.rpt", Weeklys.Tables[0], reportParam);
                     
                }
                else if (Qtype == "06")
                {
                   
                    //ReportHelper.PrintReport("rptWeeklySamityWise_Part03.rpt", Weeklys.Tables[0], new Dictionary<string, object>());
                    ReportHelper.PrintReport("rptWeeklySamityWise_Part03.rpt", Weeklys.Tables[0], reportParam);
                }

                return Content(string.Empty);                
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

     




        public ActionResult GenerateStaffWiseStatement(string OfficeId, string Qtype, string DateTo)
        {
            try
            {
                var param = new { officeID = OfficeId, QType = Convert.ToInt32(Qtype), DateTo = DateTo };

                var alldata = groupwiseReportService.GetDataOrganizerWiseRecoveryStatement(param, "getStaffWiseStatement");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateTo);
                reportParam.Add("DateTo", DateTo);

                ReportHelper.PrintReport("rptStaffWiseStatement.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateSamityWiseWeeklyDSKReport(string from_date, string to_date, string Qtype)
        {
            try
            {

                var param = new { BranchCode = SessionHelper.LoginUserOfficeID, DateFrom = from_date, DateTo = to_date, Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID };

                DataSet Weeklys;
                

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("from_date", from_date);
                reportParam.Add("to_date", to_date);
                // ReportHelper.PrintReport("rptTodaysSummary.rpt", TodaysSummarys.Tables[0], reportParam);
                if (LoggedInOrganizationID==4)
                {
                    Weeklys = weeklyReportService.GetDataSamityWiseWeeklyDSKReport(param);
                    if (Qtype == "02")
                    {

                        // ReportHelper.PrintReport("rptWeeklySamityWise_Part01.rpt", Weeklys.Tables[0], reportParam);
                        ReportHelper.PrintReport("rptWeeklySamityWisePart01.rpt", Weeklys.Tables[0], reportParam);
                    }
                    else if (Qtype == "04")
                    {

                        //ReportHelper.PrintReport("rptWeeklySamityWise_Part02.rpt", Weeklys.Tables[0], new Dictionary<string, object>());   
                        ReportHelper.PrintReport("rptWeeklySamityWise_Part02.rpt", Weeklys.Tables[0], reportParam);

                    }
                    else if (Qtype == "06")
                    {

                        //ReportHelper.PrintReport("rptWeeklySamityWise_Part03.rpt", Weeklys.Tables[0], new Dictionary<string, object>());
                        ReportHelper.PrintReport("rptWeeklySamityWise_Part03.rpt", Weeklys.Tables[0], reportParam);
                    }
                }
                else
                {
                    Weeklys = weeklyReportService.GetDataSamityWiseWeeklyReport(param);
                    if (Qtype == "02")
                    {

                        // ReportHelper.PrintReport("rptWeeklySamityWise_Part01.rpt", Weeklys.Tables[0], reportParam);
                        ReportHelper.PrintReport("rptWeeklySamityWisePart01.rpt", Weeklys.Tables[0], reportParam);
                    }
                    else if (Qtype == "04")
                    {

                        //ReportHelper.PrintReport("rptWeeklySamityWise_Part02.rpt", Weeklys.Tables[0], new Dictionary<string, object>());   
                        ReportHelper.PrintReport("rptWeeklySamityWise_Part02.rpt", Weeklys.Tables[0], reportParam);

                    }
                    else if (Qtype == "06")
                    {

                        //ReportHelper.PrintReport("rptWeeklySamityWise_Part03.rpt", Weeklys.Tables[0], new Dictionary<string, object>());
                        ReportHelper.PrintReport("rptWeeklySamityWise_Part03.rpt", Weeklys.Tables[0], reportParam);
                    }
                }

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult KhatWaryReport(string from_date, string to_date)
        {
            try
            {

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, Date_From = from_date, Date_To = to_date };
                var Weeklys = weeklyReportService.GetKhatWaryReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", from_date);
                reportParam.Add("DateTo", to_date);
                // ReportHelper.PrintReport("rptTodaysSummary.rpt", TodaysSummarys.Tables[0], reportParam);


                    //ReportHelper.PrintReport("rptWeeklySamityWise_Part01.rpt", Weeklys.Tables[0], new Dictionary<string, object>());                    
                ReportHelper.PrintReport("KhatWaryReport.rpt", Weeklys.Tables[0], reportParam);
               
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateStaffwiseWeeklyReport(string from_date, string to_date, string Qtype)
        {
            try
            {

                var param = new { BranchCode = SessionHelper.LoginUserOfficeID, DateFrom = from_date, DateTo = to_date, Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID };

                DataSet StaffWiseStats;
                if (LoggedInOrganizationID==5)
                {
                    StaffWiseStats = weeklyReportService.GetDataSamityWiseWeeklyReportJCF(param);
                }
                else
                    StaffWiseStats = weeklyReportService.GetDataSamityWiseWeeklyReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("from_date", from_date);
                reportParam.Add("to_date", to_date);

                if (Qtype == "01")
                {
                    ReportHelper.PrintReport("rptWeeklyStaffWisePart01.rpt", StaffWiseStats.Tables[0], reportParam);
                    //ReportHelper.PrintReport("rptWeeklyStaffWise_Part01.rpt", StaffWiseStats.Tables[0], reportParam);
                }
                else if (Qtype == "03")
                {

                    //ReportHelper.PrintReport("rptWeekStaffWise_Part02.rpt", StaffWiseStats.Tables[0], new Dictionary<string, object>());
                    ReportHelper.PrintReport("rptWeekStaffWise_Part02.rpt", StaffWiseStats.Tables[0], reportParam);
                }
                else if (Qtype == "05")
                {

                    //ReportHelper.PrintReport("rptWeeklyStaffWise_Part03.rpt", StaffWiseStats.Tables[0], new Dictionary<string, object>());
                    ReportHelper.PrintReport("rptWeeklyStaffWise_Part03.rpt", StaffWiseStats.Tables[0], reportParam);
                }

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateStaffwiseWeeklyDSKReport(string from_date, string to_date, string Qtype)
        {
            try
            {
                //var param = new { BranchCode = SessionHelper.LoginUserOfficeID, DateFrom = from_date, DateTo = to_date, Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID };


                //var StaffWiseStats = weeklyReportService.GetDataSamityWiseWeeklyReport(param);

                //var reportParam = new Dictionary<string, object>();

                DataSet StaffWiseStats;
                var param = new { BranchCode = SessionHelper.LoginUserOfficeID, DateFrom = from_date, DateTo = to_date, Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID };

                if (LoggedInOrganizationID==4)
                {
                    StaffWiseStats = weeklyReportService.GetDataSamityWiseWeeklyDSKReport(param);
                }
                
                else
                {
                    StaffWiseStats = weeklyReportService.GetDataSamityWiseWeeklyReport(param);
                }
                var reportParam = new Dictionary<string, object>();

                if (LoggedInOrganizationID == 4)
                {
                   
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("from_date", from_date);
                    reportParam.Add("to_date", to_date);

                    if (Qtype == "01")
                    {
                        ReportHelper.PrintReport("rptWeeklyStaffWisePart01.rpt", StaffWiseStats.Tables[0], reportParam);
                        //ReportHelper.PrintReport("rptWeeklyStaffWise_Part01.rpt", StaffWiseStats.Tables[0], reportParam);
                    }
                    else if (Qtype == "03")
                    {

                        //ReportHelper.PrintReport("rptWeekStaffWise_Part02.rpt", StaffWiseStats.Tables[0], new Dictionary<string, object>());
                        ReportHelper.PrintReport("rptWeekStaffWise_Part02.rpt", StaffWiseStats.Tables[0], reportParam);
                    }
                    else if (Qtype == "05")
                    {

                        //ReportHelper.PrintReport("rptWeeklyStaffWise_Part03.rpt", StaffWiseStats.Tables[0], new Dictionary<string, object>());
                        ReportHelper.PrintReport("rptWeeklyStaffWise_Part03.rpt", StaffWiseStats.Tables[0], reportParam);
                    }
                }
                else
                {

                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("from_date", from_date);
                    reportParam.Add("to_date", to_date);

                    if (Qtype == "01")
                    {
                        ReportHelper.PrintReport("rptWeeklyStaffWisePart01.rpt", StaffWiseStats.Tables[0], reportParam);
                        //ReportHelper.PrintReport("rptWeeklyStaffWise_Part01.rpt", StaffWiseStats.Tables[0], reportParam);
                    }
                    else if (Qtype == "03")
                    {

                        //ReportHelper.PrintReport("rptWeekStaffWise_Part02.rpt", StaffWiseStats.Tables[0], new Dictionary<string, object>());
                        ReportHelper.PrintReport("rptWeekStaffWise_Part02.rpt", StaffWiseStats.Tables[0], reportParam);
                    }
                    else if (Qtype == "05")
                    {

                        //ReportHelper.PrintReport("rptWeeklyStaffWise_Part03.rpt", StaffWiseStats.Tables[0], new Dictionary<string, object>());
                        ReportHelper.PrintReport("rptWeeklyStaffWise_Part03.rpt", StaffWiseStats.Tables[0], reportParam);
                    }

                }
              

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult GenerateStaffwiseWeeklyDSK_WithoutCategoryGroupReport(string from_date, string to_date, string Qtype)
        {
            try
            {

                var param = new { BranchCode = SessionHelper.LoginUserOfficeID, DateFrom = from_date, DateTo = to_date, Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID };


                var StaffWiseStats = weeklyReportService.GetDataSamityWiseWeeklyDSK_WithoutCategoryGroupReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("from_date", from_date);
                reportParam.Add("to_date", to_date);

                if (Qtype == "01")
                {
                    ReportHelper.PrintReport("rptWeeklyStaffWisePart01WithoutCategory.rpt", StaffWiseStats.Tables[0], reportParam);
                    //ReportHelper.PrintReport("rptWeeklyStaffWise_Part01.rpt", StaffWiseStats.Tables[0], reportParam);
                }
                else if (Qtype == "03")
                {
                if (LoggedInOrganizationID==4)
                    {
                        //ReportHelper.PrintReport("rptWeekStaffWise_Part02.rpt", StaffWiseStats.Tables[0], new Dictionary<string, object>());
                        ReportHelper.PrintReport("rptWeekStaffWise_Part02WithoutCategory_DSK.rpt", StaffWiseStats.Tables[0], reportParam);
                    }
                else
                    //ReportHelper.PrintReport("rptWeekStaffWise_Part02.rpt", StaffWiseStats.Tables[0], new Dictionary<string, object>());
                    ReportHelper.PrintReport("rptWeekStaffWise_Part02WithoutCategory.rpt", StaffWiseStats.Tables[0], reportParam);
                }
                else if (Qtype == "05")
                {
                    if (LoggedInOrganizationID == 4)
                    {
                        ReportHelper.PrintReport("rptWeeklyStaffwise_Part03WithoutCategory_DSK.rpt", StaffWiseStats.Tables[0], reportParam);
                    }
                    else

                        //ReportHelper.PrintReport("rptWeeklyStaffWise_Part03.rpt", StaffWiseStats.Tables[0], new Dictionary<string, object>());
                        ReportHelper.PrintReport("rptWeeklyStaffWise_Part03WithoutCategory.rpt", StaffWiseStats.Tables[0], reportParam);
                }

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult GenerateCenterwise_loan_SCReport(string DateFrom, string DateTo)
        {
            try
            {

                var param = new { Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Org = SessionHelper.LoginUserOrganizationID };
                var CenterwiseLSC = dailyReportService.GetDataCenterwise_loan_SC(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);

                ReportHelper.PrintReport("rptCenterwiseLoanSC.rpt", CenterwiseLSC.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GeneratePomisTargetAchivement(string DateFrom, string DateTo)
        {
            try
            {
                var param = new
                {
                    org = LoggedInOrganizationID,
                    OfficeFrom = LoginUserOfficeID,
                    OfficeTo = LoginUserOfficeID,
                    StartDate = DateFrom,
                    EndDate = DateTo
                };
                //var param = new { Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Org = SessionHelper.LoginUserOrganizationID };
                var CenterwiseLSC = dailyReportService.GetPomisTargetAchivement(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("DateFrom", DateFrom);
                //reportParam.Add("DateTo", DateTo);

                ReportHelper.PrintReport("rptPOMISTargetAndAchievement.rpt", CenterwiseLSC.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //public ActionResult GenerateCenterwise_loan_SCReport(string DateFrom, string DateTo)
        //{
        //    try
        //    {
        //        var param = new { Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo};
        //        var CenterwiseLSC = dailyReportService.GetDataCenterwise_loan_SC(param);

        //        var reportParam = new Dictionary<string, object>();
        //        reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //        reportParam.Add("DateFrom", DateFrom);
        //        reportParam.Add("DateTo", DateTo);


        //        //ReportHelper.PrintReport("rptNewOverDueMemberList.rpt", OverdueMls.Tables[0], new Dictionary<string, object>());                    
        //        ReportHelper.PrintReport("rptCenterwiseLoanSC.rpt", CenterwiseLSC.Tables[0], reportParam);

        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}

        #endregion

        #region Events
        //
        // GET: /WeeklySamityWiseReport_Part01/
        public ActionResult Index()
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
        public ActionResult StaffwiseStatement()
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
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }

        public ActionResult StaffwiseStatementWithoutCategory()
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

        public ActionResult POMISTargetAcheivement()
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
        public ActionResult Centerwise_loan_SC()
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
        public ActionResult KhatWaryReport()
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
        // GET: /WeeklySamityWiseReport_Part01/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /WeeklySamityWiseReport_Part01/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /WeeklySamityWiseReport_Part01/Create
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
        // GET: /WeeklySamityWiseReport_Part01/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /WeeklySamityWiseReport_Part01/Edit/5
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
        // GET: /WeeklySamityWiseReport_Part01/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /WeeklySamityWiseReport_Part01/Delete/5
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
        #endregion