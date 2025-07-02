using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.Reports;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace gBanker.Web.Controllers
{
    public class AccNoteReportController : BaseController
    {
        #region Variables
        private readonly IAccTrxMasterService accTrxMasterService;
        private readonly IAccTrxDetailService accTrxDetailService;
        private readonly IAccChartService accChartService;
        private readonly IAccNoteService accNoteService;
        private readonly IProcessInfoService processInfoService;
        private readonly IAccReportService accReportService;
        private readonly IOfficeService officeService;

        public AccNoteReportController(IAccTrxMasterService accTrxMasterService, IAccTrxDetailService accTrxDetailService, IAccChartService accChartService, IAccNoteService accNoteService, IProcessInfoService processInfoService, IAccReportService accReportService, IOfficeService officeService)
        {
            this.accTrxMasterService = accTrxMasterService;
            this.accTrxDetailService = accTrxDetailService;
            this.accChartService = accChartService;
            this.accNoteService = accNoteService;
            this.processInfoService = processInfoService;
            this.accReportService = accReportService;
            this.officeService = officeService;
        }
        #endregion

        #region Methods
        public JsonResult GetNoteList()
        {

            var noteList = accNoteService.GetAll().Where(c => c.IsActive == true && c.OrgID == LoggedInOrganizationID);
            var viewNote = noteList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.NoteID.ToString(),
                Text = x.NoteNo.ToString() + " " + x.NoteName.ToString()
            });
            var note_items = new List<SelectListItem>();
            if (viewNote.ToList().Count > 0)
            {
                note_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            note_items.AddRange(viewNote);
            return Json(note_items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerateNoteReport(string NoteID, string CurYear)
        {
            try
            {
                var param = new { NoteID = NoteID, CurYear = CurYear };
                var allVouchers = accReportService.GetNoteReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("Param_OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("current_year",CurYear);
                reportParam.Add("pre_year", (Convert.ToInt32(CurYear)-1).ToString());
                ReportHelper.PrintReport("rpt_acc_note.rpt", allVouchers.Tables[0], reportParam);          
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion

        #region Events
        // GET: NoteReport        
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["NoteList"] = items;
            return View();
        }

        // GET: NoteReport/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NoteReport/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NoteReport/Create
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

        // GET: NoteReport/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NoteReport/Edit/5
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

        // GET: NoteReport/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NoteReport/Delete/5
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

