using gBanker.Service;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Web.Helpers;
using gBanker.Service.ReportServies;
using System.Text;
using System.Data;

namespace gBanker.Web.Controllers
{
    public class DayInititalController : BaseController
    {

        private readonly IDayInitialService dayInitialService;
        private readonly IOfficeService officeService;
        private readonly IUltimateReportService ultimateReportService;
        public DayInititalController(IDayInitialService dayInitialService,IOfficeService officeService, IUltimateReportService ultimateReportService)
        {
              this.dayInitialService = dayInitialService;
              this.officeService = officeService;
              this.ultimateReportService = ultimateReportService;
        }
        // GET: DayInitital
        public ActionResult Index()
        {
            DateTime vdate = TransactionDate;
            var model = new DayInitialViewModel();
            if (IsDayInitiated)

                model.BusinessDate = vdate;
                MapDropDownList(model);
                return View(model);
        }
        private void MapDropDownList(DayInitialViewModel model)
        {

            var alloffice = officeService.GetAll().Where(d => d.OfficeID == LoginUserOfficeID && d.OrgID == LoggedInOrganizationID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;

        }
        // GET: DayInitital/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: DayInitital/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: DayInitital/Create
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
       // [HttpPost]
        public JsonResult DayInitialProcess(string officeId, string businessDate)
        {
            try
            {

                 DateTime? transactionDate;
                string OrginizationName;
                string Processtype;
                DateTime? LastDayEndDate;
                var model = new DayInitialViewModel();
                dayInitialService.DayInitialProcess(SessionHelper.LoginUserOfficeID.Value, Convert.ToDateTime(businessDate),Convert.ToString(LoggedInEmployeeID), Convert.ToDateTime(model.CreateDate),Convert.ToInt16( LoggedInOrganizationID));
                var dayInitialStatus = dayInitialService.ValidateDayInitial(SessionHelper.LoginUserOfficeID, out transactionDate, out OrginizationName, out Processtype, out   LastDayEndDate, Convert.ToInt16(LoggedInOrganizationID));
                SessionHelper.TransactionDay = dayInitialStatus;
                SessionHelper.TransactionDate = transactionDate.HasValue ? transactionDate.Value : default(DateTime);
                SessionHelper.OrganizationName = OrganizationName;
                SessionHelper.ProcessType = ProcessType;
                SessionHelper.LastDayEndDate = LastDayEndDate;
                SessionHelper.IsDayInitiated = !string.IsNullOrEmpty(dayInitialStatus);

                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                SessionHelper.IsDayInitiated = false;
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // GET: DayInitital/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        // POST: DayInitital/Edit/5
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
        // GET: DayInitital/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: DayInitital/Delete/5
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


        #region Delete DayInitial

        //UI

        public ActionResult ManageDayInitials()
        {
            return View();
        }

        // Delete ProcessInfo
        public JsonResult DeleteDayInitial(int DayInitialsId)
        {
            string result = "OK";
            try
            {
                Int64 UpdateUser = Convert.ToInt64(LoggedInEmployeeID);
                DateTime UpdateDate = DateTime.Now;

                var param2 = new { DayInitialsId = DayInitialsId, UpdateUser = UpdateUser, UpdateDate = UpdateDate };
                var val = ultimateReportService.GetDataWithParameter(param2, "SP_DeleteDayInitials");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        // List


        // Show List
        public JsonResult GetDayInitialList(string officeId, string businessDate, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                string officeIds = Convert.ToString(officeId);

                if (businessDate != null)
                    sb.Append(" AND pri.InitialDate ='" + businessDate + "'");


                if (officeId != null && officeId != "0")
                    sb.Append(" AND pri.OfficeId =" + officeIds);

                List<DayInitialViewModel> List_ViewModel = new List<DayInitialViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = ultimateReportService.GetDataWithParameter(param, "SP_Get_ProcessInfo_List");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new DayInitialViewModel
                {
                    rowSl = row.Field<Int64>("rowSl"),
                    OfficeName = row.Field<string>("OfficeName"),
                    InitialDate = row.Field<string>("InitialDate"),
                    ProcessInfoID = row.Field<Int64>("ProcessInfoID"),

                }).ToList();


                var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End Function




        #endregion


    }
}
