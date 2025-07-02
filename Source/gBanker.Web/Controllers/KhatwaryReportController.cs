using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using gBanker.Service.ReportExecutionService;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace gBanker.Web.Controllers
{
    public class KhatwaryReportController : BaseController
    {
        #region Variables
        private readonly IWeeklyReportService weeklyReportService;
        private readonly IDailyReportService dailyReportService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IOfficeService officeService;
        public KhatwaryReportController(IWeeklyReportService weeklyReportService, 
            IDailyReportService dailyReportService,
            IUltimateReportService UltimateReportService, IOfficeService officeService)
        {
            this.weeklyReportService = weeklyReportService;
            this.dailyReportService = dailyReportService;
            this.ultimateReportService = UltimateReportService;
            this.officeService = officeService;
 
        }
        #endregion

        #region Methods
        public JsonResult GetMainProductList()
        {
            try
            {
                List<ProductViewModel> List_ProductViewModel = new List<ProductViewModel>();
                var productList = weeklyReportService.GetMainProductList();

                List_ProductViewModel = productList.Tables[0].AsEnumerable()
                .Select(row => new ProductViewModel
                {
                    MainProductCode = row.Field<string>("MainProductCode"),
                    MainItemName = row.Field<string>("MainItemName")

                }).ToList();

                return Json(List_ProductViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // Recoverable and Recovery Employee wise and Center wise
        public ActionResult DailyRecoverableAndRecoveryRegisterEmployeeProductDateWise()
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
            var model = new MemberViewModel();
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            //var blnk_items = new List<SelectListItem>();
            //model.CenterList = blnk_items;
            return View(model);
        }// End View

        public ActionResult GetDailyRecoverableAndRecoveryRegisterEmployeeProductDateWise(string DateFrom, string DateTo, int Qtype, int CenterID)
        {
            try
            {

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype, CenterID = CenterID };
                DataSet Weeklys;
                if (LoggedInOrganizationID == 113)
                {
                    Weeklys = ultimateReportService.DailyRecoverableAndRecoveryRegisterEmployeeCenterDateWise(param);
                }
                else
                {
                    //if (Qtype == 3)
                    //    Weeklys = weeklyReportService.DailyRecoverableAndRecoveryRegisterProductWise(param);
                    //else
                    Weeklys = ultimateReportService.DailyRecoverableAndRecoveryRegisterEmployeeCenterDateWise(param);
                }


                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                if (LoggedInOrganizationID == 113)
                {
                    if (Qtype == 1)
                    {
                        ReportHelper.PrintReport("rpt_KJ_DailyRecoverableRecoveryRegisterProductWise.rpt", Weeklys.Tables[0], reportParam);
                    }
                    if (Qtype == 2)
                    {
                        ReportHelper.PrintReport("rpt_KJ_DailyRecoverableRecoveryRegisterEmployeeWise.rpt", Weeklys.Tables[0], reportParam);
                    }

                }
                else if (LoggedInOrganizationID == 11) // For Prism
                {
                    if (Qtype == 1)
                    {
                        ReportHelper.PrintReport("rpt_KJ_DailyRecoverableRecoveryRegisterProductWise__New.rpt", Weeklys.Tables[0], reportParam);
                    }
                    if (Qtype == 2)
                    {
                        ReportHelper.PrintReport("rpt_KJ_DailyRecoverableRecoveryRegisterEmployeeWise.rpt", Weeklys.Tables[0], reportParam);
                    }
                }
                else
                {
                    if (Qtype == 1)
                    {
                        ReportHelper.PrintReport("rpt_KJ_DailyRecoverableRecoveryRegisterProductWise.rpt", Weeklys.Tables[0], reportParam);
                    }
                    if (Qtype == 2)
                    {
                        ReportHelper.PrintReport("rpt_KJ_DailyRecoverableRecoveryRegisterEmployeeWise.rpt", Weeklys.Tables[0], reportParam);
                    }

                }

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //public ActionResult DailyRecoverableAndRecoveryRegisterAreaDateWise()
        //{

        //    IEnumerable<SelectListItem> items = new SelectList(" ");
        //    ViewData["HOList"] = items;
        //    ViewData["ZoneList"] = items;
        //    ViewData["AreaList"] = items;
        //    ViewData["OfficeList"] = items;
        //    ViewData["AccCodeList"] = items;
        //    var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
        //    //ViewData["OfficeLevel"] = offcdetail.OfficeLevel;
        //    ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];

        //    if (offcdetail.OfficeLevel == 1)
        //    {
        //        ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
        //        ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
        //        ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
        //        ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
        //    }
        //    else
        //    {
        //        ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
        //        ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
        //        ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
        //        ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;
        //    }

        //    ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");

        //    DateTime VDate;
        //    VDate = System.DateTime.Now;
        //    if (IsDayInitiated)
        //    {
        //        ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
        //    }
        //    else
        //    {
        //        ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
        //    }
        //    //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
        //    var model = new MemberViewModel();
        //    model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
        //    var blnk_items = new List<SelectListItem>();
        //    model.CenterList = blnk_items;
        //    return View(model);
        //}

        public ActionResult DailyRecoverableAndRecoveryRegisterAreaDateWise()
        {

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["AccCodeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            //ViewData["OfficeLevel"] = offcdetail.OfficeLevel;
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];

            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;
            }

            ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");

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
            var model = new MemberViewModel();
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            var blnk_items = new List<SelectListItem>();
            model.CenterList = blnk_items;
            return View(model);
        }




        //public ActionResult ProcessDailyRecoverableAndRecoveryRegisterAreaDateWise(string DateFrom, string DateTo, string ddlZone, string ddlArea)
        //{
        //    try
        //    {
        //        var param = new { DateFrom = DateFrom, DateTo = DateTo };
        //        DataSet Weeklys;

        //        Weeklys = weeklyReportService.ProcessRecoverableAndRecoveryRegisterAreaDateWise(param);

        //        return Json(new { Result = "SUCCESS", Message = "SUCCESS" }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        public ActionResult ProcessDailyRecoverableAndRecoveryRegisterAreaDateWise(string DateFrom, string DateTo, string ddlZone, string ddlArea)
        {
            try
            {
                var param = new { DateFrom = DateFrom, DateTo = DateTo };
                DataSet Weeklys;

                Weeklys = weeklyReportService.ProcessRecoverableAndRecoveryRegisterAreaDateWise(param);

                return Json(new { Result = "SUCCESS", Message = "SUCCESS" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        //public ActionResult GetDailyRecoverableAndRecoveryRegisterAreaDateWise(string DateFrom, string DateTo, string ddlZone, string ddlArea)
        //{
        //    try
        //    {

        //        var param = new { DateFrom = DateFrom, DateTo = DateTo };
        //        DataSet Weeklys;

        //        Weeklys = weeklyReportService.DailyRecoverableAndRecoveryRegisterAreaDateWise(param);

        //        var reportParam = new Dictionary<string, object>();
        //        reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
        //        reportParam.Add("DateFrom", DateFrom);
        //        reportParam.Add("DateTo", DateTo);

        //        ReportHelper.PrintReport("DailyRecovarableAndRecoveryRegisterAreaDateWise.rpt", Weeklys.Tables[0], reportParam);

        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}

      
        public ActionResult GetDailyRecoverableAndRecoveryRegisterAreaDateWise(string Qtype, string OfficeId, string DateFrom, string DateTo)
        {
            try
            {

                var param = new { Qtype = Convert.ToInt32(Qtype), OfficeID = Convert.ToInt32(OfficeId), Org = SessionHelper.LoginUserOrganizationID,  DateTo = DateTo  };
                DataSet Weeklys;
            
                Weeklys = weeklyReportService.DailyRecoverableAndRecoveryRegisterAreaDateWise(param);  

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);

                 ReportHelper.PrintReport("DailyRecovarableAndRecoveryRegisterAreaDateWise.rpt", Weeklys.Tables[0], reportParam);
                  
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        //public ActionResult CheckProcessDailyRecoverableAndRecoveryRegisterAreaDateWise(string DateFrom, string DateTo)
        //{
        //    try
        //    {
        //        var param = new { DateFrom = DateFrom, DateTo = DateTo };
        //        DataSet Weeklys;

        //        Weeklys = weeklyReportService.CheckProcessRecoverableAndRecoveryRegisterAreaDateWise(param);

        //        var result = Weeklys.Tables[0].Rows[0]["RESULT"].ToString();

        //        return Json(new { Result = result, Message = result }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}




        public ActionResult CheckProcessDailyRecoverableAndRecoveryRegisterAreaDateWise(string DateFrom, string DateTo)
        {
            try
            {
                var param = new { DateFrom = DateFrom, DateTo = DateTo };
                DataSet Weeklys;

                Weeklys = weeklyReportService.CheckProcessRecoverableAndRecoveryRegisterAreaDateWise(param);

                var result = Weeklys.Tables[0].Rows[0]["RESULT"].ToString();

                return Json(new { Result = result, Message = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetMainSavingProductList()
        {
            try
            {
                List<ProductViewModel> List_ProductViewModel = new List<ProductViewModel>();
                var productList = weeklyReportService.GetSavingMainProductList();

                List_ProductViewModel = productList.Tables[0].AsEnumerable()
                .Select(row => new ProductViewModel
                {
                    MainProductCode = row.Field<string>("MainProductCode"),
                    MainItemName = row.Field<string>("MainItemName")

                }).ToList();

                return Json(List_ProductViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetKhatWaryReport(string MainProductCode, string DateFrom, string DateTo, string Qtype)
        {
            try
            {

                var param = new { Qtype = Qtype, OfficeID = SessionHelper.LoginUserOfficeID, MainProductCode = MainProductCode.Trim(), Date_From = DateFrom, Date_To = DateTo };
                var Weeklys = weeklyReportService.GetKhatWaryReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                          
                ReportHelper.PrintReport("KhatWaryReport.rpt", Weeklys.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GetKhatWaryReportExport(string MainProductCode, string DateFrom, string DateTo, string Qtype)
        {
            try
            {

                var param = new { Qtype = Qtype, OfficeID = SessionHelper.LoginUserOfficeID, MainProductCode = MainProductCode.Trim(), Date_From = DateFrom, Date_To = DateTo };
                var allRepaymentSchedule = weeklyReportService.GetKhatWaryReport(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.ExportExcelReport("KhatWaryReport.rpt", allRepaymentSchedule.Tables[0], reportParam);
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GetRebateReport(string DateFrom, string DateTo, string ExportType)
        {
            try
            {
                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, Date1 = DateFrom, Date2 = DateTo };
                var Weeklys = weeklyReportService.GetRebateReport(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                if (ExportType == "pdf")
                    ReportHelper.PrintReport("RebateInfo.rpt", Weeklys.Tables[0], reportParam);
                else
                    ReportHelper.ExportExcelReportFormated("RebateInfo1.rpt", Weeklys.Tables[0], reportParam, "RebateReport");

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //public ActionResult GetRebateReport(string DateFrom, string DateTo)
        //{
        //    try
        //    {
        //        var param = new { OfficeID = SessionHelper.LoginUserOfficeID, Date1 = DateFrom, Date2 = DateTo };
        //        var Weeklys = weeklyReportService.GetRebateReport(param);
        //        var reportParam = new Dictionary<string, object>();
        //        reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
        //        reportParam.Add("DateFrom", DateFrom);
        //        reportParam.Add("DateTo", DateTo);
        //        ReportHelper.PrintReport("RebateInfo.rpt", Weeklys.Tables[0], reportParam);
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        public ActionResult GetDualLoanReport(string DateFrom)
        {
            try
            {


                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, TrxDate = DateFrom };
                var Weeklys = weeklyReportService.GetDualLoanReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
               

                ReportHelper.PrintReport("rptDual_Loan_Info.rpt", Weeklys.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GetNagtiveLedgerBalance(string DateFrom, string DateTo)
        {
            try
            {
           
                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo };
                var Weeklys = weeklyReportService.GetNegativeLoanLedgerBalance(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                             
                ReportHelper.PrintReport("NegativeLedgerBalance.rpt", Weeklys.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GetStaffWiseSpecialSavingReport(string DateFrom, string DateTo)
        {
            try
            {
               
                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, Date_From = DateFrom, Date_To = DateTo };
                var Weeklys = weeklyReportService.GetStaffWiseSpecialSavingReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                              
                ReportHelper.PrintReport("StaffWiseSpecialSavingReport.rpt", Weeklys.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult GetMemberTransfer(string DateFrom, string DateTo, int Qtype)
        {
            try
            {

                var param = new { Qtype = Qtype, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, };
                    var Weeklys = weeklyReportService.GetMemberTransfer(param);

                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateFrom", DateFrom);
                    reportParam.Add("DateTo", DateTo);


                    ReportHelper.PrintReport("rptmembertransfer.rpt", Weeklys.Tables[0], reportParam);
                
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GetDailyRecoverableAndRecoveryRegisterCenterDateWise(string DateFrom, string DateTo, int Qtype, int CenterID, string ExportType)
        {
            try
            {

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype, CenterID = CenterID };
                DataSet Weeklys;
                if (LoggedInOrganizationID == 113)
                {
                    Weeklys = weeklyReportService.DailyRecoverableAndRecoveryRegisterCenterDateWise(param);
                }
                else
                {
                    if (Qtype == 3)
                        Weeklys = weeklyReportService.DailyRecoverableAndRecoveryRegisterProductWiseFROMPKSF(param);
                    else
                        Weeklys = weeklyReportService.DailyRecoverableAndRecoveryRegisterCenterDateWiseFROMPKSD(param);
                }


                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                if (LoggedInOrganizationID == 113)
                {
                    if (Qtype == 1)
                    {
                        if (ExportType == "pdf")
                            ReportHelper.PrintReport("DailyRecovarableAndRecoveryRegisterCenterDateWise_Adra.rpt", Weeklys.Tables[0], reportParam);
                        else
                            ReportHelper.ExportExcelReportFormated("DailyRecovarableAndRecoveryRegisterCenterDateWise_Adra.rpt", Weeklys.Tables[0], reportParam, "DailyRecoverableAndRecoveryRegister");
                    }
                    if (Qtype == 2)
                    {
                        if (ExportType == "pdf")
                            ReportHelper.PrintReport("RecoverableAndRecoveryRegisterLoaneeDateWise.rpt", Weeklys.Tables[0], reportParam);
                        else
                            ReportHelper.ExportExcelReportFormated("RecoverableAndRecoveryRegisterLoaneeDateWise.rpt", Weeklys.Tables[0], reportParam, "DailyRecoverableAndRecoveryRegister");
                    }
                    if (Qtype == 5)
                    {
                        if (ExportType == "pdf")
                            ReportHelper.PrintReport("RecoverableAndRecoveryRegisterLoaneeDateWise.rpt", Weeklys.Tables[0], reportParam);
                        else
                            ReportHelper.ExportExcelReportFormated("RecoverableAndRecoveryRegisterLoaneeDateWise.rpt", Weeklys.Tables[0], reportParam, "DailyRecoverableAndRecoveryRegister");
                    }
                }
                else
                {
                    if (Qtype == 1)
                    {
                        if (ExportType == "pdf")
                            ReportHelper.PrintReport("DailyRecovarableAndRecoveryRegisterCenterDateWise.rpt", Weeklys.Tables[0], reportParam);
                        else
                            ReportHelper.ExportExcelReportFormated("DailyRecovarableAndRecoveryRegisterCenterDateWise.rpt", Weeklys.Tables[0], reportParam, "DailyRecoverableAndRecoveryRegister");
                    }
                    if (Qtype == 2)
                    {
                        if (ExportType == "pdf")
                            ReportHelper.PrintReport("RecoverableAndRecoveryRegisterLoaneeDateWise.rpt", Weeklys.Tables[0], reportParam);
                        else
                            ReportHelper.ExportExcelReportFormated("RecoverableAndRecoveryRegisterLoaneeDateWise.rpt", Weeklys.Tables[0], reportParam, "DailyRecoverableAndRecoveryRegister");
                    }

                    if (Qtype == 3)
                    {
                        if (ExportType == "pdf")
                            ReportHelper.PrintReport("RecoverableAndRecoveryRegisterLoaneeProductWise.rpt", Weeklys.Tables[0], reportParam);
                        else
                            ReportHelper.ExportExcelReportFormated("RecoverableAndRecoveryRegisterLoaneeProductWise.rpt", Weeklys.Tables[0], reportParam, "DailyRecoverableAndRecoveryRegister");
                    }

                    if (Qtype == 5)
                    {
                        if (ExportType == "pdf")
                            ReportHelper.PrintReport("RecoverableAndRecoveryRegisterLoaneeDateWise.rpt", Weeklys.Tables[0], reportParam);
                        else
                            ReportHelper.ExportExcelReportFormated("RecoverableAndRecoveryRegisterLoaneeDateWise.rpt", Weeklys.Tables[0], reportParam, "DailyRecoverableAndRecoveryRegister");
                    }
                }

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //public ActionResult GetDailyRecoverableAndRecoveryRegisterCenterDateWise(string DateFrom, string DateTo, int Qtype, int CenterID)
        //{
        //    try
        //    {

        //        var param = new { OfficeID = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype, CenterID = CenterID };
        //        DataSet Weeklys;
        //        if (LoggedInOrganizationID == 113)
        //        {
        //            Weeklys = weeklyReportService.DailyRecoverableAndRecoveryRegisterCenterDateWise(param);
        //        }
        //        else
        //        {
        //            if (Qtype == 3)
        //                Weeklys = weeklyReportService.DailyRecoverableAndRecoveryRegisterProductWiseFROMPKSF(param);
        //            else
        //                Weeklys = weeklyReportService.DailyRecoverableAndRecoveryRegisterCenterDateWiseFROMPKSD(param);
        //        }


        //        var reportParam = new Dictionary<string, object>();
        //        reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
        //        reportParam.Add("DateFrom", DateFrom);
        //        reportParam.Add("DateTo", DateTo);
        //        if (LoggedInOrganizationID == 113)
        //        {
        //            if (Qtype == 1)
        //            {
        //                ReportHelper.PrintReport("DailyRecovarableAndRecoveryRegisterCenterDateWise_Adra.rpt", Weeklys.Tables[0], reportParam);
        //            }
        //            if (Qtype == 2)
        //            {
        //                ReportHelper.PrintReport("RecoverableAndRecoveryRegisterLoaneeDateWise.rpt", Weeklys.Tables[0], reportParam);
        //            }
        //            if (Qtype == 5)
        //            {
        //                ReportHelper.PrintReport("RecoverableAndRecoveryRegisterLoaneeDateWise.rpt", Weeklys.Tables[0], reportParam);
        //            }
        //        }
        //        else
        //        {
        //            if (Qtype == 1)
        //            {
        //                ReportHelper.PrintReport("DailyRecovarableAndRecoveryRegisterCenterDateWise.rpt", Weeklys.Tables[0], reportParam);
        //            }
        //            if (Qtype == 2)
        //            {
        //                ReportHelper.PrintReport("RecoverableAndRecoveryRegisterLoaneeDateWise.rpt", Weeklys.Tables[0], reportParam);
        //            }

        //            if (Qtype == 3)
        //            {
        //                ReportHelper.PrintReport("RecoverableAndRecoveryRegisterLoaneeProductWise.rpt", Weeklys.Tables[0], reportParam);
        //            }

        //            if (Qtype == 5)
        //            {
        //                ReportHelper.PrintReport("RecoverableAndRecoveryRegisterLoaneeDateWise.rpt", Weeklys.Tables[0], reportParam);
        //            }
        //        }

        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}

        public ActionResult GetRecoverableAndRecoveryRegisterDSKFormat(string DateTo, int Qtype)
        {
            try
            {


                var paramValues = new List<ParameterValue>();

                paramValues.Add(new ParameterValue() { Name = "Qtype", Value = Convert.ToString(Qtype) });
                paramValues.Add(new ParameterValue() { Name = "Org", Value = Convert.ToString(LoggedInOrganizationID) });
                paramValues.Add(new ParameterValue() { Name = "Office", Value = Convert.ToString(LoginUserOfficeID) });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = DateTo});
                PrintSSRSReport("/gBanker_Reports/Recoverable_Register", paramValues.ToArray(), "gBankerDbContext");
                return Content(string.Empty);


                //var param = new { Qtype= Qtype, Org=LoggedInOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = DateTo };
                //var Weeklys = weeklyReportService.RecoverableAndRecoveryRegisterDSKFormat(param);

                //var reportParam = new Dictionary<string, object>();
                //reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("DateFrom", DateFrom);
                //reportParam.Add("DateTo", DateTo);

                //if (Qtype == 1)
                //{
                //    ReportHelper.PrintReport("DailyRecovarableAndRecoveryRegisterCenterDateWise.rpt", Weeklys.Tables[0], reportParam);


                //}
                //if (Qtype == 2)
                //{


                //    ReportHelper.PrintReport("RecoverableAndRecoveryRegisterLoaneeDateWise.rpt", Weeklys.Tables[0], reportParam);


                //}


                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        // GET: KhatwaryReport
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

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["MainProductView"] = items;
            //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult RebateInfo()
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
        public ActionResult DualLoanInfo()
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
        public ActionResult NegativeLedgerBalance()
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
        public ActionResult StaffWiseSpecialSavingReport()
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
        //public ActionResult DailyRecoverableAndRecoveryRegisterCenterDateWise()
        // {
        //     DateTime VDate;
        //     VDate = System.DateTime.Now;
        //     if (IsDayInitiated)
        //     {
        //         ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
        //     }
        //     else
        //     {
        //         ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
        //     }
        //     //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
        //     return View();
        // }

        public ActionResult DailyRecoverableAndRecoveryRegisterCenterDateWise()
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
            var model = new MemberViewModel();
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            var blnk_items = new List<SelectListItem>();
            model.CenterList = blnk_items;
            return View(model);
        }

        //private void MapDropDownList(MemberViewModel model)
        //{
        //    List<MemberViewModel> List_MemberViewModel = new List<MemberViewModel>();
        //    var param = new { SearchByCode = "", SearchBy = "", SearchType = "dis" };
        //    var disList = ultimateReportService.GetLocationList(param);

        //    List_MemberViewModel = disList.Tables[0].AsEnumerable()
        //    .Select(row => new MemberViewModel
        //    {
        //        DistrictCode = row.Field<string>("DistrictCode"),
        //        DistrictName = row.Field<string>("DistrictName")
        //    }).ToList();
        //    var viewDist = List_MemberViewModel.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.DistrictCode.ToString(),
        //        Text = x.DistrictName.ToString()
        //    });
        //    var pob_items = new List<SelectListItem>();
        //    pob_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
        //    pob_items.AddRange(viewDist);
        //    model.PlaceOfBirthList = pob_items;
        //}



            public ActionResult MemberTransferReport()
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
        public ActionResult RecoverableAndRecoveryRegisterDSK()
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
        #endregion

        #region Events
        // GET: KhatwaryReport/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: KhatwaryReport/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: KhatwaryReport/Create
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

        // GET: KhatwaryReport/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: KhatwaryReport/Edit/5
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

        // GET: KhatwaryReport/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: KhatwaryReport/Delete/5
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
