using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using gBanker.Service.ReportServies;
using gBanker.Web.ViewModels;
using System.Data;
using gBanker.Service;
using gBanker.Web.Helpers;

namespace gBanker.Web.Controllers
{
    public class VirtualImmaturedLSTController : BaseController
    {

        #region Variables
       
        private readonly IUltimateReportService ultimateReportService;
        private readonly ISavingSummaryService savingSummaryService;
        private readonly IGroupwiseReportService groupwiseReportService;

        public VirtualImmaturedLSTController(  IUltimateReportService ultimateReportService, ISavingSummaryService savingSummaryService, IGroupwiseReportService groupwiseReportService)
        {
            this.ultimateReportService = ultimateReportService;
            this.savingSummaryService = savingSummaryService;
            this.groupwiseReportService = groupwiseReportService;
        }

        #endregion

        // GET: ImmaturedLTS
        public ActionResult VirtualImmaturedLTSList()
        {
            VirtualImmatureListViewModel model = new VirtualImmatureListViewModel();

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["LoggedInUser"] = LoggedInEmployee.EmpName;
            ViewData["LoggedInOfficeID"] = LoginUserOfficeID;

            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            ViewData["ProductListByMember"] = items;
            ViewData["LoanTermList"] = items;

            model.OfficeId = (int)LoginUserOfficeID;

            return View(model);
        }

        public JsonResult GetVirtualImmatureList(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue, string typeFilterColumn,
            string OfficeID         ,
            string CenterID         ,
            string MemberID         ,
            string ProductID        ,
            string NoOfAccount      ,
            string InstallmentNo   

            )
        {
            try
            {
               
                List<VirtualImmatureListViewModel> List_EmployeeViewModel = new List<VirtualImmatureListViewModel>();
                var param = new {

                            OfficeID            = OfficeID      ,
                            CenterID            = CenterID      ,
                            MemberID            = MemberID      ,
                            ProductID           = ProductID     ,
                            NoOfAccount         = NoOfAccount   ,
                            InstallmentNo       = InstallmentNo ,
                            CalcDate            =TransactionDate //change on 13 jun2022

                };
               var  DataList = new DataSet();
                DataList = ultimateReportService.GetDataWithParameter(param, "getVirtualImmatureList");

                List_EmployeeViewModel = DataList.Tables[0].AsEnumerable()
                .Select(row => new VirtualImmatureListViewModel
                {
                    SavingSummaryID     = row.Field<Int64>("SavingSummaryID"),
                    OfficeId            = row.Field<int>("OfficeID"),
                    CenterID            = row.Field<int>("CenterID"),
                    ProductID           = row.Field<int>("ProductID"),
                    NoOfAccount         = row.Field<int>("NoOfAccount"),
                    OpeningDate         = row.Field<DateTime>("OpeningDate"),
                    OpeningDateMsg      = row.Field<string>("OpeningDateMsg"),
                    CenterCode          = row.Field<string>("CenterCode"),
                    Memberid            = row.Field<long>("Memberid"),
                    MemberCode          = row.Field<string>("MemberCode"),
                    MemberName          = row.Field<string>("MemberName"),
                    Balance             = row.Field<decimal>("Balance"),
                    SavingInstallment   = row.Field<decimal>("SavingInstallment"),  // ADDED.. This
                    SerialNo            = row.Field<int>("InsNo"),
                    InsNo               = row.Field<int>("InsNo"),
                    
                    InstallmentDateMsg = row.Field<string>("InstallmentDateMsg"),
                    maxInstallmentNo = row.Field<int>("maxInstallmentNo"),

                }).ToList();

                var currentPageRecords = List_EmployeeViewModel.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords.OrderBy(x=> x.InsNo).ToList(), TotalRecordCount = List_EmployeeViewModel.LongCount(), JsonRequestBehavior.AllowGet });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// list 

        public JsonResult saveInstallment(string MemberIdInsNo, string SavingInstallmentValue, string InstallmentDate, 
            long SavingSummaryID, int OfficeId, int CenterID, int ProductID, int NoOfAccount, string OpeningDate,
            string CenterCode, long Memberid, string MemberCode, string MemberName, decimal Balance

            )
        {

            string[] parts = MemberIdInsNo.Split('_');
            var memberId = parts[0];
            var installmentNo = parts[1];

            if (Convert.ToInt64(memberId) > 0 && SavingInstallmentValue != "")
            {
                var v = 0;

                var VImmatureParams = new  {
                      SavingSummaryID       = SavingSummaryID
                    , OfficeID              = OfficeId
                    , CenterID              = CenterID
                    , ProductID             = ProductID
                    , NoOfAccount           = NoOfAccount
                    , OpeningDate           = OpeningDate
                    , CenterCode            = CenterCode
                    , Memberid              = Memberid
                    , MemberCode            = MemberCode
                    , SavingInstallment     = SavingInstallmentValue
                    , MemberName            = MemberName
                    , InstallmentDate       = InstallmentDate
                    , Balance               = Balance
                    , InsNo                 = installmentNo


                };
                var save = ultimateReportService.GetDataWithParameter(VImmatureParams, "INSERT_VirtualImmature");
            }
            return Json(SavingInstallmentValue, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GETSavingImatureList(int jtStartIndex, int jtPageSize, string jtSorting, int CenterID = 0, long MemberID = 0, int ProductID = 0, int NoOfAccount = 0, string Option = "")
        {
            try
            {
                SavingTrxViewModel SavingTrxVM = new SavingTrxViewModel();
                SavingTrxVM.CenterID = Convert.ToInt32(CenterID);
                SavingTrxVM.ProductID = Convert.ToInt16(ProductID);
                SavingTrxVM.MemberID = Convert.ToInt64(MemberID);
                SavingTrxVM.NoOfAccount = Convert.ToInt32(NoOfAccount);
                var SavingSummaryObj = savingSummaryService.GetSingleRow((int)SessionHelper.LoginUserOfficeID, SavingTrxVM.ProductID, SavingTrxVM.CenterID, SavingTrxVM.MemberID, SavingTrxVM.NoOfAccount); //.SingleOrDefault();
                List<SavingInstallmentViewModel> List_ViewModel = new List<SavingInstallmentViewModel>();
                var param = new
                {
                    SavingSummaryId = SavingSummaryObj != null? SavingSummaryObj.SavingSummaryID : 0  ,
                    ImmatureDate = SessionHelper.TransactionDate,
                    ProductID = SavingSummaryObj != null ?  SavingTrxVM.ProductID : 0,
                    SaveYesNO = 0, // Save 1 | SELECT 0
                    OfficeID = SessionHelper.LoginUserOfficeID,
                    CreateUser = SessionHelper.LoggedInEmployeeID
                };
                var empList = groupwiseReportService.GetDataDataseAccess(param, "GenerateVirtualImmatureLTS");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new SavingInstallmentViewModel
                {
                    ImmatureLTSID = row.Field<long>("ImmatureLTSID"),
                    SavingSummaryID = row.Field<long>("SavingSummaryID"),
                    Calcnterest = row.Field<decimal>("Calcnterest"),
                    Deposit = row.Field<decimal>("Deposit"),
                    WithDrawal = row.Field<decimal>("WithDrawal"),
                    Interest = row.Field<decimal>("Interest"),
                    Transffered = row.Field<bool>("Transffered"),
                    TransDate = row.Field<DateTime>("TransDate"),
                    ProductID = row.Field<int>("ProductID"),
                    OfficeID = row.Field<int>("OfficeId"),
                    CreateUser = row.Field<string>("CreateUser"),
                    CreateDate = row.Field<DateTime>("CreateDate"),
                    CurrentInterest = row.Field<decimal>("CurrentInterest"),
                    InterestRate = row.Field<decimal>("InterestRate"),
                    WithdrawalRate = row.Field<decimal>("WithdrawalRate"),
                    OpeningDate = row.Field<DateTime>("OpeningDate"),
                    SavingInstallment = row.Field<decimal>("SavingInstallment"),
                    Duration = row.Field<int>("Duration")
                }).ToList();

                var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.Count() });
                //return Json(new { Result = "OK",  TotalRecordCount = totalCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        [HttpPost]
        public ActionResult SaveCorrection(
       string center,
       string member,
       string product,
       string noOfAccount
       )
        {
            try
            {

                SavingTrxViewModel SavingTrxVM = new SavingTrxViewModel();

                SavingTrxVM.CenterID = Convert.ToInt32(center);
                SavingTrxVM.ProductID = Convert.ToInt16(product);
                SavingTrxVM.MemberID = Convert.ToInt64(member);
                SavingTrxVM.NoOfAccount = Convert.ToInt32(noOfAccount);
                var SavingSummaryObj = savingSummaryService.GetSingleRow((int)SessionHelper.LoginUserOfficeID, SavingTrxVM.ProductID, SavingTrxVM.CenterID, SavingTrxVM.MemberID, SavingTrxVM.NoOfAccount); //.SingleOrDefault();
                var param = new
                {
                    SavingSummaryId = SavingSummaryObj.SavingSummaryID,
                    ImmatureDate = SessionHelper.TransactionDate,
                    ProductID = SavingTrxVM.ProductID,
                    SaveYesNO = 1, // Save 1 | SELECT 0
                    OfficeID = SessionHelper.LoginUserOfficeID,
                    CreateUser = SessionHelper.LoggedInEmployeeID
                };

                //  OfficeID = SessionHelper.LoginUserOfficeID, CenterID = center };
                var alldata = groupwiseReportService.GetDataDataseAccess(param, "GenerateVirtualImmatureLTS");
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }// END

    }// End Class
}// ENd Namespace