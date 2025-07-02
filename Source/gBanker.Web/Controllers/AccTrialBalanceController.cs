using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class AccTrialBalanceController : BaseController
    {
        #region Variables
        private readonly IAccTrxMasterService accTrxMasterService;
        private readonly IAccTrxDetailService accTrxDetailService;
        private readonly IAccChartService accChartService;
        private readonly IAccLastVoucherService accLastVoucherService;
        private readonly IProcessInfoService processInfoService;
        private readonly IAccReportService accReportService;
        private readonly IOfficeService officeService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IArchiveDbMappingService archiveDbMappingService;
        public AccTrialBalanceController(IAccTrxMasterService accTrxMasterService, IAccTrxDetailService accTrxDetailService, IAccChartService accChartService, IAccLastVoucherService accLastVoucherService, IProcessInfoService processInfoService, IAccReportService accReportService, IOfficeService officeService, IUltimateReportService ultimateReportService, IArchiveDbMappingService archiveDbMappingService)
        {
            this.accTrxMasterService = accTrxMasterService;
            this.accTrxDetailService = accTrxDetailService;
            this.accChartService = accChartService;
            this.accLastVoucherService = accLastVoucherService;
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
            var area_Code = "";
            var office_items = new List<SelectListItem>();
            if (area_val == "0" || area_val == null)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                return Json(office_items, JsonRequestBehavior.AllowGet);
            }
            if (area_val != "0" || area_val != null)
            {
                area_Code = officeService.GetById(Convert.ToInt32(area_val == null ? "0" : area_val)).OfficeCode;
                var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 4 && c.FirstLevel == ho_code && c.SecondLevel == zone_code && c.ThirdLevel == area_Code && c.OrgID == Convert.ToInt32(LoggedInOrganizationID));
                var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.OfficeID.ToString(),
                    Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
                });

                if (viewOffice.ToList().Count > 0)
                {
                    office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                }
                office_items.AddRange(viewOffice);
            }
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GenerateAdminCosrTrialBalanceReport(string office_id, string from_date, string to_date, string acc_level, string qType)
        {
            try
            {

                var param = new { from_date = from_date, to_date = to_date, office_id = office_id, AccLevel = acc_level, AllOffice = qType };
                var allVouchers = accReportService.GetDataAdminCostTrialBalance(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("from_date", from_date);
                reportParam.Add("to_date", to_date);
                if (LoggedInOrganizationID == 5)
                {
                    ReportHelper.PrintReport("rpt_acc_trial_balance_AdminCost_JCF.rpt", allVouchers.Tables[0], reportParam);
                }

                ReportHelper.PrintReport("rpt_acc_trial_balance_AdminCost.rpt", allVouchers.Tables[0], reportParam);
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateAdminCosrTrialBalanceReport_5B(string office_id, string from_date, string to_date, string acc_level, string qType)
        {
            try
            {

                var param = new { Org = LoggedInOrganizationID, office_id = office_id, Date = to_date, qType = qType };
                var allVouchers = accReportService.GetDataAdminCostTrialBalance5_B(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("POMIS_5B_JCF.rpt", allVouchers.Tables[0], reportParam);
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateTrialBalanceReport(string office_id, string from_date, string to_date, string acc_level, string qType)
        {
            try
            {
                if (qType == "1" || qType == "0")
                {
                    office_id = "1";
                }

                var param = new { from_date = from_date, to_date = to_date, office_id = office_id, AccLevel = acc_level, AllOffice = qType };
                var allVouchers = accReportService.GetDataTrialBalance(param);

                var params_ = new { orgId = LoggedInOrganizationID };
                var orgInfo = ultimateReportService.GetDataWithParameter(params_, "GetOrganizationInfo");
                var orgName = orgInfo.Tables[0].Rows[0]["OrganizationName"];
                var OrgAddress = orgInfo.Tables[0].Rows[0]["OrgAddress"];

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("param_orgAddress", OrgAddress);
                reportParam.Add("from_date", from_date);
                reportParam.Add("to_date", to_date);
                ReportHelper.PrintReport("rpt_acc_trial_balance.rpt", allVouchers.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateTrialBalanceReportArchive(string office_id, string from_date, string to_date, string acc_level, string qType)
        {
            try
            {
                if (qType == "1" || qType == "0")
                {
                    office_id = "1";
                }

                var archiveMapDb = "";
                var archiveMap = archiveDbMappingService.GetMany(p => p.OrgId == LoggedInOrganizationID).FirstOrDefault();
                if (archiveMap != null)
                    archiveMapDb = archiveMap.ArchiveDbName;

                var param = new { from_date = from_date, to_date = to_date, office_id = office_id, AccLevel = acc_level, AllOffice = qType, archiveMapDb = archiveMapDb };
                var allVouchers = accReportService.GetDataTrialBalanceArchive(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("from_date", from_date);
                reportParam.Add("to_date", to_date);
                ReportHelper.PrintReport("rpt_acc_trial_balance.rpt", allVouchers.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        /*
        public ActionResult GenerateTrialBalanceReport(string office_id, string from_date, string to_date, string acc_level, string qType)
        {
            DataSet allVouchers;
            try
            {
                if (qType == "1" || qType == "0")
                {
                    office_id = "1";
                }
                var archiveMapDb = "";
                var param = new { from_date = from_date, to_date = to_date, office_id = office_id, AccLevel = acc_level, AllOffice = qType };

                var archiveMap = archiveDbMappingService.GetMany(p => p.OrgId == LoggedInOrganizationID).FirstOrDefault();
                if (archiveMap != null)
                    archiveMapDb = archiveMap.ArchiveDbName;
                var paramCheckArchive = new { from_date = from_date, to_date = to_date, archiveMapDb = archiveMapDb };
                var getData = ultimateReportService.GetDataWithParameter(paramCheckArchive, "CheckAccTrxMasterDataExists");
                
                 if (getData.Tables[0].AsEnumerable().Any())
                {
                    var paramArchive = new { from_date = from_date, to_date = to_date, office_id = office_id, AccLevel = acc_level, AllOffice = qType, archiveMapDb = archiveMapDb };
                    allVouchers = accReportService.GetDataTrialBalanceArchive(paramArchive);
                }
                else
                {
                    allVouchers = accReportService.GetDataTrialBalance(param);
                }                
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("from_date", from_date);
                reportParam.Add("to_date", to_date);
                ReportHelper.PrintReport("rpt_acc_trial_balance.rpt", allVouchers.Tables[0], reportParam);
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
        // GET: AccTrialBalance
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;

            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoggedInEmployee.OfficeID));
            var HeadOfficeCode = offcdetail.FirstLevel;
            var Headoffcdetail = officeService.GetByOfficeCode(HeadOfficeCode);
            var Secondoffcdetail = officeService.GetByOfficeCode(offcdetail.SecondLevel);
            var Thirdoffcdetail = officeService.GetByOfficeCode(offcdetail.ThirdLevel);
            var Fourthoffcdetail = officeService.GetByOfficeCode(offcdetail.FourthLevel);

            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];

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

        public ActionResult AdminCostTrialBalance()
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
        public ActionResult AdminCostTrialBalance_5B()
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

        // GET: AccTrialBalance/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccTrialBalance/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccTrialBalance/Create
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

        // GET: AccTrialBalance/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccTrialBalance/Edit/5
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

        // GET: AccTrialBalance/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccTrialBalance/Delete/5
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

        public ActionResult TrialBalanceArchive()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;

            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoggedInEmployee.OfficeID));
            var HeadOfficeCode = offcdetail.FirstLevel;
            var Headoffcdetail = officeService.GetByOfficeCode(HeadOfficeCode);
            var Secondoffcdetail = officeService.GetByOfficeCode(offcdetail.SecondLevel);
            var Thirdoffcdetail = officeService.GetByOfficeCode(offcdetail.ThirdLevel);
            var Fourthoffcdetail = officeService.GetByOfficeCode(offcdetail.FourthLevel);

            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];

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
        #endregion
    }
}
