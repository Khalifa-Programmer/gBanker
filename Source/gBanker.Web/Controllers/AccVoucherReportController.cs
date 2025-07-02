using gBanker.Data.CodeFirstMigration;
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
    public class AccVoucherReportController : BaseController
    {
        // GET: AccVoucherReport
        #region Variables
        private readonly IAccTrxMasterService accTrxMasterService;
        private readonly IAccTrxDetailService accTrxDetailService;
        private readonly IAccChartService accChartService;
        private readonly IAccLastVoucherService accLastVoucherService;
        private readonly IProcessInfoService processInfoService;
        private readonly IAccReportService accReportService;
        private readonly IWeeklyReportService weeklyReportService;
        //private readonly IPurposeService purposeService;
        private readonly IAccReconcileService reConcileService;
        private readonly IUltimateReportService ultimateRepostService;
        private readonly IOfficeService officeService;
        public AccVoucherReportController(IAccTrxMasterService accTrxMasterService, IAccTrxDetailService accTrxDetailService, IAccChartService accChartService, IAccLastVoucherService accLastVoucherService, IProcessInfoService processInfoService, IAccReportService accReportService, IWeeklyReportService weeklyReportService,IAccReconcileService reConcileService,IUltimateReportService ultimateRepostService,IOfficeService officeService)
        {
            this.accTrxMasterService = accTrxMasterService;
            this.accTrxDetailService = accTrxDetailService;
            this.accChartService = accChartService;
            this.accLastVoucherService = accLastVoucherService;
            this.processInfoService = processInfoService;
            this.accReportService = accReportService;
            this.weeklyReportService = weeklyReportService;
            this.reConcileService = reConcileService;
            this.ultimateRepostService = ultimateRepostService;
            this.officeService = officeService;
        }
        #endregion

        #region Methods
        private void MapDropDownList(AccVoucherEntryViewModel model)
        {
            //var allCategory = accCategoryService.GetAll();
            //var viewCat = allCategory.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.CategoryID.ToString(),
            //    Text = x.CategoryName.ToString()
            //});
            //var cat_items = new List<SelectListItem>();
            //cat_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //cat_items.AddRange(viewCat);
            //model.CategoryList = cat_items;

            var type_item = new List<SelectListItem>();
            type_item.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            type_item.Add(new SelectListItem() { Text = "Cash (Debit)", Value = "CAD" });
            type_item.Add(new SelectListItem() { Text = "Cash (Credit)", Value = "CAC" });
            //type_item.Add(new SelectListItem() { Text = "Bank", Value = "Ba" });
            type_item.Add(new SelectListItem() { Text = "Bank(Debit)", Value = "BDr" });
            type_item.Add(new SelectListItem() { Text = "Bank(Credit)", Value = "BCr" });
            //type_item.Add(new SelectListItem() { Text = "Bank(Cheque)", Value = "Ba" });
            type_item.Add(new SelectListItem() { Text = "Bank(Cash)", Value = "Bc" });
            type_item.Add(new SelectListItem() { Text = "BankToBank", Value = "BB" });
            type_item.Add(new SelectListItem() { Text = "Journal", Value = "Jr" });
            model.VoucherTypeList = type_item;

            var blnk_items = new List<SelectListItem>();
            //blnk_items.Add(new SelectListItem() { Text = "", Value = "0", Selected = true });            
            model.VoucherNoList = blnk_items;
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetVoucherList(string voucherType, string trxDt, string offcId)
        {
            

            try
            {
                List<AccTrxMaster> List_AccTrxMaster = new List<AccTrxMaster>();
                var param = new { VoucherType = voucherType, TrxDate = trxDt, OfficeID = offcId };
                var getVoucher = accReportService.GetAccVoucherByVoucherType(param);

                List_AccTrxMaster = getVoucher.Tables[0].AsEnumerable()
                .Select(row => new AccTrxMaster
                {
                    TrxMasterID = row.Field<long>("TrxMasterID"),
                    VoucherNo = row.Field<string>("VoucherNo")
                    
                }).ToList();

                return Json(List_AccTrxMaster, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }


            //var getVoucher = accTrxMasterService.GetByTrxDt_VType(voucherType, Convert.ToDateTime(trxDt), Convert.ToInt32(offcId));
            //var viewVoucher = getVoucher.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.TrxMasterID.ToString(),
            //    Text = x.VoucherNo.ToString()
            //});
            //var voucher_items = new List<SelectListItem>();
            //if (viewVoucher.ToList().Count > 0)
            //{
            //    voucher_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //}
            //voucher_items.AddRange(viewVoucher);
            //return Json(voucher_items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerateVoucherReport(string voucher_id)
        {
            try
            {

                var param = new { voucher_id = voucher_id };
                DataSet allVouchers;
                if (LoggedInOrganizationID==54)
                {
                    allVouchers = accReportService.GetDataVoucherBuro(param);
                }
                else
                {
                    allVouchers = accReportService.GetDataVoucher(param);
                }
                
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_OrgName", ApplicationSettings.OrganiztionName);
                if (LoggedInOrganizationID == 150)
                {
                    ReportHelper.PrintReport("rpt_acc_voucherAUS.rpt", allVouchers.Tables[0], reportParam);
                }
                ReportHelper.PrintReport("rpt_acc_voucher.rpt", allVouchers.Tables[0], reportParam);               
                return Content(string.Empty);

                // return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateAllVoucherReport(string officeId, string voucher_type, string trxDt, string trxDtTo)
        {
            try
            {

                var param = new { voucher_type = voucher_type, office_id = officeId, trx_date = trxDt, trx_dateTo = trxDtTo };
                DataSet allVouchers;
                if (LoggedInOrganizationID==54)
                {
                    allVouchers = accReportService.GetDataVoucherAllBURO(param);
                }
                else
                {
                    allVouchers = accReportService.GetDataVoucherAll(param);
                }
                
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_OrgName", ApplicationSettings.OrganiztionName);

                if(LoggedInOrganizationID == 150)
                {
                if (voucher_type == "CAC")
                        ReportHelper.PrintReport("rpt_acc_Debitvoucher_all_Amount_New_AUS.rpt", allVouchers.Tables[0], reportParam);
                    else if (voucher_type == "CAD")
                        ReportHelper.PrintReport("rpt_acc_Debitvoucher_all_Amount_New_AUS.rpt", allVouchers.Tables[0], reportParam);
                    else if (voucher_type == "BDr")
                        ReportHelper.PrintReport("rpt_acc_voucher_all_Amount_AUS.rpt", allVouchers.Tables[0], reportParam);
                    else if (voucher_type == "BCr")
                        ReportHelper.PrintReport("rpt_acc_voucher_all_Amount_AUS.rpt", allVouchers.Tables[0], reportParam);
                    else if (voucher_type == "Ba")
                        ReportHelper.PrintReport("rpt_acc_voucher_allDateWise_AUS.rpt", allVouchers.Tables[0], reportParam);
                    else if (voucher_type == "Bc")
                        ReportHelper.PrintReport("rpt_acc_voucher_allDateWise_AUS.rpt", allVouchers.Tables[0], reportParam);
                    else if (voucher_type == "Jr")
                        ReportHelper.PrintReport("rpt_acc_voucher_allDateWise_AUS.rpt", allVouchers.Tables[0], reportParam);
                    else if (voucher_type == "BB")
                        ReportHelper.PrintReport("rpt_acc_voucher_allDateWise_AUS.rpt", allVouchers.Tables[0], reportParam);
                }
                else
                {
                    if (voucher_type == "CAC")
                    ReportHelper.PrintReport("rpt_acc_Debitvoucher_all_Amount_New.rpt", allVouchers.Tables[0], reportParam);
                else if (voucher_type == "CAD")
                    ReportHelper.PrintReport("rpt_acc_Debitvoucher_all_Amount_New.rpt", allVouchers.Tables[0], reportParam);
                else if (voucher_type == "BDr")
                        //ReportHelper.PrintReport("rpt_acc_voucher_all_Amount.rpt", allVouchers.Tables[0], reportParam);
                        ReportHelper.PrintReport("rpt_acc_voucher_allDateVoucherTypeWise.rpt", allVouchers.Tables[0], reportParam);
                    else if (voucher_type == "BCr")
                        ReportHelper.PrintReport("rpt_acc_voucher_allDateVoucherTypeWise.rpt", allVouchers.Tables[0], reportParam);
                    //ReportHelper.PrintReport("rpt_acc_voucher_all_Amount.rpt", allVouchers.Tables[0], reportParam);
                    else if (voucher_type == "Ba")
                    ReportHelper.PrintReport("rpt_acc_voucher_allDateWise.rpt", allVouchers.Tables[0], reportParam);
                else if (voucher_type == "Bc")
                    ReportHelper.PrintReport("rpt_acc_voucher_allDateWise.rpt", allVouchers.Tables[0], reportParam);
                else if (voucher_type == "Jr")
                    ReportHelper.PrintReport("rpt_acc_voucher_allDateWise.rpt", allVouchers.Tables[0], reportParam);
                else if (voucher_type == "BB")
                    ReportHelper.PrintReport("rpt_acc_voucher_allDateWise.rpt", allVouchers.Tables[0], reportParam);
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
        public ActionResult Index()
        {
            var model = new AccVoucherEntryViewModel();
            MapDropDownList(model);

            //model.TrxDate = processInfoService.GetByOfficeId(Convert.ToInt32(SessionHelper.LoginUserOfficeID)).BusinessDate;
            //model.TrxDate = processInfoService.GetInitialDtByOfficeId(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            //model.TrxDate = SessionHelper.TransactionDate;
            //if (IsDayInitiated)
            //    model.TrxDate = TransactionDate;

            //ViewData["TransactionDate"] = model.TrxDate.ToString("dd-MMM-yyyy");

            model.VoucherType = "0";
            //model.OfficeID = 6;
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            //Session[sessionName] = CreateTable();

            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                if (LoggedInOrganizationID != 54)
                {
                    ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
                    ViewData["TrxdateTo"] = TransactionDate.ToString("dd-MMM-yyyy");
                    var param1 = new { OfficeID = LoginUserOfficeID, BusinessDate = TransactionDate, OrgID = SessionHelper.LoginUserOrganizationID };
                    var LoanInstallMent = weeklyReportService.CheckAutoVoucher(param1);
                    if (LoanInstallMent.Tables[0].Rows.Count == 0)
                    {
                        var param = new { OfficeID = LoginUserOfficeID, BusinessDate = TransactionDate, OrgID = SessionHelper.LoginUserOrganizationID };
                        weeklyReportService.AutoVoucherCollectionProcess(param);
                    }
                }
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
                ViewData["TrxdateTo"] = TransactionDate.ToString("dd-MMM-yyyy");
                //var param1 = new { OfficeID = LoginUserOfficeID, BusinessDate = TransactionDate, OrgID = SessionHelper.LoginUserOrganizationID };
                //var LoanInstallMent = weeklyReportService.CheckAutoVoucher(param1);
                //if (LoanInstallMent.Tables[0].Rows.Count == 0)
                //{
                //    var param = new { OfficeID = LoginUserOfficeID, BusinessDate = TransactionDate, OrgID = SessionHelper.LoginUserOrganizationID };
                //    weeklyReportService.AutoVoucherCollectionProcess(param);
                //}
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
                ViewData["TrxdateTo"] = VDate.ToString("dd-MMM-yyyy");
            }
            return View(model);
        }

       // GET: AccVoucherReport/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccVoucherReport/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccVoucherReport/Create
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

        // GET: AccVoucherReport/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccVoucherReport/Edit/5
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

        // GET: AccVoucherReport/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccVoucherReport/Delete/5
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
