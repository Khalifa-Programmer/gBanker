using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class FullyRepaidController : BaseController
    {
             
        #region Variables
        private readonly IDailyReportService dailyReportService;
        private readonly IProductService productService;


        public FullyRepaidController(IDailyReportService dailyReportService, IProductService productService)
        {
            this.dailyReportService = dailyReportService;
            this.productService = productService;
    
        }
        #endregion

        #region Methods

        public ActionResult GenerateFullyrepaid_DateRangeReport(string Qtype, string Date1, string Date2, string Product, string ExportType)
        {
            try
            {

                var param = new { Qtype = Qtype, Office = SessionHelper.LoginUserOfficeID, Date1 = Date1, Date2 = Date2, Org = SessionHelper.LoginUserOrganizationID, Product = Product, EmpID = LoggedInEmployeeID };
                var FullyReapids = dailyReportService.GetDataFullyRepaid_DateRangeReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("Date1", Date1);
                reportParam.Add("Date2", Date2);

                if (ExportType == "pdf")
                {
                    ReportHelper.PrintReport("rptFullyRep_DateRang.rpt", FullyReapids.Tables[0], reportParam);
                }
                else
                {
                    ReportHelper.ExportExcelReportFormated("rptFullyRep_DateRang.rpt", FullyReapids.Tables[0], reportParam, "PaidOffList_Excel");
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateExpiredForcast_DateRangeReport(string Qtype, string Date1, string Date2, string Product)
        {
            try
            {

                var param = new { Qtype = Qtype, Office = SessionHelper.LoginUserOfficeID, Month = Date1, Year = Date2, Org = SessionHelper.LoginUserOrganizationID, Product = Product, EmpID = LoggedInEmployeeID };
                var FullyReapids = dailyReportService.GetDataExpiredForcast_DateRangeReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);

                string monthName = new DateTime(2010, Convert.ToInt32(Date1), 1).ToString("MMM", CultureInfo.InvariantCulture);
                reportParam.Add("Date1", monthName);
                reportParam.Add("Date2", Date2);
                //reportParam.Add("Product", Product);

                //ReportHelper.PrintReport("rptFullyRep.rpt", FullyReapids.Tables[0], new Dictionary<string, object>());                    
                ReportHelper.PrintReport("rptExpiredForcast_DateRang.rpt", FullyReapids.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public JsonResult GetProductList()
        {
            var getProduct = productService.GetAll().Where(s => s.IsActive == true && s.OrgID == LoggedInOrganizationID).OrderBy(e => e.ProductCode);
            var viewProduct = getProduct.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = x.ProductCode.ToString() + ", " + x.ProductName.ToString()
            });
            var prod_items = new List<SelectListItem>();
            if (viewProduct.ToList().Count > 0)
            {
                prod_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            }
            prod_items.AddRange(viewProduct);
            return Json(prod_items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerateFullyrepaidReport(string ExportType)
        {
            try
            {

                var param = new { Office = SessionHelper.LoginUserOfficeID, Org = SessionHelper.LoginUserOrganizationID, EmpID = LoggedInEmployeeID };
                var FullyReapids = dailyReportService.GetDataFullyRepaidReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);

                if (ExportType == "pdf")
                {
                    ReportHelper.PrintReport("rptFullyRep.rpt", FullyReapids.Tables[0], reportParam);
                }
                else
                {
                    ReportHelper.ExportExcelReportFormated("rptFullyRep.rpt", FullyReapids.Tables[0], reportParam, "PaidOff_Today__Excel");
                }

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //public ActionResult GenerateFullyrepaidReport()
        //{
        //    try
        //    {

        //        var param = new { Office = SessionHelper.LoginUserOfficeID, Org = SessionHelper.LoginUserOrganizationID, EmpID =LoggedInEmployeeID};
        //        var FullyReapids = dailyReportService.GetDataFullyRepaidReport(param);

        //        var reportParam = new Dictionary<string, object>();
        //        reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);

        //        //ReportHelper.PrintReport("rptFullyRep.rpt", FullyReapids.Tables[0], new Dictionary<string, object>());                    
        //        ReportHelper.PrintReport("rptFullyRep.rpt", FullyReapids.Tables[0], reportParam);

        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        //public ActionResult GenerateFullyrepaid_DateRangeReport(string Qtype, string Date1, string Date2, string Product)
        //{
        //    try
        //    {

        //        var param = new { Qtype = Qtype, Office = SessionHelper.LoginUserOfficeID, Date1 = Date1, Date2 = Date2, Org = SessionHelper.LoginUserOrganizationID, Product = Product, EmpID = LoggedInEmployeeID };
        //        var FullyReapids = dailyReportService.GetDataFullyRepaid_DateRangeReport(param);

        //        var reportParam = new Dictionary<string, object>();
        //        reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //        reportParam.Add("Date1", Date1);
        //        reportParam.Add("Date2", Date2);
        //        //reportParam.Add("Product", Product);

        //        //ReportHelper.PrintReport("rptFullyRep.rpt", FullyReapids.Tables[0], new Dictionary<string, object>());                    
        //        ReportHelper.PrintReport("rptFullyRep_DateRang.rpt", FullyReapids.Tables[0], reportParam);

        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        #endregion

        #region Events
        //
        // GET: /FullyRepaid/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FRDateRange()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ProductList"] = items;
            
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            }            
            //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }
      
        //
        // GET: /FullyRepaid/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /FullyRepaid/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /FullyRepaid/Create
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

        //
        // GET: /FullyRepaid/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /FullyRepaid/Edit/5
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

        //
        // GET: /FullyRepaid/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /FullyRepaid/Delete/5
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

        public ActionResult ExpiredForcastDateRange()
        {

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ProductList"] = items;

            ViewBag.Months = new SelectList(Enumerable.Range(1, 12).Select(x =>
             new SelectListItem()
             {
                 Text = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames[x - 1] + " (" + x + ")",
                 Value = x.ToString()
             }), "Value", "Text");


                ViewBag.Years = new SelectList(Enumerable.Range(-DateTime.Today.Year, 10).Select(x =>

                   new SelectListItem()
                   {
                       Text = x.ToString().Replace("-", ""),
                       Value = x.ToString().Replace("-", ""),
                   }), "Value", "Text");
                return View();
            }




    }
}
        #endregion