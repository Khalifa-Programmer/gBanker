using AutoMapper;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class MemberBlackListController : BaseController
    {
        private readonly IMemberBlackListService memberBlackListService;
        private readonly IOfficeService officeService;
        private readonly IMemberService memberService;
        private readonly ICenterService centerService;
        private readonly IAccReportService accReportService;
        private readonly IGroupwiseReportService groupwiseReportService;
        private readonly IUltimateReportService ultimateReportService;

        public MemberBlackListController(IMemberBlackListService memberBlackListService, IOfficeService officeService, IMemberService memberService, ICenterService centerService, IAccReportService accReportService, IGroupwiseReportService groupwiseReportService, IUltimateReportService ultimateReportService)
        {
            this.memberBlackListService = memberBlackListService;
            this.officeService = officeService;
            this.memberService = memberService;
            this.centerService = centerService;
            this.accReportService = accReportService;
            this.groupwiseReportService = groupwiseReportService;
            this.ultimateReportService = ultimateReportService;
        }

        //DropDown Map
        private void MapDropDownList(MemberBlackListViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }

            var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt16(LoggedInOrganizationID)); ;
            var viewCenter = allcenter.Select(x => x).ToList()
                .Select(x => new SelectListItem
                {
                    Text = string.Format("{0} - {1}", x.CenterCode, x.CenterName),
                    Value = x.CenterID.ToString()
                });
            var List = new List<SelectListItem>();
            List.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            List.AddRange(viewCenter);
            model.centerListItems = List;
        }
        //Main View
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new MemberBlackListViewModel();
            MapDropDownList(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(MemberBlackListViewModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var entity = new MemberBlackList
                    {
                        OfficeID = SessionHelper.LoginUserOfficeID,
                        MemberID = model.MemberID,
                        MemberCode = model.MemberCode,
                        PhoneNo = model.PhoneNo,
                        NationalID = model.NationalID,
                        SmartCard = model.SmartCard,
                        OtherIdNo = model.OtherIdNo,
                        IsActive = true,
                        CreateUser = SessionHelper.LoggedInEmployeeID.ToString(),
                        CreateDate = DateTime.Now
                    };
                    memberBlackListService.Create(entity);

                    return GetSuccessMessageResult();
                }
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: MemberBlackList/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = new MemberBlackListViewModel();
            var param = new { @MemberBlackListID = id };
            var entity = ultimateReportService.GetDataWithParameter(param, "Proc_GetMemberBlackListByID");


            var memberBlckList = entity.Tables[0].AsEnumerable()
                .Select(x => new MemberBlackListViewModel
                {
                    MemberBlackListID = x.Field<long>("MemberBlackListID"),
                    CenterID = x.Field<int>("CenterID"),
                    OfficeID = x.Field<int>("OfficeID"),
                    MemberID = x.Field<long>("MemberID"),
                    MemberCode = x.Field<string>("MemberCode"),
                    PhoneNo = x.Field<string>("PhoneNo"),
                    NationalID = x.Field<string>("NationalID"),
                    SmartCard = x.Field<string>("SmartCard"),
                    OtherIdNo = x.Field<string>("OtherIdNo"),
                }).ToList().FirstOrDefault();

            model.MemberBlackListID = memberBlckList.MemberBlackListID;
            model.CenterID = memberBlckList.CenterID;
            model.OfficeID = memberBlckList.OfficeID;
            model.MemberID = memberBlckList.MemberID;
            model.MemberCode = memberBlckList.MemberCode;
            model.PhoneNo = memberBlckList.PhoneNo;
            model.NationalID = memberBlckList.NationalID;
            model.SmartCard = memberBlckList.SmartCard;
            model.OtherIdNo = memberBlckList.OtherIdNo;

            var member = GetMember(Convert.ToInt64(model.MemberID));
            ViewBag.MemberName = string.Format("{0} - {1}", member.MemberCode, member.FirstName);

            MapDropDownList(model);

            return View(model);
        }

        // POST: MemberBlackList/Edit/5
        [HttpPost]
        public ActionResult Edit(MemberBlackListViewModel model, int id)
        {
            try
            {
                var entity = memberBlackListService.GetById(id);
                if (ModelState.IsValid)
                {
                    entity.MemberBlackListID = model.MemberBlackListID;
                    entity.OfficeID = model.OfficeID;
                    entity.MemberID = model.MemberID;
                    entity.MemberCode = model.MemberCode;
                    entity.PhoneNo = model.PhoneNo;
                    entity.NationalID = model.NationalID;
                    entity.SmartCard = model.SmartCard;
                    entity.OtherIdNo = model.OtherIdNo;
                    entity.UpdateUser = SessionHelper.LoggedInEmployeeID.ToString();
                    entity.UpdateDate = DateTime.Now;

                    memberBlackListService.Update(entity);
                    return GetSuccessMessageResult();
                }
                else
                {
                    return GetErrorMessageResult();
                }

            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        public Member GetMember(long memberid)
        {
            var mbr = memberService.GetByMemberId(memberid);
            return mbr;
        }


        public JsonResult DeleteMemberBlackList(int MemberBlackListID)
        {
            var result = "OK";
            try
            {
                var param = new { @MemberBlackListID = MemberBlackListID};

                if (MemberBlackListID >= 0)
                {
                    ultimateReportService.GetDataWithParameter(param, "Proc_DeleteMemberBlackList");
                }
                else
                {
                    return Json(new { Result = "ERROR" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: MemberBlackList
        public JsonResult GetMemberBlackList(int jtStartIndex, int jtPageSize, string jtSorting)
        {
            try
            {
                List<MemberBlackListViewModel> List_ViewModel = new List<MemberBlackListViewModel>();
                var memberBlackList = ultimateReportService.GetDataWithoutParameter("Proc_GetMemberBlackList");
                List_ViewModel = memberBlackList.Tables[0].AsEnumerable()
                    .Select(row => new MemberBlackListViewModel
                    {
                        MemberBlackListID = row.Field<long>("MemberBlackListID"),
                        CenterID = row.Field<int>("CenterID"),
                        Center = row.Field<string>("Center"),
                        MemberCode = row.Field<string>("MemberCode"),
                        MemberName = row.Field<string>("MemberName"),
                        PhoneNo = row.Field<string>("PhoneNo"),
                        NationalID = row.Field<string>("NationalID"),
                        SmartCard = row.Field<string>("SmartCard"),
                        OtherIdNo = row.Field<string>("OtherIdNo"),
                    }).ToList();

                var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //Get Member List
        public ActionResult GetMemberList(string memberid, string centerId)
        {
            var memberList = new List<Member>();
            var mbr = memberService.GetByCenterId(Convert.ToInt32(centerId), Convert.ToInt32(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID)).ToList();
            var members = mbr.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();
            return Json(members, JsonRequestBehavior.AllowGet);
        }
    }
}