using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace gBanker.Web.Controllers
{
    public class AccLedgerReportController : BaseController
    {
        #region Variables
        private readonly IAccTrxMasterService accTrxMasterService;
        private readonly IAccTrxDetailService accTrxDetailService;
        private readonly IAccChartService accChartService;
        private readonly IAccLastVoucherService accLastVoucherService;
        private readonly IProcessInfoService processInfoService;
        private readonly IAccReportService accReportService;
        private readonly IOfficeService officeService;
        private readonly IArchiveDbMappingService archiveDbMappingService;
        public AccLedgerReportController(IAccTrxMasterService accTrxMasterService, IAccTrxDetailService accTrxDetailService, IAccChartService accChartService, IAccLastVoucherService accLastVoucherService, IProcessInfoService processInfoService, IAccReportService accReportService, IOfficeService officeService, IArchiveDbMappingService archiveDbMappingService)
        {
            this.accTrxMasterService = accTrxMasterService;
            this.accTrxDetailService = accTrxDetailService;
            this.accChartService = accChartService;
            this.accLastVoucherService = accLastVoucherService;
            this.processInfoService = processInfoService;
            this.accReportService = accReportService;
            this.officeService = officeService;
            this.archiveDbMappingService = archiveDbMappingService;
        }
        #endregion

        #region Methods
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
        public JsonResult GetAccListSearch(string acc_level, string term = "")
        {
            List<AccChartViewModel> List_AccChartViewModel = new List<AccChartViewModel>();
            var param = new { AccLevel = acc_level, OfficeID = LoginUserOfficeID, searchTerm = term, mode = 1 };
            var allSavingsummary = accReportService.GetAccChartListSearch(param);
            if (allSavingsummary.Tables[0].Rows.Count > 0)
            {
                List_AccChartViewModel = allSavingsummary.Tables[0].AsEnumerable()
               .Select(row => new AccChartViewModel
               {
                   AccID = row.Field<int>("AccID"),
                   AccCode = row.Field<string>("AccCode") + " - " + row.Field<string>("AccName")
               }).ToList();
            }
            return Json(List_AccChartViewModel, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetAccListSearch(string acc_level, string term = "")
        //{

        //    List<AccChartViewModel> List_AccChartViewModel = new List<AccChartViewModel>();
        //    var param = new { AccLevel = acc_level, OfficeID = LoginUserOfficeID, searchTerm = term, mode = 1 };
        //    var allSavingsummary = accReportService.GetAccChartListSearch(param);
        //    if (allSavingsummary.Tables[0].Rows.Count > 0)
        //    {
        //        List_AccChartViewModel = allSavingsummary.Tables[0].AsEnumerable()
        //       .Select(row => new AccChartViewModel
        //       {
        //           AccID = row.Field<int>("AccID"),
        //           AccCode = row.Field<string>("AccCode"),
        //           AccName = row.Field<string>("AccName")
        //       }).ToList();
        //    }

        //    var viewCode = List_AccChartViewModel.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.AccID.ToString(),
        //        Text = x.AccCode.ToString() + " - " + x.AccName.ToString()
        //    });
        //    var acc_items = new List<SelectListItem>();
        //    acc_items.AddRange(viewCode);

        //    return Json(acc_items, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetAccID(string acc_level, string accCode)
        {

            //var accCodeList = accChartService.GetAll().Where(c => c.AccLevel == Convert.ToInt32(acc_level));


            string[] acc = accCode.Split('-');

            List<AccChartViewModel> List_AccChartViewModel = new List<AccChartViewModel>();
            var param = new { AccLevel = acc_level, OfficeID = LoginUserOfficeID, searchTerm = acc[0], mode = 0 };
            var allSavingsummary = accReportService.GetAccChartListSearch(param);
            if (allSavingsummary.Tables[0].Rows.Count > 0)
            {
                List_AccChartViewModel = allSavingsummary.Tables[0].AsEnumerable()
               .Select(row => new AccChartViewModel
               {
                   AccID = row.Field<int>("AccID"),
                   AccCode = row.Field<string>("AccCode"),
                   AccName = row.Field<string>("AccName")
               }).ToList();
            }

            return Json(List_AccChartViewModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHOListByOffice()
        {
            //var First_Level = officeService.GetByOfficeOrgID(Convert.ToInt32(SessionHelper.LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID));
            var OfficeList = officeService.GetAll().Where(c => c.OfficeID == LoginUserOfficeID);
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
        public JsonResult GetAccList(string acc_level)
        {

            //var accCodeList = accChartService.GetAll().Where(c => c.AccLevel == Convert.ToInt32(acc_level));
            List<AccChartViewModel> List_AccChartViewModel = new List<AccChartViewModel>();
            var param = new { AccLevel = acc_level, OfficeID=LoginUserOfficeID };
            var allSavingsummary = accReportService.GetAccChartList(param);
            if (allSavingsummary.Tables[0].Rows.Count > 0)
            {
                List_AccChartViewModel = allSavingsummary.Tables[0].AsEnumerable()
               .Select(row => new AccChartViewModel
               {
                   AccID = row.Field<int>("AccID"),
                   AccCode = row.Field<string>("AccCode"),
                   AccName = row.Field<string>("AccName")
               }).ToList();
            }
            // var accCodeList = accChartService.getm().Where(c => c.AccLevel == Convert.ToInt32(acc_level));
            var viewCode = List_AccChartViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.AccID.ToString(),
                Text = x.AccCode.ToString() + " - " + x.AccName.ToString()
            });
            var acc_items = new List<SelectListItem>();
            if (viewCode.ToList().Count > 0)
            {
                acc_items.Add(new SelectListItem() { Text = "Select None", Value = "0", Selected = true });
            }
            acc_items.AddRange(viewCode);

          return Json(acc_items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerateLedgerReport(string office_id, string from_date, string to_date, string acc_level, string acc_id)
        {
            try
            {
                if (acc_id == "0" || acc_id == "")
                {
                    var param = new { office_id = office_id, from_date = from_date, to_date = to_date, AccLevel = acc_level };
                    var allVouchers = accReportService.GetACCLedgerDataLedgerReport(param);
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("from_date", from_date);
                    reportParam.Add("to_date", to_date);
                    ReportHelper.PrintReport("rpt_acc_ledger.rpt", allVouchers.Tables[0], reportParam);
                }
                else
                {
                    var param = new { office_id = office_id, from_date = from_date, to_date = to_date, AccLevel = acc_level, AccId = acc_id };
                    var allVouchers = accReportService.GetACCDataLedgerCodeWiseReport(param);
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("from_date", from_date);
                    reportParam.Add("to_date", to_date);
                    ReportHelper.PrintReport("rpt_acc_ledger.rpt", allVouchers.Tables[0], reportParam);
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion
        public ActionResult GenerateLedgerReportExport(string office_id, string from_date, string to_date, string acc_level, string acc_id)
        {
            try
            {
                if (acc_id == "0")
                {
                    var param = new { office_id = office_id, from_date = from_date, to_date = to_date, AccLevel = acc_level };
                    var allVouchers = accReportService.GetDataLedgerReport(param);
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("from_date", from_date);
                    reportParam.Add("to_date", to_date);
                    ReportHelper.ExportExcelReport("rpt_acc_ledger.rpt", allVouchers.Tables[0], reportParam);
                    return Content(string.Empty);

                    //  var param = new { office_id = office_id, from_date = from_date, to_date = to_date, AccLevel = acc_level };
                    //  //var allVouchers = accReportService.GetDataLedgerReport(param);
                    //  GridView gv = new GridView();
                    //  //var allRepaymentSchedule = creditscoreService.GetAll();
                    //  var allRepaymentSchedule = accReportService.GetDataLedgerReport(param);
                    //  var detail = allRepaymentSchedule.Tables[0];
                    //  gv.DataSource = detail;
                    //  gv.DataBind();
                    //  Response.ClearContent();
                    //  Response.Buffer = true;
                    //  Response.AddHeader("content-disposition", "attachment; filename=GenerateLedgerReport.xls");
                    //  Response.ContentType = "application/ms-excel";
                    //  Response.Charset = "";
                    //  StringWriter sw = new StringWriter();
                    //  HtmlTextWriter htw = new HtmlTextWriter(sw);
                    //  gv.RenderControl(htw);
                    //  Response.Output.Write(sw.ToString());
                    //  Response.Flush();
                    //  Response.End();
                    //  ReportHelper.ExportExcelReport("Payroll/rptMothlySalaryReport.rpt", salaryData.Tables[0], new Dictionary<string, object>(), subReportDB, new rptMothlySalaryReport());
                    //  return RedirectToAction("DailySavingCollectionReport");
                }
                else
                {
                    var param = new { office_id = office_id, from_date = from_date, to_date = to_date, AccLevel = acc_level, AccId = acc_id };
                    var allVouchers = accReportService.GetDataLedgerCodeWiseReport(param);
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("from_date", from_date);
                    reportParam.Add("to_date", to_date);
                    ReportHelper.ExportExcelReport("rpt_acc_ledger.rpt", allVouchers.Tables[0], reportParam);
                    return Content(string.Empty);

                    //   var param = new { office_id = office_id, from_date = from_date, to_date = to_date, AccLevel = acc_level, AccId = acc_id };
                    //   //var allVouchers = accReportService.GetDataLedgerCodeWiseReport(param);
                    //   GridView gv = new GridView();
                    //   //var allRepaymentSchedule = creditscoreService.GetAll();
                    //   var allRepaymentSchedule = accReportService.GetDataLedgerCodeWiseReport(param);
                    //   var detail = allRepaymentSchedule.Tables[0];
                    //   gv.DataSource = detail;
                    //   gv.DataBind();
                    //   Response.ClearContent();
                    //   Response.Buffer = true;
                    //   Response.AddHeader("content-disposition", "attachment; filename=GenerateLedgerReport.xls");
                    //   Response.ContentType = "application/ms-excel";
                    //   Response.Charset = "";
                    //   StringWriter sw = new StringWriter();
                    //   HtmlTextWriter htw = new HtmlTextWriter(sw);
                    //   gv.RenderControl(htw);
                    //   Response.Output.Write(sw.ToString());
                    //   Response.Flush();
                    //   Response.End();

                    //   return RedirectToAction("DailySavingCollectionReport");
                }

                    //return Content(string.Empty);

                }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #region Events
        // GET: AccTrialBalance
        public ActionResult Index()
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

        #endregion

        #region GL Archive

        public ActionResult GLArchive()
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
            return View();
        }


        public ActionResult GenerateLedgerReportArchive(string office_id, string from_date, string to_date, string acc_level, string acc_id)
        {
            try
            {
                var archiveMapDb = "";
                var archiveMap = archiveDbMappingService.GetMany(p => p.OrgId == LoggedInOrganizationID).FirstOrDefault();
                if (archiveMap != null)
                    archiveMapDb = archiveMap.ArchiveDbName;

                if (acc_id == "0" || acc_id == "null")
                {
                    var param = new { office_id = office_id, from_date = from_date, to_date = to_date, AccLevel = acc_level, archiveMapDb = archiveMapDb };
                    var allVouchers = accReportService.GetDataLedgerReportArchive(param);
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("from_date", from_date);
                    reportParam.Add("to_date", to_date);
                    ReportHelper.PrintReport("rpt_acc_ledger.rpt", allVouchers.Tables[0], reportParam);
                }
                else
                {
                    var param = new { office_id = office_id, from_date = from_date, to_date = to_date, AccLevel = acc_level, AccId = acc_id, archiveMapDb = archiveMapDb };
                    var allVouchers = accReportService.GetDataLedgerCodeWiseReportArchive(param);
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("from_date", from_date);
                    reportParam.Add("to_date", to_date);
                    ReportHelper.PrintReport("rpt_acc_ledger.rpt", allVouchers.Tables[0], reportParam);
                }

                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion        
    }
}
