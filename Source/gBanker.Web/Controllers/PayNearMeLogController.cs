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
    public class PayNearMeLogController : BaseController
    {
        #region Variables
        private readonly IMemberService memberService;        
        private readonly IPNMConfirmService pnmConfirmService;

        public PayNearMeLogController(IMemberService memberService, IPNMConfirmService pnmConfirmService)
        {            
            this.memberService = memberService;
            this.pnmConfirmService = pnmConfirmService;            
        }
        #endregion

        #region Methods
        public JsonResult GetPNMInfo(int jtStartIndex, int jtPageSize, string jtSorting, string dateFrom, string dateTo, string memCode)
        {
            try
            {
                var pnmDetail = pnmConfirmService.GetAll();
                if (dateFrom != "" && dateTo != "")
                    pnmDetail = pnmDetail.Where(w => Convert.ToDateTime(w.payment_timestamp_dt) >= Convert.ToDateTime(dateFrom) && Convert.ToDateTime(w.payment_timestamp_dt) <= Convert.ToDateTime(dateTo));
                if (memCode != "")
                {
                    pnmDetail = pnmDetail.Where(w => w.site_identifier == memCode);
                }

                var detail = pnmDetail.ToList();
                var totCount = detail.Count();
                var currentPageRecords = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totCount });
                //return Json(new { Result = "OK", Records = pnmDetail, TotalRecordCount = 1 });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        #endregion

        #region Events
        // GET: SmsLog
        public ActionResult Index()
        {
            ViewData["dayInitial"] = TransactionDate;
            return View();
        }

        // GET: SmsLog/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SmsLog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SmsLog/Create
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

        // GET: SmsLog/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SmsLog/Edit/5
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

        // GET: SmsLog/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SmsLog/Delete/5
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
