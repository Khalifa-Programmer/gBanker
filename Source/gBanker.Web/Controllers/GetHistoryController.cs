using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class GetHistoryController : BaseController
    {
        private readonly ILoanTrxService loantrxService;

        private readonly IMemberService memberService;
        private readonly ICenterService centerService;
        public GetHistoryController(ILoanTrxService loantrxService, ICenterService centerService,
            IMemberService memberService
            )
        {
            this.loantrxService = loantrxService;
            this.centerService = centerService;
            this.memberService = memberService;


        }
        public JsonResult GetCenterList()
        {
            var getCenter = centerService.GetByOfficeId(Convert.ToInt32(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID));
            var viewCenter = getCenter.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterCode.ToString() + ", " + x.CenterName.ToString()
            });
            var voucher_items = new List<SelectListItem>();
            if (viewCenter.ToList().Count > 0)
            {
                voucher_items.Add(new SelectListItem() { Text = "Get All", Value = "0", Selected = true });
            }
            voucher_items.AddRange(viewCenter);
            return Json(voucher_items, JsonRequestBehavior.AllowGet);
        }
        public void ProductType()
        {
            var prodTypeList = new List<SelectListItem>();
            prodTypeList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            prodTypeList.Add(new SelectListItem() { Text = "Loan", Value = "L" });
            prodTypeList.Add(new SelectListItem() { Text = "Savings", Value = "S" });
            ViewData["ProductTypeList"] = prodTypeList;
        }
        public ActionResult GetMemberList(string memberid, string centerId)
        {
            var MemberByCenterSessionKey = string.Format("MemberByCenterSessionKey_{0}", centerId);
            var memberList = new List<Member>();
            if (Session[MemberByCenterSessionKey] != null)
                memberList = Session[MemberByCenterSessionKey] as List<Member>;
            else
            {
                int CenterID = Convert.ToInt32(centerId);
                int officeId = Convert.ToInt32(LoginUserOfficeID);
                int OrgID = Convert.ToInt32(LoggedInOrganizationID);
                var mbrList = memberService.GetByCenterId(CenterID, officeId, OrgID);

                var mbr = mbrList.ToList();

                //var mbr = memberService.GetByCenterId(Convert.ToInt32(centerId), Convert.ToInt32(LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID)).ToList();
                Session[MemberByCenterSessionKey] = mbr;
                memberList = mbr;
            }
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();

            return Json(members, JsonRequestBehavior.AllowGet);
        }
        // GET: GetHistory
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            ProductType();
            TransactionType();
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
                ViewData["TrxdateTo"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
                ViewData["TrxdateTo"] = VDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }

        public void TransactionType()
        {
            var TransactionType = new List<SelectListItem>();
            TransactionType.Add(new SelectListItem() { Text = "Select All", Value = "0" });
            TransactionType.Add(new SelectListItem() { Text = "Daily", Value = "10" });
            TransactionType.Add(new SelectListItem() { Text = "Special", Value = "20" });
            TransactionType.Add(new SelectListItem() { Text = "Transfer", Value = "21" });
            TransactionType.Add(new SelectListItem() { Text = "Others", Value = "30" });
            ViewData["TransactionTypeList"] = TransactionType;
        }
        public JsonResult GenerateHistory(int jtStartIndex, int jtPageSize, string jtSorting, string DateFrom, string DateTo, string centerIdFrom, string productType, string TransType, long? memberID = 0)
        {
            try
            {
                var allHistory = loantrxService.GetHistory(Convert.ToInt16(LoggedInOrganizationID), SessionHelper.LoginUserOfficeID.Value, Convert.ToDateTime(DateFrom), Convert.ToDateTime(DateTo), Convert.ToInt16(centerIdFrom), memberID, productType, TransType);
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
        // GET: GetHistory/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GetHistory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GetHistory/Create
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

        // GET: GetHistory/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: GetHistory/Edit/5
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

        // GET: GetHistory/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GetHistory/Delete/5
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
