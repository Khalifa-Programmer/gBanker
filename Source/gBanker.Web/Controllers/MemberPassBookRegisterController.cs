using AutoMapper;
using gBanker.Data.CodeFirstMigration;
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
    public class MemberPassBookRegisterController : BaseController
    {
       #region Variables
        private readonly IMemberPassBookRegisterService MemberPassBookRegisterService;
        private readonly IMemberPassBookStockService MemberPassBookStockService;
        private readonly IMemberService memberService;
        private readonly IOfficeService officeService;
        private readonly ICenterService centerService;
        private readonly IUltimateReportService ultimateReportService;
        public MemberPassBookRegisterController(IMemberPassBookStockService MemberPassBookStockService,IOfficeService officeService, IMemberPassBookRegisterService MemberPassBookRegisterService, IMemberService memberService, ICenterService centerService, IUltimateReportService ultimateReportService)
        {
            this.memberService = memberService;
            this.officeService = officeService;
            this.MemberPassBookRegisterService = MemberPassBookRegisterService;
            this.centerService = centerService;
            this.MemberPassBookStockService = MemberPassBookStockService;
            this.ultimateReportService = ultimateReportService;
        }
        #endregion

        private void MapDropDownList(MemberPassBookRegisterViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }
            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID));

            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)), Value = m.MemberID.ToString() });

            model.memberListItems = viewMember;
            ViewData["Member"] = viewMember;

           

            var alloffice = officeService.GetAll().Where(l => l.OfficeID == SessionHelper.LoginUserOfficeID.Value && l.OrgID == LoggedInOrganizationID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;




            var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt16(LoggedInOrganizationID)); ;

            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });

            model.centerListItems = viewCenter;


            var allLot = MemberPassBookStockService.GetAll().Where(x => x.OrgID == Convert.ToInt16(LoggedInOrganizationID) && x.OfficeID == Convert.ToInt16(LoginUserOfficeID)).ToList();

            var viewLot = allLot.Select(m => new SelectListItem() { Text = string.Format("{0}", m.LotNo), Value = m.LotNo.ToString() });

            model.LotListItems = viewLot;


            var status_item = new List<SelectListItem>();
            status_item.Add(new SelectListItem() { Text = "Active", Value = "1", Selected = true });
            status_item.Add(new SelectListItem() { Text = "Drop", Value = "0" });
            model.StatusListItems = status_item;


        }
        public JsonResult GetMemberPassBookRegister(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn = "",   string filterValue = "", string DateFromValue = "", string DateToValue = "")
        {
            try
            {
                long totalCount;
                //var allSavingsummary = MemberPassBookRegisterService.getPassBookRegister(SessionHelper.LoginUserOfficeID.Value, SessionHelper.LoginUserOrganizationID);

                //var currentPageRecords = Mapper.Map<IEnumerable<getPassBookRegister_Result>, IEnumerable<MemberPassBookRegisterViewModel>>(allSavingsummary);
                //return Json(new { Result = "OK", Records = currentPageRecords });
                List<MemberPassBookRegisterViewModel> List_LoanApprovalViewModel = new List<MemberPassBookRegisterViewModel>();
                var param = new { OfficeID = SessionHelper.LoginUserOfficeID.Value, OrgID = SessionHelper.LoginUserOrganizationID, DateFrom =DateFromValue, DateTo =DateToValue, FilterColumn=filterColumn, FilterValue=filterValue };
                var loanInfo = ultimateReportService.GetMembersPassBookRegisterInformation(param);
                List_LoanApprovalViewModel = loanInfo.Tables[0].AsEnumerable()
                    .Select(row => new MemberPassBookRegisterViewModel
                    {
                        MemberPassBookRegisterID= row.Field<long>("MemberPassBookRegisterID"),
                        OfficeCode = row.Field<string>("OfficeCode"),
                        MemberID = row.Field<long>("MemberID"),
                        OfficeName = row.Field<string>("OfficeName"),
                        CenterCode = row.Field<string>("CenterCode"),//Convert.ToDecimal(string.IsNullOrEmpty(row.Field<string>("PrincipalLoan")) ? "0" : row.Field<string>("PrincipalLoan")),
                        MemberCode = row.Field<string>("MemberCode"), //Convert.ToDecimal(string.IsNullOrEmpty(row.Field<string>("LoanRepaid")) ? "0" : row.Field<string>("LoanRepaid")),
                        MemberName = row.Field<string>("MemberName"),
                        LotNo = row.Field<long>("LotNo"),//Convert.ToDecimal(string.IsNullOrEmpty(row.Field<string>("IntPaid")) ? "0" : row.Field<string>("IntPaid"))
                        MemberPassBookNO = row.Field<long>("MemberPassBookNO"),
                        PassBookStartDateMSG = row.Field<string>("PassBookStartDateMSG"),
                        PassBookCloseDateMSG = row.Field<string>("PassBookCloseDateMSG"),

                    }).ToList();
                return Json(new { Result = "OK", Records = List_LoanApprovalViewModel, TotalRecordCount = List_LoanApprovalViewModel.Count() });

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
                var mbr = memberService.GetByCenterId(int.Parse(centerId), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID)).ToList();
                Session[MemberByCenterSessionKey] = mbr;
                memberList = mbr;
            }
            //var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, m.FirstName).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, m1.FirstName) }).ToList();
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();
            return Json(members, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AdminIndex()
        {
            return View();
        }

        // GET: MemberPassBookRegister
        public ActionResult Index()
        {
            return View();
        }

        // GET: MemberPassBookRegister/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MemberPassBookRegister/Create
        public ActionResult Create()
        {
            var model = new MemberPassBookRegisterViewModel();
            if (IsDayInitiated)
                model.PassBookStartDate = TransactionDate;

            MapDropDownList(model);

            return View(model);
        }

        // POST: MemberPassBookRegister/Create
        [HttpPost]
        public ActionResult Create(MemberPassBookRegisterViewModel model, FormCollection form)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = Mapper.Map<MemberPassBookRegisterViewModel, MemberPassBookRegister>(model);
                    entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                    var errors = MemberPassBookRegisterService.IsValidPassBookCreate(entity);
                    if (errors.ToList().Count == 0)
                    {
                        if (LoggedInOrganizationID == 8)
                        {
                            var param = new { OfficeId = LoginUserOfficeID, MemberId = entity.MemberID };
                            var div_items = ultimateReportService.validateMemberPassbook(param);
                            if (div_items.Tables[0].Rows.Count > 0)
                            {
                                return GetErrorMessageResult();
                            }
                        }
                        
                            entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                        //Add Validlation Logic.
                        entity.IsActive = true;
                        entity.Status = 1;
                        entity.PassBookStartDate = TransactionDate;
                        entity.PassBookCloseDate = TransactionDate;
                        MemberPassBookRegisterService.Create(entity);
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
        public Member GetMember(long memberid)
        {
            var mbr = memberService.GetByMemberId(memberid);
            return mbr;
        }
        public ActionResult AdminEdit(int id)
        {
            var loanapproval = MemberPassBookRegisterService.GetByIdLong(id);

            var member = GetMember(Convert.ToInt64(loanapproval.MemberID));
            var entity = Mapper.Map<MemberPassBookRegister, MemberPassBookRegisterViewModel>(loanapproval);
            ViewBag.MemberName = string.Format("{0} - {1}", member.MemberCode, member.FirstName);
            MapDropDownList(entity);

            return View(entity);
        }
        // GET: MemberPassBookRegister/Edit/5
        public ActionResult Edit(int id)
        {
            var loanapproval = MemberPassBookRegisterService.GetByIdLong(id);

            var member = GetMember(Convert.ToInt64(loanapproval.MemberID));
            var entity = Mapper.Map<MemberPassBookRegister, MemberPassBookRegisterViewModel>(loanapproval);
            ViewBag.MemberName = string.Format("{0} - {1}", member.MemberCode, member.FirstName);
            MapDropDownList(entity);

            return View(entity);
        }

        // POST: MemberPassBookRegister/Edit/5
        [HttpPost]
        public ActionResult Edit(MemberPassBookRegisterViewModel model)
        {
            try
            {
                var entity = Mapper.Map<MemberPassBookRegisterViewModel, MemberPassBookRegister>(model);
                var getPurpose = MemberPassBookRegisterService.GetByIdLong(entity.MemberPassBookRegisterID);
                // TODO: Add insert logic here
                //if (ModelState.IsValid)
                //{
                    entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                    var errors = MemberPassBookRegisterService.IsValidPassBook(entity);
                    if (errors.ToList().Count == 0)
                    {
                        getPurpose.LotNo = entity.LotNo;
                        getPurpose.MemberPassBookNO = entity.MemberPassBookNO;
                        getPurpose.PassBookStartDate = entity.PassBookStartDate;
                        getPurpose.Status = entity.Status;
                        getPurpose.IsActive = true;
                        MemberPassBookRegisterService.Update(getPurpose);
                        return GetSuccessMessageResult();
                    }
                    else
                        return GetErrorMessageResult();
                //}
                //return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        [HttpPost]
        public ActionResult AdminEdit(MemberPassBookRegisterViewModel model)
        {
            try
            {
                var entity = Mapper.Map<MemberPassBookRegisterViewModel, MemberPassBookRegister>(model);
                var getPurpose = MemberPassBookRegisterService.GetByIdLong(entity.MemberPassBookRegisterID);
                // TODO: Add insert logic here
                //if (ModelState.IsValid)
                //{
                entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                //var errors = MemberPassBookRegisterService.IsValidPassBook(entity);
                //if (errors.ToList().Count == 0)
                //{
                getPurpose.CenterID = entity.CenterID;
                getPurpose.MemberID = entity.MemberID;
                getPurpose.LotNo = entity.LotNo;
                getPurpose.MemberPassBookNO = entity.MemberPassBookNO;
                getPurpose.PassBookStartDate = entity.PassBookStartDate;
                getPurpose.Status = entity.Status;
                getPurpose.IsActive = true;
                MemberPassBookRegisterService.Update(getPurpose);
                return GetSuccessMessageResult();
                //}
                //else
                //    return GetErrorMessageResult();
                //}
                //return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: MemberPassBookRegister/Delete/5
        public ActionResult Delete(long id)
        {
            var purpose = MemberPassBookRegisterService.GetByIdLong(id);
            var entity = Mapper.Map<MemberPassBookRegister, MemberPassBookRegisterViewModel>(purpose);
            return View(entity);
        }

        // POST: MemberPassBookRegister/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                MemberPassBookRegisterService.Inactivate(id, null);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
