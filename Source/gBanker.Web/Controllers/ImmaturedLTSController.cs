using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System.Data;
using System.IO;
using Microsoft.AspNet.Identity.EntityFramework;
using BasicDataAccess;

namespace gBanker.Web.Controllers
{
    public class ImmaturedLTSController : BaseController
    {


        #region Variables
        private readonly IEmployeeService employeeService;
        private readonly IProductService productService;
        private readonly ICenterService centerService;
        private readonly IOfficeService officeService;
        private readonly IMemberService memberService;
        private readonly IGroupwiseReportService groupwiseReportService;
        private readonly IUltimateReportService unlimitedReportService;
        private readonly IPOMISReportService pomisReportService;
        private readonly IWeeklyReportService weeklyReportService;

        private readonly ILoanTrxService loanTrxService;
        private readonly ILoanSummaryService loanSummaryService;
        private readonly IDailyLoanTrxService dailyLoanTrxService;

        private readonly ISavingTrxService savingTrxService;
        private readonly ISavingSummaryService savingSummaryService;
        private readonly IDailySavingTrxService dailySavingTrxService;
        private readonly ISpecialLoanCollectionService specialLoanCollectionService;


        public ImmaturedLTSController(IEmployeeService employeeService, IOfficeService officeService, IProductService productService, ICenterService centerService, IMemberService memberService, IGroupwiseReportService groupwiseReportService, IUltimateReportService unlimitedReportService, IPOMISReportService pomisReportService, IWeeklyReportService weeklyReportService, ILoanTrxService loanTrxService, ILoanSummaryService loanSummaryService, IDailyLoanTrxService dailyLoanTrxService, ISavingTrxService savingTrxService, ISavingSummaryService savingSummaryService, IDailySavingTrxService dailySavingTrxService, ISpecialLoanCollectionService specialLoanCollectionService)
        {
            this.employeeService = employeeService;
            this.officeService = officeService;
            this.productService = productService;
            this.centerService = centerService;
            this.memberService = memberService;
            this.groupwiseReportService = groupwiseReportService;
            this.unlimitedReportService = unlimitedReportService;
            this.pomisReportService = pomisReportService;
            this.weeklyReportService = weeklyReportService;

            this.loanTrxService = loanTrxService;
            this.loanSummaryService = loanSummaryService;
            this.dailyLoanTrxService = dailyLoanTrxService;

            this.savingTrxService = savingTrxService;
            this.savingSummaryService = savingSummaryService;
            this.dailySavingTrxService = dailySavingTrxService;
            this.specialLoanCollectionService = specialLoanCollectionService;
        }
        #endregion


        public JsonResult GETSavingMatureList_Mature(int jtStartIndex, int jtPageSize, string jtSorting, int CenterID = 0, long MemberID = 0, int ProductID = 0, int NoOfAccount = 0, string Option = "")
        {
            try
            {
                SavingTrxViewModel SavingTrxVM = new SavingTrxViewModel();
                SavingTrxVM.CenterID = Convert.ToInt32(CenterID);
                SavingTrxVM.ProductID = Convert.ToInt16(ProductID);
                SavingTrxVM.MemberID = Convert.ToInt64(MemberID);
                SavingTrxVM.NoOfAccount = Convert.ToInt32(NoOfAccount);
                var SavingSummaryObj = savingSummaryService.GetSingleRow((int)SessionHelper.LoginUserOfficeID, SavingTrxVM.ProductID, SavingTrxVM.CenterID, SavingTrxVM.MemberID, SavingTrxVM.NoOfAccount); //.SingleOrDefault();
                List<MatureLTSViewModel> List_ViewModel = new List<MatureLTSViewModel>();
                List<SavingsLedgerViewModel> List_ViewModel2 = new List<SavingsLedgerViewModel>();
                var param = new
                {
                    SavingSummaryId = SavingSummaryObj.SavingSummaryID,
                    MatureDate = SessionHelper.TransactionDate,
                    ProductID = SavingTrxVM.ProductID,
                    SaveYesNO = 0, // Save 1 | SELECT 0
                    OfficeID = SessionHelper.LoginUserOfficeID,
                    CreateUser = SessionHelper.LoggedInEmployeeID
                };
                var empList = groupwiseReportService.GetDataDataseAccess(param, "getMatureLTS");

                List_ViewModel = empList.Tables[1].AsEnumerable()
                .Select(row => new MatureLTSViewModel
                {
                    MatureLTSID = row.Field<long>("MatureLTSID"),
                    SavingSummaryID = row.Field<long>("SavingSummaryID"),
                    MembersTotalDeposit = row.Field<decimal>("MembersTotalDeposit"),
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
                    Duration = row.Field<int>("Duration"),
                    InstallmentPaid = row.Field<int>("InstallmentPaid"),
                    ValidForMSDS = row.Field<int>("ValidForMSDS")
                }).ToList();

                List_ViewModel2 = empList.Tables[0].AsEnumerable()
               .Select(row => new SavingsLedgerViewModel
               {
                   MemberID = row.Field<long>("MemberID"),
                   Withdrawal = row.Field<decimal>("Withdrawal"),
                   Savings = row.Field<decimal>("Savings"),
                   Balance = row.Field<decimal>("Balance"),
                   MonthlyInterest = row.Field<decimal>("MonthlyInterest"),
                   Penalty = row.Field<decimal>("Penalty"),
                   running_total = row.Field<decimal>("running_total"),

                   InstallmentDate = row.Field<DateTime>("InstallmentDate"),
                   ReportStartDate = row.Field<DateTime>("ReportStartDate"),
                   AccountOpeningDate = row.Field<DateTime>("AccountOpeningDate"),


                   OfficeID = row.Field<int>("OfficeID"),
                   CenterID = row.Field<int>("CenterID"),
                   EmployeeID = row.Field<int>("EmployeeID"),
                   ProductID = row.Field<int>("ProductID"),
                   TranType = row.Field<int>("TranType"),

                   ZoneCode = row.Field<string>("ZoneCode"),
                   ZoneName = row.Field<string>("ZoneName"),
                   AreaCode = row.Field<string>("AreaCode"),
                   AreaName = row.Field<string>("AreaName"),
                   BranchCode = row.Field<string>("BranchCode"),
                   BranchName = row.Field<string>("BranchName"),
                   CenterCode = row.Field<string>("CenterCode"),
                   CenterName = row.Field<string>("CenterName"),
                   StaffID = row.Field<string>("StaffID"),
                   StaffName = row.Field<string>("StaffName"),
                   LoaneeNo = row.Field<string>("LoaneeNo"),
                   LoaneeName = row.Field<string>("LoaneeName"),
                   ProductCode = row.Field<string>("ProductCode"),
                   ItemName = row.Field<string>("ItemName"),
                   NoOfAccount = row.Field<string>("NoOfAccount"),
                   TrxType = row.Field<string>("TrxType"),
                   AccountNo = row.Field<string>("AccountNo")


               }).ToList();

                var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                var currentPageRecords2 = List_ViewModel2.Skip(jtStartIndex).Take(jtPageSize);
                if (Option == "Summary")
                    return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.Count() });
                else
                    return Json(new { Result = "OK", Records = currentPageRecords2, TotalRecordCount = List_ViewModel2.Count() });
                //return Json(new { Result = "OK",  TotalRecordCount = totalCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }



        [HttpPost]
        public ActionResult SaveCorrectionMatureLTS_Mature(string center, string member, string product, string noOfAccount)
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
                    MatureDate = SessionHelper.TransactionDate,
                    ProductID = SavingTrxVM.ProductID,
                    SaveYesNO = 1, // Save 1 | SELECT 0
                    OfficeID = SessionHelper.LoginUserOfficeID,
                    CreateUser = SessionHelper.LoggedInEmployeeID
                };

                //  OfficeID = SessionHelper.LoginUserOfficeID, CenterID = center };
                var alldata = groupwiseReportService.GetDataDataseAccess(param, "getMatureLTS");
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }



        public JsonResult GetNoOfAccountListByProductandMemberFromSavingTrx_Mature(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<MemberwiseProductAndAccountNoViewModel> List_AccountNoMemberandProductwise = new List<MemberwiseProductAndAccountNoViewModel>();

                var param = new { Qtype = 2, MemberID = MemberID, ProductID = ProductID };
                var alldata = groupwiseReportService.GetDataDataseAccess(param, "MemberwiseProductAndAccountNoforDropDown__SavingSummary");
                //var alldata = groupwiseReportService.GetDataDataseAccess(param, "MemberwiseProductAndAccountNoforDropDown");

                List_AccountNoMemberandProductwise = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndAccountNoViewModel
                {
                    ProductID = row.Field<string>("ProductID"),
                    NoOfAccount = row.Field<string>("NoOfAccount"),
                    //ProductName = row.Field<string>("ProductName")

                }).ToList();

                //return Json(List_MemberwiseProduct.ToList(), JsonRequestBehavior.AllowGet);
                return Json(List_AccountNoMemberandProductwise, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetProductListByMemberSavingInstallment_Mature(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<MemberwiseProductAndAccountNoViewModel> List_MemberwiseProduct = new List<MemberwiseProductAndAccountNoViewModel>();

                var param = new { Qtype = 1, MemberID = MemberID, ProductID = 0, TransactionDate = SessionHelper.TransactionDate };
                var alldata = groupwiseReportService.GetDataDataseAccess(param, "MemberwiseProductAndAccountNoforDropDown__SavingInstallment_Mature");
                //var alldata = groupwiseReportService.GetDataDataseAccess(param, "MemberwiseProductAndAccountNoforDropDown");

                List_MemberwiseProduct = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndAccountNoViewModel
                {
                    ProductID = row.Field<string>("ProductID"),
                    //LoanTerm = row.Field<string>("LoanTerm"),
                    ProductName = row.Field<string>("ProductName")

                }).ToList();

                return Json(List_MemberwiseProduct, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        public JsonResult GetMemberListForSInstallment_Mature(int centerId)
        {
            try
            {
                List<GetMemberListViewModel> List_Members = new List<GetMemberListViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId, TransactionDate = SessionHelper.TransactionDate };
                var alldata = groupwiseReportService.GetDataDataseAccess(param, "GetMemberList_Dropdown_SavingInstallment_Mature");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new GetMemberListViewModel
                {
                    MemberID = row.Field<string>("MemberID"),
                    //LoanTerm = row.Field<string>("LoanTerm"),
                    MemberName = row.Field<string>("MemberName")

                }).ToList();

                return Json(List_Members, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }// END Saving Installment

        public ActionResult MaturedLTSList_Mature()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["LoggedInUser"] = LoggedInEmployee.EmpName;
            ViewData["LoggedInOfficeID"] = LoggedInEmployee.OfficeID;

            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            ViewData["ProductListByMember"] = items;
            ViewData["LoanTermList"] = items;

            return View();
        }

        public JsonResult GetCenterList_Mature()
        {
            //var getCenter = centerService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.OrgID == LoggedInOrganizationID).OrderBy(e => e.CenterCode);
            //var viewCenter = getCenter.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.CenterID.ToString(),
            //    Text = x.CenterCode.ToString() + ", " + x.CenterName.ToString()
            //    //Text = x.CenterName.ToString()
            //});
            //var center_items = new List<SelectListItem>();
            //if (viewCenter.ToList().Count > 0)
            //{
            //    center_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            //}
            List<CenterViewModel> List_CenterViewModel = new List<CenterViewModel>();
            var paramFOWISE = new { OfficeId = LoginUserOfficeID };
            var div_items = groupwiseReportService.GetDataDataseAccess(paramFOWISE, "GetOnlyCenter");

            List_CenterViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new CenterViewModel
            {
                CenterID = row.Field<int>("CenterID"),
                CenterCode = row.Field<string>("CenterCode"),
                CenterName = row.Field<string>("CenterName")
            }).ToList();

            var viewCenter = List_CenterViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterCode.ToString() + " " + x.CenterName.ToString()
            });
            var center_items = new List<SelectListItem>();
            center_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            center_items.AddRange(viewCenter);
            //center_items.AddRange(viewCenter);
            return Json(center_items, JsonRequestBehavior.AllowGet);
        }





        // GET: ImmaturedLTS
        public ActionResult ImmaturedLTSList()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["LoggedInUser"] = LoggedInEmployee.EmpName;
            ViewData["LoggedInOfficeID"] = LoggedInEmployee.OfficeID;

            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            ViewData["ProductListByMember"] = items;
            ViewData["LoanTermList"] = items;

            return View();
        }

        public JsonResult GetNoOfAccountListByProductandMemberFromSavingTrx(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<MemberwiseProductAndAccountNoViewModel> List_AccountNoMemberandProductwise = new List<MemberwiseProductAndAccountNoViewModel>();

                var param = new { Qtype = 2, MemberID = MemberID, ProductID = ProductID };
                var alldata = groupwiseReportService.GetDataDataseAccess(param, "MemberwiseProductAndAccountNoforDropDown__SavingSummary");
                //var alldata = groupwiseReportService.GetDataDataseAccess(param, "MemberwiseProductAndAccountNoforDropDown");

                List_AccountNoMemberandProductwise = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndAccountNoViewModel
                {
                    ProductID = row.Field<string>("ProductID"),
                    NoOfAccount = row.Field<string>("NoOfAccount"),
                    //ProductName = row.Field<string>("ProductName")

                }).ToList();

                //return Json(List_MemberwiseProduct.ToList(), JsonRequestBehavior.AllowGet);
                return Json(List_AccountNoMemberandProductwise, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //public JsonResult GetMemberListForSInstallment(int centerId)
        //{
        //    try
        //    {
        //        List<GetMemberListViewModel> List_Members = new List<GetMemberListViewModel>();

        //        var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId, TransactionDate = SessionHelper.TransactionDate };
        //        var alldata = groupwiseReportService.GetDataDataseAccess(param, "GetMemberList_Dropdown_SavingInstallment_Mature");

        //        List_Members = alldata.Tables[0].AsEnumerable()
        //        .Select(row => new GetMemberListViewModel
        //        {
        //            MemberID = row.Field<string>("MemberID"),
        //            //LoanTerm = row.Field<string>("LoanTerm"),
        //            MemberName = row.Field<string>("MemberName")

        //        }).ToList();

        //        return Json(List_Members, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}// END Saving Installment

        //public JsonResult GetProductListByMemberSavingInstallment(int Qtype, string MemberID, string ProductID)
        //{
        //    try
        //    {
        //        List<MemberwiseProductAndAccountNoViewModel> List_MemberwiseProduct = new List<MemberwiseProductAndAccountNoViewModel>();

        //        var param = new { Qtype = 1, MemberID = MemberID, ProductID = 0, TransactionDate = SessionHelper.TransactionDate };
        //        var alldata = groupwiseReportService.GetDataDataseAccess(param, "MemberwiseProductAndAccountNoforDropDown__SavingInstallment_Mature");
        //        //var alldata = groupwiseReportService.GetDataDataseAccess(param, "MemberwiseProductAndAccountNoforDropDown");

        //        List_MemberwiseProduct = alldata.Tables[0].AsEnumerable()
        //        .Select(row => new MemberwiseProductAndAccountNoViewModel
        //        {
        //            ProductID = row.Field<string>("ProductID"),
        //            //LoanTerm = row.Field<string>("LoanTerm"),
        //            ProductName = row.Field<string>("ProductName")

        //        }).ToList();

        //        return Json(List_MemberwiseProduct, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [HttpPost]
        public ActionResult SaveCorrectionMatureLTS(
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
                var alldata = groupwiseReportService.GetDataDataseAccess(param, "getMatureLTS");
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }// END

        public JsonResult GetCenterList()
        {
            //var getCenter = centerService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.OrgID == LoggedInOrganizationID).OrderBy(e => e.CenterCode);
            //var viewCenter = getCenter.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.CenterID.ToString(),
            //    Text = x.CenterCode.ToString() + ", " + x.CenterName.ToString()
            //    //Text = x.CenterName.ToString()
            //});
            //var center_items = new List<SelectListItem>();
            //if (viewCenter.ToList().Count > 0)
            //{
            //    center_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            //}
            List<CenterViewModel> List_CenterViewModel = new List<CenterViewModel>();
            var paramFOWISE = new { OfficeId = LoginUserOfficeID };
            var div_items = groupwiseReportService.GetDataDataseAccess(paramFOWISE, "GetOnlyCenter");

            List_CenterViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new CenterViewModel
            {
                CenterID = row.Field<int>("CenterID"),
                CenterCode = row.Field<string>("CenterCode"),
                CenterName = row.Field<string>("CenterName")
            }).ToList();

            var viewCenter = List_CenterViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterCode.ToString() + " " + x.CenterName.ToString()
            });
            var center_items = new List<SelectListItem>();
            center_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            center_items.AddRange(viewCenter);
            //center_items.AddRange(viewCenter);
            return Json(center_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMemberListForSInstallment(int centerId)
        {
            try
            {
                List<GetMemberListViewModel> List_Members = new List<GetMemberListViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId };
                var alldata = groupwiseReportService.GetDataDataseAccess(param, "GetMemberList_Dropdown_SavingInstallment");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new GetMemberListViewModel
                {
                    MemberID = row.Field<string>("MemberID"),
                    //LoanTerm = row.Field<string>("LoanTerm"),
                    MemberName = row.Field<string>("MemberName")

                }).ToList();

                return Json(List_Members, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }// END Saving Installment
        public JsonResult GetProductListByMemberSavingInstallment(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<MemberwiseProductAndAccountNoViewModel> List_MemberwiseProduct = new List<MemberwiseProductAndAccountNoViewModel>();

                var param = new { Qtype = 1, MemberID = MemberID, ProductID = 0 };
                var alldata = groupwiseReportService.GetDataDataseAccess(param, "MemberwiseProductAndAccountNoforDropDown__SavingInstallment");
                //var alldata = groupwiseReportService.GetDataDataseAccess(param, "MemberwiseProductAndAccountNoforDropDown");

                List_MemberwiseProduct = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndAccountNoViewModel
                {
                    ProductID = row.Field<string>("ProductID"),
                    //LoanTerm = row.Field<string>("LoanTerm"),
                    ProductName = row.Field<string>("ProductName")

                }).ToList();

                return Json(List_MemberwiseProduct, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
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
                    SavingSummaryId = SavingSummaryObj.SavingSummaryID,
                    ImmatureDate = SessionHelper.TransactionDate,
                    ProductID = SavingTrxVM.ProductID,
                    SaveYesNO = 0, // Save 1 | SELECT 0
                    OfficeID = SessionHelper.LoginUserOfficeID,
                    CreateUser = SessionHelper.LoggedInEmployeeID
                };
                var empList = groupwiseReportService.GetDataDataseAccess(param, "getImmatureLTS");

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
                    OpeningDate=row.Field<DateTime>("OpeningDate"),
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
                var alldata = groupwiseReportService.GetDataDataseAccess(param, "getImmatureLTS");
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }// END


    }// End Class
}