using AutoMapper;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class SavingsInstallmentUpdateController : BaseController
    {
        private readonly IEmployeeService employeeService;
        private readonly IOfficeService officeService;
        private readonly ISavingSummaryService savingSummaryService;
        private readonly IProductService productService;
        private readonly IMemberCategoryService membercategoryService;
        private readonly ICenterService centerService;
        private readonly IPurposeService purposeService;
        private readonly IMemberService memberService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly ISpecialLoanCollectionService specialLoanCollectionService;
        public SavingsInstallmentUpdateController(ISavingSummaryService savingSummaryService, IProductService productService, IMemberCategoryService membercategoryService, IOfficeService officeService, ICenterService centerService, IPurposeService purposeService, IMemberService memberService, IUltimateReportService ultimateReportService, ISpecialLoanCollectionService specialLoanCollectionService, IEmployeeService employeeService)
        {
            this.savingSummaryService = savingSummaryService;
            this.productService = productService;
            this.membercategoryService = membercategoryService;
            this.officeService = officeService;
            this.centerService = centerService;
            this.purposeService = purposeService;
            this.memberService = memberService;
            this.ultimateReportService = ultimateReportService;
            this.specialLoanCollectionService = specialLoanCollectionService;
            this.employeeService = employeeService;

        }
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

            var allpurpose = purposeService.SearchPurpose(Convert.ToInt32(LoggedInOrganizationID));

            var viewPurpose = allpurpose.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.PurposeCode, m.PurposeName), Value = m.PurposeID.ToString() });

            model.purposeListItems = viewPurpose;

            var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt32(LoggedInOrganizationID));

            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });

            model.centerListItems = viewCenter;

            var allbranch = officeService.GetAll().Where(o => o.OfficeID == SessionHelper.LoginUserOfficeID.Value && o.OrgID == LoggedInOrganizationID);

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

            var allmembercategory = membercategoryService.GetAll().Where(m => m.OrgID == LoggedInOrganizationID);

            var viewmembercategory = allmembercategory.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCategoryID, m.CategoryName), Value = m.MemberCategoryID.ToString() });

            model.membercategoryListItems = viewmembercategory;

        }
        public ActionResult LedgerPost(SavingSummaryViewModel model)
        {
            var members = "Success";
            var val = savingSummaryService.SetOpeningSavingEntry(Convert.ToInt16(LoggedInOrganizationID), SessionHelper.LoginUserOfficeID.Value);
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
            var result = new { rate = vrate.ToString(), jdate = vdate.ToString("dd-MMM-yyyy") };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetEmployeeList(string memberid, string centerId)
        {
            var MemberByCenterSessionKey = string.Format("EmployeeByOfficeSessionKey_{0}", LoginUserOfficeID);
            var memberList = new List<Employee>();
            if (Session[MemberByCenterSessionKey] != null)
                memberList = Session[MemberByCenterSessionKey] as List<Employee>;
            else

            {
                var emr = employeeService.SearchEmployee().ToList();
                //var emr = employeeService.SearchEmployeeByOffice(Convert.ToInt16(LoginUserOfficeID)).ToList();
                Session[MemberByCenterSessionKey] = emr;
                memberList = emr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.EmployeeCode, (string.IsNullOrEmpty(m.EmpName) ? "" : m.EmpName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.EmployeeID, EmployeeName = string.Format("{0} - {1}", m1.EmployeeCode, (string.IsNullOrEmpty(m1.EmpName) ? "" : m1.EmpName)) }).ToList();

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
        public JsonResult GetBalance(decimal Deposit, decimal Withdrawal, decimal CumInterest, decimal MonthlyInterest, decimal Penalty)
        {
            decimal vBalance = (Deposit + CumInterest + Penalty) - (Withdrawal);
            var result = new { balance = vBalance.ToString() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSavingSummary(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                long totalCount;
                var allSavingsummary = savingSummaryService.GetSavingSummarySavingInterestUpdate(SessionHelper.LoginUserOfficeID.Value, filterColumn, filterValue, jtStartIndex, jtPageSize, out totalCount);
                var currentPageRecords = Mapper.Map<IEnumerable<DBSavingSummaryDetails>, IEnumerable<SavingSummaryViewModel>>(allSavingsummary);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        public JsonResult GetSavingReinstate(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue, DateTime DateFromValue, DateTime DateToValue)
        {
            try
            {
                long totalCount;
                var allSavingsummary = savingSummaryService.GetSavingReinstate(SessionHelper.LoginUserOfficeID.Value, filterColumn, filterValue, jtStartIndex, jtPageSize, out totalCount, DateFromValue, DateToValue);
                var currentPageRecords = Mapper.Map<IEnumerable<DBSavingSummaryDetails>, IEnumerable<SavingSummaryViewModel>>(allSavingsummary);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        public Member GetMember(long memberid)
        {
            var mbr = memberService.GetByIdLong(memberid);
            return mbr;
        }
        public Employee GetEmployeeBuro(int employeeid)
        {
            var mbr = employeeService.GetById(employeeid);
            return mbr;
        }

        public Center GetEmployee(int employeeid)
        {
            var mbr = centerService.GetById(employeeid);
            return mbr;
        }
        // GET: SavingsInstallmentUpdate
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SavingReinstate()
        {
            return View();
        }

        
        // GET: SavingsInstallmentUpdate/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SavingsInstallmentUpdate/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SavingsInstallmentUpdate/Create
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

        // GET: SavingsInstallmentUpdate/Edit/5
        public ActionResult Edit(int id)
        {
            if (savingSummaryService.IsContinued(id))
            {
                var savingsummary = savingSummaryService.GetById(id);

                var member = GetMember(Convert.ToInt64(savingsummary.MemberID));
                var employee = GetEmployeeBuro(Convert.ToInt16(savingsummary.Ref_EmployeeID));
                var entity = Mapper.Map<SavingSummary, SavingSummaryViewModel>(savingsummary);
                ViewBag.MemberName = string.Format("{0} - {1}", member.MemberCode, member.FirstName);
                ViewBag.employeeName = string.Format("{0} - {1}", employee.EmployeeCode, employee.EmpName);
                MapDropDownList(entity);

                return View(entity);
            }
            else
                ModelState.AddModelError("Validation", "Discontinued ID, please enter a diferent id and name.");
            return RedirectToAction("Index");
        }

        // POST: SavingsInstallmentUpdate/Edit/5
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
                        savingSum.SavingInstallment = entity.SavingInstallment;
                        savingSum.MaturedDate = entity.MaturedDate;
                    if (LoggedInOrganizationID==5)
                    {
                        savingSum.OpeningDate = entity.OpeningDate;
                    }
                    if (LoggedInOrganizationID == 29)
                    {
                        if (model.IsActive == true)
                        {
                            var param = new { @OrgID = LoggedInOrganizationID, @ProductID = savingSum.ProductID, @CenterID = savingSum.CenterID, @OfficeID = LoginUserOfficeID, @SavingInstallment = entity.SavingInstallment, @MaturedDate = entity.MaturedDate };
                            ultimateReportService.updateSavingInstallmentSap(param);
                        }
                        else
                        {
                            savingSum.Ref_EmployeeID = entity.Ref_EmployeeID;
                            savingSummaryService.Update(savingSum);
                        }
                    }
                    else
                    {
                        savingSum.Ref_EmployeeID = entity.Ref_EmployeeID;
                        savingSummaryService.Update(savingSum);
                    }
 
                        return GetSuccessMessageResult();
                }
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: SavingsInstallmentUpdate/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        public ActionResult SavingReinstateYes(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult SavingReinstateYes(long id, SavingSummaryViewModel model)
        {
            try
            {
                var entity = Mapper.Map<SavingSummaryViewModel, SavingSummary>(model);
                // TODO: Add delete logic here
                entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);

                var param = new { @OfficeId=LoginUserOfficeID, @MemberId="", @SavingSummaryID = id, @TransactionDate=TransactionDate, @CreateUser=LoggedInEmployeeID };
                ultimateReportService.SavingReinstate(param);
                return RedirectToAction("SavingReinstate");
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        // POST: SavingsInstallmentUpdate/Delete/5
        [HttpPost]
        public ActionResult Delete(long id, SavingSummaryViewModel model)
        {
            try
            {
                var entity = Mapper.Map<SavingSummaryViewModel, SavingSummary>(model);
                // TODO: Add delete logic here
                entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);

                var param = new { @SavingSummaryID = id };
                ultimateReportService.DelSavingSummary(param);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
    }
}
