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
    public class ZeroBalanceController : BaseController
    {

        private readonly ILoanTrxService loantrxService;
        
         private readonly IMemberService memberService;
         private readonly IOfficeService officeService;
         public ZeroBalanceController(ILoanTrxService loantrxService, IMemberService memberService, IOfficeService officeService)
        {
            this.loantrxService = loantrxService;
            this.memberService = memberService;
            this.officeService = officeService;
           
        }
         public ActionResult GetMemberList(string memberid, string oficeId)
         {
             //var MemberByOfficeSessionKey = string.Format("MemberByOfficeSessionKey_{0}", oficeId);
             var memberList = new List<Member>();
             //if (Session[MemberByOfficeSessionKey] != null)
             //    memberList = Session[MemberByOfficeSessionKey] as List<Member>;
             //else
             //{

             var mbr = memberService.SearchMember(int.Parse(oficeId), Convert.ToInt32(LoggedInOrganizationID));
             // Session[MemberByOfficeSessionKey] = mbr;
             memberList = mbr.ToList();
             //}
             var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + " " + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + " " + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + " " + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + " " + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

             return Json(members, JsonRequestBehavior.AllowGet);
         }
         private void MapDropDownList(GetZeroBalance model)
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



             var alloffice = officeService.GetAll().Where(l => l.OfficeID == SessionHelper.LoginUserOfficeID.Value && l.OrgID == LoggedInOrganizationID);

             var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

             model.officeListItems = viewOffice;

            var LoanStatusList = new List<SelectListItem>();
            LoanStatusList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            LoanStatusList.Add(new SelectListItem { Text = "With Loan", Value = "1" });
            LoanStatusList.Add(new SelectListItem { Text = "Without Loan", Value = "2" });
            LoanStatusList.Add(new SelectListItem { Text = "Both", Value = "3" });
            ViewData["LoanStatus"] = LoanStatusList;
        }
        public JsonResult GenerateZeroBalanceList(int jtStartIndex, int jtPageSize, string jtSorting, long MemberId, int LoanStatusType)
         {
             try
             {
                 //int writeoffyear = 0;

                 var allHistory = loantrxService.GeZeroBalanceList(SessionHelper.LoginUserOfficeID.Value, SessionHelper.LoginUserOfficeID.Value, MemberId, LoanStatusType);
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
        //
        // GET: /ZeroBalance/
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            var model = new GetZeroBalance();
            MapDropDownList(model);
            return View(model);
        }
        //
        // GET: /ZeroBalance/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /ZeroBalance/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ZeroBalance/Create
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
        // GET: /ZeroBalance/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /ZeroBalance/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ZeroBalance/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ZeroBalance/Delete/5
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
