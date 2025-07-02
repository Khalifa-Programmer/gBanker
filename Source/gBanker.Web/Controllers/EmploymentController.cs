using AutoMapper;
using gBanker.Data.DBDetailModels;
using gBanker.Service;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using gBanker.Web.Helpers;
using gBanker.Service.ReportServies;
using System.Data;
using gBanker.Service.StoredProcedure;
using System.Text;

namespace gBanker.Web.Controllers
{
    public class EmploymentController : BaseController
    {


        #region Variables
        private readonly IExpireInfoService expireInfoService;
        private readonly IBranchService branchService;
        private readonly IOfficeService officeService;
        private readonly ILoanSummaryService loansSummaryService;
        private readonly IProductService productService;
        private readonly IMemberCategoryService membercategoryService;
        private readonly ICenterService centerService;
        private readonly IPurposeService purposeService;
        private readonly IMemberService memberService;
        private readonly ILoanApprovalService loanapprovalService;
        private readonly IInvestorService investorService;
        private readonly IMemberPassBookRegisterService memberPassBookRegisterService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IWeeklyReportService weeklyReportService;
        private readonly IEmployeeSPService employeeSPService;
        private readonly IOrganizationService organizationService;
        private readonly IApproveCellingService ApproveCellingService;
        private readonly IGroupwiseReportService groupwiseReportService;

        // GET: LoanApproval
        public EmploymentController(ILoanSummaryService loansSummaryService, IUltimateReportService ultimateReportService, ILoanApprovalService loanapprovalService, IProductService productService, IMemberCategoryService membercategoryService, IOfficeService officeService, ICenterService centerService, IPurposeService purposeService, IMemberService memberService, IInvestorService investorService, IMemberPassBookRegisterService memberPassBookRegisterService, IWeeklyReportService weeklyReportService, IExpireInfoService expireInfoService, IOrganizationService organizationService, IEmployeeSPService employeeSPService, IApproveCellingService ApproveCellingService, IGroupwiseReportService groupwiseReportService)
        {
            this.loansSummaryService = loansSummaryService;
            this.productService = productService;
            this.membercategoryService = membercategoryService;
            this.officeService = officeService;
            this.centerService = centerService;
            this.purposeService = purposeService;
            this.memberService = memberService;
            this.loanapprovalService = loanapprovalService;
            this.investorService = investorService;
            this.memberPassBookRegisterService = memberPassBookRegisterService;
            this.ultimateReportService = ultimateReportService;
            this.weeklyReportService = weeklyReportService;
            this.expireInfoService = expireInfoService;
            this.organizationService = organizationService;
            this.employeeSPService = employeeSPService;
            this.ApproveCellingService = ApproveCellingService;
            this.groupwiseReportService = groupwiseReportService;
        }
        #endregion


        // GET: Employment
        public ActionResult Index()
        {
            return View();
        }// END 


        #region Methods

        public JsonResult GetLoanApprovals(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                long totalCount;

                var param1 = new { @EmpID = LoggedInEmployeeID };
                var LoanInstallMent = ultimateReportService.GetCenterROleWise(param1);

                IEnumerable<DBLoanApproveDetailModel> allSavingsummary;
                if (LoanInstallMent != null)
                {
                    var empType = LoanInstallMent.Tables[0].Rows[0]["Name"].ToString();
                    if (empType == "FO")
                    {
                        allSavingsummary = loansSummaryService.GetLoanApproveDetailPaged(SessionHelper.LoginUserOfficeID.Value, filterColumn, filterValue, jtStartIndex, jtPageSize, jtSorting, out totalCount, TransactionDate, Convert.ToInt16(LoggedInOrganizationID), Convert.ToInt16(LoggedInEmployeeID));
                    }
                    else

                        allSavingsummary = loansSummaryService.GetLoanApproveDetailPaged(SessionHelper.LoginUserOfficeID.Value, filterColumn, filterValue, jtStartIndex, jtPageSize, jtSorting, out totalCount, TransactionDate, Convert.ToInt16(LoggedInOrganizationID));
                }
                else
                    allSavingsummary = loansSummaryService.GetLoanApproveDetailPaged(SessionHelper.LoginUserOfficeID.Value, filterColumn, filterValue, jtStartIndex, jtPageSize, jtSorting, out totalCount, TransactionDate, Convert.ToInt16(LoggedInOrganizationID));

                var currentPageRecords = Mapper.Map<IEnumerable<DBLoanApproveDetailModel>, IEnumerable<LoanApprovalViewModel>>(allSavingsummary);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        private void MapDropDownList(LoanApprovalViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }
            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID));
            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName) + ' ' + (string.IsNullOrEmpty(m.RefereeName) ? "" : m.RefereeName)), Value = m.MemberID.ToString() });
            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;


            var allpurpose = purposeService.SearchPurpose(Convert.ToInt16(LoggedInOrganizationID));
            var viewPurpose = allpurpose.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.PurposeCode, m.PurposeName), Value = m.PurposeID.ToString() });
            model.purposeListItems = viewPurpose;

            // Center Starts

            var param1 = new { @EmpID = LoggedInEmployeeID };
            var LoanInstallMent = ultimateReportService.GetCenterROleWise(param1);
            List<CenterViewModel> List_CenterViewModel = new List<CenterViewModel>();
            DataSet centerInfo;
            var paramC = new { OfficeId = LoginUserOfficeID };

            if (LoanInstallMent != null)
            {
                var empType = LoanInstallMent.Tables[0].Rows[0]["Name"].ToString();
                if (empType == "FO")
                {
                    var paramFOWISE = new { OfficeId = LoginUserOfficeID, EmpID = LoggedInEmployeeID, empType = empType };
                    centerInfo = groupwiseReportService.GetDataDataseAccess(paramFOWISE, "GetOnlyCenterFOWISE");
                }
                else

                    centerInfo = groupwiseReportService.GetDataDataseAccess(paramC, "GetOnlyCenter");
            }
            else
            {
                paramC = new { OfficeId = LoginUserOfficeID };
                centerInfo = groupwiseReportService.GetDataDataseAccess(paramC, "GetOnlyCenter");
            }

            List_CenterViewModel = centerInfo.Tables[0].AsEnumerable()
            .Select(row => new CenterViewModel
            {
                CenterID = row.Field<int>("CenterID"),
                CenterCode = row.Field<string>("CenterCode"),
                CenterName = row.Field<string>("CenterName")
            }).ToList();

            var viewCenter = List_CenterViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterCode.ToString() + ", " + x.CenterName.ToString()
            });
            var center_items = new List<SelectListItem>();
            if (viewCenter.ToList().Count > 0)
            {
                center_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            }
            center_items.AddRange(viewCenter);
            //var centerList = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });
            model.centerListItems = center_items;

            // Center Ends

            var alloffice = officeService.GetAll().Where(l => l.OfficeID == SessionHelper.LoginUserOfficeID.Value && l.OrgID == LoggedInOrganizationID);
            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });
            model.officeListItems = viewOffice;

            List<ProductViewModel> List_MemberPassBookRegisterViewModel = new List<ProductViewModel>();
            var param = new { PaymentFrq = string.IsNullOrEmpty(model.frequencyMode) ? "0" : model.frequencyMode };
            var div_items = ultimateReportService.GetProductListByFrequencyMode(param);

            var viewProduct = div_items.Tables[0].AsEnumerable().Select(row => new SelectListItem
            {
                Value = row.Field<Int16>("ProductID").ToString(),
                Text = row.Field<string>("ProductCode") + ' ' + row.Field<string>("ProductName")
            });
            var proditems = new List<SelectListItem>();
            proditems.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            proditems.AddRange(viewProduct);
            model.productListItems = proditems;

            var allInvestor = investorService.GetAll().Where(i => i.IsActive == true && i.OrgID == LoggedInOrganizationID).OrderBy(i => i.InvestorCode);
            var viewInvestor = allInvestor.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.InvestorCode, m.InvestorName), Value = m.InvestorID.ToString() });
            model.investorListItems = viewInvestor;
            var paymentMode = new List<SelectListItem>();

            if (LoggedInOrganizationID == 82)
            {
                paymentMode.Add(new SelectListItem() { Text = "Cash", Value = "101", Selected = true });
                paymentMode.Add(new SelectListItem() { Text = "Bank", Value = "102" });
                paymentMode.Add(new SelectListItem() { Text = "ProductLoan", Value = "103" });
            }
            else
            {
                paymentMode.Add(new SelectListItem() { Text = "Cash", Value = "101", Selected = true });
                paymentMode.Add(new SelectListItem() { Text = "Bank", Value = "102" });
            }


            model.paymentMode = paymentMode;
            var disType = new List<SelectListItem>();
            disType.Add(new SelectListItem() { Text = "Once at a time", Value = "1", Selected = true });
            model.disType = disType;


            var mempassBook = memberPassBookRegisterService.GetAll().Where(i => i.IsActive == true && i.OrgID == LoggedInOrganizationID).OrderBy(i => i.MemberPassBookNO);
            var viewmempassBook = mempassBook.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.MemberPassBookRegisterID.ToString(),
                Text = string.Format("{0} ", x.MemberPassBookNO.ToString())
            });
            var mempassBookitems = new List<SelectListItem>();
            mempassBookitems.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            mempassBookitems.AddRange(viewmempassBook);
            model.MemberPassBookNOListItems = mempassBookitems;
            ViewData["memberPass"] = viewmempassBook;


            var paymentFrequency = new List<SelectListItem>();
            paymentFrequency.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            paymentFrequency.Add(new SelectListItem() { Text = "Weekly", Value = "W" });
            paymentFrequency.Add(new SelectListItem() { Text = "Monthly", Value = "M" });
            paymentFrequency.Add(new SelectListItem() { Text = "Fortnightly", Value = "F" });
            paymentFrequency.Add(new SelectListItem() { Text = "Daily", Value = "D" });
            model.freqMode = paymentFrequency;
        }

        public JsonResult GetProfessionList(string LoanSummaryID, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                string ProfessionIds = Convert.ToString(LoanSummaryID);
                 
                List<LoanApprovalViewModel> List_ViewModel = new List<LoanApprovalViewModel>();
                var param = new { LoanSummaryID = LoanSummaryID };
                var empList = employeeSPService.GetDataWithParameter(param, "EmploymentDetailsData");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new LoanApprovalViewModel
                {
                    txtMaleFullTimeP1       = row.Field<int?>("SEmpMaleFullTimeP1"),
                    txtFeMaleFullTimeP1     = row.Field<int?>("SEmpFeMaleFullTimeP1"),
                    txtMalePartTimeP1       = row.Field<int?>("SEmpMalePartTimeP1"),
                    txtFeMalePartTimeP1     = row.Field<int?>("SEmpFeMalePartTimeP1"),

                    txtMaleFullTimeP3       = row.Field<int?>("WEmpMaleFullTimeP3"),
                    txtFeMaleFullTimeP3     = row.Field<int?>("WEmpFeMaleFullTimeP3"),
                    txtMalePartTimeP3       = row.Field<int?>("WEmpMalePartTimeP3"),
                    txtFeMalePartTimeP3     = row.Field<int?>("WEmpFeMalePartTimeP3"),
 
                }).ToList();

                if (LoanSummaryID != null)
                {
                    return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
                }

                var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function


        public JsonResult UpdateEmployment(string LoanSummaryID, string txtMaleFullTimeP1, string txtFeMaleFullTimeP1,  string txtMalePartTimeP1, string txtFeMalePartTimeP1, string txtMaleFullTimeP3, string txtFeMaleFullTimeP3, string txtMalePartTimeP3, string txtFeMalePartTimeP3)
        {
            string result = "OK";
            try
            { 
                DateTime UpdateDate = DateTime.Now;
                var param = new { LoanSummaryID = LoanSummaryID, OfficeId = LoginUserOfficeID, MemberId = 0, CenterId = 0, txtMaleFullTimeP1 = txtMaleFullTimeP1, txtFeMaleFullTimeP1 = txtFeMaleFullTimeP1, txtMalePartTimeP1 = txtMalePartTimeP1, txtFeMalePartTimeP1 = txtFeMalePartTimeP1, txtMaleFullTimeP3 = txtMaleFullTimeP3, txtFeMaleFullTimeP3 = txtFeMaleFullTimeP3, txtMalePartTimeP3 = txtMalePartTimeP3, txtFeMalePartTimeP3 = txtFeMalePartTimeP3 };
                var val = employeeSPService.GetDataWithParameter(param, "SetEmploymentDetails");

            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Methods

    }// END Class
}// END Namespace