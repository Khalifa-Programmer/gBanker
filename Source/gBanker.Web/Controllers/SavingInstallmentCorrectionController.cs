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
    public class SavingInstallmentCorrectionController : BaseController
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
        public SavingInstallmentCorrectionController(IEmployeeService employeeService, IOfficeService officeService, IProductService productService, ICenterService centerService, IMemberService memberService, IGroupwiseReportService groupwiseReportService, IUltimateReportService unlimitedReportService, IPOMISReportService pomisReportService, IWeeklyReportService weeklyReportService,  ILoanTrxService loanTrxService, ILoanSummaryService loanSummaryService, IDailyLoanTrxService dailyLoanTrxService, ISavingTrxService savingTrxService, ISavingSummaryService savingSummaryService, IDailySavingTrxService dailySavingTrxService, ISpecialLoanCollectionService specialLoanCollectionService)
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

        #region Methods

        public ActionResult GetMemberListAuto(string memberid, string centerId)
        {
            var MemberByCenterSessionKey = string.Format("MemberByCenterSessionKey_{0}", centerId);
            var memberList = new List<Member>();
            if (Session[MemberByCenterSessionKey] != null)
                memberList = Session[MemberByCenterSessionKey] as List<Member>;
            else
            {
                // var mbr = memberService.GetByCenterId(int.Parse(centerId), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID)).ToList();

                List<Member> List_Members = new List<Member>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId };
                var alldata = groupwiseReportService.GetDataLedgerReport(param, "GetMemberListSavingAutoComplete_WriteOff");
                //var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetMemberListSavingAutoComplete_WriteOff");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new Member
                {
                    MemberID = row.Field<Int64>("MemberID"),
                    //LoanTerm = row.Field<string>("LoanTerm"),
                    FirstName = row.Field<string>("MemberName"),
                    MemberCode = row.Field<string>("MemberCode"),
                }).ToList();

                Session[MemberByCenterSessionKey] = List_Members; // mbr;
                memberList = List_Members; // mbr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }
         public ActionResult GetSavingSummaryMemberListAuto(string memberid, string centerId)
        {
            var MemberByCenterSessionKey = string.Format("MemberByCenterSessionKey_{0}", centerId);
            var memberList = new List<Member>();
            if (Session[MemberByCenterSessionKey] != null)
                memberList = Session[MemberByCenterSessionKey] as List<Member>;
            else
            {
                // var mbr = memberService.GetByCenterId(int.Parse(centerId), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID)).ToList();

                List<Member> List_Members = new List<Member>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId };
                var alldata = groupwiseReportService.GetDataLedgerReport(param, "GetMemberListSavingSummaryAutoComplete_WriteOff");
                //var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetMemberListSavingAutoComplete_WriteOff");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new Member
                {
                    MemberID = row.Field<Int64>("MemberID"),
                    //LoanTerm = row.Field<string>("LoanTerm"),
                    FirstName = row.Field<string>("MemberName"),
                    MemberCode = row.Field<string>("MemberCode"),
                }).ToList();

                Session[MemberByCenterSessionKey] = List_Members; // mbr;
                memberList = List_Members; // mbr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSavingCorrectionFromMemberListAuto(string memberid, string centerId)
        {
            var MemberByCenterSessionKey = string.Format("MemberByCenterSessionKey_{0}", centerId);
            var memberList = new List<Member>();
            if (Session[MemberByCenterSessionKey] != null)
                memberList = Session[MemberByCenterSessionKey] as List<Member>;
            else
            {
                // var mbr = memberService.GetByCenterId(int.Parse(centerId), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID)).ToList();

                List<Member> List_Members = new List<Member>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId };
                var alldata = groupwiseReportService.GetDataLedgerReport(param, "GetMemberListSavingLedgerAutoComplete");
                //var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetMemberListSavingAutoComplete_WriteOff");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new Member
                {
                    MemberID = row.Field<Int64>("MemberID"),
                    //LoanTerm = row.Field<string>("LoanTerm"),
                    FirstName = row.Field<string>("MemberName"),
                    MemberCode = row.Field<string>("MemberCode"),
                }).ToList();

                Session[MemberByCenterSessionKey] = List_Members; // mbr;
                memberList = List_Members; // mbr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetActiveMemberListAuto(string memberid, string centerId)
        {
            var MemberByCenterSessionKey_Active = string.Format("MemberByCenterSessionKey_Active_{0}", centerId);
            var memberList = new List<Member>();
            if (Session[MemberByCenterSessionKey_Active] != null)
                memberList = Session[MemberByCenterSessionKey_Active] as List<Member>;
            else
            {
                List<Member> List_Members = new List<Member>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetMemberListActiveAutoComplete");
                //var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetMemberListSavingAutoComplete_WriteOff");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new Member
                {
                    MemberID = row.Field<Int64>("MemberID"),
                    //LoanTerm = row.Field<string>("LoanTerm"),
                    FirstName = row.Field<string>("MemberName"),
                    MemberCode = row.Field<string>("MemberCode"),
                }).ToList();

                Session[MemberByCenterSessionKey_Active] = List_Members; // mbr;
                memberList = List_Members; // mbr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMemberById(long memberid, string centerId) // KHALID
        {
            var MemberByCenterSessionKey = string.Format("MemberByCenterSessionKey_MemberId_{0}", centerId);
            var memberList = new List<Member>();

            List<Member> List_Members = new List<Member>();

            var param = new { MemberId = memberid, CenterID = centerId };
            var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetMemberByIdCenter");

            List_Members = alldata.Tables[0].AsEnumerable()
            .Select(row => new Member
            {
                MemberID = row.Field<Int64>("MemberID"),
                    //LoanTerm = row.Field<string>("LoanTerm"),
                    FirstName = row.Field<string>("MemberName"),
                MemberCode = row.Field<string>("MemberCode"),
            }).ToList();

            memberList = List_Members; // mbr;

            var members = memberList.Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSavingCorrectionMemberListAuto(string memberid, string centerId)
        {
            var MemberByCenterSessionKey_TO = string.Format("MemberByCenterSessionKey_TO_{0}", centerId);
            var memberList = new List<Member>();
            if (Session[MemberByCenterSessionKey_TO] != null)
                memberList = Session[MemberByCenterSessionKey_TO] as List<Member>;
            else
            {
                // var mbr = memberService.GetByCenterId(int.Parse(centerId), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID)).ToList();

                List<Member> List_Members = new List<Member>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId };
                var alldata = groupwiseReportService.GetDataLedgerReport(param, "GetMemberListSavingLedgerAutoComplete");
                //var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetMemberListSavingAutoComplete_WriteOff");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new Member
                {
                    MemberID = row.Field<Int64>("MemberID"),
                    //LoanTerm = row.Field<string>("LoanTerm"),
                    FirstName = row.Field<string>("MemberName"),
                    MemberCode = row.Field<string>("MemberCode"),
                }).ToList();

                Session[MemberByCenterSessionKey_TO] = List_Members; // mbr;
                memberList = List_Members; // mbr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSavingLedgerMemberListAuto(string memberid, string centerId)
        {
            var MemberByCenterSessionKey = string.Format("MemberByCenterSessionKey_{0}", centerId);
            var memberList = new List<Member>();
            if (Session[MemberByCenterSessionKey] != null)
                memberList = Session[MemberByCenterSessionKey] as List<Member>;
            else
            {
                List<Member> List_Members = new List<Member>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId };
                var alldata = groupwiseReportService.GetDataLedgerReport(param, "GetMemberListSavingLedgerAutoComplete");
                //var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetMemberListSavingAutoComplete_WriteOff");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new Member
                {
                    MemberID = row.Field<Int64>("MemberID"),
                    //LoanTerm = row.Field<string>("LoanTerm"),
                    FirstName = row.Field<string>("MemberName"),
                    MemberCode = row.Field<string>("MemberCode"),
                }).ToList();

                Session[MemberByCenterSessionKey] = List_Members; // mbr;
                memberList = List_Members; // mbr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDailySavingTrxData(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                long totalCount;
                var allDailySavingTrxes = dailySavingTrxService.GetDailySavingTrxDetailPaged(filterColumn, filterValue, jtStartIndex, jtPageSize, out totalCount, LoggedInOrganizationID,LoginUserOfficeID);
                var currentPageRecords = Mapper.Map<IEnumerable<DailySavingTrx>, IEnumerable<DailySavingTrxViewModel>>(allDailySavingTrxes);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });
                //return Json(new { Result = "OK", Records = allDailySavingTrxes, TotalRecordCount = totalCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        public JsonResult GetProductListByMemberSavingInstallment(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<MemberwiseProductAndAccountNoViewModel> List_MemberwiseProduct = new List<MemberwiseProductAndAccountNoViewModel>();

                var param = new { Qtype = 1, MemberID = MemberID, ProductID = 0 };
                var alldata = groupwiseReportService.GetDataLedgerReport(param, "MemberwiseProductAndAccountNoforDropDown__SavingInstallment");
                //var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "MemberwiseProductAndAccountNoforDropDown");

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
        public JsonResult GetProductListByMember(long memberId)
        {
            var getProduct = loanSummaryService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.OrgID == LoggedInOrganizationID && s.MemberID == memberId);
            var viewProduct = getProduct.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = productService.GetById(x.ProductID).ProductCode + ", " + productService.GetById(x.ProductID).ProductName
            });
            var prod_items = new List<SelectListItem>();
            if (viewProduct.ToList().Count > 0)
            {
                prod_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            }
            prod_items.AddRange(viewProduct);
            return Json(prod_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLoanTermList(long memberId, int productId)
        {
            var getLoanTerm = loanSummaryService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.OrgID == LoggedInOrganizationID && s.MemberID == memberId && s.ProductID == productId).OrderBy(e => e.LoanTerm);
            var viewLoanTerm = getLoanTerm.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.LoanTerm.ToString(),
                //Text = x.MemberCode.ToString() + ", " + x.MiddleName.ToString()
                //Text = x.MemberCode.ToString() + ", " + x.FirstName.ToString() + " " + x.MiddleName.ToString() + " " + x.LastName.ToString()
                Text = x.LoanTerm.ToString()
                //Text = x.CenterName.ToString()
            });
            var loanterm_items = new List<SelectListItem>();
            if (viewLoanTerm.ToList().Count > 0)
            {
                loanterm_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            }
            loanterm_items.AddRange(viewLoanTerm);
            return Json(loanterm_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNoOfAccountList(long memberId, int productId)
        {
            var getLoanTerm = loanSummaryService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.OrgID == LoggedInOrganizationID && s.MemberID == memberId && s.ProductID == productId).OrderBy(e => e.LoanTerm);
            var viewLoanTerm = getLoanTerm.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.LoanTerm.ToString(),
                //Text = x.MemberCode.ToString() + ", " + x.MiddleName.ToString()
                //Text = x.MemberCode.ToString() + ", " + x.FirstName.ToString() + " " + x.MiddleName.ToString() + " " + x.LastName.ToString()
                Text = x.LoanTerm.ToString()
                //Text = x.CenterName.ToString()
            });
            var loanterm_items = new List<SelectListItem>();
            if (viewLoanTerm.ToList().Count > 0)
            {
                loanterm_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            }
            loanterm_items.AddRange(viewLoanTerm);
            return Json(loanterm_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEmployeeList()
        {
            var getEmployee = employeeService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.OrgID == LoggedInOrganizationID).OrderBy(e => e.EmployeeCode);
            var viewEmployee = getEmployee.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.EmployeeID.ToString(),
                Text = x.EmpName.ToString()
            });
            var emp_items = new List<SelectListItem>();
            if (viewEmployee.ToList().Count > 0)
            {
                emp_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            }
            emp_items.AddRange(viewEmployee);
            return Json(emp_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductList()
        {
            var getProduct = productService.GetAll().Where(s => s.IsActive == true && s.OrgID == LoggedInOrganizationID).OrderBy(e => e.ProductCode);
            var viewProduct = getProduct.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = x.ProductName.ToString()
            });
            var prod_items = new List<SelectListItem>();
            if (viewProduct.ToList().Count > 0)
            {
                prod_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            }
            prod_items.AddRange(viewProduct);
            return Json(prod_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMainProductList()
        {
            try
            {
                List<ProductViewModel> List_ProductViewModel = new List<ProductViewModel>();
                var productList = weeklyReportService.GetMainProductList();

                List_ProductViewModel = productList.Tables[0].AsEnumerable()
                .Select(row => new ProductViewModel
                {
                    MainProductCode = row.Field<string>("MainProductCode"),
                    MainItemName = row.Field<string>("MainItemName")

                }).ToList();

                return Json(List_ProductViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
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
        public JsonResult GetOfficeList()
        {
            var getOffice = officeService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.OrgID == LoggedInOrganizationID && s.OfficeLevel == 4).OrderBy(e => e.OfficeCode);
            var viewOffice = getOffice.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMemberList(int centerId)
        {
            try
            {
                List<GetMemberListViewModel> List_Members = new List<GetMemberListViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId };
                var alldata = groupwiseReportService.GetDataLedgerReport(param, "GetMemberList_Dropdown_SavingSummary");

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
        public JsonResult GetMemberListByMemberCode(int centerId)
        {
            var getMember = memberService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.MemberStatus == "1" && s.OrgID == LoggedInOrganizationID && s.CenterID == centerId).OrderBy(e => e.MemberCode);
            var viewMember = getMember.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.MemberID.ToString(),
                //Text = x.MemberCode.ToString() + ", " + x.FirstName.ToString() + " " + (string.IsNullOrEmpty(x.MiddleName) ? " " : x.MiddleName.ToString()) + " " + (string.IsNullOrEmpty(x.LastName) ? " " : x.LastName.ToString())
                Text = x.MemberCode.ToString() + ", " + (string.IsNullOrEmpty(x.FirstName) ? " " : x.FirstName.ToString()) + " " + (string.IsNullOrEmpty(x.MiddleName) ? " " : x.MiddleName.ToString()) + " " + (string.IsNullOrEmpty(x.LastName) ? " " : x.LastName.ToString())
                //Text = x.MemberCode.ToString() + ", " + x.MiddleName.ToString()
                //Text = x.CenterName.ToString()
            });
            var member_items = new List<SelectListItem>();
            if (viewMember.ToList().Count > 0)
            {
                member_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            }
            member_items.AddRange(viewMember);
            return Json(member_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductListByMemberWithProcedure(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<MemberwiseProductAndLoanTermViewModel> List_MemberwiseProduct = new List<MemberwiseProductAndLoanTermViewModel>();

                var param = new { Qtype = 1, MemberID = MemberID, ProductID = 0 };
                var alldata = groupwiseReportService.GetDataLedgerReport(param, "MemberwiseProductAndLoanTermforDropDown");

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
                var alldata = groupwiseReportService.GetDataLedgerReport(param, "MemberwiseProductAndLoanTermforDropDown_LoanSummary");

                List_LoanTermMemberandProductwise = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndLoanTermViewModel
                {
                    ProductID = row.Field<string>("ProductID"),
                    LoanTerm = row.Field<string>("LoanTerm"),
                    //ProductName = row.Field<string>("ProductName")

                }).ToList();

                //return Json(List_MemberwiseProduct.ToList(), JsonRequestBehavior.AllowGet);
                return Json(List_LoanTermMemberandProductwise, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetSavingSummaryIDListByProductandLoanTermWithProcedure(string CenterID, string MemberID, string ProductID, string NoOfAccount)
        {
            try
            {
                List<MemberwiseProductAndAccountNoViewModel> SavingSummaryIDNoOfAccountmwise = new List<MemberwiseProductAndAccountNoViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = CenterID, MemberID = MemberID, ProductID = ProductID, NoOfAccount = NoOfAccount };
                var alldata = groupwiseReportService.GetDataLedgerReport(param, "MemberwiseProductAndNoOfAccountSavingSummaryIDforDropDown_SavingSummary");

                SavingSummaryIDNoOfAccountmwise = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndAccountNoViewModel
                {
                    //ProductID = row.Field<string>("ProductID"),
                    SavingSummaryID = row.Field<long>("SavingSummaryID")
                    //ProductName = row.Field<string>("ProductName")

                }).ToList();

                //return Json(List_MemberwiseProduct.ToList(), JsonRequestBehavior.AllowGet);
                return Json(SavingSummaryIDNoOfAccountmwise, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetSavingInstallmentByProductandMemberWithProcedure(string CenterID, string MemberID, string ProductID, string NoOfAccount, string TrxDate)
        {
            try
            {
                List<MemberwiseProductAndAccountNoViewModel> SavingInstallmentAmount = new List<MemberwiseProductAndAccountNoViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = CenterID, MemberID = MemberID, ProductID = ProductID, NoOfAccount = NoOfAccount, TrxDate = TrxDate };
                var alldata = groupwiseReportService.GetDataLedgerReport(param, "Member_Product_NoOfAccountwise_SavingInstallment_Amount");

                SavingInstallmentAmount = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndAccountNoViewModel
                {
                    SavingSummaryIDPre = row.Field<long>("SavingSummaryIDPre"),
                    Deposit = row.Field<decimal>("Deposit"),
                    Withdrawal = row.Field<decimal>("Withdrawal")


                }).ToList();

                //return Json(List_MemberwiseProduct.ToList(), JsonRequestBehavior.AllowGet);
                return Json(SavingInstallmentAmount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetProductListByMemberFromSavingTrx(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<MemberwiseProductAndAccountNoViewModel> List_MemberwiseProduct = new List<MemberwiseProductAndAccountNoViewModel>();

                var param = new { Qtype = 1, MemberID = MemberID, ProductID = 0 };
                var alldata = groupwiseReportService.GetDataLedgerReport(param, "MemberwiseProductAndAccountNoforDropDown__SavingSummary");
                //var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "MemberwiseProductAndAccountNoforDropDown");
                
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
        public JsonResult GetProductListByMemberFromSavingTrxTO(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<MemberwiseProductAndAccountNoViewModel> List_MemberwiseProduct = new List<MemberwiseProductAndAccountNoViewModel>();

                var param = new { Qtype = 1, MemberID = MemberID, ProductID = 0 };
                var alldata = groupwiseReportService.GetDataLedgerReport(param, "MemberwiseProductAndAccountNoforDropDown__SavingSummary");
                //var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "MemberwiseProductAndAccountNoforDropDown");

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
        public JsonResult GetProductListByMemberFromSavingTrxByMemberCode(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<MemberwiseProductAndAccountNoViewModel> List_MemberwiseProduct = new List<MemberwiseProductAndAccountNoViewModel>();

                var param = new { Qtype = 1, MemberID = MemberID, ProductID = 0 };
                var alldata = groupwiseReportService.GetDataLedgerReport(param, "MemberwiseProductAndAccountNoforDropDown");

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
        public JsonResult GetNoOfAccountListByProductandMemberFromSavingTrx(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<MemberwiseProductAndAccountNoViewModel> List_AccountNoMemberandProductwise = new List<MemberwiseProductAndAccountNoViewModel>();

                var param = new { Qtype = 2, MemberID = MemberID, ProductID = ProductID };
                var alldata = groupwiseReportService.GetDataLedgerReport(param, "MemberwiseProductAndAccountNoforDropDown__SavingSummary");
                //var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "MemberwiseProductAndAccountNoforDropDown");

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
        //public JsonResult SavingInstallmentAmountSave(string center, string member, string product, string NoOfAccount, string Deposit, string Withdrawal, string SavingSummaryID, string SavingSummaryIDPre)
        
        //{

        //    //var Summary = loanSummaryService.GetAll().Where(x => x.CenterID == Convert.ToInt32(center) && x.MemberID == Convert.ToInt64(member) && x.ProductID == Convert.ToInt32(product) && x.LoanTerm == Convert.ToInt32(loanterm) && x.OfficeID == SessionHelper.LoginUserOfficeID).FirstOrDefault();
        //    var Summary = savingSummaryService.GetBySavingSummaryId(Convert.ToInt64(SavingSummaryID));

        //    DailySavingTrx trx = new DailySavingTrx();
            
        //    trx.SavingSummaryID = Summary.SavingSummaryID;
        //    trx.OfficeID = Summary.OfficeID;
        //    trx.MemberID = Summary.MemberID;
        //    trx.ProductID = Summary.ProductID;
        //    trx.CenterID = Summary.CenterID;
        //    trx.NoOfAccount = Summary.NoOfAccount;
        //    trx.TransactionDate = SessionHelper.TransactionDate;
        //    trx.DueSavingInstallment = 0;
        //    trx.SavingInstallment = 0;
        //    trx.Deposit = Convert.ToDecimal(Deposit);
        //    trx.Withdrawal = Convert.ToDecimal(Withdrawal);
        //    trx.Balance = 0;
        //    trx.Penalty = 0;
        //    trx.TransType = 41;
        //    trx.MonthlyInterest = 0;
        //    trx.PresenceInd = true;
        //    trx.TransferDeposit = 0;
        //    trx.TransferWithdrawal = 0;
        //    trx.EmployeeID = Summary.EmployeeId;
        //    trx.MemberCategoryID = Summary.MemberCategoryID;
        //    trx.OrgID = Summary.OrgID;
        //    trx.IsActive = 1;
        //    trx.CreateUser = Summary.CreateUser;
        //    trx.CreateDate = DateTime.Now;
            
        //    var SummaryFrom = savingSummaryService.GetBySavingSummaryId(Convert.ToInt64(SavingSummaryIDPre));

        //    DailySavingTrx trxFrom = new DailySavingTrx();

        //    trxFrom.SavingSummaryID = SummaryFrom.SavingSummaryID;
        //    trxFrom.OfficeID = SummaryFrom.OfficeID;
        //    trxFrom.MemberID = SummaryFrom.MemberID;
        //    trxFrom.ProductID = SummaryFrom.ProductID;
        //    trxFrom.CenterID = SummaryFrom.CenterID;
        //    trxFrom.NoOfAccount = SummaryFrom.NoOfAccount;
        //    trxFrom.TransactionDate = SessionHelper.TransactionDate;
        //    trxFrom.DueSavingInstallment = 0;
        //    trxFrom.SavingInstallment = 0;
        //    trxFrom.Deposit = -1*Convert.ToDecimal(Deposit);
        //    trxFrom.Withdrawal = -1*Convert.ToDecimal(Withdrawal);
        //    trxFrom.Balance = 0;
        //    trxFrom.Penalty = 0;
        //    trxFrom.TransType = 41;
        //    trxFrom.MonthlyInterest = 0;
        //    trxFrom.PresenceInd = true;
        //    trxFrom.TransferDeposit = 0;
        //    trxFrom.TransferWithdrawal = 0;
        //    trxFrom.EmployeeID = SummaryFrom.EmployeeId;
        //    trxFrom.MemberCategoryID = SummaryFrom.MemberCategoryID;
        //    trxFrom.OrgID = SummaryFrom.OrgID;
        //    trxFrom.IsActive = 1;
        //    trxFrom.CreateUser = SummaryFrom.CreateUser;
        //    trxFrom.CreateDate = DateTime.Now;

        //    dailySavingTrxService.Create(trx);
        //    dailySavingTrxService.Create(trxFrom);
        //    var result = 1;
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //    // return Json(new { result ="1",JsonRequestBehavior.AllowGet});

        //}
        public JsonResult SavingInstallmentAmountSave(string CenterIDTo = "0", string memberCodeTo = "0", string memberNameTo = "0", string productCodeTo = "0", string productNameTo = "0", string memberCode = "0", string memberName = "0", string productCode = "0", string productName = "0", string NoOfAccount = "0", string Deposit = "0", string Withdrawal = "0", string SavingSummaryID = "0", string SavingSummaryIDPre = "0", string DepositTrans = "0", string WithdrawalTrans = "0")
        {
            var param1 = new { @OfficeID = SessionHelper.LoginUserOfficeID};
            var LoanInstallMent = groupwiseReportService.GetDataLedgerReport(param1, "getMaxDAyEndDate");
            var Summary = savingSummaryService.GetBySavingSummaryId(Convert.ToInt64(SavingSummaryID));
            
            DailySavingTrx trx = new DailySavingTrx();

            DailySavingTrx trxFrom = new DailySavingTrx();

            if (IsDayInitiated)
            {
                trx.TransactionDate = TransactionDate;
                trxFrom.TransactionDate = TransactionDate;

            }
               
            else
            {

                if (LoanInstallMent != null)
                {
                    trx.TransactionDate = Convert.ToDateTime( LoanInstallMent.Tables[0].Rows[0]["BusinessDate"].ToString());
                    trxFrom.TransactionDate = Convert.ToDateTime(LoanInstallMent.Tables[0].Rows[0]["BusinessDate"].ToString());
                }
            }
              

            trx.SavingSummaryID = Summary.SavingSummaryID;
            trx.OfficeID = Summary.OfficeID;
            trx.MemberID = Summary.MemberID;
            trx.ProductID = Summary.ProductID;
            trx.CenterID = Summary.CenterID;

            trx.MemberCode = memberCodeTo;
            trx.MemberName = memberNameTo;
            trx.ProductCode = productCodeTo;
            trx.ProductName = productNameTo;


            trx.NoOfAccount = Summary.NoOfAccount;
            //trx.TransactionDate = SessionHelper.TransactionDate;
            trx.DueSavingInstallment = 0;
            trx.SavingInstallment = Convert.ToDecimal(DepositTrans);
            trx.Deposit = 0;
            trx.Withdrawal = Convert.ToDecimal(WithdrawalTrans);
            trx.Balance = 0;
            trx.Penalty = 0;
            trx.TransType = 41;
            trx.MonthlyInterest = 0;
            trx.PresenceInd = true;
            trx.TransferDeposit = 0;
            trx.TransferWithdrawal = 0;
            trx.EmployeeID = Summary.EmployeeId;
            trx.MemberCategoryID = Summary.MemberCategoryID;
            trx.OrgID = Summary.OrgID;
            trx.IsActive = 1;
            trx.CreateUser = Convert.ToString(LoggedInEmployeeID);
            trx.CreateDate = DateTime.Now;
            
            var SummaryFrom = savingSummaryService.GetBySavingSummaryId(Convert.ToInt64(SavingSummaryIDPre));

            

            trxFrom.SavingSummaryID = SummaryFrom.SavingSummaryID;
            trxFrom.OfficeID = SummaryFrom.OfficeID;
            trxFrom.MemberID = SummaryFrom.MemberID;
            trxFrom.ProductID = SummaryFrom.ProductID;
            trxFrom.CenterID = SummaryFrom.CenterID;

            trxFrom.MemberCode = memberCode;
            trxFrom.MemberName = memberName;
            trxFrom.ProductCode = productCode;
            trxFrom.ProductName = productName;

            
            trxFrom.NoOfAccount = SummaryFrom.NoOfAccount;
            //trxFrom.TransactionDate = SessionHelper.TransactionDate;
            trxFrom.DueSavingInstallment = 0;
            trxFrom.SavingInstallment = -1 * Convert.ToDecimal(DepositTrans);
            trxFrom.Deposit = 0;
            trxFrom.Withdrawal = -1*Convert.ToDecimal(WithdrawalTrans);
            trxFrom.Balance = 0;
            trxFrom.Penalty = 0;
            trxFrom.TransType = 41;
            trxFrom.MonthlyInterest = 0;
            trxFrom.PresenceInd = true;
            trxFrom.TransferDeposit = 0;
            trxFrom.TransferWithdrawal = 0;
            trxFrom.EmployeeID = SummaryFrom.EmployeeId;
            trxFrom.MemberCategoryID = SummaryFrom.MemberCategoryID;
            trxFrom.OrgID = SummaryFrom.OrgID;
            trxFrom.IsActive = 1;
            trxFrom.CreateUser = Convert.ToString(LoggedInEmployeeID);
            trxFrom.CreateDate = DateTime.Now;

            //dailySavingTrxService.Create(trx);
            //dailySavingTrxService.Create(trxFrom);


            var storeProcedureName = "Proc_Set_SavingInstallmentCorrection";
            if (IsDayInitiated)
            {

                var param = new
                {
                    @OfficeID = LoginUserOfficeID,
                    @MemberIDTo = Summary.MemberID,
                    @MemberCodeTo = memberCodeTo,
                    @MemberNameTo = memberNameTo,
                    @ProductIDTO = Summary.ProductID,
                    @ProductCodeTo = productCodeTo,
                    @ProductNameTo = productNameTo,
                    @MemberID = SummaryFrom.MemberID,
                    @MemberCode = memberCode,
                    @MemberName = memberName,
                    @ProductID = SummaryFrom.ProductID,
                    @ProductCode = productCode,
                    @ProductName = productName,
                    @NoOfAccount = NoOfAccount,
                    @Deposit = -1 * Convert.ToDecimal(DepositTrans),
                    @Withdrawal = -1 * Convert.ToDecimal(WithdrawalTrans),
                    @SavingSummaryID = Summary.SavingSummaryID,
                    @SavingSummaryIDPre = SummaryFrom.SavingSummaryID,
                    @DepositTrans = Convert.ToDecimal(DepositTrans),
                    @WithdrawalTrans = Convert.ToDecimal(WithdrawalTrans),
                    @TrxDate = TransactionDate,
                    @CreateUser = LoggedInEmployeeID,
                    @CenterID = Summary.CenterID,
                    @EmployeeID = Summary.EmployeeId,
                    @MemberCategoryID = Summary.MemberCategoryID,
                    @OrgID = LoggedInOrganizationID,
                    @OldTrxDate = TransactionDate

                };
                using (var gbData = new gBankerDataAccess())
                {
                    int query_result = gbData.ExecuteNonQuery(storeProcedureName, param);
                }

            }
            else
            {
                if (LoanInstallMent != null)
                {
                    trx.TransactionDate = Convert.ToDateTime(LoanInstallMent.Tables[0].Rows[0]["BusinessDate"].ToString());
                    trxFrom.TransactionDate = Convert.ToDateTime(LoanInstallMent.Tables[0].Rows[0]["BusinessDate"].ToString());
                }
                var param = new
                {
                    @OfficeID = LoginUserOfficeID,
                    @MemberIDTo = Summary.MemberID,
                    @MemberCodeTo = memberCodeTo,
                    @MemberNameTo = memberNameTo,
                    @ProductIDTO = Summary.ProductID,
                    @ProductCodeTo = productCodeTo,
                    @ProductNameTo = productNameTo,
                    @MemberID = SummaryFrom.MemberID,
                    @MemberCode = memberCode,
                    @MemberName = memberName,
                    @ProductID = SummaryFrom.ProductID,
                    @ProductCode = productCode,
                    @ProductName = productName,
                    @NoOfAccount = NoOfAccount,
                    @Deposit = -1 * Convert.ToDecimal(DepositTrans),
                    @Withdrawal = -1 * Convert.ToDecimal(WithdrawalTrans),
                    @SavingSummaryID = Summary.SavingSummaryID,
                    @SavingSummaryIDPre = SummaryFrom.SavingSummaryID,
                    @DepositTrans = Convert.ToDecimal(DepositTrans),
                    @WithdrawalTrans = Convert.ToDecimal(WithdrawalTrans),
                    @TrxDate = Convert.ToDateTime(LoanInstallMent.Tables[0].Rows[0]["BusinessDate"].ToString()),
                    @CreateUser = LoggedInEmployeeID,
                    @CenterID = Summary.CenterID,
                    @EmployeeID = Summary.EmployeeId,
                    @MemberCategoryID = Summary.MemberCategoryID,
                    @OrgID = LoggedInOrganizationID,
                    @OldTrxDate = Convert.ToDateTime(LoanInstallMent.Tables[0].Rows[0]["BusinessDate"].ToString())

                };
                using (var gbData = new gBankerDataAccess())
                {
                    int query_result = gbData.ExecuteNonQuery(storeProcedureName, param);
                }
            }

            var result = 1;
            return Json(result, JsonRequestBehavior.AllowGet);
            // return Json(new { result ="1",JsonRequestBehavior.AllowGet});

        }
        public ActionResult LedgerPost(LoanSummaryViewModel model)
        {
            var members = "Success";
            var param = new { OfficeId = LoginUserOfficeID, OrgID = SessionHelper.LoginUserOrganizationID, CreateUser=LoggedInEmployeeID };
            weeklyReportService.SavingInstallmentCorrectionLedgerPost(param);

            return Json(members, JsonRequestBehavior.AllowGet);
        }
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }

        #endregion

        #region Events

        public ActionResult SavingInstallmentCorrection()
        {
            //specialLoanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);
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

        //
        // GET: /SavingInstallmentCorrection/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /SavingInstallmentCorrection/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /SavingInstallmentCorrection/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SavingInstallmentCorrection/Create
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

        //
        // GET: /SavingInstallmentCorrection/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /SavingInstallmentCorrection/Edit/5
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

        //
        // GET: /SavingInstallmentCorrection/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        ////
        //// POST: /SavingInstallmentCorrection/Delete/5
        //[HttpPost]

        public ActionResult Delete(int id)
              
        {
            try
            {
                //specialLoanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);
                //var lan = dailyLoanTrxService.GetById(id);
                dailySavingTrxService.Delete(id);

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
