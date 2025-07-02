using AutoMapper;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gBanker.Web.Core.Extensions;
using gBanker.Data.DBDetailModels;
using gBanker.Web.Helpers;
using gBanker.Web.Filters;

namespace gBanker.Web.Controllers
{
    public class LoanSummaryController : BaseController
    {
        #region Variables
        private readonly IOfficeService officeService;
        private readonly ILoanSummaryService loansSummaryService;
        private readonly IProductService productService;
        private readonly IMemberCategoryService membercategoryService;
        private readonly ICenterService centerService;
        private readonly IPurposeService purposeService;
        private readonly IMemberService memberService;
        private readonly IInvestorService investorService;
        public LoanSummaryController(ILoanSummaryService loansSummaryService, IProductService productService, IMemberCategoryService membercategoryService, IOfficeService officeService, ICenterService centerService, IPurposeService purposeService, IMemberService memberService, IInvestorService investorService)
        {
            this.loansSummaryService = loansSummaryService;
            this.productService = productService;
            this.membercategoryService = membercategoryService;
            this.officeService = officeService;
            this.centerService = centerService;
            this.purposeService = purposeService;
            this.memberService = memberService;
            this.investorService = investorService;
        }
        #endregion

        #region Methods
        public JsonResult GetListOfInstallment(int productid, decimal principal)
        {
            decimal vLoanInstallment = 0;
            decimal vInterestInstallment = 0;
            int vduration = 0;
            decimal vIntrate = 0;
            var pbr = productService.GetById(productid);
            vLoanInstallment = Math.Round(Convert.ToDecimal(pbr.LoanInstallment) * principal, 0);
            vInterestInstallment = Math.Round(Convert.ToDecimal(pbr.InterestInstallment) * principal, 0);
            vduration = Convert.ToInt16(pbr.Duration);
            vIntrate = Convert.ToDecimal(pbr.InterestRate);
            var result = new { loan = vLoanInstallment.ToString(), interest = vInterestInstallment.ToString(), duration = vduration.ToString(), intrestRate = vIntrate.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBalance(decimal principal, decimal intCharge, decimal loanRepaid, decimal intPaid)
        {
            decimal vBalance = (principal + intCharge) - (loanRepaid + intPaid);
            var result = new { balance = vBalance.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLoanSummary(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                long totalCount;
                var allloansummary = loansSummaryService.GetLoanSummaryDetailPaged(SessionHelper.LoginUserOfficeID.Value, filterColumn, filterValue, jtStartIndex, jtPageSize, out totalCount);

                // var totalCount = allloansummary.Count();
                // var entities = allloansummary.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<DBLoanApproveDetailModel>, IEnumerable<LoanSummaryViewModel>>(allloansummary);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });

                //var entities = Mapper.Map<IEnumerable<DBLoanApproveDetailModel>, IEnumerable<LoanSummaryViewModel>>(allloansummary);
                //return Json(new { Result = "OK", Records = entities });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        private void MapDropDownList(LoanSummaryViewModel model)
        {

            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }
            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID));

            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + '-' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName))), Value = m.MemberID.ToString() });

            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;

            var allpurpose = purposeService.SearchPurpose( Convert.ToInt16(LoggedInOrganizationID));

            var viewPurpose = allpurpose.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.PurposeCode, m.PurposeName), Value = m.PurposeID.ToString() });

            model.purposeListItems = viewPurpose;

            var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt16(LoggedInOrganizationID));

            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });

            model.centerListItems = viewCenter;

            var allbranch = officeService.GetAll().Where(l => l.OfficeID == SessionHelper.LoginUserOfficeID.Value);

            var viewbranch = allbranch.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.branchListItems = viewbranch;



            var allproduct = productService.SearchProduct(0, Convert.ToInt16(LoggedInOrganizationID),"L");

            var viewproduct = allproduct.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.ProductCode, m.ProductName), Value = m.ProductID.ToString() });

            model.productListItems = viewproduct;

            model.MemberProductItemsSelected = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(allproduct);



            var allInvestor = investorService.GetAll().Where(i => i.IsActive == true && i.OrgID==LoggedInOrganizationID);

            var viewInvestor = allInvestor.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.InvestorCode, m.InvestorName), Value = m.InvestorID.ToString() });

            model.investorListItems = viewInvestor;

            model.MemberProductItemsSelected = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(allproduct);


            var allmembercategory = membercategoryService.GetAll().Where(m => m.IsActive == true && m.OrgID==LoggedInOrganizationID);

            var viewmembercategory = allmembercategory.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCategoryID, m.CategoryName), Value = m.MemberCategoryID.ToString() });

            model.membercategoryListItems = viewmembercategory;

            var paymentMode = new List<SelectListItem>();
            paymentMode.Add(new SelectListItem() { Text = "Transfer", Value = "0", Selected = true });
            paymentMode.Add(new SelectListItem() { Text = "Cash", Value = "0" });
            model.paymentMode = paymentMode.AsEnumerable();
            // paymentMode.Add(new SelectListItem() { Text = "Bank", Value = "98" });


            var search = new List<SelectListItem>();
            search.Add(new SelectListItem() { Text = "CenterID", Value = "0", Selected = true });
            search.Add(new SelectListItem() { Text = "MemberID", Value = "1" });
            model.SearchOption = search.AsEnumerable();


        }
        public Member GetMember(Int64 memberid)
        {
            var mbr = memberService.GetByMemberId(memberid);
            return mbr;
        }
        public Center GetEmployee(int employeeid)
        {
            var mbr = centerService.GetById(employeeid);
            return mbr;
        }
        #endregion

        #region Events
        public ActionResult PosttoLedger(LoanSummaryViewModel model, FormCollection form)
        {
            var entity = Mapper.Map<LoanSummaryViewModel, LoanSummary>(model);
            if (ModelState.IsValid)
            {
                loansSummaryService.CreateLoanTrx(entity);
                //do something with account
                return RedirectToAction("Index");
            }
            return View("SignUp");
        }
        public ActionResult LedgerPost(LoanSummaryViewModel model)
        {
            var members = "Success";
            var val = loansSummaryService.SetOpeningLoanEntry(SessionHelper.LoginUserOfficeID.Value);
            return Json(members, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetMemberList(string memberid, string centerId)
        {
            var MemberByCenterSessionKey = string.Format("MemberByCenterSessionKey_{0}", centerId);
            var memberList = new List<Member>();
            if (Session[MemberByCenterSessionKey] != null)
                memberList = Session[MemberByCenterSessionKey] as List<Member>;
            else
            {
                var mbr = memberService.GetByCenterId(int.Parse(centerId), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID)).ToList();
                Session[MemberByCenterSessionKey] = mbr;
                memberList = mbr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + " " + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + " " + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + " " + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + " " + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

            return Json
                (members, JsonRequestBehavior.AllowGet);
        }
        // GET: LoanSummary
        public ActionResult Index()
        {
            var model = new LoanSummaryViewModel() { CurLoan = 0, PreLoan = 0, WriteOffInterest = 0, WriteOffLoan = 0, CumLoanDue = 0 };

            MapDropDownList(model);

            return View(model);
        }
        // GET: LoanSummary/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: LoanSummary/Create
        public ActionResult Create()
        {

            var model = new LoanSummaryViewModel() { CurLoan = 0, PreLoan = 0, WriteOffInterest = 0, WriteOffLoan = 0, CumLoanDue = 0 };
            if (IsDayInitiated)
            {
                model.ApproveDate = TransactionDate;
                model.InstallmentDate = TransactionDate;
                model.InstallmentStartDate = TransactionDate;
                model.LoanCloseDate = TransactionDate;
            }
            model.LoanStatus = 1;
            MapDropDownList(model);

            return View(model);
        }
        // POST: LoanSummary/Create
        [HttpPost]
        public ActionResult Create(LoanSummaryViewModel model, FormCollection form)
        {
            try
            {

                var entity = Mapper.Map<LoanSummaryViewModel, LoanSummary>(model);

                //Add Validlation Logic.

                if (ModelState.IsValid)
                {

                    string msg = "";

                    var member = GetMember(Convert.ToInt64(entity.MemberID));
                    int membercaregoryid = member.MemberCategoryID;

                    var employee = GetEmployee(entity.CenterID);
                    int employeeid = employee.EmployeeId;
                    entity.EmployeeId = Convert.ToInt16(employeeid);
                    entity.MemberCategoryID = Convert.ToByte(membercaregoryid);


                    entity.IsActive = true;

                    var errors = loansSummaryService.IsValidLoan(entity);

                    if (errors.ToList().Count == 0)
                    {
                        entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                        loansSummaryService.Create(entity);
                        return GetSuccessMessageResult();
                    }
                    else
                        return GetErrorMessageResult(errors);
                }
                return GetErrorMessageResult();

            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        // GET: LoanSummary/Edit/5
        public ActionResult Edit(int id)
        {

            if (loansSummaryService.IsContinued(id))
            {
                var loansummary = loansSummaryService.GetById(id);

                var member = GetMember(Convert.ToInt64(loansummary.MemberID));
                var entity = Mapper.Map<LoanSummary, LoanSummaryViewModel>(loansummary);
                ViewBag.MemberName = string.Format("{0} - {1}", member.MemberCode, member.FirstName);
                MapDropDownList(entity);

                return View(entity);
            }
            else
                ModelState.AddModelError("Validation", "Discontinued ID, please enter a diferent id and name.");
            return RedirectToAction("Index");

        }
        // POST: LoanSummary/Edit/5
        [HttpPost]
        public ActionResult Edit(LoanSummaryViewModel model)
        {
            try
            {

                var entity = Mapper.Map<LoanSummaryViewModel, LoanSummary>(model);

                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    var getLoanSummary = loansSummaryService.GetByIdLong(Convert.ToInt64(entity.LoanSummaryID));
                    var errors = loansSummaryService.IsValidLoanForEdit(entity);
                    if (errors.ToList().Count == 0)
                    {
                        getLoanSummary.Advance = entity.Advance;
                        getLoanSummary.ApproveDate = entity.ApproveDate;
                        getLoanSummary.Balance = entity.Balance;
                        getLoanSummary.ContinuousDrop = entity.ContinuousDrop;
                        getLoanSummary.CumLoanDue = entity.CumLoanDue;
                        getLoanSummary.CurLoan = entity.CurLoan;
                        getLoanSummary.DisburseDate = entity.DisburseDate;
                        getLoanSummary.DropInstallment = entity.DropInstallment;
                        getLoanSummary.DueRecovery = entity.DueRecovery;
                        getLoanSummary.Duration = entity.Duration;
                        getLoanSummary.Holidays = entity.Holidays;
                        getLoanSummary.InstallmentDate = entity.InstallmentDate;
                        getLoanSummary.InstallmentNo = entity.InstallmentNo;
                        getLoanSummary.InstallmentStartDate = entity.InstallmentStartDate;
                        getLoanSummary.IntCharge = entity.IntCharge;
                        getLoanSummary.InterestRate = entity.InterestRate;
                        getLoanSummary.IntInstallment = entity.IntInstallment;
                        getLoanSummary.IntPaid = entity.IntPaid;
                        getLoanSummary.LoanCloseDate = entity.LoanCloseDate;
                        getLoanSummary.LoanInstallment = entity.LoanInstallment;
                        getLoanSummary.LoanRepaid = entity.LoanRepaid;
                        getLoanSummary.LoanStatus = entity.LoanStatus;
                        getLoanSummary.LoanTerm = entity.LoanTerm;
                        getLoanSummary.OverdueDate = entity.OverdueDate;
                        getLoanSummary.PreLoan = entity.PreLoan;
                        getLoanSummary.PrincipalLoan = entity.PrincipalLoan;
                        getLoanSummary.ProductID = entity.ProductID;
                        getLoanSummary.PurposeID = entity.PurposeID;
                        getLoanSummary.TransType = entity.TransType;
                        getLoanSummary.WriteOffInterest = entity.WriteOffInterest;
                        getLoanSummary.WriteOffLoan = entity.WriteOffLoan;

                        getLoanSummary.IsActive = true;

                        loansSummaryService.Update(getLoanSummary);
                        return GetSuccessMessageResult();

                    }
                    else
                        return GetErrorMessageResult(errors);
                }
                return GetErrorMessageResult();

            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        //[HttpParamAction]
        //[AcceptVerbs(HttpVerbs.Post)]

        // GET: LoanSummary/Delete/5
        public ActionResult Delete(int id)
        {
            loansSummaryService.Inactivate(id, null);
            return RedirectToAction("Index");

        }
        // POST: LoanSummary/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                loansSummaryService.Inactivate(id, null);
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
