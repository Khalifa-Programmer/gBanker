using gBanker.Core.Utility;
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
    public class DayEndController : BaseController
    {
        private readonly IDayEndService dayEndService;
        private readonly IOfficeService officeService;
        private readonly IDayInitialService dayInitialService;
        private readonly IWeeklyReportService weeklyReportService;
        public DayEndController(IDayEndService dayEndService, IOfficeService officeService, IDayInitialService dayInitialService, IWeeklyReportService weeklyReportService)
        {
            this.dayEndService = dayEndService;
            this.officeService = officeService;
            this.dayInitialService = dayInitialService;
            this.weeklyReportService = weeklyReportService;
        }
        // GET: DayEnd
        public ActionResult Index()
        {
            DateTime vdate = TransactionDate;
            var model = new DayEndViewModel();
            if (IsDayInitiated)

                model.BusinessDate = vdate;
            MapDropDownList(model);

            //Populate day end process model
            PopulateDayEndProcessModel(model);

            //track sms bulk cookie
            TrackSMSBulkCookie();

            return View(model);
        }
        private void MapDropDownList(DayEndViewModel model)
        {

            var alloffice = officeService.GetAll().Where(d => d.OfficeID == LoginUserOfficeID && d.OrgID == LoggedInOrganizationID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;

        }
        public JsonResult DayEndProcess(string officeId, string businessDate)
        {
            try
            {
                if (!IsDayInitiated)
                {
                    return GetErrorMessageResult("Please run the start work process");
                }
                var param = new { OfficeId = LoginUserOfficeID, BusinessDate = businessDate, OrgID = SessionHelper.LoginUserOrganizationID, CreateUser = LoggedInEmployeeID };
                weeklyReportService.DayEndProcess(param);

                SessionHelper.TransactionDate = default(DateTime);
                SessionHelper.IsDayInitiated = false;
                try
                {
                    DateTime? transactionDate;
                    string OrginizationName;
                    string Processtype;
                    DateTime? LastDayEndDate;
                    var dayInitialStatus = dayInitialService.ValidateDayInitial(SessionHelper.LoginUserOfficeID, out transactionDate, out OrginizationName, out Processtype, out LastDayEndDate, Convert.ToInt16(LoggedInOrganizationID));

                    SessionHelper.TransactionDay = dayInitialStatus;
                    SessionHelper.TransactionDate = transactionDate.HasValue ? transactionDate.Value : default(DateTime);
                    SessionHelper.OrganizationName = OrganizationName;
                    SessionHelper.ProcessType = ProcessType;
                    SessionHelper.LastDayEndDate = LastDayEndDate;
                    SessionHelper.IsDayInitiated = !string.IsNullOrEmpty(dayInitialStatus);


                }
                catch (Exception ex)
                {
                    SessionHelper.TransactionDay = "";
                    SessionHelper.TransactionDate = default(DateTime);
                    SessionHelper.IsDayInitiated = false;
                    return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }

                //let's track sms bulk cookie 
                TrackSMSBulkCookie();

                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
                //return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        // GET: DayEnd/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: DayEnd/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: DayEnd/Create
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
        // GET: DayEnd/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        // POST: DayEnd/Edit/5
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
        // GET: DayEnd/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: DayEnd/Delete/5
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


        #region Private Methods

        private void PopulateDayEndProcessModel(DayEndViewModel model)
        {
            model.LoginUserOfficeID = (int)SessionHelper.LoginUserOfficeID;
            model.LoginUserOrganizationID = (int)SessionHelper.LoginUserOrganizationID;
            model.LoggedInOrganizationCode = SessionHelper.LoggedInOrganizationCode;
            string sendAutoSMS = "1";
            string sendAutoSMSBaseUrl = "http://localhost:59312";
            try
            {
                sendAutoSMS = System.Configuration.ConfigurationManager.AppSettings["SendAutoSMS"];
            }
            catch
            {
                sendAutoSMS = "0";
            }

            try
            {
                sendAutoSMSBaseUrl = System.Configuration.ConfigurationManager.AppSettings["SendAutoSMSByLocalhost"];
            }
            catch
            {
                sendAutoSMSBaseUrl = "http://localhost:59312";
            }

            model.ApiBaseUrl = sendAutoSMSBaseUrl;
            model.SendAutoSMS = sendAutoSMS;
        }

        private void TrackSMSBulkCookie()
        {
            HttpCookie httpCookie = new HttpCookie(BulkSMSAuthConstants.BulkSMSAuthClientKey);
            DateTime now = DateTime.Now;

            // Set the cookie value.
            httpCookie.Value = BulkSMSAuthConstants.BulkSMSAuthClientValue;
            // Set the cookie expiration date.
            httpCookie.Expires = now.AddMinutes(20);

            //remove the cookie first
            Response.Cookies.Remove(BulkSMSAuthConstants.BulkSMSAuthClientKey);

            // Add the cookie.
            Response.Cookies.Add(httpCookie);
        }

        #endregion
    }
}
