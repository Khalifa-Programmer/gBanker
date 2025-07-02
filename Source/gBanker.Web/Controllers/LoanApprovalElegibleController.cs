using AutoMapper;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
using gBanker.Service;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gBanker.Web.Core.Extensions;
using gBanker.Web.Helpers;
using gBanker.Service.ReportServies;
using System.Data;
using gBanker.Data.CodeFirstMigration;
using gBanker.Service.SMSSenderServices.Models;
using gBanker.Core.Utility;

namespace gBanker.Web.Controllers
{
    public class LoanApprovalElegibleController : BaseController
    {
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
        private readonly IUltimateReportService ultimateReportService;
        private readonly ILoanCollectionReportService loanCollectionReportService;
        private readonly IMemberPassBookRegisterService memberPassBookRegisterService;
        private readonly IApproveCellingService approveCellingService;
        private readonly ISMSSenderService smsSenderService;
        private readonly IGroupwiseReportService groupwiseReportService;
        public LoanApprovalElegibleController(ILoanSummaryService loansSummaryService,
            IApproveCellingService approveCellingService,
            IMemberPassBookRegisterService memberPassBookRegisterService,
            ILoanApprovalService loanapprovalService, IProductService productService,
            IMemberCategoryService membercategoryService, IOfficeService officeService,
            ICenterService centerService, IPurposeService purposeService,
            IMemberService memberService, IInvestorService investorService,
            IUltimateReportService ultimateReportService
            , ILoanCollectionReportService loanCollectionReportService
            , ISMSSenderService smsSenderService,
            IGroupwiseReportService groupwiseReportService
            )
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
            this.ultimateReportService = ultimateReportService;
            this.loanCollectionReportService = loanCollectionReportService;
            this.memberPassBookRegisterService = memberPassBookRegisterService;
            this.approveCellingService = approveCellingService;
            this.smsSenderService = smsSenderService;
            this.groupwiseReportService = groupwiseReportService;
        }
        public JsonResult GetLoanApprovals(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                long totalCount;
                List<LoanProposalListViewModel> List_LoanApprovalViewModel = new List<LoanProposalListViewModel>();
                var param = new { OfficeID = SessionHelper.LoginUserOfficeID.Value, OrgID = SessionHelper.LoginUserOrganizationID, filterColumnName = filterColumn, filterValue = filterValue, UserID = LoggedInEmployeeID };
                var allSavingsummary = ultimateReportService.getLoanProposalListRoleWise(param);
                if (allSavingsummary.Tables[0].Rows.Count > 0)
                {
                    List_LoanApprovalViewModel = allSavingsummary.Tables[0].AsEnumerable()
               .Select(row => new LoanProposalListViewModel
               {
                   LoanSummaryID = row.Field<long>("LoanSummaryID"),
                   OfficeID = row.Field<int>("OfficeID"),
                   MemberID = row.Field<long>("MemberID"),
                   CenterID = row.Field<int>("CenterID"),
                   LoanTerm = row.Field<int>("LoanTerm"),
                   LoanNo = row.Field<string>("LoanNo"),
                   OfficeCode = row.Field<string>("OfficeCode"),
                   CenterCode = row.Field<string>("CenterCode"),
                   MemberCode = row.Field<string>("MemberCode"),
                   RoleId = row.Field<int>("RoleId"),
                   ProductCode = row.Field<string>("ProductCode"),
                   PurposeCode = row.Field<string>("PurposeCode"),
                   PrincipalLoan = row.Field<decimal>("PrincipalLoan"),
                   ApproveDate = row.Field<DateTime>("ApproveDate"),
                   LoanInstallment = row.Field<decimal>("LoanInstallment"),
                   IntInstallment = row.Field<decimal>("IntInstallment"),
                   InterestRate = row.Field<decimal>("InterestRate"),

               }).ToList();
                    var currentPageRecords = List_LoanApprovalViewModel;
                    return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = allSavingsummary.Tables[0].Rows.Count });
                }

                else
                {
                    var currentPageRecords = 1;
                    return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = 1 });
                }


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        //========================================= Loan Eligible Approval for GUK : Mostak ================================//
        public ActionResult IndexGUK()
        {
            return View();
        }
        public Purpose GetPurpose(int purposeid)
        {
            var pbr = purposeService.GetById(purposeid);
            return pbr;
        }
        public ActionResult EditGUK(long id)
        {
            var loanapproval = loanapprovalService.GetByIdLong(id);

            var member = GetMember(Convert.ToInt64(loanapproval.MemberID));
            var mPassBook = GetMemPassBook(Convert.ToInt64(loanapproval.MemberPassBookRegisterID));
            var entity = Mapper.Map<LoanSummary, LoanApprovalEligibleViewModel>(loanapproval);
            ViewBag.MemberName = string.Format("{0} - {1}", member.MemberCode, member.FirstName + " " + member.MiddleName + " " + member.LastName);
            //ViewBag.memberPass = string.Format("{0}", mPassBook.MemberPassBookNO);
            var purpose = GetPurpose(Convert.ToInt32(loanapproval.PurposeID));
            ViewBag.purposeList = string.Format("{0} - {1}", purpose.PurposeCode, purpose.PurposeName);

            var prod = GetProduct(loanapproval.ProductID);
            //ViewBag.frequencyMode = prod.PaymentFrequency;
            entity.frequencyMode = prod.PaymentFrequency;

            MapDropDownList(entity);

            return View(entity);
        }


        // POST: LoanApprovalElegible/EditGUK/5
        [HttpPost]
        public ActionResult EditGUK(LoanApprovalEligibleViewModel model)
        {
            try
            {
                if (!IsDayInitiated)
                {
                    return GetErrorMessageResult("Please run the start work process");
                }
                var entity = Mapper.Map<LoanApprovalEligibleViewModel, LoanSummary>(model);

                var mProdCode = productService.GetAll().Where(p => p.ProductID == entity.ProductID).FirstOrDefault();
                var mProductCode = mProdCode.MainProductCode;

                var getLoanSummary = loanapprovalService.GetByIdLong(Convert.ToInt64(model.LoanSummaryID));
                getLoanSummary.IsApproved = true;
                getLoanSummary.CoApplicantName = entity.CoApplicantName;
                if (LoggedInOrganizationID == 12)
                {
                    getLoanSummary.Guarantor = SessionHelper.LoggedInEmployeeID.ToString();
                }
                else
                {
                    getLoanSummary.Guarantor = entity.Guarantor;
                }

                getLoanSummary.MemberPassBookRegisterID = entity.MemberPassBookRegisterID;
                getLoanSummary.FinalDisbursement = 0;
                getLoanSummary.PartialAmount = 0;
                getLoanSummary.PartialIntCharge = 0;
                getLoanSummary.PartialIntPaid = 0;
                if (entity.PrincipalLoan > getLoanSummary.ApprovedAmount)
                {
                    return GetErrorMessageResult("Disburse Amount can not be greater than ApprovedAmount");
                }

                int roleID = SessionHelper.LoginUserRoleId;
                var approveCellingInfo = approveCellingService.GetApproveCellingbyroleAndproductId(roleID, entity.ProductID);

                if (approveCellingInfo != null && (approveCellingInfo.MinRange > model.ApprovedAmount || approveCellingInfo.MaxRange < model.ApprovedAmount))
                    return GetErrorMessageResult($"The given Approved Ammount must be in {approveCellingInfo.MinRange} to {approveCellingInfo.MaxRange}");

                getLoanSummary.LoanInstallment = entity.LoanInstallment;
                getLoanSummary.IntInstallment = entity.IntInstallment;
                getLoanSummary.PrincipalLoan = entity.PrincipalLoan;

                if (getLoanSummary.DisbursementType == 2)
                {
                    getLoanSummary.PrincipalLoan = 0;
                    getLoanSummary.LoanInstallment = 0;
                    getLoanSummary.IntInstallment = 0;
                }



                //Validate Check
                var errors = new { OfficeID = LoginUserOfficeID, MemberID = getLoanSummary.MemberID, CenterID = getLoanSummary.CenterID, MainProductCode = mProductCode, ProductId = getLoanSummary.ProductID, PrincipalLoan = getLoanSummary.PrincipalLoan, Duration = getLoanSummary.Duration };
                var div_items = ultimateReportService.validateLOanProposal(errors);
                if (div_items.Tables[0].Rows.Count > 0)
                {
                    string vErr = div_items.Tables[0].Rows[0]["ErrorCode"].ToString();
                    if (vErr == "1")
                    {
                        if (entity.PurposeID == 0)
                        {
                            return GetErrorMessageResult("Please put the Purpose");
                        }

                        if (entity.Duration == 0)
                        {
                            return GetErrorMessageResult("Please put the Duration");
                        }

                        entity.ApprovedAmount = model.ApprovedAmount;
                        if (LoggedInOrganizationID == 54 || LoggedInOrganizationID == 6)
                        {
                            var LoanAccount = new { Qtype = 1, OfficeID = LoginUserOfficeID, MemberID = entity.MemberID, ProductID = entity.ProductID, loanTerm = entity.LoanTerm };
                            var Loan_items = ultimateReportService.GenerateLoanAndSavingAccount(LoanAccount);
                            if (Loan_items.Tables[0].Rows.Count > 0)
                            {
                                entity.LoanAccountNo = Loan_items.Tables[0].Rows[0][0].ToString();
                            }
                        }

                        loanapprovalService.Update(getLoanSummary);

                        return GetSuccessMessageResult();
                    }
                    else
                    {
                        return GetErrorMessageResult(div_items.Tables[0].Rows[0]["ErrorName"].ToString());
                    }
                }
                else
                    return GetErrorMessageResult();

            }
            catch
            {
                return View();
            }
        }

        //========================================= Loan Eligible Approval for GUK : END ================================//

        public JsonResult GetLoanDetails(string MemberID)
        {
            List<LoanApprovalEligibleViewModel> List_LoanApprovalViewModel = new List<LoanApprovalEligibleViewModel>();
            var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID.Value, Member = Convert.ToInt64(MemberID) };
            var loanInfo = ultimateReportService.GetMembersLoanInformation(param);
            List_LoanApprovalViewModel = loanInfo.Tables[0].AsEnumerable()
                .Select(row => new LoanApprovalEligibleViewModel
                {
                    ProductCode = row.Field<string>("ProductCode"),
                    DisburseDate = row.Field<string>("DisburseDate"),
                    PrincipalLoan = row.Field<decimal>("PrincipalLoan"),//Convert.ToDecimal(string.IsNullOrEmpty(row.Field<string>("PrincipalLoan")) ? "0" : row.Field<string>("PrincipalLoan")),
                    LoanRepaid = row.Field<decimal>("LoanRepaid"), //Convert.ToDecimal(string.IsNullOrEmpty(row.Field<string>("LoanRepaid")) ? "0" : row.Field<string>("LoanRepaid")),
                    LoanCloseDate = row.Field<string>("LoanCloseDate"),
                    IntPaid = row.Field<decimal>("IntPaid"),//Convert.ToDecimal(string.IsNullOrEmpty(row.Field<string>("IntPaid")) ? "0" : row.Field<string>("IntPaid"))
                    InstallmentNo = row.Field<int>("InstallmentNo")
                }).ToList();
            return Json(List_LoanApprovalViewModel, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetRepaymentSchedule(string MemberID, string ProductID)
        {
            List<RepaymentScheduleViewModel> List_LoanRepaymentScheduleModel = new List<RepaymentScheduleViewModel>();
            //MemberID = "7485";
            //ProductID = "69";
            var param = new { officeId = SessionHelper.LoginUserOfficeID.Value, MemberID = Convert.ToInt64(MemberID), ProductId = Convert.ToInt16(ProductID) };
            var loanInfo = ultimateReportService.GetRepaymentSchedule(param);
            List_LoanRepaymentScheduleModel = loanInfo.Tables[0].AsEnumerable()
                .Select(row => new RepaymentScheduleViewModel
                {
                    CenterCode = row.Field<string>("CenterCode"),
                    LoanTerm = row.Field<int>("LoanTerm"),
                    RepaymentDate = row.Field<string>("RepaymentDate"),
                    InstallmentNo = row.Field<int>("InstallmentNo"),
                    PrincipalLoan = row.Field<decimal>("PrincipalLoan"),//Convert.ToDecimal(string.IsNullOrEmpty(row.Field<string>("PrincipalLoan")) ? "0" : row.Field<string>("PrincipalLoan")),
                    LoanInstallment = row.Field<decimal>("LoanInstallment"), //Convert.ToDecimal(string.IsNullOrEmpty(row.Field<string>("LoanRepaid")) ? "0" : row.Field<string>("LoanRepaid")),
                    LoanBalnce = row.Field<decimal>("LoanBalnce"),
                    IntInstallment = row.Field<decimal>("IntPaid"),
                    IntCharge = row.Field<decimal>("IntCharge"),
                    InterestBalance = row.Field<decimal>("InterestBalance")//Convert.ToDecimal(string.IsNullOrEmpty(row.Field<string>("IntPaid")) ? "0" : row.Field<string>("IntPaid"))
                }).ToList();
            return Json(List_LoanRepaymentScheduleModel, JsonRequestBehavior.AllowGet);
        }
        private void MapDropDownList(LoanApprovalEligibleViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }
            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID));

            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)), Value = m.MemberID.ToString() });

            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;

            var allpurpose = purposeService.SearchPurpose(Convert.ToInt16(LoggedInOrganizationID));

            var viewPurpose = allpurpose.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.PurposeCode, m.PurposeName), Value = m.PurposeID.ToString() });

            model.purposeListItems = viewPurpose;

            var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt16(LoggedInOrganizationID)); ;

            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });

            model.centerListItems = viewCenter;

            var alloffice = officeService.GetAll().Where(l => l.OfficeID == SessionHelper.LoginUserOfficeID.Value && l.OrgID == LoggedInOrganizationID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;


            // var allSearchProd = productService.SearchProduct(0, Convert.ToInt16(LoggedInOrganizationID),"L");
            var allSearchProd = productService.SearchProductForLoanEligible(0, Convert.ToInt16(LoggedInOrganizationID), "L", model.ProductID);
            var viewProdList = allSearchProd.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = string.Format("{0} - {1}", x.ProductCode.ToString(), x.ProductName.ToString())
            });
            var proditems = new List<SelectListItem>();
            proditems.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            proditems.AddRange(viewProdList);
            model.productListItems = proditems;

            //model.MemberProductItemsSelected = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(allproduct);

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

            //paymentMode.Add(new SelectListItem() { Text = "Cash", Value = "101", Selected = true });
            //paymentMode.Add(new SelectListItem() { Text = "Bank", Value = "102" });

            model.paymentMode = paymentMode;

            var disType = new List<SelectListItem>();
            disType.Add(new SelectListItem() { Text = "Once at a time", Value = "1", Selected = true });
            disType.Add(new SelectListItem() { Text = "Partial ", Value = "2" });


            model.disType = disType;


            var mempassBook = memberPassBookRegisterService.GetAll()
                    .Where(i => i.IsActive == true && i.OrgID == LoggedInOrganizationID && i.MemberPassBookRegisterID == model.MemberPassBookRegisterID)
                    .OrderBy(i => i.MemberPassBookNO);
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


            //List<MemberPassBookRegister> List_PassbookRegisterViewModel = new List<MemberPassBookRegister>();

            //var paramPass = new { LoggedInOrganizationID = LoggedInOrganizationID, OfficeId = LoginUserOfficeID };
            //var PassbookInfo = groupwiseReportService.GetDataDataseAccess(paramPass, "GetPassbook"); // (@LoggedInOrganizationID int, @OfficeId int )

            //List_PassbookRegisterViewModel = PassbookInfo.Tables[0].AsEnumerable()
            //.Select(row => new MemberPassBookRegister
            //{
            //    MemberPassBookRegisterID = row.Field<Int64>("MemberPassBookRegisterID"),
            //    MemberPassBookNO = row.Field<Int64>("MemberPassBookNO")

            //}).ToList();

            //var viewmempassBook = List_PassbookRegisterViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.MemberPassBookRegisterID.ToString(),
            //    Text = string.Format("{0} ", x.MemberPassBookNO.ToString())
            //});
            //var mempassBookitems = new List<SelectListItem>();
            //mempassBookitems.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //mempassBookitems.AddRange(viewmempassBook);
            //model.MemberPassBookNOListItems = mempassBookitems;
            //ViewData["memberPass"] = viewmempassBook;

            //------------------------------------------------------------------

            //var allinvestor = investorService.SearchInvestor();

            // var viewInvestor = allinvestor.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.InvestorCode, m.InvestorName), Value = m.InvestorID.ToString() });

            //var mempassBook = memberPassBookRegisterService.GetAll().Where(i => i.IsActive == true && i.OrgID == LoggedInOrganizationID).OrderBy(i => i.MemberPassBookNO);
            //var viewmempassBook = mempassBook.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.MemberPassBookRegisterID.ToString(),
            //    Text = string.Format("{0} ", x.MemberPassBookNO.ToString())
            //});
            //var mempassBookitems = new List<SelectListItem>();
            //mempassBookitems.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //mempassBookitems.AddRange(viewmempassBook);
            //model.MemberPassBookNOListItems = mempassBookitems;
            //ViewData["memberPass"] = viewmempassBook;
        }
        public ActionResult GenerateReport(string MemberID)
        {
            var param = new { OfficeId = LoginUserOfficeID, MemberID = MemberID };
            var allproducts = loanCollectionReportService.GetRepaymentInfo(param);
            var reportParam = new Dictionary<string, object>();
            reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
            ReportHelper.PrintReport("RepaymentSchedule.rpt", allproducts.Tables[0], reportParam);
            return Content(string.Empty);
        }
        public Member GetMember(long memberid)
        {
            var mbr = memberService.GetByMemberId(memberid);
            return mbr;
        }
        public Center GetEmployee(int employeeid)
        {
            var mbr = centerService.GetById(employeeid);
            return mbr;
        }
        public Product GetProduct(int productid)
        {
            var mbr = productService.GetById(productid);
            return mbr;
        }
        // GET: LoanApprovalElegible
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetMemberPassBookList(string Member_id)
        {
            List<MemberPassBookRegisterViewModel> List_MemberPassBookRegisterViewModel = new List<MemberPassBookRegisterViewModel>();
            var param = new { OfficeId = LoginUserOfficeID, MemberId = Member_id };
            var div_items = ultimateReportService.GetMemberPasBookList(param);

            List_MemberPassBookRegisterViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new MemberPassBookRegisterViewModel
            {
                MemberPassBookRegisterID = row.Field<long>("MemberPassBookRegisterID"),
                MemberPassBookNO = row.Field<long>("MemberPassBookNO")

            }).ToList();

            var viewProduct = List_MemberPassBookRegisterViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.MemberPassBookRegisterID.ToString(),
                Text = x.MemberPassBookNO.ToString()
            });
            var d_items = new List<SelectListItem>();
            d_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            d_items.AddRange(viewProduct);
            return Json(d_items, JsonRequestBehavior.AllowGet);
        }
        public MemberPassBookRegister GetMemPassBook(long mPassid)
        {
            var mps = memberPassBookRegisterService.GetByIdLong(mPassid);
            return mps;
        }
        // GET: LoanApprovalElegible/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LoanApprovalElegible/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoanApprovalElegible/Create
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

        // GET: LoanApprovalElegible/Edit/5
        public ActionResult Edit(long id)
        {
            var loanapproval = loanapprovalService.GetByIdLong(id);

            var member = GetMember(Convert.ToInt64(loanapproval.MemberID));
            var mPassBook = GetMemPassBook(Convert.ToInt64(loanapproval.MemberPassBookRegisterID));
            var entity = Mapper.Map<LoanSummary, LoanApprovalEligibleViewModel>(loanapproval);
            ViewBag.MemberName = string.Format("{0} - {1}", member.MemberCode, member.FirstName + " " + member.MiddleName + " " + member.LastName);
            //ViewBag.memberPass = string.Format("{0}", mPassBook.MemberPassBookNO);
            MapDropDownList(entity);

            return View(entity);
        }

        // POST: LoanApprovalElegible/Edit/5
        [HttpPost]
        public ActionResult Edit(LoanApprovalEligibleViewModel model)
        {
            try
            {
                if (!IsDayInitiated)
                {
                    return GetErrorMessageResult("Please run the start work process");
                }
                var entity = Mapper.Map<LoanApprovalEligibleViewModel, LoanSummary>(model);
                var getLoanSummary = loanapprovalService.GetByIdLong(Convert.ToInt64(model.LoanSummaryID));
                getLoanSummary.IsApproved = true;
                getLoanSummary.CoApplicantName = entity.CoApplicantName;
                //getLoanSummary.Guarantor = entity.Guarantor;
                if (LoggedInOrganizationID == 12)
                {
                    getLoanSummary.Guarantor = SessionHelper.LoggedInEmployeeID.ToString();
                }
                else
                {
                    getLoanSummary.Guarantor = entity.Guarantor;
                }
                getLoanSummary.MemberPassBookRegisterID = entity.MemberPassBookRegisterID;
                getLoanSummary.FinalDisbursement = 0;
                getLoanSummary.PartialAmount = 0;
                getLoanSummary.PartialIntCharge = 0;
                getLoanSummary.PartialIntPaid = 0;
                if (entity.PrincipalLoan > getLoanSummary.ApprovedAmount)
                {
                    return GetErrorMessageResult("Disburse Amount can not be greater than ApprovedAmount");
                }

                int roleID = SessionHelper.LoginUserRoleId;
                var approveCellingInfo = approveCellingService.GetApproveCellingbyroleAndproductId(roleID, entity.ProductID);

                if (approveCellingInfo != null && (approveCellingInfo.MinRange > model.ApprovedAmount || approveCellingInfo.MaxRange < model.ApprovedAmount))
                    return GetErrorMessageResult($"The given Approved Ammount must be in {approveCellingInfo.MinRange} to {approveCellingInfo.MaxRange}");

                getLoanSummary.LoanInstallment = entity.LoanInstallment;
                getLoanSummary.IntInstallment = entity.IntInstallment;
                getLoanSummary.PrincipalLoan = entity.PrincipalLoan;

                if (getLoanSummary.DisbursementType == 2)
                {
                    getLoanSummary.PrincipalLoan = 0;
                    getLoanSummary.LoanInstallment = 0;
                    getLoanSummary.IntInstallment = 0;
                }
                loanapprovalService.Update(getLoanSummary);

                //if (SessionHelper.LoginUserOrganizationID == 10)
                //{

                //    var memberInfo = memberService.GetByMemberId(model.MemberID);

                //    //execute sms
                //    string MessageDetails ="Bastob: "+ memberInfo.FirstName + " আপনার নামে " + model.PrincipalLoan + " টাকা ঋণ অনুমোদন করা হয়েছে। আপনি শাখার সাথে যোগাযোগ করুন। ধন্যবাদ";
                //    var smsNotSentTo = "";
                //    string result = "Message Sent Successfully";

                //    SMSViewModel itemSMS = new SMSViewModel();

                //    itemSMS.OfficeId = (int)SessionHelper.LoginUserOfficeID;
                //    itemSMS.SummaryId = getLoanSummary.LoanSummaryID;
                //    itemSMS.RecordId = 0;
                //    itemSMS.MessageDetails = MessageDetails ;
                //    itemSMS.Length = itemSMS.MessageDetails.Length;
                //    itemSMS.PhoneNo = memberInfo.PhoneNo;


                //    var response = ExecuteSendSMS(itemSMS);
                //    if (response.IsError) smsNotSentTo += $"{itemSMS.PhoneNo},";

                //    if (!response.IsError)
                //    {
                //        var param = new { @RecordId = itemSMS.RecordId };

                //        //update SMSLOGDetail to SET SendSMS = 0 and SentToAPI=1 SentOK=1
                //        ultimateReportService.GetSMSDataWithParameter(param, "UpdateSMS");
                //    }

                //    var bulkResponseMessage = string.IsNullOrWhiteSpace(smsNotSentTo)
                //                        ? $@"{result}" : $@"SMS Send Partially Successful, But Not Sent Mobile Number(s) : {smsNotSentTo.TrimEnd(',')}";


                //}// END Organaization id 10 sms send process

                return GetSuccessMessageResult();
            }
            catch
            {
                return View();
            }
        }

        #region Execute Send SMS
        public SMSSendResponse ExecuteSendSMS(SMSViewModel model)
        {
            var response = new SMSSendResponse();
            try
            {
                var request = new SMSSendRequest
                {
                    RecipientMobile = model.PhoneNo,
                    Message = model.MessageDetails,
                    RequestType = SMSSendRequestTypeConstants.SINGLE_SMS,
                    MessageType = SMSMessageTypeConstants.UNICODE,
                    Organization = SessionHelper.LoggedInOrganizationCode
                };

                //let's send sms
                response = smsSenderService.SendSMS(request);

                if (response.IsError)
                {
                    //Let's insert into SMS_SentLog for NOT SENT
                    var param1 = new { @GrettingsText = model.MessageDetails, @PhoneNo = model.PhoneNo, @SMSType = "Grettings", @OfficeId = model.OfficeId, @OrgId = (int)SessionHelper.LoginUserOrganizationID };
                    ultimateReportService.GetDataWithParameter(param1, "Insert_SMS_FAILEDLogSMSLOGDetail");

                    return response;
                }

                var param = new
                {
                    @OfficeID = model.OfficeId,
                    @OrgId = (int)SessionHelper.LoginUserOrganizationID,
                    @SummaryId = model.SummaryId,
                    @DateSent = DateTime.Now,
                    @SMSType = 1,
                    @SMSPrice = 0.0,
                    @RecordId = model.RecordId,
                    @SMSDETAIL = model.MessageDetails,
                    @Length = model.Length,
                    @SMSCount = model.SMSCount
                };

                //let's insert into [SMS_SentLog]
                ultimateReportService.GetSMSDataWithParameter(param, "Insert_SMS_SentLog");
                return response;
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.Message = "There was an error on sending SMS or check your internet connection!";
            }

            return response;
        }
        #endregion

        // GET: LoanApprovalElegible/Delete/5
        public ActionResult Delete(int id)
        {
            loanapprovalService.Inactivate(id, null);
            return RedirectToAction("Index");
        }

        //public ActionResult RejectApprovals(long LoanSummaryID)
        public ActionResult RejectApprovals(long LoanSummaryID, string Remarks)
        {
            var members = "Success";
            var getLoanSummary = loanapprovalService.GetByIdLong(LoanSummaryID);
            // var entity = Mapper.Map<LoanApprovalEligibleViewModel, LoanSummary>(model);
            getLoanSummary.IsActive = false;
            getLoanSummary.InActiveDate = DateTime.Now;
            getLoanSummary.Remarks = Remarks;
            loanapprovalService.Update(getLoanSummary);
            //loanapprovalService.Inactivate(LoanSummaryID, null);
            return Json(members, JsonRequestBehavior.AllowGet);

        }
        // POST: LoanApprovalElegible/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, LoanApprovalEligibleViewModel model)
        {
            try
            {
                if (!IsDayInitiated)
                {
                    return GetErrorMessageResult("Please run the start work process");
                }

                // TODO: Add delete logic here

                loanapprovalService.Inactivate(id, null);
                // TODO: Add delete logic here
                // UpdateMethod(id, System.DateTime.Now);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
