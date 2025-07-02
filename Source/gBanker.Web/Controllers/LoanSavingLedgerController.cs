using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.Reports;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using gBanker.Service.ReportExecutionService;
using Kendo.Mvc.Extensions;
using DotNetOpenAuth.Messaging;

namespace gBanker.Web.Controllers
{
    public class LoanSavingLedgerController : BaseController
    {
        #region Variables
        private readonly IDailyReportService dailyReportService;
        private readonly ICenterService centerService;
        private readonly IMemberService memberService;
        private readonly IMemberCategoryService memberCategoryService;
        private readonly IProductService productService;
        private readonly IWeeklyReportService weeklyReportService;
        private readonly ILoanTrxService loanTrxService;
        private readonly ILoanSummaryService loanSummaryService;
        private readonly IGroupwiseReportService groupwiseReportService;
        private readonly IOfficeService officeService;
        private static DataTable mydt;

        public LoanSavingLedgerController(IDailyReportService dailyReportService, ICenterService centerService, IMemberService memberService, IMemberCategoryService memberCategoryService, IProductService productService, IWeeklyReportService weeklyReportService, ILoanTrxService loanTrxService, ILoanSummaryService loanSummaryService, IGroupwiseReportService groupwiseReportService, IOfficeService officeService)
        {
            this.dailyReportService = dailyReportService;
            this.centerService = centerService;
            this.memberService = memberService;
            this.memberCategoryService = memberCategoryService;
            this.productService = productService;
            this.weeklyReportService = weeklyReportService;
            this.loanTrxService = loanTrxService;
            this.loanSummaryService = loanSummaryService;
            this.groupwiseReportService = groupwiseReportService;
            this.officeService = officeService;
        }
        #endregion

        #region Methods
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetCenterList()
        {
            var getCenter = centerService.GetByOfficeId(Convert.ToInt32(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID));
            var viewCenter = getCenter.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterCode.ToString() +", "+ x.CenterName.ToString()
            });
            var voucher_items = new List<SelectListItem>();
            if (viewCenter.ToList().Count > 0)
            {
                voucher_items.Add(new SelectListItem() { Text = "Get All", Value = "0", Selected = true });
            }
            voucher_items.AddRange(viewCenter);
            return Json(voucher_items, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetMainProductList()
        //{
        //    var getProduct = productService.GetAll().FirstOrDefault();
        //    if (getProduct != null)
        //    {
        //        var viewCenter = getProduct.Select(x => x).ToList().Select(x => new SelectListItem
        //        {
        //            Value = x.CenterID.ToString(),
        //            Text = x.CenterCode.ToString() + ", " + x.CenterName.ToString()
        //        });
        //    }            
        //    var voucher_items = new List<SelectListItem>();
        //    if (viewCenter.ToList().Count > 0)
        //    {
        //        voucher_items.Add(new SelectListItem() { Text = "Get All", Value = "0", Selected = true });
        //    }
        //    voucher_items.AddRange(viewCenter);
        //    return Json(voucher_items, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult GetMemberList(int centerId)
        {
            try
            {
                List<GetMemberListViewModel> List_Members = new List<GetMemberListViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GetMemberList_Dropdown");

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

        public JsonResult GetAccountCodeList(string AccoCode)
        {
            try
            {
                List<AccChartViewModel> List_Members = new List<AccChartViewModel>();

                var param = new { AccLevel = Convert.ToInt16(AccoCode) };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Proc_get_AccCode");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new AccChartViewModel
                {
                    AccCode = row.Field<string>("AccCode"),
                    //LoanTerm = row.Field<string>("LoanTerm"),
                    AccName = row.Field<string>("AccName")

                }).ToList();

                var viewProduct = List_Members.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.AccCode.ToString(),
                    Text = x.AccName.ToString() 
                });

                var d_items = new List<SelectListItem>();
                d_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
                d_items.AddRange(viewProduct);
                return Json(d_items, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetProductList()
        {
            var getProduct = productService.GetAll().Where(s => s.IsActive == true && s.OrgID == LoggedInOrganizationID).OrderBy(e => e.ProductCode);
            var viewProduct = getProduct.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = x.ProductCode.ToString() + ", " + x.ProductName.ToString()
            });
            var prod_items = new List<SelectListItem>();
            if (viewProduct.ToList().Count > 0)
            {
                prod_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            }
            prod_items.AddRange(viewProduct);
            return Json(prod_items, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetMainProductList()
        {
            try
            {
                List<SelectListItem> List_ProductViewModel = new List<SelectListItem>();
                var productList = weeklyReportService.GetMainProductList();

                List_ProductViewModel = productList.Tables[0].AsEnumerable()
                .Select(row => new SelectListItem
                {
                    Value = row.Field<string>("MainProductCode"),
                    Text = row.Field<string>("MainItemName")
                }).ToList();

                var mainProductList = new List<SelectListItem>();
                if (List_ProductViewModel.ToList().Count > 0)
                {
                    mainProductList.AddRange(List_ProductViewModel);
                    //mainProductList.Add(new SelectListItem() { Text = "Get All", Value = "0", Selected = true });
                }                
                return Json(mainProductList, JsonRequestBehavior.AllowGet);                
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GenerateLoanLedgerReport(string Qtype, string Center, string Member, string ProductID, string LoanTerm)
        {
            try
            {

                var param = new { Qtype = Qtype, Office = SessionHelper.LoginUserOfficeID, Center = Center, Member = Member, Org = SessionHelper.LoginUserOrganizationID, ProductID = ProductID, LoanTerm = LoanTerm };
                var LoanLedgers = dailyReportService.GetDataLoanLedgerReport(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("UptoDate", TransactionDate);
                ReportHelper.PrintReport("rptLoanLedger.rpt", LoanLedgers.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        /*
        public ActionResult GenerateLoanBalanceReport(string Qtype, string Center)
        {
            try
            {

                var param = new { Qtype = Qtype, Office = SessionHelper.LoginUserOfficeID, Center = Center, Org = SessionHelper.LoginUserOrganizationID };
                var LoanBalances = dailyReportService.GetDataLoanBalanceReport(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("UptoDate", TransactionDate);
                if (LoggedInOrganizationID == 99)
                {

                    ReportHelper.PrintReport("rptLoanBalance_Proshikha.rpt", LoanBalances.Tables[0], reportParam);
                }
                else
                    ReportHelper.PrintReport("rptLoanBalance.rpt", LoanBalances.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        */
        
        public ActionResult GenerateLoanBalanceReportExcel(string Qtype, string Center)
        {
            try
            {
                mydt = new DataTable();
                var param = new { Qtype = Qtype, Office = SessionHelper.LoginUserOfficeID, Center = Center, Org = SessionHelper.LoginUserOrganizationID };
                var LoanBalances = dailyReportService.GetDataLoanBalanceReport(param);
                mydt = LoanBalances.Tables[0];
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("UptoDate", TransactionDate);
                var or = 200;
                if (LoggedInOrganizationID == 200 || LoggedInOrganizationID == 60)
                {
                    ReportHelper.PrintExcelReport("rptLoanBalance_Proshikha.rpt", mydt, reportParam);                   
                }
                else if (LoggedInOrganizationID == 99 || LoggedInOrganizationID == 60)
                {
                    ReportHelper.PrintReport("rptLoanBalance_Proshikha.rpt", mydt, reportParam);
                }
                else
                    ReportHelper.PrintReport("rptLoanBalance.rpt", mydt, reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult GenerateLoanBalanceReportPDF(string Qtype, string Center)
        {
            try
            {
                var param = new { Qtype = Qtype, Office = SessionHelper.LoginUserOfficeID, Center = Center, Org = SessionHelper.LoginUserOrganizationID };                
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("UptoDate", TransactionDate);
                //var or = 200;
                if (LoggedInOrganizationID == 200)
                {                   
                    ReportHelper.PrintPdfReport("rptLoanBalance_Proshikha.rpt", mydt, reportParam);
                }
                else if (LoggedInOrganizationID == 99)
                {
                    ReportHelper.PrintReport("rptLoanBalance_Proshikha.rpt", mydt, reportParam);
                }
                else
                    ReportHelper.PrintReport("rptLoanBalance.rpt", mydt, reportParam);
                
                mydt.Clear();
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateBranchWiseRatioAnalysisReport(string DateFrom, string DateTo)
        {
            try
            {

                var param = new { FirstDate = Convert.ToDateTime(DateFrom), Date = Convert.ToDateTime(DateTo), Org = LoggedInOrganizationID };
                var MLists = dailyReportService.GetBranchWiseRatioAnalysisReport(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("FromDate", DateFrom);
                reportParam.Add("ToDate", DateTo);
                ReportHelper.PrintReport("rptBranchWiseAnalysisReport.rpt", MLists.Tables[0], reportParam);
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateProductivityRatioReport( string DateFrom, string DateTo, int office_id)
        {
            try
            {

                //var param = new { Office = SessionHelper.LoginUserOfficeID, Center = Center };
                var param = new { StartDate = Convert.ToDateTime(DateFrom), ProcessDate = Convert.ToDateTime(DateTo), OfficeID = office_id };
                var MLists = dailyReportService.GetProductivityRatioReport(param);

                var reportParam = new Dictionary<string, object>();
              //  reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("FromDate", DateFrom);
                reportParam.Add("ToDate", DateTo);

                //ReportHelper.PrintReport("rptMemberList.rpt", MLists.Tables[0], new Dictionary<string, object>());
                //ReportHelper.PrintReport("rptProductivityRatioAnalysis.rpt", MLists.Tables[0], reportParam);
                ReportHelper.PrintReport("rptProductivityRatioAnalysisNew.rpt", MLists.Tables[0], reportParam);

                return Content(string.Empty);

                // return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //public ActionResult GenerateWriteOffListReport(string DateTo, string memberStatus, string ReportSt)
        //{
        //    try
        //    {

        //        if (ReportSt == "1")
        //        {

        //            //var param = new { Office = SessionHelper.LoginUserOfficeID, Center = Center };
        //            var param = new { OrgID = LoggedInOrganizationID, Office = LoginUserOfficeID, MemberID = 1, Trandate = Convert.ToDateTime(DateTo), writeoffyear = Convert.ToInt16(memberStatus), DisburseDate = Convert.ToDateTime(DateTo) };
        //            var MLists = dailyReportService.GetPreWriteOffListReport(param);

        //            var reportParam = new Dictionary<string, object>();

        //            // reportParam.Add("FromDate", DateFrom);
        //            reportParam.Add("DateTo", DateTo);

        //            //ReportHelper.PrintReport("rptMemberList.rpt", MLists.Tables[0], new Dictionary<string, object>());
        //            ReportHelper.PrintReport("rptgetWriteOffList_Borrower.rpt", MLists.Tables[0], reportParam);
        //        }

        //        else
        //        {

        //            //var param = new { Office = SessionHelper.LoginUserOfficeID, Center = Center };
        //            var param = new { OrgID = LoggedInOrganizationID, Office = LoginUserOfficeID, MemberID = 1, Trandate = Convert.ToDateTime(DateTo), writeoffyear = Convert.ToInt16(memberStatus), DisburseDate = Convert.ToDateTime(DateTo) };
        //            var MLists = dailyReportService.GetPreWriteOffListReport(param);

        //            var reportParam = new Dictionary<string, object>();

        //            // reportParam.Add("FromDate", DateFrom);
        //            reportParam.Add("DateTo", DateTo);

        //            //ReportHelper.PrintReport("rptMemberList.rpt", MLists.Tables[0], new Dictionary<string, object>());
        //            ReportHelper.PrintReport("rptgetWriteOffListLoanType_Product.rpt", MLists.Tables[0], reportParam);
        //        }

        //        return Content(string.Empty);

        //        // return Json(new { Result = "OK" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}

        public ActionResult GenerateWriteOffListReport(string DateTo, string memberStatus, string ReportSt, string ExportType)        {            try            {                if (ReportSt == "1")                {                    var param = new { OrgID = LoggedInOrganizationID, Office = LoginUserOfficeID, MemberID = 1, Trandate = Convert.ToDateTime(DateTo), writeoffyear = Convert.ToInt16(memberStatus), DisburseDate = Convert.ToDateTime(DateTo) };                    var MLists = dailyReportService.GetPreWriteOffListReport(param);                    var reportParam = new Dictionary<string, object>();                    reportParam.Add("DateTo", DateTo);                    if (ExportType == "pdf")                    {                        ReportHelper.PrintReport("rptgetWriteOffList_Borrower.rpt", MLists.Tables[0], reportParam);                    }                    else //Excel
                    {                        ReportHelper.ExportExcelReportFormated("rptgetWriteOffList_Borrower.rpt", MLists.Tables[0], reportParam, "Pre_WriteOff_Borrowerwise");                    }                }                else                {                    var param = new { OrgID = LoggedInOrganizationID, Office = LoginUserOfficeID, MemberID = 1, Trandate = Convert.ToDateTime(DateTo), writeoffyear = Convert.ToInt16(memberStatus), DisburseDate = Convert.ToDateTime(DateTo) };                    var MLists = dailyReportService.GetPreWriteOffListReport(param);                    var reportParam = new Dictionary<string, object>();                    reportParam.Add("DateTo", DateTo);                    if (ExportType == "pdf")                    {                        ReportHelper.PrintReport("rptgetWriteOffListLoanType_Product.rpt", MLists.Tables[0], reportParam);                    }                    else //Excel
                    {                        ReportHelper.ExportExcelReportFormated("rptgetWriteOffListLoanType_Product.rpt", MLists.Tables[0], reportParam, "Pre_WriteOff_ProductWise");                    }                }                return Content(string.Empty);            }            catch (Exception ex)            {                return Json(new { Result = "ERROR", Message = ex.Message });            }        }

        public ActionResult GenerateWriteOffOfficeWiseListReport(string DateTo, string memberStatus, string ReportSt)
        {
            try
            {

                if (ReportSt == "1")
                {

                    //var param = new { Office = SessionHelper.LoginUserOfficeID, Center = Center };
                    var param = new { OrgID = LoggedInOrganizationID, Office = LoginUserOfficeID, MemberID = 1, Trandate = Convert.ToDateTime(DateTo), writeoffyear = Convert.ToInt16(memberStatus), DisburseDate = Convert.ToDateTime(DateTo), AllOffice=1 };
                    var MLists = dailyReportService.GetPreWriteOffListReportAllOffice(param);

                    var reportParam = new Dictionary<string, object>();

                    // reportParam.Add("FromDate", DateFrom);
                    reportParam.Add("DateTo", DateTo);

                    //ReportHelper.PrintReport("rptMemberList.rpt", MLists.Tables[0], new Dictionary<string, object>());
                    ReportHelper.PrintReport("rptgetWriteOffList_BorrowerOfficeWise.rpt", MLists.Tables[0], reportParam);
                }

                else
                {

                    //var param = new { Office = SessionHelper.LoginUserOfficeID, Center = Center };
                    var param = new { OrgID = LoggedInOrganizationID, Office = LoginUserOfficeID, MemberID = 1, Trandate = Convert.ToDateTime(DateTo), writeoffyear = Convert.ToInt16(memberStatus), DisburseDate = Convert.ToDateTime(DateTo), AllOffice = 1 };
                    var MLists = dailyReportService.GetPreWriteOffListReportAllOffice(param);

                    var reportParam = new Dictionary<string, object>();

                    // reportParam.Add("FromDate", DateFrom);
                    reportParam.Add("DateTo", DateTo);

                    //ReportHelper.PrintReport("rptMemberList.rpt", MLists.Tables[0], new Dictionary<string, object>());
                    ReportHelper.PrintReport("rptgetWriteOffList_BorrowerOfficeWise.rpt", MLists.Tables[0], reportParam);
                }

                return Content(string.Empty);

                // return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateQualityRatioReport(string DateFrom, string DateTo, int office_id)
        {
            try
            {

                //var param = new { Office = SessionHelper.LoginUserOfficeID, Center = Center };
                var param = new {StartDate = Convert.ToDateTime(DateFrom), EndDate = Convert.ToDateTime(DateTo), OfficeID = office_id };
                var MLists = dailyReportService.GetQualityRatioAnalysisReport(param);

                var reportParam = new Dictionary<string, object>();
                //  reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("FromDate", DateFrom);
                reportParam.Add("ToDate", DateTo);

                //ReportHelper.PrintReport("rptMemberList.rpt", MLists.Tables[0], new Dictionary<string, object>());
                //ReportHelper.PrintReport("rptPortfolioQualityRatio.rpt", MLists.Tables[0], reportParam);
                ReportHelper.PrintReport("rptPortfolioQualityRatioNew.rpt", MLists.Tables[0], reportParam);

                return Content(string.Empty);

                // return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult GenerateSustainabilityAndProfitabilityRatioReport(string DateFrom, string DateTo, int office_id)
        {
            try
            {
                var param = new {StartDate = Convert.ToDateTime(DateFrom), EndDate = Convert.ToDateTime(DateTo), OfficeID = office_id };
                var MLists = dailyReportService.GetSustainabilityAndProfitabilityRatioReport(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("FromDate", DateFrom);
                reportParam.Add("ToDate", DateTo);
                ReportHelper.PrintReport("rptProfitabilitySustainabilityRatio.rpt", MLists.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult GenerateFinancialManagmentSolvencyRatioReport(string DateFrom, string DateTo, int office_id)
        {
            try
            {
                var param = new {StartDate = Convert.ToDateTime(DateFrom), EndDate = Convert.ToDateTime(DateTo), OfficeID = office_id };
                var MLists = dailyReportService.GetFinancialManagmentSolvencyRatioReport(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("FromDate", DateFrom);
                reportParam.Add("ToDate", DateTo);
                ReportHelper.PrintReport("rptFinancialManagmentSolvencyRatio.rpt", MLists.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult GenerateAccountCodeWiseTrialBalance(string DateFrom, string DateTo, string memberStatus, string AccCode)
        {
            try
            {

                //var param = new { Office = SessionHelper.LoginUserOfficeID, Center = Center };
                var param = new { from_date = Convert.ToDateTime(DateFrom), to_date = Convert.ToDateTime(DateTo), AccLevel = Convert.ToInt16(memberStatus), AccCode = AccCode };
                var MLists = dailyReportService.GetTrialBalance_AccountCodeWise(param);

                var reportParam = new Dictionary<string, object>();
                //  reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("FromDate", DateFrom);
                reportParam.Add("ToDate", DateTo);

                //ReportHelper.PrintReport("rptMemberList.rpt", MLists.Tables[0], new Dictionary<string, object>());
                ReportHelper.PrintReport("Rpt_Acc_TrialBalance_AccountCodeWise.rpt", MLists.Tables[0], reportParam);

                return Content(string.Empty);

                // return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult GenerateAccountCodeWiseTrialBalanceOfficeWise(string DateFrom, string DateTo, string memberStatus, string AccCode)
        {
            try
            {

                //var param = new { Office = SessionHelper.LoginUserOfficeID, Center = Center };
                var param = new { from_date = Convert.ToDateTime(DateFrom), to_date = Convert.ToDateTime(DateTo), AccLevel = Convert.ToInt16(memberStatus), AccCode = AccCode };
                var MLists = dailyReportService.GetTrialBalance_AccountCodeWise(param);

                var reportParam = new Dictionary<string, object>();
                //  reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("FromDate", DateFrom);
                reportParam.Add("ToDate", DateTo);

                //ReportHelper.PrintReport("rptMemberList.rpt", MLists.Tables[0], new Dictionary<string, object>());
                ReportHelper.PrintReport("Rpt_Acc_TrialBalance_AccountCodeWise_DSK.rpt", MLists.Tables[0], reportParam);

                return Content(string.Empty);

                // return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
//-----------------------------------------------------------------------------------------------------

 public ActionResult WaitingForLoanList()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");

            ViewData["EmployeeList"] = items;

            if (IsDayInitiated)
                if (IsDayInitiated)
                {
                    ViewData["TrxdateTo"] = TransactionDate.ToString("dd-MMM-yyyy");
                }
                else
                {
                    ViewData["TrxdateTo"] = TransactionDate.ToString("dd-MMM-yyyy");
                }

            return View();
        }



        public ActionResult GenerateWaitingForLoanList(string DateTo, string EmployeeId)
        {
            try
            {

                //var param = new { Office = SessionHelper.LoginUserOfficeID, Center = Center };
                var param = new { Office = SessionHelper.LoginUserOfficeID, EndDate = Convert.ToDateTime(DateTo), EmployeeID = EmployeeId };
                var MLists = dailyReportService.GetWaitingForLoanList(param);

                var reportParam = new Dictionary<string, object>();
                //  reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                //  reportParam.Add("FromDate", DateFrom);
                reportParam.Add("ToDate", DateTo);

                //ReportHelper.PrintReport("rptMemberList.rpt", MLists.Tables[0], new Dictionary<string, object>());
                ReportHelper.PrintReport("RPTWaitingforLoanMemberList.rpt", MLists.Tables[0], reportParam);

                return Content(string.Empty);

                // return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //public ActionResult GenerateWaitingForLoanList( string DateTo)
        //{
        //    try
        //    {

        //        //var param = new { Office = SessionHelper.LoginUserOfficeID, Center = Center };
        //        var param = new { Office = SessionHelper.LoginUserOfficeID, EndDate = Convert.ToDateTime(DateTo) };
        //        var MLists = dailyReportService.GetWaitingForLoanList(param);

        //        var reportParam = new Dictionary<string, object>();
        //        //  reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //      //  reportParam.Add("FromDate", DateFrom);
        //        reportParam.Add("ToDate", DateTo);

        //        //ReportHelper.PrintReport("rptMemberList.rpt", MLists.Tables[0], new Dictionary<string, object>());
        //        ReportHelper.PrintReport("RPTWaitingforLoanMemberList.rpt", MLists.Tables[0], reportParam);

        //        return Content(string.Empty);

        //        // return Json(new { Result = "OK" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        public ActionResult GenerateSavingLedgerReport(string Qtype, string Center, string Member)
        {
            try
            {

                //var param = new { Office = SessionHelper.LoginUserOfficeID, Center = Center };
                var param = new { Qtype = Qtype, Office = SessionHelper.LoginUserOfficeID, Center = Center, Member = Member, Org = SessionHelper.LoginUserOrganizationID };
                var SavingLedgers = dailyReportService.GetDataSavingLedgerReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                //reportParam);

                //ReportHelper.PrintReport("rptSavingLedger.rpt", SavingLedgers.Tables[0], new Dictionary<string, object>());
                ReportHelper.PrintReport("rptSavingLedger.rpt", SavingLedgers.Tables[0], reportParam);

                return Content(string.Empty);

                // return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateSavingBalanceReport(string Qtype, string Center)
        {
            try
            {

                //var param = new { Office = SessionHelper.LoginUserOfficeID, Center = Center };
                var param = new { Qtype = Qtype, Office = SessionHelper.LoginUserOfficeID, Center = Center, Org = SessionHelper.LoginUserOrganizationID };
                var SavingBalances = dailyReportService.GetDataSavingBalanceReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                if (LoggedInOrganizationID == 99)
                {
                    ReportHelper.PrintReport("rptSavingBalace_Proshikha.rpt", SavingBalances.Tables[0], reportParam);
                }
                else


                //ReportHelper.PrintReport("rptSavingBalace.rpt", SavingBalances.Tables[0], new Dictionary<string, object>());
                ReportHelper.PrintReport("rptSavingBalace.rpt", SavingBalances.Tables[0], reportParam);

                return Content(string.Empty);

                // return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //public ActionResult GenerateTodaysSummaryReportNew(string Date, string Qtype)
        //  {
        //      try
        //      {
        //          var param1 = new { Office = SessionHelper.LoginUserOfficeID, Date = Date, Qtype = 1, Org = SessionHelper.LoginUserOrganizationID };
        //                        dailyReportService.GetDataTodaysSummaryReportNew(param1);

        //          var param = new { Office = SessionHelper.LoginUserOfficeID, Date = Date, Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID };
        //          var TodaysSummarys = dailyReportService.GetDataTodaysSummaryReportNew(param);

        //          var reportParam = new Dictionary<string, object>();
        //          reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //          reportParam.Add("FromDate", Date);
        //          ReportHelper.PrintReport("rptTodaysSummary.rpt", TodaysSummarys.Tables[0], reportParam);
        //          //ReportHelper.PrintReport("rptTodaysSummaryDesktop.rpt", TodaysSummarys.Tables[0], reportParam);

        //          return Content(string.Empty);
        //      }
        //      catch (Exception ex)
        //      {
        //          return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
        //      }
        //  }



        public ActionResult GenerateTodaysSummaryReportNew(string Date, string Qtype)
        {
            try
            {
                DataSet TodaysSummarys;
                var param1 = new { Office = SessionHelper.LoginUserOfficeID, Date = Date, Qtype = 1, Org = SessionHelper.LoginUserOrganizationID };
                if (LoggedInOrganizationID == 218)
                {
                    dailyReportService.GetDataTodaysSummaryReportNewSouthAfrica(param1);
                }
                else
                {
                    dailyReportService.GetDataTodaysSummaryReportNew(param1);
                }
               

                var param = new { Office = SessionHelper.LoginUserOfficeID, Date = Date, Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID };
                if (LoggedInOrganizationID == 218)
                {
                    TodaysSummarys = dailyReportService.GetDataTodaysSummaryReportNewSouthAfrica(param);
                }
                else
                {
                    TodaysSummarys = dailyReportService.GetDataTodaysSummaryReportNew(param);
                }
                    

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("FromDate", Date);
                if (LoggedInOrganizationID == 190)
                {
                    ReportHelper.PrintReport("rptTodaysSummaryOTR_RDRS.rpt", TodaysSummarys.Tables[0], reportParam);
                }
                else if (LoggedInOrganizationID==30)
                {
                    ReportHelper.PrintReport("rptTodaysSummaryOTR_Mukti.rpt", TodaysSummarys.Tables[0], reportParam);
                }
                else if (LoggedInOrganizationID == 218)
                {
                    ReportHelper.PrintReport("rptTodaysSummary_SouthAfrica.rpt", TodaysSummarys.Tables[0], reportParam);
                }
                else
                ReportHelper.PrintReport("rptTodaysSummaryOTR.rpt", TodaysSummarys.Tables[0], reportParam);
                //ReportHelper.PrintReport("rptTodaysSummaryDesktop.rpt", TodaysSummarys.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult LoanOutstandingSaving()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            return View();
        }
        public ActionResult GenerateLoanOutstandingSavingsReport(string Qtype, string Center)
        {
            try
            {

                //var param = new { Office = SessionHelper.LoginUserOfficeID, Center = Center };
                var param = new { officeID = SessionHelper.LoginUserOfficeID, SamityId = Center };
                var SavingBalances = dailyReportService.GetLoanOutstandingSavingsReport(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("param_orgNo", SessionHelper.LoginUserOrganizationID);
                reportParam.Add("param_TransactionDate", SessionHelper.TransactionDate.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture));
                //ReportHelper.PrintReport("rptSavingBalace.rpt", SavingBalances.Tables[0], new Dictionary<string, object>());
                ReportHelper.PrintReport("LoanOutStandingSavings.rpt", SavingBalances.Tables[0], reportParam);
                return Content(string.Empty);

                // return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateTodaysSummaryReportNewSSRS(string Date, string Qtype)
        {

            var param1 = new { Office = SessionHelper.LoginUserOfficeID, Date = Date, Qtype = 1, Org = SessionHelper.LoginUserOrganizationID };
            dailyReportService.GetDataTodaysSummaryReportNew(param1);

            var paramValues = new List<ParameterValue>();

            paramValues.Add(new ParameterValue() { Name = "Org", Value = SessionHelper.LoginUserOrganizationID.Value.ToString() });
            paramValues.Add(new ParameterValue() { Name = "Office", Value = SessionHelper.LoginUserOfficeID.Value.ToString() });
            paramValues.Add(new ParameterValue() { Name = "Date", Value = Date });
            paramValues.Add(new ParameterValue() { Name = "QType", Value = Qtype });
            paramValues.Add(new ParameterValue() { Name = "OrganizationName", Value = ApplicationSettings.OrganiztionName });
            PrintSSRSReport("/gBanker_Reports/TodaysSummary", paramValues.ToArray(), "gBankerDbContext");

            return Content(string.Empty);
         }
        //public ActionResult GenerateMemberlistReport(string Qtype, string Center, string DateFrom, string DateTo, string memberStatus, string memberCategory, string gender)
        //{
        //    try
        //    {

        //        //var param = new { Office = SessionHelper.LoginUserOfficeID, Center = Center };
        //        var param = new { Qtype = Qtype, Office = SessionHelper.LoginUserOfficeID, Center = Center, Org = SessionHelper.LoginUserOrganizationID, FromDate = Convert.ToDateTime(DateFrom), ToDate = Convert.ToDateTime(DateTo), MeberStatus = Convert.ToInt32(memberStatus), MemberCategory = Convert.ToInt32(memberCategory), Gender = gender };
        //        var MLists = dailyReportService.GetDataMemberlistMemCategoryWiseReport(param);

        //        var reportParam = new Dictionary<string, object>();
        //        reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //        reportParam.Add("FromDate", DateFrom);
        //        reportParam.Add("ToDate", DateTo);

        //        //ReportHelper.PrintReport("rptMemberList.rpt", MLists.Tables[0], new Dictionary<string, object>());
        //        ReportHelper.PrintReport("rptMemberListMemberCategoryWise.rpt", MLists.Tables[0], reportParam);

        //        return Content(string.Empty);

        //        // return Json(new { Result = "OK" });
        //    }

        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        public ActionResult GenerateMemberlistReport(string Qtype, string Center, string DateFrom, string DateTo, string memberStatus, string memberCategory, string gender, string ExportType)
        {
            try
            {

                //var param = new { Office = SessionHelper.LoginUserOfficeID, Center = Center };
                var param = new { Qtype = Qtype, Office = SessionHelper.LoginUserOfficeID, Center = Center, Org = SessionHelper.LoginUserOrganizationID, FromDate = Convert.ToDateTime(DateFrom), ToDate = Convert.ToDateTime(DateTo), MeberStatus = Convert.ToInt32(memberStatus), MemberCategory = Convert.ToInt32(memberCategory), Gender = gender };
                var MLists = dailyReportService.GetDataMemberlistMemCategoryWiseReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("FromDate", DateFrom);
                reportParam.Add("ToDate", DateTo);

                //ReportHelper.PrintReport("rptMemberList.rpt", MLists.Tables[0], new Dictionary<string, object>());

                if (ExportType == "pdf")
                {
                    ReportHelper.PrintReport("rptMemberListMemberCategoryWise.rpt", MLists.Tables[0], reportParam);
                }
                else
                {
                    ReportHelper.ExportExcelReportFormated("rptMemberListMemberCategoryWise.rpt", MLists.Tables[0], reportParam, "MemberlistReport");
                }

                return Content(string.Empty);

                // return Json(new { Result = "OK" });
            }

            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult AccountBranchWiseTrialBalance()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["AccountCodeList"] = items;
            ViewData["TrxdateFrom"] = TransactionDate.ToString("dd-MMM-yyyy");
            ViewData["TrxdateTo"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }



        public ActionResult GenerateAccountBranchWiseTrialBalance(string DateFrom, string DateTo, string memberStatus, string AccCode)
        {
            try
            {

                var paramValues = new List<ParameterValue>();

                paramValues.Add(new ParameterValue() { Name = "from_date", Value = DateFrom });
                paramValues.Add(new ParameterValue() { Name = "to_date", Value = DateTo });
                paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = memberStatus });
                paramValues.Add(new ParameterValue() { Name = "AccCode", Value = AccCode });

                paramValues.Add(new ParameterValue() { Name = "OrganizationName", Value = ApplicationSettings.OrganiztionName });
                PrintSSRSReport("/gBanker_Reports/Rpt_Acc_TrialBalance_Branch_Wise_VERC", paramValues.ToArray(), "gBankerDbContext");

                return Content(string.Empty);

                // return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateDisburseReport(string Qtype, string Center, string Datefrom, string DateTo, string Product)
          {
              try
              {
                                                             
                  //var param = new { Office = SessionHelper.LoginUserOfficeID, Center = Center };
                  var param = new { Qtype = Qtype, Office = SessionHelper.LoginUserOfficeID, Center = Center, DateFrom = Convert.ToDateTime(Datefrom), DateTo = Convert.ToDateTime(DateTo), Org = SessionHelper.LoginUserOrganizationID, Product = Product };
                  var Disburses = dailyReportService.GetDataDisburseReport(param);

                  var reportParam = new Dictionary<string, object>();
                  reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                  //reportParam);

                  //ReportHelper.PrintReport("rptMemberList.rpt", MLists.Tables[0], new Dictionary<string, object>());
                  ReportHelper.PrintReport("rptDisburseHistory.rpt", Disburses.Tables[0], reportParam);

                  return Content(string.Empty);

                  // return Json(new { Result = "OK" });
              }
              catch (Exception ex)
              {
                  return Json(new { Result = "ERROR", Message = ex.Message });
              }
          }
        public ActionResult GeneratePendingDisburseListReport(string Qtype, string Center, string Datefrom, string DateTo, string Product)
          {
              try
              {

                  var param = new { Qtype = Qtype, Office = SessionHelper.LoginUserOfficeID, Center = Center, DateFrom = Convert.ToDateTime(Datefrom), DateTo = Convert.ToDateTime(DateTo), Org = SessionHelper.LoginUserOrganizationID, Product = Product };
                  var Pdisburselists = dailyReportService.GetDataDisburseReport(param);
                  var reportParam = new Dictionary<string, object>();
                  reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                  ReportHelper.PrintReport("rptPendingDisburseList.rpt", Pdisburselists.Tables[0], reportParam);
                  return Content(string.Empty);
              }
              catch (Exception ex)
              {
                  return Json(new { Result = "ERROR", Message = ex.Message });
              }
          }
        public ActionResult GenerateRepaymentScheduleReport(string member, string product,string loanterm)
        {
            try
            {
                var param = new { OfficeId = SessionHelper.LoginUserOfficeID, MemberID = member, ProductID = product, Loanterm = loanterm };
                var Pdisburselists = dailyReportService.getRepaymentScheduleReport(param);
                if (Pdisburselists.Tables[0].Rows.Count > 0)
                {
                    if (LoggedInOrganizationID == 218)
                    {
                        var reportParam = new Dictionary<string, object>();
                        reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                        ReportHelper.PrintReport("rptRepaymentScheduleRegisterSA.rpt", Pdisburselists.Tables[0], reportParam);
                    }
                    else
                    {
                        var reportParam = new Dictionary<string, object>();
                        reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                        ReportHelper.PrintReport("rptRepaymentScheduleRegister.rpt", Pdisburselists.Tables[0], reportParam);
                    }
                    
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
        //
        public ActionResult MemberList()
        {
            var model = new MemberViewModel();
            MapDropDownList(model);
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            ViewData["TrxdateFrom"] = TransactionDate.ToString("dd-MMM-yyyy");
            ViewData["TrxdateTo"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View(model);
        }
        private void MapDropDownList(MemberViewModel model)
        {
            List<MemberViewModel> List_ViewModel = new List<MemberViewModel>();

            //Member Category DropDwon
            var allMemcatService = memberCategoryService.GetAll().Where(w => w.IsActive == true && w.OrgID == LoggedInOrganizationID).OrderBy(o => o.MemberCategoryCode).ToList();
            var viewMemCatService = allMemcatService.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.MemberCategoryID.ToString(),
                Text = string.Format("{0}, {1}", x.MemberCategoryCode.ToString(), x.CategoryName.ToString())
            });
            var cat_items = new List<SelectListItem>();
            cat_items.Add(new SelectListItem() { Text = "Get All", Value = "0", Selected = true });
            cat_items.AddRange(viewMemCatService);
            model.MemberCategoryList = cat_items;

            //Gender DropDown
            var gender_items = new List<SelectListItem>();
            gender_items.Add(new SelectListItem() { Text = "Get All", Value = "0", Selected = true });
            gender_items.Add(new SelectListItem() { Text = "Male", Value = "M" });
            gender_items.Add(new SelectListItem() { Text = "Female", Value = "F" });
            gender_items.Add(new SelectListItem() { Text = "Transgender", Value = "T" });
            model.GenderList = gender_items;

        }


        // GET: /LoanSavingLedger/
        public ActionResult LoanBalance()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            return View();
        }
        public ActionResult LoanLedger()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            ViewData["ProductListByMember"] = items;
            ViewData["LoanTermList"] = items;
            return View();
        }
        public ActionResult SavingLedger()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            return View();
        }
        public ActionResult SavingBalance()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            return View();
        }
        public ActionResult TodaysSummary()
        {
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            
            else
                {
                    DateTime date = Convert.ToDateTime(string.IsNullOrEmpty(SessionHelper.LastDayEndDate.ToString()) ? "00-000-0000" : SessionHelper.LastDayEndDate.ToString());
                    ViewData["Trxdate"] = date.ToString("dd-MMM-yyyy");
                   // SessionHelper.LastDayEndDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }
        public ActionResult TodaysSummarySSRS()
        {
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }

            else
            {
                DateTime date = Convert.ToDateTime(string.IsNullOrEmpty(SessionHelper.LastDayEndDate.ToString()) ? "00-000-0000" : SessionHelper.LastDayEndDate.ToString());
                ViewData["Trxdate"] = date.ToString("dd-MMM-yyyy");
            }

            //else
            //{
            //    ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            //}
            //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }

        public ActionResult RatioAnalysis()
        {
 

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = offcdetail.OfficeLevel;
            ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;
            ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            ViewData["TrxdateFrom"] = TransactionDate.ToString("dd-MMM-yyyy");
            ViewData["TrxdateTo"] = TransactionDate.ToString("dd-MMM-yyyy");
            if (LoggedInOrganizationID != 54)
            {
                if (IsDayInitiated)
                {
                    var param1 = new { OfficeID = LoginUserOfficeID, BusinessDate = TransactionDate, OrgID = SessionHelper.LoginUserOrganizationID };
                    var LoanInstallMent = weeklyReportService.CheckAutoVoucher(param1);
                    if (LoanInstallMent.Tables[0].Rows.Count == 0)
                    {
                        var param = new { OfficeID = LoginUserOfficeID, BusinessDate = TransactionDate, OrgID = SessionHelper.LoginUserOrganizationID };
                        weeklyReportService.AutoVoucherCollectionProcess(param);
                    }
                }
            }

            return View();

        }
        public ActionResult DisburseList()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            ViewData["ProductList"] = items;

            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
                ViewData["TrxdateTo"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
                ViewData["TrxdateTo"] = VDate.ToString("dd-MMM-yyyy");
            }
            //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();

        }
        public ActionResult PendingDisburseList()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            ViewData["ProductList"] = items;

            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
                ViewData["TrxdateTo"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
                ViewData["TrxdateTo"] = VDate.ToString("dd-MMM-yyyy");
            }
            //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult BranchwiseRatioAnalysis()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");

            ViewData["TrxdateFrom"] = TransactionDate.ToString("dd-MMM-yyyy");
            ViewData["TrxdateTo"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult AccountCodeWiseTrialBalance()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["AccountCodeList"] = items;
            ViewData["TrxdateFrom"] = TransactionDate.ToString("dd-MMM-yyyy");
            ViewData["TrxdateTo"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }
        //
        public ActionResult RepaymentScheduleReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
           
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            ViewData["ProductListByMember"] = items;
            ViewData["LoanTermList"] = items;
            return View();
        }
        public ActionResult PreWriteOffList()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            if (IsDayInitiated)
            {
                ViewData["TrxdateTo"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxdateFrom"] = TransactionDate.ToString("dd-MMM-yyyy");
                ViewData["TrxdateTo"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
           
            return View();
        }

        public ActionResult PreWriteOffListOfficeWise()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            if (IsDayInitiated)
            {
                ViewData["TrxdateTo"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxdateFrom"] = TransactionDate.ToString("dd-MMM-yyyy");
                ViewData["TrxdateTo"] = TransactionDate.ToString("dd-MMM-yyyy");
            }

            return View();
        }
        //public ActionResult WaitingForLoanList()
        //{
        //    IEnumerable<SelectListItem> items = new SelectList(" ");
        //    if (IsDayInitiated)
        //    {
        //        ViewData["TrxdateTo"] = TransactionDate.ToString("dd-MMM-yyyy");
        //    }
        //    else
        //    {
        //        ViewData["TrxdateTo"] = TransactionDate.ToString("dd-MMM-yyyy");
        //    }

        //    return View();
        //}
        // GET: /LoanSavingLedger/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /LoanSavingLedger/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /LoanSavingLedger/Create
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
        // GET: /LoanSavingLedger/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /LoanSavingLedger/Edit/5
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
        // GET: /LoanSavingLedger/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /LoanSavingLedger/Delete/5
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

        public ActionResult DailyTransactionTopsheet()
        {
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }

            else
            {
                DateTime date = Convert.ToDateTime(string.IsNullOrEmpty(SessionHelper.LastDayEndDate.ToString()) ? "00-000-0000" : SessionHelper.LastDayEndDate.ToString());
                ViewData["Trxdate"] = date.ToString("dd-MMM-yyyy");
                // SessionHelper.LastDayEndDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }

        //public ActionResult GenerateDailyTransactionTopsheet(string Date, string Qtype)
        //{
        //    try
        //    {
        //        var param1 = new { Office = SessionHelper.LoginUserOfficeID, Date = Date, Qtype = 1, Org = SessionHelper.LoginUserOrganizationID };
        //        //dailyReportService.GetDataTodaysSummaryReportNew(param1);
        //        dailyReportService.GetDataDailyTransactionTopsheet(param1);
        //        var param = new { Office = SessionHelper.LoginUserOfficeID, Date = Date, Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID };
        //        //var TodaysSummarys = dailyReportService.GetDataTodaysSummaryReportNew(param);
        //        var TodaysSummarys = dailyReportService.GetDataDailyTransactionTopsheet(param);

        //        var reportParam = new Dictionary<string, object>();
        //        reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //        reportParam.Add("FromDate", Date);
        //        ReportHelper.PrintReport("rpt_DailyTransactionTopsheet.rpt", TodaysSummarys.Tables[0], reportParam);
        //        //ReportHelper.PrintReport("rptTodaysSummaryDesktop.rpt", TodaysSummarys.Tables[0], reportParam);

        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public ActionResult ProductWiseLoanBalance()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["MainProductList"] = items;
            return View();
        }


    } // END CLASS
} // END NAMESPACE
       #endregion