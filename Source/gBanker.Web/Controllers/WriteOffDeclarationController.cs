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
using System.Data;
using gBanker.Service.ReportServies;
namespace gBanker.Web.Controllers
{
    public class WriteOffDeclarationController : BaseController
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
          private readonly ILoanTrxService loantrxService;
          private readonly IUltimateReportService ultimateReportService;
          private readonly ISpecialLoanCollectionService specialLoanCollectionService;
          public WriteOffDeclarationController(ILoanSummaryService loansSummaryService, IProductService productService, IMemberCategoryService membercategoryService, IOfficeService officeService, ICenterService centerService, IPurposeService purposeService, IMemberService memberService, IInvestorService investorService, ILoanTrxService loantrxService,IUltimateReportService ultimateReportService,ISpecialLoanCollectionService specialLoanCollectionService)
          {
              this.loansSummaryService = loansSummaryService;
              this.productService = productService;
              this.membercategoryService = membercategoryService;
              this.officeService = officeService;
              this.centerService = centerService;
              this.purposeService = purposeService;
              this.memberService = memberService;
              this.investorService = investorService;
              this.loantrxService = loantrxService;
              this.ultimateReportService = ultimateReportService;
              this.specialLoanCollectionService = specialLoanCollectionService;
          }
        #endregion
        private void MapDropDownList(WriteOffDeclarationViewModel model)
          {

              if (!SessionHelper.LoginUserOfficeID.HasValue)
              {
                  RedirectToAction("Login", "Account");
                  return;
              }
              var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID));
              var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + '-' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName))), Value = m.MemberID.ToString() });
              model.memberListItems = viewMember;
              ViewData["Member"] = viewMember;
              var allpurpose = purposeService.SearchPurpose(Convert.ToInt16(LoggedInOrganizationID));
              var viewPurpose = allpurpose.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.PurposeCode, m.PurposeName), Value = m.PurposeID.ToString() });
              model.purposeListItems = viewPurpose;
              var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt16(LoggedInOrganizationID));
              var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });
              model.centerListItems = viewCenter;
              var allbranch = officeService.GetAll().Where(l => l.OfficeID == SessionHelper.LoginUserOfficeID.Value && l.OrgID==LoggedInOrganizationID);
              var viewbranch = allbranch.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });
              model.branchListItems = viewbranch;
              var allSearchProd = productService.SearchProduct(0, Convert.ToInt32(LoggedInOrganizationID), "L");
              var viewProdList = allSearchProd.Select(x => x).ToList().Select(x => new SelectListItem
              {
                  Value = x.ProductID.ToString(),
                  Text = string.Format("{0} - {1}", x.ProductCode.ToString(), x.ProductName.ToString())
              });
              var proditems = new List<SelectListItem>();
              proditems.Add(new SelectListItem() { Text = "", Value = "", Selected = true });
              model.productListItems = proditems;
              model.MemberProductItemsSelected = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(allSearchProd);
              var allInvestor = investorService.GetAll().Where(i => i.IsActive == true && i.OrgID==LoggedInOrganizationID);
              var viewInvestor = allInvestor.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.InvestorCode, m.InvestorName), Value = m.InvestorID.ToString() });
              model.investorListItems = viewInvestor;
              model.MemberProductItemsSelected = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(allSearchProd);
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
        public JsonResult GetProductList(string Member_id, string center_id)
        {
            List<ProductViewModel> List_ProductViewModel = new List<ProductViewModel>();
            var param = new { OfficeId = LoginUserOfficeID, MemberId = Member_id, CenterId = center_id };
            var div_items = ultimateReportService.GetProductList(param);

            List_ProductViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new ProductViewModel
            {
                ProductID = row.Field<Int16>("ProductID"),
                ProductCode = row.Field<string>("ProductCode"),
                ProductName = row.Field<string>("ProductName")
            }).ToList();

            var viewProduct = List_ProductViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = x.ProductCode.ToString() + " " + x.ProductName.ToString()
            });
            var d_items = new List<SelectListItem>();
            d_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            d_items.AddRange(viewProduct);
            return Json(d_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetListOfInstallment(int prodId, long memId, int cenId)
          {
              decimal vprincipalLoan = 0;
              decimal vLoanRepaid = 0;
              int vduration = 0;
              int vLoanTerm = 0;
              decimal vInterestCHarge = 0;
              decimal vInterestPaid = 0;
              string disburseDate = "";
              decimal vwriteOffloan = 0;
              decimal vwriteOffInterest = 0;
              var param = new { @OrgID = SessionHelper.LoginUserOrganizationID, @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = Convert.ToInt32(cenId), @ProductID = prodId, @MemberID = Convert.ToInt64(memId), @qType = 1, };
              var pbr = ultimateReportService.GetriteOffSummarylist(param);

              //var pbr = loansSummaryService.GetAll().Where(l => l.OrgID==LoggedInOrganizationID && l.OfficeID == LoginUserOfficeID && l.CenterID == cenId && l.ProductID == prodId && l.MemberID == memId && l.LoanStatus == 1 && l.IsActive == true).FirstOrDefault();
              if (pbr.Tables[0].Rows.Count > 0)
              {
                  vprincipalLoan = Convert.ToDecimal(pbr.Tables[0].Rows[0]["PrincipalLoan"].ToString()); //pbr.PrincipalLoan;
                  vLoanRepaid = Convert.ToDecimal(pbr.Tables[0].Rows[0]["LoanRepaid"].ToString());// pbr.LoanRepaid;
                  vduration = Convert.ToInt16(pbr.Tables[0].Rows[0]["Duration"].ToString());// pbr.Duration;
                  vLoanTerm = Convert.ToInt16(pbr.Tables[0].Rows[0]["LoanTerm"].ToString());// pbr.LoanTerm;
                  disburseDate = pbr.Tables[0].Rows[0]["DisburseDate"].ToString(); //Convert.ToString(pbr.DisburseDate);
                  vInterestCHarge = Convert.ToDecimal(pbr.Tables[0].Rows[0]["IntCharge"].ToString());// pbr.IntCharge;
                  vInterestPaid = Convert.ToDecimal(pbr.Tables[0].Rows[0]["IntPaid"].ToString()); //pbr.IntPaid;
                  vwriteOffloan = Convert.ToDecimal(vprincipalLoan) - Convert.ToDecimal(vLoanRepaid);// (pbr.PrincipalLoan - pbr.LoanRepaid);
                  vwriteOffInterest = Convert.ToDecimal(vInterestCHarge) - Convert.ToDecimal(vInterestPaid); //(pbr.IntCharge - pbr.IntPaid);
              }


              var result = new { PrincipalLoan = vprincipalLoan.ToString(), LoanRepaid = vLoanRepaid.ToString(), LoanTerm = vLoanTerm.ToString(), DisburseDate = disburseDate.ToString(), Duration = vduration.ToString(), IntCharge = vInterestCHarge.ToString(), IntPaid = vInterestPaid.ToString(), WriteOffLoan = vwriteOffloan.ToString(), WriteOffInterest = vwriteOffInterest.ToString() };
              return Json(result, JsonRequestBehavior.AllowGet);
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
        // GET: WriteOffDeclaration
        public ActionResult Index()
        {

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
            var model = new WriteOffDeclarationViewModel() { CurLoan = 0, PreLoan = 0, WriteOffInterest = 0, WriteOffLoan = 0, CumLoanDue = 0 };

            MapDropDownList(model);

            return View(model);
        }
        public JsonResult GenerateWriteOffList(int jtStartIndex, int jtPageSize, string jtSorting, string DateFrom, long MemberId)
        {
            try
            {

                var allHistory = loantrxService.getWriteOffDeclarationList(Convert.ToInt16(LoggedInOrganizationID),SessionHelper.LoginUserOfficeID.Value, MemberId, Convert.ToDateTime(DateFrom));
                var detail = allHistory.ToList();
                var totCount = detail.Count();
                var currentPageRecords = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totCount });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        // GET: WriteOffDeclaration/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: WriteOffDeclaration/Create
        public ActionResult Create()
        {
            specialLoanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);

            var model = new WriteOffDeclarationViewModel() { CurLoan = 0, PreLoan = 0, WriteOffInterest = 0, WriteOffLoan = 0, CumLoanDue = 0 };
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
        // POST: WriteOffDeclaration/Create
        [HttpPost]
        public ActionResult Create(WriteOffDeclarationViewModel model, FormCollection form)
        {
            try
            {
                var entity = Mapper.Map<WriteOffDeclarationViewModel, LoanSummary>(model);
                // TODO: Add insert logic here
                loansSummaryService.setWriteOffList(Convert.ToInt16(LoggedInOrganizationID), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt64(entity.MemberID), entity.CenterID, entity.ProductID, entity.LoanTerm, TransactionDate, entity.WriteOffLoan, entity.WriteOffInterest);
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        // GET: WriteOffDeclaration/Edit/5
        public ActionResult Edit(int id)
        {

            if (loansSummaryService.IsContinued(id))
            {
                var loansummary = loansSummaryService.GetByIdLong(id);
                var member = GetMember(Convert.ToInt64(loansummary.MemberID));
                var entity = Mapper.Map<LoanSummary, WriteOffDeclarationViewModel>(loansummary);
                MapDropDownList(entity);
                ViewBag.MemberName = string.Format("{0} - {1}", member.MemberCode, member.FirstName);
                return View(entity);
            }
            else
                ModelState.AddModelError("Validation", "Discontinued ID, please enter a diferent id and name.");
            return RedirectToAction("Index");
        }
        // POST: WriteOffDeclaration/Edit/5
        [HttpPost]
        public ActionResult Edit(WriteOffDeclarationViewModel model)
        {
            try
            {
                var entity = Mapper.Map<WriteOffDeclarationViewModel, LoanSummary>(model);
                // TODO: Add insert logic here
                loansSummaryService.setWriteOffList(Convert.ToInt16(LoggedInOrganizationID), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt64(entity.MemberID), entity.CenterID, entity.ProductID, entity.LoanTerm, TransactionDate, entity.WriteOffLoan, entity.WriteOffInterest);
                return GetSuccessMessageResult();

            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        // GET: WriteOffDeclaration/Delete/5
        public ActionResult Delete(long id)
        {
            var getLoanSummary = loansSummaryService.GetByIdLong(id);
            // TODO: Add delete logic here
            loansSummaryService.delWriteOffList(Convert.ToInt16(LoggedInOrganizationID), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt64(getLoanSummary.MemberID), getLoanSummary.CenterID, getLoanSummary.ProductID, getLoanSummary.LoanTerm, TransactionDate, 0, 0);
            return RedirectToAction("Index");
        }
        // POST: WriteOffDeclaration/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                var getLoanSummary = loansSummaryService.GetById(id);
                // TODO: Add delete logic here
                loansSummaryService.delWriteOffList(Convert.ToInt16(LoggedInOrganizationID), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt64(getLoanSummary.MemberID), getLoanSummary.CenterID, getLoanSummary.ProductID, getLoanSummary.LoanTerm, TransactionDate, getLoanSummary.WriteOffLoan, getLoanSummary.WriteOffInterest);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
