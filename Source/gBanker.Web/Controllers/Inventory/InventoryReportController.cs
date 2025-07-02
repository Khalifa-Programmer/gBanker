using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
using gBanker.Service;
using gBanker.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace gBanker.Web.Controllers.Inventory
{
    public class InventoryReportController : BaseController
    {
        #region Variables
        private readonly IInvWarehouseService iInvWarehouseService;
        private readonly IInvTrxMasterService iInvTrxMasterService;
        private readonly IInvTrxDetailService iInvTrxDetailService;
        private readonly IEmployeeService iEmployeeService;
        private readonly IOfficeService iOfficeService;
        public InventoryReportController(IInvWarehouseService iInvWarehouseService
            , IInvTrxMasterService iInvTrxMasterService
            , IInvTrxDetailService iInvTrxDetailService
            , IEmployeeService iEmployeeService
            , IOfficeService iOfficeService
            )
        {
            this.iInvWarehouseService = iInvWarehouseService;
            this.iInvTrxMasterService = iInvTrxMasterService;
            this.iInvTrxDetailService = iInvTrxDetailService;
            this.iEmployeeService = iEmployeeService;
            this.iOfficeService = iOfficeService;
        }
        #endregion Variables
        // GET: InventoryReport
        public ActionResult Index()
        {
            if (LoginUserOfficeID.HasValue)
            {
                var off = iOfficeService.GetById(LoginUserOfficeID.Value);
                ViewBag.OfficeLevel = off.OfficeLevel;
                return View();
            }
            else
                return RedirectToAction("index", "Home");

        }

        public ActionResult Voucher()
        {
            if (!DateTime.MinValue.Equals(SessionHelper.TransactionDate))
                ViewBag.TransactionDate = SessionHelper.TransactionDate.ToString("dd-MMM-yyyy");
            else
                ViewBag.TransactionDate = DateTime.Now.ToString("dd-MMM-yyyy");
            return View();
        }

        #region service
        public JsonResult VoucherPosting(string date)
        {
            string msg = ""; int result = 0;
            if (LoginUserOfficeID.HasValue)
            {
                try
                {
                    DateTime trxDate = DateTime.Parse(date);
                    string voucherNo = trxDate.ToString("ddMMyy") + LoginUserOfficeID.Value;
                    if (!DateTime.MinValue.Equals(trxDate))
                    {
                        using (gBankerDbContext db = new gBankerDbContext())
                        {
                            List<InventoryDailyVoucher> invDetailObj = db.Database.SqlQuery<InventoryDailyVoucher>("SELECT * FROM InventoryDailyVoucher WHERE OfficeID=" + LoginUserOfficeID.Value + " AND TransactionDate='" + trxDate + "'").ToList();
                            if (invDetailObj.Any())
                            {
                                voucherNo = invDetailObj.First().VoucherNo;
                                db.Database.ExecuteSqlCommand("DELETE FROM InventoryDailyVoucher WHERE OfficeID=" + LoginUserOfficeID.Value + " AND TransactionDate='" + trxDate + "'");
                                db.Database.ExecuteSqlCommand("UPDATE Inv_TrxMaster SET IsPosted=0 WHERE OfficeID=" + LoginUserOfficeID.Value + " AND TrxDate='" + trxDate + "' AND IsPosted=1");
                            }
                        }
                        var master = iInvTrxMasterService.GetMany(x => x.OfficeID == LoginUserOfficeID.Value && x.TrxDate == trxDate && x.IsPosted == false);
                        if (master.Any())
                        {
                            var storeDetails = new List<Inv_TrxDetail>();
                            long[] mid = { };
                            using (gBankerDbContext db = new gBankerDbContext())
                            {
                                List<InventoryDailyVoucher> invDetailObj = db.Database.SqlQuery<InventoryDailyVoucher>("SELECT * FROM InventoryDailyVoucher WHERE OfficeID=" + LoginUserOfficeID.Value + " AND TransactionDate='" + trxDate + "'").ToList();
                                if (invDetailObj.Any())
                                {
                                    db.Database.ExecuteSqlCommand("DELETE FROM InventoryDailyVoucher WHERE OfficeID=" + LoginUserOfficeID.Value + " AND TransactionDate='" + trxDate + "'");
                                    db.Database.ExecuteSqlCommand("UPDATE Inv_TrxMaster SET IsPosted=1 WHERE OfficeID=" + LoginUserOfficeID.Value + " AND TrxDate='" + trxDate + "'");
                                }
                                // store in
                                var rfs = master.Where(x => x.Reference == "storein");
                                if (rfs.Any())
                                {
                                    mid = rfs.Select(x => x.TrxMasterID).ToArray();
                                    storeDetails = iInvTrxDetailService.GetMany(x => mid.Contains(x.TrxMasterID)).ToList();
                                    var sd = storeDetails.GroupBy(g => g.AccID)
                                        .Select(s =>
                                        new
                                        {
                                            AccID = s.Key
                                            ,
                                            Debit = s.Sum(x => x.Debit)
                                            ,
                                            Credit = s.Sum(x => x.Credit)
                                        });
                                    foreach (var s in sd)
                                    {
                                        InventoryDailyVoucher v = new InventoryDailyVoucher();
                                        InventoryDailyVoucherHistory vh = new InventoryDailyVoucherHistory();

                                        v.AccID = vh.AccID = s.AccID.Value;
                                        v.CreateDate = vh.CreateDate = DateTime.Now;//storeDetails.First().CreateDate;
                                        v.CreateUser = vh.CreateUser = int.Parse(storeDetails.First().CreateUser);
                                        v.Credit = vh.Credit = s.Credit.Value;
                                        v.Debit = vh.Debit = s.Debit.Value;
                                        v.NarationBng = vh.NarationBng = "";
                                        v.NarationEng = vh.NarationEng = "Store In";
                                        v.OfficeID = vh.OfficeID = rfs.First().OfficeID;
                                        v.TransactionDate = vh.TransactionDate = rfs.First().TrxDate;
                                        v.VoucherNo = vh.VoucherNo = voucherNo;
                                        v.VoucherType = vh.VoucherType = "JR";
                                        v.IsVoucherChange = false;

                                        db.InventoryDailyVouchers.Add(v);
                                        db.InventoryDailyVoucherHistorys.Add(vh);
                                        //db.SaveChanges();

                                    }
                                }
                                // store Out
                                rfs = master.Where(x => x.Reference == "storeout");
                                if (rfs.Any())
                                {
                                    mid = rfs.Select(x => x.TrxMasterID).ToArray();
                                    storeDetails = iInvTrxDetailService.GetMany(x => mid.Contains(x.TrxMasterID)).ToList();
                                    var sd = storeDetails.GroupBy(g => g.AccID)
                                        .Select(s =>
                                        new
                                        {
                                            AccID = s.Key
                                            ,
                                            Debit = s.Sum(x => x.Debit)
                                            ,
                                            Credit = s.Sum(x => x.Credit)
                                        });

                                    foreach (var s in sd)
                                    {
                                        InventoryDailyVoucher v = new InventoryDailyVoucher();
                                        InventoryDailyVoucherHistory vh = new InventoryDailyVoucherHistory();

                                        v.AccID = vh.AccID = s.AccID.Value;
                                        v.CreateDate = vh.CreateDate = storeDetails.First().CreateDate;
                                        v.CreateUser = vh.CreateUser = int.Parse(storeDetails.First().CreateUser);
                                        v.Credit = vh.Credit = s.Credit.Value;
                                        v.Debit = vh.Debit = s.Debit.Value;
                                        v.NarationBng = vh.NarationBng = "";
                                        v.NarationEng = vh.NarationEng = "Store Out";
                                        v.OfficeID = vh.OfficeID = rfs.First().OfficeID;
                                        v.TransactionDate = vh.TransactionDate = rfs.First().TrxDate;
                                        v.VoucherNo = vh.VoucherNo = voucherNo;
                                        v.VoucherType = vh.VoucherType = "JR";
                                        v.IsVoucherChange = false;

                                        db.InventoryDailyVouchers.Add(v);
                                        db.InventoryDailyVoucherHistorys.Add(vh);
                                        //db.SaveChanges();
                                    }
                                }
                                // Dispose
                                rfs = master.Where(x => x.Reference == "storedispose");
                                if (rfs.Any())
                                {
                                    mid = rfs.Select(x => x.TrxMasterID).ToArray();
                                    storeDetails = iInvTrxDetailService.GetMany(x => mid.Contains(x.TrxMasterID)).ToList();
                                    var sd = storeDetails.GroupBy(g => g.AccID)
                                        .Select(s =>
                                        new
                                        {
                                            AccID = s.Key
                                            ,
                                            Debit = s.Sum(x => x.Debit)
                                            ,
                                            Credit = s.Sum(x => x.Credit)
                                        });
                                    foreach (var s in sd)
                                    {
                                        InventoryDailyVoucher v = new InventoryDailyVoucher();
                                        InventoryDailyVoucherHistory vh = new InventoryDailyVoucherHistory();

                                        v.AccID = vh.AccID = s.AccID.Value;
                                        v.CreateDate = vh.CreateDate = storeDetails.First().CreateDate;
                                        v.CreateUser = vh.CreateUser = int.Parse(storeDetails.First().CreateUser);
                                        v.Credit = vh.Credit = s.Credit.Value;
                                        v.Debit = vh.Debit = s.Debit.Value;
                                        v.NarationBng = vh.NarationBng = "";
                                        v.NarationEng = vh.NarationEng = "Store Dispose";
                                        v.OfficeID = vh.OfficeID = rfs.First().OfficeID;
                                        v.TransactionDate = vh.TransactionDate = rfs.First().TrxDate;
                                        v.VoucherNo = vh.VoucherNo = voucherNo;
                                        v.VoucherType = vh.VoucherType = "JR";
                                        v.IsVoucherChange = false;
                                        db.InventoryDailyVouchers.Add(v);
                                        db.InventoryDailyVoucherHistorys.Add(vh);

                                    }
                                }
                                db.SaveChanges();
                            }

                            master.ToList().ForEach(x => x.IsPosted = true);
                            foreach (var m in master)
                                iInvTrxMasterService.Update(m);
                            msg = "Positng successfully";
                            result = 1;
                        }
                        else
                            msg = "Data not found";
                    }
                    else
                        msg = "Date format not valid";
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
            }
            else
                msg = "Re-Login";
            return Json(new { Message = msg, Result = result }, JsonRequestBehavior.AllowGet);
        }
        #endregion service

        #region Report
        protected void PrintReport<T>(string reportName, T dataSource, Dictionary<string, object> parameters)
        {
            try
            {
                ReportDocument crDocument = new ReportDocument();

                ExportOptions crExportOptions = new ExportOptions();
                DiskFileDestinationOptions crDiskFileDestination = new DiskFileDestinationOptions();
                string strFName;
                //All CR file assumed that it resides in the reports folder....
                string strReportPathAbsolute = Server.MapPath("~/Reports/" + reportName);
                crDocument.Load(strReportPathAbsolute);
                crDocument.SetDataSource(dataSource);

                foreach (KeyValuePair<string, object> kvp in parameters)
                {
                    crDocument.SetParameterValue(kvp.Key, kvp.Value);

                }
                strFName = Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
                crDiskFileDestination.DiskFileName = strFName;
                crExportOptions = crDocument.ExportOptions;
                crExportOptions.DestinationOptions = crDiskFileDestination;
                crExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                crExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                crDocument.Export();
                Response.ContentType = "application/pdf";
                Response.WriteFile(strFName);
                Response.End();
                // Response.Close();
                System.IO.File.Delete(strFName);

            }
            catch (Exception ex)
            {
            }

        }
        public ActionResult CommonReports
            (int reportTypeId, int categoryId, int itemId, DateTime? from
            , DateTime? to, string empCode, int? OfficeID, string OfficeCode)
        {
            try
            {
                string report = "";
                if (reportTypeId > 0)
                {
                    var paramValues = new List<gBanker.Service.ReportExecutionService.ParameterValue>();
                    //var wList = iInvWarehouseService.GetAll();
                    int offID = 0;
                    if (!string.IsNullOrEmpty(OfficeCode))
                    {
                        var off = iOfficeService.GetByOfficeCode(OfficeCode);
                        if (off != null)
                            offID = off.OfficeID;
                        else
                            offID = -1;
                    }

                    var wList = iInvWarehouseService.GetOfficeIDWise((offID != 0 ? offID : (LoginUserOfficeID ?? 0)));
                    //var Org = iOrganizationService.GetById(SessionHelper.LoginUserOrganizationID ?? 0);
                    //var 
                    if (reportTypeId == 1)
                    {
                        //if(itemId==0)
                        return Content("<b>This Report now Off</b>");
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "OrgName", Value = (SessionHelper.OrganizationName).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "WarehouseID", Value = (wList.First().WarehouseID).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "FromDate", Value = (from ?? DateTime.Now).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "toDate", Value = (to ?? DateTime.Now).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "ItemID", Value = (itemId).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "CategoryId", Value = (categoryId).ToString() });
                        report = "/gBanker_Reports/rpt_inv_Register";
                    }
                    else if (reportTypeId == 2 | reportTypeId == 3)
                    {
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "OrgName", Value = (SessionHelper.OrganizationName).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "FromDate", Value = (from ?? DateTime.Now).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "ToDate", Value = (to ?? DateTime.Now).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "ItemID", Value = (itemId).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "storeID", Value = (wList.First().WarehouseID).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "CategoryId", Value = (categoryId).ToString() });
                        report = "/gBanker_Reports/" + (reportTypeId == 2 ? "rpt_Inv_StoreInCommon"
                            : "rpt_Inv_StoreOutCommon");
                    }
                    else if (reportTypeId == 4)
                    {
                        int empID = 0;
                        if (!string.IsNullOrEmpty(empCode))
                        {
                            var emp = iEmployeeService.GetByCode(empCode);
                            if (emp != null)
                                empID = emp.EmployeeID;
                        }
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "OrgName", Value = (SessionHelper.OrganizationName).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "empID", Value = empID.ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "FromDate", Value = (from ?? DateTime.Now).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "ToDate", Value = (to ?? DateTime.Now).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "ItemID", Value = (itemId).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "storeID", Value = (wList.First().WarehouseID).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "CategoryId", Value = (categoryId).ToString() });
                        report = "/gBanker_Reports/" + (empID > 0 ? "rpt_Inv_StoreOutIndividualXEmployeeCommon" : "rpt_Inv_StoreOutIndividualCommon");
                    }
                    else if (reportTypeId == 5 || reportTypeId == 10)
                    {
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "OrgName", Value = (SessionHelper.OrganizationName).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "FromDate", Value = (from ?? DateTime.Now).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "ToDate", Value = (to ?? DateTime.Now).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "storeID", Value = (wList.First().WarehouseID).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "CategoryId", Value = (categoryId).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "ReportTypeId", Value = (reportTypeId).ToString() });
                        report = "/gBanker_Reports/rpt_Inv_RequisitionCommonReport";
                    }
                    else if (reportTypeId == 6 | reportTypeId == 7)
                    {
                        string reportType = "";
                        if (reportTypeId == 6 && !string.IsNullOrEmpty(OfficeCode))
                        {
                            //var off = iOfficeService.GetByOfficeCode(OfficeCode);
                            // OfficeID = off.OfficeID;
                            if (offID > 0)
                                OfficeID = offID;
                            reportType = "Individual";
                        }
                        else if (reportTypeId == 7 && OfficeID.HasValue)
                            reportType = "AllBranch";
                        else
                            return Content("Required field not full fill");
                        if (!OfficeID.HasValue)
                            return Content("Office not found");
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "OrgName", Value = (SessionHelper.OrganizationName).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "officeID", Value = (OfficeID ?? 0).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "ReportType", Value = reportType });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "CategoryID", Value = (categoryId).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "ItemID", Value = (itemId).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "FromDate", Value = (from ?? DateTime.Now).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "ToDate", Value = (to ?? DateTime.Now).ToString() });
                        report = "/gBanker_Reports/" + (reportTypeId == 6 ? "rpt_Inv_IndividualBranch" : reportTypeId == 7 ? "rpt_Inv_AllBranch" : "");
                    }
                    else if (reportTypeId == 8)
                    {
                        var off = iOfficeService.GetById(OfficeID ?? 0);
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "OrgName", Value = (SessionHelper.OrganizationName).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "officeID", Value = (OfficeID ?? 0).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "FromDate", Value = (from ?? DateTime.Now).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "ToDate", Value = (to ?? DateTime.Now).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "POfficeCode", Value = off.OfficeCode });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "POfficeName", Value = off.OfficeName });
                        report = "/gBanker_Reports/rpt_Inv_StoreMaterial";
                    }
                    else if (reportTypeId == 9/*Store Summary Report*/)
                    {
                        int storeID = 0;
                        if (wList.Any())
                            storeID = wList.First().WarehouseID;
                        var off = iOfficeService.GetById(LoginUserOfficeID ?? 0);
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "OrgName", Value = (SessionHelper.OrganizationName).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "warehouseID", Value = (storeID).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "FromDate", Value = (from ?? DateTime.Now).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "ToDate", Value = (to ?? DateTime.Now).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "POfficeCode", Value = off.OfficeCode });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "POfficeName", Value = off.OfficeName });
                        report = "/gBanker_Reports/rpt_Inv_StoreSummaryReport";
                    }
                    else if (reportTypeId == 11)
                    {
                        int storeID = 0;
                        if (wList.Any())
                            storeID = wList.First().WarehouseID;

                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "OrgName", Value = (SessionHelper.OrganizationName).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "RequestWareID", Value = (storeID).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "from", Value = (from ?? DateTime.Now).ToString("dd-MMM-yyyy") });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "to", Value = (to ?? DateTime.Now).ToString("dd-MMM-yyyy") });
                        // paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "thisOffice", Value = !string.IsNullOrEmpty(OfficeCode)?1:});
                        report = "/gBanker_Reports/rpt_Inv_DisposeRequestStatusReport";
                    }
                    else if (reportTypeId == 20 || reportTypeId == 21 || reportTypeId == 22 || reportTypeId == 23)
                    {
                        int storeID = 0;
                        if (wList.Any())
                            storeID = wList.First().WarehouseID;
                        if (reportTypeId == 23)
                        {
                            paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "officeCode", Value = OfficeCode });
                            paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "OfficeType", Value = (SessionHelper.LoggedInOfficeDetail.OfficeLevel == 2 ? "ZO" : SessionHelper.LoggedInOfficeDetail.OfficeLevel==1 ? "Ho" : "") });
                        }
                        else
                            paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "OfficeType", Value =(SessionHelper.LoggedInOfficeDetail.OfficeLevel == 1? "Ho":"")});
                            
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "OrgName", Value = (SessionHelper.OrganizationName).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "OfficeID", Value = (OfficeID ?? storeID).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "ItemID", Value = (itemId).ToString() });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "FromDate", Value = (from ?? DateTime.Now).ToString("dd-MMM-yyyy") });
                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "ToDate", Value = (to ?? DateTime.Now).ToString("dd-MMM-yyyy") });

                        paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "reportType", Value = (reportTypeId == 20 ? "" : reportTypeId == 21 ? "Item" : reportTypeId == 22 ? "OffItem" : reportTypeId == 23 ? "OffDisposeItem" : "") });

                        report = "/gBanker_Reports/" + (reportTypeId == 20 ? "rpt_ConsolidateDispose"
                            : reportTypeId == 21 ? "rpt_ConsolidateDisposeItemWise"
                            : reportTypeId == 22 ? "rpt_ConsolidateDisposeItemOfficeWise"
                            : reportTypeId == 23 ? "rpt_ConsolidateOfficeWiseDisposeItem"
                            : "");
                    }
                    PrintSSRSReport(report, paramValues.ToArray(), "gBankerDbContext");
                    return Content(string.Empty);
                }
                else
                    return Content("<b>Report Type Not Found</b>");

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        //public ActionResult CommonReports(int reportTypeId, int itemId, DateTime? from, DateTime? to)
        //{
        //    try
        //    {
        //        string report = "";
        //        if (reportTypeId > 0)
        //        {
        //            var paramValues = new List<gBanker.Service.ReportExecutionService.ParameterValue>();
        //            var wList = iInvWarehouseService.GetMany(x => x.OfficeID == LoginUserOfficeID);
        //            //wList = iInvWarehouseService.GetOfficeIDWise(LoginUserOfficeID ?? 0);
        //            //var Org = iOrganizationService.GetById(SessionHelper.LoginUserOrganizationID ?? 0);
        //            //var 
        //            if (reportTypeId == 1)
        //            {
        //                //if(itemId==0)
        //                //    return Content("<b>Item Not Found</b>");
        //                paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "OrgName", Value = (SessionHelper.OrganizationName).ToString() });
        //                paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "WarehouseID", Value = (wList.First().WarehouseID).ToString() });
        //                paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "FromDate", Value = (from ?? DateTime.Now).ToString() });
        //                paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "toDate", Value = (to ?? DateTime.Now).ToString() });
        //                paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "ItemID", Value = (itemId).ToString() });
        //                report = "/gBanker_Reports/rpt_inv_Register";
        //            }
        //            else if (reportTypeId == 2)
        //            {
        //                paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "OrgName", Value = (SessionHelper.OrganizationName).ToString() });
        //                paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "WarehouseID", Value = (wList.First().WarehouseID).ToString() });
        //                paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "FromDate", Value = (from ?? DateTime.Now).ToString() });
        //                paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "toDate", Value = (to ?? DateTime.Now).ToString() });
        //                report = "/gBanker_Reports/rpt_inv_DailyRegister";
        //            }

        //            PrintSSRSReport(report, paramValues.ToArray(), "gBankerDbContext");
        //            return Content(string.Empty);
        //        }
        //        else
        //            return Content("<b>Report Type Not Found</b>");

        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(ex.Message);
        //    }
        //}

        public ActionResult StoreRreport(string type)
        {
            try
            {
                string report = "";
                if (string.IsNullOrEmpty(type))
                    report = "sp_StoreDetails";
                else if (type == "Consulted")
                    report = "sp_StoreDetailsForConsulted";
                using (gBankerDbContext db = new gBankerDbContext())
                {

                    List<Inv_StoreViewModel> objList = db.Database.SqlQuery<Inv_StoreViewModel>("exec " + report + " " + (LoginUserOfficeID ?? 0) + "," + 54 + "").ToList();
                    PrintReport("invcrptStoreDetails.rpt", objList, new Dictionary<string, object>());
                }
            }
            catch (Exception)
            {
            }
            return Content(string.Empty);
        }

        public ActionResult RequsitionReport(string status, int rmID)
        {
            try
            {
                if (string.IsNullOrEmpty(status))
                    return Content("Status Not Found");
                else if (rmID == 0) return Content("Requsition Master ID Not Found");
                else
                {
                    //var paramSe = new
                    //{
                    //    reqMasterID = rmID,
                    //    apStatus = status
                    //};
                    var paramValues = new List<gBanker.Service.ReportExecutionService.ParameterValue>();
                    var wList = iInvWarehouseService.GetAll();
                    wList = iInvWarehouseService.GetOfficeIDWise(LoginUserOfficeID ?? 0);
                    paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "OrgName", Value = (SessionHelper.OrganizationName).ToString() });
                    paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "reqMasterID", Value = (rmID).ToString() });
                    paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "apStatus", Value = status });
                    PrintSSRSReport("/gBanker_Reports/rpt_inv_Requsition", paramValues.ToArray(), "gBankerDbContext");
                    return Content(string.Empty);
                    //var objList = new gBankerReportDataAccess().GetDataOnDateset("sp_RequsitionForReport ", paramSe);
                    //PrintReport("InvRequsition.rpt", objList, new Dictionary<string, object>());

                }

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        public ActionResult VoucherReport(long masterID)
        {
            try
            {
                if (masterID == 0) return Content("ID Not Found");
                else
                {
                    var paramValues = new List<gBanker.Service.ReportExecutionService.ParameterValue>();
                    paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "masterID", Value = masterID.ToString() });
                    PrintSSRSReport("/gBanker_Reports/rpt_inv_Voucher", paramValues.ToArray(), "gBankerDbContext");
                    return Content(string.Empty);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }
        public ActionResult ItemXStoreDetailsReport(long masterID, string type)
        {
            try
            {
                if (masterID == 0) return Content("ID Not Found");
                else
                {
                    string report = "";
                    if (type == "I") report = "rpt_Inv_ItemXStoreDetailsReportForInBuro";
                    else if (type == "O") report = "rpt_Inv_ItemXStoreDetailsReportForOutBuro";
                    else if (type == "D") report = "rpt_Inv_ItemXStoreDetailsReportForDisposeBuro";

                    var paramValues = new List<gBanker.Service.ReportExecutionService.ParameterValue>();
                    paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "TrxMasterID", Value = masterID.ToString() });
                    PrintSSRSReport("/gBanker_Reports/" + report + "", paramValues.ToArray(), "gBankerDbContext");
                    return Content(string.Empty);
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }
        public ActionResult StoreReportForArea(int reuqisitionID, string status)
        {
            try
            {
                if (reuqisitionID == 0) return Content("ID Not Found");
                else
                {
                    var paramValues = new List<gBanker.Service.ReportExecutionService.ParameterValue>();
                    paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "status", Value = status });
                    paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "OfficeID", Value = LoginUserOfficeID.Value.ToString() });
                    paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "masterID", Value = reuqisitionID.ToString() });
                    PrintSSRSReport("/gBanker_Reports/rpt_inv_StoreReportBuro", paramValues.ToArray(), "gBankerDbContext");
                    return Content(string.Empty);

                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult DayWiseVoucher(int reportTypeId, DateTime from, DateTime to)
        {
            try
            {
                if (DateTime.MinValue.Equals(from) | DateTime.MinValue.Equals(to))
                    return Content("Date Not Found");
                else
                {
                    var paramValues = new List<gBanker.Service.ReportExecutionService.ParameterValue>();
                    paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "From", Value = from.ToString("dd MMM yyyy") });
                    paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "OfficeID", Value = LoginUserOfficeID.Value.ToString() });
                    paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "to", Value = to.ToString("dd MMM yyyy") });
                    PrintSSRSReport("/gBanker_Reports/rpt_inv_DayXVoucher", paramValues.ToArray(), "gBankerDbContext");
                    return Content(string.Empty);

                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult GetDisposeRequestReport(string disposeNo)
        {
            var paramValues = new List<gBanker.Service.ReportExecutionService.ParameterValue>();
            paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "OrgName", Value = (SessionHelper.OrganizationName).ToString() });
            paramValues.Add(new gBanker.Service.ReportExecutionService.ParameterValue() { Name = "DisposeRequestNo", Value = disposeNo });
            PrintSSRSReport("/gBanker_Reports/rpt_Inv_DisposeRequestReport", paramValues.ToArray(), "gBankerDbContext");
            return Content(string.Empty);
        }
        #endregion Report
    }
}