
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
    public class LoanCorrectionController : BaseController
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
        private readonly ILoanCorrectionService loanCorrectionService;
        private readonly ISmsLogService smsLogService;
        private readonly ISmsConfigService smsConfigService;
        public LoanCorrectionController(ISpecialLoanCollectionService specialLoanCollectionService, ILoanSummaryService LoanSummaryService, ICenterService centerService, IProductService productService, IMemberCategoryService membercategoryService, IOfficeService officeService, IPurposeService purposeService, IMemberService memberService, ILoanCorrectionService loanCorrectionService, ISmsLogService smsLogService, ISmsConfigService smsConfigService)
        {
            this.specialLoanCollectionService = specialLoanCollectionService;
            this.centerService = centerService;
            this.productService = productService;
            this.membercategoryService = membercategoryService;
            this.officeService = officeService;
            this.purposeService = purposeService;
            this.memberService = memberService;
            this.LoanSummaryService = LoanSummaryService;
            this.loanCorrectionService = loanCorrectionService;
            this.smsLogService = smsLogService;
            this.smsConfigService = smsConfigService;
        }
        // GET: SpecialLoanCollection
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetSpecialCollection(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                string vday = string.Empty;
                if (IsDayInitiated)
                    vday = TransactionDay;
                var specialLoandetail = loanCorrectionService.GetLoanCorrectionDetail(LoggedInOrganizationID,LoginUserOfficeID, TransactionDate);
                var detail = specialLoandetail.ToList();



                var totalCount = detail.Count();
                var entities = detail.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<proc_get_LoanCorrection_Result>, IEnumerable<LoanCorrectionViewModel>>(entities);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });


                //var viewploansummary = Mapper.Map<IEnumerable<proc_get_SpecialLoanCollection_Result>, IEnumerable<SpecialLoanCollectionViewModel>>(detail);
                //return Json(new { Result = "OK", Records = viewploansummary });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        private void MapDropDownList(LoanCorrectionViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }
            var Transtype = new List<SelectListItem>();
            Transtype.Add(new SelectListItem() { Text = "Cash", Value = "30", Selected = true });
            Transtype.Add(new SelectListItem() { Text = "Transfer", Value = "31" });
           
            model.cashListItems = Transtype.AsEnumerable();

            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID),Convert.ToInt16(LoggedInOrganizationID));

            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)), Value = m.MemberID.ToString() });

            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;

            var allpurpose = purposeService.SearchPurpose(Convert.ToInt16(LoggedInOrganizationID));

            var viewPurpose = allpurpose.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.PurposeCode, m.PurposeName), Value = m.PurposeID.ToString() });

            model.purposeListItems = viewPurpose;


            string vCoday = TransactionDay;

            //var allcenter = centerService.SearchSpecialCenter(vCoday, SessionHelper.LoginUserOfficeID.Value);
            var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt16(LoggedInOrganizationID));
            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });

            model.centerListItems = viewCenter;

            var alloffice = officeService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;




            var allSearchProd = productService.SearchProduct(0, Convert.ToInt16(LoggedInOrganizationID),"L");
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

            var allmembercategory = membercategoryService.GetAll().Where(m=>m.OrgID==LoggedInOrganizationID);

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
                var mbr = memberService.GetByCenterId(int.Parse(centerId), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID)).ToList();
                Session[MemberByCenterSessionKey] = mbr;
                memberList = mbr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInstallment(string officeId, string centerId, string MemId, int productid,int loanTerm)
        {
            decimal vLoanInstallment = 0;
            decimal vInterestInstallment = 0;
            decimal vTotalIns=0;
            var LoanInstallMent = LoanSummaryService.GetAll().Where(l => l.OrgID==LoggedInOrganizationID && l.OfficeID == Convert.ToInt16(officeId) && l.CenterID == Convert.ToInt16(centerId) && l.MemberID == Convert.ToInt16(MemId) && l.ProductID == productid && l.LoanTerm == loanTerm && l.IsActive == true).FirstOrDefault();

            //var LoanInstallMent = LoanSummaryService.GetAll().Where(l => l.OfficeID == Convert.ToInt16(officeId) && l.CenterID == Convert.ToInt16(centerId) && l.MemberID == Convert.ToInt16(MemId) && l.ProductID == productid && l.LoanTerm == loanTerm).FirstOrDefault();
            if (LoanInstallMent != null)
            {
                vLoanInstallment = LoanInstallMent.LoanInstallment;
                vInterestInstallment = LoanInstallMent.IntInstallment;
                vTotalIns = vLoanInstallment + vInterestInstallment;

            }
            else
            {
                vLoanInstallment = 0;
                vInterestInstallment = 0;
                vTotalIns = 0;
            }
                    
            var result = new { loan = vLoanInstallment.ToString(), interest = vInterestInstallment.ToString(),total=vTotalIns.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMaxLoanTerm(string officeId, string centerId, string MemId, string ProdId)
        {
            int vLoanTerm;
            var model = new SpecialLoanCollectionViewModel();
            model.OfficeID = Convert.ToInt16(officeId);
            model.CenterID = Convert.ToInt32(centerId);
            model.MemberID = Convert.ToInt16(MemId);
            model.ProductID = Convert.ToInt16(ProdId);


            var entity = Mapper.Map<SpecialLoanCollectionViewModel, LoanSummary>(model);
            entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
            var mlt = specialLoanCollectionService.getMaxLoanterm(entity);
            //Session[ProductSessionKey] = pbr;
            vLoanTerm = Convert.ToInt16(mlt);

            var result = new { LoanTerm = vLoanTerm.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        // GET: SpecialLoanCollection/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
        // GET: SpecialLoanCollection/Create
        public ActionResult Create()
        {
            var model = new LoanCorrectionViewModel();

            if (IsDayInitiated)
                model.TrxDate = TransactionDate;
            MapDropDownList(model);
            specialLoanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);

            return View(model);
        }
        // POST: SpecialLoanCollection/Create
        [HttpPost]
        public ActionResult Create(LoanCorrectionViewModel model)
        {
            try
            {
                specialLoanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate,LoggedInOrganizationID);

                var entity = Mapper.Map<LoanCorrectionViewModel, DailyLoanTrx>(model);

                //Add Validlation Logic.

                if (ModelState.IsValid)
                {


                    int type = 0;
                    var prod = GetProduct(entity.ProductID);
                    if (prod.PaymentFrequency == "W")
                    {
                        type = 1;
                    }
                    else if (prod.PaymentFrequency == "M")
                    {
                        type = 2;
                    }
                    else
                    {
                        type = 3;
                    }
                    entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                    var errors = specialLoanCollectionService.IsValidLoan(entity);
                    if (errors.ToList().Count == 0)
                    {
                        entity.TrxDate = TransactionDate;
                        specialLoanCollectionService.SpecialCollection( Convert.ToInt16(LoggedInOrganizationID),LoginUserOfficeID, entity.CenterID, entity.ProductID, Convert.ToInt16(entity.MemberID), entity.LoanTerm, TransactionDay, TransactionDate, type, entity.TrxType, entity.LoanPaid, entity.IntPaid);
                        specialLoanCollectionService.LoanCorrection(LoggedInOrganizationID,LoginUserOfficeID, entity.CenterID, entity.ProductID, Convert.ToInt16(entity.MemberID), entity.LoanTerm, TransactionDay, TransactionDate, type, entity.TrxType, entity.LoanPaid, entity.IntPaid);
                        return GetSuccessMessageResult();
                        //ModelState.Clear();
                        //var emtpy = new LoanCorrectionViewModel();
                        //if (IsDayInitiated)
                        //    model.TrxDate = TransactionDate;

                        //    MapDropDownList(emtpy);
                        //    return View(emtpy);
                    }
                    else
                        return GetErrorMessageResult(errors);
                    //    ModelState.AddModelErrors(errors);
                    //if (IsDayInitiated)
                    //    model.TrxDate = TransactionDate;
                    //    MapDropDownList(model);
                    //    return View(model);

                    //if (IsDayInitiated)
                    //    model.TrxDate = TransactionDate;
                    //    MapDropDownList(model);
                    //    return View(model);
                }
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        // GET: SpecialLoanCollection/Edit/5
        public ActionResult Edit(int id, SpecialLoanCollectionViewModel Model)
        {
            return View();
        }
        // POST: SpecialLoanCollection/Edit/5
        [HttpPost]
        public ActionResult Edit(LoanCorrectionViewModel Model)
        {
            try
            {
                specialLoanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate,LoggedInOrganizationID);

                var loanser = loanCorrectionService.GetById(Convert.ToInt16(Model.LoanCorrectionTrxID));

                var entity = Mapper.Map<LoanCorrectionViewModel, LoanCorrectionTrx>(Model);

                if (ModelState.IsValid)
                {
                    int type = 0;

                    var prod = GetProduct(loanser.ProductID);

                    if (prod.PaymentFrequency == "W")
                    {

                        type = 1;

                    }
                    else if (prod.PaymentFrequency == "M")
                    {

                        type = 2;

                    }
                    else
                    {

                        type = 3;

                    }
                    var errors = loanCorrectionService.IsValidLoan(loanser);

                    if (errors.ToList().Count == 0)
                    {
                        entity.TrxDate = TransactionDate;

                        specialLoanCollectionService.LoanCorrection(LoggedInOrganizationID,LoginUserOfficeID, loanser.CenterID, loanser.ProductID, Convert.ToInt16(loanser.MemberID), loanser.LoanTerm, TransactionDay, TransactionDate, type, loanser.TrxType, entity.LoanPaid, entity.IntPaid);

                        return GetSuccessMessageResult();
                    }

                    else

                    return GetErrorMessageResult();
                }

                return GetErrorMessageResult();
               
            }

            catch (Exception ex)
            {

                return GetErrorMessageResult(ex);

            }

        }
        // GET: SpecialLoanCollection/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        [HttpPost]
        public JsonResult DeleteDisburse(long LoanSummaryID)
        {
            try
            {
                //disburseservice.Delete(LoanSummaryID);
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        // POST: SpecialLoanCollection/Delete/5
        [HttpPost]
        public ActionResult Delete(int DailyLoanTrxID, SpecialLoanCollectionViewModel model)
        {
            try
            {
                specialLoanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate,LoggedInOrganizationID);

                 var dailyLoanCollectionID = specialLoanCollectionService.GetAll().Where(l => l.OrgID==LoggedInOrganizationID && l.OfficeID == Convert.ToInt16(LoginUserOfficeID) && l.CenterID == Convert.ToInt16(model.CenterID) && l.MemberID == Convert.ToInt16(model.MemberID) && l.ProductID == model.ProductID && l.LoanTerm == model.LoanTerm && l.TrxType==model.TrxType && l.TrxDate==TransactionDate).FirstOrDefault();
                 if (dailyLoanCollectionID != null)
                 {
                     var dailyLoanCollectionTRXID = dailyLoanCollectionID.DailyLoanTrxID;
                     specialLoanCollectionService.Delete(Convert.ToUInt16( dailyLoanCollectionTRXID));
                 }
                var sp = loanCorrectionService.GetById(DailyLoanTrxID);
                var entity = Mapper.Map<LoanCorrectionTrx, SpecialLoanCollectionViewModel>(sp);
                loanCorrectionService.Delete(DailyLoanTrxID);
                // TODO: Add delete logic here
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult GetBalance(decimal loanRepaid, decimal intPaid)
        {
            decimal vBalance =  (loanRepaid + intPaid);
            var result = new { balance = vBalance.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
