using gBanker.Core.Utility;
using gBanker.Data.DBDetailModels;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Service.StoredProcedure;
using gBanker.Web.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gBanker.Web.Controllers
{
    public class OverdueMemberListController : BaseController
    {
        #region Variables

        private readonly IDailyReportService dailyReportService;
        private readonly IGroupwiseReportService groupwiseReportService;
        private readonly IUltimateReportService unlimitedReportService;
        private readonly IProductService productService;
        private readonly IEmployeeSPService employeespService;
        private readonly IOfficeService officeService;
        public OverdueMemberListController(IDailyReportService dailyReportService, IProductService productService, 
            IGroupwiseReportService groupwiseReportService,
            IEmployeeSPService employeespService,
            IUltimateReportService unlimitedReportService, IOfficeService officeService
)
        {
            this.dailyReportService = dailyReportService;
            this.groupwiseReportService = groupwiseReportService;
            this.unlimitedReportService = unlimitedReportService;
            this.productService = productService;
            this.employeespService = employeespService;
            this.officeService = officeService;


        }

        #endregion

        public ActionResult Overdue_Borrower_List_Disburse_Date_Range_Wise()
        {
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
            IEnumerable<SelectListItem> items = new SelectList(" ");

            ViewData["ProductList"] = items;
            ViewData["EmployeeList"] = items;
            return View();
        }

        public JsonResult GetEmployeeList_Overdue()
        {
            var param = new
            {
                office_val = LoginUserOfficeID
            };
            var holist = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "SP_GetEmployeeList_Overdue");
            List<ConstantDropdownItem> homodel = new List<ConstantDropdownItem>();
            if (holist != null && holist.Tables.Count > 0 && holist.Tables[0].Rows.Count > 0)
            {
                homodel = holist.Tables[0].AsEnumerable()
                .Select(row => new ConstantDropdownItem
                {
                    Value = row.Field<Int16>("Value").ToString(),
                    Text = row.Field<string>("Text")
                }).ToList();
            }
            return Json(homodel, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GenerateOverdueMemberListAllReportDisburseDateWise_Product_Employee(string DateFrom, string DateTo, string prod, string emp)
        {
            try
            {
                // PROC_RPT_OverDueLoaneeList_DisburseDateWise
                // @Org Int
                //, @Office Int
                //, @StartDate date   --Disburse Date
                //, @EndDate date     --Disburse Date
                //, @EmployeeId Int
                //, @ProductId Int

                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, StartDate = DateFrom, EndDate = DateTo, EmployeeId = Convert.ToInt32(emp), ProductId = Convert.ToInt32(prod) };
                var OverdueMls = groupwiseReportService.GetDataUltimateReleaseReport(param, "PROC_RPT_OverDueLoaneeList_DisburseDateWise");

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);

                //ReportHelper.PrintReport("rptNewOverDueMemberList.rpt", OverdueMls.Tables[0], new Dictionary<string, object>());                    
                ReportHelper.PrintReport("rptOverDueLoaneeListDisburseDateWise.rpt", OverdueMls.Tables[0], reportParam);


                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateOverdueMemberListAllReportExport_Product_Employee(string DateFrom, string DateTo, string prod, string emp)
        {

            var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, StartDate = DateFrom, EndDate = DateTo, EmployeeId = Convert.ToInt32(emp), ProductId = Convert.ToInt32(prod) };

            GridView gv = new GridView();
            var allRepaymentSchedule = groupwiseReportService.GetDataUltimateReleaseReport(param, "PROC_RPT_OverDueLoaneeList_DisburseDateWise");
            var detail = allRepaymentSchedule.Tables[0];
            gv.DataSource = detail;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=rptOverDueLoaneeListDisburseDateWise.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Overdue_Borrower_List_Disburse_Date_Range_Wise");
        }



        public ActionResult GenerateOverdueMemberListNewtReportExport_Employee(string Qtype, string OfficeId, string Month, string Year, string Employee)
        {

            var param =
                new
                {
                    LoginUserOfficeID = SessionHelper.LoginUserOfficeID,
                    LoginUserOrganizationID = SessionHelper.LoginUserOrganizationID,
                    Qtype = Convert.ToInt32(Qtype),
                    OfficeList = OfficeId,
                    EmployeeList = Employee,
                    Month = Convert.ToInt32(Month),
                    Year = Convert.ToInt32(Year)
                };

            GridView gv = new GridView();
            var allRepaymentSchedule = groupwiseReportService.GetDataUltimateReleaseReport(param, "Proc_Get_DueLoan_Employee");
            var detail = allRepaymentSchedule.Tables[0];
            gv.DataSource = detail;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=rptNewOverDueMemberList.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Index");
        }

        public ActionResult GenerateOverdueMemberListNewtReport_Employee(string Qtype, string OfficeId, string Month, string Year, string Employee)
        {
            try
            {
                var param =
                    new
                    {
                        LoginUserOfficeID = SessionHelper.LoginUserOfficeID,
                        LoginUserOrganizationID = SessionHelper.LoginUserOrganizationID,
                        Qtype = Convert.ToInt32(Qtype),
                        OfficeList = OfficeId,
                        EmployeeList = Employee,
                        Month = Convert.ToInt32(Month),
                        Year = Convert.ToInt32(Year)
                    };
                var OverdueMls = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "Proc_Get_DueLoan_Employee");

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);

                // Get the first day of the month
                DateTime firstDayOfMonth = new DateTime(Convert.ToInt32(Year), Convert.ToInt32(Month), 1);
                // Get the last day of the month
                DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                reportParam.Add("DateFrom", firstDayOfMonth.ToShortDateString());
                reportParam.Add("DateTo", lastDayOfMonth.ToShortDateString());
                ReportHelper.PrintReport("rptNewOverDueMemberList.rpt", OverdueMls.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult Index_Employee()
        {

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["EmployeeList"] = items;
            ViewData["MonthList"] = items;
            ViewData["YearList"] = items;
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            var HeadOfficeCode = offcdetail.FirstLevel;
            var Headoffcdetail = officeService.GetByOfficeCode(HeadOfficeCode);
            var Secondoffcdetail = officeService.GetByOfficeCode(offcdetail.SecondLevel);
            var Thirdoffcdetail = officeService.GetByOfficeCode(offcdetail.ThirdLevel);
            var Fourthoffcdetail = officeService.GetByOfficeCode(offcdetail.FourthLevel);

            ViewData["OfficeLevel"] = offcdetail.OfficeLevel;

            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = Headoffcdetail.OfficeID;
                ViewData["SecondLevel"] = "";
                ViewData["ThirdLevel"] = "";
                ViewData["FourthLevel"] = "";
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = Headoffcdetail.OfficeID;
                ViewData["SecondLevel"] = Secondoffcdetail.OfficeID;
                ViewData["ThirdLevel"] = "";
                ViewData["FourthLevel"] = "";
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = Headoffcdetail.OfficeID;
                ViewData["SecondLevel"] = Secondoffcdetail.OfficeID;
                ViewData["ThirdLevel"] = Thirdoffcdetail.OfficeID;
                ViewData["FourthLevel"] = "";
            }
            else
            {
                ViewData["FirstLevel"] = Headoffcdetail.OfficeID;
                ViewData["SecondLevel"] = Secondoffcdetail.OfficeID;
                ViewData["ThirdLevel"] = Thirdoffcdetail.OfficeID;
                ViewData["FourthLevel"] = Fourthoffcdetail.OfficeID;

            }

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
            return View();
        }

        public JsonResult GetHOList()
        {
            var param = new
            {
                LoginUserOfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID),
                LoggedInOrganizationID = Convert.ToInt32(LoggedInOrganizationID)
            };
            var holist = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "SP_GetHOList");
            List<ConstantDropdownItem> homodel = new List<ConstantDropdownItem>();
            if (holist != null && holist.Tables.Count > 0 && holist.Tables[0].Rows.Count > 0)
            {
                homodel = holist.Tables[0].AsEnumerable()
                .Select(row => new ConstantDropdownItem
                {
                    Value = row.Field<int>("Value").ToString(),
                    Text = row.Field<string>("Text")
                }).ToList();
            }
            return Json(homodel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMonthList()
        {
            var holist = employeespService.GetDataWithoutParameter("SP_GetMonths");
            List<ConstantDropdownItem> homodel = new List<ConstantDropdownItem>();
            if (holist != null && holist.Tables.Count > 0 && holist.Tables[0].Rows.Count > 0)
            {
                homodel = holist.Tables[0].AsEnumerable()
                .Select(row => new ConstantDropdownItem
                {
                    Value = row.Field<string>("Value"),
                    Text = row.Field<string>("Text")
                }).ToList();
            }
            return Json(homodel, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetYearList()
        {
            var holist = employeespService.GetDataWithoutParameter("SP_GetYears");
            List<ConstantDropdownItem> homodel = new List<ConstantDropdownItem>();
            if (holist != null && holist.Tables.Count > 0 && holist.Tables[0].Rows.Count > 0)
            {
                homodel = holist.Tables[0].AsEnumerable()
                .Select(row => new ConstantDropdownItem
                {
                    Value = row.Field<string>("Value"),
                    Text = row.Field<string>("Text")
                }).ToList();
            }
            return Json(homodel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetZoneList_multiple(string HO_val)
        {
            var param = new
            {
                HO_val = Convert.ToInt32(HO_val),
                OrgID = Convert.ToInt32(LoggedInOrganizationID)
            };
            var holist = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "SP_GetZoneList");
            List<ConstantDropdownItem> homodel = new List<ConstantDropdownItem>();
            if (holist != null && holist.Tables.Count > 0 && holist.Tables[0].Rows.Count > 0)
            {
                homodel = holist.Tables[0].AsEnumerable()
                .Select(row => new ConstantDropdownItem
                {
                    Value = row.Field<int>("Value").ToString(),
                    Text = row.Field<string>("Text")
                }).ToList();
            }
            return Json(homodel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAreaList_multiple(string HO_val, string zone_val)
        {
            var param = new
            {
                HO_val = Convert.ToInt32(HO_val),
                zone_val = zone_val,
                OrgID = Convert.ToInt32(LoggedInOrganizationID)
            };
            var holist = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "SP_GetAreaList");
            List<ConstantDropdownItem> homodel = new List<ConstantDropdownItem>();
            if (holist != null && holist.Tables.Count > 0 && holist.Tables[0].Rows.Count > 0)
            {
                homodel = holist.Tables[0].AsEnumerable()
                .Select(row => new ConstantDropdownItem
                {
                    Value = row.Field<int>("Value").ToString(),
                    Text = row.Field<string>("Text")
                }).ToList();
            }
            return Json(homodel, JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetOfficeList_multiple(string HO_val, string zone_val, string area_val)
        {
            var param = new
            {
                HO_val = Convert.ToInt32(HO_val),
                zone_val = zone_val,
                area_val = area_val,
                OrgID = Convert.ToInt32(LoggedInOrganizationID)
            };
            var holist = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "SP_GetOfficeList");
            List<ConstantDropdownItem> homodel = new List<ConstantDropdownItem>();
            if (holist != null && holist.Tables.Count > 0 && holist.Tables[0].Rows.Count > 0)
            {
                homodel = holist.Tables[0].AsEnumerable()
                .Select(row => new ConstantDropdownItem
                {
                    Value = row.Field<int>("Value").ToString(),
                    Text = row.Field<string>("Text")
                }).ToList();
            }
            return Json(homodel, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetEmployeeList_multiple(string office_val)
        {
            var param = new
            {
                office_val = office_val
            };
            var holist = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "SP_GetEmployeeList");
            List<ConstantDropdownItem> homodel = new List<ConstantDropdownItem>();
            if (holist != null && holist.Tables.Count > 0 && holist.Tables[0].Rows.Count > 0)
            {
                homodel = holist.Tables[0].AsEnumerable()
                .Select(row => new ConstantDropdownItem
                {
                    Value = row.Field<Int16>("Value").ToString(),
                    Text = row.Field<string>("Text")
                }).ToList();
            }
            return Json(homodel, JsonRequestBehavior.AllowGet);
        }




        public JsonResult GetProductList()
        {
            var getProduct = productService.GetAll().Where(s => s.IsActive == true && s.OrgID == LoggedInOrganizationID).OrderBy(e => e.ProductCode);
            var viewProduct = getProduct.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = x.ProductCode + ' ' + x.ProductName.ToString()
            });
            var prod_items = new List<SelectListItem>();
            if (viewProduct.ToList().Count > 0)
            {
                prod_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            }
            prod_items.AddRange(viewProduct);
            return Json(prod_items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerateOverdueMemberListNewtReportMainProduct(string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Org = SessionHelper.LoginUserOrganizationID };
                var OverdueMls = dailyReportService.GetDataNewOverdueMemberListReport(param);
                //var alldata = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "Proc_Get_DueLoan");

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);


                //ReportHelper.PrintReport("rptNewOverDueMemberList.rpt", OverdueMls.Tables[0], new Dictionary<string, object>());                    
                ReportHelper.PrintReport("rptNewOverDueMemberListMPW.rpt", OverdueMls.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateOverdueMemberListNewtReportExportMainProduct(string DateFrom, string DateTo)
        {
            var param = new { Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Org = SessionHelper.LoginUserOrganizationID };
            groupwiseReportService.GetDataUltimateReleaseReport(param, "Proc_Get_DueLoan");

            GridView gv = new GridView();
            var allRepaymentSchedule = groupwiseReportService.GetDataUltimateReleaseReport(param, "Proc_Get_DueLoan");
            var detail = allRepaymentSchedule.Tables[0];
            gv.DataSource = detail;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=rptNewOverDueMemberListMainProduct.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("OverdueMemberListNewReportMainProduct");
        }
        public ActionResult GenerateOverdueMemberListNewtReport(string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Org = SessionHelper.LoginUserOrganizationID };
                var OverdueMls = dailyReportService.GetDataNewOverdueMemberListReport(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptNewOverDueMemberList.rpt", OverdueMls.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateOverdueNewMemberListNewtReport(string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Org = SessionHelper.LoginUserOrganizationID };
                var OverdueMls = dailyReportService.GetDataNewOverdueNewMemberListReport(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptNewOverDueMemberListThisMonth.rpt", OverdueMls.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult GenerateOverdueMemberListNewtReportExport(string DateFrom, string DateTo)
        {
            var param = new { Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Org = SessionHelper.LoginUserOrganizationID };
            groupwiseReportService.GetDataUltimateReleaseReport(param, "Proc_Get_DueLoan");

            GridView gv = new GridView();
            var allRepaymentSchedule = groupwiseReportService.GetDataUltimateReleaseReport(param, "Proc_Get_DueLoan");
            var detail = allRepaymentSchedule.Tables[0];
            gv.DataSource = detail;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=rptNewOverDueMemberList.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Index");
        }
        public ActionResult GenerateOverdueMemberListNewtReportExport_RDRS(string DateFrom, string DateTo)
        {
            var param = new { Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Org = SessionHelper.LoginUserOrganizationID };
            groupwiseReportService.GetDataUltimateReleaseReport(param, "PROC_RPT_OverDueLoaneeList_DisburseDateWise");

            GridView gv = new GridView();
            var allRepaymentSchedule = groupwiseReportService.GetDataUltimateReleaseReport(param, "PROC_RPT_OverDueLoaneeList_DisburseDateWise");
            var detail = allRepaymentSchedule.Tables[0];
            gv.DataSource = detail;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=rptNewOverDueMemberList.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Index");
        }

        public ActionResult GenerateOverdueMemberListAllReportMainProduct(string DateTo, string productMainCode = "", string dueType = "")
        {
            try
            {
                //get data new overdue member list all report
                var overdueMls = GetDataNewOverdueMemberListAllReport(DateTo, productMainCode, dueType);

                //var alldata = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "Rpt_OverDueLoaneeList_All");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptOverDueLoaneeListAllMPW.rpt", overdueMls.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateOverdueMemberListAllReportExportMainProduct(string DateTo, string productMainCode = "", string dueType = "")
        {
            //get data new overdue member list all report for export
            var allRepaymentSchedule = GetDataNewOverdueMemberListAllReportForExport(DateTo, productMainCode, dueType);

            GridView gv = new GridView();

            var detail = allRepaymentSchedule.Tables[0];
            gv.DataSource = detail;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=rptOverDueLoaneeListAllMainProduct.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return RedirectToAction("OverdueMemberListAllReportMainProduct");

        }
        public ActionResult OverdueMemberListAllReportNew()
        {
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
            IEnumerable<SelectListItem> items = new SelectList(" ");

            ViewData["ProductList"] = items;

            return View();
        }
        public ActionResult GenerateOverdueMemberListAllReportNew(string DateTo, string qType, string prod, string period)
        {
            try
            {
                if (qType == "1")
                {
                    var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = DateTo, prod = prod };
                    var OverdueMls = dailyReportService.GetDataNewOverdueMemberListAllReportProductWise(param);

                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    //reportParam.Add("DateFrom", DateFrom);
                    reportParam.Add("DateTo", DateTo);                   
                    //ReportHelper.PrintReport("rptOverDueLoaneeListProductWise.rpt", OverdueMls.Tables[0], reportParam);
                    ReportHelper.PrintReport("rptOverDueLoaneeListAllPeriodType_Biva.rpt", OverdueMls.Tables[0], reportParam);
                    
                }
                else if (qType == "2")
                {
                    var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = DateTo };
                    var OverdueMls = dailyReportService.GetDataNewOverdueMemberListAllReport(param);

                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    //reportParam.Add("DateFrom", DateFrom);
                    reportParam.Add("DateTo", DateTo);


                    //ReportHelper.PrintReport("rptOverDueLoaneeListAll.rpt", OverdueMls.Tables[0], reportParam);
                    ReportHelper.PrintReport("rptOverDueLoaneeListAllPeriodType_Biva.rpt", OverdueMls.Tables[0], reportParam);
                    
                }

                else if (qType == "3")
                {
                    var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, EndDate = DateTo, period = period };
                    var OverdueMls = dailyReportService.GetDataNewOverduePeriodListAllReport(param);
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    //reportParam.Add("DateFrom", DateFrom);
                    reportParam.Add("DateTo", DateTo);
                    ReportHelper.PrintReport("rptOverDueLoaneeListAllPeriodType_Biva.rpt", OverdueMls.Tables[0], reportParam);
                    //ReportHelper.PrintReport("rptOverDueLoaneeListAllPeriodType.rpt", OverdueMls.Tables[0], reportParam);

                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateOverdueMemberListAllReport(string DateTo, string qType, string prod)
        {
            try
            {
                if (qType == "2")
                {
                    var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = DateTo };
                    DataSet OverdueMls;
                    if (LoggedInOrganizationID == 190)
                    {
                        OverdueMls = dailyReportService.GetDataNewOverdueMemberListAllReportRDRS(param);
                    }
                    else if (LoggedInOrganizationID == 6)
                    {
                        OverdueMls = dailyReportService.GetDataNewOverdueMemberListAllReportPrayas(param);
                    }
                    else
                     OverdueMls = dailyReportService.GetDataNewOverdueMemberListAllReport(param);

                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    //reportParam.Add("DateFrom", DateFrom);
                    reportParam.Add("DateTo", DateTo);


                    //ReportHelper.PrintReport("rptNewOverDueMemberList.rpt", OverdueMls.Tables[0], new Dictionary<string, object>());                    
                    ReportHelper.PrintReport("rptOverDueLoaneeListAll.rpt", OverdueMls.Tables[0], reportParam);
                }
                else if (qType == "1")
                {

                    var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = DateTo, prod = prod };
                    DataSet OverdueMls;
                     
                    if (LoggedInOrganizationID == 190)
                    {
                        OverdueMls = dailyReportService.GetDataNewOverdueMemberListAllReportProductWiseRDRS(param);
                    }
                    else
                    {
                        OverdueMls = dailyReportService.GetDataNewOverdueMemberListAllReportProductWise(param);
                    }
                        var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    //reportParam.Add("DateFrom", DateFrom);
                    reportParam.Add("DateTo", DateTo);


                    //ReportHelper.PrintReport("rptNewOverDueMemberList.rpt", OverdueMls.Tables[0], new Dictionary<string, object>());                    
                    ReportHelper.PrintReport("rptOverDueLoaneeListProductWise.rpt", OverdueMls.Tables[0], reportParam);
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult GenerateOverdueMemberListAllReport_RDRS(string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, StartDate = DateFrom, EndDate = DateTo, EmployeeId = LoggedInEmployeeID, ProductId = 0 };
                var OverdueMls = dailyReportService.GetDataNewOverdueMemberListReportDisburseDateWise(param);
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptOverDueLoaneeListDisburseDateWise_RDRS.rpt", OverdueMls.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateOverdueMemberListAllReportDisburseDateWise(string DateFrom, string DateTo)
        {
            try
            {

                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo };
                var OverdueMls = dailyReportService.GetDataNewOverdueMemberListAllReportDisburseDateWise(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);


                //ReportHelper.PrintReport("rptNewOverDueMemberList.rpt", OverdueMls.Tables[0], new Dictionary<string, object>());                    
                ReportHelper.PrintReport("rptOverDueLoaneeListAll.rpt", OverdueMls.Tables[0], reportParam);


                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateOverdueMemberListAllReportExport(string DateTo)
        {
            //try
            //{
            var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = DateTo };
            groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_OverDueLoaneeList_All");

            GridView gv = new GridView();
            var allRepaymentSchedule = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_OverDueLoaneeList_All");
            var detail = allRepaymentSchedule.Tables[0];
            gv.DataSource = detail;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=rptOverDueLoaneeListAll.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("OverdueMemberListAllReport");
        }
        public ActionResult OverdueMemberListAllReport_RDRS()
        {
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
            IEnumerable<SelectListItem> items = new SelectList(" ");

            ViewData["ProductList"] = items;

            return View();
        }
        public ActionResult OverdueMemberListAllReport()
        {
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
            IEnumerable<SelectListItem> items = new SelectList(" ");

            ViewData["ProductList"] = items;

            return View();
        }
        // GET: /OverdueMemberList/
        public ActionResult OverdueMemberListNewReportMainProduct()
        {
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
            return View();
        }
        public ActionResult OverdueMemberListAllReportMainProduct()
        {
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
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ProductList"] = items;
            ViewData["DueType"] = items;

            return View();
        }

        //public ActionResult OverdueMemberListAllReport()
        //{
        //    DateTime VDate;
        //    VDate = System.DateTime.Now;
        //    if (IsDayInitiated)
        //    {
        //        ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
        //    }
        //    else
        //    {
        //        ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
        //    }
        //    IEnumerable<SelectListItem> items = new SelectList(" ");

        //    ViewData["ProductList"] = items;

        //    return View();
        //}

        public ActionResult OverdueMemberListAllReport_DisburseDateWise()
        {
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
            IEnumerable<SelectListItem> items = new SelectList(" ");

            ViewData["ProductList"] = items;

            return View();
        }
        public ActionResult Index()
        {
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
            return View();
        }

        //
        // GET: /OverdueMemberList/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /OverdueMemberList/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /OverdueMemberList/Create
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
        // GET: /OverdueMemberList/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /OverdueMemberList/Edit/5
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
        // GET: /OverdueMemberList/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /OverdueMemberList/Delete/5
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


        #region Ajax Calls

        public JsonResult GetProductListForOverdue()
        {

            var getProduct = productService.GetMany(s => s.IsActive == true && s.OrgID == LoggedInOrganizationID && s.MainProductCode !=null && s.MainProductCode !="" ).GroupBy(g=>g.MainProductCode).Select(f=>f.FirstOrDefault()).OrderBy(e => e.ProductCode);
            var viewProduct = getProduct.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.MainProductCode.ToString(),
                Text = $@"{x.MainProductCode.ToString()} - {x.ProductName.ToString()}"
            });
            var prod_items = new List<SelectListItem>();
            if (viewProduct.ToList().Count > 0)
            {
                prod_items.Add(new SelectListItem() { Text = "Select All", Value = "", Selected = true });
            }
            prod_items.AddRange(viewProduct);
            return Json(prod_items, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private Methods

        private DataSet GetDataNewOverdueMemberListAllReport(string dateTo,string productMainCode="", string dueType="")
        {
            if (SessionHelper.LoginUserOrganizationID == MFIConstants.Society_For_Social_Service_SSS)
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = dateTo, productMainCode = productMainCode, dueType=dueType };
                var OverdueMls = dailyReportService.GetDataNewOverdueMemberListAllReportByFilter(param);

                return OverdueMls;
            }
            else
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = dateTo };
                var OverdueMls = dailyReportService.GetDataNewOverdueMemberListAllReport(param);
                return OverdueMls;
            }
        }

        private DataSet GetDataNewOverdueMemberListAllReportForExport(string dateTo, string productMainCode = "", string dueType = "")
        {
            if (SessionHelper.LoginUserOrganizationID == MFIConstants.Society_For_Social_Service_SSS)
            {                
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = dateTo, productMainCode = productMainCode, dueType = dueType };                
                var allRepaymentSchedule = groupwiseReportService.ExportExcellData(param, "Rpt_OverDueLoaneeList_All_By_Filter");

                return allRepaymentSchedule;
            }
            else
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = dateTo };
                var allRepaymentSchedule = groupwiseReportService.ExportExcellData(param, "Rpt_OverDueLoaneeList_All");
                return allRepaymentSchedule;
            }
        }

        #endregion
    }
}
