using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportExecutionService;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.Reports;
using gBanker.Web.ViewModels;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kendo.Mvc.Extensions;
using Microsoft.Ajax.Utilities;

namespace gBanker.Web.Controllers
{
    public class SSRSReportController : BaseController
    {

        #region Variables
        private readonly IAccTrxMasterService accTrxMasterService;
        private readonly IAccTrxDetailService accTrxDetailService;
        private readonly IAccChartService accChartService;
        private readonly IAccLastVoucherService accLastVoucherService;
        private readonly IProcessInfoService processInfoService;
        private readonly IAccReportService accReportService;
        private readonly IOfficeService officeService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IWeeklyReportService weeklyReportService;
        private readonly ICenterService centerService;
        private readonly IMemberService memberService;

        //private Office offcdetail;
        //private string HeadOfficeCode;
        //private Office Headoffcdetail;
        //private Office Secondoffcdetail;
        //private Office Thirdoffcdetail;
        //private Office Fourthoffcdetail;
        
        public SSRSReportController(IAccTrxMasterService accTrxMasterService, IAccTrxDetailService accTrxDetailService, IAccChartService accChartService, IAccLastVoucherService accLastVoucherService, IProcessInfoService processInfoService,
            IAccReportService accReportService, IOfficeService officeService, IUltimateReportService ultimateReportService, IWeeklyReportService weeklyReportService, ICenterService centerService, IMemberService memberService)
        {
            this.accTrxMasterService = accTrxMasterService;
            this.accTrxDetailService = accTrxDetailService;
            this.accChartService = accChartService;
            this.accLastVoucherService = accLastVoucherService;
            this.processInfoService = processInfoService;
            this.accReportService = accReportService;
            this.officeService = officeService;
            this.ultimateReportService = ultimateReportService;
            this.weeklyReportService = weeklyReportService;
            this.centerService = centerService;
            this.memberService = memberService;

            //offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));            
            //HeadOfficeCode = offcdetail.FirstLevel;
            //Headoffcdetail = officeService.GetByOfficeCode(HeadOfficeCode);
            //Secondoffcdetail = officeService.GetByOfficeCode(offcdetail.SecondLevel);
            //Thirdoffcdetail = officeService.GetByOfficeCode(offcdetail.ThirdLevel);
            //Fourthoffcdetail = officeService.GetByOfficeCode(offcdetail.FourthLevel);            
        }
        #endregion

        #region 

        public ActionResult CashBookLedger() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["CashBookList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForAccLevel();
            return View();
        }
        public ActionResult GenerateCashBookLedgerReport(string office_id, string from_date, string to_date, int acc_level, int acc_Id)
        {
            try
            {
                var paramValues = new List<ParameterValue>();

                paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "to_date", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "office_id", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = acc_level.ToString() });
                paramValues.Add(new ParameterValue() { Name = "AccId", Value = acc_Id.ToString() });

                PrintSSRSReport("/gBanker_Reports/ASI_Cashbook", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult CashBook() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {

                //ViewData["TrxDate"] = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForAccLevel();
            return View();
        }
        public ActionResult GenerateCashBookReport(string office_id, string from_date, string to_date, int acc_level, int acc_Id)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "to_date", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "office_id", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = acc_level.ToString() });
                // paramValues.Add(new ParameterValue() { Name = "AccId", Value = acc_Id.ToString() });

                PrintSSRSReport("/gBanker_Reports/AIS_Cash_Book", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult CenterList() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }
            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {

                //ViewData["TrxDate"] = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForAccLevel();
            return View();
        }
        public ActionResult GenerateCenterList(string office_id, string from_date = ""
              , string to_date = "", int acc_level = 0, int acc_Id = 0
              , string type = "", string typeData = "", string status = "")
        {
            try
            {

                var orgId = SessionHelper.LoginUserOrganizationID;

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Org", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Office", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "CenterStatus", Value = status });
                paramValues.Add(new ParameterValue() { Name = "type", Value = type });
                paramValues.Add(new ParameterValue() { Name = "typeData", Value = (string.IsNullOrEmpty(typeData) ? "" : typeData) });
                //paramValues.Add(new ParameterValue() { Name = "CenterStatus", Value = status });

                PrintSSRSReport("/gBanker_Reports/MIS_CenterList", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult GenerateCenterListExport(string office_id, string from_date = "", string to_date = "", int acc_level = 0, int acc_Id = 0)
        {
            try
            {
                var CenterStatus = 1;
                int Center = 0;
                int Member = 0;
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, CenterStatus = CenterStatus};
                GridView gv = new GridView();
                var allRepaymentSchedule = weeklyReportService.CenterExport(param);
                var detail = allRepaymentSchedule.Tables[0];
                gv.DataSource = detail;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=CenterExport.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return RedirectToAction("CenterExport");
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GeneralSavingsCollectionSheet() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {

                //ViewData["TrxDate"] = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            DropdownForContractualSavingsQType();
            return View();
        }
        public ActionResult GenerateSavingsCollectionSheet(string office_id, int Qtype, int EmpId, int CenterId, int ProductId, string Date = "")
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Qtype", Value = Qtype.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Org", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeId", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "EmpId", Value = EmpId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "CenterId", Value = CenterId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "ProductId", Value = ProductId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Date", Value = Date.ToString() });


                PrintSSRSReport("/gBanker_Reports/MIS_General_Savings-1_Collection_Statement", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult BalanceSheetHO() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {

                //ViewData["TrxDate"] = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForAccLevel();
            return View();
        }
        public ActionResult GenerateAISBalanceSheet(string office_id, string date = "", int AccLevel = 0, int AllOffice = 0)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "to_date", Value = date.ToString() });
                paramValues.Add(new ParameterValue() { Name = "office_id", Value = office_id.ToString() });
                paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = AccLevel.ToString() });
                paramValues.Add(new ParameterValue() { Name = "And_Condition", Value = "" });
                paramValues.Add(new ParameterValue() { Name = "AllOffice", Value = AllOffice.ToString() });
                PrintSSRSReport("/gBanker_Reports/AIS_BalanceSheet_HO_New", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult AISBalanceSheet() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {

                //ViewData["TrxDate"] = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForAccLevel();
            return View();
        }
        public ActionResult GenerateAISBalanceSheetN(string office_id, string date = "", int AccLevel = 0, string And_Condition = "", int AllOffice = 0)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "to_date", Value = date.ToString() });
                paramValues.Add(new ParameterValue() { Name = "office_id", Value = office_id.ToString() });
                paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = AccLevel.ToString() });
                paramValues.Add(new ParameterValue() { Name = "And_Condition", Value = And_Condition.ToString() });
                paramValues.Add(new ParameterValue() { Name = "AllOffice", Value = AllOffice.ToString() });


                PrintSSRSReport("/gBanker_Reports/AIS_BalanceSheet", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult TrialBalanceAccCodeWise() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;
            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {

                //ViewData["TrxDate"] = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForAccLevel();
            return View();
        }
        public ActionResult GenerateAccTrialBalanceAccWise(string office_id, string from_date, string to_date, string AccLevel, string AccCode)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "to_date", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = AccLevel });
                paramValues.Add(new ParameterValue() { Name = "AccCode", Value = AccCode });


                PrintSSRSReport("/gBanker_Reports/MIS_Ledger_Book", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        private void MapDropdownForBranchLoanPortfolioStatement()
        {
            var qTypeList = new List<SelectListItem>();
            qTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            qTypeList.Add(new SelectListItem { Text = "Product Wise (Main)", Value = "1" });
            qTypeList.Add(new SelectListItem { Text = "Product Wise (Details)", Value = "0" });
            ViewData["QTypeList"] = qTypeList;
            //var qTypeList = new List<SelectListItem>();
            //qTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            //qTypeList.Add(new SelectListItem { Text = "Product Wise", Value = "1" });
            //ViewData["QTypeList"] = qTypeList;
        }
        public ActionResult BranchLoanPortfolioStatement() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {

                //ViewData["TrxDate"] = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForBranchLoanPortfolioStatement();
            return View();
        }
        public ActionResult GenerateBranchLoanPortfolioStatement(string office_id, string from_date, string to_date, int qType)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "QType", Value = qType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OrgID", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "EmpID", Value = "0" });
                paramValues.Add(new ParameterValue() { Name = "FirstDate", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "ToDate", Value = to_date });


                PrintSSRSReport("/gBanker_Reports/AIS_Loan_Portfolio_Statement", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult MonthlyRebateList() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {

                //ViewData["TrxDate"] = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForLoanAging();
            return View();
        }
        public ActionResult GenerateMonthlyRebateList(string office_id, string from_date, string to_date, int qType)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;

                var paramValues = new List<ParameterValue>();
                //paramValues.Add(new ParameterValue() { Name = "QType", Value = qType.ToString() });
                //paramValues.Add(new ParameterValue() { Name = "OrgID", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = office_id });
                //paramValues.Add(new ParameterValue() { Name = "EmpID", Value = "0" });
                paramValues.Add(new ParameterValue() { Name = "Date1", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "Date2", Value = to_date });


                PrintSSRSReport("/gBanker_Reports/MIS_Monthly_Rebeat_List", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult AccountTransactionDetailsLoan() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            ViewData["ProductListByMember"] = items;
            ViewData["LoanTermList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {

                //ViewData["TrxDate"] = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate;
            }
            //MapDropdownForLoanAging();
            return View();
        }
        public ActionResult GenerateAccountTransactionDetailsLoan(string Qtype, string Center, string office_id, string Member, string ProductID, string loanterm, string from_date, string to_date)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var OfficeData = officeService.GetById(Convert.ToInt32(office_id));
                //int qtype = 1;
                //int productId = 0;
                //int LoanTerm = 0;

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Qtype", Value = Qtype });
                paramValues.Add(new ParameterValue() { Name = "Org", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Office", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "Center", Value = Center });
                paramValues.Add(new ParameterValue() { Name = "Member", Value = Member });
                paramValues.Add(new ParameterValue() { Name = "ProductID", Value = ProductID });
                paramValues.Add(new ParameterValue() { Name = "LoanTerm", Value = loanterm });
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });


                PrintSSRSReport("/gBanker_Reports/AIS_Account_Transaction_Details", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult LoanCollectionSheet() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            //ViewData["OfficeLevel"] = offcdetail.OfficeLevel;
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {

                //ViewData["TrxDate"] = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForLoanAgingAndCenterList();
            return View();
        }
        public ActionResult GenerateLoanCollectionSheet(string office_id, string from_date, string to_date, int qType, int centerID)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;

                var OfficeData = officeService.GetById(Convert.ToInt32(office_id));
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Qtype", Value = qType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Org", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeId", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "EmpId", Value = "0" });
                paramValues.Add(new ParameterValue() { Name = "CenterId", Value = centerID.ToString() });
                paramValues.Add(new ParameterValue() { Name = "ProductId", Value = "0" });
                //paramValues.Add(new ParameterValue() { Name = "LoanTerm", Value = "0" });
                paramValues.Add(new ParameterValue() { Name = "Date", Value = from_date });
                //paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });


                PrintSSRSReport("/gBanker_Reports/LoanCollectionSheet", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public ActionResult ResultAccountsInfo2() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = offcdetail.OfficeLevel;
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {

                //ViewData["TrxDate"] = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForLoanAging();
            return View();
        }
        public ActionResult GenerateResultAccountsInfo2(string office_id, string from_date, string to_date, int qType)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;

                var OfficeData = officeService.GetById(Convert.ToInt32(office_id));

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Qtype", Value = qType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OrgID", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeId", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });


                PrintSSRSReport("/gBanker_Reports/AccountsInfo_2", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult ResultAccountsInfo1() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = offcdetail.OfficeLevel;
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {

                //ViewData["TrxDate"] = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForLoanAging();
            return View();
        }
        public ActionResult GenerateResultAccountsInfo1(string office_id, string from_date, string to_date, int qType)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;

                var OfficeData = officeService.GetById(Convert.ToInt32(office_id));

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Qtype", Value = qType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OrgID", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeId", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });


                PrintSSRSReport("/gBanker_Reports/AccountsInfo_1", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion k
        //er
        // Starts Sabet
        private void MapDropdownForBuroSavimngsPortfolioStatement()
        {
            var qTypeList = new List<SelectListItem>();
            qTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            qTypeList.Add(new SelectListItem { Text = "All", Value = 2.ToString() });
            ViewData["QTypeList"] = qTypeList;
        }
        public ActionResult BuroSavimngsPortfolioStatement() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList("");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["OrganizerList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForBuroSavimngsPortfolioStatement();

            return View();
        }
        public ActionResult GenerateTSavingsPortfolioStatementReport(int qType, string office_id, string Emp_Id, string from_date, string to_date)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Qtype", Value = qType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = office_id });
                //paramValues.Add(new ParameterValue() { Name = "EmpId", Value = EmpId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "EmpID", Value = Emp_Id.ToString() });
                //paramValues.Add(new ParameterValue() { Name = "EmpId", Value = Emp_Id.ToString() });
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });

                PrintSSRSReport("/gBanker_Reports/MIS_Proc_RPT_BuroSavimngsPortfolioStatement", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        private void MapDropdownForInterestReport()
        {
            var interestList = new List<SelectListItem>();
            interestList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            interestList.Add(new SelectListItem { Text = "Member Wise", Value = "4" });
            interestList.Add(new SelectListItem { Text = "Samity Wise", Value = "5" });
            interestList.Add(new SelectListItem { Text = "Product Wise", Value = "6" });
            ViewData["InterestTypeList"] = interestList;
        }
        public ActionResult InterestReport()
        {
            IEnumerable<SelectListItem> items = new SelectList("");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);
            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForInterestReport();
            return View();
        }
        public ActionResult GenerateMemberWiseInterestReport(int qType, int office_id, string to_date)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Qtype", Value = qType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Org", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Office", Value = office_id.ToString() });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });
                if (qType == 4)
                {
                    PrintSSRSReport("/gBanker_Reports/InterestReportMemberWise", paramValues.ToArray(), "gBankerReportMIS");
                }
                if (qType == 5)
                {
                    PrintSSRSReport("/gBanker_Reports/InterestsReportSamityWise", paramValues.ToArray(), "gBankerReportMIS");
                }
                if (qType == 6)
                {
                    PrintSSRSReport("/gBanker_Reports/InterestsReportProductWise", paramValues.ToArray(), "gBankerReportMIS");
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult MonthlyOverdueStatement() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }

            return View();
        }
        public ActionResult GenerateMonthlyOverdueReport(string office_id, string to_date)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Org", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Office", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });

                PrintSSRSReport("/gBanker_Reports/Monthly_Overdue_List", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult GetAccName(int accLevel)
        {

            var accName = accChartService.GetAll().Where(p => p.IsActive == true && p.AccLevel == accLevel).ToList();
            var viewAccName = accName.AsEnumerable().Select(p => new SelectListItem
            {
                Text = string.Format("{0}-{1}", p.AccCode, p.AccName),
                Value = p.AccID.ToString()
            }).ToList();
            var accNameList = new List<SelectListItem>();
            accNameList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            accNameList.AddRange(viewAccName);
            return Json(accNameList, JsonRequestBehavior.AllowGet);
          
        }
        public ActionResult GenerateBorrowerDataReport(string dateTo, string isChecked)
        {
            int OfficeID;
            int OfficeIDTo;
            try
            {
                if (isChecked == "true")
                {
                    OfficeID = 00000;
                    OfficeIDTo = 99999;
                }
                else
                {
                    OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
                    OfficeIDTo = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
                }
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = OfficeID.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeIDTo", Value = OfficeIDTo.ToString() });
                paramValues.Add(new ParameterValue() { Name = "PrintDate", Value = dateTo });
                PrintSSRSReport("/gBanker_Reports/BorrowersData", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult BorrowersData()
        {
            return View();
        }

        public JsonResult GetAccNameBUro(int accLevel)
        {

            //Bappa
            var paramG = new { @Qtype = 1 };
            var model = ultimateReportService.getAccBuroCode(paramG);
            string vAccCode;
            if (model.Tables[0].Rows.Count > 0)
            {
                List<AccChartViewModel> List_ProductViewModel = new List<AccChartViewModel>();
                var div_items = ultimateReportService.getAccBuroCode(paramG);
                List_ProductViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new AccChartViewModel
            {
                AccID = row.Field<int>("AccID"),
                AccCode = row.Field<string>("AccCode"),
                AccName = row.Field<string>("AccName")
            }).ToList();

                var viewProduct = List_ProductViewModel.Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.AccID.ToString(),
                    Text = x.AccCode.ToString() + " " + x.AccName.ToString()
                });
                var accNameList = new List<SelectListItem>();
                accNameList.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
                accNameList.AddRange(viewProduct);
                return Json(accNameList, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var accName = accChartService.GetAll().Where(p => p.IsActive == true && p.AccLevel == accLevel).ToList();
                var viewAccName = accName.AsEnumerable().Select(p => new SelectListItem
                {
                    Text = string.Format("{0}-{1}", p.AccCode, p.AccName),
                    Value = p.AccID.ToString()
                }).ToList();
                var accNameList = new List<SelectListItem>();
                accNameList.Add(new SelectListItem { Text = "Please Select", Value = "" });
                accNameList.AddRange(viewAccName);
                return Json(accNameList, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAccNameForLedgerBook(int accLevel)
        {
            var accName = accChartService.GetAll().Where(p => p.IsActive == true && p.AccLevel == accLevel).ToList();
            var viewAccName = accName.AsEnumerable().Select(p => new SelectListItem
            {
                Text = string.Format("{0}-{1}", p.AccCode, p.AccName),
                Value = p.AccCode.ToString()
            }).ToList();
            var accNameList = new List<SelectListItem>();
            accNameList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            accNameList.AddRange(viewAccName);
            return Json(accNameList, JsonRequestBehavior.AllowGet);
        }
        public void MapDropdownForAccLevel()
        {
            var accLevel = accChartService.GetAll().Where(p => p.IsActive == true);
            var grouped = accLevel.GroupBy(item => item.AccLevel);
            var accountLevelList = grouped.Select(grp => grp.OrderBy(item => item.AccLevel).First());
            var viewAccLevel = accountLevelList.AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.AccLevel.ToString(),
                Value = p.AccLevel.ToString(),
                Selected = p.AccLevel == 3 ? true : false
            }).ToList();
            var accLevelList = new List<SelectListItem>();
            accLevelList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            accLevelList.AddRange(viewAccLevel);
            ViewData["AccLevelList"] = accLevelList;

            var accNameList = new List<SelectListItem>();
            accNameList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            ViewData["AccNameList"] = accNameList;
        }
        public void MapDropdownForAccName_GeneralAccount()
        {
            var accNameList = new List<SelectListItem>();
            accNameList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            ViewData["AccNameList"] = accNameList;
        }
        public ActionResult GeneralAccountStatement() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForAccName_GeneralAccount();
            return View();
        }
        public ActionResult GeneralAccountStatementReport(string office_id, string from_date, string to_date, int acc_Id)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "to_date", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "office_id", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = "3" });
                paramValues.Add(new ParameterValue() { Name = "AccId", Value = acc_Id.ToString() });

                PrintSSRSReport("/gBanker_Reports/Gen_Acc_Statement", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult LedgerBook() //View
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForAccLevel();
            return View();
        }
        public ActionResult GenerateLedgerBookReport(string office_id, string from_date, string to_date, int acc_level, int acc_Id)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "to_date", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "office_id", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = acc_level.ToString() });
                paramValues.Add(new ParameterValue() { Name = "AccId", Value = acc_Id.ToString() });

                PrintSSRSReport("/gBanker_Reports/AIS_Ledger_Book", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult LoanDisbursementRealizationOutstandingStatement()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }
        public ActionResult GenerateLoanDisbursementStatementReport(string office_id, string from_date, string to_date)
        {
            try
            {
                int qtype = 1;
                var orgId = SessionHelper.LoginUserOrganizationID;
                int productId = 0;
                var param = new { QType = qtype, OrgID = orgId, OfficeID = office_id, ProductID = productId, DateFrom = from_date, DateTo = to_date };

                var POMIS1As = ultimateReportService.GetDataWithParameter(param, "RPT_LoanDis_Realization_Outstanding");
                var subReportParam = new { OrgId = orgId, OfficeId = office_id, DateTo = to_date, QType = qtype };
                var subReport = ultimateReportService.GetDataWithParameter(subReportParam, "RPT_LoanSize_Buro");
                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("CustomerBased", subReport.Tables[0]);


                var reportParam = new Dictionary<string, object>();
                //reportParam.Add("DateFrom", from_date);
                //reportParam.Add("DateTo", to_date);

                ReportHelper.PrintWithSubReport("HO_Loan_Disbursement_Realization_Outstanding.rpt", POMIS1As.Tables[0], new Dictionary<string, object>(), subReportDB, new HO_Loan_Disbursement_Realization_Outstanding());

                ////PrintSSRSReport("/gBanker_Reports/MIS_Loan_Disbursement_Realization_Outstanding", paramValues.ToArray(), "gBankerReport");

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DropdownAccLevelForIncExp()
        {
            var accLevel = accChartService.GetAll().Where(p => p.IsActive == true);
            var grouped = accLevel.GroupBy(item => item.AccLevel);
            var accountLevelList = grouped.Select(grp => grp.OrderBy(item => item.AccLevel).First());
            var viewAccLevel = accountLevelList.AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.AccLevel.ToString(),
                Value = p.AccLevel.ToString(),
                Selected = p.AccLevel == 3 ? true : false
            }).ToList();
            var accLevelList = new List<SelectListItem>();
            accLevelList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            accLevelList.AddRange(viewAccLevel);
            ViewData["AccLevelList"] = accLevelList;
        }
        public ActionResult IncomeExpense()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            DropdownAccLevelForIncExp();
            return View();
        }
        public ActionResult GenerateIncomeExpenseReport(string office_id, string from_date, string to_date, int acc_level)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "to_date", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "office_id", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = acc_level.ToString() });
                paramValues.Add(new ParameterValue() { Name = "And_Condition", Value = "" });
                paramValues.Add(new ParameterValue() { Name = "AllOffice", Value = 0.ToString() });

                PrintSSRSReport("/gBanker_Reports/IncomeExpense", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult BankBook() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["CashBookList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForAccLevel();
            return View();
        }
        public ActionResult GenerateBankBookReport(string office_id, string from_date, string to_date, int acc_level, int acc_Id)
        {
            try
            {
                var paramValues = new List<ParameterValue>();

                paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "to_date", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "office_id", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = acc_level.ToString() });
                paramValues.Add(new ParameterValue() { Name = "AccId", Value = acc_Id.ToString() });

                PrintSSRSReport("/gBanker_Reports/ASI_Bankbook", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult TrialBalance() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }
            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            DropdownAccLevelForIncExp();
            return View();
        }
        public ActionResult GenerateTrialBalanceReport(string office_id, string from_date, string to_date, int acc_level)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "to_date", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "office_id", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = acc_level.ToString() });
                paramValues.Add(new ParameterValue() { Name = "AllOffice", Value = 0.ToString() });

                PrintSSRSReport("/gBanker_Reports/TrialBalance", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        private void MapDropdownForMemberQType()
        {
            var qTypeList = new List<SelectListItem>();
            qTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            qTypeList.Add(new SelectListItem { Text = "All", Value = 7.ToString() });
            ViewData["QTypeList"] = qTypeList;
        }
        public ActionResult MISMemberList() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForMemberQType();
            return View();
        }
        public ActionResult GenerateMISMemberListReport(string office_id, string from_date, string to_date, int qType)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Qtype", Value = qType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Office", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "Center", Value = 0.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Org", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "FromDate", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "ToDate", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "MeberStatus", Value = 0.ToString() });

                PrintSSRSReport("/gBanker_Reports/MIS_MemberList", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult MISAdvanceStatement() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }
        public ActionResult GenerateMISAdvanceStatementReport(string office_id, string transaction_date)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "OrgID", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "TrxDate", Value = transaction_date });

                PrintSSRSReport("/gBanker_Reports/MIS_Advance_Statement", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        ////Sabet//
        //private void MapDropDownReportType()
        //{
        //    var reportTypeList = new List<SelectListItem>();
        //    reportTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
        //    reportTypeList.Add(new SelectListItem { Text = "Detail", Value = "D" });
        //    reportTypeList.Add(new SelectListItem { Text = "Summery", Value = "S" });
        //    ViewData["ReportTypeList"] = reportTypeList;
        //}
        //private void MapDropDownPeriodType()
        //{
        //    var periodTypeList = new List<SelectListItem>();
        //    periodTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
        //    periodTypeList.Add(new SelectListItem { Text = "Monthly", Value = "1" });
        //    periodTypeList.Add(new SelectListItem { Text = "Quarterly", Value = "2" });
        //    periodTypeList.Add(new SelectListItem { Text = "Yearly", Value = "3" });
        //    periodTypeList.Add(new SelectListItem { Text = "Range Periodical", Value = "4" });
        //    ViewData["PeriodTypeList"] = periodTypeList;
        //}
        //private void MapDropDownProductFilter()
        //{
        //    var getFilteredProduct = ultimateReportService.GetDataWithoutParameter("BUROHO_FilterByProduct");
        //    var viewFilteredProduct = getFilteredProduct.Tables[0].AsEnumerable().Select(p => new SelectListItem
        //    {
        //        Text = p.Field<string>("ProductName"),
        //        Value = p.Field<int>("ProductID").ToString()
        //    });
        //    var productList = new List<SelectListItem>();
        //    productList.Add(new SelectListItem { Text = "Please Select", Value = "" });
        //    productList.AddRange(viewFilteredProduct);
        //    ViewData["ProductList"] = productList;
        //}

        //private void GetLoginWiseOfficeType()
        //{
        //    var viewOfficeTypeList = new List<SelectListItem>();
        //    var loginUserId = LoggedInEmployeeID;
        //    var param = new { UserId = loginUserId };
        //    if (loginUserId == null)
        //        RedirectToAction("Login", "Account");
        //    else
        //    {
        //        var officeTypeList = ultimateReportService.GetDataWithParameter(param, "BUROHO_FilterByName");
        //        viewOfficeTypeList = officeTypeList.Tables[0].AsEnumerable().Select(p => new SelectListItem
        //        {
        //            Text = p.Field<string>("OfficeLevelName"),
        //            Value = p.Field<int>("OfficeLevel").ToString()
        //        }).ToList();
        //        var ofcTypeList = new List<SelectListItem>();
        //        ofcTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
        //        ofcTypeList.AddRange(viewOfficeTypeList);
        //        ViewData["FilterByList"] = ofcTypeList;
        //    }
        //}
        //private void GetAccNameForLedgerBook()
        //{
        //    var accName = accChartService.GetAll().Where(p => p.IsActive == true && p.AccLevel == 3).ToList();
        //    var viewAccName = accName.AsEnumerable().Select(p => new SelectListItem
        //    {
        //        Text = string.Format("{0}-{1}", p.AccCode, p.AccName),
        //        Value = p.AccCode.ToString()
        //    }).ToList();
        //    var accNameList = new List<SelectListItem>();
        //    accNameList.Add(new SelectListItem { Text = "Please Select", Value = "" });
        //    accNameList.AddRange(viewAccName);
        //    ViewData["AccNameList"] = accNameList;
        //}
        //public JsonResult GetFilteredOfficeList([DataSourceRequest]DataSourceRequest request, int filterByOffice)
        //{
        //    int Result = 0; string Message = ""; object Data = "";
        //    try
        //    {
        //        List<SSRSReportViewModel> viewOfficeList = new List<SSRSReportViewModel>();
        //        var loginUserId = LoggedInEmployeeID;
        //        var param = new { UserId = loginUserId, OfficeLevel = filterByOffice };
        //        var getOfficeList = ultimateReportService.GetDataWithParameter(param, "BUROHO_FilterByOffice");
        //        viewOfficeList = getOfficeList.Tables[0].AsEnumerable().Select(p => new SSRSReportViewModel()
        //        {
        //            rowSl = p.Field<Int64>("rowSl"),
        //            OfficeLevel = p.Field<int>("OfficeLevel"),
        //            HOCode = p.Field<string>("HOCode"),
        //            HOName = p.Field<string>("HOName"),
        //            OfficeID = p.Field<int>("OfficeID"),
        //            OfficeCode = p.Field<string>("OfficeCode"),
        //            OfficeName = p.Field<string>("OfficeName"),
        //            EmployeeID = p.Field<int>("EmployeeID"),
        //            OfficeLevelName = p.Field<string>("OfficeLevelName")
        //        }).ToList();
        //        DataSourceResult result = viewOfficeList.ToDataSourceResult(request);
        //        return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { Result = Result, Message = e.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //public ActionResult AISBalanceSheet_New()
        //{
        //    MapDropDownReportType();
        //    MapDropDownPeriodType();
        //    GetLoginWiseOfficeType();
        //    ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    return View();
        //}
        //public ActionResult GenerateAISBalanceSheet_New(string reportType, string from_date, string to_date, int AccLevel, string And_Condition, int AllOffice, int PeriodType, string officeid_Multi)
        //{
        //    try
        //    {
        //        var paramValues = new List<ParameterValue>();
        //        paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date });
        //        paramValues.Add(new ParameterValue() { Name = "to_date", Value = to_date });
        //        paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = AccLevel.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "And_Condition", Value = And_Condition });
        //        paramValues.Add(new ParameterValue() { Name = "AllOffice", Value = AllOffice.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });

        //        if (reportType == "D")
        //            PrintSSRSReport("/gBanker_Reports/HO_ AIS_BalanceSheet", paramValues.ToArray(), "gBankerReport");
        //        else
        //            PrintSSRSReport("/gBanker_Reports/HO_AIS_BalanceSheetSummery", paramValues.ToArray(), "gBankerReport");

        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public ActionResult HO_IncomeStatement()
        //{
        //    MapDropDownReportType();
        //    MapDropDownPeriodType();
        //    GetLoginWiseOfficeType();
        //    ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    return View();
        //}
        //public ActionResult GenerateIncomeStatement(string reportType, string from_date, string to_date, int AccLevel, string And_Condition, int AllOffice, int PeriodType, string officeid_Multi)
        //{
        //    try
        //    {
        //        var paramValues = new List<ParameterValue>();
        //        paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date });
        //        paramValues.Add(new ParameterValue() { Name = "to_date", Value = to_date });
        //        paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = AccLevel.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "And_Condition", Value = And_Condition });
        //        paramValues.Add(new ParameterValue() { Name = "AllOffice", Value = AllOffice.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });

        //        if (reportType == "D")
        //            PrintSSRSReport("/gBanker_Reports/HO_IncomeStatementDetail", paramValues.ToArray(), "gBankerReport");
        //        else
        //            PrintSSRSReport("/gBanker_Reports/HO_IncomeStatementSummery", paramValues.ToArray(), "gBankerReport");

        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public ActionResult HO_ReceiptPayment()
        //{
        //    MapDropDownReportType();
        //    MapDropDownPeriodType();
        //    GetLoginWiseOfficeType();
        //    ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    return View();
        //}
        //public ActionResult GenerateReceiptPayment(string reportType, string from_date, string to_date, int AccLevel, string And_Condition, int AllOffice, int PeriodType, string officeid_Multi)
        //{
        //    try
        //    {
        //        var paramValues = new List<ParameterValue>();
        //        paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date });
        //        paramValues.Add(new ParameterValue() { Name = "to_date", Value = to_date });
        //        paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = AccLevel.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "And_Condition", Value = And_Condition });
        //        paramValues.Add(new ParameterValue() { Name = "AllOffice", Value = AllOffice.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });

        //        if (reportType == "D")
        //            PrintSSRSReport("/gBanker_Reports/HO_Receipt_PaymentsDetail", paramValues.ToArray(), "gBankerReport");

        //        if (reportType == "S")
        //        {
        //            var param = new { from_date = from_date, to_date = to_date, AccLevel = AccLevel, And_Condition = "", AllOffice = AllOffice, PeriodType = PeriodType, officeid_Multi = officeid_Multi };

        //            var data = ultimateReportService.GetDataWithParameter(param, "BUROHO_Proc_Rpt_Acc_ReceivePayment_Summary");
        //            var reportParam = new Dictionary<string, object>();
        //            reportParam.Add("From_Date", from_date);
        //            reportParam.Add("To_Date", to_date);
        //            ReportHelper.PrintReport("HO_Receipt_PaymentSummery.rpt", data.Tables[0], reportParam);
        //        }
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public ActionResult HO_LedgerBook()
        //{
        //    GetAccNameForLedgerBook();
        //    MapDropDownPeriodType();
        //    GetLoginWiseOfficeType();
        //    ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    return View();
        //}
        //public ActionResult GenerateHO_LedgerBook(string accCode, string from_date, string to_date, int AccLevel, int PeriodType, string officeid_Multi)
        //{
        //    try
        //    {
        //        var paramValues = new List<ParameterValue>();
        //        paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date });
        //        paramValues.Add(new ParameterValue() { Name = "to_date", Value = to_date });
        //        paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = AccLevel.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "AccCode", Value = accCode });
        //        paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });
        //        PrintSSRSReport("/gBanker_Reports/HO_MIS_Ledger_Book", paramValues.ToArray(), "gBankerReport");
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public ActionResult HO_AccountsInfo_1()
        //{
        //    MapDropDownReportType();
        //    MapDropDownPeriodType();
        //    GetLoginWiseOfficeType();
        //    ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    return View();
        //}
        //public ActionResult GenerateAccountsInfo1(string reportType, string from_date, string to_date, int PeriodType, string officeid_Multi)
        //{
        //    try
        //    {
        //        var paramValues = new List<ParameterValue>();
        //        paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = from_date });
        //        paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });
        //        paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });
        //        PrintSSRSReport("/gBanker_Reports/HO_AccountsInfo_1", paramValues.ToArray(), "gBankerReport");
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public ActionResult HO_AccountsInfo_2()
        //{
        //    MapDropDownReportType();
        //    MapDropDownPeriodType();
        //    GetLoginWiseOfficeType();
        //    ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    return View();
        //}
        //public ActionResult GenerateAccountsInfo2(string reportType, string from_date, string to_date, int PeriodType, string officeid_Multi)
        //{
        //    try
        //    {
        //        var paramValues = new List<ParameterValue>();
        //        paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = from_date });
        //        paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });
        //        paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });
        //        PrintSSRSReport("/gBanker_Reports/HO_AccountsInfo_2", paramValues.ToArray(), "gBankerReport");
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public ActionResult HO_MonthlyStatistics()
        //{
        //    MapDropDownReportType();
        //    MapDropDownPeriodType();
        //    GetLoginWiseOfficeType();
        //    ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    return View();
        //}
        //public ActionResult GenerateMonthlyStatistics(string reportType, string from_date, string to_date, int PeriodType, string officeid_Multi)
        //{
        //    try
        //    {
        //        var paramValues = new List<ParameterValue>();
        //        paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date });
        //        paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });
        //        paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });
        //        PrintSSRSReport("/gBanker_Reports/HO_Monthly_Statistics_Report", paramValues.ToArray(), "gBankerReport");
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public ActionResult SavingPortfolioStatement()
        //{
        //    MapDropDownReportType();
        //    MapDropDownPeriodType();
        //    GetLoginWiseOfficeType();
        //    ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    return View();
        //}

        //public ActionResult GenerateSavingPortfolioStatement(string reportType, string from_date, string to_date, int PeriodType, string officeid_Multi)
        //{
        //    try
        //    {
        //        var reportParam = new Dictionary<string, object>();
        //        var paramMainReport = new { DateFrom = from_date, DateTo = to_date, PeriodType = PeriodType, officeid_Multi = officeid_Multi };
        //        var mainReportData = ultimateReportService.GetDataWithParameter(paramMainReport, "BUROHO_Proc_RPT_SavingsPortfolioStatement");
        //        var paramSubReport = new { Date_From = from_date, Date_To = to_date, PeriodType = PeriodType, officeid_Multi = officeid_Multi };
        //        var subReportData = ultimateReportService.GetDataWithParameter(paramSubReport, "BUROHO_RPT_SavingsSize");
        //        var subReportDB = new Dictionary<string, DataTable>();
        //        subReportDB.Add("DistributionOfSavingsSize", subReportData.Tables[0]);
        //        ReportHelper.PrintWithSubReport("HO_rptBranchWiseSavingsPortfolioStatement.rpt", mainReportData.Tables[0], new Dictionary<string, object>(), subReportDB, new HO_rptBranchWiseSavingsPortfolioStatement());
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        //public ActionResult LoanDisbursementRealizationOutstanding()
        //{
        //    MapDropDownProductFilter();
        //    MapDropDownReportType();
        //    MapDropDownPeriodType();
        //    GetLoginWiseOfficeType();
        //    ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    return View();
        //}
        //public ActionResult GenerateDisbursementRealizationOutstanding(string reportType, string from_date, string to_date, int PeriodType, string officeid_Multi, int product)
        //{
        //    try
        //    {
        //        var paramValues = new List<ParameterValue>();
        //        paramValues.Add(new ParameterValue() { Name = "ProductID", Value = product.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = from_date });
        //        paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });
        //        paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });
        //        if (reportType == "D")
        //            PrintSSRSReport("/gBanker_Reports/HO_LoanDisbursementRealizationAndOutStanding", paramValues.ToArray(), "gBankerReport");
        //        else
        //            PrintSSRSReport("/gBanker_Reports/HO_LoanDisbursementRealizationAndOutStandingSummery", paramValues.ToArray(), "gBankerReport");
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public ActionResult SectorWiseLoanAnalysis()
        //{
        //    MapDropDownReportType();
        //    MapDropDownPeriodType();
        //    GetLoginWiseOfficeType();
        //    ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    return View();
        //}
        //public ActionResult GenerateSectorWiseLoanAnalysis(string reportType, string from_date, string to_date, int PeriodType, string officeid_Multi)
        //{
        //    try
        //    {
        //        var paramValues = new List<ParameterValue>();
        //        paramValues.Add(new ParameterValue() { Name = "Date_From", Value = from_date });
        //        paramValues.Add(new ParameterValue() { Name = "Date_To", Value = to_date });
        //        paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });
        //        PrintSSRSReport("/gBanker_Reports/HO_SectoralBreakDownOfLoan", paramValues.ToArray(), "gBankerReport");
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public ActionResult LoanAging()
        //{
        //    MapDropDownReportType();
        //    MapDropDownPeriodType();
        //    GetLoginWiseOfficeType();
        //    ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    return View();
        //}
        //public ActionResult GenerateLoanAging(string reportType, string from_date, string to_date, int PeriodType, string officeid_Multi)
        //{
        //    try
        //    {
        //        var paramValues = new List<ParameterValue>();
        //        paramValues.Add(new ParameterValue() { Name = "Date_From", Value = from_date });
        //        paramValues.Add(new ParameterValue() { Name = "Date_To", Value = to_date });
        //        paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });
        //        PrintSSRSReport("/gBanker_Reports/HO_LoanAgingHO", paramValues.ToArray(), "gBankerReport");
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public ActionResult LoanAgingComparativeReport()
        //{
        //    MapDropDownReportType();
        //    MapDropDownPeriodType();
        //    GetLoginWiseOfficeType();
        //    ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    return View();
        //}
        //public ActionResult GenerateLoanAgingComparativeReport(string reportType, string from_date, string to_date, int PeriodType, string officeid_Multi)
        //{
        //    try
        //    {
        //        var paramValues = new List<ParameterValue>();
        //        paramValues.Add(new ParameterValue() { Name = "Date_From", Value = from_date });
        //        paramValues.Add(new ParameterValue() { Name = "Date_To", Value = to_date });
        //        paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });
        //        PrintSSRSReport("/gBanker_Reports/HO_MIS_Loan_Aging", paramValues.ToArray(), "gBankerReport");
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public ActionResult LoanPortfolioDetail()
        //{
        //    MapDropDownProductFilter();
        //    MapDropDownReportType();
        //    MapDropDownPeriodType();
        //    GetLoginWiseOfficeType();
        //    ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    return View();
        //}
        //public ActionResult GenerateLoanPortfolioDetail(string reportType, string from_date, string to_date, int PeriodType, string officeid_Multi, int product)
        //{
        //    try
        //    {
        //        var paramValues = new List<ParameterValue>();
        //        paramValues.Add(new ParameterValue() { Name = "ProductID", Value = product.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "Date_From", Value = from_date });
        //        paramValues.Add(new ParameterValue() { Name = "Date_To", Value = to_date });
        //        paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });
        //        if (reportType == "D")
        //            PrintSSRSReport("/gBanker_Reports/HO_MIS_LoanPortfolioMarch", paramValues.ToArray(), "gBankerReport");
        //        else
        //            PrintSSRSReport("/gBanker_Reports/HO_LoanPortfolioStatementSummery", paramValues.ToArray(), "gBankerReport");
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public ActionResult LoanTerm()
        //{
        //    MapDropDownProductFilter();
        //    MapDropDownReportType();
        //    MapDropDownPeriodType();
        //    GetLoginWiseOfficeType();
        //    ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
        //    return View();
        //}
        //public ActionResult GenerateLoanTerm(string reportType, string from_date, string to_date, int PeriodType, string officeid_Multi, int product)
        //{
        //    try
        //    {
        //        var paramValues = new List<ParameterValue>();
        //        paramValues.Add(new ParameterValue() { Name = "ProductID", Value = product.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = from_date });
        //        paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });
        //        paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });
        //        PrintSSRSReport("/gBanker_Reports/HO_LoanTerm", paramValues.ToArray(), "gBankerReport");
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}



        //Sabet Starts 05/04/2020
        private void MapDropDownReportType()
        {
            var reportTypeList = new List<SelectListItem>();
            reportTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            reportTypeList.Add(new SelectListItem { Text = "Detail", Value = "D" });
            reportTypeList.Add(new SelectListItem { Text = "Summery", Value = "S" });
            ViewData["ReportTypeList"] = reportTypeList;
        }
        private void MapDropDownPeriodType()
        {
            var periodTypeList = new List<SelectListItem>();
            periodTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            periodTypeList.Add(new SelectListItem { Text = "Monthly", Value = "1" });
            periodTypeList.Add(new SelectListItem { Text = "Quarterly", Value = "2" });
            periodTypeList.Add(new SelectListItem { Text = "Yearly", Value = "3" });
            periodTypeList.Add(new SelectListItem { Text = "Range Periodical", Value = "4" });
            ViewData["PeriodTypeList"] = periodTypeList;
        }
        private void MapDropDownProductFilter()
        {
            var getFilteredProduct = ultimateReportService.GetDataWithoutParameter("BUROHO_FilterByProduct");
            var viewFilteredProduct = getFilteredProduct.Tables[0].AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.Field<string>("ProductName"),
                Value = p.Field<int>("ProductID").ToString()
            });
            var productList = new List<SelectListItem>();
            productList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            productList.AddRange(viewFilteredProduct);
            ViewData["ProductList"] = productList;
        }
        private void GetLoginWiseOfficeType()
        {
            var viewOfficeTypeList = new List<SelectListItem>();
            var loginUserId = LoggedInEmployeeID;
            var param = new { UserId = loginUserId };
            if (loginUserId == null)
                RedirectToAction("Login", "Account");
            else
            {
                var officeTypeList = ultimateReportService.GetDataWithParameter(param, "BUROHO_FilterByName");
                viewOfficeTypeList = officeTypeList.Tables[0].AsEnumerable().Select(p => new SelectListItem
                {
                    Text = p.Field<string>("OfficeLevelName"),
                    Value = p.Field<int>("OfficeLevel").ToString()
                }).ToList();
                var ofcTypeList = new List<SelectListItem>();
                ofcTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
                ofcTypeList.AddRange(viewOfficeTypeList);
                ViewData["FilterByList"] = ofcTypeList;
            }
        }
        private void GetAccNameForLedgerBook()
        {
            var accName = ultimateReportService.GetDataWithoutParameter("GetAccNameForLedgerBook_HO");
            var viewAccName = accName.Tables[0].AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.Field<string>("AccCode") + "-" + p.Field<string>("AccName"),
                Value = p.Field<string>("AccCode")

                //Text = string.Format("{0}-{1}", p.AccCode, p.AccName),
                //Value = p.AccCode.ToString()
            }).ToList();
            var accNameList = new List<SelectListItem>();
            accNameList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            accNameList.AddRange(viewAccName);
            ViewData["AccNameList"] = accNameList;
        }
        public JsonResult GetFilteredOfficeList([DataSourceRequest]DataSourceRequest request, int filterByOffice, string[] SearchOffice)
        {
            int Result = 0; string Message = ""; object Data = "";
            try
            {
                var officeCodes = string.Empty;
                if (SearchOffice!=null && SearchOffice.Any())
                    officeCodes = SearchOffice[0].ToString().Replace(",","_");

                List<SSRSReportViewModel> viewOfficeList = new List<SSRSReportViewModel>();
                var loginUserId = LoggedInEmployeeID;
                var param = new { UserId = loginUserId, OfficeLevel = filterByOffice, SearchOffice = officeCodes };
                var getOfficeList = ultimateReportService.GetDataWithParameter(param, "BUROHO_FilterByOffice_New");
                viewOfficeList = getOfficeList.Tables[0].AsEnumerable().Select(p => new SSRSReportViewModel()
                {
                    rowSl = p.Field<Int64>("rowSl"),
                    OfficeLevel = p.Field<int>("OfficeLevel"),
                    HOCode = p.Field<string>("HOCode"),
                    HOName = p.Field<string>("HOName"),
                    OfficeID = p.Field<int>("OfficeID"),
                    OfficeCode = p.Field<string>("OfficeCode"),
                    OfficeName = p.Field<string>("OfficeName"),
                    EmployeeID = p.Field<int>("EmployeeID"),
                    OfficeLevelName = p.Field<string>("OfficeLevelName")
                }).ToList();
                DataSourceResult result = viewOfficeList.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Result = Result, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult AISBalanceSheet_New()
        {
            MapDropDownReportType();
            MapDropDownPeriodType();
            GetLoginWiseOfficeType();
            ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult GenerateAISBalanceSheet_New(string reportType, string from_date, string to_date, int AccLevel, string And_Condition, int AllOffice, int PeriodType, string officeid_Multi)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(officeid_Multi) && officeid_Multi.ToCharArray(0, 1)[0].ToString() == "_")
                    officeid_Multi = officeid_Multi.Substring(1, officeid_Multi.Length - 1);

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "to_date", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = AccLevel.ToString() });
                paramValues.Add(new ParameterValue() { Name = "And_Condition", Value = And_Condition });
                paramValues.Add(new ParameterValue() { Name = "AllOffice", Value = AllOffice.ToString() });
                paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });

                if (reportType == "D")
                    PrintSSRSReport("/gBanker_Reports/HO_ AIS_BalanceSheet", paramValues.ToArray(), "gBankerReport");
                else
                    PrintSSRSReport("/gBanker_Reports/HO_AIS_BalanceSheetSummery", paramValues.ToArray(), "gBankerReport");

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult HO_IncomeStatement()
        {
            MapDropDownReportType();
            MapDropDownPeriodType();
            GetLoginWiseOfficeType();
            ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult GenerateIncomeStatement(string reportType, string from_date, string to_date, int AccLevel, string And_Condition, int AllOffice, int PeriodType, string officeid_Multi)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(officeid_Multi) && officeid_Multi.ToCharArray(0, 1)[0].ToString() == "_")
                    officeid_Multi = officeid_Multi.Substring(1, officeid_Multi.Length - 1);

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "to_date", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = AccLevel.ToString() });
                paramValues.Add(new ParameterValue() { Name = "And_Condition", Value = And_Condition });
                paramValues.Add(new ParameterValue() { Name = "AllOffice", Value = AllOffice.ToString() });
                paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });

                if (reportType == "D")
                    PrintSSRSReport("/gBanker_Reports/HO_IncomeStatementDetail", paramValues.ToArray(), "gBankerReport");
                else
                    PrintSSRSReport("/gBanker_Reports/HO_IncomeStatementSummery", paramValues.ToArray(), "gBankerReport");

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult HO_ReceiptPayment()
        {
            MapDropDownReportType();
            MapDropDownPeriodType();
            GetLoginWiseOfficeType();
            ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult GenerateReceiptPayment(string reportType, string from_date, string to_date, int AccLevel, string And_Condition, int AllOffice, int PeriodType, string officeid_Multi)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(officeid_Multi) && officeid_Multi.ToCharArray(0, 1)[0].ToString() == "_")
                    officeid_Multi = officeid_Multi.Substring(1, officeid_Multi.Length - 1);

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "to_date", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = AccLevel.ToString() });
                paramValues.Add(new ParameterValue() { Name = "And_Condition", Value = And_Condition });
                paramValues.Add(new ParameterValue() { Name = "AllOffice", Value = AllOffice.ToString() });
                paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });

                if (reportType == "D")
                    PrintSSRSReport("/gBanker_Reports/HO_Receipt_PaymentsDetail", paramValues.ToArray(), "gBankerReport");

                if (reportType == "S")
                {
                    var param = new { from_date = from_date, to_date = to_date, AccLevel = AccLevel, And_Condition = "", AllOffice = AllOffice, PeriodType = PeriodType, officeid_Multi = officeid_Multi };

                    var data = ultimateReportService.GetDataWithParameter(param, "BUROHO_Proc_Rpt_Acc_ReceivePayment_Summary");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("From_Date", from_date);
                    reportParam.Add("To_Date", to_date);
                    ReportHelper.PrintReport("HO_Receipt_PaymentSummery.rpt", data.Tables[0], reportParam);
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult HO_LedgerBook()
        {
            GetAccNameForLedgerBook();
            MapDropDownPeriodType();
            GetLoginWiseOfficeType();
            ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult GenerateHO_LedgerBook(string accCode, string from_date, string to_date, int AccLevel, int PeriodType, string officeid_Multi)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(officeid_Multi) && officeid_Multi.ToCharArray(0, 1)[0].ToString() == "_")
                    officeid_Multi = officeid_Multi.Substring(1, officeid_Multi.Length - 1);

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "to_date", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = AccLevel.ToString() });
                paramValues.Add(new ParameterValue() { Name = "AccCode", Value = accCode });
                paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });
                PrintSSRSReport("/gBanker_Reports/HO_MIS_Ledger_Book", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult HO_AccountsInfo_1()
        {
            MapDropDownReportType();
            MapDropDownPeriodType();
            GetLoginWiseOfficeType();
            ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult GenerateAccountsInfo1(string reportType, string from_date, string to_date, int PeriodType, string officeid_Multi)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(officeid_Multi) && officeid_Multi.ToCharArray(0, 1)[0].ToString() == "_")
                    officeid_Multi = officeid_Multi.Substring(1, officeid_Multi.Length - 1);

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });
                PrintSSRSReport("/gBanker_Reports/HO_AccountsInfo_1", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult HO_AccountsInfo_2()
        {
            MapDropDownReportType();
            MapDropDownPeriodType();
            GetLoginWiseOfficeType();
            ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult GenerateAccountsInfo2(string reportType, string from_date, string to_date, int PeriodType, string officeid_Multi)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(officeid_Multi) && officeid_Multi.ToCharArray(0, 1)[0].ToString() == "_")
                    officeid_Multi = officeid_Multi.Substring(1, officeid_Multi.Length - 1);

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });
                PrintSSRSReport("/gBanker_Reports/HO_AccountsInfo_2", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult HO_MonthlyStatistics()
        {
            MapDropDownReportType();
            MapDropDownPeriodType();
            GetLoginWiseOfficeType();
            ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult GenerateMonthlyStatistics(string reportType, string from_date, string to_date, int PeriodType, string officeid_Multi)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(officeid_Multi) && officeid_Multi.ToCharArray(0, 1)[0].ToString() == "_")
                    officeid_Multi = officeid_Multi.Substring(1, officeid_Multi.Length - 1);

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });
                PrintSSRSReport("/gBanker_Reports/HO_Monthly_Statistics_Report", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult SavingPortfolioStatement()
        {
            MapDropDownReportType();
            MapDropDownPeriodType();
            GetLoginWiseOfficeType();
            ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
            return View();
        }

        public ActionResult GenerateSavingPortfolioStatement(string reportType, string from_date, string to_date, int PeriodType, string officeid_Multi)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(officeid_Multi) && officeid_Multi.ToCharArray(0, 1)[0].ToString() == "_")
                    officeid_Multi = officeid_Multi.Substring(1, officeid_Multi.Length - 1);

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });
                paramValues.Add(new ParameterValue() { Name = "Date_From", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "Date_To", Value = to_date });
                PrintSSRSReport("/gBanker_Reports/HO_SavingsPortfolioStatement", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult LoanDisbursementRealizationOutstanding()
        {
            MapDropDownProductFilter();
            MapDropDownReportType();
            MapDropDownPeriodType();
            GetLoginWiseOfficeType();
            ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult GenerateDisbursementRealizationOutstanding(string reportType, string from_date, string to_date, int PeriodType, string officeid_Multi, int product)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(officeid_Multi) && officeid_Multi.ToCharArray(0, 1)[0].ToString() == "_")
                    officeid_Multi = officeid_Multi.Substring(1, officeid_Multi.Length - 1);

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "ProductID", Value = product.ToString() });
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });
                if (reportType == "D")
                    PrintSSRSReport("/gBanker_Reports/HO_LoanDisbursementRealizationAndOutStanding", paramValues.ToArray(), "gBankerReport");
                else
                    PrintSSRSReport("/gBanker_Reports/HO_LoanDisbursementRealizationAndOutStandingSummery", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult SectorWiseLoanAnalysis()
        {
            MapDropDownReportType();
            MapDropDownPeriodType();
            GetLoginWiseOfficeType();
            ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult GenerateSectorWiseLoanAnalysis(string reportType, string from_date, string to_date, int PeriodType, string officeid_Multi)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(officeid_Multi) && officeid_Multi.ToCharArray(0, 1)[0].ToString() == "_")
                    officeid_Multi = officeid_Multi.Substring(1, officeid_Multi.Length - 1);

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Date_From", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "Date_To", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });
                PrintSSRSReport("/gBanker_Reports/HO_SectoralBreakDownOfLoan", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult LoanAging()
        {
            MapDropDownReportType();
            MapDropDownPeriodType();
            GetLoginWiseOfficeType();
            ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult GenerateLoanAging(string reportType, string from_date, string to_date, int PeriodType, string officeid_Multi)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(officeid_Multi) && officeid_Multi.ToCharArray(0, 1)[0].ToString() == "_")
                    officeid_Multi = officeid_Multi.Substring(1, officeid_Multi.Length - 1);

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Date_From", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "Date_To", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });
                if (reportType == "D")
                    PrintSSRSReport("/gBanker_Reports/HO_LoanAgingHO", paramValues.ToArray(), "gBankerDbContextForFixedAssetReport");
                if (reportType == "S")
                    PrintSSRSReport("/gBanker_Reports/HO_MIS_Loan_Aging", paramValues.ToArray(), "gBankerDbContextForFixedAssetReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult LoanAgingComparativeReport()
        {
            MapDropDownReportType();
            MapDropDownPeriodType();
            GetLoginWiseOfficeType();
            ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult GenerateLoanAgingComparativeReport(string reportType, string from_date, string to_date, int PeriodType, string officeid_Multi)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(officeid_Multi) && officeid_Multi.ToCharArray(0, 1)[0].ToString() == "_")
                    officeid_Multi = officeid_Multi.Substring(1, officeid_Multi.Length - 1);

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Date_From", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "Date_To", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });
                PrintSSRSReport("/gBanker_Reports/HO_MIS_Loan_Aging", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult LoanPortfolioDetail()
        {
            MapDropDownProductFilter();
            MapDropDownReportType();
            MapDropDownPeriodType();
            GetLoginWiseOfficeType();
            ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult GenerateLoanPortfolioDetail(string reportType, string from_date, string to_date, int PeriodType, string officeid_Multi, int product)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(officeid_Multi) && officeid_Multi.ToCharArray(0, 1)[0].ToString() == "_")
                    officeid_Multi = officeid_Multi.Substring(1, officeid_Multi.Length - 1);

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "ProductID", Value = product.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Date_From", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "Date_To", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });
                if (reportType == "D")
                    PrintSSRSReport("/gBanker_Reports/HO_MIS_LoanPortfolioMarch", paramValues.ToArray(), "gBankerReport");
                else
                    PrintSSRSReport("/gBanker_Reports/HO_LoanPortfolioStatementSummery", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult LoanTerm()
        {
            MapDropDownProductFilter();
            MapDropDownReportType();
            MapDropDownPeriodType();
            GetLoginWiseOfficeType();
            ViewData["DateFrom"] = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewData["DateTo"] = DateTime.Now.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult GenerateLoanTerm(string reportType, string from_date, string to_date, int PeriodType, string officeid_Multi, int product)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(officeid_Multi) && officeid_Multi.ToCharArray(0, 1)[0].ToString() == "_")
                    officeid_Multi = officeid_Multi.Substring(1, officeid_Multi.Length - 1);

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "ProductID", Value = product.ToString() });
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "PeriodType", Value = PeriodType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "officeid_Multi", Value = officeid_Multi });
                PrintSSRSReport("/gBanker_Reports/HO_LoanTerm", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        ////EndSabet
        public ActionResult DailyDepositRegister() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }
        public ActionResult GenerateDailyDepositRegisterReport(string office_id, string transaction_date)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "OrgID", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "Date", Value = transaction_date });
                
                    
                PrintSSRSReport("/gBanker_Reports/DailyDepositRegisterNew", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult ReceiptsAndPayments() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            DropdownAccLevelForIncExp();
            return View();
        }
        public ActionResult GenerateReceiptsAndPaymentsReport(string office_id, string from_date, string to_date, int acc_level)
        {
            try
            {
                //var paramValues = new List<ParameterValue>();
                //paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date });
                //paramValues.Add(new ParameterValue() { Name = "to_date", Value = to_date });
                //paramValues.Add(new ParameterValue() { Name = "office_id", Value = office_id });
                //paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = acc_level.ToString() });
                //paramValues.Add(new ParameterValue() { Name = "And_Condition", Value = "" });
                //paramValues.Add(new ParameterValue() { Name = "AllOffice", Value = 0.ToString() });

                var param = new { from_date = from_date, to_date = to_date, office_id = Convert.ToInt32(office_id), AccLevel = acc_level, And_Condition = "", AllOffice = 0 };

                var data = ultimateReportService.GetDataWithParameter(param, "Proc_Rpt_Acc_ReceivePayment");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("From_Date", from_date);
                reportParam.Add("To_Date", to_date);
                ReportHelper.PrintReport("AIS_ReceiptsAndPayments.rpt", data.Tables[0], reportParam);
                return Content(string.Empty);
                //PrintSSRSReport("/gBanker_Reports/ReceiptsAndPayments", paramValues.ToArray(), "gBankerDbContext");
                //return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult LoanAgingStatement() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForLoanAging();
            return View();
        }
        private void MapDropdownForLoanAging()
        {
            var qTypeList = new List<SelectListItem>();
            qTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            qTypeList.Add(new SelectListItem { Text = "Product Wise", Value = 9.ToString() });
            ViewData["QTypeList"] = qTypeList;
        }
        private void MapDropdownForLoanAgingAndCenterList()
        {            
            var qTypeList = new List<SelectListItem>();
            qTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            qTypeList.Add(new SelectListItem { Text = "All", Value = "0" });
            qTypeList.Add(new SelectListItem { Text = "Center Wise", Value = "2" });
            qTypeList.Add(new SelectListItem { Text = "Product Wise", Value = "3" });
            qTypeList.Add(new SelectListItem { Text = "Center & Product Wise", Value = "4" });
            ViewData["QTypeList"] = qTypeList;

            var officeId = SessionHelper.LoginUserOfficeID;
            var centerInfo = centerService.GetAll().Where(p => p.IsActive == true && p.OfficeID == officeId).OrderBy(p => p.CenterCode).ToList();
            var viewCenterInfo = centerInfo.AsEnumerable().Select(p => new SelectListItem
            {
                Text = string.Format("{0}-{1}", p.CenterCode, p.CenterName),
                Value = p.CenterID.ToString()
            }).ToList();
            var centerList = new List<SelectListItem>();
            centerList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            centerList.AddRange(viewCenterInfo);
            ViewData["CenterList"] = centerList;
        }
        public ActionResult GenerateLoanAgingStatementReport(string office_id, string from_date, string to_date, int qType)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var param = new
                {
                    OrgID = orgId,
                    OfficeID = office_id,
                    OfficeIDTO = office_id,
                    CenterID = 00,
                    CenterIDTo = 99999999,
                    StaffID = 00,
                    StaffIDTo = 999999,
                    productID = 00,
                    ProductIDTo = 99999999,
                    DateFrom = from_date,
                    DateTo = to_date,
                    Qtype = 1
                };
                var dataTableForQtype1 = ultimateReportService.GetDataWithParameter(param, "OverdueClassificationConsolidation");

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "OrgID", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "OfficeIDTO", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "CenterID", Value = 00.ToString() });
                paramValues.Add(new ParameterValue() { Name = "CenterIDTo", Value = 99999999.ToString() });
                paramValues.Add(new ParameterValue() { Name = "StaffID", Value = 00.ToString() });
                paramValues.Add(new ParameterValue() { Name = "StaffIDTo", Value = 999999.ToString() });
                paramValues.Add(new ParameterValue() { Name = "productID", Value = 00.ToString() });
                paramValues.Add(new ParameterValue() { Name = "ProductIDTo", Value = 99999999.ToString() });
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "Qtype", Value = "9" });

                PrintSSRSReport("/gBanker_Reports/Loan_Aging", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Buro_Receipt_Payments()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            DropdownAccLevelForIncExp();
            return View();
        }
        public ActionResult GenerateBuro_Receipt_PaymentsReport(string office_id, string from_date, string to_date, int acc_level)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "to_date", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "office_id", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = acc_level.ToString() });
                paramValues.Add(new ParameterValue() { Name = "And_Condition", Value = "" });
                paramValues.Add(new ParameterValue() { Name = "AllOffice", Value = 0.ToString() });

                PrintSSRSReport("/gBanker_Reports/Buro_Receipt_Payments", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void DropdownForContractualSavingsQType()
        {
            var qTypeList = new List<SelectListItem>();
            qTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            qTypeList.Add(new SelectListItem { Text = "All", Value = "1" });
            qTypeList.Add(new SelectListItem { Text = "Center Wise", Value = "2" });
            qTypeList.Add(new SelectListItem { Text = "Product Wise", Value = "3" });
            qTypeList.Add(new SelectListItem { Text = "Center & Product Wise", Value = "4" });
            ViewData["QTypeList"] = qTypeList;

            var officeId = SessionHelper.LoginUserOfficeID;
            var centerInfo = centerService.GetAll().Where(p => p.IsActive == true && p.OfficeID == officeId).OrderBy(p => p.CenterCode).ToList();
            var viewCenterInfo = centerInfo.AsEnumerable().Select(p => new SelectListItem
            {
                Text = string.Format("{0}-{1}", p.CenterCode, p.CenterName),
                Value = p.CenterID.ToString()
            }).ToList();
            var centerList = new List<SelectListItem>();
            centerList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            centerList.AddRange(viewCenterInfo);
            ViewData["CenterList"] = centerList;

        }
        public ActionResult ContractualSavingsCollectionStatement()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            DropdownForContractualSavingsQType();
            return View();
        }
        public ActionResult GenerateContractualSavingsCollectionReport(string office_id, string date, int qType, int CenterId)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Qtype", Value = qType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Org", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeId", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "EmpId", Value = "0" });
                paramValues.Add(new ParameterValue() { Name = "CenterId", Value = CenterId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "ProductId", Value = "0" });
                paramValues.Add(new ParameterValue() { Name = "Date", Value = date });
                PrintSSRSReport("/gBanker_Reports/Buro_Contractual_Savings_Collection_Statement", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void MapDropDownListForVoucher(AccVoucherEntryViewModel model)
        {
            var type_item = new List<SelectListItem>();
            type_item.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            type_item.Add(new SelectListItem() { Text = "Cash (Debit)", Value = "CAD" });
            type_item.Add(new SelectListItem() { Text = "Cash (Credit)", Value = "CAC" });
            type_item.Add(new SelectListItem() { Text = "Bank(Debit)", Value = "BDr" });
            type_item.Add(new SelectListItem() { Text = "Bank(Credit)", Value = "BCr" });
            type_item.Add(new SelectListItem() { Text = "Journal", Value = "Jr" });
            type_item.Add(new SelectListItem() { Text = "Cash Contra", Value = "BC" });
            model.VoucherTypeList = type_item;

            var type_item2 = new List<SelectListItem>();
            type_item2.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            model.VoucherNoList = type_item2;
        }
        public ActionResult Voucher()
        {
            var model = new AccVoucherEntryViewModel();
            MapDropDownListForVoucher(model);

            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);

            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                if (LoggedInOrganizationID != 54)
                {
                    ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
                    ViewData["TrxdateTo"] = TransactionDate.ToString("dd-MMM-yyyy");
                    var param1 = new { OfficeID = LoginUserOfficeID, BusinessDate = TransactionDate, OrgID = SessionHelper.LoginUserOrganizationID };
                    var LoanInstallMent = weeklyReportService.CheckAutoVoucher(param1);

                    if (LoanInstallMent.Tables[0].Rows.Count == 0)
                    {
                        var param = new { OfficeID = LoginUserOfficeID, BusinessDate = TransactionDate, OrgID = SessionHelper.LoginUserOrganizationID };
                        weeklyReportService.AutoVoucherCollectionProcess(param);
                    }
                }

            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
                ViewData["TrxdateTo"] = VDate.ToString("dd-MMM-yyyy");

            }

            return View(model);
        }
        public ActionResult GenerateAllVoucherReport(string officeId, string voucher_type, string from_date, string to_date)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "voucher_type", Value = voucher_type });
                paramValues.Add(new ParameterValue() { Name = "trx_date", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "office_id", Value = officeId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "trx_dateTo", Value = to_date });
                if (voucher_type == "BC" || voucher_type == "Jr")
                {
                    PrintSSRSReport("/gBanker_Reports/Cash_Contra_Voucher", paramValues.ToArray(), "gBankerReport");
                }
                else
                {
                    PrintSSRSReport("/gBanker_Reports/Credit_Voucher", paramValues.ToArray(), "gBankerReport");
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GenerateVoucherNoWiseReport(string officeId, string voucher_type, string from_date, string to_date, int voucherNo)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "voucher_type", Value = voucher_type });
                paramValues.Add(new ParameterValue() { Name = "trx_date", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "office_id", Value = officeId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "trx_dateTo", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "voucher_id", Value = voucherNo.ToString() });
                if (voucher_type == "BC" || voucher_type == "Jr")
                {
                    PrintSSRSReport("/gBanker_Reports/VoucherNoWise_Journal_CashContra_Voucher", paramValues.ToArray(), "gBankerReport");
                }
                else
                {
                    PrintSSRSReport("/gBanker_Reports/VoucherNoWise", paramValues.ToArray(), "gBankerReport");
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult LoanFineRealizable()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }
        public ActionResult GenerateLoanFineRealizableReport(string office_id, string from_date, string to_date)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "QType", Value = 1.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OrgID", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "StartDate", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "EndDate", Value = to_date });

                PrintSSRSReport("/gBanker_Reports/Buro_Loan_Fine_Realizable", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void MapDropdownForAccountTransactionDetails()
        {
            var qTypeList = new List<SelectListItem>();
            qTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            qTypeList.Add(new SelectListItem { Text = "All", Value = "1" });
            qTypeList.Add(new SelectListItem { Text = "Product Wise", Value = "2" });
            qTypeList.Add(new SelectListItem { Text = "Product & No. of Account Wise", Value = "3" });
            ViewData["QTypeList"] = qTypeList;

            var centerInfo = centerService.GetAll().Where(p => p.IsActive == true).ToList();
            var viewCenterInfo = centerInfo.AsEnumerable().Select(p => new SelectListItem
            {
                Text = string.Format("{0}-{1}", p.CenterCode, p.CenterName),
                Value = p.CenterID.ToString()
            }).ToList();
            var centerList = new List<SelectListItem>();
            centerList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            centerList.AddRange(viewCenterInfo);
            ViewData["CenterList"] = centerList;

            //var memberInfo = memberService.GetAll().Where(p => p.IsActive == true).ToList();
            //var viewMemberInfo = memberInfo.AsEnumerable().Select(p => new SelectListItem
            //{
            //    Text = string.Format("{0}-{1}", p.MemberCode, p.FirstName),
            //    Value = p.MemberID.ToString()
            //}).ToList();
            //var memberList = new List<SelectListItem>();
            //memberList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            //memberList.AddRange(viewMemberInfo);
            //ViewData["MemberList"] = memberList;
        }
        public ActionResult AccountTransactionDetailsSavings()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            ViewData["ProductListByMember"] = items;
            ViewData["NoOfAccountList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
           // MapDropdownForAccountTransactionDetails();
            return View();
        }
        public ActionResult GenerateAccountTransactionDetailsSavingsReport(string Qtype, string office_id, string Center, string Member, string ProductID, string NoOfAccount, string DateFrom, string DateTo)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Qtype", Value = Qtype });
                paramValues.Add(new ParameterValue() { Name = "Org", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Office", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "Center", Value = Center });
                paramValues.Add(new ParameterValue() { Name = "Member", Value = Member });
                paramValues.Add(new ParameterValue() { Name = "ProductID", Value = ProductID });
                paramValues.Add(new ParameterValue() { Name = "NoOfAccount", Value = NoOfAccount });
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = DateFrom });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = DateTo });

                PrintSSRSReport("/gBanker_Reports/Account_Transaction_Details", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void MapDDLForSavingsRefundRegister()
        {
            var qTypeList = new List<SelectListItem>();
            qTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            qTypeList.Add(new SelectListItem { Text = "All", Value = "1" });
            qTypeList.Add(new SelectListItem { Text = "Center Wise", Value = "2" });
            ViewData["QTypeList"] = qTypeList;

            var centerInfo = centerService.GetAll().Where(p => p.IsActive == true).ToList();
            var viewCenterInfo = centerInfo.AsEnumerable().Select(p => new SelectListItem
            {
                Text = string.Format("{0}-{1}", p.CenterCode, p.CenterName),
                Value = p.CenterID.ToString()
            }).ToList();
            var centerList = new List<SelectListItem>();
            centerList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            centerList.AddRange(viewCenterInfo);
            ViewData["CenterList"] = centerList;
        }
        public ActionResult SavingsRefundRegister()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDDLForSavingsRefundRegister();
            return View();
        }
        public ActionResult GenerateSavingsRefundRegisterReport(string office_id, string from_date, string to_date, int qType, int centerID)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Qtype", Value = qType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Org", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Office", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "Center", Value = centerID.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Member", Value = "0" });
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });
                PrintSSRSReport("/gBanker_Reports/SavingsRefundRegister", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void MapDropdownForYearMonth()
        {
            var PleaseSelect = new SelectListItem { Text = "Please Select", Value = "" };
            var yearList = new List<SelectListItem>();
            yearList.Add(PleaseSelect);
            for (int i = DateTime.Now.Year; i >= (DateTime.Now.Year) - 1; i--)
            {
                yearList.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            ViewData["YearList"] = yearList;

            var monthList = new List<SelectListItem>();
            monthList.Add(new SelectListItem() { Text = "Please Select", Value = "" });
            for (var i = 1; i <= 12; i++)
            {
                monthList.Add(new SelectListItem { Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(i), Value = i.ToString() });
            }
            ViewData["MonthList"] = monthList;
        }
        public ActionResult EmployeeSalaryInfo()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;

            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoggedInEmployee.OfficeID));
            var HeadOfficeCode = offcdetail.FirstLevel;
            var Headoffcdetail = officeService.GetByOfficeCode(HeadOfficeCode);
            var Secondoffcdetail = officeService.GetByOfficeCode(offcdetail.SecondLevel);
            var Thirdoffcdetail = officeService.GetByOfficeCode(offcdetail.ThirdLevel);
            var Fourthoffcdetail = officeService.GetByOfficeCode(offcdetail.FourthLevel);

            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];

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
            var param = new { @OfficeID = SessionHelper.LoggedInEmployee.OfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForYearMonth();
            return View();
        }
        public ActionResult GenerateEmployeeSalaryInfoReport(string office_id, int year, int month)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Office", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "Year", Value = year.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Month", Value = month.ToString() });
                PrintSSRSReport("/gBanker_Reports/EmployeeSalaryInfo", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        //public ActionResult GenerateBorrowerDataReport(string dateTo, string isChecked)
        //{
        //    int OfficeID;
        //    int OfficeIDTo;
        //    try
        //    {
        //        if(isChecked == "true")
        //        {
        //            OfficeID = 00000;
        //            OfficeIDTo = 99999;
        //        }
        //        else
        //        {
        //            OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
        //            OfficeIDTo = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
        //        }
        //        var paramValues = new List<ParameterValue>();
        //        paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = OfficeID.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "OfficeIDTo", Value = OfficeIDTo.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "PrintDate", Value = dateTo });
        //        PrintSSRSReport("/gBanker_Reports/BorrowersData", paramValues.ToArray(), "gBankerReport");
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        // Ends Sabet

        // sakib start

        private void MapDropdownForSectoralBreakDownOfLoan()
        {
            var qTypeList = new List<SelectListItem>();
            qTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            qTypeList.Add(new SelectListItem { Text = "All", Value = 1.ToString() });
            ViewData["QTypeList"] = qTypeList;
        }
        public ActionResult SectoralBreakDownOfLoan() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }
            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForSectoralBreakDownOfLoan();
            return View();
        }
        public ActionResult GenerateSectoralBreakDownOfLoanReport(int qtype, string office_id, string from_date, string to_date)
        {
            try
            {

                var MainProductCode = "";

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Qtype", Value = qtype.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "Date_From", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "Date_To", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "MainProductCode", Value = MainProductCode.ToString() });

                PrintSSRSReport("/gBanker_Reports/SectoralBreakDownOfLoan", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PassBookCheckRegister() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;
            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }
        public ActionResult GeneratePassBookCheckRegisterReport
             (int office_id, string dateTo, int? center)
        {
            try
            {
                var org = SessionHelper.LoginUserOrganizationID;

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Org", Value = org.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Office", Value = office_id.ToString() });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = dateTo });
                paramValues.Add(new ParameterValue() { Name = "centerID", Value = (center ?? 0).ToString() });
                PrintSSRSReport("/gBanker_Reports/PassBookCheckRegister", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void MapDropdownForLoanDisbursement()
        {
            var qTypeList = new List<SelectListItem>();
            qTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            qTypeList.Add(new SelectListItem { Text = "All", Value = 1.ToString() });
            ViewData["QTypeList"] = qTypeList;
        }
        public ActionResult LoanDisbursement() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForLoanDisbursement();
            return View();
        }
        public ActionResult GenerateLoanDisbursementReport(int qtype, string office_id, string from_date, string to_date)
        {
            try
            {


                var org = SessionHelper.LoginUserOrganizationID;
                int Center = 0;
                int Member = 0;

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Qtype", Value = qtype.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Office", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "Org", Value = org.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Center", Value = Center.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Member", Value = Member.ToString() });



                PrintSSRSReport("/gBanker_Reports/LoanDisbursement", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GenerateLoanDisbursementReportExport(int qtype, string office_id, string from_date, string to_date)
        {
            try
            {
                //int Center = 0;
                //int Member = 0;
                //var param = new { Qtype = qtype, @Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, Member = Member, DateFrom = from_date, DateTo = to_date };
                //var allRepaymentSchedule = weeklyReportService.LoanDisburseExport(param);
                //var reportParam = new Dictionary<string, object>();
                //reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("from_date", from_date);
                //reportParam.Add("to_date", to_date);
                //ReportHelper.ExportExcelReport("LoanDisbursement.rpt", allRepaymentSchedule.Tables[0], reportParam);
                //return Content(string.Empty);

                int Center = 0;
                int Member = 0;
                var param = new { Qtype = qtype, @Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, Member = Member, DateFrom = from_date, DateTo = to_date };
                GridView gv = new GridView();
                var allRepaymentSchedule = weeklyReportService.LoanDisburseExport(param);
                var detail = allRepaymentSchedule.Tables[0];
                gv.DataSource = detail;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=LoanDisburseExport.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return RedirectToAction("LoanDisburseExport");

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Buro_SavingsAccountOpening_Register() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }
        public ActionResult GenerateBuro_SavingsAccountOpening_RegisterReport(int office_id, string from_date, string dateTo)
        {
            try
            {
                var org = SessionHelper.LoginUserOrganizationID;

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "OrgID", Value = org.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = office_id.ToString() });
                paramValues.Add(new ParameterValue() { Name = "StartDate", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "EndDate", Value = dateTo });

                PrintSSRSReport("/gBanker_Reports/Buro_SavingsAccountOpening_Register", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void MapDropdownForProgramOrganizerLoan_Portfolio_ReportQType()
        {
            var qTypeList = new List<SelectListItem>();
            qTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            qTypeList.Add(new SelectListItem { Text = "All", Value = 2.ToString() });
            ViewData["QTypeList"] = qTypeList;
        }
        public ActionResult ProgramOrganizerLoan_Portfolio_Report() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["OrganizerList"] = items;

            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForProgramOrganizerLoan_Portfolio_ReportQType();
            return View();
        }
        public ActionResult GenerateProgramOrganizerLoan_Portfolio_Report(int qType, string office_id, string Emp_Id, string from_date, string to_date)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "QType", Value = qType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OrgID", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "EmpID", Value = Emp_Id.ToString() });
                paramValues.Add(new ParameterValue() { Name = "FirstDate", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "ToDate", Value = to_date });

                PrintSSRSReport("/gBanker_Reports/ProgramOrganizerLoan_Portfolio_Report", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MapDropdownForMIS_Loan_Aging()
        {
            var qTypeList = new List<SelectListItem>();
            qTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            qTypeList.Add(new SelectListItem { Text = "All", Value = 10.ToString() });
            ViewData["QTypeList"] = qTypeList;
        }
        public ActionResult MIS_Loan_Aging() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForMIS_Loan_Aging();
            return View();
        }
        public ActionResult GenerateMIS_Loan_Aging(string office_id, string from_date, string to_date, int qType)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var paramValues = new List<ParameterValue>();


                paramValues.Add(new ParameterValue() { Name = "OrgID", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "OfficeIDTO", Value = 8.ToString() });
                paramValues.Add(new ParameterValue() { Name = "CenterID", Value = 00.ToString() });
                paramValues.Add(new ParameterValue() { Name = "CenterIDTo", Value = 99999999.ToString() });
                paramValues.Add(new ParameterValue() { Name = "StaffID", Value = 00.ToString() });
                paramValues.Add(new ParameterValue() { Name = "StaffIDTo", Value = 999999.ToString() });
                paramValues.Add(new ParameterValue() { Name = "productID", Value = 00.ToString() });
                paramValues.Add(new ParameterValue() { Name = "ProductIDTo", Value = 999999.ToString() });
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "Qtype", Value = qType.ToString() });

                PrintSSRSReport("/gBanker_Reports/MIS_Loan_Aging", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MapDropdownForMIS_LoanPortfolioMarch()
        {
            var qTypeList = new List<SelectListItem>();
            qTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            qTypeList.Add(new SelectListItem { Text = "All", Value = 5.ToString() });
            ViewData["QTypeList"] = qTypeList;
        }
        public ActionResult MIS_LoanPortfolioMarch() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level]; ;
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForMIS_LoanPortfolioMarch();
            return View();
        }
        public ActionResult GenerateMIS_LoanPortfolioMarch(int qType, string office_id, string from_date, string to_date)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var paramValues = new List<ParameterValue>();

                paramValues.Add(new ParameterValue() { Name = "QType", Value = qType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OrgID", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "EmpID", Value = 0.ToString() });
                paramValues.Add(new ParameterValue() { Name = "FirstDate", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "ToDate", Value = to_date });


                PrintSSRSReport("/gBanker_Reports/MIS_LoanPortfolioMarch", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // sakib end


        // Meraj
        //Daily Withdrawal List
        public ActionResult DailyWithdrawalList() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            //MapDropdownForMemberQType();
            return View();
        }

        public ActionResult GenerateDailyWithdrawalListReport(string office_id, string date)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Org", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeId", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "Date", Value = date });


                PrintSSRSReport("/gBanker_Reports/DailyWithdrawalList", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //DailySummarySamitywise
        public ActionResult DailySummarySamitywise() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            //MapDropdownForMemberQType();
            return View();
        }

        public ActionResult GenerateDailySummarySamityWiseReport(string office_id, string date)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Org", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeId", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "Date", Value = date });


                PrintSSRSReport("/gBanker_Reports/DailySummarySamitywise", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // AIS_Acc_Cash_Book

        public ActionResult AISAccCashBook() //View
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            DropdownAccLevelForIncExp();
            return View();
        }
        public ActionResult GenerateAISAccCashBookReport(string office_id, string from_date, string to_date, int acc_level)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "to_date", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "office_id", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = acc_level.ToString() });

                PrintSSRSReport("/gBanker_Reports/AIS_Acc_Cash_Book", paramValues.ToArray(), "gBankerDbContext");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //Dropout Member List
        public ActionResult DropoutMemberList() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            //MapDropdownForMemberQType();
            return View();
        }

        //public ActionResult GenerateDropoutMemberListReport(string office_id, string from_date, string to_date)
        //{
        //    try
        //    {
        //        var orgId = SessionHelper.LoginUserOrganizationID;
        //        var paramValues = new List<ParameterValue>();
        //        paramValues.Add(new ParameterValue() { Name = "OrgID", Value = orgId.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = office_id });
        //        paramValues.Add(new ParameterValue() { Name = "FromDate", Value = from_date });
        //        paramValues.Add(new ParameterValue() { Name = "ToDate", Value = to_date });

        //        PrintSSRSReport("/gBanker_Reports/Buro_Member_Dropout_list", paramValues.ToArray(), "gBankerReport");
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public ActionResult GenerateDropoutMemberListReport
        (string office_id, string from_date, string to_date, int? qType)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "OrgID", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "FromDate", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "ToDate", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "qType", Value = (qType ?? 0).ToString() });

                PrintSSRSReport("/gBanker_Reports/Buro_Member_Dropout_list", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Loan Proposal Approval And Disbursement
        private void MapDropdownForLoanProposalApprovalAndDisbursement()
        {
            var qTypeList = new List<SelectListItem>();
            qTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            qTypeList.Add(new SelectListItem { Text = "All", Value = 2.ToString() });
            ViewData["QTypeList"] = qTypeList;
        }
        public ActionResult LoanProposalApprovalAndDisbursement() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForLoanProposalApprovalAndDisbursement();
            return View();
        }
        public ActionResult GenerateLoanProposalApprovalAndDisbursementReport(int qtype, string office_id, string from_date, string to_date)
        {
            try
            {


                var org = SessionHelper.LoginUserOrganizationID;
                int Center = 0;
                int Member = 0;

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Qtype", Value = qtype.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Org", Value = org.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Office", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "Center", Value = Center.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Member", Value = Member.ToString() });
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });

                PrintSSRSReport("/gBanker_Reports/LoanProposalApprovalAndDisbursement", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Meraj
        private void MapDropdownForLoanDisbursementRealizationAndOutStanding()
        {
            var qTypeList = new List<SelectListItem>();
            qTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            qTypeList.Add(new SelectListItem { Text = "All", Value = 3.ToString() });
            ViewData["QTypeList"] = qTypeList;
        }
        public ActionResult LoanDisbursementRealizationAndOutStanding()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForLoanDisbursementRealizationAndOutStanding();
            return View();
        }
        public ActionResult GenerateLoanDisbursementRealizationAndOutStandingReport(int qtype, string office_id, string from_date, string to_date)
        {
            try
            {
                //int qtype = 3;
                var orgId = SessionHelper.LoginUserOrganizationID;
                int productId = 0;
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "QType", Value = qtype.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OrgID", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "ProductID", Value = productId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });

                PrintSSRSReport("/gBanker_Reports/LoanDisbursementRealizationAndOutStanding", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Meran N
        private void MapDropdownForHOLoanPortfolioStatement()
        {
            var qTypeList = new List<SelectListItem>();
            qTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            qTypeList.Add(new SelectListItem { Text = "All", Value = 4.ToString() });
            ViewData["QTypeList"] = qTypeList;
        }
        public ActionResult HOLoanPortfolioStatement() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForHOLoanPortfolioStatement();
            return View();
        }
        public ActionResult GenerateHOLoanPortfolioStatementReport(string office_id, string from_date, string to_date, int qType)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "QType", Value = qType.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OrgID", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "EmpID", Value = "0" });
                paramValues.Add(new ParameterValue() { Name = "FirstDate", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "ToDate", Value = to_date });


                PrintSSRSReport("/gBanker_Reports/HO_Loan_Portfolio_1", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Meraj NN

        private void MapDropdownForLoanAgingHO()
        {
            var qTypeList = new List<SelectListItem>();
            qTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            qTypeList.Add(new SelectListItem { Text = "All", Value = 11.ToString() });
            ViewData["QTypeList"] = qTypeList;
        }
        public ActionResult LoanAgingHOStatement() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForLoanAgingHO();
            return View();
        }
        public ActionResult GenerateLoanAgingHOStatementReport(string office_id, string from_date, string to_date, int qType)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var param = new
                {
                    OrgID = orgId,
                    OfficeID = office_id,
                    OfficeIDTO = office_id,
                    CenterID = 00,
                    CenterIDTo = 99999999,
                    StaffID = 00,
                    StaffIDTo = 999999,
                    productID = 00,
                    ProductIDTo = 99999999,
                    DateFrom = from_date,
                    DateTo = to_date,
                    Qtype = 1
                };
                var dataTableForQtype1 = ultimateReportService.GetDataWithParameter(param, "OverdueClassificationConsolidation");
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "OrgID", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "OfficeIDTO", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "CenterID", Value = 00.ToString() });
                paramValues.Add(new ParameterValue() { Name = "CenterIDTo", Value = 99999999.ToString() });
                paramValues.Add(new ParameterValue() { Name = "StaffID", Value = 00.ToString() });
                paramValues.Add(new ParameterValue() { Name = "StaffIDTo", Value = 999999.ToString() });
                paramValues.Add(new ParameterValue() { Name = "productID", Value = 00.ToString() });
                paramValues.Add(new ParameterValue() { Name = "ProductIDTo", Value = 99999999.ToString() });
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "Qtype", Value = qType.ToString() });

                PrintSSRSReport("/gBanker_Reports/LoanAgingHO", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Meraj
        public ActionResult MonthlyStatisticsReport() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }

            return View();
        }
        public ActionResult GenerateStatiticsReport(string office_id, string to_date)
        {
            var param = new { @OfficeId = office_id, DateTo = to_date };
            var result = accReportService.GenerateStatiticsReport(param);
            var result1 = 10;
            return Json(result1, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerateMonthlyStatisticsReport(string office_id, string to_date)
        {
            try
            {
                int qtype = 1;
                var orgId = SessionHelper.LoginUserOrganizationID;
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Qtype", Value = qtype.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OrgId", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeId", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });

                PrintSSRSReport("/gBanker_Reports/Monthly_Statistics_Report", paramValues.ToArray(), "gBankerDBCOntext");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult IncomeExpense_HO()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            DropdownAccLevelForIncExp();
            return View();
        }
        public ActionResult GenerateIncomeExpense_HO_Report(string office_id, string from_date, string to_date, int acc_level)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "from_date", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "to_date", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "office_id", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "AccLevel", Value = acc_level.ToString() });
                paramValues.Add(new ParameterValue() { Name = "And_Condition", Value = "" });
                paramValues.Add(new ParameterValue() { Name = "AllOffice", Value = 0.ToString() });

                PrintSSRSReport("/gBanker_Reports/AIS_Income Expense_HO", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetAccLevelByAccID(string AccID)
        {
            try
            {
                List<MemberPassBookRegisterViewModel> List_MemberPassBookRegisterViewModel = new List<MemberPassBookRegisterViewModel>();
                var param = new { OfficeId = LoginUserOfficeID, AccID = AccID };
                var div_items = ultimateReportService.GetAccLevelByAccId(param);

                List_MemberPassBookRegisterViewModel = div_items.Tables[0].AsEnumerable()
                .Select(row => new MemberPassBookRegisterViewModel
                {
                    AccID = row.Field<int>("AccID"),
                    AccName = row.Field<string>("AccName"),
                    CashBook = row.Field<string>("CashBook"),
                    AccLevel = row.Field<int>("AccLevel")

                }).ToList();

                return Json(List_MemberPassBookRegisterViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult getAccLevelForBankBookByAccID(string AccID)
        {
            try
            {
                List<MemberPassBookRegisterViewModel> List_MemberPassBookRegisterViewModel = new List<MemberPassBookRegisterViewModel>();
                var param = new { OfficeId = LoginUserOfficeID, AccID = AccID };
                var div_items = ultimateReportService.getAccLevelForBankBookByAccID(param);

                List_MemberPassBookRegisterViewModel = div_items.Tables[0].AsEnumerable()
                .Select(row => new MemberPassBookRegisterViewModel
                {
                    AccID = row.Field<int>("AccID"),
                    AccName = row.Field<string>("AccName"),
                    CashBook = row.Field<string>("CashBook"),
                    AccLevel = row.Field<int>("AccLevel")

                }).ToList();

                return Json(List_MemberPassBookRegisterViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult GetMemberCashBookList(string Member_id)
        {
            List<MemberPassBookRegisterViewModel> List_MemberPassBookRegisterViewModel = new List<MemberPassBookRegisterViewModel>();
            var param = new { OfficeId = LoginUserOfficeID };
            var div_items = ultimateReportService.GetMemberCashBookList(param);

            List_MemberPassBookRegisterViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new MemberPassBookRegisterViewModel
            {
                AccID = row.Field<int>("AccID"),
                AccName = row.Field<string>("AccName"),
                CashBook = row.Field<string>("CashBook")

            }).ToList();

            var viewProduct = List_MemberPassBookRegisterViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.AccID.ToString(),
                Text = string.Format("{0} - {1}", x.CashBook, x.AccName)
            });
            var d_items = new List<SelectListItem>();
            d_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            d_items.AddRange(viewProduct);
            return Json(d_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBankBookList(string Member_id)
        {
            List<MemberPassBookRegisterViewModel> List_MemberPassBookRegisterViewModel = new List<MemberPassBookRegisterViewModel>();
            var param = new { OfficeId = LoginUserOfficeID };
            var div_items = ultimateReportService.GetBankBookList(param);

            List_MemberPassBookRegisterViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new MemberPassBookRegisterViewModel
            {
                AccID = row.Field<int>("AccID"),
                AccCode = row.Field<string>("AccCode"),
                AccName = row.Field<string>("AccName")

            }).ToList();

            var viewProduct = List_MemberPassBookRegisterViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.AccID.ToString(),
                Text = string.Format("{0} - {1}", x.AccCode, x.AccName)
            });
            var d_items = new List<SelectListItem>();
            d_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            d_items.AddRange(viewProduct);
            return Json(d_items, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult GetgetOrganizerList()
        {
            List<EmployeeViewModel> List_EmployeeViewModel = new List<EmployeeViewModel>();
            var param = new { OfficeId = LoginUserOfficeID };
            var div_items = ultimateReportService.GetgetOrganizerList(param);

            List_EmployeeViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new EmployeeViewModel
            {
                EmployeeID = row.Field<int>("EmployeeID"),
                EmpName = row.Field<string>("EmployeeName")
            }).ToList();

            var viewProduct = List_EmployeeViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.EmployeeID.ToString(),
                Text = string.Format("{0} - {1}", x.EmployeeID, x.EmpName)
            });
            var d_items = new List<SelectListItem>();
            //d_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            d_items.AddRange(viewProduct);
            return Json(d_items, JsonRequestBehavior.AllowGet);
        }

        // Zone, Area, Branch starts
        public JsonResult GetZoneList(string HO_val)
        {
            var ofcId = officeService.GetById(Convert.ToInt32(HO_val)).OfficeCode;
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 2 && c.FirstLevel == ofcId && c.OrgID == Convert.ToInt32(LoggedInOrganizationID));
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAreaList(string HO_val, string zone_val)
        {
            var ho_code = officeService.GetById(Convert.ToInt32(HO_val)).OfficeCode;
            var zone_code = officeService.GetById(Convert.ToInt32(zone_val)).OfficeCode;
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 3 && c.FirstLevel == ho_code && c.SecondLevel == zone_code && c.OrgID == Convert.ToInt32(LoggedInOrganizationID) && c.OfficeID== LoginUserOfficeID);
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOfficeList(string HO_val, string zone_val, string area_val)
        {
            var ho_code = officeService.GetById(Convert.ToInt32(HO_val)).OfficeCode;
            var zone_code = officeService.GetById(Convert.ToInt32(zone_val)).OfficeCode;
            var area_code = "";
            if (area_val != null)
            {
                area_code = officeService.GetById(Convert.ToInt32(area_val == null ? "0" : area_val)).OfficeCode;
            }

            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 4 && c.FirstLevel == ho_code && c.SecondLevel == zone_code && c.ThirdLevel == area_code && c.OrgID == Convert.ToInt32(LoggedInOrganizationID));
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            if (viewOffice.ToList().Count > 0)
            {
                office_items.AddRange(viewOffice);
            }
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        // Zone, Area, Branch Ends

        private void MapDropDownForBlankReport()
        {
            var reportList = new List<SelectListItem>();
            reportList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            reportList.Add(new SelectListItem { Text = "Blank Loan Collection Sheet", Value = "1" });
            reportList.Add(new SelectListItem { Text = "Blank Contractual Savings Collection Sheet", Value = "2" });
            reportList.Add(new SelectListItem { Text = "Blank General Savings Collection Sheet", Value = "3" });
            ViewData["BlankReportList"] = reportList;
        }
        public ActionResult BlankCollectionReportSheet() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }            
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();


            if (!IsDayInitiated)
            {                
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropDownForBlankReport();
            return View();
        }
        public ActionResult GenerateBlankLoanCollectionSheet(int office_id, int reportType, string date)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;                
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Org", Value = orgId.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OfficeId", Value = office_id.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Date", Value = date });
                if(reportType == 1)
                {
                    PrintSSRSReport("/gBanker_Reports/LoanCollectionSheet_HW", paramValues.ToArray(), "gBankerReport");
                }
                if (reportType == 2)
                {
                    PrintSSRSReport("/gBanker_Reports/Buro_Contractual_Savings_Collection_Statement_HW", paramValues.ToArray(), "gBankerReport");
                }
                if (reportType == 3)
                {
                    PrintSSRSReport("/gBanker_Reports/MIS_General_Savings-1_Collection_Statement_HW", paramValues.ToArray(), "gBankerReport");
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void MapDropdownForbKashTransactionLog()
        {
            var productTypeList = new List<SelectListItem>();
            productTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            productTypeList.Add(new SelectListItem { Text = "Loan", Value = "L" });
            productTypeList.Add(new SelectListItem { Text = "Savings", Value = "S" });
            productTypeList.Add(new SelectListItem { Text = "Both", Value = "B" });
            ViewData["ProductTypeList"] = productTypeList;
        }
        public ActionResult bKashDepositBalanceExport(int office_id, string from_date, string to_date, string productType)
        {
            try
            {
                
                    var param = new { DateFrom=from_date, DateTo=to_date, OfficeID = office_id, ProdType = productType };
                    GridView gv = new GridView();
                    var allRepaymentSchedule = accReportService.ExportbKashDepositBalance(param);
                    var detail = allRepaymentSchedule.Tables[0];
                    gv.DataSource = detail;
                    gv.DataBind();
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=GenerateLedgerReport.xls");
                    Response.ContentType = "application/ms-excel";
                    Response.Charset = "";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new HtmlTextWriter(sw);
                    gv.RenderControl(htw);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();

                    return RedirectToAction("bKashDepositBalance");
                       

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult bKashTransactionLog() 
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;
            }            
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForbKashTransactionLog();
            return View();
        }
        public ActionResult GetbKashTransactionLogReport(int office_id, string from_date, string to_date, string productType)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });               
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = office_id.ToString()});
                paramValues.Add(new ParameterValue() { Name = "ProdType", Value = productType });

                PrintSSRSReport("/gBanker_Reports/Customer_Balance", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult bKashErrorTransactionLog()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;
            }
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }
        public ActionResult YearlyPlanningGoal()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;
            }
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }            
            return View();
        }
        public ActionResult WeeklyPlanningGoal()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["OrganizerList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;
            }
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }
        public ActionResult GetWeeklyPlanningGoalReport(int office_id, string to_date, string Emp_Id)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = office_id.ToString() });

                paramValues.Add(new ParameterValue() { Name = "PrintDate", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "EmpID", Value = Emp_Id.ToString() });
                PrintSSRSReport("/gBanker_Reports/Weekly_planning", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public ActionResult GetYearlyPlanningGoalReport(int office_id,  string to_date)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = office_id.ToString() });

                paramValues.Add(new ParameterValue() { Name = "PrintDate", Value = to_date });
                 PrintSSRSReport("/gBanker_Reports/Yearly_Target_bu", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetbKashErrorTransactionLogReport(int office_id, string from_date, string to_date)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = office_id.ToString() });                
                PrintSSRSReport("/gBanker_Reports/BKashErrorTransactionLog", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region New Report For Buro
        public ActionResult SecondLedgerRegister()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            //ViewData["OfficeLevel"] = offcdetail.OfficeLevel;
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {

                //ViewData["TrxDate"] = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForLoanAgingAndCenterList();
            var qTypeList = new List<SelectListItem>();
            qTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            qTypeList.Add(new SelectListItem { Text = "All", Value = "0" });
            qTypeList.Add(new SelectListItem { Text = "Center Wise", Value = "2" });
            qTypeList.Add(new SelectListItem { Text = "Employee Wise", Value = "3" });
            ViewData["QTypeList"] = qTypeList;
            return View();
        }
        public JsonResult GetCenterList(int officeID)
        {
            var centerList = centerService.GetMany(x => x.IsActive == true
            && x.OfficeID == officeID && x.OrgID == LoggedInOrganizationID).ToList();
            var viewOffice = centerList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterCode.ToString() + " " + x.CenterName.ToString()
            });
            var centerItem = new List<SelectListItem>();
            centerItem.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            if (viewOffice.ToList().Count > 0)
                centerItem.AddRange(viewOffice);
            return Json(centerItem, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CSOpeningAll()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            //ViewData["OfficeLevel"] = offcdetail.OfficeLevel;
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {

                //ViewData["TrxDate"] = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForLoanAgingAndCenterList();
            var qTypeList = new List<SelectListItem>();
            qTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            qTypeList.Add(new SelectListItem { Text = "All", Value = "2" });
            //qTypeList.Add(new SelectListItem { Text = "Center Wise", Value = "2" });
            qTypeList.Add(new SelectListItem { Text = "Employee Wise", Value = "3" });
            ViewData["QTypeList"] = qTypeList;
            return View();
        }
        public ActionResult CSOpening()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            //ViewData["OfficeLevel"] = offcdetail.OfficeLevel;
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;

            }

            //ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                //ViewData["TrxDate"] = Convert.ToDateTime(allProducts.Tables[0].Rows[0][1].ToString());
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            MapDropdownForLoanAgingAndCenterList();
            var qTypeList = new List<SelectListItem>();
            qTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            //qTypeList.Add(new SelectListItem { Text = "All", Value = "0" });
            //qTypeList.Add(new SelectListItem { Text = "Center Wise", Value = "2" });
            //qTypeList.Add(new SelectListItem { Text = "Employee Wise", Value = "3" });
            ViewData["QTypeList"] = qTypeList;
            return View();
        }

        public ActionResult ReportLedger(string pg, string fromdate, string toDate
            , int? office_id, int? qType, int? data)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                if (pg == "sec")
                {
                    paramValues.Add(new ParameterValue() { Name = "OfficeId", Value = office_id.ToString() });
                    paramValues.Add(new ParameterValue() { Name = "DateTo", Value = toDate });
                    paramValues.Add(new ParameterValue() { Name = "EmpIdFrom", Value = (qType == 3 ? (data ?? 0) : 0).ToString() });
                    paramValues.Add(new ParameterValue() { Name = "EmpIdTo", Value = (qType == 3 ? (data ?? 0) : 999999).ToString() });
                    paramValues.Add(new ParameterValue() { Name = "CenterIdFrom", Value = (qType == 2 ? (data ?? 0) : 0).ToString() });
                    paramValues.Add(new ParameterValue() { Name = "CenterIdTo", Value = (qType == 2 ? (data ?? 0) : 999999).ToString() });
                    pg = "secondledger";
                }
                if (pg == "csOpening")
                {
                    qType = (qType ?? 0);
                    paramValues.Add(new ParameterValue() { Name = "QType", Value = (qType > 0 ? qType : 0).ToString() });
                    paramValues.Add(new ParameterValue() { Name = "OfficeId", Value = office_id.ToString() });
                    paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = fromdate });
                    paramValues.Add(new ParameterValue() { Name = "DateTo", Value = toDate });
                    paramValues.Add(new ParameterValue() { Name = "EmpIdFrom", Value = (qType == 3 ? (data ?? 0) : 0).ToString() });
                    paramValues.Add(new ParameterValue() { Name = "EmpIdTo", Value = (qType == 3 ? (data ?? 0) : 999999).ToString() });
                    pg = ((qType == 1 | qType == 0) ? "csOpening"
                        : qType == 2 ? "csOpeningAll"
                        : qType == 3 ? "csOpeningAll" : "");
                }
                PrintSSRSReport("/gBanker_Reports/" + pg, paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion New Report For Buro

        #region Branch Monitoring Report
        public ActionResult BranchMonitoringReport() //VIEW
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            ViewData["ProductListByMember"] = items;
            ViewData["LoanTermList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;
            }
            
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate;
            }
            return View();
        }

        public ActionResult GenerateBranchMonitoringReport(string OfficeId, string DateFrom, string DateTo)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "OfficeId", Value = OfficeId });
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = DateFrom });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = DateTo });

                PrintSSRSReport("/gBanker_Reports/BranchMonitoringReport", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion       

        public ActionResult CurrentDueOverdueLoanee()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["OrganizerList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;
            }
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }
        public ActionResult GenerateCurrentDueOverdueLoaneeReport(string office_id, string to_date)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "OfficeId", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });
                PrintSSRSReport("/gBanker_Reports/CurrentDueOverdueLoanee", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult ALLFPsSummaryReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            ViewData["ProductListByMember"] = items;
            ViewData["LoanTermList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;
            }

            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate;
            }
            return View();
        }
        public ActionResult GenerateALLFPsReport(int office_id, string to_date)
        {
            try
            {
                var office = officeService.GetById(office_id);
                var org = OrganizationName;
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "OfficeId", Value = office_id.ToString() });
                paramValues.Add(new ParameterValue() { Name = "ToDate", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "OfficeName", Value = office.OfficeName });
                paramValues.Add(new ParameterValue() { Name = "OfficeCode", Value = office.OfficeCode });
                paramValues.Add(new ParameterValue() { Name = "OrganizationName", Value = OrganizationName });
                PrintSSRSReport("/gBanker_Reports/FPCollectionSummary", paramValues.ToArray(), "gBankerReport");
                
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult AgrosorActivitiesStatement()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["OrganizerList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
            if (offcdetail.OfficeLevel == 1)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 2)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
            }
            else if (offcdetail.OfficeLevel == 3)
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
            }
            else
            {
                ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
                ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
                ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
                ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;
            }
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);

            var detail = allProducts.ToString();

            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }
        public ActionResult GenerateAgrosorActivitiesReport(string office_id, string to_date)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "OfficeId", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });
                PrintSSRSReport("/gBanker_Reports/AgrosorActivitiesStatement", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult LegalInfoStatement()
        {
            ViewData["TrxDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult GenerateLegalInfoStatementReport(string from_date, string to_date)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "OfficeId", Value = LoginUserOfficeID.ToString() });
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = from_date});
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = to_date });
                PrintSSRSReport("/gBanker_Reports/LegalInfo", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetCenterList_Custom()
        {
            List<CenterViewModel> List_CenterViewModel = new List<CenterViewModel>();
            DataSet div_items;
            var param = new { OfficeId = LoginUserOfficeID, LoginUserRoleId = SessionHelper.LoginUserRoleId, LoggedInEmployeeID = SessionHelper.LoggedInEmployeeID };

            div_items = ultimateReportService.GetDataWithParameter(param, "GetOnlyCenter_custom");

            List_CenterViewModel = div_items.Tables[0].AsEnumerable()
            .Select(row => new CenterViewModel
            {
                CenterID = row.Field<int>("CenterID"),
                CenterCode = row.Field<string>("CenterCode"),
                CenterName = row.Field<string>("CenterName")
            }).ToList();

            var viewCenter = List_CenterViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.CenterID.ToString(),
                Text = x.CenterName.ToString()
            });
            var center_items = new List<SelectListItem>();
            if (viewCenter.ToList().Count > 0)
            {
                center_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            }
            center_items.AddRange(viewCenter);
            return Json(center_items, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetRecoverableAndRecoveryDateWise_Area_Zone_Wise_Center(string Qtype, string OfficeId, string DateFrom, string DateTo, string CenterId)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "OfficeIDFrom", Value = "00000" });
                paramValues.Add(new ParameterValue() { Name = "OfficeIDTo", Value = "99999" });
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = DateFrom });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = DateTo });
                paramValues.Add(new ParameterValue() { Name = "OrgName", Value = OrganizationName });
                paramValues.Add(new ParameterValue() { Name = "Qtype", Value = Qtype });
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = OfficeId });
                paramValues.Add(new ParameterValue() { Name = "CenterId", Value = CenterId });
                PrintSSRSReport("/gBanker_Reports/RecoverableRecovery_Area_Zone_TEST_Center", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult RecoverableAndRecovery()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;

            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoggedInEmployee.OfficeID));
            var HeadOfficeCode = offcdetail.FirstLevel;
            var Headoffcdetail = officeService.GetByOfficeCode(HeadOfficeCode);
            var Secondoffcdetail = officeService.GetByOfficeCode(offcdetail.SecondLevel);
            var Thirdoffcdetail = officeService.GetByOfficeCode(offcdetail.ThirdLevel);
            var Fourthoffcdetail = officeService.GetByOfficeCode(offcdetail.FourthLevel);

            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];

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
            ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");

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
            var model = new MemberViewModel();
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            var blnk_items = new List<SelectListItem>();
            model.CenterList = blnk_items;
            return View(model);
        }
        //public ActionResult RecoverableAndRecovery()
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
        //    var model = new MemberViewModel();
        //    model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);            
        //    return View(model);
        //}
        public ActionResult GetRecoverableAndRecoveryDateWise(string DateFrom, string DateTo)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "OfficeIDFrom", Value = "00000" });
                paramValues.Add(new ParameterValue() { Name = "OfficeIDTo", Value = "99999" });
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = DateFrom });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = DateTo });
                paramValues.Add(new ParameterValue() { Name = "OrgName", Value = OrganizationName });                
                PrintSSRSReport("/gBanker_Reports/RecoverableRecovery", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public JsonResult ZoneAreaShowHide()
        {
            var param = new { OfficeID = SessionHelper.LoginUserOfficeID, OrgID = LoggedInOrganizationID, LoginUserEmployeeID = SessionHelper.LoginUserEmployeeID, LoginUserRoleId = SessionHelper.LoginUserRoleId };
            var alldata = ultimateReportService.GetDataWithParameter(param, "Proc_Get_Zone_Area_Show_Hide");

            var listItems = alldata.Tables[0].AsEnumerable()
                .Select(x => new SelectListItem
                {
                    Value = x.Field<string>("Value"),
                    Text = x.Field<string>("Text")
                });

            return Json(listItems, JsonRequestBehavior.AllowGet);
        }





        public ActionResult GetRecoverableAndRecoveryDateWise_Area_Zone_Wise(string Qtype, string OfficeId, string DateFrom, string DateTo)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "OfficeIDFrom", Value = "00000" });
                paramValues.Add(new ParameterValue() { Name = "OfficeIDTo", Value = "99999" });
                paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = DateFrom });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = DateTo });
                paramValues.Add(new ParameterValue() { Name = "OrgName", Value = OrganizationName });
                paramValues.Add(new ParameterValue() { Name = "Qtype", Value = Qtype });
                paramValues.Add(new ParameterValue() { Name = "OfficeID", Value = OfficeId });

                PrintSSRSReport("/gBanker_Reports/RecoverableRecovery_Area_Zone_TEST", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GeneratePOMISEmploymentReport(string from_date, string to_date, string office_id)
        {
            try
            { 
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "OfficeId", Value = office_id });
                paramValues.Add(new ParameterValue() { Name = "OrgId", Value = (SessionHelper.LoginUserOrganizationID).ToString() });
                paramValues.Add(new ParameterValue() { Name = "FromDate", Value = from_date });
                paramValues.Add(new ParameterValue() { Name = "ToDate", Value = to_date });
                PrintSSRSReport("/gBanker_Reports/POMISEmployment", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GenerateProductWiseLoanBalanceReport(string Qtype, string MainProductCode)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Qtype", Value = Qtype });
                paramValues.Add(new ParameterValue() { Name = "Office", Value = (SessionHelper.LoginUserOfficeID).ToString() });
                paramValues.Add(new ParameterValue() { Name = "MainProductCode", Value = MainProductCode });
                paramValues.Add(new ParameterValue() { Name = "Org", Value = LoggedInOrganizationID.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OrgName", Value = ApplicationSettings.OrganiztionName });
                PrintSSRSReport("/gBanker_Reports/LoanBalanceProductWise", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GenerateWorkingAreaInfoReport(string end_date)
        {
            try
            {
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "OfficeId", Value = 1.ToString() });
                paramValues.Add(new ParameterValue() { Name = "OrgId", Value = (SessionHelper.LoginUserOrganizationID).ToString() });
                //paramValues.Add(new ParameterValue() { Name = "FromDate", Value = from_date });
                //paramValues.Add(new ParameterValue() { Name = "ToDate", Value = to_date });
                paramValues.Add(new ParameterValue() { Name = "enddate", Value = end_date });
                PrintSSRSReport("/gBanker_Reports/WorkingAreaInfo", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}