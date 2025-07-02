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

namespace gBanker.Web.Controllers
{
    public class SavingSummaryController : BaseController
    {
        private readonly IOfficeService officeService;
        private readonly ISavingSummaryService savingSummaryService;
        private readonly IProductService productService;
        private readonly IMemberCategoryService membercategoryService;
        private readonly ICenterService centerService;
        private readonly IPurposeService purposeService;
        private readonly IMemberService memberService;
        // GET: SavingSummary
        public SavingSummaryController(ISavingSummaryService savingSummaryService, IProductService productService, IMemberCategoryService membercategoryService, IOfficeService officeService, ICenterService centerService, IPurposeService purposeService, IMemberService memberService)
        {
            this.savingSummaryService = savingSummaryService;
            this.productService = productService;
            this.membercategoryService = membercategoryService;
            this.officeService = officeService;
            this.centerService = centerService;
            this.purposeService = purposeService;
            this.memberService = memberService;

        }
        //Methods
        /// <Allmethods>
        private void MapDropDownList(SavingSummaryViewModel model)
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

            var allpurpose = purposeService.SearchPurpose( Convert.ToInt32(LoggedInOrganizationID));

            var viewPurpose = allpurpose.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.PurposeCode, m.PurposeName), Value = m.PurposeID.ToString() });

            model.purposeListItems = viewPurpose;

            var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID));

            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });

            model.centerListItems = viewCenter;

            var allbranch = officeService.GetAll().Where(o=>o.OfficeID==SessionHelper.LoginUserOfficeID.Value && o.OrgID==LoggedInOrganizationID);

            var viewbranch = allbranch.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewbranch;


            var allSearchProd = productService.SearchProduct(0, Convert.ToInt32(LoggedInOrganizationID),"S");
            var viewProdList = allSearchProd.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = string.Format("{0} - {1}", x.ProductCode.ToString(), x.ProductName.ToString())
            });
            var proditems = new List<SelectListItem>();
            proditems.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            proditems.AddRange(viewProdList);
            model.productListItems = proditems;
            
             var allmembercategory = membercategoryService.GetAll().Where(m=>m.OrgID==LoggedInOrganizationID);

            var viewmembercategory = allmembercategory.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCategoryID, m.CategoryName), Value = m.MemberCategoryID.ToString() });

            model.membercategoryListItems = viewmembercategory;

        }
        public ActionResult LedgerPost(SavingSummaryViewModel model)
        {
            var members = "Success";
            var val = savingSummaryService.SetOpeningSavingEntry(Convert.ToInt16(LoggedInOrganizationID),SessionHelper.LoginUserOfficeID.Value);
            return Json(members, JsonRequestBehavior.AllowGet);
        
        }
        public JsonResult GetRateAndDate(int productid, long memberId)
        {
            decimal vrate = 0;
            DateTime vdate = System.DateTime.Now;
        
            var pbr = productService.GetById(productid);
            var mbr = memberService.GetByIdLong(memberId);

            if (pbr != null)
            vrate = Convert.ToDecimal(pbr.InterestRate);
            
            if (mbr != null)
            vdate = mbr.JoinDate;
            var result = new { rate = vrate.ToString(), jdate = vdate.ToString("dd-MMM-yyyy")};
            return Json(result, JsonRequestBehavior.AllowGet);
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
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //var allSaving = savingSummaryService.GetAll();

            //var viewSavingsummary = Mapper.Map<IEnumerable<SavingSummary>, IEnumerable<SavingSummaryViewModel>>(allSaving);

            //return View(viewSavingsummary);
            return View();
        }
        public JsonResult GetBalance(decimal Deposit, decimal Withdrawal, decimal CumInterest, decimal MonthlyInterest, decimal Penalty)
        {
            decimal vBalance = (Deposit + CumInterest +Penalty) - (Withdrawal);
            var result = new { balance = vBalance.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSavingSummary(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                long totalCount;
                //var allSavingsummary = savingSummaryService.GetSavingSummaryDetail(SessionHelper.LoginUserOfficeID.Value, filterColumn, filterValue);
                var allSavingsummary = savingSummaryService.GetSavingSummaryDetailPaged(SessionHelper.LoginUserOfficeID.Value, filterColumn, filterValue, jtStartIndex, jtPageSize, out totalCount);
                //var totalCount = allSavingsummary.Count();
                //var entities = allSavingsummary.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<DBSavingSummaryDetails>, IEnumerable<SavingSummaryViewModel>>(allSavingsummary);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });


                //var entities = Mapper.Map<IEnumerable<DBSavingSummaryDetails>, IEnumerable<SavingSummaryViewModel>>(allSavingsummary);
                //return Json(new { Result = "OK", Records = entities });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        // GET: SavingSummary/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
        // GET: SavingSummary/Create
        public ActionResult Create()
        {
            var model = new SavingSummaryViewModel();
            MapDropDownList(model);
            return View(model);
        }
        // POST: SavingSummary/Create
        [HttpPost]
        public ActionResult Create(SavingSummaryViewModel model)
        {
            try
            {
                var entity = Mapper.Map<SavingSummaryViewModel, SavingSummary>(model);

                //Add Validlation Logic.

                if (ModelState.IsValid)
                {

                    var member = GetMember(Convert.ToInt64(entity.MemberID));
                    int membercaregoryid = 0;
                    if(member!=null)
                        membercaregoryid = member.MemberCategoryID;

                        var employee = GetEmployee(entity.CenterID);
                        int employeeid = employee.EmployeeId;
                        entity.EmployeeId = Convert.ToInt16(employeeid);
                        entity.MemberCategoryID = Convert.ToByte(membercaregoryid);

                        entity.IsActive = true;
                        entity.SavingStatus = 1;
                        entity.TransType = 0;
                        var errors = savingSummaryService.IsValidSaving(entity);

                    if (errors.ToList().Count == 0)
                    {
                        entity.OrgID =  Convert.ToInt16(LoggedInOrganizationID);
                        savingSummaryService.Create(entity);
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
        // GET: SavingSummary/Edit/5
        public ActionResult Edit(long id)
        {
            if (savingSummaryService.IsContinued(id))
            {
                var savingsummary = savingSummaryService.GetByIdLong(id);

                var member = GetMember(Convert.ToInt64(savingsummary.MemberID));
                var entity = Mapper.Map<SavingSummary, SavingSummaryViewModel>(savingsummary);
                ViewBag.MemberName = string.Format("{0} - {1}", member.MemberCode, member.FirstName);
                MapDropDownList(entity);

                return View(entity);
            }
            else
                ModelState.AddModelError("Validation", "Discontinued ID, please enter a diferent id and name.");
                return RedirectToAction("Index");
            
        }
        // POST: SavingSummary/Edit/5
        [HttpPost]
        public ActionResult Edit(long id, SavingSummaryViewModel model)
        {
            try
            {


                var entity = Mapper.Map<SavingSummaryViewModel, SavingSummary>(model);
                var savingSum = savingSummaryService.GetByIdLong(Convert.ToInt64(entity.SavingSummaryID));
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    string msg = "";
                    var errors = savingSummaryService.IsValidSavingForEdit(entity);
                    if (errors.ToList().Count == 0)
                    {
                       
                        savingSum.ProductID = entity.ProductID;
                        savingSum.NoOfAccount = entity.NoOfAccount;


                        savingSum.TransactionDate = entity.TransactionDate; ;
                        savingSum.Deposit = entity.Deposit;
                        savingSum.Withdrawal = entity.Withdrawal;
                        savingSum.Balance = entity.Balance;
                        savingSum.InterestRate = entity.InterestRate;

                        savingSum.SavingInstallment = entity.SavingInstallment;
                        savingSum.CumInterest = entity.CumInterest;
                        savingSum.MonthlyInterest = entity.MonthlyInterest;
                        savingSum.Penalty = entity.Penalty;
                        savingSum.OpeningDate = entity.OpeningDate;
                        savingSum.MaturedDate = entity.MaturedDate;
                        savingSummaryService.Update(savingSum);
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
        // GET: SavingSummary/Delete/5
        public ActionResult Delete(long id)
        {
            savingSummaryService.Inactivate(id, null);
            return RedirectToAction("Index");
        }
        // POST: SavingSummary/Delete/5
        [HttpPost]
        public ActionResult Delete(long id, FormCollection collection)
        {
            try
            {
                savingSummaryService.Inactivate(id, null);
                return RedirectToAction("Index");
                // TODO: Add delete logic here

               // return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
