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
    public class RepaymentScheduleController : BaseController
    {
        #region Variables
        private readonly IOfficeService officeService;
        private readonly IMemberService memberService;
        private readonly ICenterService centerService;
        private readonly IRepaymentScheduleService repaymentScheduleService;
        private readonly IProductService productService;

        public RepaymentScheduleController(IOfficeService officeService, ICenterService centerService, IMemberService memberService, IRepaymentScheduleService repaymentScheduleService, IProductService productService)
          {
              
              this.officeService = officeService;
              this.centerService = centerService;
              this.repaymentScheduleService = repaymentScheduleService;
              this.memberService = memberService;
              this.productService = productService;
          }
        #endregion

        #region Methods
        private void MapDropDownList(RepaymentScheduleViewModel model)
        {
            if (!SessionHelper.LoginUserOfficeID.HasValue)
            {
                RedirectToAction("Login", "Account");
                return;
            }
            //var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID));

            //var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + '-' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName))), Value = m.MemberID.ToString() });

            //model.memberListItems = viewMember;
            //ViewData["Member"] = viewMember;



            var allProd = productService.GetAll().Where(w => w.IsActive == true && w.OrgID==LoggedInOrganizationID).OrderBy(e=>e.ProductCode);

            var viewProduct = allProd.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.ProductCode, m.ProductName), Value = m.ProductID.ToString() });

            model.productListItems = viewProduct;



        }
        public ActionResult GetMemberList(string memberid, string oficeId)
        {
            
            var memberList = new List<Member>();

            var mbr = memberService.SearchMember(int.Parse(oficeId), Convert.ToInt16(LoggedInOrganizationID));
            //var mbr = memberService.GetAll().Where(w => w.OfficeID == Convert.ToInt32(oficeId));
            memberList = mbr.ToList();
            //}
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + " " + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + " " + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + " " + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + " " + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRepaymentSchedule(int jtStartIndex, int jtPageSize, string jtSorting, int MemberId, int ProductID)
        {
            try
            {
                 var model = new RepaymentScheduleViewModel();
                 var entity = Mapper.Map<RepaymentScheduleViewModel, GetRepaymentSchedule_Result>(model);

                 //var entity = Mapper.Map<RepaymentScheduleViewModel, RepaymentSchedule>(model);
                 //var allRepaymentSchedule = repaymentScheduleService.GetAll().Where(x => x.OfficeID == LoginUserOfficeID && x.MemberID == MemberId && x.ProductID == ProductID);
                // var allRepaymentSchedule = repaymentScheduleService.GetRePaymentDetail(LoginUserOfficeID, MemberId, ProductID);

                 var allRepaymentSchedule = repaymentScheduleService.GetRepaymentSchedule(LoginUserOfficeID, MemberId, ProductID);
                var detail = allRepaymentSchedule.ToList();
                var totCount = detail.Count();
                var currentPageRecords = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totCount });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        #endregion

        #region Events
        // GET: RepaymentSchedule
        public ActionResult Index()
        {
            var model = new RepaymentScheduleViewModel();
            MapDropDownList(model);
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            return View(model);
        }

        // GET: RepaymentSchedule/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RepaymentSchedule/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RepaymentSchedule/Create
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

        // GET: RepaymentSchedule/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RepaymentSchedule/Edit/5
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

        // GET: RepaymentSchedule/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RepaymentSchedule/Delete/5
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
        #endregion
    }
}
