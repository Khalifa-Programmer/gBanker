
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
using gBanker.Data.CodeFirstMigration;
using System.Configuration;
using gBanker.Web.Helpers;
namespace gBanker.Web.Controllers
{
    public class ExceptionalLoanCollectionController : BaseController
    {

        private readonly ICenterService centerService;
        private readonly ISpecialLoanCollectionService specialLoanCollectionService;
        private readonly IBranchService branchService;
        private readonly IOfficeService officeService;
        private readonly IProductService productService;
        private readonly IMemberCategoryService membercategoryService;
        private readonly IPurposeService purposeService;
        private readonly IMemberService memberService;
        private readonly ILoanSummaryService LoanSummaryService;

        public ExceptionalLoanCollectionController(ISpecialLoanCollectionService specialLoanCollectionService, ILoanSummaryService LoanSummaryService, ICenterService centerService, IProductService productService, IMemberCategoryService membercategoryService, IOfficeService officeService, IPurposeService purposeService, IMemberService memberService)
        {
            this.specialLoanCollectionService = specialLoanCollectionService;
            this.centerService = centerService;
            this.productService = productService;
            this.membercategoryService = membercategoryService;
            this.officeService = officeService;
            this.purposeService = purposeService;
            this.memberService = memberService;
            this.LoanSummaryService = LoanSummaryService;
        }
        public ActionResult GetSpecialCollection(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {


                string vday = string.Empty;
                if (IsDayInitiated)
                    vday = TransactionDay;
                var specialLoandetail = specialLoanCollectionService.GetSpecialLoanCollectionDetail(LoggedInOrganizationID, LoginUserOfficeID, vday, filterColumn, filterValue);
                var detail = specialLoandetail.ToList();



                var totalCount = detail.Count();
                var entities = detail.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<proc_get_SpecialLoanCollection_Result>, IEnumerable<SpecialLoanCollectionViewModel>>(entities);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        private void MapDropDownList(SpecialLoanCollectionViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }
            var Transtype = new List<SelectListItem>();
            Transtype.Add(new SelectListItem() { Text = "Cash", Value = "20", Selected = true });
            Transtype.Add(new SelectListItem() { Text = "Transfer", Value = "21" });

            model.cashListItems = Transtype.AsEnumerable();

            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID));

            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)), Value = m.MemberID.ToString() });

            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;

            var allpurpose = purposeService.SearchPurpose(Convert.ToInt32(LoggedInOrganizationID));

            var viewPurpose = allpurpose.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.PurposeCode, m.PurposeName), Value = m.PurposeID.ToString() });

            model.purposeListItems = viewPurpose;


            string vCoday = TransactionDay;

            //var allcenter = centerService.SearchSpecialCenter(vCoday, SessionHelper.LoginUserOfficeID.Value);
            var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID));
            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });

            model.centerListItems = viewCenter;

            var alloffice = officeService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.OrgID == LoggedInOrganizationID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;




            var allSearchProd = productService.SearchProduct(0, Convert.ToInt32(LoggedInOrganizationID),"L");
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

            var allmembercategory = membercategoryService.GetAll().Where(a => a.OrgID == LoggedInOrganizationID);

            var viewmembercategory = allmembercategory.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCategoryID, m.CategoryName), Value = m.MemberCategoryID.ToString() });

            model.membercategoryListItems = viewmembercategory;



        }
        public ActionResult GetMemberList(string memberid, string centerId)
        {
            var MemberByCenterSessionKey = string.Format("MemberByCenterSessionKey_{0}", centerId);
            var memberList = new List<Member>();
            if (Session[MemberByCenterSessionKey] != null)
                memberList = Session[MemberByCenterSessionKey] as List<Member>;
            else
            {
                var mbr = memberService.GetByCenterId(int.Parse(centerId), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID)).ToList();
                Session[MemberByCenterSessionKey] = mbr;
                memberList = mbr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInstallment(string officeId, string centerId, string MemId, int productid, int loanTerm, int trxType)
        {
            decimal vLoanInstallment = 0;
            decimal vInterestInstallment = 0;
            decimal vTotalIns = 0;
            string vInterestCalcMethod = string.Empty, vPaymentFreq = string.Empty;
            decimal vLoanDue = 0;
            decimal vInterestDue = 0;
            decimal vPrincipalLoan = 0;
            decimal vLoanRepaid = 0;
            Int64 vDailyLoanTrxID = 0;

            var specialLoandetail = specialLoanCollectionService.GetSetSLCTxtKeyPress(Convert.ToInt32(LoggedInOrganizationID), Convert.ToInt32(LoginUserOfficeID), Convert.ToInt32(centerId), Convert.ToInt32(productid), Convert.ToInt64(MemId), loanTerm, TransactionDate, trxType);
            var detail = specialLoandetail.ToList();



            var totalCount = detail.Count();


            if (totalCount > 0)
            {
                var LoanInstallMent = detail.Where(l => l.OrgID == LoggedInOrganizationID && l.OfficeID == Convert.ToInt16(officeId) && l.centerid == Convert.ToInt16(centerId) && l.memberid == Convert.ToInt64(MemId) && l.productid == productid && l.LoanTerm == loanTerm).FirstOrDefault();
                if (LoanInstallMent != null)
                {
                    vLoanInstallment = Convert.ToDecimal(LoanInstallMent.LoanInstallment);
                    vInterestInstallment = Convert.ToDecimal(LoanInstallMent.IntPaid);
                    vTotalIns = vLoanInstallment + vInterestInstallment;
                    var prod = productService.GetById(productid);
                    vInterestCalcMethod = prod.InterestCalculationMethod;
                    vPaymentFreq = prod.PaymentFrequency;
                    vLoanDue = LoanInstallMent.DueLoanInstallment;
                    vInterestDue = LoanInstallMent.DueInterestInstallment;
                    vPrincipalLoan = LoanInstallMent.PrincipalLoan;
                    vLoanRepaid = LoanInstallMent.LoanRepaid;
                    //vDailyLoanTrxID = LoanInstallMent.DailyLoanTrxID;
                }
                else
                {
                    vLoanInstallment = 0;
                    vInterestInstallment = 0;
                    vTotalIns = 0;
                    var prod = productService.GetById(productid);
                    vInterestCalcMethod = prod.InterestCalculationMethod;
                    vPaymentFreq = prod.PaymentFrequency;
                }
            }
            else
            {
                vLoanInstallment = 0;
                vInterestInstallment = 0;
                vTotalIns = 0;
                var prod = productService.GetById(productid);
                vInterestCalcMethod = prod.InterestCalculationMethod;
                vPaymentFreq = prod.PaymentFrequency;
            }
   
            var result = new { loan = vLoanInstallment.ToString(), interest = vInterestInstallment.ToString(), interestCalcMethod = vInterestCalcMethod, PaymentFreq = vPaymentFreq, LoanDue = vLoanDue, InterestDue = vInterestDue, PrincipalLoan = vPrincipalLoan, LoanRepaid = vLoanRepaid, DailyLoanTrxID = vDailyLoanTrxID, total = vTotalIns.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMaxLoanTerm(string officeId, string centerId, string MemId, string ProdId)
        {
            int vLoanTerm;
            var model = new SpecialLoanCollectionViewModel();
            model.OfficeID = Convert.ToInt16(officeId);
            model.CenterID = Convert.ToInt16(centerId);
            model.MemberID = Convert.ToInt64(MemId);
            model.ProductID = Convert.ToInt16(ProdId);


            var entity = Mapper.Map<SpecialLoanCollectionViewModel, LoanSummary>(model);
            entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
            var mlt = specialLoanCollectionService.getMaxLoanterm(entity);
            //Session[ProductSessionKey] = pbr;
            vLoanTerm = Convert.ToInt16(mlt);

            var result = new { LoanTerm = vLoanTerm.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public Member GetMember(long memberid)
        {
            var mbr = memberService.GetByIdLong(memberid);
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
        // GET: ExceptionalLoanCollection
        public ActionResult Index()
        {
            return View();
        }

        // GET: ExceptionalLoanCollection/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ExceptionalLoanCollection/Create
        public ActionResult Create()
        {
            var model = new SpecialLoanCollectionViewModel();

            if (IsDayInitiated)
                model.TrxDate = TransactionDate;
            MapDropDownList(model);
            return View(model);
        }

        // POST: ExceptionalLoanCollection/Create
        [HttpPost]
        public ActionResult Create(SpecialLoanCollectionViewModel Model)
        {
            try
            {
                if (!IsDayInitiated)
                {
                    return GetErrorMessageResult("Please run the start work process");
                }
                specialLoanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);

              var entity = Mapper.Map<SpecialLoanCollectionViewModel, DailyLoanTrx>(Model);
                if (ModelState.IsValid)
                {
                    var specialLoandetail = specialLoanCollectionService.GetSetSLCTxtKeyPress(Convert.ToInt32(LoggedInOrganizationID), Convert.ToInt32(LoginUserOfficeID), Convert.ToInt32(entity.CenterID), Convert.ToInt32(entity.ProductID), Convert.ToInt64(entity.MemberID), entity.LoanTerm, TransactionDate, entity.TrxType);
                    var detail = specialLoandetail.ToList();



                    var totalCount = detail.Count();


                    if (totalCount > 0)
                    {
                        var LoanInstallMent = detail.Where(l => l.OrgID == LoggedInOrganizationID && l.OfficeID == Convert.ToInt16(LoginUserOfficeID) && l.centerid == Convert.ToInt16(entity.CenterID) && l.memberid == Convert.ToInt64(entity.MemberID) && l.productid == Convert.ToInt32(entity.ProductID) && l.LoanTerm == entity.LoanTerm).FirstOrDefault();
                        if (LoanInstallMent != null)
                        {

                            var errors = specialLoanCollectionService.IsValidLoan(entity);

                            if (errors.ToList().Count == 0)
                            {

                                 entity.Advance = 0;
                                entity.CumIntCharge = Convert.ToDecimal(LoanInstallMent.CumIntCharge);
                                entity.DueRecovery = 0;
                                entity.EmployeeID = LoanInstallMent.EmployeeID;
                                entity.InstallmentDate = Convert.ToDateTime(LoanInstallMent.InstallmentDate);
                                entity.InstallmentNo = Convert.ToInt16(LoanInstallMent.InstallmentNo);
                                entity.IntCharge = Convert.ToDecimal(LoanInstallMent.IntCharge);
                                entity.IntDue = LoanInstallMent.DueLoanInstallment;
                                entity.InterestCalculationMethod = LoanInstallMent.InterestCalculationMethod;
                                entity.IntPaid = Convert.ToDecimal(LoanInstallMent.IntPaid);
                                entity.InvestorID = Convert.ToByte(LoanInstallMent.InvestorID);
                                entity.LoanDue = LoanInstallMent.DueLoanInstallment;
                                entity.LoanPaid = Convert.ToDecimal(LoanInstallMent.LoanInstallment);
                                entity.LoanRepaid = LoanInstallMent.LoanRepaid;
                                entity.LoanSummaryID = Convert.ToInt64(LoanInstallMent.loansummaryId);
                                entity.LoanTerm = LoanInstallMent.LoanTerm;
                                entity.MemberCategoryID = LoanInstallMent.MemberCategoryID;
                                entity.MemberCode = LoanInstallMent.membercode;
                                entity.MemberName = LoanInstallMent.MemberName;
                                entity.OfficeID = Convert.ToInt16(LoginUserOfficeID);
                                entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                                entity.PrincipalLoan = LoanInstallMent.PrincipalLoan;
                                entity.ProductCode = LoanInstallMent.ProductCode;
                                entity.ProductName = LoanInstallMent.ProductName;
                                entity.TrxDate = Convert.ToDateTime(LoanInstallMent.trxdate);

                                specialLoanCollectionService.Create(entity);
                                return GetSuccessMessageResult();
                            }
                            else
                                return GetErrorMessageResult();
                        }
                        else
                            return GetErrorMessageResult();

                    }
                    return GetErrorMessageResult();
                }

                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: ExceptionalLoanCollection/Edit/5
        public ActionResult Edit(long id)
        {
            var loanser = specialLoanCollectionService.GetByIdLong(id);
            var member = GetMember(Convert.ToInt64(loanser.MemberID));
            var entity = Mapper.Map<DailyLoanTrx, SpecialLoanCollectionViewModel>(loanser);
            ViewBag.MemberName = string.Format("{0} - {1}", member.MemberCode, member.FirstName);
            MapDropDownList(entity);
            return View(entity);
        }

        // POST: ExceptionalLoanCollection/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, SpecialLoanCollectionViewModel Model)
        {
            try
            {
                if (!IsDayInitiated)
                {
                    return GetErrorMessageResult("Please run the start work process");
                }
                specialLoanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);

                var loanser = specialLoanCollectionService.GetByIdLong(Convert.ToInt64(Model.DailyLoanTrxID));
                var entity = Mapper.Map<SpecialLoanCollectionViewModel, DailyLoanTrx>(Model);
                if (ModelState.IsValid)
                {

                    var errors = specialLoanCollectionService.IsValidLoan(loanser);

                    if (errors.ToList().Count == 0)
                    {

                        loanser.LoanPaid = Model.LoanPaid;
                        loanser.IntPaid = Model.IntPaid;
                        loanser.TotalPaid = Model.TotalPaid;
                        loanser.TrxType = Model.TrxType;
                        specialLoanCollectionService.Update(loanser);
                        return GetSuccessMessageResult();
                    }
                    else
                        return GetErrorMessageResult(errors);
                }
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: ExceptionalLoanCollection/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ExceptionalLoanCollection/Delete/5
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
    }
}
