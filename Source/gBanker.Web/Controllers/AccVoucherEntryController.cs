using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Service;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using gBanker.Web.Helpers;
using gBanker.Service.ReportServies;
using System.Text;

namespace gBanker.Web.Controllers
{
    public class AccVoucherEntryController : BaseController
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
        private readonly bool CustomIsReconsile;
       
        public AccVoucherEntryController(IAccTrxMasterService accTrxMasterService, IAccTrxDetailService accTrxDetailService, IAccChartService accChartService, IAccLastVoucherService accLastVoucherService, IProcessInfoService processInfoService, IOfficeService officeService, IAccReportService accReportService, IApplicationSettingsService applicationSettingsService, IWeeklyReportService weeklyReportService,IAccReconcileService reConcileService,IUltimateReportService ultimateRepostService)
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
           
        }
        string sessionName = "STR_VoucherDataTable" + SessionHelper.LoginUserEmployeeID.ToString();
        StringBuilder sb = new StringBuilder();
        #endregion

        #region Methods
        private static double totalDebit;
        private static double totalCredit;
        public JsonResult GetVoucherList(string trxDate, int jtStartIndex = 0, int jtPageSize = 20, string jtSorting = null)
        {
            try
            {
               

                if (trxDate != "")
                {
                    List<AccTrxMasterViewModel> List_AccTrxMasterViewModel = new List<AccTrxMasterViewModel>();
                    var param = new { OrgID = LoggedInOrganizationID, OfficeID = LoginUserOfficeID, TrxDate = TransactionDate };
                    var empList = accReportService.GetAccDataForReport(param, "Proc_Get_AccountDetails");

                    List_AccTrxMasterViewModel = empList.Tables[0].AsEnumerable()
                    .Select(row => new AccTrxMasterViewModel
                    {
                        TrxMasterID = row.Field<long>("TrxMasterID"),
                        VoucherNo = row.Field<string>("VoucherNo"),
                        TrxDtMsg = row.Field<string>("TrxDtMsg"),
                        VoucherType = row.Field<string>("VoucherType"),
                        VoucherDesc = row.Field<string>("VoucherDesc"),
                        Reference = row.Field<string>("Reference"),
                        TotDebit = row.Field<decimal>("TotDebit"),
                        TotCredit = row.Field<decimal>("TotCredit"),
                        IsAutoVoucherMsg = row.Field<string>("IsAutoVoucherMsg"),
                        IsReconcileVoucherMSG= row.Field<string>("IsReconcileVoucherMSG")

                    }).ToList();
                    var currentPageRecords = List_AccTrxMasterViewModel.Skip(jtStartIndex).Take(jtPageSize);
                    return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_AccTrxMasterViewModel.LongCount(), JsonRequestBehavior.AllowGet });


                }
                else
                {
                    return Json(new { Result = "OK", Records = "" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        public JsonResult CheckEnoughCreditBalance(string account, string creditBalance)
        {
            try
            {
                var credit_Balance = Convert.ToDecimal(creditBalance);
                var param = new { @OfficeID = SessionHelper.LoginUserOfficeID, @AccID = account, @TrxDate = TransactionDate.ToShortDateString() };
                var result_set = ultimateRepostService.GetDataWithParameter(param, "Proc_Check_Enough_DebitBalance_For_VoucherCreate");
                decimal debit_Balance = (decimal)result_set.Tables[0].Rows[0]["DebitBalance"];

                if (credit_Balance <= debit_Balance)
                {
                    return Json(new { Result = "OK", Message = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Result = "ERROR",
                        Message = "This account doesn't have enough debit balance. <br/>" +
                        $"Debit balance: {debit_Balance} Tk only"
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetZoneList(string Freq_id)
        {
            List<OfficeViewModel> List_MemberPassBookRegisterViewModel = new List<OfficeViewModel>();
            var param = new {  OfficeID = LoginUserOfficeID };
            var div_items = ultimateRepostService.GetZoneList(param);
            var viewProduct = div_items.Tables[0].AsEnumerable().Select(row => new SelectListItem
            {
                Value = row.Field<Int16>("OfficeID").ToString(),
                Text = row.Field<string>("OfficeCode") + ' ' + row.Field<string>("OfficeName")
            });
            var d_items = new List<SelectListItem>();
            d_items.Add(new SelectListItem() { Text = "Select All", Value = "0" });
            d_items.AddRange(viewProduct);
            return Json(d_items, JsonRequestBehavior.AllowGet);
        }
      
        public JsonResult GetTotal()
        {
            try
            {
                return Json(totalDebit, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("ERROR", JsonRequestBehavior.AllowGet);
            }

        }// End Function

        public JsonResult GetReconcileVoucherList(string trxDate, int jtStartIndex = 0, int jtPageSize = 20, string jtSorting = null, string filterColumn="", string vOfficeID="",string filterValuePurpose="",string filterValueVoucherType="", string filterValue="",string DateFromValue="",string DateToValue="")
        {
            try
            {


                if (trxDate != "")
                {
                    List<AccTrxMasterViewModel> List_AccTrxMasterViewModel = new List<AccTrxMasterViewModel>();
                    var param = new { OrgID = LoggedInOrganizationID, OfficeID = LoginUserOfficeID, TrxDate = TransactionDate, filterColumnName = filterColumn, filterValue = filterValue, DateFromValue=DateFromValue, DateToValue=DateToValue, vOfficeID=vOfficeID, filterValuePurpose= filterValuePurpose, filterValueVoucherType = filterValueVoucherType };
                    var empList = accReportService.GetAccDataForReport(param, "Proc_Get_AccountDetailsReconcileFilterZoneWise");

                    List_AccTrxMasterViewModel = empList.Tables[0].AsEnumerable()
                    .Select(row => new AccTrxMasterViewModel
                    {
                        TrxMasterID = row.Field<long>("TrxMasterID"),
                        VoucherNo = row.Field<string>("VoucherNo"),
                        TrxDtMsg = row.Field<string>("TrxDtMsg"),
                        VoucherType = row.Field<string>("VoucherType"),
                        VoucherDesc = row.Field<string>("VoucherDesc"),
                        Reference = row.Field<string>("Reference"),
                        TotDebit = row.Field<decimal>("TotDebit"),
                        TotCredit = row.Field<decimal>("TotCredit"),
                        IsAutoVoucherMsg = row.Field<string>("IsAutoVoucherMsg"),
                        IsReconcile = row.Field<int>("IsReconcile"),
                        Purpose= row.Field<string>("Purpose")

                    }).ToList();
                    totalDebit = 0.0;
                    if (List_AccTrxMasterViewModel.Count() > 0)
                    {
                        foreach (var v in List_AccTrxMasterViewModel)
                        {
                            totalDebit = totalDebit + Convert.ToDouble(v.TotDebit);
                        }

                    }
                    var currentPageRecords = List_AccTrxMasterViewModel.Skip(jtStartIndex).Take(jtPageSize);
                    return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_AccTrxMasterViewModel.LongCount(), JsonRequestBehavior.AllowGet });


                }
                else
                {
                    return Json(new { Result = "OK", Records = "" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        public JsonResult ViewTrxDetail(string trxMasterId)
        {

            try
            {
                List<AccVoucherEntryViewModel> List_AccVoucherEntryViewModel = new List<AccVoucherEntryViewModel>();

                var param = new { TrxMasterID = trxMasterId };
                var VoucherList = accReportService.GetVoucherDetail(param, "SP_ViewTrxDetail");

                List_AccVoucherEntryViewModel = VoucherList.Tables[0].AsEnumerable()
                .Select(row => new AccVoucherEntryViewModel()
                {
                    AccFullName = row.Field<string>("AccName"),
                    Narration = row.Field<string>("Narration"),
                    Debit = row.Field<decimal>("Debit"),
                    Credit = row.Field<decimal>("Credit"),
                    AccID = row.Field<int>("AccID"),
                    TrxDetailsID = row.Field<long>("TrxDetailsID"),
                    AccMode = row.Field<string>("AccMode")
                }).ToList();

                return Json(List_AccVoucherEntryViewModel, JsonRequestBehavior.AllowGet);



                var viewDetail = accTrxDetailService.GetByTrxMasterId(Convert.ToInt64(trxMasterId));
                //List<AccVoucherEntryViewModel> List_AccVoucherEntryViewModel = new List<AccVoucherEntryViewModel>();
                //foreach (var vd in viewDetail)
                //{
                //    var Acc_Name = accChartService.GetById(Convert.ToInt32(vd.AccID)).AccName.ToString();
                //    var details = new AccVoucherEntryViewModel() { AccFullName = Acc_Name, Narration = vd.Narration, Debit = vd.Debit, Credit = vd.Credit, AccID = vd.AccID, TrxDetailsID = vd.TrxDetailsID, AccMode = "U" };
                //    List_AccVoucherEntryViewModel.Add(details);
                //}
                //return Json(List_AccVoucherEntryViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }


            //try
            //{
            //    var viewDetail = accTrxDetailService.GetByTrxMasterId(Convert.ToInt64(trxMasterId));
            //    List<AccVoucherEntryViewModel> List_AccVoucherEntryViewModel = new List<AccVoucherEntryViewModel>();
            //    foreach (var vd in viewDetail)
            //    {
            //        var Acc_Name = accChartService.GetById(Convert.ToInt32(vd.AccID)).AccName.ToString();
            //        var details = new AccVoucherEntryViewModel() { AccFullName = Acc_Name, Narration = vd.Narration, Debit = vd.Debit, Credit = vd.Credit, AccID = vd.AccID, TrxDetailsID = vd.TrxDetailsID, AccMode = "U" };
            //        List_AccVoucherEntryViewModel.Add(details);
            //    }
            //    return Json(List_AccVoucherEntryViewModel, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    return Json(new { Result = "ERROR", Message = ex.Message });
            //}
        }
        private void MapDropDownList(AccVoucherEntryViewModel model)
        {
          
            var type_item = new List<SelectListItem>();
            type_item.Add(new SelectListItem() { Text = "Debit", Value = "Dr" });
            type_item.Add(new SelectListItem() { Text = "Credit", Value = "Cr" });            
            model.VoucherTypeList = type_item;

            var transaction_item = new List<SelectListItem>();
            transaction_item.Add(new SelectListItem() { Text = "Cash", Value = "Ca" });
            transaction_item.Add(new SelectListItem() { Text = "Bank(Cheque)", Value = "Ba" });
            transaction_item.Add(new SelectListItem() { Text = "Bank(Cash)", Value = "Bc" });
            transaction_item.Add(new SelectListItem() { Text = "Journal", Value = "Jr" });
            transaction_item.Add(new SelectListItem() { Text = "BankToBank", Value = "BB" });

            model.TransactionTypeList = transaction_item;

            //var allpurpose = purposeService.SearchPurpose(Convert.ToInt32(LoggedInOrganizationID));

            //var viewPurpose = allpurpose.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.PurposeCode, m.PurposeName), Value = m.PurposeID.ToString() });

            //model.purposeListItems = viewPurpose;

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
        private void ReconcileMapDropDownList(AccVoucherEntryViewModel model)
        {

            var type_item = new List<SelectListItem>();
            type_item.Add(new SelectListItem() { Text = "Debit", Value = "Dr" });
            type_item.Add(new SelectListItem() { Text = "Credit", Value = "Cr" });
            model.VoucherTypeList = type_item;

            var transaction_item = new List<SelectListItem>();
            transaction_item.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            transaction_item.Add(new SelectListItem() { Text = "Cash", Value = "Ca" });
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
        public JsonResult GetNewVoucher(string offc_id)
        {
            string latest_voucher = "";
            var v = accLastVoucherService.GetByOffcId(Convert.ToInt32(offc_id));
            if (v == null || v.VoucherNo == "") // if there is no voucher for this office
            {
                latest_voucher = "1-" + DateTime.Now.Year.ToString();
                int new_voucher = Convert.ToInt32(latest_voucher.Substring(0, latest_voucher.Length - 5)) + 1;
                var crt = new AccLastVoucher();
                crt.OfficeID = Convert.ToInt32(offc_id);
                crt.VoucherNo = new_voucher.ToString() + "-" + DateTime.Now.Year.ToString();
                accLastVoucherService.Create(crt);
            }
            else // collect last voucher no
            {
                latest_voucher = v.VoucherNo;
                int VoucherId = v.LastVoucherID;
                int new_voucher = Convert.ToInt32(latest_voucher.Substring(0, latest_voucher.Length - 5)) + 1;
                var updt = new AccLastVoucher();
                updt = accLastVoucherService.GetByLastVoucherId(Convert.ToInt32(VoucherId));
                //updt.OfficeID = Convert.ToInt32(offc_id);
                updt.VoucherNo = new_voucher.ToString() + "-" + DateTime.Now.Year.ToString();
                accLastVoucherService.Update(updt);
            }

            return Json(latest_voucher, JsonRequestBehavior.AllowGet);
        }
        public string GenerateNewVoucher(string offc_id)
        {
            string latest_voucher = "";
            var v = accLastVoucherService.GetByOffcId(Convert.ToInt32(offc_id));
            if (v == null || v.VoucherNo == "") // if there is no voucher for this office
            {
                latest_voucher = "1-" + DateTime.Now.Year.ToString();
                int new_voucher = Convert.ToInt32(latest_voucher.Substring(0, latest_voucher.Length - 5)) + 1;
                var crt = new AccLastVoucher();
                crt.OfficeID = Convert.ToInt32(offc_id);
                crt.VoucherNo = new_voucher.ToString() + "-" + DateTime.Now.Year.ToString();
                accLastVoucherService.Create(crt);
            }
            else // collect last voucher no
            {
                latest_voucher = v.VoucherNo;
                int VoucherId = v.LastVoucherID;
                int new_voucher = Convert.ToInt32(latest_voucher.Substring(0, latest_voucher.Length - 5)) + 1;
                var updt = new AccLastVoucher();
                updt = accLastVoucherService.GetByLastVoucherId(Convert.ToInt32(VoucherId));
                //updt.OfficeID = Convert.ToInt32(offc_id);
                updt.VoucherNo = new_voucher.ToString() + "-" + DateTime.Now.Year.ToString();
                accLastVoucherService.Update(updt);
            }

            return latest_voucher;
        }
        public JsonResult GetAccCode(string acc_code, int OfficeLevel, string TransactionType,string IsReconcile)
        {
            IEnumerable<AccChart> chart;

            //     var BankCode = accChartService.GetByAccCode(applicationSettingsService.GetAll().Where(c => c.OfficeID == LoginUserOfficeID).FirstOrDefault().BankAccount);
            //if (IsReconcile== "true")
            //     {
            //         if (TransactionType == "Bc")
            //         {
            //             chart = accChartService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID && m.IsTransaction == true && m.IsActive == true && m.OfficeLevel >= OfficeLevel && m.ModuleID == 8 && m.SecondLevel == BankCode.SecondLevel).ToList();
            //         }
            //         else if (TransactionType == "Ca")
            //         {
            //             chart = accChartService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID && m.IsTransaction == true && m.IsActive == true && m.OfficeLevel >= OfficeLevel && m.ModuleID == 8 && m.SecondLevel != BankCode.SecondLevel).ToList();
            //         }
            //         else if (TransactionType == "Ba")
            //         {
            //             chart = accChartService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID && m.IsTransaction == true && m.IsActive == true && m.OfficeLevel >= OfficeLevel && m.ModuleID == 8 && m.SecondLevel != BankCode.SecondLevel).ToList();
            //         }

            //         else
            //         {
            //             chart = accChartService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID && m.IsTransaction == true && m.IsActive == true && m.OfficeLevel >= OfficeLevel && m.ModuleID == 1).ToList();
            //         }
            //     }
            //else
            //     {
            //         if (TransactionType == "Bc")
            //         {
            //             chart = accChartService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID && m.IsTransaction == true && m.IsActive == true && m.OfficeLevel >= OfficeLevel && m.ModuleID == 1 && m.SecondLevel == BankCode.SecondLevel).ToList();
            //         }
            //         else if (TransactionType == "Ca")
            //         {
            //             chart = accChartService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID && m.IsTransaction == true && m.IsActive == true && m.OfficeLevel >= OfficeLevel && m.ModuleID == 1 && m.SecondLevel != BankCode.SecondLevel).ToList();
            //         }
            //         else if (TransactionType == "Ba")
            //         {
            //             chart = accChartService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID && m.IsTransaction == true && m.IsActive == true && m.OfficeLevel >= OfficeLevel && m.ModuleID == 1 && m.SecondLevel != BankCode.SecondLevel).ToList();
            //         }

            //         else
            //         {
            //             chart = accChartService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID && m.IsTransaction == true && m.IsActive == true && m.OfficeLevel >= OfficeLevel && m.ModuleID == 1).ToList();
            //         }
            //     }

            if (IsReconcile == null)
            {
                IsReconcile = "false";
            }
            if (TransactionType == null)
            {
                TransactionType = "Jr";
            }
            List<AccChartViewModel> List_ProductViewModel = new List<AccChartViewModel>();
            var param = new { OfficeID = LoginUserOfficeID, OfficeLevel = OfficeLevel, TransactionType = TransactionType, IsReconcile = IsReconcile, OrgID =LoggedInOrganizationID};
            var div_items = ultimateRepostService.GetAccCodeListAccordingToOffice(param);

            List_ProductViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new AccChartViewModel
            {
                AccID = row.Field<int>("AccID"),
                AccCode = row.Field<string>("AccCode"),
                AccName = row.Field<string>("AccName"),
               // AccFullName = row.Field<string>("AccFullName")
            }).ToList();
            var acc = List_ProductViewModel.Where(m => string.Format("{0} - {1}", m.AccCode, m.AccName).ToLower().Contains(acc_code.ToLower())).Select(m1 => new { m1.AccID, AccFullName = string.Format("{0} - {1}", m1.AccCode, m1.AccName) }).ToList();
            return Json(acc, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBankAccCode(string acc_code, int OfficeLevel, string TransactionType)
        {

            List<AccChartViewModel> List_ProductViewModel = new List<AccChartViewModel>();
            var param = new { OfficeID = LoginUserOfficeID, OfficeLevel = OfficeLevel, TransactionType = TransactionType, OrgID = LoggedInOrganizationID };
            var div_items = ultimateRepostService.GetBankAccCodeListAccordingToOffice(param);

            List_ProductViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new AccChartViewModel
            {
                AccID = row.Field<int>("AccID"),
                AccCode = row.Field<string>("AccCode"),
                AccName = row.Field<string>("AccName"),
                // AccFullName = row.Field<string>("AccFullName")
            }).ToList();

            var acc = List_ProductViewModel.Where(m => string.Format("{0} - {1}", m.AccCode, m.AccName).ToLower().Contains(acc_code.ToLower())).Select(m1 => new { m1.AccID, AccFullName = string.Format("{0} - {1}", m1.AccCode, m1.AccName) }).ToList();
            return Json(acc, JsonRequestBehavior.AllowGet);

  
        }
        public JsonResult AddVoucherItemReconcile(Dictionary<string, string> allTrx, List<string> allVoucherId, string NewAccFullName, string NewNarration, string NewDebit, string NewCredit, string NewAccID, string VoucherType, string TransactionType, string BAccid, string IsReconcileData)
        {
            try
            {
                List<AccVoucherEntryViewModel> List_AccVoucherEntryViewModel = new List<AccVoucherEntryViewModel>();

                var trx = allTrx;

                var trxId = 1;
                var VoucherTrxIds = allVoucherId.Where(w => int.TryParse(w, out trxId));

                var VoucherTrxDetail = new List<AccTrxDetail>();
                foreach (var id in VoucherTrxIds)
                {
                    var txtAccFullNameId = "txtAccFullNameId" + id;
                    var txtNarrationId = "txtNarrationId" + id;
                    var txtAccId = "txtAccId" + id;
                    var txtCreditId = "txtCreditId" + id;
                    var txtDebitId = "txtDebitId" + id;
                    var txtAccModeId = "txtAccModeId" + id;
                    var txtTrxDetailsID = "txtTrxDetailsID" + id;
                    var txtIsReconcileDataID = "txtIsReconcileDataID" + id;

                    var IsReconcileDataID = "";
                    var AccFullName = "";
                    var Narration = "";
                    var AccMode = "";
                    int AccID = 0;
                    decimal lblDebit = 0;
                    decimal lblCredit = 0;
                    long TrxDetailsID = 0;

                    if (allTrx.ContainsKey(txtAccFullNameId))
                        AccFullName = allTrx[txtAccFullNameId];
                    if (allTrx.ContainsKey(txtNarrationId))
                        Narration = allTrx[txtNarrationId];
                    if (allTrx.ContainsKey(txtAccModeId))
                        AccMode = allTrx[txtAccModeId];
                    if (allTrx.ContainsKey(txtAccId))
                        int.TryParse(allTrx[txtAccId], out AccID);
                    if (allTrx.ContainsKey(txtDebitId))
                        decimal.TryParse(allTrx[txtDebitId], out lblDebit);
                    if (allTrx.ContainsKey(txtCreditId))
                        decimal.TryParse(allTrx[txtCreditId], out lblCredit);
                    if (allTrx.ContainsKey(txtTrxDetailsID))
                        long.TryParse(allTrx[txtTrxDetailsID], out TrxDetailsID);
                    if (allTrx.ContainsKey(txtIsReconcileDataID))
                        IsReconcileDataID = allTrx[txtIsReconcileDataID];
                    var details = new AccVoucherEntryViewModel() { AccFullName = AccFullName, Narration = Narration, Debit = lblDebit, Credit = lblCredit, AccID = AccID, AccMode = AccMode, TrxDetailsID = TrxDetailsID, IsReconcileDataID = IsReconcileDataID };
                    List_AccVoucherEntryViewModel.Add(details);
                }
                var NewDetail = new AccVoucherEntryViewModel() { AccFullName = NewAccFullName, Narration = NewNarration, Debit = Convert.ToDecimal(NewDebit), Credit = Convert.ToDecimal(NewCredit), AccID = Convert.ToInt32(NewAccID), AccMode = "S", TrxDetailsID = 0, IsReconcileDataID = IsReconcileData };
                List_AccVoucherEntryViewModel.Add(NewDetail);

                if (TransactionType == "Bc")
                {
                    var cashCode = accChartService.GetByAccCode(applicationSettingsService.GetAll().Where(c => c.OfficeID == LoginUserOfficeID).FirstOrDefault().CashBook);
                    if (VoucherType == "Dr")
                    {
                        var AddRowDetail = new AccVoucherEntryViewModel() { AccFullName = cashCode.AccCode + ", " + cashCode.AccName, Narration = NewNarration, Debit = 0, Credit = Convert.ToDecimal(NewDebit), AccID = cashCode.AccID, AccMode = "S", TrxDetailsID = 0, IsReconcileDataID = "N" };
                        List_AccVoucherEntryViewModel.Add(AddRowDetail);
                    }
                    else if (VoucherType == "Cr")
                    {
                        var AddRowDetail = new AccVoucherEntryViewModel() { AccFullName = cashCode.AccCode + ", " + cashCode.AccName, Narration = NewNarration, Debit = Convert.ToDecimal(NewCredit), Credit = 0, AccID = cashCode.AccID, AccMode = "S", TrxDetailsID = 0, IsReconcileDataID = "N" };
                        List_AccVoucherEntryViewModel.Add(AddRowDetail);
                    }
                }
                if (TransactionType == "Ba" || TransactionType == "BB")
                {
                    var cashCode = accChartService.GetByAccID(Convert.ToInt32(BAccid));
                    if (VoucherType == "Dr")
                    {
                        var AddRowDetail = new AccVoucherEntryViewModel() { AccFullName = cashCode.AccCode + ", " + cashCode.AccName, Narration = NewNarration, Debit = 0, Credit = Convert.ToDecimal(NewDebit), AccID = cashCode.AccID, AccMode = "S", TrxDetailsID = 0, IsReconcileDataID = "N" };
                        List_AccVoucherEntryViewModel.Add(AddRowDetail);
                    }
                    else if (VoucherType == "Cr")
                    {
                        var AddRowDetail = new AccVoucherEntryViewModel() { AccFullName = cashCode.AccCode + ", " + cashCode.AccName, Narration = NewNarration, Debit = Convert.ToDecimal(NewCredit), Credit = 0, AccID = cashCode.AccID, AccMode = "S", TrxDetailsID = 0, IsReconcileDataID = "N" };
                        List_AccVoucherEntryViewModel.Add(AddRowDetail);
                    }
                }
                return Json(List_AccVoucherEntryViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        public JsonResult AddVoucherItem(Dictionary<string, string> allTrx, List<string> allVoucherId, string NewAccFullName, string NewNarration, string NewDebit, string NewCredit, string NewAccID, string VoucherType, string TransactionType, string BAccid, string IsReconcileData)
        {
            try
            {
                List<AccVoucherEntryViewModel> List_AccVoucherEntryViewModel = new List<AccVoucherEntryViewModel>();

                var trx = allTrx;

                var trxId = 1;
                var VoucherTrxIds = allVoucherId.Where(w => int.TryParse(w, out trxId));

                var VoucherTrxDetail = new List<AccTrxDetail>();
                foreach (var id in VoucherTrxIds)
                {
                    var txtAccFullNameId = "txtAccFullNameId" + id;
                    var txtNarrationId = "txtNarrationId" + id;
                    var txtAccId = "txtAccId" + id;
                    var txtCreditId = "txtCreditId" + id;
                    var txtDebitId = "txtDebitId" + id;
                    var txtAccModeId = "txtAccModeId" + id;
                    var txtTrxDetailsID = "txtTrxDetailsID" + id;
                    var txtIsReconcileDataID = "txtIsReconcileDataID" + id;

                    var IsReconcileDataID = "";
                    var AccFullName = "";
                    var Narration = "";
                    var AccMode = "";
                    int AccID = 0;
                    decimal lblDebit = 0;
                    decimal lblCredit = 0;
                    long TrxDetailsID = 0;

                    if (allTrx.ContainsKey(txtAccFullNameId))
                        AccFullName = allTrx[txtAccFullNameId];
                    if (allTrx.ContainsKey(txtNarrationId))
                        Narration = allTrx[txtNarrationId];
                    if (allTrx.ContainsKey(txtAccModeId))
                        AccMode = allTrx[txtAccModeId];
                    if (allTrx.ContainsKey(txtAccId))
                        int.TryParse(allTrx[txtAccId], out AccID);
                    if (allTrx.ContainsKey(txtDebitId))
                        decimal.TryParse(allTrx[txtDebitId], out lblDebit);
                    if (allTrx.ContainsKey(txtCreditId))
                        decimal.TryParse(allTrx[txtCreditId], out lblCredit);
                    if (allTrx.ContainsKey(txtTrxDetailsID))
                        long.TryParse(allTrx[txtTrxDetailsID], out TrxDetailsID);
                    if (allTrx.ContainsKey(txtIsReconcileDataID))
                        IsReconcileDataID = allTrx[txtIsReconcileDataID];
                    var details = new AccVoucherEntryViewModel() { AccFullName = AccFullName, Narration = Narration, Debit = lblDebit, Credit = lblCredit, AccID = AccID, AccMode = AccMode, TrxDetailsID = TrxDetailsID, IsReconcileDataID = IsReconcileDataID };
                    List_AccVoucherEntryViewModel.Add(details);
                }
                var NewDetail = new AccVoucherEntryViewModel() { AccFullName = NewAccFullName, Narration = NewNarration, Debit = Convert.ToDecimal(NewDebit), Credit = Convert.ToDecimal(NewCredit), AccID = Convert.ToInt32(NewAccID), AccMode = "S", TrxDetailsID = 0, IsReconcileDataID = IsReconcileData };
                List_AccVoucherEntryViewModel.Add(NewDetail);

                if (TransactionType == "Bc" )
                {
                    var cashCode = accChartService.GetByAccCode(applicationSettingsService.GetAll().Where(c => c.OfficeID == LoginUserOfficeID).FirstOrDefault().CashBook);
                    if (VoucherType == "Dr")
                    {
                        var AddRowDetail = new AccVoucherEntryViewModel() { AccFullName = cashCode.AccCode + ", " + cashCode.AccName, Narration = NewNarration, Debit = 0, Credit = Convert.ToDecimal(NewDebit), AccID = cashCode.AccID, AccMode = "S", TrxDetailsID = 0, IsReconcileDataID = "N" };
                        List_AccVoucherEntryViewModel.Add(AddRowDetail);
                    }
                    else if (VoucherType == "Cr")
                    {
                        var AddRowDetail = new AccVoucherEntryViewModel() { AccFullName = cashCode.AccCode + ", " + cashCode.AccName, Narration = NewNarration, Debit = Convert.ToDecimal(NewCredit), Credit = 0, AccID = cashCode.AccID, AccMode = "S", TrxDetailsID = 0, IsReconcileDataID = "N" };
                        List_AccVoucherEntryViewModel.Add(AddRowDetail);
                    }
                }
                if (TransactionType == "Ba" || TransactionType == "BB")
                {
                    var cashCode = accChartService.GetByAccID(Convert.ToInt32(BAccid));
                    if (VoucherType == "Dr")
                    {
                        var AddRowDetail = new AccVoucherEntryViewModel() { AccFullName = cashCode.AccCode + ", " + cashCode.AccName, Narration = NewNarration, Debit = 0, Credit = Convert.ToDecimal(NewDebit), AccID = cashCode.AccID, AccMode = "S", TrxDetailsID = 0, IsReconcileDataID = "N" };
                        List_AccVoucherEntryViewModel.Add(AddRowDetail);
                    }
                    else if (VoucherType == "Cr")
                    {
                        var AddRowDetail = new AccVoucherEntryViewModel() { AccFullName = cashCode.AccCode + ", " + cashCode.AccName, Narration = NewNarration, Debit = Convert.ToDecimal(NewCredit), Credit = 0, AccID = cashCode.AccID, AccMode = "S", TrxDetailsID = 0, IsReconcileDataID = "N" };
                        List_AccVoucherEntryViewModel.Add(AddRowDetail);
                    }
                }
                return Json(List_AccVoucherEntryViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        public JsonResult DeleteTrxDetailReconcile(Dictionary<string, string> allTrx, List<string> allVoucherId, string row_index)
        {
            try
            {
                List<AccVoucherEntryViewModel> List_AccVoucherEntryViewModel = new List<AccVoucherEntryViewModel>();

                var trx = allTrx;

                var trxId = 1;
                var VoucherTrxIds = allVoucherId.Where(w => int.TryParse(w, out trxId));

                var VoucherTrxDetail = new List<AccTrxDetail>();
                foreach (var id in VoucherTrxIds)
                {
                    var txtAccFullNameId = "txtAccFullNameId" + id;
                    var txtNarrationId = "txtNarrationId" + id;
                    var txtAccId = "txtAccId" + id;
                    var txtCreditId = "txtCreditId" + id;
                    var txtDebitId = "txtDebitId" + id;
                    var txtAccModeId = "txtAccModeId" + id;
                    var txtTrxDetailsID = "txtTrxDetailsID" + id;

                    var AccFullName = "";
                    var Narration = "";
                    var AccMode = "";
                    int AccID = 0;
                    decimal lblDebit = 0;
                    decimal lblCredit = 0;
                    long TrxDetailsID = 0;

                    if (allTrx.ContainsKey(txtAccFullNameId))
                        AccFullName = allTrx[txtAccFullNameId];
                    if (allTrx.ContainsKey(txtNarrationId))
                        Narration = allTrx[txtNarrationId];
                    if (allTrx.ContainsKey(txtAccModeId))
                        AccMode = allTrx[txtAccModeId];
                    if (allTrx.ContainsKey(txtAccId))
                        int.TryParse(allTrx[txtAccId], out AccID);
                    if (allTrx.ContainsKey(txtDebitId))
                        decimal.TryParse(allTrx[txtDebitId], out lblDebit);
                    if (allTrx.ContainsKey(txtCreditId))
                        decimal.TryParse(allTrx[txtCreditId], out lblCredit);
                    if (allTrx.ContainsKey(txtTrxDetailsID))
                        long.TryParse(allTrx[txtTrxDetailsID], out TrxDetailsID);

                    if (id != row_index)
                    {
                        var details = new AccVoucherEntryViewModel() { AccFullName = AccFullName, Narration = Narration, Debit = lblDebit, Credit = lblCredit, AccID = AccID, AccMode = AccMode };
                        List_AccVoucherEntryViewModel.Add(details);
                    }
                    else // row which will delete
                    {
                        if (AccMode == "U") // delete from AccTrxDetail table
                        {
                            accTrxDetailService.Delete(Convert.ToInt32(TrxDetailsID));
                        }
                    }
                }
                return Json(List_AccVoucherEntryViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult DeleteTrxDetail(Dictionary<string, string> allTrx, List<string> allVoucherId, string row_index)
        {
            try
            {
                List<AccVoucherEntryViewModel> List_AccVoucherEntryViewModel = new List<AccVoucherEntryViewModel>();

                var trx = allTrx;

                var trxId = 1;
                var VoucherTrxIds = allVoucherId.Where(w => int.TryParse(w, out trxId));

                var VoucherTrxDetail = new List<AccTrxDetail>();
                foreach (var id in VoucherTrxIds)
                {
                    var txtAccFullNameId = "txtAccFullNameId" + id;
                    var txtNarrationId = "txtNarrationId" + id;
                    var txtAccId = "txtAccId" + id;
                    var txtCreditId = "txtCreditId" + id;
                    var txtDebitId = "txtDebitId" + id;
                    var txtAccModeId = "txtAccModeId" + id;
                    var txtTrxDetailsID = "txtTrxDetailsID" + id;

                    var AccFullName = "";
                    var Narration = "";
                    var AccMode = "";
                    int AccID = 0;
                    decimal lblDebit = 0;
                    decimal lblCredit = 0;
                    long TrxDetailsID = 0;

                    if (allTrx.ContainsKey(txtAccFullNameId))
                        AccFullName = allTrx[txtAccFullNameId];
                    if (allTrx.ContainsKey(txtNarrationId))
                        Narration = allTrx[txtNarrationId];
                    if (allTrx.ContainsKey(txtAccModeId))
                        AccMode = allTrx[txtAccModeId];
                    if (allTrx.ContainsKey(txtAccId))
                        int.TryParse(allTrx[txtAccId], out AccID);
                    if (allTrx.ContainsKey(txtDebitId))
                        decimal.TryParse(allTrx[txtDebitId], out lblDebit);
                    if (allTrx.ContainsKey(txtCreditId))
                        decimal.TryParse(allTrx[txtCreditId], out lblCredit);
                    if (allTrx.ContainsKey(txtTrxDetailsID))
                        long.TryParse(allTrx[txtTrxDetailsID], out TrxDetailsID);

                    if (id != row_index)
                    {
                        var details = new AccVoucherEntryViewModel() { AccFullName = AccFullName, Narration = Narration, Debit = lblDebit, Credit = lblCredit, AccID = AccID, AccMode = AccMode };
                        List_AccVoucherEntryViewModel.Add(details);
                    }
                    else // row which will delete
                    {
                        if (AccMode == "U") // delete from AccTrxDetail table
                        {
                            accTrxDetailService.Delete(Convert.ToInt32(TrxDetailsID));
                        }
                    }
                }
                return Json(List_AccVoucherEntryViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult VoucherDelete(string trxMasterId)
        {
            try
            {


                var param = new { @trxMasterId = trxMasterId};
                var model = accReportService.AccdelVoucher(param);

                return Json(new { result = "OK" }, JsonRequestBehavior.AllowGet);
               // return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR" });
            }
        }
        public JsonResult IsReconVoucherDelete(string trxMasterId)
        {
            try
            {


                var param = new { @trxMasterId = trxMasterId, @OfficeID = LoginUserOfficeID };
                var model = accReportService.IsReconAccdelVoucher(param);

                return Json(new { result = "OK" }, JsonRequestBehavior.AllowGet);
                // return Json(new { result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR" });
            }
        }
        private DataTable CreateTable()
        {
            var dt = new DataTable();
            //dt.Columns.Add("Sl", typeof(int));
            dt.Columns.Add("AccFullName", typeof(string));
            dt.Columns.Add("Narration", typeof(string));
            dt.Columns.Add("Debit", typeof(decimal));
            dt.Columns.Add("Credit", typeof(decimal));
            dt.Columns.Add("AccID", typeof(int));
            return dt;
        }
        public JsonResult SaveVoucherMaster(string trx_dt, string offc_id, string voucher_type, string transaction_type, string reference, string voucher_desc, bool rectify, bool IsReconcile, string ReffNo, string Purpose, string ReceivedOfficeId, decimal Debit, decimal Credit)
        {
            

            long Trx_Master_ID;
            if (trx_dt == "")
            {
                return Json(new { Result = "ERROR", JsonRequestBehavior.AllowGet });

            }

            AccReconcileViewModel reconcile = new AccReconcileViewModel();
            //var AccTrxMasterList = new List<AccTrxMaster>();
            var accTrx_Master = new AccTrxMaster() { 
                OrgID = LoggedInOrganizationID,
                OfficeID = Convert.ToInt32(offc_id),
                TrxDate = Convert.ToDateTime(trx_dt),
                VoucherNo = GenerateNewVoucher(offc_id), 
                VoucherDesc = voucher_desc,
                VoucherType = transaction_type, 
                Reference = reference, 
                IsAutoVoucher = false, 
                IsPosted = false, 
                IsYearlyClosing = false, 
                IsActive = true, 
                CreateUser = SessionHelper.LoggedInEmployeeID.ToString(), 
                CreateDate = DateTime.Now, 
                IsRectify = rectify };
           
            if (IsReconcile)
            {
                accTrx_Master.IsReconcileVoucher = true;
            }
            else
            {
                accTrx_Master.IsReconcileVoucher = false;
            }
            //AccTrxMasterList.Add(accTrx_Master);
            var Acc_Trx_Master = new AccTrxMaster();
            Acc_Trx_Master = accTrxMasterService.Create(accTrx_Master);
            if (Acc_Trx_Master.TrxMasterID > 0)
            {
                if (IsReconcile)
                {
                    Trx_Master_ID = Acc_Trx_Master.TrxMasterID;
                    reconcile.ReffNo = ReffNo;
                    reconcile.TrxMasterID = Trx_Master_ID;
                    reconcile.TrxDate = Convert.ToDateTime(trx_dt);
                    reconcile.ReceiverOfficeId = Convert.ToInt32(ReceivedOfficeId); // add received office id
                    reconcile.Debit = Convert.ToDecimal(Debit);
                    reconcile.Credit = Convert.ToDecimal(Credit);
                    reconcile.Purpose = Purpose;
                    reconcile.OrgID = SessionHelper.LoginUserOrganizationID;
                    reconcile.IsReconcile = false;
                    reconcile.IsActive = true;
                    SaveReconcilVoucher(reconcile);
                }
                Trx_Master_ID = Acc_Trx_Master.TrxMasterID;
            }
            else
                Trx_Master_ID = 0;

                return Json(Trx_Master_ID, JsonRequestBehavior.AllowGet);

        }
        public JsonResult SaveVoucherMasterReconcile(string trx_dt, string offc_id, string voucher_type, string transaction_type, string reference, string voucher_desc, bool rectify, bool IsReconcile, string ReffNo, string Purpose, string ReceivedOfficeId, decimal Debit, decimal Credit)
        {


            long Trx_Master_ID;
            if (trx_dt == "")
            {
                return Json(new { Result = "ERROR", JsonRequestBehavior.AllowGet });

            }

            AccReconcileViewModel reconcile = new AccReconcileViewModel();
            //var AccTrxMasterList = new List<AccTrxMaster>();
            var accTrx_Master = new AccTrxMaster()
            {
                OrgID = LoggedInOrganizationID,
                OfficeID = Convert.ToInt32(offc_id),
                TrxDate = Convert.ToDateTime(trx_dt),
                VoucherNo = GenerateNewVoucher(offc_id),
                VoucherDesc = voucher_desc,
                VoucherType = transaction_type,
                Reference = reference,
                IsAutoVoucher = false,
                IsPosted = false,
                IsYearlyClosing = false,
                IsActive = true,
                CreateUser = SessionHelper.LoggedInEmployeeID.ToString(),
                CreateDate = DateTime.Now,
                IsRectify = rectify
            };

            if (IsReconcile)
            {
                accTrx_Master.IsReconcileVoucher = true;
            }
            else
            {
                accTrx_Master.IsReconcileVoucher = false;
            }
            //AccTrxMasterList.Add(accTrx_Master);
            var Acc_Trx_Master = new AccTrxMaster();
            Acc_Trx_Master = accTrxMasterService.Create(accTrx_Master);
            if (Acc_Trx_Master.TrxMasterID > 0)
            {
                if (IsReconcile)
                {
                    Trx_Master_ID = Acc_Trx_Master.TrxMasterID;
                    reconcile.ReffNo = ReffNo;
                    reconcile.TrxMasterID = Trx_Master_ID;
                    reconcile.TrxDate = Convert.ToDateTime(trx_dt);
                    reconcile.ReceiverOfficeId = Convert.ToInt32(ReceivedOfficeId); // add received office id
                    reconcile.Debit = Convert.ToDecimal(Debit);
                    reconcile.Credit = Convert.ToDecimal(Credit);
                    reconcile.Purpose = Purpose;
                    reconcile.OrgID = SessionHelper.LoginUserOrganizationID;
                    reconcile.IsReconcile = false;
                    reconcile.IsActive = true;
                    SaveReconcilVoucher(reconcile);
                }
                Trx_Master_ID = Acc_Trx_Master.TrxMasterID;
            }
            else
                Trx_Master_ID = 0;

            return Json(Trx_Master_ID, JsonRequestBehavior.AllowGet);

        }
        public ActionResult SaveVoucherDetailsReconcile(Dictionary<string, string> allTrx, List<string> allVoucherId, string MasterId)
        {
            string Result = "";
            try
            {
                var trx = allTrx;

                var trxId = 1;
                var VoucherTrxIds = allVoucherId.Where(w => int.TryParse(w, out trxId));

                var VoucherTrxDetail = new List<AccTrxDetail>();
                foreach (var id in VoucherTrxIds)
                {
                    var txtNarrationId = "txtNarrationId" + id;
                    var txtAccId = "txtAccId" + id;
                    var txtCreditId = "txtCreditId" + id;
                    var txtDebitId = "txtDebitId" + id;
                    var txtAccModeId = "txtAccModeId" + id;
                    var txtTrxDetailsID = "txtTrxDetailsID" + id;
                    var Narration = "";
                    int AccID = 0;
                    var AccMode = "";
                    decimal lblDebit = 0;
                    decimal lblCredit = 0;
                    long TrxDetailsID = 0;
                    if (allTrx.ContainsKey(txtNarrationId))
                        Narration = allTrx[txtNarrationId];
                    if (allTrx.ContainsKey(txtAccId))
                        int.TryParse(allTrx[txtAccId], out AccID);
                    if (allTrx.ContainsKey(txtAccModeId))
                        AccMode = allTrx[txtAccModeId];
                    if (allTrx.ContainsKey(txtDebitId))
                        decimal.TryParse(allTrx[txtDebitId], out lblDebit);
                    if (allTrx.ContainsKey(txtCreditId))
                        decimal.TryParse(allTrx[txtCreditId], out lblCredit);
                    if (allTrx.ContainsKey(txtTrxDetailsID))
                        long.TryParse(allTrx[txtTrxDetailsID], out TrxDetailsID);
                    if (AccMode == "S")
                    {
                        var VoucherTrx = new AccTrxDetail() { TrxMasterID = Convert.ToInt64(MasterId), AccID = AccID, Credit = lblCredit, Debit = lblDebit, Narration = Narration, IsActive = true, CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID), CreateDate = DateTime.Now };
                        VoucherTrxDetail.Add(VoucherTrx);
                    }

                }
                accTrxDetailService.SaveDailyTrxDetail(VoucherTrxDetail);
                ////////////
                var Acc_Trx_Master = new AccTrxMaster();
                Acc_Trx_Master = accTrxMasterService.GetByIdLong(Convert.ToInt64(MasterId));
                bool IsReconcileVoucher = false;
                if (Acc_Trx_Master.IsReconcileVoucher == null)
                {
                    IsReconcileVoucher = false;
                }
                else if (Acc_Trx_Master.IsReconcileVoucher == false)
                {
                    IsReconcileVoucher = false;
                }
                else {
                    IsReconcileVoucher = true;
                }
                 AccTrxMaster  ss;

                if (IsReconcileVoucher)
                {
                    //ss = accTrxMasterService.GetByIdLong(Convert.ToInt64(MasterId));
                    var param = new { officeid = LoginUserOfficeID, BusinessDate = Convert.ToDateTime(TransactionDate), orgID = LoggedInOrganizationID, CreateUser = LoggedInEmployeeID };
                    var div_items = ultimateRepostService.SetReconVoucher(param);
                }

                ////loanCollectionService.SaveDailyLoanCollection(loanTrxViewCollection);
                 ss = accTrxMasterService.GetByIdLong(Convert.ToInt64(MasterId));
                Result = "Voucher Saved Successfully, Voucher number is " + ss.VoucherNo;

                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveVoucherDetails(Dictionary<string, string> allTrx, List<string> allVoucherId, string MasterId)
        {
            string Result = "";
            try
            {
                var trx = allTrx;

                var trxId = 1;
                var VoucherTrxIds = allVoucherId.Where(w => int.TryParse(w, out trxId));

                var VoucherTrxDetail = new List<AccTrxDetail>();
                foreach (var id in VoucherTrxIds)
                {
                    var txtNarrationId = "txtNarrationId" + id;
                    var txtAccId = "txtAccId" + id;
                    var txtCreditId = "txtCreditId" + id;
                    var txtDebitId = "txtDebitId" + id;
                    var txtAccModeId = "txtAccModeId" + id;
                    var txtTrxDetailsID = "txtTrxDetailsID" + id;
                    var Narration = "";
                    int AccID = 0;
                    var AccMode = "";
                    decimal lblDebit = 0;
                    decimal lblCredit = 0;
                    long TrxDetailsID = 0;
                    if (allTrx.ContainsKey(txtNarrationId))
                        Narration = allTrx[txtNarrationId];
                    if (allTrx.ContainsKey(txtAccId))
                        int.TryParse(allTrx[txtAccId], out AccID);
                    if (allTrx.ContainsKey(txtAccModeId))
                        AccMode = allTrx[txtAccModeId];
                    if (allTrx.ContainsKey(txtDebitId))
                        decimal.TryParse(allTrx[txtDebitId], out lblDebit);
                    if (allTrx.ContainsKey(txtCreditId))
                        decimal.TryParse(allTrx[txtCreditId], out lblCredit);
                    if (allTrx.ContainsKey(txtTrxDetailsID))
                        long.TryParse(allTrx[txtTrxDetailsID], out TrxDetailsID);
                    if (AccMode == "S")
                    {
                        var VoucherTrx = new AccTrxDetail() { TrxMasterID = Convert.ToInt64(MasterId), AccID = AccID, Credit = lblCredit, Debit = lblDebit, Narration = Narration, IsActive = true, CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID), CreateDate = DateTime.Now };
                        VoucherTrxDetail.Add(VoucherTrx);
                    }

                }
                accTrxDetailService.SaveDailyTrxDetail(VoucherTrxDetail);
                ////////////
                var Acc_Trx_Master = new AccTrxMaster();
                Acc_Trx_Master = accTrxMasterService.GetByIdLong(Convert.ToInt64(MasterId));
                bool IsReconcileVoucher = false;
                if (Acc_Trx_Master.IsReconcileVoucher == null)
                {
                    IsReconcileVoucher = false;
                }
                else if (Acc_Trx_Master.IsReconcileVoucher == false)
                {
                    IsReconcileVoucher = false;
                }
                else {
                    IsReconcileVoucher = true;
                }
                if (IsReconcileVoucher)
                {
                    var param = new { officeid = LoginUserOfficeID, BusinessDate = Convert.ToDateTime(TransactionDate), orgID = LoggedInOrganizationID, CreateUser = LoggedInEmployeeID };
                    var div_items = ultimateRepostService.SetReconVoucher(param);
                }

                ////loanCollectionService.SaveDailyLoanCollection(loanTrxViewCollection);
                var trxMaster = accTrxMasterService.GetByIdLong(Convert.ToInt64(MasterId));
                Result = "Voucher Saved Successfully, Voucher number is " + trxMaster.VoucherNo;

                return Json(Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(Result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SaveCashVoucher(Dictionary<string, string> allTrx, List<string> allVoucherId, string trx_dt, string offc_id, string transaction_type, string reference, string voucher_desc, bool rectify)
        {
            string Result = "";

            if (trx_dt == "")
            {
                return Json(new { Result = "ERROR", JsonRequestBehavior.AllowGet });

            }

            if (transaction_type == "Ca")
            {                
                try
                {
                    var trx = allTrx;

                    var trxId = 1;
                    var VoucherTrxIds = allVoucherId.Where(w => int.TryParse(w, out trxId));

                    var VoucherTrxDetail = new List<AccTrxDetail>();
                    sb.Append(@"Voucher Saved Successfully, Voucher numbers are <br \>");
                    foreach (var id in VoucherTrxIds)
                    {
                        var txtNarrationId = "txtNarrationId" + id;
                        var txtAccId = "txtAccId" + id;
                        var txtCreditId = "txtCreditId" + id;
                        var txtDebitId = "txtDebitId" + id;
                        var txtAccModeId = "txtAccModeId" + id;
                        var txtTrxDetailsID = "txtTrxDetailsID" + id;

                        var Narration = "";
                        int AccID = 0;
                        var AccMode = "";
                        decimal lblDebit = 0;
                        decimal lblCredit = 0;
                        long TrxDetailsID = 0;

                        if (allTrx.ContainsKey(txtNarrationId))
                            Narration = allTrx[txtNarrationId];
                        if (allTrx.ContainsKey(txtAccId))
                            int.TryParse(allTrx[txtAccId], out AccID);
                        if (allTrx.ContainsKey(txtAccModeId))
                            AccMode = allTrx[txtAccModeId];
                        if (allTrx.ContainsKey(txtDebitId))
                            decimal.TryParse(allTrx[txtDebitId], out lblDebit);
                        if (allTrx.ContainsKey(txtCreditId))
                            decimal.TryParse(allTrx[txtCreditId], out lblCredit);
                        if (allTrx.ContainsKey(txtTrxDetailsID))
                            long.TryParse(allTrx[txtTrxDetailsID], out TrxDetailsID);

                        if (AccMode == "S")
                        {
                            var VoucherMaster = new AccTrxMaster() {
                                OrgID = LoggedInOrganizationID,
                                OfficeID = Convert.ToInt32(offc_id),
                                TrxDate = Convert.ToDateTime(trx_dt),
                                VoucherNo = GenerateNewVoucher(offc_id),
                                VoucherDesc = voucher_desc,
                                VoucherType = transaction_type,
                                Reference = reference,
                                IsAutoVoucher = false,
                                IsPosted = false,
                                IsYearlyClosing = false,
                                IsActive = true,
                                CreateUser = SessionHelper.LoggedInEmployeeID.ToString(),
                                CreateDate = DateTime.Now,
                                IsRectify = rectify,
                                //IsReconcileVoucher = true
                        };
                            var Acc_Trx_Master = new AccTrxMaster();
                            Acc_Trx_Master = accTrxMasterService.Create(VoucherMaster);
                            sb.Append(VoucherMaster.VoucherNo + @"<br \>");
                            if (Acc_Trx_Master.TrxMasterID > 0)
                            {
                                var VoucherTrx = new AccTrxDetail() { TrxMasterID = Acc_Trx_Master.TrxMasterID, AccID = AccID, Credit = lblCredit, Debit = lblDebit, Narration = Narration, IsActive = true, CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID), CreateDate = DateTime.Now };
                                VoucherTrxDetail.Add(VoucherTrx);
                            }
                        }

                    }
                    accTrxDetailService.SaveDailyTrxDetail(VoucherTrxDetail);
                    //loanCollectionService.SaveDailyLoanCollection(loanTrxViewCollection);
                    Result = sb.ToString();

                    //return Json(new { Result = "OK" });
                    return Json(Result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(Result, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveCashVoucherReconcile(Dictionary<string, string> allTrx, List<string> allVoucherId, string trx_dt, string offc_id, string transaction_type, string reference, string voucher_desc, bool rectify)
        {
            string Result = "";

            if (trx_dt == "")
            {
                return Json(new { Result = "ERROR", JsonRequestBehavior.AllowGet });

            }

            if (transaction_type == "Ca")
            {
                try
                {
                    var trx = allTrx;

                    var trxId = 1;
                    var VoucherTrxIds = allVoucherId.Where(w => int.TryParse(w, out trxId));

                    var VoucherTrxDetail = new List<AccTrxDetail>();
                    sb.Append(@"Voucher Saved Successfully, Voucher numbers are <br \>");
                    foreach (var id in VoucherTrxIds)
                    {
                        var txtNarrationId = "txtNarrationId" + id;
                        var txtAccId = "txtAccId" + id;
                        var txtCreditId = "txtCreditId" + id;
                        var txtDebitId = "txtDebitId" + id;
                        var txtAccModeId = "txtAccModeId" + id;
                        var txtTrxDetailsID = "txtTrxDetailsID" + id;

                        var Narration = "";
                        int AccID = 0;
                        var AccMode = "";
                        decimal lblDebit = 0;
                        decimal lblCredit = 0;
                        long TrxDetailsID = 0;

                        if (allTrx.ContainsKey(txtNarrationId))
                            Narration = allTrx[txtNarrationId];
                        if (allTrx.ContainsKey(txtAccId))
                            int.TryParse(allTrx[txtAccId], out AccID);
                        if (allTrx.ContainsKey(txtAccModeId))
                            AccMode = allTrx[txtAccModeId];
                        if (allTrx.ContainsKey(txtDebitId))
                            decimal.TryParse(allTrx[txtDebitId], out lblDebit);
                        if (allTrx.ContainsKey(txtCreditId))
                            decimal.TryParse(allTrx[txtCreditId], out lblCredit);
                        if (allTrx.ContainsKey(txtTrxDetailsID))
                            long.TryParse(allTrx[txtTrxDetailsID], out TrxDetailsID);

                        if (AccMode == "S")
                        {
                            var VoucherMaster = new AccTrxMaster()
                            {
                                OrgID = LoggedInOrganizationID,
                                OfficeID = Convert.ToInt32(offc_id),
                                TrxDate = Convert.ToDateTime(trx_dt),
                                VoucherNo = GenerateNewVoucher(offc_id),
                                VoucherDesc = voucher_desc,
                                VoucherType = transaction_type,
                                Reference = reference,
                                IsAutoVoucher = false,
                                IsPosted = false,
                                IsYearlyClosing = false,
                                IsActive = true,
                                CreateUser = SessionHelper.LoggedInEmployeeID.ToString(),
                                CreateDate = DateTime.Now,
                                IsRectify = rectify,
                                //IsReconcileVoucher = true
                            };
                            var Acc_Trx_Master = new AccTrxMaster();
                            Acc_Trx_Master = accTrxMasterService.Create(VoucherMaster);
                            sb.Append(VoucherMaster.VoucherNo + @"<br \>");
                            if (Acc_Trx_Master.TrxMasterID > 0)
                            {
                                var VoucherTrx = new AccTrxDetail() { TrxMasterID = Acc_Trx_Master.TrxMasterID, AccID = AccID, Credit = lblCredit, Debit = lblDebit, Narration = Narration, IsActive = true, CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID), CreateDate = DateTime.Now };
                                VoucherTrxDetail.Add(VoucherTrx);
                            }
                        }

                    }
                    accTrxDetailService.SaveDailyTrxDetail(VoucherTrxDetail);
                    //loanCollectionService.SaveDailyLoanCollection(loanTrxViewCollection);
                    Result = sb.ToString();

                    //return Json(new { Result = "OK" });
                    return Json(Result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(Result, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateCashVoucher(Dictionary<string, string> allTrx, List<string> allVoucherId, string trx_dt, string offc_id, string transaction_type, string reference, string voucher_desc, bool rectify)
        {
            string Result = "";
            if (transaction_type == "Ca")
            {
                try
                {
                    var trx = allTrx;

                    var trxId = 1;
                    var VoucherTrxIds = allVoucherId.Where(w => int.TryParse(w, out trxId));

                    var VoucherTrxDetail = new List<AccTrxDetail>();
                    sb.Append(@"Voucher Saved Successfully.");
                    foreach (var id in VoucherTrxIds)
                    {
                        var txtNarrationId = "txtNarrationId" + id;
                        var txtAccId = "txtAccId" + id;
                        var txtCreditId = "txtCreditId" + id;
                        var txtDebitId = "txtDebitId" + id;
                        var txtAccModeId = "txtAccModeId" + id;
                        var txtTrxDetailsID = "txtTrxDetailsID" + id;

                        var Narration = "";
                        int AccID = 0;
                        var AccMode = "";
                        decimal lblDebit = 0;
                        decimal lblCredit = 0;
                        long TrxDetailsID = 0;

                        if (allTrx.ContainsKey(txtNarrationId))
                            Narration = allTrx[txtNarrationId];
                        if (allTrx.ContainsKey(txtAccId))
                            int.TryParse(allTrx[txtAccId], out AccID);
                        if (allTrx.ContainsKey(txtAccModeId))
                            AccMode = allTrx[txtAccModeId];
                        if (allTrx.ContainsKey(txtDebitId))
                            decimal.TryParse(allTrx[txtDebitId], out lblDebit);
                        if (allTrx.ContainsKey(txtCreditId))
                            decimal.TryParse(allTrx[txtCreditId], out lblCredit);
                        if (allTrx.ContainsKey(txtTrxDetailsID))
                            long.TryParse(allTrx[txtTrxDetailsID], out TrxDetailsID);

                        if (AccMode == "S")
                        {
                            sb.Append(@" Voucher numbers are <br \>");
                            var VoucherMaster = new AccTrxMaster()
                            {
                                OrgID = LoggedInOrganizationID,
                                OfficeID = Convert.ToInt32(offc_id),
                                TrxDate = Convert.ToDateTime(trx_dt),
                                VoucherNo = GenerateNewVoucher(offc_id),
                                VoucherDesc = voucher_desc,
                                VoucherType = transaction_type,
                                Reference = reference,
                                IsAutoVoucher = false,
                                IsPosted = false,
                                IsYearlyClosing = false,
                                IsActive = true,
                                CreateUser = SessionHelper.LoggedInEmployeeID.ToString(),
                                CreateDate = DateTime.Now,
                                IsRectify = rectify
                            };
                            var Acc_Trx_Master = new AccTrxMaster();
                            Acc_Trx_Master = accTrxMasterService.Create(VoucherMaster);
                            sb.Append(VoucherMaster.VoucherNo + @"<br \>");
                            if (Acc_Trx_Master.TrxMasterID > 0)
                            {
                                var VoucherTrx = new AccTrxDetail() { TrxMasterID = Acc_Trx_Master.TrxMasterID, AccID = AccID, Credit = lblCredit, Debit = lblDebit, Narration = Narration, IsActive = true, CreateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID), CreateDate = DateTime.Now };
                                VoucherTrxDetail.Add(VoucherTrx);
                            }
                        }
                    }
                    accTrxDetailService.SaveDailyTrxDetail(VoucherTrxDetail);
                    //loanCollectionService.SaveDailyLoanCollection(loanTrxViewCollection);
                    Result = sb.ToString();

                    //return Json(new { Result = "OK" });
                    return Json(Result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(Result, JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateVoucherMaster(string trxMasterId, string trx_dt, string offc_id, string voucher_type, string voucher_no, string reference, string voucher_desc, bool rectify)
        {
            int Trx_Master_ID;
            try
            {
                var updt = accTrxMasterService.GetByIdLong(Convert.ToInt64(trxMasterId));
                updt.TrxDate = Convert.ToDateTime(trx_dt);
                //updt.VoucherNo = voucher_no;
                //updt.VoucherDesc = voucher_desc;
                //updt.VoucherType = voucher_type;
                updt.Reference = reference;
                updt.IsRectify = rectify;
                accTrxMasterService.Update(updt);
                Trx_Master_ID = 1;
                return Json(Trx_Master_ID, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult OffcLastWorkingDay()
        {
            return Json(new { Result = "OK", Records = "" });
        }
        private string GetlastWorkingDay(int offc_id)
        {
            var param = new { OfficeID = offc_id };
            var workingDay = accReportService.GetDataLastWorkngDay(param);
            //var workingDt = Convert.ToDateTime(string.IsNullOrEmpty(workingDay.Tables[0].Rows[0]["Column1"].ToString()) ? DateTime.Now.ToString() : workingDay.Tables[0].Rows[0]["Column1"]);
            var workingDt = workingDay.Tables[0].Rows[0]["Column1"].ToString();
            return workingDt;
        }
        public JsonResult SaveLedgerPost(string lastDate)
        {
            try
            {
                var allVoucher = accTrxMasterService.GetAll().Where(v => v.IsActive == true && v.OrgID == LoggedInOrganizationID && v.OfficeID == Convert.ToInt32(SessionHelper.LoginUserOfficeID) && v.TrxDate == Convert.ToDateTime(lastDate) && v.IsPosted == false);
                int result = 0;
                foreach(var v in allVoucher)
                {
                    v.IsPosted = true;
                    accTrxMasterService.Update(v);
                    result = 1;
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex) {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }           
        }
        #endregion

        #region Events
        // GET: AccVoucherEntry
        public ActionResult Index()
        {
            var model = new AccTrxMasterViewModel();
            if (LoggedInOrganizationID != 54)
            {
                if (IsDayInitiated)
                {


                    model.TrxDate = TransactionDate;
                    ViewData["TransactionDate"] = model.TrxDate.ToString("dd-MMM-yyyy");

                    var param1 = new { OfficeID = LoginUserOfficeID, BusinessDate = TransactionDate, OrgID = SessionHelper.LoginUserOrganizationID };
                    var LoanInstallMent = weeklyReportService.CheckAutoVoucher(param1);
                    if (LoanInstallMent.Tables[0].Rows.Count == 0)
                    {
                        var param = new { OfficeID = LoginUserOfficeID, BusinessDate = TransactionDate, OrgID = SessionHelper.LoginUserOrganizationID };
                        weeklyReportService.AutoVoucherCollectionProcess(param);
                    }

                }
            }
            //if (IsDayInitiated)
            //{
            //    model.TrxDate = TransactionDate;
            //    ViewData["TransactionDate"] = model.TrxDate.ToString("dd-MMM-yyyy");

            //    var param1 = new { OfficeID = LoginUserOfficeID, BusinessDate = TransactionDate, OrgID = SessionHelper.LoginUserOrganizationID };
            //    var LoanInstallMent = weeklyReportService.CheckAutoVoucher(param1);

            //    if (LoanInstallMent.Tables[0].Rows.Count==0)
            //    {
            //        var param = new { OfficeID = LoginUserOfficeID, BusinessDate = TransactionDate, OrgID = SessionHelper.LoginUserOrganizationID };
            //        weeklyReportService.AutoVoucherCollectionProcess(param);
            //    }

            //}
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            model.OfficeLevel = offcdetail.OfficeLevel;
            var workingDt = GetlastWorkingDay(offcdetail.OfficeID);
            if (workingDt != "")
            {

                model.TrxDate = Convert.ToDateTime(workingDt);
                model.LastWorkingDate = Convert.ToDateTime(workingDt);
            }

           

            return View(model);
        }

        public ActionResult ReconcileIndex()
        {
            var model = new AccTrxMasterViewModel();

            if (LoggedInOrganizationID != 54)
            {
                if (IsDayInitiated)
                {
                    model.TrxDate = TransactionDate;
                    ViewData["TransactionDate"] = model.TrxDate.ToString("dd-MMM-yyyy");

                    var param1 = new { OfficeID = LoginUserOfficeID, BusinessDate = TransactionDate, OrgID = SessionHelper.LoginUserOrganizationID };
                    var LoanInstallMent = weeklyReportService.CheckAutoVoucher(param1);

                    if (LoanInstallMent.Tables[0].Rows.Count == 0)
                    {
                        var param = new { OfficeID = LoginUserOfficeID, BusinessDate = TransactionDate, OrgID = SessionHelper.LoginUserOrganizationID };
                        weeklyReportService.AutoVoucherCollectionProcess(param);
                    }

                }
            }


            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            model.OfficeLevel = offcdetail.OfficeLevel;
            var workingDt = GetlastWorkingDay(offcdetail.OfficeID);
            if (workingDt != "")
            {

                model.TrxDate = Convert.ToDateTime(workingDt);
                model.LastWorkingDate = Convert.ToDateTime(workingDt);
            }


            var allOffice = officeService.GetAllZoneOffice("oo",LoggedInOrganizationID);
            var viewPurpose = allOffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeCode.ToString() });
            model.OfficeListItems = viewPurpose;
            return View(model);
        }
        // GET: AccVoucherEntry/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: AccVoucherEntry/Create
        public ActionResult Create()
        {
            var model = new AccVoucherEntryViewModel();
            MapDropDownList(model);
            //model.TrxDate = processInfoService.GetInitialDtByOfficeId(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            if (IsDayInitiated)
            {
                model.TrxDate = TransactionDate;
                ViewData["TransactionDate"] = model.TrxDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                var param = new { OfficeId= Convert.ToInt32(SessionHelper.LoginUserOfficeID), OrgID =Convert.ToInt32(LoggedInOrganizationID) };
            var workingDay = accReportService.GetDataValidWorkngDay(param);
            //var workingDt = Convert.ToDateTime(string.IsNullOrEmpty(workingDay.Tables[0].Rows[0]["Column1"].ToString()) ? DateTime.Now.ToString() : workingDay.Tables[0].Rows[0]["Column1"]);
            var workingDt = workingDay.Tables[0].Rows[0]["vBusinessDate"].ToString();
            

                if (workingDt != "")
                {

                    model.TrxDate = Convert.ToDateTime(workingDt);
                    ViewData["TransactionDate"] = model.TrxDate.ToString("dd-MMM-yyyy");
                    //model.LastWorkingDate = Convert.ToDateTime(workingDt);
                }
            }
            
            model.VoucherType = "Dr";
            model.ReffNoList = ReffNoList();
            model.OfficeList = OfficeList();

            model.IsRectify = false;
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            model.OfficeLevel = officeService.GetById(model.OfficeID).OfficeLevel;
            //Session[sessionName] = CreateTable();
            return View(model);
        }

        public ActionResult ReconcileCreate()
        {
            var model = new AccVoucherEntryViewModel();
            ReconcileMapDropDownList(model);
            //model.TrxDate = processInfoService.GetInitialDtByOfficeId(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            if (IsDayInitiated)
            {
                model.TrxDate = TransactionDate;
                ViewData["TransactionDate"] = model.TrxDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                var param = new { OfficeId = Convert.ToInt32(SessionHelper.LoginUserOfficeID), OrgID = Convert.ToInt32(LoggedInOrganizationID) };
                var workingDay = accReportService.GetDataValidWorkngDay(param);
                //var workingDt = Convert.ToDateTime(string.IsNullOrEmpty(workingDay.Tables[0].Rows[0]["Column1"].ToString()) ? DateTime.Now.ToString() : workingDay.Tables[0].Rows[0]["Column1"]);
                var workingDt = workingDay.Tables[0].Rows[0]["vBusinessDate"].ToString();


                if (workingDt != "")
                {

                    model.TrxDate = Convert.ToDateTime(workingDt);
                    ViewData["TransactionDate"] = model.TrxDate.ToString("dd-MMM-yyyy");
                    //model.LastWorkingDate = Convert.ToDateTime(workingDt);
                }
            }

            model.VoucherType = "Dr";
            model.ReffNoList = ReffNoList();
            model.OfficeList = OfficeList();

            model.IsRectify = false;
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            model.OfficeLevel = officeService.GetById(model.OfficeID).OfficeLevel;
            //Session[sessionName] = CreateTable();
            return View(model);
        }
        // POST: AccVoucherEntry/Create
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
        // GET: AccVoucherEntry/Edit/5
        public ActionResult Edit(long id)
        {
            var trxMaster = new AccTrxMaster();
            trxMaster = accTrxMasterService.GetByIdLong(id);
            var model = new AccVoucherEntryViewModel();
            model.TrxMasterID = trxMaster.TrxMasterID;
            model.OfficeID = trxMaster.OfficeID;
            model.TrxDate = trxMaster.TrxDate;
            model.VoucherNo = trxMaster.VoucherNo;
            model.VoucherDesc = trxMaster.VoucherDesc;
            model.Reference = trxMaster.Reference;
            model.IsAutoVoucher = trxMaster.IsAutoVoucher;
            model.IsRectify = Convert.ToBoolean(trxMaster.IsRectify);
            model.OfficeLevel = officeService.GetById(trxMaster.OfficeID).OfficeLevel;
            if (trxMaster.IsAutoVoucher == true)
                model.AutoVoucher = "1";
            else if (trxMaster.IsAutoVoucher == false)
                model.AutoVoucher = "0";
            MapDropDownList(model);
            model.TransactionType = trxMaster.VoucherType;
            //model.TrxDate = processInfoService.GetByOfficeId(6).BusinessDate;
            ViewData["TransactionDate"] = model.TrxDate.ToString("dd-MMM-yyyy");
            return View(model);
        }
        // POST: AccVoucherEntry/Edit/5
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
        // GET: AccVoucherEntry/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: AccVoucherEntry/Delete/5
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

        #region Re Concile

        

        public IEnumerable<SelectListItem> ReffNoList()
        {


            var reConcileList = reConcileService.GetAll().Where(b => b.IsActive == true && b.IsReconcile == false && b.ReceiverOfficeId == SessionHelper.LoginUserOfficeID);
            var viewReconcile = reConcileList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ReffNo.ToString(),
                Text = x.ReffNo.ToString()

            });
            var reConcileItem = new List<SelectListItem>();
            reConcileItem.Add(new SelectListItem() { Text = "Select Refference No", Value = "" });
            reConcileItem.AddRange(viewReconcile);
            return reConcileItem;

        }
        public ActionResult ReconcileVoucherList()
        {
            var model = new AccTrxMasterViewModel();
            if (LoggedInOrganizationID != 54)
            {


                if (IsDayInitiated)
                {
                    model.TrxDate = TransactionDate;
                    ViewData["TransactionDate"] = model.TrxDate.ToString("dd-MMM-yyyy");

                    var param1 = new { OfficeID = LoginUserOfficeID, BusinessDate = TransactionDate, OrgID = SessionHelper.LoginUserOrganizationID };
                    var LoanInstallMent = weeklyReportService.CheckAutoVoucher(param1);

                    if (LoanInstallMent.Tables[0].Rows.Count == 0)
                    {
                        var param = new { OfficeID = LoginUserOfficeID, BusinessDate = TransactionDate, OrgID = SessionHelper.LoginUserOrganizationID };
                        weeklyReportService.AutoVoucherCollectionProcess(param);
                    }

                }
            }
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            model.OfficeLevel = offcdetail.OfficeLevel;
            var workingDt = GetlastWorkingDay(offcdetail.OfficeID);
            if (workingDt != "")
            {

                model.TrxDate = Convert.ToDateTime(workingDt);
                model.LastWorkingDate = Convert.ToDateTime(workingDt);
            }



            return View(model);
        }

        public ActionResult ReconcileVoucherSend()
        {
            var model = new AccVoucherEntryViewModel();
            ReconcileMapDropDownList(model);
            //model.TrxDate = processInfoService.GetInitialDtByOfficeId(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            if (IsDayInitiated)
            {
                model.TrxDate = TransactionDate;
                ViewData["TransactionDate"] = model.TrxDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                var param = new { OfficeId = Convert.ToInt32(SessionHelper.LoginUserOfficeID), OrgID = Convert.ToInt32(LoggedInOrganizationID) };
                var workingDay = accReportService.GetDataValidWorkngDay(param);
                //var workingDt = Convert.ToDateTime(string.IsNullOrEmpty(workingDay.Tables[0].Rows[0]["Column1"].ToString()) ? DateTime.Now.ToString() : workingDay.Tables[0].Rows[0]["Column1"]);
                var workingDt = workingDay.Tables[0].Rows[0]["vBusinessDate"].ToString();


                if (workingDt != "")
                {

                    model.TrxDate = Convert.ToDateTime(workingDt);
                    ViewData["TransactionDate"] = model.TrxDate.ToString("dd-MMM-yyyy");
                    //model.LastWorkingDate = Convert.ToDateTime(workingDt);
                }
            }

            model.VoucherType = "Dr";
            model.ReffNoList = ReffNoList();
            model.OfficeList = OfficeList();

            model.IsRectify = false;
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            model.OfficeLevel = officeService.GetById(model.OfficeID).OfficeLevel;
            //Session[sessionName] = CreateTable();
            return View(model);
        }
        public JsonResult GetVoucherTypeReconcile(string ReffNo)
        {
            try
            {
               
                List<AccReconcileViewModel> List_MemberwiseProduct = new List<AccReconcileViewModel>();

                var param = new { ReffNo = ReffNo, trxDate = Convert.ToDateTime(TransactionDate) };
                var alldata = accReportService.GetVoucherList(param);
                
                List_MemberwiseProduct = alldata.Tables[0].AsEnumerable()
                .Select(row => new AccReconcileViewModel
                {
                    VoucherID = row.Field<string>("VoucherTypeValue"),
                    //LoanTerm = row.Field<string>("LoanTerm"),
                    VoucherName = row.Field<string>("VoucherType"),
                    
                    
                }).ToList();
                var Components = List_MemberwiseProduct.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.VoucherID.ToString(),
                    Text = string.Format("{0}", x.VoucherName)

                });

                var Component_items = new List<SelectListItem>();
                if (Components.ToList().Count > 0)
                {
                    //Component_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });

                }
                
                Component_items.AddRange(Components);
                return Json( Component_items , JsonRequestBehavior.AllowGet);
                // return Json(List_MemberwiseProduct, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ReconcileReceive(long Id)
        {
            var trxMaster = new AccTrxMaster();
            trxMaster = accTrxMasterService.GetByIdLong(Id);

            var model = new AccVoucherEntryViewModel();
            ReconcileMapDropDownList(model);
            if (IsDayInitiated)
            {
                model.TrxDate = TransactionDate;
                ViewData["TransactionDate"] = model.TrxDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                var param2 = new { OfficeId = Convert.ToInt32(SessionHelper.LoginUserOfficeID), OrgID = Convert.ToInt32(LoggedInOrganizationID) };
                var workingDay = accReportService.GetDataValidWorkngDay(param2);
                var workingDt = workingDay.Tables[0].Rows[0]["vBusinessDate"].ToString();


                if (workingDt != "")
                {

                    model.TrxDate = Convert.ToDateTime(workingDt);
                    ViewData["TransactionDate"] = model.TrxDate.ToString("dd-MMM-yyyy");
                    //model.LastWorkingDate = Convert.ToDateTime(workingDt);
                }
            }

            model.VoucherType = "Dr";
            model.ReffNoList = ReffNoList(Id);
            model.OfficeList = OfficeList();

            model.IsRectify = false;
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            model.OfficeLevel = officeService.GetById(model.OfficeID).OfficeLevel;
            //Session[sessionName] = CreateTable();
            return View(model);
        }

        public ActionResult ReconcileVoucherReceive(long Id)
        {
            var trxMaster = new AccTrxMaster();
            trxMaster = accTrxMasterService.GetByIdLong(Id);

            var model = new AccVoucherEntryViewModel();
            ReconcileMapDropDownList(model);
            if (IsDayInitiated)
            {
                model.TrxDate = TransactionDate;
                ViewData["TransactionDate"] = model.TrxDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                var param2 = new { OfficeId = Convert.ToInt32(SessionHelper.LoginUserOfficeID), OrgID = Convert.ToInt32(LoggedInOrganizationID) };
                var workingDay = accReportService.GetDataValidWorkngDay(param2);
                var workingDt = workingDay.Tables[0].Rows[0]["vBusinessDate"].ToString();


                if (workingDt != "")
                {

                    model.TrxDate = Convert.ToDateTime(workingDt);
                    ViewData["TransactionDate"] = model.TrxDate.ToString("dd-MMM-yyyy");
                    //model.LastWorkingDate = Convert.ToDateTime(workingDt);
                }
            }

            model.VoucherType = "Dr";
            model.ReffNoList = ReffNoList(Id);
            model.OfficeList = OfficeList();

            model.IsRectify = false;
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            model.OfficeLevel = officeService.GetById(model.OfficeID).OfficeLevel;
            //Session[sessionName] = CreateTable();
            return View(model);
        }

        public IEnumerable<SelectListItem> ReffNoList(long TrxMasterId)
        {



            //var reConcileList = reConcileService.GetAll().Where(b => b.IsActive == true && b.IsReconcile == false && b.ReceiverOfficeId == SessionHelper.LoginUserOfficeID);

            var param = new { TrxMasterId=TrxMasterId, ReceiverOfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID) };
            var Reconsiles = accReportService.GetReconcileList(param);

            List<AccReconcileViewModel> ReconcileviewModellst = new List<AccReconcileViewModel>();

            ReconcileviewModellst = Reconsiles.Tables[0].AsEnumerable()
           .Select(row => new AccReconcileViewModel
           {
               ReffNo = row.Field<string>("ReffNo")
           }).ToList();

            var viewReconcile = ReconcileviewModellst.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ReffNo.ToString(),
                Text = x.ReffNo.ToString()
            });
            var reConcileItem = new List<SelectListItem>();
           // reConcileItem.Add(new SelectListItem() { Text = "Select Refference No", Value = "" });
            reConcileItem.AddRange(viewReconcile);
            return reConcileItem;



            //var reConcileList = reConcileService.GetAll().Where(b => b.IsActive == true && b.IsReconcile == false && b.ReceiverOfficeId == SessionHelper.LoginUserOfficeID && b.TrxMasterID == TrxMasterId);
            //var viewReconcile = reConcileList.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.ReffNo.ToString(),
            //    Text = x.ReffNo.ToString()

            //});
            //var reConcileItem = new List<SelectListItem>();
            ////reConcileItem.Add(new SelectListItem() { Text = "Select Refference No", Value = "" });
            //reConcileItem.AddRange(viewReconcile);
            //return reConcileItem;
        }

        public JsonResult GetVoucherType(string ReffNo)
        {
            try
            {
                List<AccReconcileViewModel> List_MemberwiseProduct = new List<AccReconcileViewModel>();

                var param = new { ReffNo = ReffNo, trxDate = Convert.ToDateTime(TransactionDate) };
                var alldata = accReportService.GetVoucherList(param);

                List_MemberwiseProduct = alldata.Tables[0].AsEnumerable()
                .Select(row => new AccReconcileViewModel
                {
                    VoucherID = row.Field<string>("VoucherTypeValue"),
                    //LoanTerm = row.Field<string>("LoanTerm"),
                    VoucherName = row.Field<string>("VoucherType")

                }).ToList();
                var Components = List_MemberwiseProduct.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.VoucherID.ToString(),
                    Text = string.Format("{0}", x.VoucherName)
                   
                });

                var Component_items = new List<SelectListItem>();
                if (Components.ToList().Count > 0)
                {
                    //Component_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                    
                }
                Component_items.AddRange(Components);
                return Json(Component_items, JsonRequestBehavior.AllowGet);
                // return Json(List_MemberwiseProduct, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public IEnumerable<SelectListItem> OfficeList()
        {


            var officeList = officeService.GetAll().Where(b => b.IsActive == true && b.OfficeID != SessionHelper.LoginUserOfficeID && b.OfficeLevel==4);
            var viewOffice = officeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode+ ' ' + x.OfficeName.ToString()

            });
            var officeItem = new List<SelectListItem>();
            officeItem.Add(new SelectListItem() { Text = "Select Office", Value = "" });
            officeItem.AddRange(viewOffice);
            return officeItem;

        }
        [HttpPost]
        public JsonResult ReffNoWiseOffice(string ReffNo)
        {



            var paramreff = new { ReffNo = ReffNo};
            var Reconsiles = accReportService.GetReconcileReffNo(paramreff);
            var officeId = Reconsiles.Tables[0].Rows[0]["SenderOfficeId"].ToString();
            var ReDebit = Reconsiles.Tables[0].Rows[0]["Debit"].ToString();
            var ReCredit = Reconsiles.Tables[0].Rows[0]["Credit"].ToString();
            //var reconcileData = reConcileService.GetAll().Where(b => b.IsActive == true && b.ReffNo == ReffNo).FirstOrDefault();


            //var officeId = reconcileData.SenderOfficeId;
            //var ReDebit = reconcileData.Debit;
            //var ReCredit = reconcileData.Credit;
            var officeList = officeService.GetAll().Where(b => b.IsActive == true && b.OfficeID == Convert.ToInt16(officeId));

            var param = new { ReffNo = ReffNo, trxDate = Convert.ToDateTime(TransactionDate) };
            var alldata = accReportService.GetVoucherList(param);
            string vVoucherType="";
            decimal vDebit = 0;
            decimal vCredit = 0;
            if (alldata.Tables[0].Rows.Count > 0)
            {
                vVoucherType = alldata.Tables[0].Rows[0]["VoucherTypeValue"].ToString();
                if (vVoucherType == "Dr")
                {
                    vDebit = Convert.ToDecimal(alldata.Tables[0].Rows[0]["Credit"].ToString());
                    vCredit = 0;
                }
                else if (vVoucherType == "Cr")
                {
                    vCredit = Convert.ToDecimal(alldata.Tables[0].Rows[0]["Debit"].ToString());
                    vDebit = 0;
                }
            }
            var viewOffice = officeList.Select(x => new
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeName.ToString()
            });
            var officeItem = new List<SelectListItem>();
            //officeItem.Add(new SelectListItem() { Text = "Select Office ID", Value = "" });

            return Json(new { viewOffice = viewOffice, ReDebit = ReDebit, ReCredit = ReCredit,vDebit=vDebit,vCredit=vCredit }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult Js_OfficeList()
        {


            var officeList = officeService.GetAll().Where(b => b.IsActive == true && b.OfficeID != SessionHelper.LoginUserOfficeID);
            var viewOffice = officeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeName.ToString()

            });
            var officeItem = new List<SelectListItem>();
            officeItem.Add(new SelectListItem() { Text = "Select Office", Value = "" });
            officeItem.AddRange(viewOffice);
            return Json(new { viewOffice = officeItem, ReDebit = 0, ReCredit = 0 }, JsonRequestBehavior.AllowGet);

        }
         public bool SaveReconcilVoucher(AccReconcileViewModel model)
        {
            bool Result = false;
            try
            {

                var dbModel = new AccReconcile();
                int update = 0;
                var parm = new { OfficeID = LoginUserOfficeID, TrxDate = TransactionDate };
                if (model.ReffNo == "")
                {
                    var dbReffNo = ultimateRepostService.Set_GenerateReffNo(parm);

                    var newReffNo = dbReffNo.Tables[0].AsEnumerable()
                    .Select(row => new AccVoucherEntryViewModel()
                    {
                        ReffNo = row.Field<string>("ReffNo")
                    }).FirstOrDefault();


                    dbModel.ReffNo = newReffNo.ReffNo;
                    //dbModel.ReffNo = 1;
                    dbModel.SenderOfficeId = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
                    dbModel.ReceiverOfficeId = model.ReceiverOfficeId;
                    dbModel.Debit = model.Debit;
                    dbModel.Credit = model.Credit;
                    dbModel.IsReconcile = false;

                }
                else
                {
                    update = 1;
                    dbModel.ReffNo = model.ReffNo;
                    dbModel.SenderOfficeId = model.ReceiverOfficeId;
                    dbModel.ReceiverOfficeId = Convert.ToInt32(SessionHelper.LoginUserOfficeID);// model.SenderOfficeId;
                    dbModel.Debit = model.Debit;
                    dbModel.Credit = model.Credit;
                    dbModel.IsReconcile = true;
                }
                dbModel.TrxMasterID = model.TrxMasterID;
                dbModel.TrxDate = model.TrxDate;
                dbModel.Purpose = model.Purpose;
                dbModel.OrgID = SessionHelper.LoginUserOrganizationID;

                dbModel.IsActive = true;
                dbModel.CreateDate = DateTime.Now;
                dbModel.CreateUser = SessionHelper.LoginUserEmployeeID.ToString();
                if (model.ReffNo!="")
                {
                    //var d = reConcileService.Update(dbModel);
                    //if (update == 1 && d.AccReconcileID > 0)
                    //{
                        var upReconcile = reConcileService.GetAll().Where(b => b.ReffNo == dbModel.ReffNo).FirstOrDefault();
                        upReconcile.IsReconcile = true;
                        reConcileService.Update(upReconcile);
                    //}
                }
                else
                {
                    var f = reConcileService.Create(dbModel);
                    if (update == 1 && f.AccReconcileID > 0)
                    {
                        var upReconcile = reConcileService.GetAll().Where(b => b.ReffNo == dbModel.ReffNo).FirstOrDefault();
                        upReconcile.IsReconcile = true;
                        reConcileService.Update(upReconcile);
                    }
                }
               
               
                Result = true;
            }
            catch (Exception e)
            {
                Result = false;
                string msg = e.Message;

            }
            return Result;
        }
        #endregion
    }
}
