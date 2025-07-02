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
namespace gBanker.Web.Controllers
{
    public class PartialDisbursementController : BaseController
    {
        private readonly IBranchService branchService;
        private readonly IOfficeService officeService;
        private readonly ILoanSummaryService loansSummaryService;
        private readonly IProductService productService;
        private readonly IMemberCategoryService membercategoryService;
        private readonly ICenterService centerService;
        private readonly IPurposeService purposeService;
        private readonly IMemberService memberService;
        private readonly ILoanApprovalService partialService;
        private readonly IInvestorService investorService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly ILoanCollectionReportService loanCollectionReportService;
        private readonly IMemberPassBookRegisterService memberPassBookRegisterService;
        private readonly ILoanDisburseService disburseservice;
        private readonly ISpecialLoanCollectionService specialLoanCollectionService;
        private readonly IAccChartService accChartService;
        private readonly IApplicationSettingsService applicationSettingsService;
        public PartialDisbursementController(ILoanSummaryService loansSummaryService, IMemberPassBookRegisterService memberPassBookRegisterService, ILoanApprovalService partialService, IProductService productService, IMemberCategoryService membercategoryService, IOfficeService officeService, ICenterService centerService, IPurposeService purposeService, IMemberService memberService, IInvestorService investorService, IUltimateReportService ultimateReportService, ILoanCollectionReportService loanCollectionReportService, ILoanDisburseService disburseservice, ISpecialLoanCollectionService specialLoanCollectionService,IAccChartService accChartService,IApplicationSettingsService applicationSettingsService)
          {
              this.loansSummaryService = loansSummaryService;
              this.productService = productService;
              this.membercategoryService = membercategoryService;
              this.officeService = officeService;
              this.centerService = centerService;
              this.purposeService = purposeService;
              this.memberService = memberService;
              this.partialService = partialService;
              this.investorService = investorService;
              this.ultimateReportService = ultimateReportService;
              this.loanCollectionReportService = loanCollectionReportService;
              this.memberPassBookRegisterService = memberPassBookRegisterService;
              this.disburseservice = disburseservice;
              this.specialLoanCollectionService = specialLoanCollectionService;
              this.accChartService = accChartService;
              this.applicationSettingsService = applicationSettingsService;
          }
        //public JsonResult GetLoanApprovals(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        //{
        //    try
        //    {
        //        long totalCount;
        //        var allSavingsummary = loansSummaryService.GetLoanApproveDetailPaged(SessionHelper.LoginUserOfficeID.Value, filterColumn, filterValue, jtStartIndex, jtPageSize, jtSorting, out totalCount, TransactionDate, Convert.ToInt16(LoggedInOrganizationID));
        //        // var allloansummary = loansSummaryService.GetLoanApproveDetail(SessionHelper.LoginUserOfficeID.Value, filterColumn, filterValue).Where(a => a.ApproveDate == TransactionDate && (a.TransType == 101 || a.TransType == 102));
        //        //var totalCount = allloansummary.Count();
        //        //var entities = allloansummary.Skip(jtStartIndex).Take(jtPageSize);
        //        var currentPageRecords = Mapper.Map<IEnumerable<DBLoanApproveDetailModel>, IEnumerable<PartialLoanDisbursementViewModel>>(allSavingsummary);
        //        return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });
        //        //var entities = Mapper.Map<IEnumerable<DBLoanApproveDetailModel>, IEnumerable<PartialLoanDisbursementViewModel>>(allloansummary);
        //        //return Json(new { Result = "OK", Records = entities });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }

        //}

        public ActionResult GetLoanDisburseInfo(int jtStartIndex, int jtPageSize, string jtSorting, string filterCCLoan, string filterColumn, string filterValue)
        {
            try
            {
                if (IsDayInitiated)
                {
                    IEnumerable<Proc_get_LoanDisburse_Result> disburseDetail = new List<Proc_get_LoanDisburse_Result>();
                    if (LoggedInOrganizationID == 3)
                        disburseDetail = disburseservice.GetPartialLoanDisburseCCLoan(LoggedInOrganizationID, LoginUserOfficeID, TransactionDate,  filterColumn, filterValue, filterCCLoan);
                    else
                        disburseDetail = disburseservice.GetPartialLoanDisburse(LoggedInOrganizationID, LoginUserOfficeID, TransactionDate, filterColumn, filterValue);
                    var detail = disburseDetail.ToList();
                    var totalCount = detail.Count();
                    var entities = detail.Skip(jtStartIndex).Take(jtPageSize);
                    var currentPageRecords = Mapper.Map<IEnumerable<Proc_get_LoanDisburse_Result>, IEnumerable<LoanDisburseViewModel>>(entities);
                    return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });
                }
                return Json(new { Result = "OK", Records = new List<LoanDisburseViewModel>(), TotalRecordCount = 0 });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult GetLoanDetails(string MemberID)
        {
            List<PartialLoanDisbursementViewModel> List_LoanApprovalViewModel = new List<PartialLoanDisbursementViewModel>();
            var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID.Value, Member = Convert.ToInt64(MemberID) };
            var loanInfo = ultimateReportService.GetMembersLoanInformation(param);
            List_LoanApprovalViewModel = loanInfo.Tables[0].AsEnumerable()
                .Select(row => new PartialLoanDisbursementViewModel
                {
                    ProductCode = row.Field<string>("ProductCode"),
                    DisburseDate = row.Field<string>("DisburseDate"),
                    PrincipalLoan = row.Field<decimal>("PrincipalLoan"),//Convert.ToDecimal(string.IsNullOrEmpty(row.Field<string>("PrincipalLoan")) ? "0" : row.Field<string>("PrincipalLoan")),
                    LoanRepaid = row.Field<decimal>("LoanRepaid"), //Convert.ToDecimal(string.IsNullOrEmpty(row.Field<string>("LoanRepaid")) ? "0" : row.Field<string>("LoanRepaid")),
                    LoanCloseDate = row.Field<string>("LoanCloseDate"),
                    IntPaid = row.Field<decimal>("IntPaid") //Convert.ToDecimal(string.IsNullOrEmpty(row.Field<string>("IntPaid")) ? "0" : row.Field<string>("IntPaid"))
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
        private void MapDropDownList(PartialLoanDisbursementViewModel model)
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


            var allSearchProd = productService.SearchProduct(0, Convert.ToInt16(LoggedInOrganizationID), "L");
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
            paymentMode.Add(new SelectListItem() { Text = "Cash", Value = "101", Selected = true });
            paymentMode.Add(new SelectListItem() { Text = "Bank", Value = "102" });

            model.paymentMode = paymentMode;

            var disType = new List<SelectListItem>();
            disType.Add(new SelectListItem() { Text = "Once at a time", Value = "1", Selected = true });
            disType.Add(new SelectListItem() { Text = "Partial ", Value = "2" });


            model.disType = disType;

            var PDis = new List<SelectListItem>();
            PDis.Add(new SelectListItem() { Text = "No", Value = "0", Selected = true });
            PDis.Add(new SelectListItem() { Text = "Yes ", Value = "1" });


            model.PDis = PDis;

            //var allinvestor = investorService.SearchInvestor();

            // var viewInvestor = allinvestor.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.InvestorCode, m.InvestorName), Value = m.InvestorID.ToString() });

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

            var BankCode = accChartService.GetByAccCode(applicationSettingsService.GetAll().Where(c => c.OfficeID == LoginUserOfficeID).FirstOrDefault().BankAccount);

            var viewAccountCode = accChartService.GetAll().Where(s => s.SecondLevel == BankCode.SecondLevel && s.ModuleID == 1);
            var accLIst = viewAccountCode.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.AccCode.ToString(),
                Text = string.Format("{0} - {1}", x.AccCode.ToString(), x.AccName.ToString())
            });


           
            var Accitems = new List<SelectListItem>();
            Accitems.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            Accitems.AddRange(accLIst);
            model.AccListListItems = Accitems;


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
        public ActionResult GetInstallment(int productid, decimal principal)
        {

            decimal vLoanInstallment = 0;
            decimal vInterestInstallment = 0;
            decimal vtotal = 0;
            int vDuration;
            var pbr = productService.GetById(productid);
            vtotal = Convert.ToDecimal((pbr.LoanInstallment * principal)) + Convert.ToDecimal((pbr.InterestInstallment * principal));


            //vLoanInstallment = Math.Round(Convert.ToDecimal(pbr.LoanInstallment) * principal);
            vInterestInstallment = Math.Round(vtotal - Math.Round(Convert.ToDecimal((pbr.LoanInstallment * principal))));
            //vInterestInstallment = Math.Round(Convert.ToDecimal(pbr.InterestInstallment.ToString()) * principal);
            vLoanInstallment = Math.Round(vtotal - vInterestInstallment);
            vDuration = Convert.ToInt16(pbr.Duration);
            var result = new { loan = vLoanInstallment.ToString(), interest = vInterestInstallment.ToString(), duration = vDuration.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public MemberPassBookRegister GetMemPassBook(long mPassid)
        {
            var mps = memberPassBookRegisterService.GetByIdLong(mPassid);
            return mps;
        }
        
        //CCLoan Loaded by OrgID
        public ActionResult Index()
        {
            var orgID = LoggedInOrganizationID;   
            ViewBag.OrgID = orgID;
            return View();
        }

        //
        // GET: /PartialDisbursement/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /PartialDisbursement/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PartialDisbursement/Create
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
        // GET: /PartialDisbursement/Edit/5
        public ActionResult Edit(long id)
        {
            var loanapproval = partialService.GetByIdLong(id);

            var member = GetMember(Convert.ToInt64(loanapproval.MemberID));
            var mPassBook = GetMemPassBook(Convert.ToInt64(loanapproval.MemberPassBookRegisterID));
            var entity = Mapper.Map<LoanSummary, PartialLoanDisbursementViewModel>(loanapproval);
            var proid = productService.GetById(loanapproval.ProductID);
            decimal prin;
            decimal loanre;
            decimal bal;
            if (!loanapproval.InstallmentDate.HasValue)
            {
                entity.PartialIntCharge = 0;
                entity.PartialIntPaid = 0;
            }
            else
            {
                var vDD = (TransactionDate - loanapproval.InstallmentDate.Value).TotalDays;
                if (proid.InterestCalculationMethod == "C")
                {


                    entity.IntCharge = loanapproval.IntCharge;
                    prin = Math.Round(loanapproval.PrincipalLoan);
                    loanre = Math.Round(loanapproval.LoanRepaid);
                    bal = Convert.ToDecimal(prin) - Convert.ToDecimal(loanre);
                    entity.PartialIntCharge = Math.Round(bal * loanapproval.InterestRate * Convert.ToInt16(vDD) / 36500);
                    entity.PartialIntPaid = 0;
                }
                else
                {
                    if (proid.InterestCalculationMethod == "F")
                    {
                        entity.PartialIntCharge = 0;
                        entity.PartialIntPaid = 0;
                    }
                    else
                    {
                       
                        if (vDD == 0)
                        {
                            entity.PartialIntCharge = loanapproval.PartialIntCharge;
                            entity.PartialIntPaid = loanapproval.PartialIntPaid;
                        }
                        else
                        {


                            entity.PartialIntCharge = Math.Round(loanapproval.PrincipalLoan * loanapproval.InterestRate * Convert.ToInt16(vDD) / 36500);
                            entity.PartialIntPaid = Math.Round(loanapproval.PrincipalLoan * loanapproval.InterestRate * Convert.ToInt16(vDD) / 36500);

                        }
                    }
                }
               
               
            }
            entity.LoanInstallment = loanapproval.LoanInstallment;
            entity.IntInstallment = loanapproval.IntInstallment;
            entity.PartialAmount = loanapproval.PartialAmount;
            entity.BankName = loanapproval.BankName;
            entity.ChequeNo = loanapproval.ChequeNo;
            entity.ChequeIssueDate = loanapproval.ChequeIssueDate;

            entity.InterestCalMethod = proid.InterestCalculationMethod;

            ViewBag.MemberName = string.Format("{0} - {1}", member.MemberCode, member.FirstName + " " + member.MiddleName + " " + member.LastName);
            //ViewBag.memberPass = string.Format("{0}", mPassBook.MemberPassBookNO);
            MapDropDownList(entity);

            return View(entity);
        }
        public JsonResult GetAccCode()
        {

            try
            {


                var BankCode = accChartService.GetByAccCode(applicationSettingsService.GetAll().Where(c => c.OfficeID == LoginUserOfficeID).FirstOrDefault().BankAccount);

                var viewAccountCode = accChartService.GetAll().Where(s => s.SecondLevel == BankCode.SecondLevel && s.ModuleID == 1);
                var accLIst = viewAccountCode.Select(c => new { DisplayText = c.AccCode + " " + c.AccName, Value = c.AccCode }).OrderBy(s => s.DisplayText);
                return Json(new { Result = "OK", Options = accLIst });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //
        // POST: /PartialDisbursement/Edit/5
        [HttpPost]
        public ActionResult Edit(PartialLoanDisbursementViewModel model)
        {
            try
            {
                if (!IsDayInitiated)
                {
                    return GetErrorMessageResult("Please run the start work process");
                }
                specialLoanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);
                var entity = Mapper.Map<PartialLoanDisbursementViewModel, LoanSummary>(model);
                var getLoanSummary = partialService.GetByIdLong(Convert.ToInt64(model.LoanSummaryID));
                var proid = productService.GetById(getLoanSummary.ProductID);
                //getLoanSummary.IsApproved = true;
                getLoanSummary.LoanInstallment = model.LoanInstallment;
                getLoanSummary.IntInstallment = model.IntInstallment;
                getLoanSummary.CoApplicantName = entity.CoApplicantName;
                getLoanSummary.Guarantor = entity.Guarantor;
                getLoanSummary.PartialAmount = model.PartialAmount;

                getLoanSummary.PartialIntCharge = model.PartialIntCharge;

                model.InterestCalMethod = proid.InterestCalculationMethod;

                getLoanSummary.PartialIntPaid = model.PartialIntPaid;
                //getLoanSummary.IntCharge=Convert.ToDecimal(model.PartialIntCharge);

                getLoanSummary.BankName = model.BankName;
                getLoanSummary.ChequeNo = model.ChequeNo;
                getLoanSummary.ChequeIssueDate = model.ChequeIssueDate;
                if (!getLoanSummary.InstallmentDate.HasValue)
                {
                    if (proid.InterestCalculationMethod == "C")
                    {
                        if (!getLoanSummary.FirstInstallmentDate.HasValue)
                        {
                            getLoanSummary.FirstInstallmentDate = TransactionDate;
                        }
                    }
                    else
                        getLoanSummary.FirstInstallmentDate = TransactionDate;
                }
                else
                {
                    getLoanSummary.FirstInstallmentDate = getLoanSummary.FirstInstallmentDate;
                }


                if (model.FinalDisbursement == 1)
                {
                    if (proid.InterestCalculationMethod == "F")
                    {
                        getLoanSummary.PartialIntCharge = Math.Round(Convert.ToDecimal(((getLoanSummary.PrincipalLoan+model.PartialAmount) * proid.InterestRate) / 100), 0);
                    }

                    if (proid.InterestCalculationMethod == "C")
                    {
                        if (!getLoanSummary.FirstInstallmentDate.HasValue)
                            getLoanSummary.FirstInstallmentDate = TransactionDate;
                        else
                            getLoanSummary.FirstInstallmentDate = getLoanSummary.FirstInstallmentDate; ;

                    }
                    else
                        getLoanSummary.FirstInstallmentDate = TransactionDate;
                    //getLoanSummary.FirstInstallmentDate = TransactionDate;
                    // getLoanSummary.InstallmentStartDate = TransactionDate;
                    if (model.InstallmentStartDate == null)
                    {
                        return GetErrorMessageResult("Pls. Check InstallmentStartDate");

                    }
                    else
                    getLoanSummary.InstallmentStartDate = Convert.ToDateTime(model.InstallmentStartDate);
                }
                else
                    getLoanSummary.InstallmentStartDate = null;

                if (Convert.ToDecimal( getLoanSummary.PrincipalLoan)+Convert.ToDecimal( getLoanSummary.PartialAmount)== Convert.ToDecimal( getLoanSummary.ApprovedAmount))
                {
                    model.FinalDisbursement = 1;
                    if (!getLoanSummary.InstallmentDate.HasValue)
                    {
                        if (proid.InterestCalculationMethod == "C")
                        {
                            getLoanSummary.FirstInstallmentDate = getLoanSummary.FirstInstallmentDate;
                        }
                        else
                            getLoanSummary.FirstInstallmentDate = TransactionDate;
                        //getLoanSummary.FirstInstallmentDate = TransactionDate;
                    }
                   // getLoanSummary.FirstInstallmentDate = TransactionDate;

                    if (model.InstallmentStartDate == null)
                    {
                        return GetErrorMessageResult("Pls. Check InstallmentStartDate");

                    }
                    else
                        getLoanSummary.InstallmentStartDate = Convert.ToDateTime(model.InstallmentStartDate);
                   
                }
                else
                getLoanSummary.InstallmentStartDate = null;
                getLoanSummary.FinalDisbursement = model.FinalDisbursement;
                getLoanSummary.DisburseDate = TransactionDate;
                getLoanSummary.InstallmentDate = TransactionDate;
                //getLoanSummary.ChequeIssueDate = entity.ChequeIssueDate;
                getLoanSummary.MemberPassBookRegisterID = entity.MemberPassBookRegisterID;


                partialService.Update(getLoanSummary);

                return GetSuccessMessageResult();
                //return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /PartialDisbursement/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /PartialDisbursement/Delete/5
        [HttpPost]
        public ActionResult Delete(long LoanSummaryID)
        {
            try
            {
                specialLoanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);
                var loanser = loansSummaryService.GetByIdLong(Convert.ToInt64(LoanSummaryID));
                loanser.InstallmentStartDate = null;
                loanser.LoanStatus = 1;
                loanser.DisburseDate = null;
                if (loanser.Posted== false)
                {
                    loanser.IsApproved = false;

                }
              
                loanser.FinalDisbursement = 0;
                loansSummaryService.Update(loanser);

                //disburseservice.Delete(LoanSummaryID);
                return Json(new { Result = "OK" });
            }
            catch (Exception ex) 
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}
