using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.Reports;
using gBanker.Web.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class AccRcvPayReportController : BaseController
    {
        #region Variables
        private readonly IAccTrxMasterService accTrxMasterService;
        private readonly IAccTrxDetailService accTrxDetailService;
        private readonly IAccChartService accChartService;
        private readonly IProcessInfoService processInfoService;
        private readonly IAccReportService accReportService;
        private readonly IOfficeService officeService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IArchiveDbMappingService archiveDbMappingService;
        public AccRcvPayReportController(IAccTrxMasterService accTrxMasterService, IAccTrxDetailService accTrxDetailService, IAccChartService accChartService, IProcessInfoService processInfoService, IAccReportService accReportService, IOfficeService officeService, IUltimateReportService ultimateReportService, IArchiveDbMappingService archiveDbMappingService)
        {
            this.accTrxMasterService = accTrxMasterService;
            this.accTrxDetailService = accTrxDetailService;
            this.accChartService = accChartService;            
            this.processInfoService = processInfoService;
            this.accReportService = accReportService;
            this.officeService = officeService;
            this.ultimateReportService = ultimateReportService;
            this.archiveDbMappingService = archiveDbMappingService;
        }
        #endregion

        #region Methods
        public JsonResult GetZoneListByOffice(string HO_val)
        {

            var vUser = LoggedInEmployeeID;

            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 2 && c.OfficeID == LoginUserOfficeID && c.OrgID == Convert.ToInt32(LoggedInOrganizationID));
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOfficeListByOffice(string HO_val, string zone_val, string area_val)
        {
          
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 4 && c.OfficeID == LoginUserOfficeID && c.OrgID == Convert.ToInt32(LoggedInOrganizationID));
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAreaListByOffice(string HO_val, string zone_val)
        {
            //var ho_code = officeService.GetById(Convert.ToInt32(HO_val)).OfficeCode;
            //var zone_code = officeService.GetById(Convert.ToInt32(zone_val)).OfficeCode;
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 3 && c.OfficeID == LoginUserOfficeID && c.OrgID == Convert.ToInt32(LoggedInOrganizationID));
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetHOList()
        {
            var First_Level = officeService.GetByOfficeOrgID(Convert.ToInt32(SessionHelper.LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID));
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 1 && c.FirstLevel == First_Level.FirstLevel);
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetZoneList(string HO_val)
        {
            var ofcId = officeService.GetById(Convert.ToInt32(HO_val)).OfficeCode;
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 2 && c.FirstLevel == ofcId && c.OrgID == Convert.ToInt32(LoggedInOrganizationID));
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
            //List<OfficeViewModel> OfficeList = new List<OfficeViewModel>();
            //var param = new { EmployeeID = LoggedInEmployeeID };
            //var div_items = ultimateReportService.GetOfficeListByEmployee(param);

            //OfficeList = div_items.Tables[0].AsEnumerable()
            //.Select(row => new OfficeViewModel
            //{
            //    OfficeID = row.Field<int>("OfficeID"),
            //    OfficeCode = row.Field<string>("OfficeCode"),
            //    OfficeName = row.Field<string>("OfficeName")
            //}).ToList();
            ////var ofcId = officeService.GetById(Convert.ToInt32(HO_val)).OfficeCode;
            ////var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 2 && c.FirstLevel == ofcId && c.OrgID == Convert.ToInt32(LoggedInOrganizationID));
            //var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.OfficeID.ToString(),
            //    Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            //});
            //var office_items = new List<SelectListItem>();
            //if (viewOffice.ToList().Count > 0)
            //{
            //    office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //}
            //office_items.AddRange(viewOffice);
            //return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAreaList(string HO_val, string zone_val)
        {
            var ho_code = officeService.GetById(Convert.ToInt32(HO_val)).OfficeCode;
            var zone_code = officeService.GetById(Convert.ToInt32(zone_val)).OfficeCode;
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 3 && c.FirstLevel == ho_code && c.SecondLevel == zone_code && c.OrgID == Convert.ToInt32(LoggedInOrganizationID));
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOfficeList(string HO_val, string zone_val, string area_val)
        {
            var ho_code = officeService.GetById(Convert.ToInt32(HO_val)).OfficeCode;
            var zone_code = officeService.GetById(Convert.ToInt32(zone_val)).OfficeCode;
            var area_code = officeService.GetById(Convert.ToInt32(area_val)).OfficeCode;
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 4 && c.FirstLevel == ho_code && c.SecondLevel == zone_code && c.ThirdLevel == area_code && c.OrgID == Convert.ToInt32(LoggedInOrganizationID));
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerateAdminCostRcvPayReport(string office_id, string from_date, string to_date, string acc_level, string qType)
        {
           
            try
            {
                if (qType == "1" || qType == "0")
                {
                    office_id = "1";

                }


                var main_param = new object();

                if (LoggedInOrganizationID != 5)
                {

                
                if (SessionHelper.LoginUserOrganizationID == 12 || SessionHelper.LoginUserOrganizationID == 10 || SessionHelper.LoginUserOrganizationID == 50)
                    main_param = new { office_id = office_id, orgName = ApplicationSettings.OrganiztionName, FromDate = to_date, ToDate = to_date, qtype = (string.IsNullOrEmpty(qType) ? 0 : int.Parse(qType)) };
                else
                    main_param = new { office_id = office_id, orgName = ApplicationSettings.OrganiztionName, FromDate = to_date, ToDate = to_date };
                //var main_param = new { office_id = office_id, orgName = ApplicationSettings.OrganiztionName, FromDate = from_date, ToDate = to_date, qtype = (string.IsNullOrEmpty(qType) ? 0 : int.Parse(qType)) };
                var rcv_param = new { from_date = from_date, to_date = to_date, office_id = office_id, AccLevel = acc_level, And_Condition = " AND t.AccType='R'", AllOffice = qType };
                var pay_param = new { from_date = from_date, to_date = to_date, office_id = office_id, AccLevel = acc_level, And_Condition = " AND t.AccType='P'", AllOffice = qType };
                var allVouchers = accReportService.GetDataIncExpReport(main_param);

                //var Rcv_DB = accReportService.GetNewAccDataForReport(rcv_param, "Proc_Rpt_Acc_ReceivePaymentAdminCost");
                var Pay_DB = accReportService.GetNewAccDataForReport(pay_param, "Proc_Rpt_Acc_ReceivePaymentAdminCost");

                var subReportDB = new Dictionary<string, DataTable>();
                //subReportDB.Add("rpt_acc_sub_receipt", Rcv_DB.Tables[0]);
                subReportDB.Add("rpt_acc_sub_payment", Pay_DB.Tables[0]);

                ReportHelper.PrintWithSubReport("rpt_acc_Receipt_Payment_AdminCost.rpt", allVouchers.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_acc_Receipt_Payment_AdminCost());
                }
                if (LoggedInOrganizationID == 5)
                {
                    main_param = new { office_id = office_id, orgName = ApplicationSettings.OrganiztionName, FromDate = to_date, ToDate = to_date };
                    var allVouchers_Jcf = accReportService.GetDataIncExpReportJCF(main_param);
                    var param = new { from_date = from_date, to_date = to_date, office_id = office_id, AccLevel = acc_level, AllOffice = qType };
                    var allVouchers_recv = accReportService.GetDataAdminCostTrialBalanceRecv(param);
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("from_date", from_date);
                    reportParam.Add("to_date", to_date);
                    var subReportDB = new Dictionary<string, DataTable>();
                    //subReportDB.Add("rpt_acc_sub_receipt", Rcv_DB.Tables[0]);
                    //subReportDB.Add("rpt_acc_sub_payment", allVouchers_recv.Tables[0]);
                    ReportHelper.PrintReport("rpt_acc_trial_balance_AdminCostRecvJCF.rpt", allVouchers_recv.Tables[0], reportParam);
                    //ReportHelper.PrintWithSubReport("rpt_acc_trial_balance_AdminCostRecvJCF.rpt.rpt", allVouchers_Jcf.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_acc_Receipt_Payment_AdminCost());
                }

                return Content(string.Empty);

              
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //public ActionResult GenerateRcvPayReport(string office_id, string from_date, string to_date, string acc_level, string qType)
        //{
        //    try
        //    {
        //        if (qType == "1" || qType == "0")
        //        {
        //            office_id = "1";

        //        }
        //        var main_param = new object();
        //        if (SessionHelper.LoginUserOrganizationID == 12 || SessionHelper.LoginUserOrganizationID == 10 || SessionHelper.LoginUserOrganizationID == 50)
        //            main_param = new { office_id = office_id, orgName = ApplicationSettings.OrganiztionName, FromDate = from_date, ToDate = to_date, qtype = (string.IsNullOrEmpty(qType) ? 0 : int.Parse(qType)) };
        //        else
        //            main_param = new { office_id = office_id, orgName = ApplicationSettings.OrganiztionName, FromDate = from_date, ToDate = to_date };

        //        var rcv_param = new { from_date = from_date, to_date = to_date, office_id = office_id, AccLevel = acc_level, And_Condition = " AND t.AccType='R'", AllOffice = qType };
        //        var pay_param = new { from_date = from_date, to_date = to_date, office_id = office_id, AccLevel = acc_level, And_Condition = " AND t.AccType='P'", AllOffice = qType };
        //        var allVouchers = accReportService.GetDataIncExpReport(main_param);

        //        var Rcv_DB = accReportService.GetNewAccDataForReport(rcv_param, "Proc_Rpt_Acc_ReceivePayment");
        //        var Pay_DB = accReportService.GetNewAccDataForReport(pay_param, "Proc_Rpt_Acc_ReceivePayment");

        //        var subReportDB = new Dictionary<string, DataTable>();
        //        subReportDB.Add("rpt_acc_sub_receipt", Rcv_DB.Tables[0]);
        //        subReportDB.Add("rpt_acc_sub_payment", Pay_DB.Tables[0]);

        //        ReportHelper.PrintWithSubReport("rpt_acc_receipt_payment.rpt", allVouchers.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_acc_receipt_payment());
        //        return Content(string.Empty);

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        //=================== Rcv Payment VERC HO : Mostak - 16 Apr 2024 =======================

        public ActionResult RcvPayHo()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;

            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            var HeadOfficeCode = offcdetail.FirstLevel;
            var Headoffcdetail = officeService.GetByOfficeCode(HeadOfficeCode);
            var Secondoffcdetail = officeService.GetByOfficeCode(offcdetail.SecondLevel);
            var Thirdoffcdetail = officeService.GetByOfficeCode(offcdetail.ThirdLevel);
            var Fourthoffcdetail = officeService.GetByOfficeCode(offcdetail.FourthLevel);

            ViewData["OfficeLevel"] = offcdetail.OfficeLevel;

            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = Headoffcdetail.OfficeID;
                ViewData["SecondLevel"] = "";
                ViewData["ThirdLevel"] = "";
                ViewData["FourthLevel"] = "";
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = Headoffcdetail.OfficeID;
                ViewData["SecondLevel"] = Secondoffcdetail.OfficeID;
                ViewData["ThirdLevel"] = "";
                ViewData["FourthLevel"] = "";
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = Headoffcdetail.OfficeID;
                ViewData["SecondLevel"] = Secondoffcdetail.OfficeID;
                ViewData["ThirdLevel"] = Thirdoffcdetail.OfficeID;
                ViewData["FourthLevel"] = "";
            }
            else
            {
                ViewData["FirstLevel"] = Headoffcdetail.OfficeID;
                ViewData["SecondLevel"] = Secondoffcdetail.OfficeID;
                ViewData["ThirdLevel"] = Thirdoffcdetail.OfficeID;
                ViewData["FourthLevel"] = Fourthoffcdetail.OfficeID;

            }
            var param = new { @OfficeID = SessionHelper.LoggedInEmployee.OfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);
            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }

            return View();
        }


        public ActionResult GenerateRcvPayReportHO(string office_id, string from_date, string to_date, string acc_level, string qType, string ddlReportType)
        {
            try
            {
                if (qType == "1" || qType == "0")
                {
                    office_id = "1";

                }
                var main_param = new object();
                if (SessionHelper.LoginUserOrganizationID == 12 || SessionHelper.LoginUserOrganizationID == 10 || SessionHelper.LoginUserOrganizationID == 50)
                    main_param = new { office_id = office_id, orgName = ApplicationSettings.OrganiztionName, FromDate = from_date, ToDate = to_date, qtype = (string.IsNullOrEmpty(qType) ? 0 : int.Parse(qType)) };
                else
                    main_param = new { office_id = office_id, orgName = ApplicationSettings.OrganiztionName, FromDate = from_date, ToDate = to_date };

                var rcv_param = new { from_date = from_date, to_date = to_date, office_id = office_id, AccLevel = acc_level, And_Condition = " AND t.AccType='R'", AllOffice = qType };
                var pay_param = new { from_date = from_date, to_date = to_date, office_id = office_id, AccLevel = acc_level, And_Condition = " AND t.AccType='P'", AllOffice = qType };
                var allVouchers = accReportService.GetDataWithParameterFromGDA(main_param, "Proc_Rpt_Acc_OffcInfo");

                var Rcv_DB = accReportService.GetDataWithParameterFromGDA(rcv_param, "Proc_Rpt_Acc_ReceivePayment_HO");
                var Pay_DB = accReportService.GetDataWithParameterFromGDA(pay_param, "Proc_Rpt_Acc_ReceivePayment_HO");

                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("rpt_acc_sub_receipt", Rcv_DB.Tables[0]);
                subReportDB.Add("rpt_acc_sub_payment", Pay_DB.Tables[0]);
                if (ddlReportType == "pdf")
                    ReportHelper.PrintWithSubReport("rpt_acc_receipt_payment.rpt", allVouchers.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_acc_receipt_payment());

                if (ddlReportType == "xls")
                    ReportHelper.ExportExcelWithSubReport("rpt_acc_receipt_payment.rpt", allVouchers.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_acc_receipt_payment());

                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateRcvPayReport(string office_id, string from_date, string to_date, string acc_level, string qType, string ddlReportType)
        {
            try
            {
                if (qType == "1" || qType == "0")
                {
                    office_id = "1";

                }
                var main_param = new object();
                if (SessionHelper.LoginUserOrganizationID == 12 || SessionHelper.LoginUserOrganizationID == 10 || SessionHelper.LoginUserOrganizationID == 50)
                    main_param = new { office_id = office_id, orgName = ApplicationSettings.OrganiztionName, FromDate = from_date, ToDate = to_date, qtype = (string.IsNullOrEmpty(qType) ? 0 : int.Parse(qType)) };
                else
                    main_param = new { office_id = office_id, orgName = ApplicationSettings.OrganiztionName, FromDate = from_date, ToDate = to_date };

                var rcv_param = new { from_date = from_date, to_date = to_date, office_id = office_id, AccLevel = acc_level, And_Condition = " AND t.AccType='R'", AllOffice = qType };
                var pay_param = new { from_date = from_date, to_date = to_date, office_id = office_id, AccLevel = acc_level, And_Condition = " AND t.AccType='P'", AllOffice = qType };
                var allVouchers = accReportService.GetDataIncExpReport(main_param);

                //DataSet Rcv_DB;
                //DataSet Pay_DB;


                //Rcv_DB = accReportService.GetNewAccDataForReport(rcv_param, "Proc_Rpt_Acc_ReceivePayment");
                //Pay_DB = accReportService.GetNewAccDataForReport(pay_param, "Proc_Rpt_Acc_ReceivePayment");

                if (LoggedInOrganizationID == 190  )
                {
                  var  Rcv_DB = accReportService.GetNewAccDataForReportRDRS(rcv_param, "Proc_Rpt_Acc_ReceivePayment");
                  var  Pay_DB = accReportService.GetNewAccDataForReportRDRS(pay_param, "Proc_Rpt_Acc_ReceivePayment");


                    var subReportDB = new Dictionary<string, DataTable>();
                    subReportDB.Add("rpt_acc_sub_receipt", Rcv_DB.Tables[0]);
                    subReportDB.Add("rpt_acc_sub_payment", Pay_DB.Tables[0]);

                    if (ddlReportType == "pdf")
                        ReportHelper.PrintWithSubReport("rpt_acc_receipt_payment.rpt", allVouchers.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_acc_receipt_payment());

                    if (ddlReportType == "xls")
                        ReportHelper.ExportExcelWithSubReport("rpt_acc_receipt_payment.rpt", allVouchers.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_acc_receipt_payment());

                }
                else
                {
                    var Rcv_DB_1 = accReportService.GetNewAccDataForReport(rcv_param, "Proc_Rpt_Acc_ReceivePayment");
                    var Pay_DB_1 = accReportService.GetNewAccDataForReport(pay_param, "Proc_Rpt_Acc_ReceivePayment");


                    var subReportDB_1 = new Dictionary<string, DataTable>();
                    subReportDB_1.Add("rpt_acc_sub_receipt", Rcv_DB_1.Tables[0]);
                    subReportDB_1.Add("rpt_acc_sub_payment", Pay_DB_1.Tables[0]);

                    if (ddlReportType == "pdf")
                        ReportHelper.PrintWithSubReport("rpt_acc_receipt_payment.rpt", allVouchers.Tables[0], new Dictionary<string, object>(), subReportDB_1, new rpt_acc_receipt_payment());

                    if (ddlReportType == "xls")
                        ReportHelper.ExportExcelWithSubReport("rpt_acc_receipt_payment.rpt", allVouchers.Tables[0], new Dictionary<string, object>(), subReportDB_1, new rpt_acc_receipt_payment());

                }



                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateRcvPayReportArchive(string office_id, string from_date, string to_date, string acc_level, string qType)
        {
            try
            {
                if (qType == "1" || qType == "0")
                {
                    office_id = "1";

                }
                var main_param = new object();
                if (SessionHelper.LoginUserOrganizationID == 12 || SessionHelper.LoginUserOrganizationID == 10)
                    main_param = new { office_id = office_id, orgName = ApplicationSettings.OrganiztionName, FromDate = from_date, ToDate = to_date, qtype = (string.IsNullOrEmpty(qType) ? 0 : int.Parse(qType)) };
                else
                    main_param = new { office_id = office_id, orgName = ApplicationSettings.OrganiztionName, FromDate = from_date, ToDate = to_date };


                var archiveMapDb = "";
                var archiveMap = archiveDbMappingService.GetMany(p => p.OrgId == LoggedInOrganizationID).FirstOrDefault();
                if (archiveMap != null)
                    archiveMapDb = archiveMap.ArchiveDbName;

                var rcv_param = new { from_date = from_date, to_date = to_date, office_id = office_id, AccLevel = acc_level, And_Condition = " AND t.AccType='R'", AllOffice = qType, archiveMapDb = archiveMapDb };
                var pay_param = new { from_date = from_date, to_date = to_date, office_id = office_id, AccLevel = acc_level, And_Condition = " AND t.AccType='P'", AllOffice = qType, archiveMapDb = archiveMapDb };
                var allVouchers = accReportService.GetDataIncExpReport(main_param);

                var Rcv_DB = accReportService.GetNewAccDataForReport(rcv_param, "Proc_Rpt_Acc_ReceivePayment_Archive");
                var Pay_DB = accReportService.GetNewAccDataForReport(pay_param, "Proc_Rpt_Acc_ReceivePayment_Archive");

                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("rpt_acc_sub_receipt", Rcv_DB.Tables[0]);
                subReportDB.Add("rpt_acc_sub_payment", Pay_DB.Tables[0]);

                ReportHelper.PrintWithSubReport("rpt_acc_receipt_payment.rpt", allVouchers.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_acc_receipt_payment());
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        /*
        public ActionResult GenerateRcvPayReport(string office_id, string from_date, string to_date, string acc_level, string qType)
        {
            DataSet allVouchers;
            DataSet Rcv_DB;
            DataSet Pay_DB;
            try     
            {
                if (qType == "1" || qType == "0")
                {
                    office_id = "1";

                }
                var main_param = new object();
                if (SessionHelper.LoginUserOrganizationID == 12 || SessionHelper.LoginUserOrganizationID == 10)
                    main_param = new { office_id = office_id, orgName = ApplicationSettings.OrganiztionName, FromDate = from_date, ToDate = to_date, qtype = (string.IsNullOrEmpty(qType) ? 0 : int.Parse(qType)) };
                else
                    main_param = new { office_id = office_id, orgName = ApplicationSettings.OrganiztionName, FromDate = from_date, ToDate = to_date };
                
                var rcv_param = new { from_date = from_date, to_date = to_date, office_id = office_id, AccLevel = acc_level, And_Condition = " AND t.AccType='R'", AllOffice = qType };
                var pay_param = new { from_date = from_date, to_date = to_date, office_id = office_id, AccLevel = acc_level, And_Condition = " AND t.AccType='P'", AllOffice = qType };
                allVouchers = accReportService.GetDataIncExpReport(main_param);


                //check archive DB starts

                var archiveMapDb = "";
                var archiveMap = archiveDbMappingService.GetMany(p => p.OrgId == LoggedInOrganizationID).FirstOrDefault();
                if (archiveMap != null)
                    archiveMapDb = archiveMap.ArchiveDbName;
                var paramCheckArchive = new { office_id = office_id, from_date = from_date, to_date = to_date, AllOffice = qType, archiveMapDb = archiveMapDb };
                var getData = ultimateReportService.GetDataWithParameter(paramCheckArchive, "CheckAccTrxMasterDataExists");

                if (getData.Tables[0].AsEnumerable().Any())
                {
                    var rcv_param_Archive = new { from_date = from_date, to_date = to_date, office_id = office_id, AccLevel = acc_level, And_Condition = " AND t.AccType='R'", AllOffice = qType, archiveMapDb = archiveMapDb };
                    var pay_param_Archive = new { from_date = from_date, to_date = to_date, office_id = office_id, AccLevel = acc_level, And_Condition = " AND t.AccType='P'", AllOffice = qType, archiveMapDb = archiveMapDb };
                    Rcv_DB = accReportService.GetNewAccDataForReport(rcv_param_Archive, "Proc_Rpt_Acc_ReceivePayment_Archive");
                    Pay_DB = accReportService.GetNewAccDataForReport(pay_param_Archive, "Proc_Rpt_Acc_ReceivePayment_Archive");
                }
                else
                {
                    Rcv_DB = accReportService.GetNewAccDataForReport(rcv_param, "Proc_Rpt_Acc_ReceivePayment");
                    Pay_DB = accReportService.GetNewAccDataForReport(pay_param, "Proc_Rpt_Acc_ReceivePayment");
                }
                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("rpt_acc_sub_receipt", Rcv_DB.Tables[0]);
                subReportDB.Add("rpt_acc_sub_payment", Pay_DB.Tables[0]);
                ReportHelper.PrintWithSubReport("rpt_acc_receipt_payment.rpt", allVouchers.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_acc_receipt_payment());
                return Content(string.Empty);                             
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        */

        #endregion

        #region Events
        // GET: AccRcvPayReport
        public ActionResult IndexByOffice()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = offcdetail.OfficeLevel;
            ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;
            
            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {

                //ViewData["TrxDate"] = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate;
            }
            return View();
        }
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;

            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            var HeadOfficeCode = offcdetail.FirstLevel;
            var Headoffcdetail = officeService.GetByOfficeCode(HeadOfficeCode);
            var Secondoffcdetail = officeService.GetByOfficeCode(offcdetail.SecondLevel);
            var Thirdoffcdetail = officeService.GetByOfficeCode(offcdetail.ThirdLevel);
            var Fourthoffcdetail = officeService.GetByOfficeCode(offcdetail.FourthLevel);

            ViewData["OfficeLevel"] = offcdetail.OfficeLevel;

            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = Headoffcdetail.OfficeID;
                ViewData["SecondLevel"] = "";
                ViewData["ThirdLevel"] = "";
                ViewData["FourthLevel"] = "";
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = Headoffcdetail.OfficeID;
                ViewData["SecondLevel"] = Secondoffcdetail.OfficeID;
                ViewData["ThirdLevel"] = "";
                ViewData["FourthLevel"] = "";
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = Headoffcdetail.OfficeID;
                ViewData["SecondLevel"] = Secondoffcdetail.OfficeID;
                ViewData["ThirdLevel"] = Thirdoffcdetail.OfficeID;
                ViewData["FourthLevel"] = "";
            }
            else
            {
                ViewData["FirstLevel"] = Headoffcdetail.OfficeID;
                ViewData["SecondLevel"] = Secondoffcdetail.OfficeID;
                ViewData["ThirdLevel"] = Thirdoffcdetail.OfficeID;
                ViewData["FourthLevel"] = Fourthoffcdetail.OfficeID;

            }
            var param = new { @OfficeID = SessionHelper.LoggedInEmployee.OfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);
            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            return View();
            //IEnumerable<SelectListItem> items = new SelectList(" ");
            //ViewData["HOList"] = items;
            //ViewData["ZoneList"] = items;
            //ViewData["AreaList"] = items;
            //ViewData["OfficeList"] = items;


            //var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            //ViewData["OfficeLevel"] = offcdetail.OfficeLevel;

            //if (offcdetail.OfficeLevel == 1)
            //{
            //    ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            //    ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            //    ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            //    ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            //}
            //else
            //{
            //    ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            //    ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            //    ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            //    ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            //}


            ////ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            ////ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            ////ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            ////ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            ////ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            //var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            //var allProducts = accReportService.GetLastInitialDate(param);

            //var detail = allProducts.ToString();

            //if (!IsDayInitiated)
            //{

            //   ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            //}
            //else
            //{
            //    ViewData["TrxDate"] = TransactionDate;
            //}
            //return View();
        }
        public ActionResult AdminCostRecPay()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = offcdetail.OfficeLevel;


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
            //ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            //ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            //ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            //ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {

                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate;
            }
            return View();
        }
        // GET: AccRcvPayReport/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: AccRcvPayReport/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: AccRcvPayReport/Create
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
        // GET: AccRcvPayReport/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        // POST: AccRcvPayReport/Edit/5
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
        // GET: AccRcvPayReport/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: AccRcvPayReport/Delete/5
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

        public ActionResult ReceivePaymentArchive()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = offcdetail.OfficeLevel;
            ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {                
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate;
            }
            return View();
        }

        #endregion
    }
}
