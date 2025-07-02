using AutoMapper;
using gBanker.Data.CodeFirstMigration.Db;
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
using System.Globalization;
using gBanker.Data.CodeFirstMigration;

namespace gBanker.Web.Controllers
{
    
    public class AccountCodeController : BaseController
    {

        private readonly IMemberCategoryService memberCategoryService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IOfficeService officeService;
        public AccountCodeController(IMemberCategoryService memberCategoryService, IUltimateReportService ultimateReportService, IOfficeService officeService)
        {
            this.memberCategoryService = memberCategoryService;
            this.ultimateReportService = ultimateReportService;
            this.officeService = officeService;
        }
        //
        // GET: /MemberCategory/
        public ActionResult Index()
        {
            //var allMemberCategory = memberCategoryService.GetAll();
            //var viewCategory = Mapper.Map<IEnumerable<MemberCategory>, IEnumerable<MemberCategoryViewModel>>(allMemberCategory);

            //return View(viewCategory);
            return View();
        }
        public JsonResult GetCodeInfo(string TrxDate, string AccCode, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue, string TypeFilterColumn)
        {
            try
            {
                long TotCount;

                // @OfficeId int, @TrxDate	Date, 	@AccCode varchar(10)
                var officeId = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
                DateTime dt = DateTime.Now;
                if (TrxDate != null)
                {
                    string[] datesplit = TrxDate.Split('/');
                    var day = datesplit[0];
                    var month = datesplit[1];
                    var year = datesplit[2];

                    dt = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));

                }


                var CusAccCode = "0000";
                if (AccCode != null)
                {
                    if (AccCode != "")
                    {
                        CusAccCode = AccCode;
                    }
                }

                var param1 = new { @OfficeId = officeId, @TrxDate = dt.Date, @AccCode = CusAccCode };
                var LoanInstallMent = ultimateReportService.GetAccountCode(param1);

                List<AccCodeViewModel> List_ViewModel = new List<AccCodeViewModel>();
                List_ViewModel = LoanInstallMent.Tables[0].AsEnumerable()
                .Select(row => new AccCodeViewModel
                {

                    //public int OfficeID { get; set; }
                    OfficeID = row.Field<int>("OfficeID"),
                    //public string OfficeCode { get; set; }
                    OfficeCode = row.Field<string>("OfficeCode"),
                    //public string OfficeName { get; set; }
                    OfficeName = row.Field<string>("OfficeName"),
                    //public int TrxMasterID { get; set; }
                    TrxMasterID = row.Field<long>("TrxMasterID"),
                    //public DateTime TrxDate { get; set; }
                    TrxDate = row.Field<string>("TrxDate"),
                    trxDay = row.Field<int>("trxDay"),
                    trxMonth = row.Field<int>("trxMonth"),
                    trxYear = row.Field<int>("trxYear"),
                    VoucherNo = row.Field<string>("VoucherNo"),
                    VoucherType = row.Field<string>("VoucherType"),
                    TrxDateFormated = row.Field<string>("TrxDateFormated"),
                    //public int TrxDetailsID { get; set; }
                    TrxDetailsID = row.Field<long>("TrxDetailsID"),
                    //public int AccID { get; set; }
                    AccID = row.Field<int>("AccID"),
                    //public string AccCode { get; set; }
                    AccCodes = row.Field<string>("AccCode"),
                    //public string AccName { get; set; }
                    AccName = row.Field<string>("AccName"),

                    //public float Credit { get; set; }
                    Credit = row.Field<decimal>("Credit"),

                    //public float Debit { get; set; }
                    Debit = row.Field<decimal>("Debit"),
                    Narration = row.Field<string>("Narration"),
                    ReconPurpose = row.Field<string>("ReconPurpose"),


                }).ToList();

                // var memberDetail = memberService.GetMemberDetail(Convert.ToInt16(LoggedInOrganizationID),SessionHelper.LoginUserOfficeID, filterColumn, filterValue, TypeFilterColumn, jtStartIndex, jtSorting, jtPageSize, out TotCount);
                var detail = List_ViewModel.ToList();
                TotCount = detail.Count();
                var currentPageRecords = detail.ToList();
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //public JsonResult UpdatePOPUPData(string hdnTrxMasterID, string hdnTrxDetailsID, string txtAccCode, string txtCredit, string txtDebit, string txtTrxDate)
        //{

        //    string result = "Data Update Successfully";
        //    try
        //    {
        //        DateTime dt = DateTime.Now;
        //        if (txtTrxDate != null)
        //        {
        //            string[] datesplit = txtTrxDate.Split('/');
        //            var day = datesplit[0];
        //            var month = datesplit[1];
        //            var year = datesplit[2];

        //            dt = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));

        //        }

        //        var param = new
        //        {
        //            hdnTrxMasterID = hdnTrxMasterID
        //          ,
        //            hdnTrxDetailsID = hdnTrxDetailsID
        //          ,
        //            txtAccCode = txtAccCode
        //          ,
        //            txtCredit = txtCredit
        //          ,
        //            txtDebit = txtDebit
        //          ,
        //            txtTrxDate = dt.Date.ToString("dd/MMM/yy") //txtTrxDate        
        //          ,
        //            CreateUser = SessionHelper.LoginUserEmployeeID
        //          ,
        //            CreateDate = dt.Date.ToString("dd/MMM/yy")
        //        };
        //        var val = ultimateReportService.UpdateAccountInfo(param);

        //    }
        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = 403;
        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult UpdatePOPUPData(string hdnTrxMasterID, string hdnTrxDetailsID, string txtAccCode, string txtCredit, string txtDebit, string txtTrxDate, string txtNarration, string txtReconPurpose, string txtVoucherType)
        {

            string result = "Data Update Successfully";
            try
            {
                DateTime dt = DateTime.Now;
                if (txtTrxDate != null)
                {
                    string[] datesplit = txtTrxDate.Split('/');
                    var day = datesplit[0];
                    var month = datesplit[1];
                    var year = datesplit[2];

                    dt = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));

                }

                if (txtReconPurpose == null)
                {
                    txtReconPurpose = "";
                }

                var param = new
                {
                    hdnTrxMasterID = hdnTrxMasterID
                  ,
                    hdnTrxDetailsID = hdnTrxDetailsID
                  ,
                    txtAccCode = txtAccCode
                  ,
                    txtCredit = txtCredit
                  ,
                    txtDebit = txtDebit
                  ,
                    txtTrxDate = dt.Date.ToString("dd/MMM/yy") //txtTrxDate        
                  ,
                    CreateUser = SessionHelper.LoginUserEmployeeID
                  ,
                    CreateDate = dt.Date.ToString("dd/MMM/yy")
                    ,
                    Narration = txtNarration
                    ,
                    ReconPurpose = txtReconPurpose
                    ,
                    VoucherType = txtVoucherType
                };
                var val = ultimateReportService.UpdateAccountInfo(param);

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region JCF


        public ActionResult MulAccCodeCorrection()
        {
            //var allMemberCategory = memberCategoryService.GetAll();
            //var viewCategory = Mapper.Map<IEnumerable<MemberCategory>, IEnumerable<MemberCategoryViewModel>>(allMemberCategory);

            //return View(viewCategory);

            //var OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            //var OfficeLevel = officeService.GetById(OfficeID).OfficeLevel;


            return View();
        }

        public JsonResult GetCodeInfoJCF(string TrxDate, string AccCode, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue, string TypeFilterColumn)
        {
            try
            {
                long TotCount;

                // @OfficeId int, @TrxDate	Date, 	@AccCode varchar(10)
                var officeId = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
                DateTime dt = DateTime.Now;
                if (TrxDate != null)
                {
                    string[] datesplit = TrxDate.Split('/');
                    var day = datesplit[0];
                    var month = datesplit[1];
                    var year = datesplit[2];
                    dt = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));

                }

                var CusAccCode = "0000";
                if (AccCode != null)
                {
                    if (AccCode != "")
                    {
                        var v = AccCode.Split('-');
                        CusAccCode = v[0].ToString().Trim();
                    }
                }

                var param1 = new { @OfficeId = officeId, @TrxDate = dt.Date, @AccCode = CusAccCode };
                var LoanInstallMent = ultimateReportService.GetAccountCodeJCF(param1);

                List<AccCodeViewModel> List_ViewModel = new List<AccCodeViewModel>();
                List_ViewModel = LoanInstallMent.Tables[0].AsEnumerable()
                .Select(row => new AccCodeViewModel
                {
                    //public int OfficeID { get; set; }
                    OfficeID = row.Field<int>("OfficeID"),
                    //public string OfficeCode { get; set; }
                    OfficeCode = row.Field<string>("OfficeCode"),
                    //public string OfficeName { get; set; }
                    OfficeName = row.Field<string>("OfficeName"),
                    //public int TrxMasterID { get; set; }
                    TrxMasterID = row.Field<long>("TrxMasterID"),
                    //public DateTime TrxDate { get; set; }
                    TrxDate = row.Field<string>("TrxDate"),
                    trxDay = row.Field<int>("trxDay"),
                    trxMonth = row.Field<int>("trxMonth"),
                    trxYear = row.Field<int>("trxYear"),
                    VoucherNo = row.Field<string>("VoucherNo"),
                    VoucherType = row.Field<string>("VoucherType"),
                    TrxDateFormated = row.Field<string>("TrxDateFormated"),
                    //public int TrxDetailsID { get; set; }
                    TrxDetailsID = row.Field<long>("TrxDetailsID"),
                    //public int AccID { get; set; }
                    AccID = row.Field<int>("AccID"),
                    //public string AccCode { get; set; }
                    AccCodes = row.Field<string>("AccCode"),
                    //public string AccName { get; set; }
                    AccName = row.Field<string>("AccName"),
                    //public float Credit { get; set; }
                    Credit = row.Field<decimal>("Credit"),
                    //public float Debit { get; set; }
                    Debit = row.Field<decimal>("Debit")

                }).ToList();

                // var memberDetail = memberService.GetMemberDetail(Convert.ToInt16(LoggedInOrganizationID),SessionHelper.LoginUserOfficeID, filterColumn, filterValue, TypeFilterColumn, jtStartIndex, jtSorting, jtPageSize, out TotCount);
                var detail = List_ViewModel.ToList();
                TotCount = detail.Count();
                var currentPageRecords = detail.ToList();
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = TotCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public JsonResult GetAccCode(string TrxDate, string acc_code)
        {
            IEnumerable<AccCodeViewModel> chart;

            var officeId = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            DateTime dt = DateTime.Now;
            if (TrxDate != null)
            {
                string[] datesplit = TrxDate.Split('/');
                var day = datesplit[0];
                var month = datesplit[1];
                var year = datesplit[2];

                dt = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));

            }


            var param1 = new { @OfficeId = officeId, @TrxDate = dt.Date, @AccCode = "" };
            var LoanInstallMent = ultimateReportService.GetAccountCodeList(param1);

            List<AccCodeViewModel> List_ViewModel = new List<AccCodeViewModel>();
            List_ViewModel = LoanInstallMent.Tables[0].AsEnumerable()
            .Select(row => new AccCodeViewModel
            {
                //public string AccCode { get; set; }
                AccCodes = row.Field<string>("AccCode"),

                //public string AccName { get; set; }
                AccName = row.Field<string>("AccName"),



            }).ToList();


            chart = List_ViewModel;
            /*
            var BankCode = accChartService.GetByAccCode(applicationSettingsService.GetAll().Where(c => c.OfficeID == LoginUserOfficeID).FirstOrDefault().BankAccount);

            if (TransactionType == "Bc")
            {
                chart = accChartService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID && m.IsTransaction == true && m.IsActive == true && m.OfficeLevel >= OfficeLevel && m.ModuleID == 1 && m.SecondLevel == BankCode.SecondLevel).ToList();
            }
            else if (TransactionType == "Ca")
            {
                chart = accChartService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID && m.IsTransaction == true && m.IsActive == true && m.OfficeLevel >= OfficeLevel && m.ModuleID == 1 && m.SecondLevel != BankCode.SecondLevel).ToList();
            }
            else if (TransactionType == "Ba")
            {
                chart = accChartService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID && m.IsTransaction == true && m.IsActive == true && m.OfficeLevel >= OfficeLevel && m.ModuleID == 1 && m.SecondLevel != BankCode.SecondLevel).ToList();
            }
            //else if (TransactionType == "Ca")
            //{
            //    chart = accChartService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID && m.IsTransaction == true && m.IsActive == true && m.OfficeLevel >= OfficeLevel && m.ModuleID == 1).ToList();
            //}
            else
            {
                chart = accChartService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID && m.IsTransaction == true && m.IsActive == true && m.OfficeLevel >= OfficeLevel && m.ModuleID == 1).ToList();
            }

            */
            var chartList = new List<AccCodeViewModel>();
            chartList = chart.ToList();
            var acc = chartList.Where(m => string.Format("{0} - {1}", m.AccCodes, m.AccName).ToLower().Contains(acc_code.ToLower())).Select(m1 => new { m1.AccCodes, AccFullName = string.Format("{0} - {1}", m1.AccCodes, m1.AccName) }).ToList();
            return Json(acc, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllAccCode(string TrxDate, string acc_code)
        {
            IEnumerable<AccCodeViewModel> chart;

            var officeId = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            DateTime dt = DateTime.Now;
            if (TrxDate != null)
            {
                string[] datesplit = TrxDate.Split('/');
                var day = datesplit[0];
                var month = datesplit[1];
                var year = datesplit[2];

                dt = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));

            }
            var param1 = new { @OfficeId = officeId, @TrxDate = dt.Date, @AccCode = "" };
            var LoanInstallMent = ultimateReportService.GetAllAccountCodeList(param1);

            List<AccCodeViewModel> List_ViewModel = new List<AccCodeViewModel>();
            List_ViewModel = LoanInstallMent.Tables[0].AsEnumerable()
            .Select(row => new AccCodeViewModel
            {
                //public string AccCode { get; set; }
                AccCodes = row.Field<string>("AccCode"),

                //public string AccName { get; set; }
                AccName = row.Field<string>("AccName"),

            }).ToList();


            chart = List_ViewModel;


            /*
            var BankCode = accChartService.GetByAccCode(applicationSettingsService.GetAll().Where(c => c.OfficeID == LoginUserOfficeID).FirstOrDefault().BankAccount);

            if (TransactionType == "Bc")
            {
                chart = accChartService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID && m.IsTransaction == true && m.IsActive == true && m.OfficeLevel >= OfficeLevel && m.ModuleID == 1 && m.SecondLevel == BankCode.SecondLevel).ToList();
            }
            else if (TransactionType == "Ca")
            {
                chart = accChartService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID && m.IsTransaction == true && m.IsActive == true && m.OfficeLevel >= OfficeLevel && m.ModuleID == 1 && m.SecondLevel != BankCode.SecondLevel).ToList();
            }
            else if (TransactionType == "Ba")
            {
                chart = accChartService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID && m.IsTransaction == true && m.IsActive == true && m.OfficeLevel >= OfficeLevel && m.ModuleID == 1 && m.SecondLevel != BankCode.SecondLevel).ToList();
            }
            //else if (TransactionType == "Ca")
            //{
            //    chart = accChartService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID && m.IsTransaction == true && m.IsActive == true && m.OfficeLevel >= OfficeLevel && m.ModuleID == 1).ToList();
            //}
            else
            {
                chart = accChartService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID && m.IsTransaction == true && m.IsActive == true && m.OfficeLevel >= OfficeLevel && m.ModuleID == 1).ToList();
            }

            */

            var chartList = new List<AccCodeViewModel>();

            chartList = chart.ToList();

            var acc = chartList.Where(m => string.Format("{0} - {1}", m.AccCodes, m.AccName).ToLower().Contains(acc_code.ToLower())).Select(m1 => new { m1.AccCodes, AccFullName = string.Format("{0} - {1}", m1.AccCodes, m1.AccName) }).ToList();
            return Json(acc, JsonRequestBehavior.AllowGet);
        }



        public JsonResult CreateNewAccCode
       (
             string hdnTrxMasterID
           , string hdnTrxDetailsID
           , string filterValue2
           , string txtCredit2
           , string txtDebit2
           , string txtTrxDate2
           , string txtTrxDate

       )
        {
            string result = "Data Saved Successfully"; ;
            try
            {
                Int64 CreateUser = Convert.ToInt64(LoggedInOrganizationID.ToString());

                DateTime dt = DateTime.Now;
                if (txtTrxDate != null)
                {
                    string[] datesplit = txtTrxDate.Split('/');
                    var day = datesplit[0];
                    var month = datesplit[1];
                    var year = datesplit[2];

                    dt = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));
                }


                var param = new
                {
                    hdnTrxMasterID = hdnTrxMasterID
                  ,
                    hdnTrxDetailsID = hdnTrxDetailsID
                  ,
                    txtAccCode = filterValue2
                  ,
                    txtCredit = txtCredit2
                  ,
                    txtDebit = txtDebit2
                  ,
                    txtTrxDate = dt.Date.ToString("dd/MMM/yy") //txtTrxDate
                  ,
                    CreateUser = SessionHelper.LoginUserEmployeeID
                  ,
                    CreateDate = DateTime.Now
                };
                // var val = ultimateReportService.UpdateAccountInfoNewAdd(param);
                var val = ultimateReportService.UpdateAccountInfoChangedInsert(param);


            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult UpdatePOPUPDataChanged(string hdnTrxMasterID, string hdnTrxDetailsID, string txtAccCode, string txtCredit, string txtDebit, string txtTrxDate)
        {

            string result = "Data Update Successfully";
            try
            {
                DateTime dt = DateTime.Now;
                if (txtTrxDate != null)
                {
                    string[] datesplit = txtTrxDate.Split('/');
                    var day = datesplit[0];
                    var month = datesplit[1];
                    var year = datesplit[2];

                    dt = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day));

                }

                var param = new
                {
                    hdnTrxMasterID = hdnTrxMasterID
                  ,
                    hdnTrxDetailsID = hdnTrxDetailsID
                  ,
                    txtAccCode = txtAccCode
                  ,
                    txtCredit = txtCredit
                  ,
                    txtDebit = txtDebit
                  ,
                    txtTrxDate = dt.Date.ToString("dd/MMM/yy") //txtTrxDate        
                  ,
                    CreateUser = SessionHelper.LoginUserEmployeeID
                  ,
                    CreateDate = dt.Date.ToString("dd/MMM/yy")
                };
                var val = ultimateReportService.UpdateAccountInfoChanged(param);

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion JCF
    } // End of Class
}// End of Namespace