using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{

    public class SavingCorrectionController : BaseController
    {
        private readonly ICenterService centerService;
        private readonly ISpecialSavingCollectionService specialSavingCollectionService;
        private readonly IBranchService branchService;
        private readonly IOfficeService officeService;
       
        private readonly IProductService productService;
        private readonly IMemberCategoryService membercategoryService;
      
        private readonly IPurposeService purposeService;
        private readonly IMemberService memberService;
        private readonly ISavingSummaryService savingSummaryService;
        private readonly IAccTrxMasterService accMasterService;
        private readonly IAccTrxDetailService accDetailService;
        private readonly ISavingCorrectionService savingCorrectionService;

        public SavingCorrectionController(ISpecialSavingCollectionService specialSavingCollectionService, ISavingSummaryService savingSummaryService, ICenterService centerService, IProductService productService, IMemberCategoryService membercategoryService, IOfficeService officeService, IPurposeService purposeService, IMemberService memberService, IAccTrxMasterService accMasterService, IAccTrxDetailService accDetailService, ISavingCorrectionService savingCorrectionService)
        {
            this.specialSavingCollectionService = specialSavingCollectionService;
            this.centerService = centerService;
            this.productService = productService;
            this.membercategoryService = membercategoryService;
            this.officeService = officeService;
            this.purposeService = purposeService;
            this.memberService = memberService;
            this.savingSummaryService = savingSummaryService;
            this.accMasterService = accMasterService;
            this.accDetailService = accDetailService;
            this.savingCorrectionService = savingCorrectionService;
        }
         [HttpPost]
        public ActionResult GetSpecialCollection(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                string vday = string.Empty;
                if (IsDayInitiated)
                    vday = TransactionDay;
                var specialLoandetail = savingCorrectionService.GetSavingCorrectionDetail(LoggedInOrganizationID,LoginUserOfficeID, TransactionDate);
                var detail = specialLoandetail.ToList();

                var totalCount = detail.Count();
                var entities = detail.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<getSavingsCorrection_Result>, IEnumerable<SavingCorrectionViewModel>>(entities);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });

                //var viewploansummary = Mapper.Map<IEnumerable<proc_get_SpecialSavingCollection_Result>, IEnumerable<SpecialSavingCollectionViewModel>>(detail);
                //return Json(new { Result = "OK", Records = viewploansummary });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

         public ActionResult GetInstallment(decimal SavingInstallment, decimal WithDrawal, decimal Penalty, decimal balance, string officeId, string centerId, string MemId, string ProdId,string noAccount)
         {
             decimal vLoanInstallment = 0;
             decimal savInsall = 0;
             decimal with = 0;
             decimal penal = 0;
             var model = new SpecialSavingCollectionViewModel();
            
             var entity = Mapper.Map<SpecialSavingCollectionViewModel, SavingCorrectionTrx>(model);

            
                 savInsall = SavingInstallment;
                 with = WithDrawal;
                 penal = Penalty;
                 var mlt = specialSavingCollectionService.GetAll().Where(s => s.OrgID==LoggedInOrganizationID && s.OfficeID == SessionHelper.LoginUserOfficeID.Value && s.CenterID ==Convert.ToInt16(centerId) && s.MemberID == Convert.ToInt16( MemId) && s.ProductID == Convert.ToInt16( ProdId) && s.NoOfAccount == Convert.ToInt16(noAccount) && s.IsActive==1).FirstOrDefault();
                 if (mlt != null)
                 {
                     vLoanInstallment = (mlt.Balance + savInsall + penal) - with;
                 }
                     
                 else
                 {
                     var sm = savingSummaryService.GetAll().Where(s => s.OrgID == LoggedInOrganizationID && s.OfficeID == SessionHelper.LoginUserOfficeID.Value && s.MemberID == Convert.ToInt16(MemId) && s.ProductID == Convert.ToInt16(ProdId) && s.NoOfAccount == Convert.ToInt16(noAccount) && s.IsActive == true).FirstOrDefault();

                   //  var sm = savingSummaryService.GetAll().Where(s => s.OfficeID == SessionHelper.LoginUserOfficeID.Value && s.CenterID == Convert.ToInt16(centerId) && s.MemberID == Convert.ToInt16(MemId) && s.ProductID == Convert.ToInt16(ProdId) && s.NoOfAccount == Convert.ToInt16(noAccount) && s.IsActive==true).FirstOrDefault();
                     if (sm != null)
                     {
                         savInsall = SavingInstallment;
                         with = WithDrawal;
                         penal = Penalty;

                         vLoanInstallment = (sm.Balance + savInsall + penal) - with;
                     }
                 }


             var result = new { loan = vLoanInstallment.ToString(), savInstall = savInsall.ToString(), withdrawal = with.ToString(), penalty = penal.ToString() };
             return Json(result, JsonRequestBehavior.AllowGet);
         }
         public JsonResult GetNoOfAccount(string officeId, string centerId, string MemId, string ProdId)
         {
             int vLoanTerm;
             int Loantrm = 0;
             decimal vBalance = 0;
            
             decimal savInsall = 0;
             decimal with = 0;
             decimal penal = 0;

             var model = new SpecialSavingCollectionViewModel();
             model.OfficeID = Convert.ToInt16(SessionHelper.LoginUserOfficeID.Value);
             model.CenterID = Convert.ToInt16(centerId);
             model.MemberID = Convert.ToInt64(MemId);
             model.ProductID = Convert.ToInt16(ProdId);


             var entity = Mapper.Map<SpecialSavingCollectionViewModel, SavingCorrectionTrx>(model);
             var mlt = specialSavingCollectionService.GetAll().Where(s => s.OrgID == LoggedInOrganizationID && s.OfficeID == SessionHelper.LoginUserOfficeID.Value && s.MemberID == entity.MemberID && s.ProductID == entity.ProductID && s.NoOfAccount == entity.NoOfAccount && s.IsActive == 1).FirstOrDefault();

             //var mlt = specialSavingCollectionService.GetAll().Where(s => s.OfficeID == SessionHelper.LoginUserOfficeID.Value && s.CenterID == entity.CenterID && s.MemberID == entity.MemberID && s.ProductID == entity.ProductID && s.NoOfAccount== entity.NoOfAccount).FirstOrDefault();
             if (mlt != null)
             {
                 savInsall = mlt.SavingInstallment;
                 with = mlt.Withdrawal;
                 penal = mlt.Penalty;
                 Loantrm = mlt.NoOfAccount;
                 vBalance = (mlt.Balance + savInsall + penal) - with;

             }
             else
             {
                 var sm = savingSummaryService.GetAll().Where(s => s.OrgID == LoggedInOrganizationID && s.OfficeID == SessionHelper.LoginUserOfficeID.Value && s.MemberID == entity.MemberID && s.ProductID == entity.ProductID && s.SavingStatus == 1 && s.IsActive == true).FirstOrDefault();
                 //var sm = savingSummaryService.GetAll().Where(s => s.OfficeID == SessionHelper.LoginUserOfficeID.Value && s.CenterID == entity.CenterID && s.MemberID == entity.MemberID && s.ProductID == entity.ProductID && s.SavingStatus == 1).FirstOrDefault();
                 if (sm != null)
                 {
                     savInsall = sm.SavingInstallment;
                     with = 0;
                     penal = 0;
                     Loantrm = sm.NoOfAccount;
                     vBalance = (sm.Balance + savInsall + penal) - with;
                 }
             }
             //Session[ProductSessionKey] = pbr;
             vLoanTerm = Loantrm;
             vBalance = vBalance;

             var result = new { LoanTerm = vLoanTerm.ToString(), Balance = vBalance, savInstall = savInsall.ToString(), withdrawal = with.ToString(), penalty = penal.ToString() };
             return Json(result, JsonRequestBehavior.AllowGet);
         }
         private void MapDropDownList(SavingCorrectionViewModel model)
        {

            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }
            var Transtype = new List<SelectListItem>();
            Transtype.Add(new SelectListItem() { Text = "Cash", Value = "30", Selected = true });
            Transtype.Add(new SelectListItem() { Text = "Transfer", Value = "31"});
           
            model.cashListItems = Transtype.AsEnumerable();

            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID));

            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)), Value = m.MemberID.ToString() });

            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;

            var allpurpose = purposeService.SearchPurpose(Convert.ToInt16(LoggedInOrganizationID));

            var viewPurpose = allpurpose.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.PurposeCode, m.PurposeName), Value = m.PurposeID.ToString() });

            model.purposeListItems = viewPurpose;


            string vCoday = TransactionDay;

           // var allcenter = centerService.SearchSpecialCenter(vCoday, SessionHelper.LoginUserOfficeID.Value);
            var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt16(LoggedInOrganizationID));

            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });

            model.centerListItems = viewCenter;

            var alloffice = officeService.GetAll().Where(s=>s.OfficeID==LoginUserOfficeID && s.OrgID==LoggedInOrganizationID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;




            var allSearchProd = productService.SearchProduct(0, Convert.ToInt16(LoggedInOrganizationID),"S");
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
        // GET: SpecialSavingCollection
        public ActionResult Index()
        {
            return View();
        }

        // GET: SpecialSavingCollection/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SpecialSavingCollection/Create
        public ActionResult Create()
        {
            var model = new SavingCorrectionViewModel();

            if (IsDayInitiated)
                model.TransactionDate = TransactionDate;
            MapDropDownList(model);

            return View(model);
        }

        // POST: SpecialSavingCollection/Create
        [HttpPost]
        public ActionResult Create(SpecialSavingCollectionViewModel model)
        {
            try
            {

                specialSavingCollectionService.delVoucher(LoginUserOfficeID, TransactionDate,LoggedInOrganizationID);

                var entity = Mapper.Map<SpecialSavingCollectionViewModel, SavingCorrectionTrx>(model);

                //Add Validlation Logic.

                if (ModelState.IsValid)
                {


                   

                        var errors = savingCorrectionService.IsValidLoan(entity);

                        if (errors.ToList().Count == 0)
                        {
                            savingCorrectionService.savingCorrection(Convert.ToInt16(LoggedInOrganizationID), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(entity.CenterID), Convert.ToInt64(entity.MemberID), Convert.ToInt16(entity.ProductID), Convert.ToInt16(entity.NoOfAccount), Convert.ToInt16(entity.TransType), entity.SavingInstallment, entity.Withdrawal, entity.Penalty, TransactionDate);

                           return GetSuccessMessageResult();
                            //ModelState.Clear();
                            //var emtpy = new SpecialSavingCollectionViewModel();
                            //if (IsDayInitiated)
                            //    model.TransactionDate = TransactionDate;
                            //MapDropDownList(emtpy);

                            //return View(emtpy);
                        }
                        else
                            return GetErrorMessageResult();

                    }
                   

                    //var emtpy1 = new SpecialSavingCollectionViewModel();
                    //        if (IsDayInitiated)
                    //            model.TransactionDate = TransactionDate;
                    //        MapDropDownList(emtpy1);

                    //        return View(emtpy1);

                return GetErrorMessageResult();
                

            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: SpecialSavingCollection/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SpecialSavingCollection/Edit/5
        [HttpPost]
        public ActionResult Edit(SavingCorrectionViewModel Model)
        {
            try
            {
                specialSavingCollectionService.delVoucher(LoginUserOfficeID, TransactionDate,LoggedInOrganizationID);

                var loanser = savingCorrectionService.GetByIdLong(Convert.ToInt64(Model.SavingCorrectionTrxID));
                var entity = Mapper.Map<SavingCorrectionViewModel, SavingCorrectionTrx>(Model);
                if (ModelState.IsValid)
                {

                    var errors = savingCorrectionService.IsValidLoan(loanser);

                    if (errors.ToList().Count == 0)
                    {

                        savingCorrectionService.savingCorrection(Convert.ToInt16(LoggedInOrganizationID), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(loanser.CenterID), Convert.ToInt64(loanser.MemberID), Convert.ToInt16(loanser.ProductID), Convert.ToInt16(loanser.NoOfAccount), Convert.ToInt16(loanser.TransType), entity.SavingInstallment, entity.Withdrawal, entity.Penalty, TransactionDate);

                        return GetSuccessMessageResult();
                    }
                    else
                        return GetErrorMessageResult();
                       // return Json(new { Result = "ERROR" });
                }
                return GetErrorMessageResult();
                //return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
            //catch (Exception ex)
            //{
            //    return Json(new { Result = "ERROR", Message = ex.Message });
            //}
        }

        // GET: SpecialSavingCollection/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SpecialSavingCollection/Delete/5
        [HttpPost]
        public ActionResult Delete(int DailySavingTrxID, SpecialLoanCollectionViewModel model)
        {
            try
            {
                specialSavingCollectionService.delVoucher(LoginUserOfficeID, TransactionDate,LoggedInOrganizationID);


                var dailyLoanCollectionID = specialSavingCollectionService.GetAll().Where(l => l.OrgID == Convert.ToInt16(LoggedInOrganizationID) && l.OfficeID == Convert.ToInt16(LoginUserOfficeID) && l.CenterID == Convert.ToInt16(model.CenterID) && l.MemberID == Convert.ToInt64(model.MemberID) && l.ProductID == model.ProductID && l.NoOfAccount == model.LoanTerm && l.TransType == model.TrxType && l.TransactionDate == TransactionDate).FirstOrDefault();
                if (dailyLoanCollectionID != null)
                {
                    var dailyLoanCollectionTRXID = dailyLoanCollectionID.DailySavingTrxID;
                    specialSavingCollectionService.Delete(Convert.ToUInt16(dailyLoanCollectionTRXID));
                }
                var sp = savingCorrectionService.GetById(DailySavingTrxID);
                var entity = Mapper.Map<SavingCorrectionTrx, SpecialSavingCollectionViewModel>(sp);
                savingCorrectionService.Delete(DailySavingTrxID);
                // TODO: Add delete logic here
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}
