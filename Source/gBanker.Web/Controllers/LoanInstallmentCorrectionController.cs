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
    public class LoanInstallmentCorrectionController : BaseController
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
        private readonly ISpecialLoanCollectionService specialLoanCollectionService;
        public LoanInstallmentCorrectionController(IEmployeeService employeeService, IOfficeService officeService, IProductService productService, ICenterService centerService, IMemberService memberService, IGroupwiseReportService groupwiseReportService, IUltimateReportService unlimitedReportService, IPOMISReportService pomisReportService, IWeeklyReportService weeklyReportService, ILoanTrxService loanTrxService, ILoanSummaryService loanSummaryService, IDailyLoanTrxService dailyLoanTrxService, ISpecialLoanCollectionService specialLoanCollectionService)
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
            this.specialLoanCollectionService = specialLoanCollectionService;
        }
        #endregion

        #region Methods
        public JsonResult GetDailyLoanTrxData(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                long totalCount;
                var allDailyLoanTrxes = dailyLoanTrxService.GetDailyLoanTrxDetailPaged(filterColumn, filterValue, jtStartIndex, jtPageSize, out totalCount, LoggedInOrganizationID,LoginUserOfficeID);
                var currentPageRecords = Mapper.Map<IEnumerable<DailyLoanTrx>, IEnumerable<DailyLoanTrxViewModel>>(allDailyLoanTrxes);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        #region Block
        //protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        //{
        //    return new JsonResult()
        //    {
        //        Data = data,
        //        ContentType = contentType,
        //        ContentEncoding = contentEncoding,
        //        JsonRequestBehavior = behavior,
        //        MaxJsonLength = Int32.MaxValue
        //    };
        //}
        //public JsonResult GetOfficeList()
        //{
        //    var getOffice = officeService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.OrgID == LoggedInOrganizationID && s.OfficeLevel == 4).OrderBy(e => e.OfficeCode);
        //    var viewOffice = getOffice.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.OfficeID.ToString(),
        //        Text = x.OfficeName.ToString()
        //    });
        //    var office_items = new List<SelectListItem>();
        //    if (viewOffice.ToList().Count > 0)
        //    {
        //        office_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
        //    }
        //    office_items.AddRange(viewOffice);
        //    return Json(office_items, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetProductListByMemberFromSavingTrx(int Qtype, string MemberID, string ProductID)
        //{
        //    try
        //    {
        //        List<MemberwiseProductAndAccountNoViewModel> List_MemberwiseProduct = new List<MemberwiseProductAndAccountNoViewModel>();

        //        var param = new { Qtype = 1, MemberID = MemberID, ProductID = 0 };
        //        var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "MemberwiseProductAndAccountNoforDropDown");

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
        //public JsonResult GetProductListByMemberFromSavingTrxByMemberCode(int Qtype, string MemberID, string ProductID)
        //{
        //    try
        //    {
        //        List<MemberwiseProductAndAccountNoViewModel> List_MemberwiseProduct = new List<MemberwiseProductAndAccountNoViewModel>();

        //        var param = new { Qtype = 1, MemberID = MemberID, ProductID = 0 };
        //        var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "MemberwiseProductAndAccountNoforDropDown");

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
        //public JsonResult GetNoOfAccountListByProductandMemberFromSavingTrx(int Qtype, string MemberID, string ProductID)
        //{
        //    try
        //    {
        //        List<MemberwiseProductAndAccountNoViewModel> List_AccountNoMemberandProductwise = new List<MemberwiseProductAndAccountNoViewModel>();

        //        var param = new { Qtype = 2, MemberID = MemberID, ProductID = ProductID };
        //        var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "MemberwiseProductAndAccountNoforDropDown");

        //        List_AccountNoMemberandProductwise = alldata.Tables[0].AsEnumerable()
        //        .Select(row => new MemberwiseProductAndAccountNoViewModel
        //        {
        //            ProductID = row.Field<string>("ProductID"),
        //            NoOfAccount = row.Field<string>("NoOfAccount"),
        //            //ProductName = row.Field<string>("ProductName")

        //        }).ToList();

        //        //return Json(List_MemberwiseProduct.ToList(), JsonRequestBehavior.AllowGet);
        //        return Json(List_AccountNoMemberandProductwise, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //public JsonResult GetDataForDailyLoanTrxTableSave(string CenterID, string MemberID, string ProductID, string Loanterm)
        //{
        //    try
        //    {
        //        List<LoanInstallmentCorrectionViewmodel> InstallmentAmount = new List<LoanInstallmentCorrectionViewmodel>();

        //        var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = CenterID, MemberID = MemberID, ProductID = ProductID, Loanterm = Loanterm };
        //        var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "Proc_get_LoanSummary_Installment_Correction");

        //        InstallmentAmount = alldata.Tables[0].AsEnumerable()
        //        .Select(row => new LoanInstallmentCorrectionViewmodel
        //        {
        //            LoanSummaryID = row.Field<long>("LoanSummaryID"),
        //            LoanNo = row.Field<string>("LoanNo"),
        //            OfficeID = row.Field<int>("OfficeID"),
        //            MemberID = row.Field<long>("MemberID"),
        //            MemberCode = row.Field<string>("MemberCode"),
        //            MemberName = row.Field<string>("MemberName"),
        //            ProductID = row.Field<Int16>("ProductID"),
        //            ProductCode = row.Field<string>("ProductCode"),
        //            ProductName = row.Field<string>("ProductName"),
        //            InterestCalculationMethod = row.Field<string>("InterestCalculationMethod"),
        //            CenterID = row.Field<int>("CenterID"),
        //            MemberCategoryID = row.Field<Int16>("MemberCategoryID"),
        //            LoanTerm = row.Field<int>("LoanTerm"),
        //            PurposeID = row.Field<Int16>("PurposeID"),
        //            PrincipalLoan = row.Field<decimal>("PrincipalLoan"),
        //            LoanRepaid = row.Field<decimal>("LoanRepaid"),
        //            LoanDue = row.Field<decimal>("LoanDue"),
        //            CumIntCharge = row.Field<decimal>("CumIntCharge"),
        //            IntCharge = row.Field<decimal>("IntCharge"),
        //            IntDue = row.Field<decimal>("IntDue"),
        //            Advance = row.Field<decimal>("Advance"),
        //            DueRecovery = row.Field<decimal>("DueRecovery"),
        //            TrxType = row.Field<Int16>("TrxType"),
        //            InstallmentNo = row.Field<Int16>("InstallmentNo"),
        //            InvestorID = row.Field<Int16>("InvestorID"),
        //            TotalPaid = row.Field<decimal>("TotalPaid"),
        //            CollectionStatus = row.Field<Int16>("CollectionStatus"),
        //            OrgID = row.Field<int>("OrgID"),
        //            IsActive = row.Field<Boolean>("IsActive"),
        //            CreateUser = row.Field<string>("CreateUser"),
        //            Duration = row.Field<int>("Duration"),
        //            DurationOverLoanDue = row.Field<decimal>("DurationOverLoanDue"),
        //            DurationOverIntDue = row.Field<decimal>("DurationOverIntDue")

        //        }).ToList();

        //        //return Json(List_MemberwiseProduct.ToList(), JsonRequestBehavior.AllowGet);
        //        return Json(InstallmentAmount, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //public JsonResult GetMemberListByMemberCode(int centerId)
        //{
        //    var getMember = memberService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.MemberStatus == "1" && s.OrgID == LoggedInOrganizationID && s.CenterID == centerId).OrderBy(e => e.MemberCode);
        //    var viewMember = getMember.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.MemberID.ToString(),
        //        //Text = x.MemberCode.ToString() + ", " + x.FirstName.ToString() + " " + (string.IsNullOrEmpty(x.MiddleName) ? " " : x.MiddleName.ToString()) + " " + (string.IsNullOrEmpty(x.LastName) ? " " : x.LastName.ToString())
        //        Text = x.MemberCode.ToString() + ", " + (string.IsNullOrEmpty(x.FirstName) ? " " : x.FirstName.ToString()) + " " + (string.IsNullOrEmpty(x.MiddleName) ? " " : x.MiddleName.ToString()) + " " + (string.IsNullOrEmpty(x.LastName) ? " " : x.LastName.ToString())
        //        //Text = x.MemberCode.ToString() + ", " + x.MiddleName.ToString()
        //        //Text = x.CenterName.ToString()
        //    });
        //    var member_items = new List<SelectListItem>();
        //    if (viewMember.ToList().Count > 0)
        //    {
        //        member_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
        //    }
        //    member_items.AddRange(viewMember);
        //    return Json(member_items, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetProductListByMember(long memberId)
        //{
        //    var getProduct = loanSummaryService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.OrgID == LoggedInOrganizationID && s.MemberID == memberId);
        //    var viewProduct = getProduct.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.ProductID.ToString(),
        //        Text = productService.GetById(x.ProductID).ProductCode + ", " + productService.GetById(x.ProductID).ProductName
        //    });
        //    var prod_items = new List<SelectListItem>();
        //    if (viewProduct.ToList().Count > 0)
        //    {
        //        prod_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
        //    }
        //    prod_items.AddRange(viewProduct);
        //    return Json(prod_items, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetLoanTermList(long memberId, int productId)
        //{
        //    var getLoanTerm = loanSummaryService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.OrgID == LoggedInOrganizationID && s.MemberID == memberId && s.ProductID == productId).OrderBy(e => e.LoanTerm);
        //    var viewLoanTerm = getLoanTerm.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.LoanTerm.ToString(),
        //        //Text = x.MemberCode.ToString() + ", " + x.MiddleName.ToString()
        //        //Text = x.MemberCode.ToString() + ", " + x.FirstName.ToString() + " " + x.MiddleName.ToString() + " " + x.LastName.ToString()
        //        Text = x.LoanTerm.ToString()
        //        //Text = x.CenterName.ToString()
        //    });
        //    var loanterm_items = new List<SelectListItem>();
        //    if (viewLoanTerm.ToList().Count > 0)
        //    {
        //        loanterm_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
        //    }
        //    loanterm_items.AddRange(viewLoanTerm);
        //    return Json(loanterm_items, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetNoOfAccountList(long memberId, int productId)
        //{
        //    var getLoanTerm = loanSummaryService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.OrgID == LoggedInOrganizationID && s.MemberID == memberId && s.ProductID == productId).OrderBy(e => e.LoanTerm);
        //    var viewLoanTerm = getLoanTerm.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.LoanTerm.ToString(),
        //        //Text = x.MemberCode.ToString() + ", " + x.MiddleName.ToString()
        //        //Text = x.MemberCode.ToString() + ", " + x.FirstName.ToString() + " " + x.MiddleName.ToString() + " " + x.LastName.ToString()
        //        Text = x.LoanTerm.ToString()
        //        //Text = x.CenterName.ToString()
        //    });
        //    var loanterm_items = new List<SelectListItem>();
        //    if (viewLoanTerm.ToList().Count > 0)
        //    {
        //        loanterm_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
        //    }
        //    loanterm_items.AddRange(viewLoanTerm);
        //    return Json(loanterm_items, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetEmployeeList()
        //{
        //    var getEmployee = employeeService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.OrgID == LoggedInOrganizationID).OrderBy(e => e.EmployeeCode);
        //    var viewEmployee = getEmployee.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.EmployeeID.ToString(),
        //        Text = x.EmpName.ToString()
        //    });
        //    var emp_items = new List<SelectListItem>();
        //    if (viewEmployee.ToList().Count > 0)
        //    {
        //        emp_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
        //    }
        //    emp_items.AddRange(viewEmployee);
        //    return Json(emp_items, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetProductList()
        //{
        //    var getProduct = productService.GetAll().Where(s => s.IsActive == true && s.OrgID == LoggedInOrganizationID).OrderBy(e => e.ProductCode);
        //    var viewProduct = getProduct.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.ProductID.ToString(),
        //        Text = x.ProductName.ToString()
        //    });
        //    var prod_items = new List<SelectListItem>();
        //    if (viewProduct.ToList().Count > 0)
        //    {
        //        prod_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
        //    }
        //    prod_items.AddRange(viewProduct);
        //    return Json(prod_items, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetMainProductList()
        //{
        //    try
        //    {
        //        List<ProductViewModel> List_ProductViewModel = new List<ProductViewModel>();
        //        var productList = weeklyReportService.GetMainProductList();

        //        List_ProductViewModel = productList.Tables[0].AsEnumerable()
        //        .Select(row => new ProductViewModel
        //        {
        //            MainProductCode = row.Field<string>("MainProductCode"),
        //            MainItemName = row.Field<string>("MainItemName")

        //        }).ToList();

        //        return Json(List_ProductViewModel.ToList(), JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        #endregion

        public ActionResult GetMemberListAuto(string memberid, string centerId)
        {
            var MemberByCenterSessionKey = string.Format("MemberByCenterSessionKey_{0}", centerId);
            var memberList = new List<Member>();
            if (Session[MemberByCenterSessionKey] != null)
                memberList = Session[MemberByCenterSessionKey] as List<Member>;
            else
            {
             
                List<Member> List_Members = new List<Member>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId };
                var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "GetMemberListLoanAutoComplete_WriteOff");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new Member
                {
                    MemberID = row.Field<Int64>("MemberID"),
                    //LoanTerm = row.Field<string>("LoanTerm"),
                    FirstName = row.Field<string>("MemberName"),
                    MemberCode = row.Field<string>("MemberCode"),
                }).ToList();

                //List_Members.Add(new Member() { MemberCode = "Select All", MemberID = 0 });
                Session[MemberByCenterSessionKey] = List_Members; // mbr;

                memberList = List_Members; // mbr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCenterList()
        {
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
            return Json(center_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMemberList(int centerId)
        {
            try
            {
                List<GetMemberListViewModel> List_Members = new List<GetMemberListViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId };
                var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "GetMemberList_Dropdown_LoanSummary");

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
        }
        public JsonResult GetProductListByMemberWithProcedure(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<MemberwiseProductAndLoanTermViewModel> List_MemberwiseProduct = new List<MemberwiseProductAndLoanTermViewModel>();

                var param = new { Qtype = 1, MemberID = MemberID, ProductID = 0 };
                var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "MemberwiseProductAndLoanTermforDropDown");

                List_MemberwiseProduct = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndLoanTermViewModel
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
        public JsonResult GetLoanTermListByProductandMemberWithProcedure(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<MemberwiseProductAndLoanTermViewModel> List_LoanTermMemberandProductwise = new List<MemberwiseProductAndLoanTermViewModel>();

                var param = new { Qtype = 2, MemberID = MemberID, ProductID = ProductID };
                var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "MemberwiseProductAndLoanTermforDropDown_LoanSummary");

                List_LoanTermMemberandProductwise = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndLoanTermViewModel
                {
                    ProductID = row.Field<string>("ProductID"),
                    LoanTerm = row.Field<string>("LoanTerm"),
                }).ToList();

                //return Json(List_MemberwiseProduct.ToList(), JsonRequestBehavior.AllowGet);
                return Json(List_LoanTermMemberandProductwise, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetLoanSummaryIDListByProductandLoanTermWithProcedure(string CenterID, string MemberID, string ProductID, string LoanTerm)
        {
            try
            {
                List<MemberwiseProductAndLoanTermViewModel> LoanSummaryIDLoantermwise = new List<MemberwiseProductAndLoanTermViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = CenterID, MemberID = MemberID, ProductID = ProductID, LoanTerm = LoanTerm };
                var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "MemberwiseProductAndLoanTermLoanSummaryIDforDropDown_LoanSummary");

                LoanSummaryIDLoantermwise = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndLoanTermViewModel
                {

                    LoanSummaryID = row.Field<long>("LoanSummaryID")

                }).ToList();

                return Json(LoanSummaryIDLoantermwise, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetInstallmentDateListByProductandLoanTermWithProcedure(string CenterID, string MemberID, string ProductID, string LoanTerm)
        {
            try
            {
                List<MemberwiseProductAndLoanTermViewModel> LoanSummaryIDLoantermwise = new List<MemberwiseProductAndLoanTermViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = CenterID, MemberID = MemberID, ProductID = ProductID, LoanTerm = LoanTerm };
                var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "Member_Product_LoanTermwise_LoanInstallment_Date");

                LoanSummaryIDLoantermwise = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndLoanTermViewModel
                {
                    InstallmentDate = row.Field<string>("InstallmentDate")

                }).ToList();

                //return Json(List_MemberwiseProduct.ToList(), JsonRequestBehavior.AllowGet);
                return Json(LoanSummaryIDLoantermwise, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetInstallmentByProductandMemberWithProcedure(string CenterID, string MemberID, string ProductID, string Loanterm, string TrxDate)
        {
            try
            {
                List<MemberwiseProductAndLoanTermViewModel> InstallmentAmount = new List<MemberwiseProductAndLoanTermViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = CenterID, MemberID = MemberID, ProductID = ProductID, Loanterm = Loanterm, TrxDate = TrxDate };
                var alldata = groupwiseReportService.GetProductListByMemberWithProcedure(param, "Member_Product_LoanTermwise_Installment_Amount");

                InstallmentAmount = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndLoanTermViewModel
                {
                    LoanSummaryIDPre = row.Field<long>("LoanSummaryIDPre"),
                    LoanPaid = row.Field<decimal>("LoanPaid"),
                    IntPaid = row.Field<decimal>("IntPaid")
                    

                }).ToList();

                //return Json(List_MemberwiseProduct.ToList(), JsonRequestBehavior.AllowGet);
                return Json(InstallmentAmount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //public JsonResult InstallmentAmountSave(string memberCodeTo, string memberNameTo, string productCodeTo, string productNameTo, string memberCode, string memberName, string productCode, string productName, string LoanInstallment, string IntInstallment, string LoanSummaryID, string LoanSummaryIDPre)
        //{
                        
        //    var Summary = loanSummaryService.GetByLoanSummaryId(Convert.ToInt64(LoanSummaryID));
                        
        //    DailyLoanTrx trx = new DailyLoanTrx();
        //    trx.CenterID = Summary.CenterID;
        //    trx.TrxDate = SessionHelper.TransactionDate;
        //    trx.LoanSummaryID = Summary.LoanSummaryID;
        //    trx.OfficeID = Summary.OfficeID;
        //    trx.MemberID = Summary.MemberID;

        //    trx.LoanNo = Summary.LoanNo;
        //    trx.MemberCode = memberCodeTo;
        //    trx.MemberName = memberNameTo;
        //    trx.ProductCode = productCodeTo;
        //    trx.ProductName = productNameTo;
            
        //    trx.ProductID = Summary.ProductID;
        //    trx.MemberCategoryID = Summary.MemberCategoryID;
        //    trx.LoanTerm = Summary.LoanTerm;
        //    trx.PurposeID = Summary.PurposeID;
        //    trx.InstallmentDate = SessionHelper.TransactionDate;
        //    trx.PrincipalLoan = Summary.PrincipalLoan;
        //    trx.LoanRepaid = Summary.LoanRepaid;
        //    trx.LoanDue = Summary.LoanInstallment;
        //    trx.LoanPaid = Convert.ToDecimal(LoanInstallment);
        //    trx.CumIntCharge = Summary.IntCharge;
        //    trx.IntCharge = Summary.IntCharge;
        //    trx.IntDue = Summary.IntInstallment;
        //    trx.IntPaid = Convert.ToDecimal(IntInstallment);
        //    trx.Advance = Summary.Advance;
        //    trx.DueRecovery = Summary.DueRecovery;
        //    trx.TrxType = 41;
        //    trx.InstallmentNo = Convert.ToInt16(Summary.InstallmentNo);
        //    trx.EmployeeID = Summary.EmployeeId;
        //    trx.OrgID = Summary.OrgID;
        //    trx.Duration = Summary.Duration;
        //    trx.CreateUser = Summary.CreateUser;
        //    trx.CreateDate = DateTime.Now;
        //    trx.IsActive = true;
        //    trx.InvestorID = Summary.InvestorID;

        //    var SummaryFrom = loanSummaryService.GetByLoanSummaryId(Convert.ToInt64(LoanSummaryIDPre));

        //    DailyLoanTrx trxFrom = new DailyLoanTrx();
        //    trxFrom.CenterID = SummaryFrom.CenterID;
        //    trxFrom.TrxDate = SessionHelper.TransactionDate;
        //    trxFrom.LoanSummaryID = SummaryFrom.LoanSummaryID;
        //    trxFrom.OfficeID = SummaryFrom.OfficeID;
        //    trxFrom.MemberID = SummaryFrom.MemberID;

        //    trxFrom.LoanNo = SummaryFrom.LoanNo;
        //    trxFrom.MemberCode = memberCode;
        //    trxFrom.MemberName = memberName;
        //    trxFrom.ProductCode = productCode;
        //    trxFrom.ProductName = productName;

        //    trxFrom.ProductID = SummaryFrom.ProductID;
        //    trxFrom.MemberCategoryID = SummaryFrom.MemberCategoryID;
        //    trxFrom.LoanTerm = SummaryFrom.LoanTerm;
        //    trxFrom.PurposeID = SummaryFrom.PurposeID;
        //    trxFrom.InstallmentDate = SessionHelper.TransactionDate;
        //    trxFrom.PrincipalLoan = SummaryFrom.PrincipalLoan;
        //    trxFrom.LoanRepaid = SummaryFrom.LoanRepaid;
        //    trxFrom.LoanDue = SummaryFrom.LoanInstallment;
        //    trxFrom.LoanPaid = -1 * Convert.ToDecimal(LoanInstallment);
        //    trxFrom.CumIntCharge = SummaryFrom.IntCharge;
        //    trxFrom.IntCharge = SummaryFrom.IntCharge;
        //    trxFrom.IntDue = SummaryFrom.IntInstallment;
        //    trxFrom.IntPaid = -1 * Convert.ToDecimal(IntInstallment);
        //    trxFrom.Advance = SummaryFrom.Advance;
        //    trxFrom.DueRecovery = SummaryFrom.DueRecovery;
        //    trxFrom.TrxType = 41;
        //    trxFrom.InstallmentNo = Convert.ToInt16(Summary.InstallmentNo);
        //    trxFrom.EmployeeID = SummaryFrom.EmployeeId;
        //    trxFrom.OrgID = SummaryFrom.OrgID;
        //    trxFrom.Duration = SummaryFrom.Duration;
        //    trxFrom.CreateUser = SummaryFrom.CreateUser;
        //    trxFrom.CreateDate = DateTime.Now;
        //    trxFrom.IsActive = true;
        //    trxFrom.InvestorID = SummaryFrom.InvestorID;

        //    dailyLoanTrxService.Create(trx);
        //    dailyLoanTrxService.Create(trxFrom);
        //    var result = 1;
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //    // return Json(new { result ="1",JsonRequestBehavior.AllowGet});

        //}
        public JsonResult InstallmentAmountSave(string CenterIDTo, string memberCodeTo, string memberNameTo, string productCodeTo, string productNameTo, string memberCode, string memberName, string productCode, string productName, string LoanInstallment, string IntInstallment, string LoanSummaryID, string LoanSummaryIDPre, string ErrorDate, string LoanInstallmentTrans, string IntInstallmentTrans)
        {


            var Summary = loanSummaryService.GetByLoanSummaryId(Convert.ToInt64(LoanSummaryID));

            DailyLoanTrx trx = new DailyLoanTrx();
            trx.CenterID = Summary.CenterID;
            trx.TrxDate = SessionHelper.TransactionDate;
            trx.LoanSummaryID = Summary.LoanSummaryID;
            trx.OfficeID = Summary.OfficeID;
            trx.MemberID = Summary.MemberID;

            trx.LoanNo = Summary.LoanNo;
            trx.MemberCode = memberCodeTo;
            trx.MemberName = memberNameTo;
            trx.ProductCode = productCodeTo;
            trx.ProductName = productNameTo;

            trx.ProductID = Summary.ProductID;
            trx.MemberCategoryID = Summary.MemberCategoryID;
            trx.LoanTerm = Summary.LoanTerm;
            trx.PurposeID = Summary.PurposeID;
            trx.InstallmentDate = SessionHelper.TransactionDate;
            trx.PrincipalLoan = Summary.PrincipalLoan;
            trx.LoanRepaid = Summary.LoanRepaid;
            trx.LoanDue = Summary.LoanInstallment;
            trx.LoanPaid = Convert.ToDecimal(LoanInstallment);
            trx.CumIntCharge = Summary.IntCharge;
            trx.IntCharge = Summary.IntCharge;
            trx.IntDue = Summary.IntInstallment;
            trx.IntPaid = Convert.ToDecimal(IntInstallment);
            trx.Advance = Summary.Advance;
            trx.DueRecovery = Summary.DueRecovery;
            trx.TrxType = 41;
            trx.InstallmentNo = Convert.ToInt16(Summary.InstallmentNo);
            trx.EmployeeID = Summary.EmployeeId;
            trx.OrgID = Summary.OrgID;
            trx.Duration = Summary.Duration;
            trx.CreateUser = Summary.CreateUser;
            trx.CreateDate = DateTime.Now;
            trx.IsActive = true;
            trx.InvestorID = Summary.InvestorID;

            var SummaryFrom = loanSummaryService.GetByLoanSummaryId(Convert.ToInt64(LoanSummaryIDPre));

            DailyLoanTrx trxFrom = new DailyLoanTrx();
            trxFrom.CenterID = SummaryFrom.CenterID;
            trxFrom.TrxDate = SessionHelper.TransactionDate;
            trxFrom.LoanSummaryID = SummaryFrom.LoanSummaryID;
            trxFrom.OfficeID = SummaryFrom.OfficeID;
            trxFrom.MemberID = SummaryFrom.MemberID;

            trxFrom.LoanNo = SummaryFrom.LoanNo;
            trxFrom.MemberCode = memberCode;
            trxFrom.MemberName = memberName;
            trxFrom.ProductCode = productCode;
            trxFrom.ProductName = productName;

            trxFrom.ProductID = SummaryFrom.ProductID;
            trxFrom.MemberCategoryID = SummaryFrom.MemberCategoryID;
            trxFrom.LoanTerm = SummaryFrom.LoanTerm;
            trxFrom.PurposeID = SummaryFrom.PurposeID;
            trxFrom.InstallmentDate = SessionHelper.TransactionDate;
            trxFrom.PrincipalLoan = SummaryFrom.PrincipalLoan;
            trxFrom.LoanRepaid = SummaryFrom.LoanRepaid;
            trxFrom.LoanDue = SummaryFrom.LoanInstallment;
            trxFrom.LoanPaid = -1 * Convert.ToDecimal(LoanInstallment);
            trxFrom.CumIntCharge = SummaryFrom.IntCharge;
            trxFrom.IntCharge = SummaryFrom.IntCharge;
            trxFrom.IntDue = SummaryFrom.IntInstallment;
            trxFrom.IntPaid = -1 * Convert.ToDecimal(IntInstallment);
            trxFrom.Advance = SummaryFrom.Advance;
            trxFrom.DueRecovery = SummaryFrom.DueRecovery;
            trxFrom.TrxType = 41;
            trxFrom.InstallmentNo = Convert.ToInt16(Summary.InstallmentNo);
            trxFrom.EmployeeID = SummaryFrom.EmployeeId;
            trxFrom.OrgID = SummaryFrom.OrgID;
            trxFrom.Duration = SummaryFrom.Duration;
            trxFrom.CreateUser = SummaryFrom.CreateUser;
            trxFrom.CreateDate = DateTime.Now;
            trxFrom.IsActive = true;
            trxFrom.InvestorID = SummaryFrom.InvestorID;
           

            var storeProcedureName = "Set_LoanInstallmentCorrection";
            if (IsDayInitiated)
            {

                var param = new
                {
                    @LoanSummaryID = LoanSummaryIDPre,
                    @OfficeID = SummaryFrom.OfficeID,
                    @CenterID = SummaryFrom.CenterID,
                    @MemberID = SummaryFrom.MemberID,
                    @ProductId = SummaryFrom.ProductID,
                    @LoanTerm = SummaryFrom.LoanTerm,
                    @OLdTrxDate = ErrorDate,
                    @NewLoanSummaryID = LoanSummaryID,
                    @TrxDate = SessionHelper.TransactionDate,
                    @NewOfficeID = Summary.OfficeID,
                    @NewCenterID = Summary.CenterID,
                    @NewMemberID = Summary.MemberID,
                    @NewProductId = Summary.ProductID,
                    @NewLoanTerm = Summary.LoanTerm,
                    @LoanPaid = LoanInstallment,
                    @IntPaid = IntInstallment,
                    @CreateUser = SessionHelper.LoginUserEmployeeID,
                    @CreateDate = DateTime.Now,
                    @transtype = 41,
                    @LoanPaidTrans = Convert.ToDecimal(LoanInstallmentTrans),
                    @IntPaidTrans = Convert.ToDecimal(IntInstallmentTrans)

                };
                using (var gbData = new gBankerDataAccess())
                {
                    int query_result = gbData.ExecuteNonQuery(storeProcedureName, param);
                } 

            }
            else
            {
                var param = new
                {
                    @LoanSummaryID = LoanSummaryIDPre,
                    @OfficeID = SummaryFrom.OfficeID,
                    @CenterID = SummaryFrom.CenterID,
                    @MemberID = SummaryFrom.MemberID,
                    @ProductId = SummaryFrom.ProductID,
                    @LoanTerm = SummaryFrom.LoanTerm,
                    @OLdTrxDate = ErrorDate,
                    @NewLoanSummaryID = LoanSummaryID,
                    @TrxDate = ErrorDate,
                    @NewOfficeID = Summary.OfficeID,
                    @NewCenterID = Summary.CenterID,
                    @NewMemberID = Summary.MemberID,
                    @NewProductId = Summary.ProductID,
                    @NewLoanTerm = Summary.LoanTerm,
                    @LoanPaid = LoanInstallment,
                    @IntPaid = IntInstallment,
                    @CreateUser = SessionHelper.LoginUserEmployeeID,
                    @CreateDate = DateTime.Now,
                    @transtype = 41,
                    @LoanPaidTrans = Convert.ToDecimal(LoanInstallmentTrans),
                    @IntPaidTrans = Convert.ToDecimal(IntInstallmentTrans)

                };
                using (var gbData = new gBankerDataAccess())
                {
                    int query_result = gbData.ExecuteNonQuery(storeProcedureName, param);
                } 

            }
              
            //dailyLoanTrxService.Create(trx);
           //dailyLoanTrxService.Create(trxFrom);
            var result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
            // return Json(new { result ="1",JsonRequestBehavior.AllowGet});

        }
        #endregion

        #region Events
        public ActionResult LoanInstallmentCorrection()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            ViewData["ProductListByMember"] = items;
            ViewData["LoanTermList"] = items;

            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }
        public ActionResult LedgerPost(LoanSummaryViewModel model)
        {
            var members = "Success";
            var param = new { OfficeId = LoginUserOfficeID, OrgID = SessionHelper.LoginUserOrganizationID, CreateUser=LoggedInEmployeeID };
            weeklyReportService.LoanInstallmentCorrectionLedgerPost(param);
            return Json(members, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details(int id)
        {
            return View();
        }
        //public ActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        public ActionResult Edit(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}
        //[HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {

                dailyLoanTrxService.Delete(id);
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
