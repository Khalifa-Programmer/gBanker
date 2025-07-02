using AutoMapper;
//using gBanker.Data.Db;
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
using System.Data;

namespace gBanker.Web.Controllers
{
    public class MemberInfoUpdateController : BaseController
    {

        #region Variables
        private readonly ICenterService centerService;
        private readonly IOfficeService officeService;
        private readonly IMemberService memberService;
        private readonly IGroupService groupService;
        private readonly ILoanSummaryService loanSummaryService;
        private readonly IMemberLastCodeService memberLastCodeService;
        private readonly IAccReportService accReportService;
        private readonly IProcessInfoService processInfoService;
        public MemberInfoUpdateController(IOfficeService officeService, ICenterService centerService, IMemberService memberService, ILoanSummaryService loanSummaryService, IGroupService groupService, IMemberLastCodeService memberLastCodeService, IAccReportService accReportService, IProcessInfoService processInfoService)
        {
            this.officeService = officeService;
            this.centerService = centerService;
            this.memberService = memberService;
            this.groupService = groupService;
            this.loanSummaryService = loanSummaryService;
            this.memberLastCodeService = memberLastCodeService;
            this.accReportService = accReportService;
            this.processInfoService = processInfoService;
        }
        #endregion

 


        #region Methods
        private void MapDropDownList(LoaneeTransferViewModel model)
        {
            var allCenter = centerService.GetAll().Where(w => w.IsActive == true && w.OfficeID == Convert.ToInt32(SessionHelper.LoginUserOfficeID) && w.OrgID == Convert.ToInt32(LoggedInOrganizationID)).OrderBy(o => o.CenterCode);
            var viewCenter = allCenter.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterCode.ToString() + ", " + x.CenterName.ToString()
            });
            var center_items = new List<SelectListItem>();
            center_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            center_items.AddRange(viewCenter);
            model.CenterList = center_items;

            var allGroup = groupService.GetAll().Where(w => w.IsActive == true && w.OfficeID == Convert.ToInt32(SessionHelper.LoginUserOfficeID) && w.OrgID == Convert.ToInt32(LoggedInOrganizationID));
            var viewGroup = allGroup.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.GroupID.ToString(),
                Text = x.GroupCode.ToString()
            });
            var group_items = new List<SelectListItem>();
            group_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            group_items.AddRange(viewGroup);
            model.GroupList = group_items;

        }
        public JsonResult GetLoaneeInfo(int jtStartIndex, int jtPageSize, string jtSorting, string CenterId, string GroupId, string MemberId)
        {
            try
            {
                int Center_Id = Convert.ToInt32(CenterId);
                int Group_Id = Convert.ToInt32(string.IsNullOrEmpty(GroupId) ? "0" : GroupId);
                int Member_Id = Convert.ToInt32(string.IsNullOrEmpty(MemberId) ? "0" : MemberId);

                long TotCount;
                //1 0 0

                if (Center_Id > 0 && Group_Id == 0 && Member_Id == 0)
                {


                    List<DBMemberDetailModel> loaneeDetail = new List<DBMemberDetailModel>();
                    var param = new { @Qtype = 1, @OrgID = SessionHelper.LoginUserOrganizationID, @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = Center_Id, @MemberID = Member_Id, @GroupID = Group_Id };
                    var ArrearItem = accReportService.GetMemberDetailBN(param);

                    loaneeDetail = ArrearItem.Tables[0].AsEnumerable()
                    .Select(row => new DBMemberDetailModel
                    {
                        MemberID = row.Field<long>("MemberID"),
                        MemberCode = row.Field<string>("MemberCode"),
                        DivisionCode = row.Field<string>("DivisionCode"),
                        PhoneNo =  row.Field<string>("PhoneNo"),
                        MemberNameBng = row.Field<string>("MemberNameBng"),

                        OfficeID = row.Field<int>("OfficeID"),
                        OfficeCode = row.Field<string>("OfficeCode"),
                        OfficeName = row.Field<string>("OfficeName"),
                        CenterID = row.Field<int>("CenterID"),
                        CenterCode = row.Field<string>("CenterCode"),
                        CenterName = row.Field<string>("CenterName"),
                        GroupID = row.Field<short>("GroupID"),
                        GroupCode = row.Field<string>("GroupCode"),
                        //FullName = string.Format("{0} {1} {2}", s.FirstName,s.MiddleName,s.LastName),
                        FullName = row.Field<string>("FullName"),
                        FirstName = row.Field<string>("FirstName"),
                        MiddleName = row.Field<string>("MiddleName"),
                        LastName = row.Field<string>("LastName"),
                        AddressLine1 = row.Field<string>("AddressLine1"),
                        AddressLine2 = row.Field<string>("AddressLine2"),
                        RefereeName = row.Field<string>("RefereeName"),

                        Gender = row.Field<string>("Gender"),
                        NationalID = row.Field<string>("NationalID"),
                        Location = row.Field<string>("Location"),

                        MemberCategoryID = row.Field<byte>("MemberCategoryID"),
                        MemberCategoryCode = row.Field<string>("CategoryShortName"),
                        CategoryName = row.Field<string>("CategoryName")
                    }).ToList();


                    var detail = loaneeDetail.ToList();
                    var totCountq = detail.Count();
                    var currentPageCodes = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                    return Json(new { Result = "OK", Records = currentPageCodes, TotalRecordCount = totCountq });
                }
                //1 0 1
                else if (Center_Id > 0 && Group_Id == 0 && Member_Id > 0)
                {
                    List<DBMemberDetailModel> loaneeDetail = new List<DBMemberDetailModel>();
                    var param = new { @Qtype = 2, @OrgID = SessionHelper.LoginUserOrganizationID, @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = Center_Id, @MemberID = Member_Id, @GroupID = Group_Id };
                    var ArrearItem = accReportService.GetMemberDetail(param);



                    loaneeDetail = ArrearItem.Tables[0].AsEnumerable()
                    .Select(row => new DBMemberDetailModel
                    {
                        MemberID = row.Field<long>("MemberID"),
                        MemberCode = row.Field<string>("MemberCode"),
                        DivisionCode = row.Field<string>("DivisionCode"),
                        MemberNameBng = row.Field<string>("MemberNameBng"),
                        PhoneNo = row.Field<string>("PhoneNo"),

                        OfficeID = row.Field<int>("OfficeID"),
                        OfficeCode = row.Field<string>("OfficeCode"),
                        OfficeName = row.Field<string>("OfficeName"),
                        CenterID = row.Field<int>("CenterID"),
                        CenterCode = row.Field<string>("CenterCode"),
                        CenterName = row.Field<string>("CenterName"),
                        GroupID = row.Field<short>("GroupID"),
                        GroupCode = row.Field<string>("GroupCode"),
                        //FullName = string.Format("{0} {1} {2}", s.FirstName,s.MiddleName,s.LastName),
                        FullName = row.Field<string>("FullName"),
                        FirstName = row.Field<string>("FirstName"),
                        MiddleName = row.Field<string>("MiddleName"),
                        LastName = row.Field<string>("LastName"),
                        AddressLine1 = row.Field<string>("AddressLine1"),
                        AddressLine2 = row.Field<string>("AddressLine2"),
                        RefereeName = row.Field<string>("RefereeName"),

                        Gender = row.Field<string>("Gender"),
                        NationalID = row.Field<string>("NationalID"),
                        Location = row.Field<string>("Location"),

                        MemberCategoryID = row.Field<byte>("MemberCategoryID"),
                        MemberCategoryCode = row.Field<string>("CategoryShortName"),
                        CategoryName = row.Field<string>("CategoryName")
                    }).ToList();


                    var detail = loaneeDetail.ToList();
                    var totCountq = detail.Count();

                    var currentPageCodes = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                    return Json(new { Result = "OK", Records = currentPageCodes, TotalRecordCount = totCountq });
                }
                //1 1 0
                else if (Center_Id > 0 && Group_Id > 0 && Member_Id == 0)
                {
                    List<DBMemberDetailModel> loaneeDetail = new List<DBMemberDetailModel>();
                    var param = new { @Qtype = 3, @OrgID = SessionHelper.LoginUserOrganizationID, @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = Center_Id, @MemberID = Member_Id, @GroupID = Group_Id };
                    var ArrearItem = accReportService.GetMemberDetail(param);



                    loaneeDetail = ArrearItem.Tables[0].AsEnumerable()
                    .Select(row => new DBMemberDetailModel
                    {
                        MemberID = row.Field<long>("MemberID"),
                        MemberCode = row.Field<string>("MemberCode"),
                        DivisionCode = row.Field<string>("DivisionCode"),
                        MemberNameBng = row.Field<string>("MemberNameBng"),
                        PhoneNo = row.Field<string>("PhoneNo"),

                        OfficeID = row.Field<int>("OfficeID"),
                        OfficeCode = row.Field<string>("OfficeCode"),
                        OfficeName = row.Field<string>("OfficeName"),
                        CenterID = row.Field<int>("CenterID"),
                        CenterCode = row.Field<string>("CenterCode"),
                        CenterName = row.Field<string>("CenterName"),
                        GroupID = row.Field<short>("GroupID"),
                        GroupCode = row.Field<string>("GroupCode"),
                        //FullName = string.Format("{0} {1} {2}", s.FirstName,s.MiddleName,s.LastName),
                        FullName = row.Field<string>("FullName"),
                        FirstName = row.Field<string>("FirstName"),
                        MiddleName = row.Field<string>("MiddleName"),
                        LastName = row.Field<string>("LastName"),
                        AddressLine1 = row.Field<string>("AddressLine1"),
                        AddressLine2 = row.Field<string>("AddressLine2"),
                        RefereeName = row.Field<string>("RefereeName"),

                        Gender = row.Field<string>("Gender"),
                        NationalID = row.Field<string>("NationalID"),
                        Location = row.Field<string>("Location"),

                        MemberCategoryID = row.Field<byte>("MemberCategoryID"),
                        MemberCategoryCode = row.Field<string>("CategoryShortName"),
                        CategoryName = row.Field<string>("CategoryName")
                    }).ToList();


                    var detail = loaneeDetail.ToList();
                    var totCountq = detail.Count();
                    var currentPageCodes = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                    return Json(new { Result = "OK", Records = currentPageCodes, TotalRecordCount = totCountq });
                }
                //1 1 1
                else if (Center_Id > 0 && Group_Id > 0 && Member_Id > 0)
                {
                    List<DBMemberDetailModel> loaneeDetail = new List<DBMemberDetailModel>();
                    var param = new { @Qtype = 4, @OrgID = SessionHelper.LoginUserOrganizationID, @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = Center_Id, @MemberID = Member_Id, @GroupID = Group_Id };
                    var ArrearItem = accReportService.GetMemberDetail(param);



                    loaneeDetail = ArrearItem.Tables[0].AsEnumerable()
                    .Select(row => new DBMemberDetailModel
                    {
                        MemberID = row.Field<long>("MemberID"),
                        MemberCode = row.Field<string>("MemberCode"),
                        DivisionCode = row.Field<string>("DivisionCode"),
                        MemberNameBng = row.Field<string>("MemberNameBng"),
                        PhoneNo = row.Field<string>("PhoneNo"),

                        OfficeID = row.Field<int>("OfficeID"),
                        OfficeCode = row.Field<string>("OfficeCode"),
                        OfficeName = row.Field<string>("OfficeName"),
                        CenterID = row.Field<int>("CenterID"),
                        CenterCode = row.Field<string>("CenterCode"),
                        CenterName = row.Field<string>("CenterName"),
                        GroupID = row.Field<short>("GroupID"),
                        GroupCode = row.Field<string>("GroupCode"),
                        //FullName = string.Format("{0} {1} {2}", s.FirstName,s.MiddleName,s.LastName),
                        FullName = row.Field<string>("FullName"),
                        FirstName = row.Field<string>("FirstName"),
                        MiddleName = row.Field<string>("MiddleName"),
                        LastName = row.Field<string>("LastName"),
                        AddressLine1 = row.Field<string>("AddressLine1"),
                        AddressLine2 = row.Field<string>("AddressLine2"),
                        RefereeName = row.Field<string>("RefereeName"),

                        Gender = row.Field<string>("Gender"),
                        NationalID = row.Field<string>("NationalID"),
                        Location = row.Field<string>("Location"),

                        MemberCategoryID = row.Field<byte>("MemberCategoryID"),
                        MemberCategoryCode = row.Field<string>("CategoryShortName"),
                        CategoryName = row.Field<string>("CategoryName")
                    }).ToList();


                    var detail = loaneeDetail.ToList();
                    var totCountq = detail.Count();
                    var currentPageCodes = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                    return Json(new { Result = "OK", Records = currentPageCodes, TotalRecordCount = totCountq });


                }
                else
                {
                    List<DBMemberDetailModel> loaneeDetail = new List<DBMemberDetailModel>();
                    var param = new { @Qtype = 4, @OrgID = SessionHelper.LoginUserOrganizationID, @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = Center_Id, @MemberID = Member_Id, @GroupID = Group_Id };
                    var ArrearItem = accReportService.GetMemberDetail(param);



                    loaneeDetail = ArrearItem.Tables[0].AsEnumerable()
                    .Select(row => new DBMemberDetailModel
                    {
                        MemberID = row.Field<long>("MemberID"),
                        MemberCode = row.Field<string>("MemberCode"),
                        DivisionCode = row.Field<string>("DivisionCode"),
                        MemberNameBng = row.Field<string>("MemberNameBng"),
                        PhoneNo = row.Field<string>("PhoneNo"),

                        OfficeID = row.Field<int>("OfficeID"),
                        OfficeCode = row.Field<string>("OfficeCode"),
                        OfficeName = row.Field<string>("OfficeName"),
                        CenterID = row.Field<int>("CenterID"),
                        CenterCode = row.Field<string>("CenterCode"),
                        CenterName = row.Field<string>("CenterName"),
                        GroupID = row.Field<short>("GroupID"),
                        GroupCode = row.Field<string>("GroupCode"),
                        //FullName = string.Format("{0} {1} {2}", s.FirstName,s.MiddleName,s.LastName),
                        FullName = row.Field<string>("FullName"),
                        FirstName = row.Field<string>("FirstName"),
                        MiddleName = row.Field<string>("MiddleName"),
                        LastName = row.Field<string>("LastName"),
                        AddressLine1 = row.Field<string>("AddressLine1"),
                        AddressLine2 = row.Field<string>("AddressLine2"),
                        RefereeName = row.Field<string>("RefereeName"),

                        Gender = row.Field<string>("Gender"),
                        NationalID = row.Field<string>("NationalID"),
                        Location = row.Field<string>("Location"),

                        MemberCategoryID = row.Field<byte>("MemberCategoryID"),
                        MemberCategoryCode = row.Field<string>("CategoryShortName"),
                        CategoryName = row.Field<string>("CategoryName")

                    }).ToList();


                    var detail = loaneeDetail.ToList();
                    var totCountq = detail.Count();
                    var currentPageCodes = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                    return Json(new { Result = "OK", Records = currentPageCodes, TotalRecordCount = totCountq });
                }


                ///////////////////////---------------------------------
                //if (Center_Id > 0 && Group_Id == 0 && Member_Id == 0)
                //{




                //    var loaneeDetail = memberService.GetApprovedMemberForTransfer(LoggedInOrganizationID, SessionHelper.LoginUserOfficeID.Value, out TotCount).Where(w => w.CenterID == Center_Id);
                //    var detail = loaneeDetail.ToList();
                //    var totCountq = detail.Count();
                //    var currentPageCodes = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                //    return Json(new { Result = "OK", Records = currentPageCodes, TotalRecordCount = totCountq });
                //}
                ////1 0 1
                //else if (Center_Id > 0 && Group_Id == 0 && Member_Id > 0)
                //{
                //    var loaneeDetail = memberService.GetApprovedMemberForTransfer(LoggedInOrganizationID, SessionHelper.LoginUserOfficeID.Value, out TotCount).Where(w => w.CenterID == Center_Id && w.MemberID == Member_Id);
                //    var detail = loaneeDetail.ToList();
                //    //var totCount = detail.Count();
                //    var currentPageCodes = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                //    return Json(new { Result = "OK", Records = currentPageCodes, TotalRecordCount = TotCount });
                //}
                ////1 1 0
                //else if (Center_Id > 0 && Group_Id > 0 && Member_Id == 0)
                //{
                //    var loaneeDetail = memberService.GetApprovedMemberForTransfer(LoggedInOrganizationID, SessionHelper.LoginUserOfficeID.Value, out TotCount).Where(w => w.CenterID == Center_Id && w.GroupID == Group_Id);
                //    var detail = loaneeDetail.ToList();
                //    //var totCount = detail.Count();
                //    var currentPageCodes = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                //    return Json(new { Result = "OK", Records = currentPageCodes, TotalRecordCount = TotCount });
                //}
                ////1 1 1
                //else if (Center_Id > 0 && Group_Id > 0 && Member_Id > 0)
                //{
                //    var loaneeDetail = memberService.GetApprovedMemberForTransfer(LoggedInOrganizationID, SessionHelper.LoginUserOfficeID.Value, out TotCount).Where(w => w.CenterID == Center_Id && w.GroupID == Group_Id && w.MemberID == Member_Id);
                //    var detail = loaneeDetail.ToList();
                //    //var totCount = detail.Count();
                //    var currentPageCodes = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                //    return Json(new { Result = "OK", Records = currentPageCodes, TotalRecordCount = TotCount });
                //}
                //else
                //{
                //    var loaneeDetail = memberService.GetApprovedMemberForTransfer(LoggedInOrganizationID, SessionHelper.LoginUserOfficeID.Value, out TotCount);
                //    var detail = loaneeDetail.ToList();
                //    //var totCount = detail.Count();
                //    var currentPageCodes = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                //    return Json(new { Result = "OK", Records = currentPageCodes, TotalRecordCount = TotCount });
                //}


                //GetApprovedMemberForTransfer
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

       


        public JsonResult saveEmpBnName(string MemberId, string bnName)
        {
            long Member_Id = 0;
            if (Convert.ToInt64(MemberId) > 0 && bnName != "")
            {
                var Member = memberService.GetByIdLong(Convert.ToInt64(MemberId));
                Member.MemberNameBng = bnName;

                memberService.Update(Member);
                Member_Id = Convert.ToInt64(Member.MemberID);
            }
            return Json(Member_Id, JsonRequestBehavior.AllowGet);
        }

        public JsonResult savePhoneNo(string MemberId, string Phone)
        {
            long Member_Id = 0;
            if (Convert.ToInt64(MemberId) > 0 && Phone != "")
            {
                //var AllMember = memberService.GetAll().Where(x=> x.IsActive == true && x.PhoneNo ==  Phone && x.MemberID != Convert.ToInt64(MemberId)); //memberService.CheckMemberPhoneNo(Phone);
                //if (AllMember != null)
                //{
                //    return Json("This Phone No Already Exist.", JsonRequestBehavior.AllowGet);
                //}
                var Member = memberService.GetByIdLong(Convert.ToInt64(MemberId));
                Member.PhoneNo = Phone;

                memberService.Update(Member);
                Member_Id = Convert.ToInt64(Member.MemberID);
            }
            return Json(Member_Id, JsonRequestBehavior.AllowGet);
        }

        public JsonResult saveNID(string MemberId, string NId)
        {
            long Member_Id = 0;
            if (Convert.ToInt64(MemberId) > 0 && NId != "")
            {
                var Member = memberService.GetByIdLong(Convert.ToInt64(MemberId));
                Member.NationalID = NId;

                memberService.Update(Member);
                Member_Id = Convert.ToInt64(Member.MemberID);
            }
            return Json(Member_Id, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult GetOfficeList()
        {


            try
            {
                var offices = officeService.GetAll().Where(w => w.OfficeLevel == 4 && w.IsActive == true && w.OrgID == LoggedInOrganizationID).Select(c => new { DisplayText = c.OfficeCode + ", " + c.OfficeName, Value = c.OfficeID });
                return Json(new { Result = "OK", Options = offices });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult GetOfficeListSelect()
        {


            try
            {
                var offices = officeService.GetAll().Where(w => w.OfficeLevel == 4 && w.IsActive == true && w.OrgID == LoggedInOrganizationID && w.OfficeID == LoginUserOfficeID).Select(c => new { DisplayText = c.OfficeCode + ", " + c.OfficeName, Value = c.OfficeID });
                return Json(new { Result = "OK", Options = offices });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult GetCenterList(string officeid)
        {


            try
            {
                var centers = centerService.GetAll().Where(w => w.IsActive == true && w.OfficeID == Convert.ToInt32(officeid) && w.OrgID == LoggedInOrganizationID).OrderBy(o => o.CenterCode).Select(c => new { DisplayText = c.CenterCode + ", " + c.CenterName, Value = c.CenterID });
                return Json(new { Result = "OK", Options = centers });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult GetGroupList(string officeid)
        {
            try
            {
                var centers = groupService.GetAll().Where(w => w.IsActive == true && w.OfficeID == Convert.ToInt32(officeid) && w.OrgID == LoggedInOrganizationID).Select(c => new { DisplayText = c.GroupCode, Value = c.GroupID });
                return Json(new { Result = "OK", Options = centers });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult GetMemberList(string centerId, string groupId)
        {
            int offcId = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            try
            {
                if (groupId == "0")
                {

                    List<DBMemberDetailModel> List_ProductViewModel = new List<DBMemberDetailModel>();
                    var param = new { @Qtype = 6, @OrgID = SessionHelper.LoginUserOrganizationID, @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = centerId, @MemberID = 1, @GroupID = 1 };

                    var div_items = accReportService.GetMemberDetail(param);

                    List_ProductViewModel = div_items.Tables[0].AsEnumerable()
                    .Select(row => new DBMemberDetailModel
                    {
                        MemberID = row.Field<long>("MemberID"),
                        MemberCode = row.Field<string>("MemberCode"),
                        FullName = row.Field<string>("FullName")
                    }).ToList();

                    var members = List_ProductViewModel.Select(x => x).ToList().Select(x => new SelectListItem
                    {
                        Value = x.MemberID.ToString(),
                        Text = x.FullName.ToString()
                    });


                    //var members = memberService.GetAll().Where(w => w.IsActive == true && w.MemberStatus == "1" && w.OrgID==LoggedInOrganizationID && w.OfficeID == offcId && w.CenterID == Convert.ToInt32(centerId)).Select(c => new { DisplayText = c.MemberCode + ", " + c.FirstName + " " + c.MiddleName + " " + c.LastName, Value = c.MemberID });
                    return Json(new { Result = "OK", Options = members });
                }
                else
                {
                    List<DBMemberDetailModel> List_ProductViewModel = new List<DBMemberDetailModel>();
                    var param = new { @Qtype = 7, @OrgID = SessionHelper.LoginUserOrganizationID, @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = centerId, @MemberID = 1, @GroupID = groupId };

                    var div_items = accReportService.GetMemberDetail(param);

                    List_ProductViewModel = div_items.Tables[0].AsEnumerable()
                    .Select(row => new DBMemberDetailModel
                    {
                        MemberID = row.Field<long>("MemberID"),
                        MemberCode = row.Field<string>("MemberCode"),
                        FullName = row.Field<string>("FullName")
                    }).ToList();

                    var members = List_ProductViewModel.Select(x => x).ToList().Select(x => new SelectListItem
                    {
                        Value = x.MemberID.ToString(),
                        Text = x.FullName.ToString()
                    });
                    //var members = memberService.GetAll().Where(w => w.IsActive == true && w.MemberStatus == "1" && w.OrgID == LoggedInOrganizationID && w.OfficeID == offcId && w.CenterID == Convert.ToInt32(centerId) && w.GroupID == Convert.ToInt32(groupId)).Select(c => new { DisplayText = c.MemberCode + ", " + c.FirstName + " " + c.MiddleName + " " + c.LastName, Value = c.MemberID });
                    return Json(new { Result = "OK", Options = members });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        [HttpPost]
        public ActionResult SaveLoaneeTransferSelected(Dictionary<string, string> allTrx, List<string> allLoanTrxId)
        {

            try
            {
                var model = new DayInitialViewModel();
                var trx = allTrx;

                var trxId = 1;
                var loanTrxIds = allLoanTrxId.Where(w => int.TryParse(w, out trxId));

                //var loanTrxViewCollection = new List<DailyLoanTrx>();
                foreach (var id in loanTrxIds)
                {
                    var CurOfficeID = "CurOfficeID" + id;

                    var CurCenterID = "CurCenterID" + id;

                    var CurMemID = "CurMemID" + id;

                    var CurMemCode = "CurMemCode" + id;

                    var NewOfficeID = "NewOfficeID" + id;

                    var NewCenterID = "NewCenterID" + id;

                    var NewGroupID = "NewGroupID" + id;

                    int CurOffice = 0;
                    int CurCenter = 0;
                    int CurMem = 0;
                    int NewOffice = 0;
                    int NewCenter = 0;
                    int NewGroup = 0;
                    string NewMemCode = "";

                    if (allTrx.ContainsKey(CurOfficeID))
                        int.TryParse(allTrx[CurOfficeID], out CurOffice);
                    if (allTrx.ContainsKey(CurCenterID))
                        int.TryParse(allTrx[CurCenterID], out CurCenter);
                    if (allTrx.ContainsKey(CurMemID))
                        int.TryParse(allTrx[CurMemID], out CurMem);
                    if (allTrx.ContainsKey(NewOfficeID))
                        int.TryParse(allTrx[NewOfficeID], out NewOffice);
                    if (allTrx.ContainsKey(NewCenterID))
                        int.TryParse(allTrx[NewCenterID], out NewCenter);
                    if (allTrx.ContainsKey(NewGroupID))
                        int.TryParse(allTrx[NewCenterID], out NewGroup);
                    if (allTrx.ContainsKey(CurMemCode))
                        NewMemCode = allTrx[CurMemCode];
                    if (NewOffice > 0 && NewCenter > 0 && NewGroup > 0)
                    {
                        //NewMemberCode = GetNewMemberCode(NewOffice, NewGroup);
                        //var trx_date = processInfoService.GetInitialDtByOfficeId(CurOffice);
                        // var trx_date = processInfoService.GetAll().Where(g => g.OfficeID == CurOffice).OrderByDescending(o => o.InitialDate).First().InitialDate;

                        var param = new { OldOfficeID = CurOffice, OldMemberID = CurMem, NewCenterID = NewCenter, CreateUser = model.CreateUser };
                        int status = accReportService.SaveLoaneeTransferSelected(param);
                    }
                }

                //var MemberByCenterSessionKey = string.Format("MemberByCenterSessionKey_{0}", entity.CenterID);
                //Session.Remove(MemberByCenterSessionKey);
                //return Json(new { Result = "OK" });
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                //return Json(new { Result = "ERROR" });
                return GetErrorMessageResult(ex);
            }
        }
        [HttpPost]
        public ActionResult SaveLoaneeTransfer(Dictionary<string, string> allTrx, List<string> allLoanTrxId)
        {

            try
            {
                var model = new DayInitialViewModel();
                var trx = allTrx;

                var trxId = 1;
                var loanTrxIds = allLoanTrxId.Where(w => int.TryParse(w, out trxId));

                //var loanTrxViewCollection = new List<DailyLoanTrx>();
                foreach (var id in loanTrxIds)
                {
                    var CurOfficeID = "CurOfficeID" + id;

                    var CurCenterID = "CurCenterID" + id;

                    var CurMemID = "CurMemID" + id;

                    var CurMemCode = "CurMemCode" + id;

                    var NewOfficeID = "NewOfficeID" + id;

                    var NewCenterID = "NewCenterID" + id;

                    var NewGroupID = "NewGroupID" + id;

                    int CurOffice = 0;
                    int CurCenter = 0;
                    int CurMem = 0;
                    int NewOffice = 0;
                    int NewCenter = 0;
                    int NewGroup = 0;
                    string NewMemCode = "";

                    if (allTrx.ContainsKey(CurOfficeID))
                        int.TryParse(allTrx[CurOfficeID], out CurOffice);
                    if (allTrx.ContainsKey(CurCenterID))
                        int.TryParse(allTrx[CurCenterID], out CurCenter);
                    if (allTrx.ContainsKey(CurMemID))
                        int.TryParse(allTrx[CurMemID], out CurMem);
                    if (allTrx.ContainsKey(NewOfficeID))
                        int.TryParse(allTrx[NewOfficeID], out NewOffice);
                    if (allTrx.ContainsKey(NewCenterID))
                        int.TryParse(allTrx[NewCenterID], out NewCenter);
                    if (allTrx.ContainsKey(NewGroupID))
                        int.TryParse(allTrx[NewCenterID], out NewGroup);
                    if (allTrx.ContainsKey(CurMemCode))
                        NewMemCode = allTrx[CurMemCode];
                    if (NewOffice > 0 && NewCenter > 0 && NewGroup > 0)
                    {
                        //NewMemberCode = GetNewMemberCode(NewOffice, NewGroup);
                        //var trx_date = processInfoService.GetInitialDtByOfficeId(CurOffice);
                        var trx_date = processInfoService.GetAll().Where(g => g.OfficeID == CurOffice).OrderByDescending(o => o.InitialDate).First().InitialDate;

                        var param = new { OldOfficeID = CurOffice, OldCenterID = CurCenter, OldMemberID = CurMem, NewOfficeID = NewOffice, NewCenterID = NewCenter, NewMemberCode = NewMemCode, TrDate = trx_date, CreateUser = model.CreateUser };
                        int status = accReportService.SaveLoaneeTransfer(param);
                    }
                }

                //var MemberByCenterSessionKey = string.Format("MemberByCenterSessionKey_{0}", entity.CenterID);
                //Session.Remove(MemberByCenterSessionKey);
                //return Json(new { Result = "OK" });
                return GetSuccessMessageResult();
            }
            catch (Exception ex)
            {
                //return Json(new { Result = "ERROR" });
                return GetErrorMessageResult(ex);
            }
        }
        private string GetNewMemberCode(int offc_id, int group_id)
        {
            string last_code = "";
            var v = memberLastCodeService.GetByOffcGroupId(Convert.ToInt16(LoggedInOrganizationID), offc_id, group_id);
            var groupCode = groupService.GetById(group_id).GroupCode;
            var offcCode = officeService.GetById(offc_id).OfficeCode;
            if (v == null || v.LastCode == "") // if there is no voucher for this office
            {
                last_code = offcCode + groupCode + "001";
                string new_code = offcCode + groupCode + "002";
                var crt = new MemberLastCode();
                crt.OfficeID = offc_id;
                crt.GroupID = group_id;
                crt.LastCode = new_code;
                memberLastCodeService.Create(crt);
            }
            else // collect last voucher no
            {
                last_code = v.LastCode;
                //int strt_val = last_code.Length - 3;
                //int cl_val = last_code.Length-1;
                //string new_code = last_code.Substring(strt_val, 3);

                string new_code = IncMemberCode(last_code.Substring(last_code.Length - 3, 3));
                var updt = new MemberLastCode();
                updt = memberLastCodeService.GetByLastCodeId(Convert.ToInt32(v.LastCodeID));
                updt.LastCode = offcCode + groupCode + new_code;
                memberLastCodeService.Update(updt);
            }

            return last_code;
        }
        private string IncMemberCode(string lastCode)
        {
            string MemCode = "";
            lastCode = (Convert.ToInt32(lastCode) + 1).ToString();
            if (lastCode.Length == 1)
            {
                MemCode = "00" + lastCode;
            }
            else if (lastCode.Length == 2)
            {
                MemCode = "0" + lastCode;
            }
            else if (lastCode.Length == 3)
            {
                MemCode = lastCode;
            }
            return MemCode;
        }
        #endregion


        #region Events
        // GET: MemberInfoUodate
        public ActionResult MemberInfo()
        {

            var model = new LoaneeTransferViewModel();
            MapDropDownList(model);
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["MemberList"] = items;
            return View(model);

        }

        #endregion


    }// END Of Class
}// END Of Namespace