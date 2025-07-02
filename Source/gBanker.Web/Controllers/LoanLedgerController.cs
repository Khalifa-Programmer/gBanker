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
    public class LoanLedgerController : BaseController
    {
        #region Variables
        private readonly ILoanLedgerReportService loanLedgerReportService;
        private readonly IOfficeService officeService;
        private readonly ICenterService centerService;
        public LoanLedgerController(ILoanLedgerReportService loanLedgerReportService, IOfficeService officeService, ICenterService centerService)
        {
            this.loanLedgerReportService = loanLedgerReportService;
            this.officeService = officeService;
            this.centerService = centerService;
        }
        #endregion

        #region Methods
        private void MapDropDownList(LoanLedgerViewModel model)
        {

            var alloffice = officeService.GetAll().Where(t => t.OfficeID == LoginUserOfficeID.Value);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;

            var allcenter = centerService.GetByOfficeId(LoginUserOfficeID.Value, Convert.ToInt16(LoggedInOrganizationID));

            var viewCenter = allcenter.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.CenterCode, m.CenterName), Value = m.CenterID.ToString() });

            model.centerListItems = viewCenter;


        }
        
        #endregion

        #region Events
        public ActionResult GenerateLoanLedgerReport(string officeId, string centerId)
        {
            try
            {

                var param = new { Qtype = 1, Office = LoginUserOfficeID, Center = centerId };
                var allproducts = loanLedgerReportService.GetDataLoanLedgerInfo(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("Header1", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("rptLoanLedger.rpt", allproducts.Tables[0], reportParam);
                return Content(string.Empty); 
                //ReportHelper.PrintReport("rptLoanLedger.rpt", allproducts.Tables[0], new Dictionary<string, object>());

                //Incase of subreport.
                //var subReportDataSources = new Dictionary<string, DataTable>();
                //subReportDataSources.Add("Subreport1", allproducts.Tables[0]);
                //subReportDataSources.Add("Subreport2", allproducts.Tables[0]);
                //ReportHelper.PrintWithSubReport("", allproducts.Tables[0], new Dictionary<string, object>(), subReportDataSources);
               // return Content(string.Empty); 
                // return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        // GET: LoanLedger
        public ActionResult Index()
        {
            var model = new LoanLedgerViewModel();
            MapDropDownList(model);
            return View(model);
        }

        // GET: LoanLedger/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LoanLedger/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoanLedger/Create
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

        // GET: LoanLedger/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoanLedger/Edit/5
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

        // GET: LoanLedger/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoanLedger/Delete/5
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
