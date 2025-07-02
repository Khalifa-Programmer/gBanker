using gBanker.Service;
using gBanker.Service.ReportExecutionService;
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
    public class MonthClosingController : BaseController
    {
        //Bappa
        private readonly IMonthClosingService monthclosingService;
        private readonly IOfficeService officeService;
        private readonly IWeeklyReportService weeklyReportService;
        private readonly IUltimateReportService ultimateReportService;
        public MonthClosingController(IMonthClosingService monthclosingService, IOfficeService officeService, IWeeklyReportService weeklyReportService, IUltimateReportService ultimateReportService)
          {
              this.monthclosingService = monthclosingService;
              this.officeService = officeService;
              this.weeklyReportService = weeklyReportService;
              this.ultimateReportService = ultimateReportService;
          }
        // [HttpPost]
        public JsonResult MonthlyProcess(string officeId, string businessDate)
        {
            try
            {
                
                   if (ProcessType == "Lowest")
                    {
                        monthclosingService.MonthlyProcess(LoginUserOfficeID, LoggedInOrganizationID, Convert.ToDateTime(businessDate));
                    }
                    else
                    {
                        var param = new { officeID = LoginUserOfficeID, OrgID = SessionHelper.LoginUserOrganizationID, ProcessDate = businessDate };
                        weeklyReportService.MonthProcessAverageMethod(param);

                        //monthclosingService.MonthlyProcessAverageMethod(LoginUserOfficeID, LoggedInOrganizationID, Convert.ToDateTime(businessDate)); 
                    }
                 
               // SessionHelper.TransactionDate = Convert.ToDateTime(businessDate);
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }


        }

        public JsonResult LLPVoucherProcess(string officeId, string businessDate)
        {
            try
            {
                var param = new { officeID = LoginUserOfficeID, ProcessDate = businessDate, OrgID = SessionHelper.LoginUserOrganizationID };
                weeklyReportService.MonthProcessLLPVoucher(param);
                
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void MapDropDownList(MonthClosingViewModel model)
        {

            var alloffice = officeService.GetAll().Where(m => m.OfficeID == LoginUserOfficeID && m.OrgID==LoggedInOrganizationID);

            var viewOffice = alloffice.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.officeListItems = viewOffice;

        }
        // GET: MonthClosing
        public ActionResult Index()
        {
            var model = new MonthClosingViewModel() ;

         
                MapDropDownList(model);
         

            return View(model);
        }

        // GET: MonthClosing
        public ActionResult LLPVoucher()
        {
            var model = new MonthClosingViewModel();
            MapDropDownList(model);
            return View(model);
        }

        // GET: MonthClosing/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: MonthClosing/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: MonthClosing/Create
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
        // GET: MonthClosing/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        // POST: MonthClosing/Edit/5
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
        // GET: MonthClosing/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: MonthClosing/Delete/5
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
        public ActionResult AppendMonthlyInstallment()
        {
            var model = new MonthClosingViewModel();
            MapDropDownList(model);
            return View(model);
        }


        public JsonResult AppendMothlyInstallmentProcess(string officeId, string businessDate)
        {
            try
            {
                var param = new { OfficeId = LoginUserOfficeID, BusinessDate = TransactionDate, CreateUser = SessionHelper.LoginUserEmployeeID, CreateDate = DateTime.Now, OrgID = SessionHelper.LoginUserOrganizationID };
                weeklyReportService.MonthlyInstallmentProcess(param);

                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // Provision Calculation LLP Voucher
        public ActionResult GenerateLLPVoucherReport(DateTime ProcessDate)
        {
            try
            {
                var param = new { @OfficeID = SessionHelper.LoginUserOfficeID, @ProcessDate = ProcessDate };
                var alldata = ultimateReportService.GetDataWithParameter(param, "rpt_LLPVoucherReport");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", ProcessDate);
                ReportHelper.PrintReport("ProvisionCalculation_LLPVoucher.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //public ActionResult AppendMonthlyInstallment()
        //{
        //    var model = new MonthClosingViewModel();
        //    MapDropDownList(model);
        //    return View(model);
        //}


        //public JsonResult AppendMothlyInstallmentProcess(string officeId, string businessDate)
        //{
        //    try
        //    {
        //        var param = new { OfficeId = LoginUserOfficeID, BusinessDate = businessDate, CreateUser = SessionHelper.LoginUserEmployeeID, CreateDate = DateTime.Now, OrgID = SessionHelper.LoginUserOrganizationID };
        //        weeklyReportService.MonthlyInstallmentProcess(param);

        //        return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

    }
}
