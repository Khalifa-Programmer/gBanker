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
using gBanker.Data.CodeFirstMigration;

namespace gBanker.Web.Controllers
{
    
    public class MemberLedgerController : BaseController
    {
        private readonly IOfficeService officeService;
        private readonly IMemberService memberService;
        private readonly ICenterService centerService;
        private readonly IMemberLedgerService memberledgerService;
        
public MemberLedgerController(IOfficeService officeService, ICenterService centerService, IMemberService memberService, IMemberLedgerService memberledgerService)
          {
              
              this.officeService = officeService;
              this.centerService = centerService;
              this.memberledgerService = memberledgerService;
              this.memberService = memberService;
            

          }
        private void MapDropDownList(MemberLedgerViewModel model)
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



            var alloffice = officeService.GetAll().Where(l => l.OfficeID == SessionHelper.LoginUserOfficeID.Value && l.OrgID==LoggedInOrganizationID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;

            //var alloffice = officeService.GetAll().Where(l => l.OfficeID == SessionHelper.LoginUserOfficeID.Value);

            //var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

          

        }
        public ActionResult GetMemberList(string memberid, string oficeId)
        {
            //var MemberByOfficeSessionKey = string.Format("MemberByOfficeSessionKey_{0}", oficeId);
            var memberList = new List<Member>();
            //if (Session[MemberByOfficeSessionKey] != null)
            //    memberList = Session[MemberByOfficeSessionKey] as List<Member>;
            //else
            //{

            var mbr = memberService.SearchMember(int.Parse(oficeId), Convert.ToInt16(LoggedInOrganizationID));
               // Session[MemberByOfficeSessionKey] = mbr;
                memberList = mbr.ToList();
            //}
                var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + " " + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + " " + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + " " + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + " " + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetMemberList(string memberid)
        //{

        //    //var MemberByCenterSessionKey = string.Format("MemberByCenterSessionKey_{0}", LoginUserOfficeID);
        //    //var memberList = new List<Member>();
        //    //if (Session[MemberByCenterSessionKey] != null)
        //    //    memberList = Session[MemberByCenterSessionKey] as List<Member>;
        //    //else
        //    //{
        //    //    var mbr = memberService.GetByCenterId(int.Parse(centerId)).ToList();
        //    //    Session[MemberByCenterSessionKey] = mbr;
        //    //    memberList = mbr;
        //    //}
        //    //var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, m.FirstName).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, m1.FirstName) }).ToList();

        //    //return Json(members, JsonRequestBehavior.AllowGet);


        //    var memberList = new List<Member>();
        //    IEnumerable<Member> mbr;
        //    mbr = memberService.GetAll().Where(m => m.OfficeID == LoginUserOfficeID);

        //    memberList = mbr.ToList();

        //    var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, m.FirstName).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, m1.FirstName) }).ToList();


        //    return Json(members, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult GetMemberLedger(int jtStartIndex, int jtPageSize, string jtSorting,int MemberId)
        {
            try
            {
                
                var allSavingsummary = memberledgerService.getGetLoanLedgerMemberWise(Convert.ToInt16( LoggedInOrganizationID),SessionHelper.LoginUserOfficeID.Value, MemberId);
                var detail = allSavingsummary.ToList();
                var totCount = detail.Count();
                var currentPageRecords = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totCount });
               
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        // GET: MemberLedger
        public ActionResult Index()
        {

            var model = new MemberLedgerViewModel();
            MapDropDownList(model);
            return View(model);
        }

        // GET: MemberLedger/Details/5
        public ActionResult Details(int id)
        {
           
            return View();
        }

        // GET: MemberLedger/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MemberLedger/Create
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

        // GET: MemberLedger/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MemberLedger/Edit/5
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

        // GET: MemberLedger/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MemberLedger/Delete/5
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
