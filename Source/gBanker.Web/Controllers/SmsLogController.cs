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
    public class SmsLogController : BaseController
    {
        #region Variables
        private readonly IMemberService memberService;        
        private readonly ISmsLogService smsLogService;
        
        public SmsLogController(IMemberService memberService, ISmsLogService smsLogService)
        {            
            this.memberService = memberService;            
            this.smsLogService = smsLogService;            
        }
        #endregion

        #region Methods
        public JsonResult GetSmsInfo(int jtStartIndex, int jtPageSize, string jtSorting, string dateFrom, string dateTo, string memCode, string smsType, string smsStatus)
        {
            try
            {
                var smsDetail = smsLogService.GetByOrgID(1);
                if(dateFrom != "" && dateTo != "")
                    smsDetail = smsDetail.Where(w => w.DateSent >= Convert.ToDateTime(dateFrom) && w.DateSent <= Convert.ToDateTime(dateTo));
                if (memCode != "")
                {
                    var mem = memberService.GetByMemeberCode(memCode);
                    if(mem != null)
                        smsDetail = smsDetail.Where(w => w.MemberID == mem.MemberID);
                }
                if(smsType != "V" )
                    smsDetail = smsDetail.Where(w => w.SmsType == smsType);

                if (smsStatus != "V")
                    smsDetail = smsDetail.Where(w => w.SmsStatus == smsStatus);

                var smsView = new List<SmsLogViewModel>();
                
                foreach(var sms in smsDetail)
                {
                    var mem = memberService.GetByMemberId(Convert.ToInt64(sms.MemberID));
                    SmsLogViewModel log = new SmsLogViewModel();
                    log.SmsLogID = sms.SmsLogID;
                    log.OrgID = sms.OrgID;
                    log.MemberID = sms.MemberID;
                    log.MemberName = mem.MemberCode + ", " + mem.FirstName + " " + mem.MiddleName + " " + mem.LastName;
                    log.SmsType = sms.SmsType;
                    log.SmsFrom = sms.SmsFrom;
                    log.SmsTo = sms.SmsTo;
                    log.SmsBody = sms.SmsBody;
                    log.DateSent = sms.DateSent;
                    log.SmsStatus = sms.SmsStatus;
                    smsView.Add(log);
                }
                var detail = smsView.ToList();
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
