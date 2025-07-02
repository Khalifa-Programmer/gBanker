using AutoMapper;
using gBanker.Data.CodeFirstMigration;
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
    public class MiscellaneouController : BaseController
    {
        private readonly IMiscellaneouService MiscellaneouService;
        private readonly IOfficeService officeService;
        private readonly IProductService productService;
        private readonly ICenterService centerService;
        private readonly ISpecialLoanCollectionService specialLoanCollectionService;
        private readonly IMemberService memberService;
        public MiscellaneouController(IMiscellaneouService MiscellaneouService, IProductService productService, IOfficeService officeService, ICenterService centerService,ISpecialLoanCollectionService specialLoanCollectionService, IMemberService memberService)
          {
              this.MiscellaneouService = MiscellaneouService;
              this.productService = productService;
              this.officeService = officeService;
              this.centerService = centerService;
              this.specialLoanCollectionService = specialLoanCollectionService;
            this.memberService = memberService;

          }
        // GET: Miscellaneou
        public JsonResult GetMiscellaneous(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                long totalCount;
                var allSavingsummary = MiscellaneouService.GetMiscellaneou(SessionHelper.LoginUserOfficeID.Value,TransactionDate);
                // var allloansummary = loansSummaryService.GetLoanApproveDetail(SessionHelper.LoginUserOfficeID.Value, filterColumn, filterValue).Where(a => a.ApproveDate == TransactionDate && (a.TransType == 101 || a.TransType == 102));
                //var totalCount = allloansummary.Count();
                //var entities = allloansummary.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<Proc_get_Miscellaneou_Result>, IEnumerable<MiscellaneouViewModel>>(allSavingsummary);
                return Json(new { Result = "OK", Records = currentPageRecords });
                //var entities = Mapper.Map<IEnumerable<DBLoanApproveDetailModel>, IEnumerable<LoanApprovalViewModel>>(allloansummary);
                //return Json(new { Result = "OK", Records = entities });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        private void MapDropDownList(MiscellaneouViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }

            var alloffice = officeService.GetAll().Where(l => l.OfficeID == SessionHelper.LoginUserOfficeID.Value && l.OrgID == LoggedInOrganizationID);
            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });
            model.officeListItems = viewOffice;

            var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt16(LoggedInOrganizationID)); 
            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });
            model.centerListItems = viewCenter;



            var allSearchProd = productService.SearchProduct(60, Convert.ToInt16(LoggedInOrganizationID),"M");
            var viewProdList = allSearchProd.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = string.Format("{0} - {1}", x.ProductCode.ToString(), x.ProductName.ToString())
            });
            var proditems = new List<SelectListItem>();
            proditems.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            proditems.AddRange(viewProdList);
            model.productListItems = proditems;


           

        }
        public ActionResult Index()
        {
            return View();
        }

        // GET: Miscellaneou/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Miscellaneou/Create
        public ActionResult Create()
        {
            var model = new MiscellaneouViewModel();
            if (IsDayInitiated)
                model.TrxDate = TransactionDate;
                MapDropDownList(model);
                return View(model);
        }

        // POST: Miscellaneou/Create
        [HttpPost]
        public ActionResult Create(MiscellaneouViewModel model)
        {
            try
            {
                model.IsActive = true;
                model.TrxDate = SessionHelper.TransactionDate;
                //var selectedMemberCategory = model.MemberCategoryList.Where(w => w.IsSelected).ToList();
                specialLoanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);
                var entity = Mapper.Map<MiscellaneouViewModel, Miscellaneou>(model);
                //Add Validlation Logic.
                if (ModelState.IsValid)
                {
                    MiscellaneouService.Create(entity);
                    return GetSuccessMessageResult();
                }
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }


        //public ActionResult Edit(int id)
        //{
        //    var loanapproval = MiscellaneouService.GetById(id);


        //    var entity = Mapper.Map<Miscellaneou, MiscellaneouViewModel>(loanapproval);


        //    var v = memberService.GetById((int)entity.MemberID);

        //    var memberName = v.MemberCode + ' ' + v.FirstName + v.MiddleName;

        //    ViewData["MemberName"] = memberName;

        //    MapDropDownList(entity);

        //    return View(entity);
        //}
        public ActionResult Edit(int id)
        {
            var loanapproval = MiscellaneouService.GetById(id);
            var entity = Mapper.Map<Miscellaneou, MiscellaneouViewModel>(loanapproval);

            var v = memberService.GetById((int)entity.MemberID);
            var memberName = "";
            if (v != null)
            {
                //return RedirectToAction("Index");
                memberName = v.MemberCode + ' ' + v.FirstName + v.MiddleName;
            }

            ViewData["MemberName"] = memberName;
            MapDropDownList(entity);
            return View(entity);
        }
        // POST: Miscellaneou/Edit/5
        [HttpPost]
        public ActionResult Edit(MiscellaneouViewModel model)
        {
            try
            {
                specialLoanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);
                var entity = Mapper.Map<MiscellaneouViewModel, Miscellaneou>(model);
                if (ModelState.IsValid)
                {
                    var getLoanSummary = MiscellaneouService.GetByIdLong(Convert.ToInt64(model.MiscellaneousID));
                    getLoanSummary.CenterID = entity.CenterID;
                    getLoanSummary.ProductID = entity.ProductID;
                    getLoanSummary.Amount = entity.Amount;
                    getLoanSummary.TrxDate = entity.TrxDate;
                    getLoanSummary.Remarks = entity.Remarks;
                    MiscellaneouService.Update(getLoanSummary);
                    return GetSuccessMessageResult();

                }
                return GetErrorMessageResult();

            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        //public ActionResult Edit(MiscellaneouViewModel model)
        //{
        //    try
        //    {
        //        specialLoanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);
        //        var entity = Mapper.Map<MiscellaneouViewModel, Miscellaneou>(model);
        //         if (ModelState.IsValid)
        //        {
        //        var getLoanSummary = MiscellaneouService.GetByIdLong(Convert.ToInt64(model.MiscellaneousID));
        //        getLoanSummary.CenterID = entity.CenterID;
        //        getLoanSummary.ProductID = entity.ProductID;
        //        getLoanSummary.Amount = entity.Amount;
        //        getLoanSummary.TrxDate = entity.TrxDate;
        //        MiscellaneouService.Update(getLoanSummary);
        //        return GetSuccessMessageResult();

        //        }
        //         return GetErrorMessageResult();

        //    }
        //    catch (Exception ex)
        //    {
        //        return GetErrorMessageResult(ex);
        //    }
        //}

        // GET: Miscellaneou/Delete/5
        public ActionResult Delete(int id)
        {
            specialLoanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);
            MiscellaneouService.Inactivate(id, null);
            return RedirectToAction("Index");
        }

        // POST: Miscellaneou/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                specialLoanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);
                MiscellaneouService.Inactivate(id, null);
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
