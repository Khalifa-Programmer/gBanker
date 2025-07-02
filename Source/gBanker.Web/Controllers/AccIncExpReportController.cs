using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.Reports;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class AccIncExpReportController : BaseController
    {
        #region Variables
        private readonly IAccTrxMasterService accTrxMasterService;
        private readonly IAccTrxDetailService accTrxDetailService;
        private readonly IAccChartService accChartService;
        private readonly IProcessInfoService processInfoService;
        private readonly IAccReportService accReportService;
        private readonly IOfficeService officeService;
        private readonly IArchiveDbMappingService archiveDbMappingService;
        private readonly IUltimateReportService ultimateReportService;
        public AccIncExpReportController(IAccTrxMasterService accTrxMasterService, IAccTrxDetailService accTrxDetailService, IAccChartService accChartService, IProcessInfoService processInfoService, IAccReportService accReportService, IOfficeService officeService, IArchiveDbMappingService archiveDbMappingService, IUltimateReportService ultimateReportService)
        {
            this.accTrxMasterService = accTrxMasterService;
            this.accTrxDetailService = accTrxDetailService;
            this.accChartService = accChartService;
            this.processInfoService = processInfoService;
            this.accReportService = accReportService;
            this.officeService = officeService;
            this.archiveDbMappingService = archiveDbMappingService;
            this.ultimateReportService = ultimateReportService;
        }
        #endregion

        #region Methods
        public JsonResult GetZoneListByOffice(string HO_val)
        {
            //var ofcId = officeService.GetById(Convert.ToInt32(HO_val)).OfficeCode;
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
            //var ho_code = officeService.GetById(Convert.ToInt32(HO_val)).OfficeCode;
            //var zone_code = officeService.GetById(Convert.ToInt32(zone_val)).OfficeCode;
            //var area_code = officeService.GetById(Convert.ToInt32(area_val)).OfficeCode;
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

        //public ActionResult GenerateIncExpReport(string office_id, string from_date, string to_date, string acc_level, string qType)
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


        //        var inc_param = new { office_id = office_id, from_date = from_date, to_date = to_date, AccLevel = acc_level, And_Condition = " AND t.AccType='I'", AllOffice = qType };
        //        var exp_param = new { office_id = office_id, from_date = from_date, to_date = to_date, AccLevel = acc_level, And_Condition = " AND t.AccType='E'", AllOffice = qType };
        //        var allVouchers = accReportService.GetDataIncExpReport(main_param);

        //        var Inc_DB = accReportService.GetNewAccDataForReport(inc_param, "Proc_Rpt_Acc_IncExp");
        //        var Exp_DB = accReportService.GetNewAccDataForReport(exp_param, "Proc_Rpt_Acc_IncExp");

        //        var subReportDB = new Dictionary<string, DataTable>();
        //        subReportDB.Add("rpt_acc_sub_exp", Exp_DB.Tables[0]);
        //        subReportDB.Add("rpt_acc_sub_inc", Inc_DB.Tables[0]);

        //        var reportParam = new Dictionary<string, object>();
        //        reportParam.Add("Parameter_param_orgName", ApplicationSettings.OrganiztionName);
        //        reportParam.Add("FromDate", from_date);
        //        reportParam.Add("ToDate", to_date);
        //        ReportHelper.PrintWithSubReport("rpt_acc_inc_exp.rpt", allVouchers.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_acc_inc_exp());

        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}

        public ActionResult GenerateIncExpReport(string office_id, string from_date, string to_date, string acc_level, string qType, string ddlReportType)
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


                var inc_param = new { office_id = office_id, from_date = from_date, to_date = to_date, AccLevel = acc_level, And_Condition = " AND t.AccType='I'", AllOffice = qType };
                var exp_param = new { office_id = office_id, from_date = from_date, to_date = to_date, AccLevel = acc_level, And_Condition = " AND t.AccType='E'", AllOffice = qType };
                var allVouchers = accReportService.GetDataIncExpReport(main_param);

                var Inc_DB = accReportService.GetNewAccDataForReport(inc_param, "Proc_Rpt_Acc_IncExp");
                var Exp_DB = accReportService.GetNewAccDataForReport(exp_param, "Proc_Rpt_Acc_IncExp");

                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("rpt_acc_sub_exp", Exp_DB.Tables[0]);
                subReportDB.Add("rpt_acc_sub_inc", Inc_DB.Tables[0]);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("Parameter_param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("FromDate", from_date);
                reportParam.Add("ToDate", to_date);

                if (ddlReportType == "pdf")
                    ReportHelper.PrintWithSubReport("rpt_acc_inc_exp.rpt", allVouchers.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_acc_inc_exp());

                if (ddlReportType == "xls")
                    ReportHelper.ExportExcelWithSubReport("rpt_acc_inc_exp.rpt", allVouchers.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_acc_inc_exp());

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateIncExpReportArchive(string office_id, string from_date, string to_date, string acc_level, string qType)
        {
            DataSet allVouchers;
            DataSet Inc_DB;
            DataSet Exp_DB;

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

                //check archive DB starts

                var archiveMapDb = "";
                var archiveMap = archiveDbMappingService.GetMany(p => p.OrgId == LoggedInOrganizationID).FirstOrDefault();
                if (archiveMap != null)
                    archiveMapDb = archiveMap.ArchiveDbName;

                var inc_param = new { office_id = office_id, from_date = from_date, to_date = to_date, AccLevel = acc_level, And_Condition = " AND t.AccType='I'", AllOffice = qType, archiveMapDb = archiveMapDb };
                var exp_param = new { office_id = office_id, from_date = from_date, to_date = to_date, AccLevel = acc_level, And_Condition = " AND t.AccType='E'", AllOffice = qType, archiveMapDb = archiveMapDb };
                allVouchers = accReportService.GetDataIncExpReport(main_param);

                Inc_DB = accReportService.GetNewAccDataForReport(inc_param, "Proc_Rpt_Acc_IncExp_Archive");
                Exp_DB = accReportService.GetNewAccDataForReport(exp_param, "Proc_Rpt_Acc_IncExp_Archive");

                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("rpt_acc_sub_exp", Exp_DB.Tables[0]);
                subReportDB.Add("rpt_acc_sub_inc", Inc_DB.Tables[0]);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("Parameter_param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("FromDate", from_date);
                reportParam.Add("ToDate", to_date);
                ReportHelper.PrintWithSubReport("rpt_acc_inc_exp.rpt", allVouchers.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_acc_inc_exp());

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        /*
        public ActionResult GenerateIncExpReport(string office_id, string from_date, string to_date, string acc_level, string qType)
        {
            DataSet allVouchers;
            DataSet Inc_DB;
            DataSet Exp_DB;

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


                var inc_param = new { office_id = office_id, from_date = from_date, to_date = to_date, AccLevel = acc_level, And_Condition = " AND t.AccType='I'", AllOffice = qType };
                var exp_param = new { office_id = office_id, from_date = from_date, to_date = to_date, AccLevel = acc_level, And_Condition = " AND t.AccType='E'", AllOffice = qType };
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
                    var inc_param_Archive = new { office_id = office_id, from_date = from_date, to_date = to_date, AccLevel = acc_level, And_Condition = " AND t.AccType='I'", AllOffice = qType, archiveMapDb = archiveMapDb };
                    var exp_param_Archive = new { office_id = office_id, from_date = from_date, to_date = to_date, AccLevel = acc_level, And_Condition = " AND t.AccType='E'", AllOffice = qType, archiveMapDb = archiveMapDb };
                    Inc_DB = accReportService.GetNewAccDataForReport(inc_param_Archive, "Proc_Rpt_Acc_IncExp_Archive");
                    Exp_DB = accReportService.GetNewAccDataForReport(exp_param_Archive, "Proc_Rpt_Acc_IncExp_Archive");
                }
                else
                {
                    Inc_DB = accReportService.GetNewAccDataForReport(inc_param, "Proc_Rpt_Acc_IncExp");
                    Exp_DB = accReportService.GetNewAccDataForReport(exp_param, "Proc_Rpt_Acc_IncExp");
                }
                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("rpt_acc_sub_exp", Exp_DB.Tables[0]);
                subReportDB.Add("rpt_acc_sub_inc", Inc_DB.Tables[0]);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("Parameter_param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("FromDate", from_date);
                reportParam.Add("ToDate", to_date);
                ReportHelper.PrintWithSubReport("rpt_acc_inc_exp.rpt", allVouchers.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_acc_inc_exp());
                
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
        // GET: AccIncExpReport
       
            public ActionResult Index()
        {


            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;

            //var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoggedInEmployee.OfficeID));
            //var HeadOfficeCode = offcdetail.FirstLevel;
            //var Headoffcdetail = officeService.GetByOfficeCode(HeadOfficeCode);
            //var Secondoffcdetail = officeService.GetByOfficeCode(offcdetail.SecondLevel);
            //var Thirdoffcdetail = officeService.GetByOfficeCode(offcdetail.ThirdLevel);
            //var Fourthoffcdetail = officeService.GetByOfficeCode(offcdetail.FourthLevel);

            //ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];

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

        //    //ViewData["TrxDate"] = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
        //    ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
        //}
        //else
        //{
        //    ViewData["TrxDate"] = TransactionDate;
        //}
        //return View();
    


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
        // GET: AccIncExpReport/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccIncExpReport/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccIncExpReport/Create
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

        // GET: AccIncExpReport/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccIncExpReport/Edit/5
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

        // GET: AccIncExpReport/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccIncExpReport/Delete/5
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

        public ActionResult IncomeExpenditureArchive()
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
        #endregion
    }
}
