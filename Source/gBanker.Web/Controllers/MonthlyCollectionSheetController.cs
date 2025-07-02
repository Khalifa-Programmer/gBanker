using gBanker.Service.ReportExecutionService;
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
    public class MonthlyCollectionSheetController : BaseController
    {
        #region Variables
        private readonly IWeeklyReportService weeklyReportService;
        private readonly IGroupwiseReportService groupwiseReportService;

        public MonthlyCollectionSheetController(IWeeklyReportService weeklyReportService, IGroupwiseReportService groupwiseReportService)
        {
            this.weeklyReportService = weeklyReportService;
        }
        #endregion
        
        #region Methods
        public ActionResult GenerateMonthlyCollectionSheetReport(string CollectionDate)
        {
            try
            {
              

                if (LoggedInOrganizationID == 49)
                {
                    var paramChina = new { Office = SessionHelper.LoginUserOfficeID, CollectionDate = CollectionDate, Org = SessionHelper.LoginUserOrganizationID };
                    var McsChina = weeklyReportService.GetDataMonthlyCollectionSheet_NewReportChina(paramChina);
                    var reportParamChina = new Dictionary<string, object>();
                    reportParamChina.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParamChina.Add("UptoDate", CollectionDate);
                    ReportHelper.PrintReport("rptMonthWiseCollectionSheetWeekLyMonthly_WithLoan_InterestInstallment.rpt", McsChina.Tables[0], reportParamChina);

                }
                else if (LoggedInOrganizationID == 7 || LoggedInOrganizationID == 4)
                {
                     
                    var param = new { Office = SessionHelper.LoginUserOfficeID, CollectionDate = CollectionDate, Org = SessionHelper.LoginUserOrganizationID };
                    var Mcs = weeklyReportService.GetDataMonthlyCollectionSheet_NewReportCCDB(param);
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("UptoDate", CollectionDate);
                    ReportHelper.PrintReport("rptMonthWiseCollectionSheetWeekLyMonthly_WithLoan_InterestInstallmentCCDB.rpt", Mcs.Tables[0], reportParam);

                }
                else if (LoggedInOrganizationID == 153)
                {
                    DataSet McsAll;
                    var param = new { Office = SessionHelper.LoginUserOfficeID, CollectionDate = CollectionDate, Org = SessionHelper.LoginUserOrganizationID };
                    McsAll = weeklyReportService.GetDataCollectionSheetWeeklyMonthlyReportMemberwise(param);
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("UptoDate", CollectionDate);
                    //reportParam.Add("CDay", ColDay);
                    ReportHelper.PrintReport("rptMonthWiseCollectionSheetWeekLyMonthlyMemberwise_Shanti.rpt", McsAll.Tables[0], reportParam);

                }
                else if (LoggedInOrganizationID == 218)
                {
                    DataSet McsAll;
                    var param = new { Office = SessionHelper.LoginUserOfficeID, CollectionDate = CollectionDate, Org = SessionHelper.LoginUserOrganizationID };
                    McsAll = weeklyReportService.GetDataCollectionSheetWeeklyMonthlyReportMemberwiseSouthAfrica(param);
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("UptoDate", CollectionDate);
                    //reportParam.Add("CDay", ColDay);
                    ReportHelper.PrintReport("rptMonthWiseCollectionSheetWeekLyMonthly_SouthAfrica.rpt", McsAll.Tables[0], reportParam);

                }
                else
                {
                    var param = new { Office = SessionHelper.LoginUserOfficeID, CollectionDate = CollectionDate, Org = SessionHelper.LoginUserOrganizationID };
                    var Mcs = weeklyReportService.GetDataMonthlyCollectionSheet_NewReport(param);
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("UptoDate", CollectionDate);
                    ReportHelper.PrintReport("rptMonthWiseCollectionSheetWeekLyMonthly_WithLoan_InterestInstallment.rpt", Mcs.Tables[0], reportParam);

                }


                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateMonthlyCollectionSheetReportSSRS(string CollectionDate)
        {
            try
            {

                //var param = new { Office = SessionHelper.LoginUserOfficeID, CollectionDate = CollectionDate, Org = SessionHelper.LoginUserOrganizationID };
                //var Mcs = weeklyReportService.GetDataMonthlyCollectionSheet_NewReport(param);
                var paramValues = new List<ParameterValue>();

                paramValues.Add(new ParameterValue() {Name="Org",Value= SessionHelper.LoginUserOrganizationID.Value.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Office", Value = SessionHelper.LoginUserOfficeID.Value.ToString() });
                paramValues.Add(new ParameterValue() { Name = "CollectionDate", Value = CollectionDate });
                paramValues.Add(new ParameterValue() { Name = "OrganizationName", Value = ApplicationSettings.OrganiztionName });
                //OrganizationName
                //PrintSSRSReport("/gBanker_Reports/MC", paramValues.ToArray(), "gBankerDbContext");
                PrintSSRSReport("/gBanker_Reports/mCollection", paramValues.ToArray(), "gBankerDbContext");
                
                //var reportParam = new Dictionary<string, object>();
                //reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("UptoDate", CollectionDate);

                ////ReportHelper.PrintReport("rptMonthWiseCollectionSheet.rpt", Mcs.Tables[0], new Dictionary<string, object>());
                //ReportHelper.PrintReport("rptMonthWiseCollectionSheet.rpt", Mcs.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult GenerateMonthlyCollectionSheetForAllReport(string CollectionDate)
        {
            try
            {

                var param = new {Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, CollectionDate = CollectionDate };
                var McsAll = weeklyReportService.GetDataMonthlyCollectionSheet_ForAllReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("UptoDate", CollectionDate);
                //reportParam.Add("CDay", ColDay);
                
                //ReportHelper.PrintReport("rptMonthWiseCollectionSheetForAll.rpt", McsAll.Tables[0], new Dictionary<string, object>());

                ReportHelper.PrintReport("rptMonthWiseCollectionSheetForAll.rpt", McsAll.Tables[0], reportParam); 

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateCollectionSheetWeeklyMonthlyReport(string CollectionDate)
        {
            try
            {

                var param = new { Office = SessionHelper.LoginUserOfficeID, CollectionDate = CollectionDate, Org = SessionHelper.LoginUserOrganizationID };
                var McsAll = weeklyReportService.GetDataCollectionSheetWeeklyMonthlyReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("UptoDate", CollectionDate);
                //reportParam.Add("CDay", ColDay);

                //ReportHelper.PrintReport("rptMonthWiseCollectionSheetForAll.rpt", McsAll.Tables[0], new Dictionary<string, object>());

                ReportHelper.PrintReport("rptMonthWiseCollectionSheetWeekLyMonthly.rpt", McsAll.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateCollectionSheetWeeklyMonthlyReportMemberwiseSeperate(string CollectionDate)
        {
            try
            {
                var param = new { Office = SessionHelper.LoginUserOfficeID, CollectionDate = CollectionDate, Org = SessionHelper.LoginUserOrganizationID };
                var McsAll = weeklyReportService.GetDataCollectionSheetWeeklyMonthlyReportMemberwiseSeperateSavings(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("UptoDate", CollectionDate);
                //reportParam.Add("CDay", ColDay);
                ReportHelper.PrintReport("rptMonthWiseCollectionSheetWeekLyMonthlyMemberwise.rpt", McsAll.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateCollectionSheetWeeklyMonthlyReportMemberwise(string CollectionDate, string ExportType)
        {
            try
            {
                DataSet McsAll;
                var param = new { Office = SessionHelper.LoginUserOfficeID, CollectionDate = CollectionDate, Org = SessionHelper.LoginUserOrganizationID };

                if (LoggedInOrganizationID == 99)
                {

                    //var orgId = SessionHelper.LoginUserOrganizationID;

                    var paramValues = new List<ParameterValue>();
                    paramValues.Add(new ParameterValue() { Name = "Org", Value = LoggedInOrganizationID.ToString() });
                    paramValues.Add(new ParameterValue() { Name = "Office", Value = SessionHelper.LoginUserOfficeID.ToString() });
                    paramValues.Add(new ParameterValue() { Name = "CollectionDate", Value = CollectionDate });

                    PrintSSRSReport("/gBanker_Reports/MonthlyCollectionSheetForProsika", paramValues.ToArray(), "gBankerDbContext");
                    return Content(string.Empty);

                }
                else if (LoggedInOrganizationID == 6)
                {
                    McsAll = weeklyReportService.GetDataCollectionSheetWeeklyMonthlyReportMemberwisePrayas(param);
                }
                else if (LoggedInOrganizationID == 218)   //= South Africa
                {
                    McsAll = weeklyReportService.GetCollectionSheetWeeklyMonthlyReportMemberwise_SouthAfrica(param);
                }
                else
                {
                    McsAll = weeklyReportService.GetDataCollectionSheetWeeklyMonthlyReportMemberwise(param);
                }


                var reportParam = new Dictionary<string, object>();

                if (LoggedInOrganizationID == 12 || LoggedInOrganizationID == 60)
                {
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("UptoDate", CollectionDate);
                    if (ExportType == "pdf")
                        ReportHelper.PrintReport("rptMonthWiseCollectionSheetWeekLyMonthlyMemberwiseWithPoneNo.rpt", McsAll.Tables[0], reportParam);
                    else
                        ReportHelper.ExportExcelReportFormated("rptMonthWiseCollectionSheetWeekLyMonthlyMemberwiseWithPoneNo.rpt", McsAll.Tables[0], reportParam, "MonthWiseCollectionSheetWeekLyMonthlyMemberwiseWithPoneNo");
                }
                else if (LoggedInOrganizationID == 6)
                {

                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("UptoDate", CollectionDate);
                    if (ExportType == "pdf")
                        ReportHelper.PrintReport("MonthlyCollectionSheetPrayas.rpt", McsAll.Tables[0], reportParam);
                    else
                        ReportHelper.ExportExcelReportFormated("MonthlyCollectionSheetPrayas.rpt", McsAll.Tables[0], reportParam, "MonthlyCollectionSheetPrayas");
                }
                else if (LoggedInOrganizationID == 58 || LoggedInOrganizationID == 42)
                {
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("UptoDate", CollectionDate);
                    if (ExportType == "pdf")
                        ReportHelper.PrintReport("rptMonthWiseCollectionSheetWeekLyMonthlyMemberwise_Mamata.rpt", McsAll.Tables[0], reportParam);
                    else
                        ReportHelper.ExportExcelReportFormated("rptMonthWiseCollectionSheetWeekLyMonthlyMemberwise_Mamata.rpt", McsAll.Tables[0], reportParam, "MonthWiseCollectionSheetWeekLyMonthlyMemberwise_Mamata");
                }
                else if (LoggedInOrganizationID == 218) //= South Africa
                {
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("UptoDate", CollectionDate);
                    if (ExportType == "pdf")
                        ReportHelper.PrintReport("rptCollectionSheet__SouthAfrica.rpt", McsAll.Tables[0], reportParam);
                    else
                        ReportHelper.ExportExcelReportFormated("rptCollectionSheet__SouthAfrica.rpt", McsAll.Tables[0], reportParam, "CollectionSheet_SouthAfrica");
                }
                else if (LoggedInOrganizationID == 190)
                {
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("UptoDate", CollectionDate);
                    //reportParam.Add("CDay", ColDay);

                    if (ExportType == "pdf")
                        ReportHelper.PrintReport("rptMonthWiseCollectionSheetWeekLyMonthlyMemberwise_RDRS.rpt", McsAll.Tables[0], reportParam);
                    else
                        ReportHelper.ExportExcelReportFormated("rptMonthWiseCollectionSheetWeekLyMonthlyMemberwise_RDRS.rpt", McsAll.Tables[0], reportParam, "rptMonthWiseCollectionSheetWeekLyMonthlyMemberwise_RDRS");

                  //  ReportHelper.PrintReport("rptMonthWiseCollectionSheetWeekLyMonthlyMemberwise_RDRS.rpt", McsAll.Tables[0], reportParam);
                }
                else
                {
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("UptoDate", CollectionDate);
                    if (ExportType == "pdf")
                        ReportHelper.PrintReport("rptMonthWiseCollectionSheetWeekLyMonthlyMemberwise_all.rpt", McsAll.Tables[0], reportParam);
                    else
                        ReportHelper.ExportExcelReportFormated("rptMonthWiseCollectionSheetWeekLyMonthlyMemberwise_all.rpt", McsAll.Tables[0], reportParam, "rptMonthWiseCollectionSheetWeekLyMonthlyMemberwise_all");
                }

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //public ActionResult GenerateCollectionSheetWeeklyMonthlyReportMemberwise(string CollectionDate)
        //{
        //    try
        //    {
        //        DataSet McsAll;
        //        var param = new { Office = SessionHelper.LoginUserOfficeID, CollectionDate = CollectionDate, Org = SessionHelper.LoginUserOrganizationID };

        //        if (LoggedInOrganizationID == 99)
        //        {

        //            //var orgId = SessionHelper.LoginUserOrganizationID;

        //            var paramValues = new List<ParameterValue>();
        //            paramValues.Add(new ParameterValue() { Name = "Org", Value = LoggedInOrganizationID.ToString() });
        //            paramValues.Add(new ParameterValue() { Name = "Office", Value = SessionHelper.LoginUserOfficeID.ToString() });
        //            paramValues.Add(new ParameterValue() { Name = "CollectionDate", Value = CollectionDate });

        //            PrintSSRSReport("/gBanker_Reports/MonthlyCollectionSheetForProsika", paramValues.ToArray(), "gBankerDbContext");
        //            return Content(string.Empty);

        //        }
        //        else if (LoggedInOrganizationID==6)
        //        {
        //            McsAll = weeklyReportService.GetDataCollectionSheetWeeklyMonthlyReportMemberwisePrayas(param);
        //        }
        //        else if (LoggedInOrganizationID == 218)   //= South Africa
        //        {
        //            McsAll = weeklyReportService.GetCollectionSheetWeeklyMonthlyReportMemberwise_SouthAfrica(param);
        //        }
        //        else
        //        {
        //            McsAll = weeklyReportService.GetDataCollectionSheetWeeklyMonthlyReportMemberwise(param);
        //        }


        //        var reportParam = new Dictionary<string, object>();

        //        if (LoggedInOrganizationID == 12 || LoggedInOrganizationID == 60)
        //        {

        //            reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //            reportParam.Add("UptoDate", CollectionDate);
        //            ReportHelper.PrintReport("rptMonthWiseCollectionSheetWeekLyMonthlyMemberwiseWithPoneNo.rpt", McsAll.Tables[0], reportParam);

        //        }
        //        else if (LoggedInOrganizationID == 6)
        //        {

        //            reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //            reportParam.Add("UptoDate", CollectionDate);
        //            //reportParam.Add("CDay", ColDay);
        //            ReportHelper.PrintReport("MonthlyCollectionSheetPrayas.rpt", McsAll.Tables[0], reportParam);

        //        }
        //        else if (LoggedInOrganizationID == 58 || LoggedInOrganizationID == 42)
        //        {
        //            reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //            reportParam.Add("UptoDate", CollectionDate);
        //            //reportParam.Add("CDay", ColDay);
        //            ReportHelper.PrintReport("rptMonthWiseCollectionSheetWeekLyMonthlyMemberwise_Mamata.rpt", McsAll.Tables[0], reportParam);
        //        }
        //        else if (LoggedInOrganizationID == 218) //= South Africa
        //        {
        //            reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //            reportParam.Add("UptoDate", CollectionDate);
        //            ReportHelper.PrintReport("rptCollectionSheet__SouthAfrica.rpt", McsAll.Tables[0], reportParam);
        //        }
        //        else if(LoggedInOrganizationID==190)
        //        {
        //            reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //            reportParam.Add("UptoDate", CollectionDate);
        //            //reportParam.Add("CDay", ColDay);
        //            ReportHelper.PrintReport("rptMonthWiseCollectionSheetWeekLyMonthlyMemberwise_RDRS.rpt", McsAll.Tables[0], reportParam);
        //        }
        //        else
        //        {
        //            reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //            reportParam.Add("UptoDate", CollectionDate);
        //            //reportParam.Add("CDay", ColDay);
        //            ReportHelper.PrintReport("rptMonthWiseCollectionSheetWeekLyMonthlyMemberwise_all.rpt", McsAll.Tables[0], reportParam);
        //        }
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}

        public ActionResult GenerateMonthlyCollectionSheetAmericaReport(string CollectionDate, string CollectionDay)
        {
            try
            {
                var param = new { Office = SessionHelper.LoginUserOfficeID, CollectionDate = CollectionDate };
                var McsAmerica = weeklyReportService.GetDataMonthlyCollectionSheet_AmericaReport(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("UptoDate", CollectionDate);
                ReportHelper.PrintReport("rptMonthlyLoanCollectionSheet_gAmerica.rpt", McsAmerica.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateCollectionSheetWeeklyMonthlyReport_WithLoanAndInterestInstallment(string CollectionDate)
        {
            try
            {

                var param = new { Office = SessionHelper.LoginUserOfficeID, CollectionDate = CollectionDate, Org = SessionHelper.LoginUserOrganizationID };
                var McsAll = weeklyReportService.GetDataCollectionSheetWeeklyMonthlyReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("UptoDate", CollectionDate);
                //reportParam.Add("CDay", ColDay);

                //ReportHelper.PrintReport("rptMonthWiseCollectionSheetForAll.rpt", McsAll.Tables[0], new Dictionary<string, object>());

                ReportHelper.PrintReport("rptMonthWiseCollectionSheetWeekLyMonthly_WithLoan&InterestInstallment.rpt", McsAll.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateCollectionSheetWeeklyMonthlyReport_WithCollection(string CollectionDate)
        {
            try
            {

                var param = new { Office = SessionHelper.LoginUserOfficeID, CollectionDate = CollectionDate, Org = SessionHelper.LoginUserOrganizationID };
                DataSet McsAll;

                if (LoggedInOrganizationID == 220)
                {
                    McsAll = weeklyReportService.GetDataCollectionSheetWeeklyMonthlyReportSagorica(param);
                }
                else
                  McsAll = weeklyReportService.GetDataCollectionSheetWeeklyMonthlyReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("UptoDate", CollectionDate);
                //reportParam.Add("CDay", ColDay);

                //ReportHelper.PrintReport("rptMonthWiseCollectionSheetForAll.rpt", McsAll.Tables[0], new Dictionary<string, object>());
                if (LoggedInOrganizationID == 99)
                {
                    ReportHelper.PrintReport("rptMonthWiseCollectionSheetWeekLyMonthly_WithCollectionProshika.rpt", McsAll.Tables[0], reportParam);

                }
                else if (LoggedInOrganizationID==5)
                {
                    ReportHelper.PrintReport("rptMonthWiseCollectionSheetWeekLyMonthly_WithCollection_JCF.rpt", McsAll.Tables[0], reportParam);
                }
                else
                ReportHelper.PrintReport("rptMonthWiseCollectionSheetWeekLyMonthly_WithCollection.rpt", McsAll.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion

        #region Events
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

        public ActionResult MCSForAll()
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

        public ActionResult CollectionSheetWeeklyMonthlyReport()
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
        public ActionResult CollectionSheetWeeklyMonthlyReportMemberwise()
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
        public ActionResult CollectionSheetWeeklyMonthlyReportMemberwiseSeperateSavings()
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

        public ActionResult MCSAmerica()
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
        public ActionResult CollectionSheetWeeklyMonthly_WithLoanAndInterestInstallment()
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

        public ActionResult CollectionSheetUsingSSRS()
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


        public ActionResult CollectionSheetWeeklyMonthlyReport_WithCollection()
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
                // TODO: Add insert logic here

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
                // TODO: Add update logic here

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
