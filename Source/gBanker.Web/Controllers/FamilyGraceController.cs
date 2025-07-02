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
    public class FamilyGraceController : BaseController
    {
       
        private readonly IOfficeService officeService;

        private readonly IFamilyGraceService familyGraceService;
     
        private readonly ICenterService centerService;
       
        private readonly IMemberService memberService;
        public FamilyGraceController(IFamilyGraceService familyGraceService, IOfficeService officeService, ICenterService centerService, IMemberService memberService)
          {
              
              this.officeService = officeService;
              this.centerService = centerService;
              this.familyGraceService = familyGraceService;
              this.memberService = memberService;
             

          }
        private void MapDropDownList(FamilyGraceViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }
            //var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID));

            //var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)), Value = m.MemberID.ToString() });

            //model.memberListItems = viewMember;
            //ViewData["Member"] = viewMember;

            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID));

            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)), Value = m.MemberID.ToString() });

            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;

            var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt16(LoggedInOrganizationID)); ;

            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });

            model.centerListItems = viewCenter;

            var alloffice = officeService.GetAll().Where(l => l.OfficeID == SessionHelper.LoginUserOfficeID.Value && l.OrgID==LoggedInOrganizationID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;

        
          

        }
        public Member GetMember(Int64 memberid)
        {
            var mbr = memberService.GetByMemberId(Convert.ToInt64(memberid));
            return mbr;
        }
        public ActionResult GetFamilyGrace(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                //if (IsDayInitiated)
                //{
                    var familygraceDetail = familyGraceService.GetFamilyGraceDetail(LoggedInOrganizationID,LoginUserOfficeID);
                    var detail = familygraceDetail.ToList();
                    var totalCount = detail.Count();
                    var entities = detail.Skip(jtStartIndex).Take(jtPageSize);
                    var currentPageRecords = Mapper.Map<IEnumerable<getFamilyGrace_Result>, IEnumerable<FamilyGraceViewModel>>(entities);

                    return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });
                //}
                return Json(new { Result = "OK", Records = new List<FamilyGraceViewModel>(), TotalRecordCount = 0 });
                //var viewploansummary = Mapper.Map<IEnumerable<Proc_get_LoanDisburse_Result>, IEnumerable<LoanDisburseViewModel>>(detail);

                //return Json(new { Result = "OK", Records = viewploansummary });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

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
        // GET: FamilyGrace
        public ActionResult Index()
        {
            return View();
        }

        // GET: FamilyGrace/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FamilyGrace/Create
        public ActionResult Create()
        {
            var model = new FamilyGraceViewModel();
            if (IsDayInitiated)
                model.GraceEndDate = TransactionDate;
            model.GraceStartDate = TransactionDate;
            MapDropDownList(model);

            return View(model);
        }

        // POST: FamilyGrace/Create
        [HttpPost]
        public ActionResult Create(FamilyGraceViewModel model, FormCollection form)
        {
            try
            {

                var entity = Mapper.Map<FamilyGraceViewModel, FamilyGrace>(model);

                //Add Validlation Logic.

                if (ModelState.IsValid)
                {
                  
                   

                    entity.IsActive = true;
                    entity.CreateUser = "d";
                    entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                    var errors = familyGraceService.IsValidRecord(entity);

                    if (errors.ToList().Count == 0)
                    {
                       
                        entity.GraceStartDate = Convert.ToDateTime(model.GraceStartDate);
                        entity.GraceEndDate = Convert.ToDateTime(model.GraceEndDate);
                       // entity.InActiveDate = Convert.ToDateTime("01 Jun 2015");
                        entity.CreateDate = Convert.ToDateTime("31 Jul 2015");
                        entity.Description = model.Description;
                        familyGraceService.Create(entity);
                        return GetSuccessMessageResult();
                    }

                    else
                    {
                      
                        return GetErrorMessageResult(errors);
                    }
                }
                return GetErrorMessageResult();

            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: FamilyGrace/Edit/5
        public ActionResult Edit(int id)
        {
            var loanapproval = familyGraceService.GetById(id);

            var member = GetMember(Convert.ToInt64(loanapproval.MemberID));
            var entity = Mapper.Map<FamilyGrace, FamilyGraceViewModel>(loanapproval);
           ViewBag.MemberName = string.Format("{0} - {1}", member.MemberCode, member.FirstName);
            MapDropDownList(entity);

            return View(entity);

            
        }

        // POST: FamilyGrace/Edit/5
        [HttpPost]
        public ActionResult Edit(FamilyGraceViewModel model)
        {
            try
            {

                var entity = Mapper.Map<FamilyGraceViewModel, FamilyGrace>(model);
                var getFamilygraceDetails = familyGraceService.GetById(entity.FamilyGraceID);
                //// TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    getFamilygraceDetails.CenterID = entity.CenterID;
                    getFamilygraceDetails.Description = entity.Description;
                    getFamilygraceDetails.GraceEndDate = entity.GraceEndDate;
                    getFamilygraceDetails.GraceStartDate = entity.GraceStartDate;
                    getFamilygraceDetails.MemberID = entity.MemberID;

                    familyGraceService.Update(getFamilygraceDetails);
                    return GetSuccessMessageResult();

                }
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: FamilyGrace/Delete/5
        public ActionResult Delete(int id)
        {
            familyGraceService.Inactivate(id, null);
            return RedirectToAction("Index");
        }

        // POST: FamilyGrace/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                familyGraceService.Inactivate(id, null);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
