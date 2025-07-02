using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
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
    public class ExpireInfoController : BaseController
    {
        private readonly IExpireInfoService expireInfoService;
        private readonly IOfficeService officeService;
        private readonly IMemberService memberService;
        private readonly ICenterService centerService;
        private readonly IAccReportService accReportService;
        private readonly IUltimateReportService ultimateReportService;

        public ExpireInfoController(IExpireInfoService expireInfoService, IOfficeService officeService, IMemberService memberService, ICenterService centerService, IAccReportService accReportService, IUltimateReportService ultimateReportService)
        {
            this.expireInfoService = expireInfoService;
            this.officeService = officeService;
            this.memberService = memberService;
            this.centerService = centerService;
            this.accReportService = accReportService;
            this.ultimateReportService = ultimateReportService;
        }
        private void MapDropDownList(ExpireInfoViewModel model)
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

            var allcenter = centerService.GetByOfficeId(SessionHelper.LoginUserOfficeID.Value, Convert.ToInt16(LoggedInOrganizationID)); ;
            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });
            model.centerListItems = viewCenter;
            var alloffice = officeService.GetAll().Where(l => l.OfficeID == SessionHelper.LoginUserOfficeID.Value && l.OrgID == LoggedInOrganizationID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;


            var gender_item = new List<SelectListItem>();

            gender_item.Add(new SelectListItem() { Text = "Gurantor", Value = "0" });
            gender_item.Add(new SelectListItem() { Text = "Self", Value = "1", Selected = true });
            model.ExpireList = gender_item;
        }
        [HttpPost]
        public ActionResult GetExpireInfo(int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                //if (IsDayInitiated)
                //{
                var expireDetail = expireInfoService.getExpireInfo(LoginUserOfficeID, filterColumn, filterValue);
                var detail = expireDetail.ToList();
                var totalCount = detail.Count();
                var entities = detail.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<getExpireInfo_Result>, IEnumerable<ExpireInfoViewModel>>(entities);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });
                ////}
                //return Json(new { Result = "OK", Records = new List<LoanDisburseViewModel>(), TotalRecordCount = 0 });
                ////var viewploansummary = Mapper.Map<IEnumerable<Proc_get_LoanDisburse_Result>, IEnumerable<LoanDisburseViewModel>>(detail);

                ////return Json(new { Result = "OK", Records = viewploansummary });
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

        // GET: ExpireInfo
        public ActionResult Index()
        {
            return View();
        }

        // GET: ExpireInfo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ExpireInfo/Create
        public ActionResult Create()
        {
            var model = new ExpireInfoViewModel();

            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {

                model.ExpireDate = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
            }
            else
            {
                model.ExpireDate = TransactionDate;
            }
            //  model.ExpireDate = TransactionDate;

            if (IsDayInitiated)
                model.ExpireDate = TransactionDate;

            MapDropDownList(model);

            return View(model);
        }

        // POST: ExpireInfo/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, ExpireInfoViewModel model)
        {
            try
            {

                var entity = Mapper.Map<ExpireInfoViewModel, ExpireInfo>(model);
                var eStatus = model.ExpireStatus;
                if (ModelState.IsValid)
                {
                    var errors = expireInfoService.IsValidExpireInfo(entity);

                    if (errors.ToList().Count == 0)
                    {
                        entity.OfficeID = Convert.ToInt16(LoginUserOfficeID);
                        entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);


                        if (entity.ExpireStatus == 0)
                        {
                            if (model.ProductID.HasValue)
                            {
                                List<long> ls = new gBankerDbContext().Database.SqlQuery<long>("SELECT l.LoanSummaryID FROM LoanSummary l " +
                                "INNER JOIN Product p on l.ProductID=p.ProductID WHERE  l.LoanStatus = 1 and Posted = 1 AND l.IsActive=1" +
                                "AND l.MemberID=" + entity.MemberID + " AND l.OfficeID=" + entity.OfficeID + " AND l.ProductID=" + model.ProductID + "").ToList();
                                if (ls.Any()) entity.LoanSummaryID = ls.First();
                            }
                        }

                        expireInfoService.setExpireInfo(Convert.ToInt16(LoginUserOfficeID), entity.CenterID, Convert.ToInt64(entity.MemberID), entity.ExpiryName, entity.Relation, entity.ExpireDate, entity.Remarks, Convert.ToInt16(LoggedInOrganizationID), Convert.ToString(model.CreateUser), Convert.ToDateTime(model.CreateDate), Convert.ToInt16(model.ExpireStatus), entity.LoanSummaryID, "","");
                        return GetSuccessMessageResult();
                    }

                    else
                    {
                        return GetErrorMessageResult(errors);
                    }
                }
                else
                    return GetErrorMessageResult();


            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: ExpireInfo/Edit/5
        public ActionResult Edit(int id)
        {
            var model = new ExpireInfoViewModel();
            if (IsDayInitiated)
                model.ExpireDate = TransactionDate;

            var expiryID = Convert.ToInt16(id);
            var getExipire = expireInfoService.GetById(expiryID);
            var member = GetMember(Convert.ToInt64(getExipire.MemberID));
            var entity = Mapper.Map<ExpireInfo, ExpireInfoViewModel>(getExipire);
            ViewBag.MemberName = string.Format("{0} - {1}", member.MemberCode, member.FirstName);
            MapDropDownList(entity);
            //return View(entity);
            //MapDropDownList(model);

            return View(entity);
        }
        public Member GetMember(long memberid)
        {
            var mbr = memberService.GetByMemberId(memberid);
            return mbr;
        }
        // POST: ExpireInfo/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ExpireInfoViewModel model)
        {
            try
            {
                // TODO: Add insert logic here

                var entity = Mapper.Map<ExpireInfoViewModel, ExpireInfo>(model);

                if (ModelState.IsValid)
                {
                    var errors = expireInfoService.IsValidExpireInfo(entity);

                    if (errors.ToList().Count == 0)
                    {
                        entity.OfficeID = Convert.ToInt16(LoginUserOfficeID);
                        entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);
                        expireInfoService.setExpireInfo(Convert.ToInt16(LoginUserOfficeID), entity.CenterID, Convert.ToInt64(entity.MemberID), entity.ExpiryName, entity.Relation, entity.ExpireDate, entity.Remarks, Convert.ToInt16(LoggedInOrganizationID), Convert.ToString(model.CreateUser), Convert.ToDateTime(model.CreateDate), Convert.ToInt16(model.ExpireStatus), entity.LoanSummaryID, entity.ApplicantName, entity.MobileNumber);
                        return GetSuccessMessageResult();
                    }

                    else
                    {

                        return GetErrorMessageResult(errors);
                    }
                }
                else
                    return GetErrorMessageResult();

            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }

        // GET: ExpireInfo/Delete/5
        public ActionResult Delete(int id)
        {
            expireInfoService.Inactivate(id, null);
            return RedirectToAction("Index");
        }

        // POST: ExpireInfo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                expireInfoService.Inactivate(id, null);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
