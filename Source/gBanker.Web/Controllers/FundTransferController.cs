using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Utility.Constants;
using static Utility.UIConstants;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using gBanker.Service.StoredProcedure;


namespace gBanker.Web.Controllers
{
    public class FundTransferController : BaseController
    {
        #region Variables
        private readonly IAccTrxMasterService accTrxMasterService;
        private readonly IAccTrxDetailService accTrxDetailService;
        private readonly IAccChartService accChartService;
        private readonly IAccLastVoucherService accLastVoucherService;
        private readonly IProcessInfoService processInfoService;
        private readonly IOfficeService officeService;
        private readonly IAccReportService accReportService;
        private readonly IApplicationSettingsService applicationSettingsService;
        private readonly IWeeklyReportService weeklyReportService;

        private readonly IAccReconcileService reConcileService;
        private readonly IUltimateReportService ultimateRepostService;
        private readonly IAccMappingForFundTransferService accFundTransferService;

        private readonly IEmployeeSPService sPService;
        #endregion

        public FundTransferController(IAccTrxMasterService accTrxMasterService, IAccTrxDetailService accTrxDetailService, IAccChartService accChartService
            , IAccLastVoucherService accLastVoucherService, IProcessInfoService processInfoService, IOfficeService officeService, IAccReportService accReportService
            , IApplicationSettingsService applicationSettingsService, IWeeklyReportService weeklyReportService, IAccReconcileService reConcileService
            , IUltimateReportService ultimateRepostService, IAccMappingForFundTransferService _accFundTransferService, IEmployeeSPService sPService)
        {
            this.accTrxMasterService = accTrxMasterService;
            this.accTrxDetailService = accTrxDetailService;
            this.accChartService = accChartService;
            this.accLastVoucherService = accLastVoucherService;
            this.processInfoService = processInfoService;
            this.officeService = officeService;
            this.accReportService = accReportService;
            this.applicationSettingsService = applicationSettingsService;
            this.weeklyReportService = weeklyReportService;
            this.reConcileService = reConcileService;
            this.ultimateRepostService = ultimateRepostService;
            this.accFundTransferService = _accFundTransferService;

            this.sPService = sPService;
        }
        // GET: FundTransfer
        // mahbub




        public JsonResult GetHOAccCodeList( string term = "" )
        {
            List<AccChartViewModel> List_AccChartViewModel = new List<AccChartViewModel>();
            var param = new { OfficeID = LoginUserOfficeID, searchTerm = term };
            var allSavingsummary = sPService.GetDataWithParameter(param, "GetHOAccCodeList");
            if (allSavingsummary.Tables[0].Rows.Count > 0)
            {
                List_AccChartViewModel = allSavingsummary.Tables[0].AsEnumerable()
               .Select(row => new AccChartViewModel
               {
                   //AccID = row.Field<int>("HOAccCode"),
                   //AccCode = row.Field<string>("HOAccCode") + " - " + row.Field<string>("AccName")
                   AccCode = row.Field<string>("HOAccCode"),
                   AccName = row.Field<string>("HOAccCode"),
               }).ToList();
            }
            return Json(List_AccChartViewModel, JsonRequestBehavior.AllowGet);
        }

        // mahbub


        [HttpPost]
        public JsonResult DeleteFundTransferVoucher(long id)
        {
            var count = new gBankerDbContext().Database.ExecuteSqlCommand("sp_FUNDTransferVoucherDelete " + id + "");
            return Json("Successfully deleted");
        }

        public ActionResult Index()
        {
            var model = new AccTrxMasterViewModel();
            if (IsDayInitiated)
                model.TrxDate = TransactionDate;
            return View(model);
        }

        public ActionResult Create()
        {
            var model = new AccVoucherEntryViewModel_dsk();
            ReconcileMapDropDownList(model);
            if (IsDayInitiated)
            {
                model.StrTrxDate = TransactionDate.ToString("dd-MMM-yyyy");
                ViewData["TransactionDate"] = model.StrTrxDate;
            }
            else
            {
                var param = new { OfficeId = Convert.ToInt32(SessionHelper.LoginUserOfficeID), OrgID = Convert.ToInt32(LoggedInOrganizationID) };
                var workingDay = accReportService.GetDataValidWorkngDay(param);
                var workingDt = workingDay.Tables[0].Rows[0]["vBusinessDate"].ToString();

                if (workingDt != "")
                    model.StrTrxDate = workingDt;
                else
                    model.StrTrxDate = DateTime.Now.Date.ToString("dd-MMM-yyyy");
                ViewData["TransactionDate"] = model.StrTrxDate;
            }

            model.VoucherType = "Dr";
            model.ReffNoList = ReffNoList();
            model.OfficeList = OfficeList("", SessionHelper.LoginUserOfficeID ?? 0);
            var SenderBankList = new List<SelectListItem>();
            SenderBankList.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            model.SenderBankList = SenderBankList;
            model.IsRectify = false;
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            model.OfficeLevel = officeService.GetById(model.OfficeID).OfficeLevel;

            return View(model);
        }

        [HttpPost] // mahfuz
        public JsonResult PostFundTransferVoucher(FundTransferViewModel model)
        {
            int status = 0; string message = "";
            if (model == null) message = "Data not found.";
            else if (DateTime.MinValue.Equals(model.Date)) message = "Date format is not correct.";
            else if (model.Sender == null) message = "Sender voucher data not found.";
            else if (model.Receiver == null) message = "Receiver voucher data not found.";
            else if (model.Sender.Where(x => x.OfficeID == 0).Any()) message = "Sender Office not found.";
            else if (model.Receiver.Where(x => x.OfficeID == 0).Any()) message = "Receiver Office not found.";
            else if (model.Sender.Where(x => x.AccID == 0).Any()) message = "Sender Voucher Account Head not found.";
            else if (model.Receiver.Where(x => x.AccID == 0).Any()) message = "Receiver Voucher Account Head not found.";
            else
            {
                bool has_HO = false;
                if (model.HO_vou != null && model.TransactionType != "Jr")
                {
                    if (model.HO_vou.Where(x => x.OfficeID == 0).Any()) message = "Office not found.";
                    else if (model.HO_vou.Where(x => x.AccID == 0).Any()) message = "Voucher Account Head not found.";
                    else
                        has_HO = true;
                }
                try
                {
                    List<AccTrxDetail> detailList = new List<AccTrxDetail>();
                    var parm = new { OfficeID = LoginUserOfficeID, TrxDate = model.Date };

                    var dbReffNo = ultimateRepostService.Set_GenerateReffNo(parm);
                    var newReffNo = dbReffNo.Tables[0].AsEnumerable()
                   .Select(row => new AccVoucherEntryViewModel_dsk()
                   {
                       ReffNo = row.Field<string>("ReffNo")
                   }).FirstOrDefault();

                    // Sender
                    #region Sender
                    var send_Master = new AccTrxMaster()
                    {
                        OrgID = LoggedInOrganizationID,
                        OfficeID = model.Sender.First().OfficeID,
                        TrxDate = model.Date,
                        VoucherNo = GenerateNewVoucher(model.Sender.First().OfficeID),
                        VoucherDesc = model.Narration,
                        VoucherType = model.TransactionType,
                        Reference = model.Reference,
                        IsAutoVoucher = false,
                        IsPosted = true,
                        IsYearlyClosing = false,
                        IsActive = true,
                        CreateUser = SessionHelper.LoggedInEmployeeID.ToString(),
                        CreateDate = DateTime.Now,
                        IsRectify = false,
                        IsReconcileVoucher = true
                    };
                    accTrxMasterService.Create(send_Master);
                    foreach (var s in model.Sender)
                    {
                        AccTrxDetail d = new AccTrxDetail
                        {
                            AccID = s.AccID,
                            CreateDate = DateTime.Now,
                            CreateUser = SessionHelper.LoggedInEmployeeID.ToString(),
                            Credit = s.Credit,
                            Debit = s.Debit,
                            IsActive = true,
                            Narration = s.Narration,
                            TrxMasterID = send_Master.TrxMasterID,
                        };
                        detailList.Add(d);
                    }
                    #endregion Sender end
                    // Sender
                    #region Receiver
                    var rec_Master = new AccTrxMaster()
                    {
                        OrgID = LoggedInOrganizationID,
                        OfficeID = model.Receiver.First().OfficeID,
                        TrxDate = model.Date,
                        VoucherNo = GenerateNewVoucher(model.Receiver.First().OfficeID),
                        VoucherDesc = model.Narration,
                        VoucherType = model.TransactionType,
                        Reference = model.Reference,
                        IsAutoVoucher = false,
                        IsPosted = true,
                        IsYearlyClosing = false,
                        IsActive = true,
                        CreateUser = SessionHelper.LoggedInEmployeeID.ToString(),
                        CreateDate = DateTime.Now,
                        IsRectify = false,
                        IsReconcileVoucher = true
                    };
                    accTrxMasterService.Create(rec_Master);
                    foreach (var r in model.Receiver)
                    {
                        AccTrxDetail d = new AccTrxDetail
                        {
                            AccID = r.AccID,
                            CreateDate = DateTime.Now,
                            CreateUser = SessionHelper.LoggedInEmployeeID.ToString(),
                            Credit = r.Credit,
                            Debit = r.Debit,
                            IsActive = true,
                            Narration = r.Narration,
                            TrxMasterID = rec_Master.TrxMasterID,
                        };
                        detailList.Add(d);
                    }
                    #endregion receiver end
                    // HO
                    #region HO
                    var ho_Master = new AccTrxMaster();
                    if (has_HO)
                    {

                        ho_Master = new AccTrxMaster()
                        {
                            OrgID = LoggedInOrganizationID,
                            OfficeID = model.HO_vou.First().OfficeID,
                            TrxDate = model.Date,
                            VoucherNo = GenerateNewVoucher(model.HO_vou.First().OfficeID),
                            VoucherDesc = model.Narration,
                            VoucherType = "Jr",//model.TransactionType,
                            Reference = model.Reference,
                            IsAutoVoucher = false,
                            IsPosted = true,
                            IsYearlyClosing = false,
                            IsActive = true,
                            CreateUser = SessionHelper.LoggedInEmployeeID.ToString(),
                            CreateDate = DateTime.Now,
                            IsRectify = false,
                            IsReconcileVoucher = true
                        };
                        accTrxMasterService.Create(ho_Master);
                        foreach (var h in model.HO_vou)
                        {
                            AccTrxDetail d = new AccTrxDetail
                            {
                                AccID = h.AccID,
                                CreateDate = DateTime.Now,
                                CreateUser = SessionHelper.LoggedInEmployeeID.ToString(),
                                Credit = h.Credit,
                                Debit = h.Debit,
                                IsActive = true,
                                Narration = "",
                                TrxMasterID = ho_Master.TrxMasterID,
                            };
                            detailList.Add(d);
                        }
                    }

                    #endregion HO end

                    var accReconcileObj = new AccReconcile()
                    {
                        CreateDate = DateTime.Now,
                        CreateUser = SessionHelper.LoggedInEmployeeID.ToString(),
                        Credit = (model.TransactionType == "Jr" ? model.Sender.Sum(x => x.Debit) : 0),
                        Debit = model.Sender.Sum(x => x.Debit),
                        HO_TrxMasterID = ((ho_Master.TrxMasterID > 0) ? ho_Master.TrxMasterID : (long?)null),
                        OrgID = LoggedInOrganizationID,
                        IsReconcile = true,
                        IsActive = true,
                        ReceiverOfficeId = model.Receiver.First().OfficeID,
                        Receiver_TrxMasterID = rec_Master.TrxMasterID,
                        ReffNo = newReffNo.ReffNo,
                        SenderOfficeId = model.Sender.First().OfficeID,
                        TrxDate = model.Date,
                        TrxMasterID = send_Master.TrxMasterID,
                    };


                    reConcileService.Create(accReconcileObj);
                    accTrxDetailService.SaveDailyTrxDetail(detailList);
                    status = 1; message = "Fund Transfer voucher create successfully";
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
            }
            return Json(new { status = status, message = message });
        }

        [HttpPost] // mahbub
        public JsonResult PostFundTransferVoucher_sp(FundTransferViewModel model)
        {
            int status = 0;
            string message = "";

            try
            {
                using (var db = new gBankerDbContext())
                {
                    var senderTable = new DataTable();
                    senderTable.Columns.Add("OfficeID", typeof(int));
                    senderTable.Columns.Add("AccID", typeof(int));
                    senderTable.Columns.Add("Debit", typeof(decimal));
                    senderTable.Columns.Add("Credit", typeof(decimal));
                    senderTable.Columns.Add("Narration", typeof(string));
                    foreach (var s in model.Sender)
                        senderTable.Rows.Add(s.OfficeID, s.AccID, s.Debit, s.Credit, s.Narration);

                    var receiverTable = new DataTable();
                    receiverTable.Columns.Add("OfficeID", typeof(int));
                    receiverTable.Columns.Add("AccID", typeof(int));
                    receiverTable.Columns.Add("Debit", typeof(decimal));
                    receiverTable.Columns.Add("Credit", typeof(decimal));
                    receiverTable.Columns.Add("Narration", typeof(string));
                    foreach (var r in model.Receiver)
                        receiverTable.Rows.Add(r.OfficeID, r.AccID, r.Debit, r.Credit, r.Narration);

                    var hoTable = new DataTable();
                    hoTable.Columns.Add("OfficeID", typeof(int));
                    hoTable.Columns.Add("AccID", typeof(int));
                    hoTable.Columns.Add("Debit", typeof(decimal));
                    hoTable.Columns.Add("Credit", typeof(decimal));
                    if (model.HO_vou != null)
                        foreach (var h in model.HO_vou)
                            hoTable.Rows.Add(h.OfficeID, h.AccID, h.Debit, h.Credit);

                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@Date", model.Date),
                        new SqlParameter("@Narration", model.Narration),
                        new SqlParameter("@Reference", model.Reference),
                        new SqlParameter("@TransactionType", model.TransactionType),
                        new SqlParameter("@SenderDetails", senderTable) { SqlDbType = SqlDbType.Structured },
                        new SqlParameter("@ReceiverDetails", receiverTable) { SqlDbType = SqlDbType.Structured },
                        new SqlParameter("@HODetails", hoTable) { SqlDbType = SqlDbType.Structured }
                    };

                    status = db.Database.ExecuteSqlCommand("EXEC PostFundTransferVoucher @Date, @Narration, @Reference, @TransactionType, @SenderDetails, @ReceiverDetails, @HODetails", parameters.ToArray());
                    message = status == 1 ? "Fund transfer voucher created successfully." : "Failed to create fund transfer voucher.";
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new { status, message });
        }


        public ActionResult AccountMappingForFundTransfer()
        {
            var lst = new List<SelectListItem>();
            lst.Add(new SelectListItem() { Text = "Select" });
            lst.AddRange(new gBankerDbContext().Database.SqlQuery<AccChart>($"sp_ChartOfAccountMappingForFundTransfer '{"HOMappingForFundTransfer"}'")
                /*new gBankerDbContext().Database.SqlQuery<AccChart>("select distinct th.* from AccChartMapping a inner join office o ON a.officeID = o.OfficeID " +
                "Inner Join AccChart ac ON a.AccID = ac.AccID Inner Join AccChart th ON ac.ThirdLevel = th.AccCode where OfficeCode = '99999' AND ac.ModuleID = 8 ORDER BY th.AccCode")*/
                .Select(s => new SelectListItem { Value = s.AccCode, Text = s.AccCode + " " + s.AccName }));
            ViewBag.HOAccounts = lst;
            lst = new List<SelectListItem>();
            lst.Add(new SelectListItem() { Text = "Select" });
            lst.AddRange(new gBankerDbContext().Database.SqlQuery<AccChart>($"sp_ChartOfAccountMappingForFundTransfer '{"NotHOMappingForFundTransfer"}'")
                /*"SELECT DISTINCT ac.* " +
                "from AccChartMapping a inner join office o ON a.officeID = o.OfficeID Inner Join AccChart ac ON a.AccID = ac.AccID " +
                "where OfficeCode <> '99999' AND ac.ModuleID = 8 ORDER BY ac.AccCode")*/.Select(s => new SelectListItem { Value = s.AccCode, Text = s.AccCode + " " + s.AccName }));
            ViewBag.BRAccounts = lst;
            return View();
        }



        //[HttpPost]
        //public JsonResult PostAccountMapping(string HOAccCode, string BrAccCode, int? id)
        //{
        //    try
        //    {
        //        if (accFundTransferService.GetMany(x => x.HOFundAccCode == HOAccCode/* || x.BRFundAccCode==BrAccCode*/).Any())
        //            return Json(new { status = 0, msg = UIMessages.DUPLICATE_ENTRY });
        //        else
        //        {
        //            if (id == 0)
        //            {
        //                AccMappingForFundTransfer obj = new AccMappingForFundTransfer()
        //                {
        //                    BRFundAccCode = BrAccCode,
        //                    HOFundAccCode = HOAccCode,
        //                    IsActive = true,
        //                    CreateBy = SessionHelper.LoggedInEmployeeID.ToString(),
        //                    CreateDate = DateTime.Now
        //                };
        //                accFundTransferService.Create(obj);
        //                return Json(new { status = 1, msg = MessageTexts.Insert_Success });
        //            }
        //            else
        //            {
        //                var obj = accFundTransferService.GetById(id.Value);
        //                obj.BRFundAccCode = BrAccCode;
        //                obj.HOFundAccCode = HOAccCode;
        //                obj.UpdateBy = LoggedInEmployeeID.ToString();
        //                obj.UpdateDate = DateTime.Now;
        //                accFundTransferService.Update(obj);
        //                return Json(new { status = 1, msg = MessageTexts.Update_Success });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { status = 0, msg = ex.Message });
        //    }
        //}


        public JsonResult PostAccountMapping(string HOAccCode, string BrAccCode, int? id)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    // Prepare parameters
                    var parameters = new List<SqlParameter>
                        {
                            new SqlParameter("@HOAccCode", HOAccCode),
                            new SqlParameter("@BrAccCode", BrAccCode),
                            new SqlParameter("@Id", id ?? (object)DBNull.Value),
                            new SqlParameter("@LoggedInEmployeeID", SessionHelper.LoggedInEmployeeID.ToString())
                        };

                    // Execute stored procedure
                    var result = db.Database.SqlQuery<PostAccountMappingResult>(
                        "EXEC PostAccountMapping @HOAccCode, @BrAccCode, @Id, @LoggedInEmployeeID",
                        parameters.ToArray()
                    ).FirstOrDefault();

                    if (result != null && result.status == 1)
                    {
                        return Json(new { status = result.status, msg = result.message }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = result?.status ?? 0, msg = result?.message ?? "Operation failed" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        public JsonResult DeleteAccountMapping(int? id)
        {
            var obj = accFundTransferService.GetById(id.Value);
            obj.IsActive = false;
            obj.UpdateBy = LoggedInEmployeeID.ToString();
            obj.UpdateDate = DateTime.Now;
            accFundTransferService.Update(obj);
            return Json(new { status = 1, msg = MessageTexts.Delete_Success });
        }
        public JsonResult GetAccountMapping(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                var lst = new gBankerDbContext().Database.SqlQuery<AccMappingForFundTransferViewModel>($"FundTransferAccMappingForGrid"
                    /*"SELECT m.ID,m.HOFundAccCode,ho.AccName AS HOFundAccName,m.BRFundAccCode,br.AccName AS BRFundAccName " +
                    "FROM AccMappingForFundTransfer m inner join AccChart ho on m.HOFundAccCode = ho.AccCode inner join AccChart br on m.BRFundAccCode = br.AccCode where m.IsActive = 1"*/).AsEnumerable();

                return Json(new { Result = "OK", Records = lst, TotalRecordCount = lst.ToList().Count() });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        //public JsonResult GetReconcileVoucherList
        //    (string trxDate, int jtStartIndex = 0, int jtPageSize = 20, string jtSorting = null, string filterColumn = "", string filterValue = "", string DateFromValue = "", string DateToValue = "")
        //{
        //    try
        //    {

        //        if (trxDate != "")
        //        {
        //            trxDate = DateTime.Parse(trxDate).ToString("dd-MMM-yyyy");
        //            DateFromValue = (string.IsNullOrEmpty(DateFromValue) ? trxDate : DateFromValue);
        //            DateToValue = (string.IsNullOrEmpty(DateToValue) ? trxDate : DateToValue);

        //            string adv_search = (string.IsNullOrEmpty(filterColumn) || filterColumn == "Viewall" ? ""
        //                : filterColumn == "OfficeCode" ? $" AND (snd.OfficeCode='{filterValue}' OR rcv.OfficeCode='{filterValue}')"
        //                : filterColumn == "VoucherCode" ? $" AND (snd_v.VoucherNo='{filterValue}' OR rcv_v.VoucherNo='{filterValue}' OR ho_v.VoucherNo='{filterValue}')"
        //                : filterColumn == "TransactionType" ? $" AND (snd_v.VoucherType='{filterValue}' OR rcv_v.VoucherType='{filterValue}' OR ho_v.VoucherType='{filterValue}')"
        //                : "");

        //            string qry = $" SELECT rc.AccReconcileID,CONVERT(varchar(20),rc.TrxDate,103)TrxDtMsg,ReffNo,Credit,Debit,snd.OfficeCode+'-'+snd.OfficeName AS SenderOffice,rcv.OfficeCode+'-'+rcv.OfficeName AS ReceiverOffice" +
        //            $", snd_v.VoucherNo AS Sndr_V_No,snd_v.VoucherType AS Sndr_V_Type,rcv_v.VoucherNo AS Rec_V_No,rcv_v.VoucherType AS Rec_V_Type,ho_v.VoucherNo AS HO_V_No,ho_v.VoucherType AS HO_V_Type " +
        //            $",rc.TrxMasterID,rc.Receiver_TrxMasterID,rc.HO_TrxMasterID,SenderOfficeId,ReceiverOfficeId FROM AccReconcile rc INNER JOIN Office snd on rc.SenderOfficeId = snd.OfficeID " +
        //            $"INNER JOIN Office rcv on rc.ReceiverOfficeId = rcv.OfficeID INNER JOIN AccTrxMaster snd_v on rc.TrxMasterID = snd_v.TrxMasterID " +
        //            $"INNER JOIN AccTrxMaster rcv_v on rc.Receiver_TrxMasterID = rcv_v.TrxMasterID LEFT JOIN AccTrxMaster ho_v ON rc.HO_TrxMasterID = ho_v.TrxMasterID WHERE rc.IsActive = 1 and rc.IsReconcile = 1 " +
        //            $"AND (rc.SenderOfficeId={LoginUserOfficeID}	OR rc.ReceiverOfficeId={LoginUserOfficeID}) " +
        //            $"AND rc.TrxDate BETWEEN '{DateFromValue}' AND '{DateToValue}' {adv_search}";


        //            var lst = new gBankerDbContext().Database.SqlQuery<AccReconcileViewModel_dsk>(qry);

        //            var currentPageRecords = lst.Skip(jtStartIndex).Take(jtPageSize);
        //            return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = lst.LongCount(), JsonRequestBehavior.AllowGet });

        //        }
        //        else
        //            return Json(new { Result = "OK", Records = "" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}

        public JsonResult GetReconcileVoucherList(
    string trxDate,
    int jtStartIndex = 0,
    int jtPageSize = 20,
    string jtSorting = null,
    string filterColumn = "",
    string filterValue = "",
    string DateFromValue = "",
    string DateToValue = "")
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@trxDate", string.IsNullOrEmpty(trxDate) ? (object)DBNull.Value : trxDate),
                        new SqlParameter("@jtStartIndex", jtStartIndex),
                        new SqlParameter("@jtPageSize", jtPageSize),
                        new SqlParameter("@jtSorting", string.IsNullOrEmpty(jtSorting) ? (object)DBNull.Value : jtSorting),
                        new SqlParameter("@filterColumn", string.IsNullOrEmpty(filterColumn) ? (object)DBNull.Value : filterColumn),
                        new SqlParameter("@filterValue", string.IsNullOrEmpty(filterValue) ? (object)DBNull.Value : filterValue),
                        new SqlParameter("@DateFromValue", string.IsNullOrEmpty(DateFromValue) ? (object)DBNull.Value : DateFromValue),
                        new SqlParameter("@DateToValue", string.IsNullOrEmpty(DateToValue) ? (object)DBNull.Value : DateToValue),
                        new SqlParameter("@LoginUserOfficeID", LoginUserOfficeID )
                    };

                    var result = db.Database.SqlQuery<AccReconcileViewModel_dsk>(
                        "EXEC GetReconcileVoucherList @trxDate, @jtStartIndex, @jtPageSize, @jtSorting, @filterColumn, @filterValue, @DateFromValue, @DateToValue, @LoginUserOfficeID",
                        parameters.ToArray()
                    ).ToList();

                    var totalRecords = result.Count;
                    var currentPageRecords = result.Skip(jtStartIndex).Take(jtPageSize);

                    return Json(new
                    {
                        Result = "OK",
                        Records = currentPageRecords,
                        TotalRecordCount = totalRecords
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        [HttpGet]
        public JsonResult LoadOfficeBank(string tranType, int officeid)
        {
            string qry = "";

            //if (tranType == "Ca")
            //    qry = $"select distinct ac.* from AccChart ac inner join ApplicationSettings aps on aps.CashBook = ac.AccCode Inner Join  office o ON aps.officeID = o.OfficeID where o.OfficeID = {officeid}";
            //if (tranType == "Ba")
            //    qry = $"select distinct ac.* from AccChart ac inner join  AccChartMapping a ON a.AccID = ac.AccID Inner Join  office o ON a.officeID = o.OfficeID inner join ApplicationSettings aps on aps.BankAccount = ac.SecondLevel where o.OfficeID = {officeid}";
            var lst = new gBankerDbContext().Database.SqlQuery<AccChart>("sp_officeBank '" + tranType + "'," + officeid + "").Select(s => new SelectListItem { Text = s.AccCode + " " + s.AccName, Value = s.AccID.ToString() });
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHOFundAccCode(string acc_code, int OfficeLevel, string TransactionType, string IsReconcile)
        {
            //IEnumerable<AccChart> chart;


            if (IsReconcile == null)
                IsReconcile = "false";
            if (TransactionType == null)
                TransactionType = "Jr";


            var List_ProductViewModel = new gBankerDbContext().Database.SqlQuery<AccChart>("sp_HOFundAccCode '" + acc_code + "','" + SessionHelper.LoggedInOfficeDetail.OfficeCode + "'")
                /*($"select distinct ac.* from AccChartMapping a inner join office o ON a.officeID = o.OfficeID Inner Join " +
                $"AccChart ac ON a.AccID = ac.AccID where ac.IsActive=1 and ac.IsTransaction=1 and OfficeCode = '{SessionHelper.LoggedInOfficeDetail.OfficeCode}' and (ac.AccCode like'%{acc_code}%' or ac.AccName like'%{acc_code}%') AND ac.ModuleID = 8 ORDER BY ac.AccCode")*/;


            var acc = List_ProductViewModel.Where(m => string.Format("{0} - {1}", m.AccCode, m.AccName).ToLower().Contains(acc_code.ToLower())).Select(m1 => new { m1.AccID, AccFullName = string.Format("{0} - {1}", m1.AccCode, m1.AccName) }).ToList();
            return Json(acc, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllVoucherXFund(long AccReconcileID)
        {
            var rec_obj = reConcileService.GetById(AccReconcileID);
            var obj = new FundTransferVoucherViewModel();
            obj.TrxDate = rec_obj.TrxDate.ToString("dd-MMM-yyyy");
            //string qry = $"select o.OfficeCode+' - '+o.OfficeName Office,m.VoucherType,m.VoucherDesc,c.AccCode+' - '+c.AccName AccountHead,d.Debit,d.Credit from AccTrxMaster m " +
            //    $"inner join AccTrxDetail d on m.TrxMasterID = d.TrxMasterID inner join AccChart c on d.AccID = c.AccID inner join Office o on m.OfficeID = o.OfficeID " +
            //    $"where m.IsActive = 1 and d.IsActive = 1 and m.TrxMasterID={rec_obj.TrxMasterID}";

            obj.Sender = new gBankerDbContext().Database.SqlQuery<InnerVoucherDetailsViewModel>($"sp_AllVoucherXFundTransfer {rec_obj.TrxMasterID}").ToList();
            obj.Receiver = new gBankerDbContext().Database.SqlQuery<InnerVoucherDetailsViewModel>($"sp_AllVoucherXFundTransfer {rec_obj.Receiver_TrxMasterID}")
                /*($"select o.OfficeCode+' - '+o.OfficeName Office,m.VoucherType,m.VoucherDesc,c.AccCode+' - '+c.AccName AccountHead,d.Debit,d.Credit from AccTrxMaster m " +
                $"inner join AccTrxDetail d on m.TrxMasterID = d.TrxMasterID inner join AccChart c on d.AccID = c.AccID inner join Office o on m.OfficeID = o.OfficeID " +
                $"where m.IsActive = 1 and d.IsActive = 1 and m.TrxMasterID={rec_obj.Receiver_TrxMasterID}")*/.ToList();
            if ((rec_obj.HO_TrxMasterID ?? 0) > 0)
                obj.HO_vou = new gBankerDbContext().Database.SqlQuery<InnerVoucherDetailsViewModel>($"sp_AllVoucherXFundTransfer {rec_obj.HO_TrxMasterID}")
                /*($"select o.OfficeCode+' - '+o.OfficeName Office,m.VoucherType,m.VoucherDesc,c.AccCode+' - '+c.AccName AccountHead,d.Debit,d.Credit from AccTrxMaster m " +
            $"inner join AccTrxDetail d on m.TrxMasterID = d.TrxMasterID inner join AccChart c on d.AccID = c.AccID inner join Office o on m.OfficeID = o.OfficeID " +
            $"where m.IsActive = 1 and d.IsActive = 1 and m.TrxMasterID={rec_obj.HO_TrxMasterID}")*/.ToList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetFundXAccHead(int sendOffID, string senderOffCode, int revOfficeID, string receiverOfficeCode, int mappingAccid)
        {
            if (senderOffCode == "99999")
            {
                var map_acccode = accChartService.GetById(mappingAccid).ThirdLevel;
                //string qry = $"select top 1 ho.*  from AccMappingForFundTransfer f INNER JOIN AccChart ho on f.BRFundAccCode = ho.AccCode " +
                //    $"where f.IsActive = 1 and ho.IsActive = 1 and ho.IsTransaction = 1  and ho.ModuleID = 8 " +
                //    $"and f.HOFundAccCode = '{map_acccode}'";
                var lst = new gBankerDbContext().Database.SqlQuery<AccChart>($"sp_FundXAccHead '{senderOffCode}','{receiverOfficeCode}','{map_acccode}'");
                if (lst.Any())
                {
                    var _obj = lst.First();
                    return Json(new { status = 1, obj = _obj }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else if (receiverOfficeCode == "99999")
            {
                var map_acccode = accChartService.GetById(mappingAccid).AccCode;
                //string qry = $"select top 2 ho.*  from AccMappingForFundTransfer f INNER JOIN AccChart ho on f.HOFundAccCode = ho.ThirdLevel " +
                //    $"where f.IsActive = 1 and ho.IsActive = 1 and ho.IsTransaction = 1  and ho.ModuleID = 8 " +
                //    $"and ho.AccCode like'%.{senderOffCode}' and f.BRFundAccCode = '{map_acccode}'";
                var lst = new gBankerDbContext().Database.SqlQuery<AccChart>($"sp_FundXAccHead '{senderOffCode}','{receiverOfficeCode}','{map_acccode}'");
                if (lst.Any())
                {
                    if (lst.Where(x => x.AccCode.Contains("." + senderOffCode)).Any())
                    {
                        var _obj = lst.Where(x => x.AccCode.Contains("." + senderOffCode)).First();
                        return Json(new { status = 1, obj = _obj }, JsonRequestBehavior.AllowGet);
                    }
                    else return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var map_acccode = accChartService.GetById(mappingAccid).AccCode;
                //string qry = $"select top 2 ho.*  from AccMappingForFundTransfer f INNER JOIN AccChart ho on f.HOFundAccCode = ho.ThirdLevel " +
                //    $"where f.IsActive = 1 and ho.IsActive = 1 and ho.IsTransaction = 1  and ho.ModuleID = 8 " +
                //    $"and(ho.AccCode like'%.{senderOffCode}' OR ho.AccCode like'%.{receiverOfficeCode}') and f.BRFundAccCode = '{map_acccode}'";

                var lst = new gBankerDbContext().Database.SqlQuery<AccChart>($"sp_FundXAccHead '{senderOffCode}','{receiverOfficeCode}','{map_acccode}'");
                if (lst.Any())
                {
                    var hoID = officeService.GetByOfficeCode("99999").OfficeID;
                    var senderObj = new AccChart(); var receiverObj = new AccChart();
                    if (lst.Where(x => x.AccCode.Contains("." + senderOffCode)).Any())
                        senderObj = lst.Where(x => x.AccCode.Contains("." + senderOffCode)).First();

                    else return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);

                    if (lst.Where(x => x.AccCode.Contains("." + receiverOfficeCode)).Any())
                        receiverObj = lst.Where(x => x.AccCode.Contains("." + receiverOfficeCode)).First();
                    else return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
                    return Json(new { status = 1, ho_ID = hoID, snd_obj = senderObj, rev_obj = receiverObj }, JsonRequestBehavior.AllowGet);
                }
                else return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        //[HttpGet]
        //public JsonResult GetFundXAccHeadForJR(int mappingAccid)
        //{
        //    var map_acccode = accChartService.GetById(mappingAccid).AccCode;

        //    var lst = new gBankerDbContext().Database.SqlQuery<AccChart>($"SELECT TOP 1 ch.*  from AccMappingForFundTransfer f INNER JOIN AccChart ch on f.HOFundAccCode = ch.AccCode where f.IsActive=1 AND ch.IsActive=1 AND f.BRFundAccCode='{map_acccode}'");
        //    if (lst.Any())
        //        return Json(new { status = 1, acc = lst.First() }, JsonRequestBehavior.AllowGet);
        //    else return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
        //}


        public JsonResult GetFundXAccHeadForJR(int mappingAccid)
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var mapAccCode = accChartService.GetById(mappingAccid).AccCode;

                    var parameters = new List<SqlParameter>
                    {
                      new SqlParameter("@MappingAccCode", mapAccCode)
                    };

                    var result = db.Database.SqlQuery<AccChart>(
                        "EXEC GetFundXAccHeadForJR @MappingAccCode",
                        parameters.ToArray()
                    ).ToList();

                    if (result.Any())
                    {
                        return Json(new { status = 1, acc = result.First() }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpGet]
        public JsonResult LoadOffice(string v_type)
        {
            var off = OfficeList(v_type, SessionHelper.LoginUserOfficeID ?? 0);
            return Json(off, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetFundForJornal()
        {
            var br = accChartService.GetByAccCode("304001");
            var _br = new { br.AccID, br.AccCode, br.AccName };
            var ho = accChartService.GetByAccCode("130016." + SessionHelper.LoggedInOfficeDetail.OfficeCode);
            var _ho = new { ho.AccID, ho.AccCode, ho.AccName };
            return Json(new { br_acc = _br, ho_acc = _ho }, JsonRequestBehavior.AllowGet);
        }
        #region Method
        private void ReconcileMapDropDownList(AccVoucherEntryViewModel_dsk model)
        {

            //var type_item = new List<SelectListItem>();
            //type_item.Add(new SelectListItem() { Text = "Debit", Value = "Dr" });
            //type_item.Add(new SelectListItem() { Text = "Credit", Value = "Cr" });
            //model.VoucherTypeList = type_item;

            var transaction_item = new List<SelectListItem>();
            transaction_item.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            //transaction_item.Add(new SelectListItem() { Text = "Cash", Value = "Ca" });
            transaction_item.Add(new SelectListItem() { Text = "Bank(Cheque)", Value = "Ba" });
            //transaction_item.Add(new SelectListItem() { Text = "Bank(Cash)", Value = "Bc" });
            transaction_item.Add(new SelectListItem() { Text = "Journal", Value = "Jr" });

            model.TransactionTypeList = transaction_item;



            List<ReconPurpose> List_ProductViewModel = new List<ReconPurpose>();
            var param = new { OfficeID = LoginUserOfficeID };
            var div_items = ultimateRepostService.GetReconPurposeList(param);

            List_ProductViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new ReconPurpose
            {

                ReconPurposeCode = row.Field<string>("ReconPurposeCode"),
                ReconPurposeName = row.Field<string>("ReconPurposeName")
            }).ToList();

            var viewProduct = List_ProductViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ReconPurposeCode.ToString(),
                Text = x.ReconPurposeCode.ToString() + " " + x.ReconPurposeName.ToString()
            });

            var d_items = new List<SelectListItem>();
            d_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            d_items.AddRange(viewProduct);
            model.ReconPurposeList = d_items;


        }
        //public IEnumerable<SelectListItem> ReffNoList()
        //{
        //    var reConcileList = reConcileService.GetAll().Where(b => b.IsActive == true && b.IsReconcile == false && b.ReceiverOfficeId == SessionHelper.LoginUserOfficeID);
        //    var viewReconcile = reConcileList.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.ReffNo.ToString(),
        //        Text = x.ReffNo.ToString()

        //    });
        //    var reConcileItem = new List<SelectListItem>();
        //    reConcileItem.Add(new SelectListItem() { Text = "Select Refference No", Value = "" });
        //    reConcileItem.AddRange(viewReconcile);
        //    return reConcileItem;

        //}


        public IEnumerable<SelectListItem> ReffNoList()
        {
            try
            {
                using (var db = new gBankerDbContext())
                {
                    var receiverOfficeId = SessionHelper.LoginUserOfficeID;

                    // Call the stored procedure
                    var result = db.Database.SqlQuery<string>(
                        "EXEC GetReffNoList @ReceiverOfficeId",
                        new SqlParameter("@ReceiverOfficeId", receiverOfficeId)
                    ).ToList();

                    // Convert the result into SelectListItem objects
                    var reConcileItem = new List<SelectListItem>
                    {
                        new SelectListItem { Text = "Select Refference No", Value = "" }
                    };

                    reConcileItem.AddRange(result.Select(reffNo => new SelectListItem
                    {
                        Value = reffNo,
                        Text = reffNo
                    }));

                    return reConcileItem;
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                throw new Exception("An error occurred while retrieving the reference number list.", ex);
            }
        }



        //public IEnumerable<SelectListItem> OfficeList(string v_type, int officeId)
        //{
        //    var officeItem = new List<SelectListItem>();
        //    officeItem.Add(new SelectListItem() { Text = "Select Office", Value = "" });
        //    if (v_type == "" || v_type == "Ba")
        //    {
        //        var officeList = officeService.GetAll().Where(b => b.IsActive == true && b.OfficeID != officeId && b.OfficeLevel == 4);
        //        var viewOffice = officeList.Select(x => x).OrderBy(x => x.OfficeCode).ToList().Select(x => new SelectListItem
        //        {
        //            Value = x.OfficeID.ToString(),
        //            Text = x.OfficeCode + ' ' + x.OfficeName.ToString()
        //        });
        //        officeItem.AddRange(viewOffice);
        //    }
        //    else
        //    {
        //        var officeList = new List<Office>();
        //        if (officeService.GetMany(x => x.OfficeID == officeId && x.OfficeCode.Contains("999")).Any())
        //            officeList = officeService.GetMany(x => x.OfficeID != officeId && !x.OfficeCode.Contains("999") && x.OfficeLevel == 4).ToList();
        //        else
        //            officeList = officeService.GetMany(x => x.OfficeID != officeId && x.OfficeCode.Contains("999")).ToList();

        //        var viewOffice = officeList.Select(x => x).OrderBy(x => x.OfficeCode).ToList().Select(x => new SelectListItem
        //        {
        //            Value = x.OfficeID.ToString(),
        //            Text = x.OfficeCode + ' ' + x.OfficeName.ToString()

        //        });
        //        officeItem.AddRange(viewOffice);

        //    }



        //    return officeItem;

        //}

        public IEnumerable<SelectListItem> OfficeList(string v_type, int officeId)
        {
            using (var db = new gBankerDbContext())
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@V_Type", v_type ?? string.Empty),
                    new SqlParameter("@OfficeId", officeId)
                };

                var result = db.Database.SqlQuery<OfficeListItem>(
                    "EXEC GetOfficeList @V_Type, @OfficeId",
                    parameters.ToArray()
                ).ToList();

                var officeItem = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Select Office", Value = "" }
                };

                officeItem.AddRange(result.Select(x => new SelectListItem
                {
                    Value = x.Value,
                    Text = x.Text
                }));

                return officeItem;
            }
        }




        //public string GenerateNewVoucher(int offc_id)
        //{
        //    string latest_voucher = "";
        //    var v = accLastVoucherService.GetByOffcId(offc_id);
        //    if (v == null || v.VoucherNo == "") // if there is no voucher for this office
        //    {
        //        latest_voucher = "1-" + DateTime.Now.Year.ToString();
        //        int new_voucher = Convert.ToInt32(latest_voucher.Substring(0, latest_voucher.Length - 5)) + 1;
        //        var crt = new AccLastVoucher();
        //        crt.OfficeID = offc_id;
        //        crt.VoucherNo = new_voucher.ToString() + "-" + DateTime.Now.Year.ToString();
        //        accLastVoucherService.Create(crt);
        //    }
        //    else // collect last voucher no
        //    {
        //        latest_voucher = v.VoucherNo;
        //        int VoucherId = v.LastVoucherID;
        //        int new_voucher = Convert.ToInt32(latest_voucher.Substring(0, latest_voucher.Length - 5)) + 1;
        //        var updt = new AccLastVoucher();
        //        updt = accLastVoucherService.GetByLastVoucherId(Convert.ToInt32(VoucherId));
        //        //updt.OfficeID = Convert.ToInt32(offc_id);
        //        updt.VoucherNo = new_voucher.ToString() + "-" + DateTime.Now.Year.ToString();
        //        accLastVoucherService.Update(updt);
        //    }

        //    return latest_voucher;
        //}


        public string GenerateNewVoucher(int offc_id)
        {
            using (var db = new gBankerDbContext())
            {
                var newVoucherParam = new SqlParameter
                {
                    ParameterName = "@NewVoucher",
                    SqlDbType = SqlDbType.NVarChar,
                    Size = 50,
                    Direction = ParameterDirection.Output
                };

                db.Database.ExecuteSqlCommand(
                    "EXEC GenerateNewVoucher @OfficeID, @NewVoucher OUTPUT",
                    new SqlParameter("@OfficeID", offc_id),
                    newVoucherParam
                );

                return newVoucherParam.Value.ToString();
            }
        }


        #endregion
    }

    public class OfficeListItem
    {

        public string Value { get; set; }
        public string Text { get; set; }
    }

    public class PostAccountMappingResult
    {
        public int status { get; set; }
        public string message { get; set; }
    }


}