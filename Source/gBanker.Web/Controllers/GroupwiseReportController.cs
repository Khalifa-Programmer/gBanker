
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Service.ReportExecutionService;
using gBanker.Web.Helpers;
using gBanker.Web.Reports;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using gBanker.Data.CodeFirstMigration;
using Newtonsoft.Json;
using System.Text;
using System.Globalization;
using gBanker.Core.Filters;
using gBanker.Service.StoredProcedure;
using gBanker.Data.DBDetailModels;
using System.Web.Script.Serialization;
using CrystalDecisions.CrystalReports.Engine;

namespace gBanker.Web.Controllers
{
    public class GroupwiseReportController : BaseController
    {
        #region Variables
        private readonly IEmployeeService employeeService;
        private readonly IProductService productService;
        private readonly ICenterService centerService;
        private readonly IOfficeService officeService;
        private readonly IMemberService memberService;
        private readonly IGroupwiseReportService groupwiseReportService;
        private readonly IUltimateReportService unlimitedReportService;
        private readonly IPOMISReportService pomisReportService;
        private readonly IWeeklyReportService weeklyReportService;
        private readonly ILoanTrxService loanTrxService;
        private readonly ILoanSummaryService loanSummaryService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly ITargetAchievementBuroService targetAchievementBuroService;
        private readonly IAccReportService accReportService;
        private readonly ILoanCollectionService loanCollectionService;
        private readonly IEmployeeSPService employeespService;



        //private readonly IStatisticsReportService statisticsReportService;
        //private readonly IStatisticsDescriptionService statisticsDescriptionService;
        private readonly IStatisticsReportDetailsService statisticsReportDetailsService;
        public GroupwiseReportController(IEmployeeService employeeService, IOfficeService officeService, IProductService productService, ICenterService centerService, IMemberService memberService, IGroupwiseReportService groupwiseReportService, IUltimateReportService unlimitedReportService, IPOMISReportService pomisReportService, IWeeklyReportService weeklyReportService, ILoanTrxService loanTrxService, ILoanSummaryService loanSummaryService, IUltimateReportService ultimateReportService
            , IStatisticsReportDetailsService statisticsReportDetailsService,
            ITargetAchievementBuroService targetAchievementBuroService,
            ILoanCollectionService loanCollectionService, IAccReportService accReportService, IEmployeeSPService employeespService)

        {
            this.employeeService = employeeService;
            this.officeService = officeService;
            this.productService = productService;
            this.centerService = centerService;
            this.memberService = memberService;
            this.groupwiseReportService = groupwiseReportService;
            this.unlimitedReportService = unlimitedReportService;
            this.pomisReportService = pomisReportService;
            this.weeklyReportService = weeklyReportService;
            this.loanTrxService = loanTrxService;
            this.loanSummaryService = loanSummaryService;
            this.ultimateReportService = ultimateReportService;
            this.statisticsReportDetailsService = statisticsReportDetailsService;
            this.targetAchievementBuroService = targetAchievementBuroService;
            this.loanCollectionService = loanCollectionService;
            this.accReportService = accReportService;
            this.employeespService = employeespService;
        }
        #endregion

        #region Methods

        

// ----------------------------------------------------------------------------------------------------

 public ActionResult StaffProfileConsolidateNew()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }

 //-----------------------------------------------------------------------------------------------------------

  public ActionResult GenerateStaffProfileConsolidateOfficewiseNew(string Qtype, string OfficeId, string DateTo)
        {
            try
            {
                var param = new { OfficeId = OfficeId, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "NFI_Staff_Info_Profile");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rpt_NFI_Staff_Info.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }
        }
        public ActionResult OfficeWiseProgramMISReportNew()

        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;

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

            return View();
        }


//----------------------------------------------------------------------------------------------------------

 public ActionResult GenerateOfficeWiseProgramMISReportNew(string Qtype, string OfficeId, string DateFrom, string DateTo)

        {
            try
            {
                var param = new { Office = OfficeId, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "Proc_RPT_MIS_LoanReportNew");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("RPT_MIS_LoanStatementCrossTabHOConWithLogoNew.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }
        }
        public ActionResult OfficeWiseOTR()

        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["EmployeeList"] = items;

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
            var param = new { @OfficeID = SessionHelper.LoggedInEmployee.OfficeID };
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
            return View();
        }



//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

 
   public ActionResult GenerateOfficewiseOTR(string Qtype, string OfficeId, string DateTo, string Employeeid, string ZoneId, string AreaId, string BranchId, string Reporttype)

        {

            try
            {
                var param = new { OfficeID = OfficeId, Org = SessionHelper.LoginUserOrganizationID, Qtype = Qtype, EmployeeID = Employeeid, ZoneId = ZoneId, AreaId = AreaId, BranchId = BranchId };
                var alldata = groupwiseReportService.MRA4ConsolidateReport(param, "GenerateOTR");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("CompanyName", ApplicationSettings.OrganiztionName);

                if (Reporttype == "1")
                {
                    ReportHelper.PrintReport("OTRReportZoneWise.rpt", alldata.Tables[0], reportParam);
                }

                if (Reporttype == "2")
                {
                    ReportHelper.PrintReport("OTRReportAreaWise.rpt", alldata.Tables[0], reportParam);
                }

                if (Reporttype == "3")
                {
                    ReportHelper.PrintReport("OTRReportBranchWise.rpt", alldata.Tables[0], reportParam);
                }

                if (Reporttype == "4")
                {
                    ReportHelper.PrintReport("OTRReportEployeeWise.rpt", alldata.Tables[0], reportParam);
                }

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        public ActionResult POMIS1HQConsolidateEmployeewise()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["EmployeeList"] = items;

            //var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoggedInEmployee.OfficeID));
            //var HeadOfficeCode = offcdetail.FirstLevel;
            //var Headoffcdetail = officeService.GetByOfficeCode(HeadOfficeCode);
            //var Secondoffcdetail = officeService.GetByOfficeCode(offcdetail.SecondLevel);
            //var Thirdoffcdetail = officeService.GetByOfficeCode(offcdetail.ThirdLevel);
            //var Fourthoffcdetail = officeService.GetByOfficeCode(offcdetail.FourthLevel);

            //ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
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
            var param = new { @OfficeID = SessionHelper.LoggedInEmployee.OfficeID };
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
            return View();
            //IEnumerable<SelectListItem> items = new SelectList(" ");
            //ViewData["HOList"] = items;
            //ViewData["ZoneList"] = items;
            //ViewData["AreaList"] = items;
            //ViewData["OfficeList"] = items;
            //return View();
        }


//--------------------------------------------------------------------------------------------------------------


 public ActionResult GenerateMRA4HQConsolidateEmployeewise(string Qtype, string OfficeId, string DateTo, string Employeeid)
        {
            try
            {

                if (Qtype == "4")
                {

                    var param = new { OfficeID = OfficeId, Org = SessionHelper.LoginUserOrganizationID, Qtype = Qtype, EmployeeID = Employeeid };
                    var alldata = groupwiseReportService.MRA4ConsolidateReport(param, "GenerateOTR");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("CompanyName", ApplicationSettings.OrganiztionName);
                    //reportParam.Add("DateTo", DateTo);
                    ReportHelper.PrintReport("BranchWiseOTRReport.rpt", alldata.Tables[0], reportParam);
                }

                else if (Qtype == "3")
                {
                    var param = new { OfficeID = OfficeId, Org = SessionHelper.LoginUserOrganizationID, Qtype = Qtype, EmployeeID = Employeeid };
                    var alldata = groupwiseReportService.MRA4ConsolidateReport(param, "GenerateOTR");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("CompanyName", ApplicationSettings.OrganiztionName);
                    // reportParam.Add("DateTo", DateTo);
                    ReportHelper.PrintReport("AreaWiseOTRReport.rpt", alldata.Tables[0], reportParam);
                }

                else if (Qtype == "2")
                {
                    var param = new { OfficeID = OfficeId, Org = SessionHelper.LoginUserOrganizationID, Qtype = Qtype, EmployeeID = Employeeid };
                    var alldata = groupwiseReportService.MRA4ConsolidateReport(param, "GenerateOTR");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("CompanyName", ApplicationSettings.OrganiztionName);
                    //reportParam.Add("DateTo", DateTo);
                    ReportHelper.PrintReport("ZoneWiseOTRReport.rpt", alldata.Tables[0], reportParam);
                }

                else
                {
                    var param = new { OfficeID = OfficeId, Org = SessionHelper.LoginUserOrganizationID, Qtype = Qtype, EmployeeID = Employeeid };
                    var alldata = groupwiseReportService.MRA4ConsolidateReport(param, "GenerateOTR");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("CompanyName", ApplicationSettings.OrganiztionName);
                    //reportParam.Add("DateTo", DateTo);
                    ReportHelper.PrintReport("HOWiseOTR.rpt", alldata.Tables[0], reportParam);
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }









        //-------------------------------------------------------------------------------------------------------
        public ActionResult KPIConsolidate() //VIEW
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

        //-----------------------------------------------------------------------------------------------------


        public ActionResult GenerateKPIReport(int qtype, string office_id, string from_date)
        {
            try
            {
                var reportParam = new Dictionary<string, object>();
                //reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                var param1 = new { QType = qtype, OfficeId = SessionHelper.LoginUserOfficeID, ReportDate = from_date };
                var alldata1 = groupwiseReportService.GetProgramMISReportJCF(param1, "PROC_RPT_KPIReportDataPreview");
                ReportHelper.PrintReport("rptKPIReportDataGUK.rpt", alldata1.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        //------------------------------------------------------------------------------------------------------


        private void MapDropdownForSectoralBreakDownOfLoan()
        {
            var qTypeList = new List<SelectListItem>();
            qTypeList.Add(new SelectListItem { Text = "Please Select", Value = "" });
            qTypeList.Add(new SelectListItem { Text = "All", Value = 1.ToString() });
            ViewData["QTypeList"] = qTypeList;
        }

        //----------------------------------------------------------------------------------------------------------

        public ActionResult GetEmployeeList_CPSP(string Employeeid, string OfficeId)
        {
            var param = new { EmployeeId = Employeeid, OfficeId = OfficeId };
            var EmployeeList = ultimateReportService.GetDataWithParameter(param, "SP_EMPLOYEE_SEARCH_AUTOCMPLETE");
            List<Employee> employees = new List<Employee>();
            employees = EmployeeList.Tables[0].AsEnumerable()
                   .Select(row => new Employee
                   {
                       EmployeeID = row.Field<Int16>("EmployeeID"),
                       EmployeeCode = row.Field<string>("EmployeeCode"),
                       EmpName = row.Field<string>("EmployeeCode") + "--" + row.Field<string>("EmpName"),
                       PhoneNo = row.Field<string>("PhoneNo"),
                       Designation = row.Field<string>("Designation"),
                   }).ToList();
            return Json(employees, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GenerateCPSPmonthlyProgressReportProcess_New(string Qtype, string OfficeId, string DateTo, string ZoneID, string AreaID, string BranchID, string EmployeeID, string memberID, string centerID)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(DateTo);
                int Year = dt.Year;
                int Month = dt.Month;

                if (centerID == "" || centerID == "null")
                    centerID = "0";

                if (memberID == "" || memberID == "null")
                    memberID = "0";

                if (Qtype == "" || Qtype == "null")
                    Qtype = "0";

                if (OfficeId == "" || OfficeId == "null")
                    OfficeId = "0";

                if (DateTo == "" || DateTo == "null")
                    DateTo = DateTime.Now.Date.ToString("yyyy-MM-dd");


                if (EmployeeID == "")
                    EmployeeID = "0";


                if (ZoneID == "null")
                    ZoneID = "0";


                if (AreaID == "null")
                    AreaID = "0";


                if (BranchID == "null")
                    BranchID = "0";



                var param = new
                {
                    ProcessYear = Year,
                    ProcessMonth = Month,
                    OrgID = LoggedInOrganizationID,
                    LoginUserOfficeID = LoginUserOfficeID,
                    getOfficeName = SessionHelper.getOfficeName,
                    LoggedInEmployeeID = SessionHelper.LoggedInEmployeeID,
                    LoginUserRoleId = SessionHelper.LoginUserRoleId,
                    OrganizationName = SessionHelper.OrganizationName,
                    LoggedInOrganizationCode = SessionHelper.LoggedInOrganizationCode,
                    OrganizationAddress = SessionHelper.OrganizationAddress,
                    TransactionDay = SessionHelper.TransactionDay,
                    TransactionDate = SessionHelper.TransactionDate,
                    TransactionDashBoardString = SessionHelper.TransactionDashBoardString,
                    ZoneID = Convert.ToInt32(ZoneID),
                    AreaID = Convert.ToInt32(AreaID),
                    BranchID = Convert.ToInt32(BranchID),
                    EmployeeID = Convert.ToInt32(EmployeeID),
                    QType = Convert.ToInt32(Qtype),
                    OfficeId = Convert.ToInt32(OfficeId),
                    centerID = Convert.ToInt32(centerID),
                    memberID = Convert.ToInt32(memberID),

                };
                var alldata = groupwiseReportService.GetDataProcessLOg(param, "CUSTOM_SP_CPSP_MONTHLY_PROCESS");

                return Json(new { Result = "SUCCESS", Message = "SUCCESS" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GenerateCPSPmonthlyProgressReportNew(string Qtype, string OfficeId, string DateTo, string ZoneID, string AreaID, string BranchID, string EmployeeID, string memberID, string centerID, string ReportType, string ReportName)
        {
            try
            {

                var reportParam = new Dictionary<string, object>();
                //reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("FromDate", from_date);

                DateTime dt = Convert.ToDateTime(DateTo);
                int Year = dt.Year;
                int Month = dt.Month;

                if (memberID == "" || memberID == "null")
                    memberID = "0";

                if (EmployeeID == "")
                    EmployeeID = "0";


                if (ZoneID == "null")
                    ZoneID = "0";


                if (AreaID == "null")
                    AreaID = "0";


                if (BranchID == "null")
                    BranchID = "0";


                var param22 =
                  new
                  {
                      ProcessYear = Year,
                      ProcessMonth = Month,
                      OrgID = LoggedInOrganizationID,
                      LoginUserOfficeID = LoginUserOfficeID,
                      getOfficeName = SessionHelper.getOfficeName,
                      LoggedInEmployeeID = SessionHelper.LoggedInEmployeeID,
                      LoginUserRoleId = SessionHelper.LoginUserRoleId,
                      OrganizationName = SessionHelper.OrganizationName,
                      LoggedInOrganizationCode = SessionHelper.LoggedInOrganizationCode,
                      OrganizationAddress = SessionHelper.OrganizationAddress,
                      TransactionDay = SessionHelper.TransactionDay,
                      TransactionDate = SessionHelper.TransactionDate,
                      TransactionDashBoardString = SessionHelper.TransactionDashBoardString,
                      ZoneID = Convert.ToInt32(ZoneID),
                      AreaID = Convert.ToInt32(AreaID),
                      BranchID = Convert.ToInt32(BranchID),
                      EmployeeID = Convert.ToInt32(EmployeeID),
                      QType = Convert.ToInt32(Qtype),
                      OfficeId = Convert.ToInt32(OfficeId),
                      centerID = Convert.ToInt32(centerID),
                      memberID = Convert.ToInt32(memberID),
                      ReportType = Convert.ToInt32(ReportType),
                      ReportName = Convert.ToInt32(ReportName)
                  };

                var reportQTYPE = groupwiseReportService.GetDataProcessLOg(param22, "CUSTOM_SP_CPSP_MONTHLY_REPORT_GET_QTYPE");

                var QT = reportQTYPE.Tables[0].Rows[0]["QTYPE"].ToString();


                var param1 =
                    new
                    {
                        ProcessYear = Year,
                        ProcessMonth = Month,
                        OrgID = LoggedInOrganizationID,
                        LoginUserOfficeID = LoginUserOfficeID,
                        getOfficeName = SessionHelper.getOfficeName,
                        LoggedInEmployeeID = SessionHelper.LoggedInEmployeeID,
                        LoginUserRoleId = SessionHelper.LoginUserRoleId,
                        OrganizationName = SessionHelper.OrganizationName,
                        LoggedInOrganizationCode = SessionHelper.LoggedInOrganizationCode,
                        OrganizationAddress = SessionHelper.OrganizationAddress,
                        TransactionDay = SessionHelper.TransactionDay,
                        TransactionDate = SessionHelper.TransactionDate,
                        TransactionDashBoardString = SessionHelper.TransactionDashBoardString,
                        ZoneID = Convert.ToInt32(ZoneID),
                        AreaID = Convert.ToInt32(AreaID),
                        BranchID = Convert.ToInt32(BranchID),
                        EmployeeID = Convert.ToInt32(EmployeeID),
                        QType = Convert.ToInt32(QT),
                        OfficeId = Convert.ToInt32(OfficeId),
                        centerID = Convert.ToInt32(centerID),
                        memberID = Convert.ToInt32(memberID),
                        ReportType = Convert.ToInt32(ReportType),
                        ReportName = Convert.ToInt32(ReportName)
                    };

                var reportConfig = groupwiseReportService.GetDataProcessLOg(param1, "CUSTOM_SP_CPSP_MONTHLY_REPORT_SET");

                if (reportConfig.Tables.Count > 0)
                {
                    if (reportConfig.Tables[0].Rows.Count > 0)
                    {
                        var Id = reportConfig.Tables[0].Rows[0]["Id"].ToString();

                        var SL = reportConfig.Tables[0].Rows[0]["SL"].ToString();

                        var Qtype22 = reportConfig.Tables[0].Rows[0]["Qtype"].ToString();

                        var ReportType22 = reportConfig.Tables[0].Rows[0]["ReportType"].ToString();

                        var ReportName22 = reportConfig.Tables[0].Rows[0]["ReportName"].ToString();

                        var SPName = reportConfig.Tables[0].Rows[0]["SPName"].ToString();

                        var rptName = reportConfig.Tables[0].Rows[0]["rptName"].ToString();

                        var IsActive = reportConfig.Tables[0].Rows[0]["IsActive"].ToString();

                        var CreateDate = reportConfig.Tables[0].Rows[0]["CreateDate"].ToString();

                        var alldata1 = groupwiseReportService.GetDataProcessLOg(param1, SPName);

                        if (alldata1.Tables.Count > 0)
                        {
                            ReportHelper.PrintReport(rptName, alldata1.Tables[0], reportParam);
                            return Content(string.Empty);
                        }

                        return Content(string.Empty);
                    }
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "No Data Found!" });
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }
        }

        public ActionResult GenerateCPSPmonthlyProgressReport_New(string Qtype, string OfficeId, string DateTo, string ZoneID, string AreaID, string BranchID, string EmployeeID)
        {
            try
            {

                var reportParam = new Dictionary<string, object>();
                //reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("FromDate", from_date);

                DateTime dt = Convert.ToDateTime(DateTo);
                int Year = dt.Year;
                int Month = dt.Month;

                if (EmployeeID == "")
                    EmployeeID = "0";


                if (ZoneID == "null")
                    ZoneID = "0";


                if (AreaID == "null")
                    AreaID = "0";


                if (BranchID == "null")
                    BranchID = "0";


                var param1 =
                    new
                    {
                        ReportingYear = Year,
                        ReportingMonth = Month,
                        OrgID = Convert.ToInt32(LoggedInOrganizationID),
                        ZoneID = Convert.ToInt32(ZoneID),
                        AreaID = Convert.ToInt32(AreaID),
                        BranchID = Convert.ToInt32(BranchID),
                        EmployeeID = Convert.ToInt32(EmployeeID),
                        QType = Convert.ToInt32(Qtype)
                    };

                var alldata1 = groupwiseReportService.GetDataProcessLOg(param1, "Proc_Get_MonthlyCPSPProgressReportRowWiseShow");
                if (alldata1.Tables.Count > 0)
                {
                    if (Qtype == "0")
                        ReportHelper.PrintReport("rptMonthlyCPSPProgressReportRowWiseOrg.rpt", alldata1.Tables[0], reportParam);
                    if (Qtype == "1")
                        ReportHelper.PrintReport("rptMonthlyCPSPProgressReportRowWiseZone.rpt", alldata1.Tables[0], reportParam);
                    if (Qtype == "2")
                        ReportHelper.PrintReport("rptMonthlyCPSPProgressReportRowWiseArea.rpt", alldata1.Tables[0], reportParam);
                    if (Qtype == "3")
                        ReportHelper.PrintReport("rptMonthlyCPSPProgressReportRowWiseBranch.rpt", alldata1.Tables[0], reportParam);
                    if (Qtype == "4")
                        ReportHelper.PrintReport("rptMonthlyCPSPProgressReportRowWiseStaff.rpt", alldata1.Tables[0], reportParam);

                    return Content(string.Empty);
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "No Data Found!" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }
        }


        public ActionResult GetOrg_CPSP()
        {
            var param = new { OrgId = LoggedInOrganizationID };
            var EmployeeList = ultimateReportService.GetDataWithParameter(param, "SP_ORG_SEARCH_AUTOCMPLETE");
            List<Employee> employees = new List<Employee>();
            employees = EmployeeList.Tables[0].AsEnumerable()
                   .Select(row => new Employee
                   {
                       OrgID = row.Field<Int32>("OrgID"),
                       EmployeeCode = row.Field<string>("OrganizationName")
                   }).ToList();
            return Json(employees, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GenerateTotalRowCountVoiceData()
        {
            try
            {
                var result_set = ultimateReportService.GetDataWithoutParameter("Proc_VoiceDataRowCount");
                var totalRow = result_set.Tables[0].Rows[0]["TotalRow"].ToString();
                var successRow = result_set.Tables[0].Rows[0]["SuccessRow"].ToString();
                var data = new
                {
                    TotalRow = totalRow,
                    SuccessRow = successRow
                };
                return Json(new { Data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult VoiceDataProcess()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["TrxTypeList"] = items;

            var trnDate = SessionHelper.TransactionDate.ToString("dd-MMM-yyyy");

            var model = new VoiceDataProcessViewModel
            {
                DateValue = trnDate,
                OfficeList = new List<SelectListItem> { new SelectListItem { Text = "Nothing Seleted" } },
                TrxTypeList = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Select All", Value = "0", Selected = true },
                    new SelectListItem { Text = "Regular", Value = "10"},
                    new SelectListItem { Text = "Special", Value = "20" },
                    new SelectListItem { Text = "Partial", Value = "9999"}
                }
            };
            return View(model);
        }
        public ActionResult WelfareFundRegister()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            return View();
        }
        public ActionResult GenerateWelfareFundRegisterReport(string Center, string DateFromValue, string DateToValue)
        {
            try
            {
                var param = new { @OfficeID = SessionHelper.LoginUserOfficeID, @CenterID = Center, @DateFrom = DateFromValue, @DateTo = DateToValue };
                var alldata = ultimateReportService.GetDataWithParameter(param, "Proc_GetMemberFundRegisterReport");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("FromDate", DateFromValue);
                reportParam.Add("ToDate", DateToValue);
                ReportHelper.PrintReport("MemberWelfareFundRegister.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateVoiceDataProcess(VoiceDataProcessViewModel model)
        {
            try
            {
                var newOfficeIDs = String.Join("_", model.OfficeIds);
                var param = new { @OfficeIDs = newOfficeIDs, @TrxType = model.TrxType, @DateValue = model.DateValue };
                ultimateReportService.RunVoiceDataProcess(param, "Proc_RunVoiceDataProcess");

                return Json(new { Message = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult LoanDisbursedProductWise()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult GenerateLoanDisbursedProductWiseReport(string from_date)
        {
            try
            {
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("FromDate", from_date);

                var param1 = new { todate = from_date, OfficeId = SessionHelper.LoginUserOfficeID };
                var alldata1 = groupwiseReportService.GetLoanDisbursedProductWiseReport(param1, "Proc_LoanDisbursedProductWiseReport");
                ReportHelper.PrintReport("rptLoanDisbursedProductWiseReport.rpt", alldata1.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }
        }
        //Monthly Loan CollectionSheet
        public ActionResult MonthlyLoanCollectionSheet()
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

        public ActionResult GenerateMonthlyLoanCollectionSheetReport(string CollectionDate)
        {
            try
            {

                DateTime CollectionMonth = DateTime.ParseExact(CollectionDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                var CollectionMY = CollectionMonth.ToString("MMM/yyyy");

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CollectionDate = CollectionDate, OrgID = SessionHelper.LoginUserOrganizationID };
                var spData = ultimateReportService.GetDataWithParameter(param, "Proc_GetMonthlyLoanCollectionSheet");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", CollectionMY);
                //reportParam.Add("CDay", ColDay);
                ReportHelper.PrintReport("rptMonthlyLoanCollectionSheet.rpt", spData.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult MonthlyProgressReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult GenerateMonthlyProgressReport(string from_date)
        {
            try
            {
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("FromDate", from_date);

                var param1 = new { todate = from_date, OfficeId = SessionHelper.LoginUserOfficeID };

                var alldata1 = groupwiseReportService.GetMonthlyProgressReport(param1, "Proc_MonthlyProgressReport");
                ReportHelper.PrintReport("rptMonthlyProgressReport.rpt", alldata1.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }
        }
        public ActionResult MonthlyProgressLoanReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult GenerateMonthlyProgressLoanReport(string from_date)
        {
            try
            {
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("FromDate", from_date);


                var param1 = new { todate = from_date, OfficeId = SessionHelper.LoginUserOfficeID };
                var alldata1 = groupwiseReportService.GetMonthlyProgressLoanReport(param1, "Proc_MonthlyProgressLoanReport");
                ReportHelper.PrintReport("rptMonthlyProgressLoanReport.rpt", alldata1.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }
        }
        public ActionResult MonthlyBenifit()
        {
            MonthlyBenifitViewModel model = new MonthlyBenifitViewModel();
            MapDropDownForMothlybenifit(model);
            return View(model);
        }

        //public ActionResult GenerateMonthlybenifitReport(string dateFrom, string dateTo, int productId)
        //{
        //    var param = new { OfficeID = SessionHelper.LoginUserOfficeID, FromDate = dateFrom, ToDate = dateTo, ProductID = productId };
        //    var alldata = ultimateReportService.GetDataWithParameter(param, "Proc_GetMonthlyBenifit");

        //    var reportParam = new Dictionary<string, object>();
        //    reportParam.Add("OrgName", SessionHelper.OrganizationName);
        //    reportParam.Add("DateFrom", dateFrom);
        //    reportParam.Add("DateTo", dateTo);
        //    ReportHelper.PrintReport("MonthlyBenifitReport.rpt", alldata.Tables[0], reportParam);
        //    return Content(string.Empty);
        //}
        public ActionResult GenerateMonthlybenifitReport(string dateFrom, string dateTo, List<string> productIds)
        {
            string productIdCombine = String.Join("_", productIds);
            var param = new { OfficeID = SessionHelper.LoginUserOfficeID, FromDate = dateFrom, ToDate = dateTo, ProductID = productIdCombine };
            var alldata = ultimateReportService.GetDataWithParameter(param, "Proc_GetMonthlyBenifit");

            var reportParam = new Dictionary<string, object>();
            reportParam.Add("OrgName", SessionHelper.OrganizationName);
            reportParam.Add("DateFrom", dateFrom);
            reportParam.Add("DateTo", dateTo);
            ReportHelper.PrintReport("MonthlyBenifitReport.rpt", alldata.Tables[0], reportParam);
            return Content(string.Empty);
        }
        private void MapDropDownForMothlybenifit(MonthlyBenifitViewModel model)
        {
            var param = new { OfficeID = SessionHelper.LoginUserOfficeID, OrgID = LoggedInOrganizationID };
            var alldata = ultimateReportService.GetDataWithParameter(param, "Proc_GetIndividualProduct");

            var listItems = alldata.Tables[0].AsEnumerable()
                .Select(x => new SelectListItem
                {
                    Value = x.Field<Int16>("ProductID").ToString(),
                    Text = x.Field<string>("ProductCode") + '-' + x.Field<string>("ProductName")
                });

            var List = new List<SelectListItem>();
            List.Add(new SelectListItem() { Text = "Select One", Value = "0" });
            List.AddRange(listItems);
            model.IndividualProductList = List;
        }
        public ActionResult MonthlyJCFReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            return View();
        }
        public ActionResult GenerateMonthlyJCFReport(string from_date)
        {
            try
            {
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("FromDate", from_date);

                var param1 = new { todate = from_date, OfficeId = SessionHelper.LoginUserOfficeID };
                var alldata1 = groupwiseReportService.GetrptMonthlyJCFReport(param1, "Proc_rptMonthlyJCFReport");
                ReportHelper.PrintReport("rptMonthlyJCFReport.rpt", alldata1.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }
        }
        // Week No Declaration Start
        private class YearList
        {
            public int YearCode { get; set; }
            public string YearName { get; set; }
        }
        public JsonResult GetYearList()
        {
            var dt = new DataTable();

            dt.Columns.Add("text", typeof(string));
            dt.Columns.Add("value", typeof(string));

            int cur_yr = 2019;

            for (int i = 0; i <= 21; i++)
            {
                dt.Rows.Add((cur_yr + i).ToString(), (cur_yr + i).ToString());
            }

            List<YearList> YearList = new List<YearList>();

            //DataTable dt = dc.GetYearList();

            YearList blnkYearList = new YearList()
            {
                YearCode = 0,
                YearName = "Please Select"
            };
            YearList.Add(blnkYearList);

            foreach (DataRow row in dt.Rows)
            {

                YearList oYearList = new YearList()
                {
                    YearCode = Convert.ToInt32(row["value"]),
                    YearName = row["text"].ToString()
                };

                YearList.Add(oYearList);
            }

            IEnumerable<SelectListItem> items = new SelectList(YearList, "YearCode", "YearName");
            return Json(items.ToList(), JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetYearList()
        //{
        //    var dt = new DataTable();

        //    dt.Columns.Add("text", typeof(string));
        //    dt.Columns.Add("value", typeof(string));

        //    int cur_yr = DateTime.Now.Year;

        //    for (int i = 0; i <= 20; i++)
        //    {
        //        dt.Rows.Add((cur_yr - i).ToString(), (cur_yr - i).ToString());
        //    }

        //    List<YearList> YearList = new List<YearList>();

        //    //DataTable dt = dc.GetYearList();

        //    YearList blnkYearList = new YearList()
        //    {
        //        YearCode = 0,
        //        YearName = "Please Select"
        //    };
        //    YearList.Add(blnkYearList);

        //    foreach (DataRow row in dt.Rows)
        //    {

        //        YearList oYearList = new YearList()
        //        {
        //            YearCode = Convert.ToInt32(row["value"]),
        //            YearName = row["text"].ToString()
        //        };

        //        YearList.Add(oYearList);
        //    }

        //    IEnumerable<SelectListItem> items = new SelectList(YearList, "YearCode", "YearName");
        //    return Json(items.ToList(), JsonRequestBehavior.AllowGet);
        //}
        public ActionResult WeekNoDeclaration()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["YearList"] = items;
            return View();
        }
        public ActionResult GenerateWeekNoDeclaration(string startDate, string year)
        {
            try
            {
                var reportParam = new Dictionary<string, object>();
                int OfficeId = Convert.ToInt32(LoginUserOfficeID);
                var param1 = new { mStartDate = startDate, mYear = year };
                var alldata = groupwiseReportService.GetWeekNoDeclaration(param1, "WeekNoDeclaration");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult WeeklyDataProcess()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["YearList"] = items;
            ViewData["WeekList"] = items;
            return View();
        }
        public ActionResult GenerateWeeklyDataProcess(string year, string WeekNo, string StartDate, string EndDate)
        {
            try
            {
                int yr = Convert.ToInt32(year);
                int wk = Convert.ToInt32(WeekNo);
                var param1 = new { year = yr, WeekNo = wk, StartDate = StartDate, EndDate = EndDate };
                var alldata = groupwiseReportService.GetWeeklyDataProcess(param1, "GenerateWeekData");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult GetYearWiseWeekNoList(string year, int? WeekNoID)
        {
            try
            {
                int yer = Convert.ToInt32(year);
                var param1 = new { year = yer, WeekNoID = WeekNoID };
                var alldata = groupwiseReportService.GetYearWiseWeekNoList(param1, "GetYearWiseWeekNoList");
                List<WeeNoViewModel> List_WeeNo = new List<WeeNoViewModel>();
                List_WeeNo = alldata.Tables[0].AsEnumerable()
                    .Select(row => new WeeNoViewModel
                    {
                        WeekYear = row.Field<int>("WeekYear"),
                        WeekNoID = row.Field<long>("WeekNoID"),
                        WeekNoSl = row.Field<int>("WeekNoSl"),
                        SD = row.Field<string>("SD"),
                        ED = row.Field<string>("ED"),
                    }).ToList();
                return Json(List_WeeNo, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        // Week No Declaration End
        public ActionResult ActiveAccount()
        {
            ViewData["OfficeID"] = SessionHelper.LoginUserOfficeID;
            var model = new ActiveAccountViewModel();
            return View(model);
        }
        //public ActionResult SearchDivision()
        //{

        //}
        public ActionResult MonthlyProjectStatementProshikaNewReport()
        {
            int orgid = (int)LoggedInOrganizationID;
            if (orgid == 99)
            //if (orgid == 44)
            {
                IEnumerable<SelectListItem> items = new SelectList(" ");
                ViewData["HOList"] = items;
                ViewData["ZoneList"] = items;
                ViewData["AreaList"] = items;
                ViewData["OfficeList"] = items;
                return View();
            }
            else
            {
                return View("sorry, you do not have permission to access this report");
            }
        }
        public ActionResult GenerateMonthlyProjectStatementProshikaNewLastReport(string OfficeId, string DateFrom, string DateTo)
        {
            try
            {
                var OfficeCode = string.Empty;
                var OfficeName = string.Empty;
                DateTime DateToMonthint = Convert.ToDateTime(DateTo);
                var DateToMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateToMonthint.Month);
                if (OfficeId != null || OfficeId != "")
                {
                    int officeidint = Convert.ToInt32(OfficeId);
                    OfficeCode = officeService.GetById(officeidint).OfficeCode;
                    OfficeName = officeService.GetById(officeidint).OfficeName;
                };
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("param_officeCode", OfficeCode);
                reportParam.Add("param_officeName", OfficeName);
                reportParam.Add("param_dateFrom", DateToMonthName);
                reportParam.Add("param_dateTo", DateTo);

                if (OfficeId == "1") { OfficeId = "0"; }
                var param1 = new { OfficeId = OfficeId, DateFrom = DateFrom, DateTo = DateTo };
                //var alldata1 = groupwiseReportService.GetProgramMISReportJCF(param1, "Proc_RPT_MIS_LoanReportZoneWise");
                var alldata1 = groupwiseReportService.GetProgramMISReportJCF(param1, "MonthlyStatementItem_GetMonthlyProjectStatement_Process");
                ReportHelper.PrintReport("RPT_MonthlyProjectStatementProshikaNewReportLatest.rpt", alldata1.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }
        }
        public JsonResult loadActiveAccount([DataSourceRequest] Kendo.Mvc.UI.DataSourceRequest request, int OfficeID, DateTime DateFrom, DateTime DateTo, string Type)
        {
            try
            {
                var param = new { OfficeID = OfficeID, DateFrom = DateFrom, DateTo = DateTo, Type = Type };
                var officewiseDivisionList = groupwiseReportService.GetActiveAccount(param, "dbo.getActiveAccountNo");
                var officewiseDivisionListViewModel = officewiseDivisionList.Tables[0].AsEnumerable()
                    .Select(row => new ActiveAccountViewModel
                    {
                        OfficeId = row.Field<int>("OfficeID"),
                        LoanAccountNo = row.Field<string>("LoanAccountNo"),
                        MemberCode = row.Field<string>("MemberCode"),
                        MemberName = row.Field<string>("MemberName"),
                        OfficeCode = row.Field<string>("OfficeCode"),
                        SamityCode = row.Field<string>("SamityCode"),

                    }).ToList();

                DataSourceResult result = officewiseDivisionListViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", Message = ex.Message });
            }

        }
        public JsonResult GetBranchOfficeList(string HO_val, string zone_val, string area_val)
        {
            var ho_code = officeService.GetById(Convert.ToInt32(HO_val)).OfficeCode;
            var zone_code = officeService.GetById(Convert.ToInt32(zone_val)).OfficeCode;
            var area_code = officeService.GetById(Convert.ToInt32(area_val)).OfficeCode;
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 4 && c.FirstLevel == ho_code && c.SecondLevel == zone_code && c.ThirdLevel == area_code && c.OrgID == Convert.ToInt32(LoggedInOrganizationID));
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            //if (viewOffice.ToList().Count > 0)
            //{
            //    office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //}
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAreaList(string HO_val, string zone_val)
        {
            var ho_code = officeService.GetById(Convert.ToInt32(HO_val)).OfficeCode;
            var zone_code = officeService.GetById(Convert.ToInt32(zone_val)).OfficeCode;
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 3 && c.FirstLevel == ho_code && c.SecondLevel == zone_code && c.OrgID == Convert.ToInt32(LoggedInOrganizationID));
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();

            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetZoneList(string HO_val)
        {
            var parentCode = officeService.GetById(Convert.ToInt32(HO_val)).OfficeCode;
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 2 && c.FirstLevel == parentCode && c.OrgID == Convert.ToInt32(LoggedInOrganizationID));
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            //if (viewOffice.ToList().Count > 0)
            //{
            //    office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //}
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetHOList()
        {
            // var First_Level = officeService.GetByOfficeOrgID(Convert.ToInt32(SessionHelper.LoginUserOfficeID), Convert.ToInt32(LoggedInOrganizationID));
            var OfficeList = officeService.GetAll().Where(c => c.OfficeLevel == 1);
            var viewOffice = OfficeList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + " " + x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            //if (viewOffice.ToList().Count > 0)
            //{
            //    office_items.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            //}
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductListByMember(long memberId)
        {

            var pram = new
            {
                OfficeID = SessionHelper.LoginUserOfficeID,
                MemberID = memberId
            };
            var ProductList = new List<SelectListItem>();
            var prod_items = new List<SelectListItem>();
            var getProduct = groupwiseReportService.GetProductListByMemberList(pram, "GetProductListByMemberList");
            if (getProduct.Tables[0].Rows.Count > 0)
            {
                ProductList = getProduct.Tables[0].AsEnumerable().Select((row, index) => new SelectListItem()
                {
                    Value = row.Field<short>("ProductID").ToString(),
                    Text = productService.GetById(row.Field<short>("ProductID")).ProductCode + ", " + productService.GetById(row.Field<short>("ProductID")).ProductName
                }).ToList();
                prod_items = new List<SelectListItem>();
                prod_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
                prod_items.AddRange(ProductList);
            }
            return Json(prod_items, JsonRequestBehavior.AllowGet);


            //var getProduct = loanSummaryService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.OrgID == LoggedInOrganizationID && s.MemberID == memberId);
            //var viewProduct = getProduct.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.ProductID.ToString(),
            //    Text = productService.GetById(x.ProductID).ProductCode + ", " + productService.GetById(x.ProductID).ProductName
            //});
            //var prod_items = new List<SelectListItem>();
            //if (viewProduct.ToList().Count > 0)
            //{
            //    prod_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            //}
            //prod_items.AddRange(viewProduct);
            //return Json(prod_items, JsonRequestBehavior.AllowGet);


            //var getProduct = loanSummaryService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.OrgID == LoggedInOrganizationID && s.MemberID == memberId);
            //var viewProduct = getProduct.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.ProductID.ToString(),
            //    Text = productService.GetById(x.ProductID).ProductCode + ", " + productService.GetById(x.ProductID).ProductName
            //});
            //var prod_items = new List<SelectListItem>();
            //if (viewProduct.ToList().Count > 0)
            //{
            //    prod_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            //}
            //prod_items.AddRange(viewProduct);
            //return Json(prod_items, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetProductListByMember(long memberId)
        //{

        //    var getProduct = loanSummaryService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.OrgID == LoggedInOrganizationID && s.MemberID == memberId);
        //    var viewProduct = getProduct.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.ProductID.ToString(),
        //        Text = productService.GetById(x.ProductID).ProductCode + ", " + productService.GetById(x.ProductID).ProductName
        //    });
        //    var prod_items = new List<SelectListItem>();
        //    if (viewProduct.ToList().Count > 0)
        //    {
        //        prod_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
        //    }
        //    prod_items.AddRange(viewProduct);
        //    return Json(prod_items, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult GetLoanTermList(long memberId, int productId)
        {
            var pram = new
            {
                OfficeID = SessionHelper.LoginUserOfficeID,
                //OfficeID = 4,
                MemberID = memberId,
                ProductID = productId
            };
            var getLoanTermList = new List<SelectListItem>();
            var getLoanTerm_items = new List<SelectListItem>();
            var getLoanTerm = groupwiseReportService.GetLoanTermList(pram, "GetLoanTermList");  //GetProductListByMemberList
            if (getLoanTerm.Tables[0].Rows.Count > 0)
            {
                getLoanTermList = getLoanTerm.Tables[0].AsEnumerable().Select((row, index) => new SelectListItem()
                {
                    Value = row.Field<byte>("LoanTerm").ToString(),
                    Text = row.Field<byte>("LoanTerm").ToString()
                }).ToList();
                getLoanTerm_items = new List<SelectListItem>();
                getLoanTerm_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
                getLoanTerm_items.AddRange(getLoanTermList);
            }
            return Json(getLoanTerm_items, JsonRequestBehavior.AllowGet);

            //var getLoanTerm = loanSummaryService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.OrgID == LoggedInOrganizationID && s.MemberID == memberId && s.ProductID == productId).OrderBy(e => e.LoanTerm);
            //var viewLoanTerm = getLoanTerm.Select(x => x).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.LoanTerm.ToString(),
            //    Text = x.LoanTerm.ToString()
            //});
            //var loanterm_items = new List<SelectListItem>();
            //if (viewLoanTerm.ToList().Count > 0)
            //{
            //    loanterm_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            //}
            //loanterm_items.AddRange(viewLoanTerm);
            //return Json(loanterm_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNoOfAccountList(long memberId, int productId)
        {
            var getLoanTerm = loanSummaryService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.OrgID == LoggedInOrganizationID && s.MemberID == memberId && s.ProductID == productId).OrderBy(e => e.LoanTerm);
            var viewLoanTerm = getLoanTerm.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.LoanTerm.ToString(),
                //Text = x.MemberCode.ToString() + ", " + x.MiddleName.ToString()
                //Text = x.MemberCode.ToString() + ", " + x.FirstName.ToString() + " " + x.MiddleName.ToString() + " " + x.LastName.ToString()
                Text = x.LoanTerm.ToString()
                //Text = x.CenterName.ToString()
            });
            var loanterm_items = new List<SelectListItem>();
            if (viewLoanTerm.ToList().Count > 0)
            {
                loanterm_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            }
            loanterm_items.AddRange(viewLoanTerm);
            return Json(loanterm_items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GenerateRecoveryStatementReport(string from_date, string to_date, string emp, string prod, string center, string offc, string qType)
        {
            try
            {
                //'84','84','00','99999999','00','99999999','00','99999999','01 Jul 2015','31 Jul 2015',3
                string CenterID = string.Empty; string CenterIDTo = string.Empty; string StaffID = string.Empty;
                string StaffIDTo = string.Empty; string productID = string.Empty; string ProductIDTo = string.Empty;


                var paramSe = new
                {
                    OfficeID = LoginUserOfficeID,
                    OfficeIDTO = LoginUserOfficeID,
                    CenterID = CenterID,
                    CenterIDTo = CenterIDTo,
                    StaffID = StaffID,
                    StaffIDTo = StaffIDTo,
                    productID = productID,
                    ProductIDTo = ProductIDTo,
                    DateFrom = from_date,
                    DateTo = to_date,
                    Qtype = 2
                };

                var alldataData = groupwiseReportService.GetDataRecoveryStatement(paramSe, "RecoverableRecoveryRegister");

                if (qType == "3") // Cneter wise
                {
                    if (Convert.ToInt32(center) != 0)
                    {
                        CenterID = center;
                        CenterIDTo = center;
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                else if (qType == "4") // Staff wise
                {
                    if (Convert.ToInt32(emp) != 0)
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = emp;
                        StaffIDTo = emp;
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                else if (qType == "5") // prod wise
                {
                    if (Convert.ToInt32(prod) != 0)
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = prod;
                        ProductIDTo = prod;
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                else if (qType == "6") // prod wise
                {
                    if (Convert.ToInt32(prod) != 0)
                    {
                        //OfficeID = LoggedInOrganizationID;
                        //OfficeIDTO = LoginUserOfficeID;

                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                var param = new
                {
                    OfficeID = LoginUserOfficeID,
                    OfficeIDTO = LoginUserOfficeID,
                    CenterID = CenterID,
                    CenterIDTo = CenterIDTo,
                    StaffID = StaffID,
                    StaffIDTo = StaffIDTo,
                    productID = productID,
                    ProductIDTo = ProductIDTo,
                    DateFrom = from_date,
                    DateTo = to_date,
                    Qtype = qType
                };

                //var param = new
                //{
                //    OfficeID = 84,
                //    OfficeIDTO = 84,
                //    CenterID = "00",
                //    CenterIDTo = "99999999",
                //    StaffID = "00",
                //    StaffIDTo = "99999999",
                //    productID = "00",
                //    ProductIDTo = "99999999",
                //    DateFrom = "01 Jul 2015",
                //    DateTo = "31 Jul 2015",
                //    Qtype = 3
                //};
                var alldata = groupwiseReportService.GetDataRecoveryStatement(param, "RecoverableRecoveryRegister");

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", from_date);
                reportParam.Add("DateTo", to_date);
                if (qType == "7")
                {
                    ReportHelper.PrintReport("RecoverableAndRecoveryRegisterLoaneeWise.rpt", alldata.Tables[0], reportParam);
                }
                else
                    ReportHelper.PrintReport("RecovarableAndRecoveryRegisterReport.rpt", alldata.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateOvedueAgeingReport(string from_date, string to_date, string emp, string prod, string center, string offc, string qType)
        {
            try
            {
                string CenterID = string.Empty; string CenterIDTo = string.Empty; string StaffID = string.Empty;
                string StaffIDTo = string.Empty; string productID = string.Empty; string ProductIDTo = string.Empty;



                var param = new
                {
                    OfficeID = LoginUserOfficeID,
                    OfficeIDTO = LoginUserOfficeID,
                    CenterID = 00000,
                    CenterIDTo = 99999,
                    StaffID = 00000,
                    StaffIDTo = 99999,
                    productID = 00000,
                    ProductIDTo = 99999,
                    DateFrom = from_date,
                    DateTo = to_date,
                    OrgID_PO = LoggedInOrganizationID,
                };


                DataSet alldata;
                var reportParam = new Dictionary<string, object>();
                if (qType == "2")
                {
                    alldata = groupwiseReportService.GetOverDueAgeing(param, "getOverDueAgeingReport");
                    reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateFrom", from_date);
                    reportParam.Add("DateTo", to_date);
                    ReportHelper.PrintReport("OverDueAgeing.rpt", alldata.Tables[0], reportParam);
                }
                else if (qType == "3")
                {
                    alldata = groupwiseReportService.GetOverDueAgeing(param, "getOverDueAgeingCenterWiseReport");
                    reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateFrom", from_date);
                    reportParam.Add("DateTo", to_date);
                    ReportHelper.PrintReport("OverDueAgeing.rpt", alldata.Tables[0], reportParam);
                }
                else if (qType == "4")
                {
                    alldata = groupwiseReportService.GetOverDueAgeing(param, "getOverDueAgeingStaffWiseReport");
                    reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateFrom", from_date);
                    reportParam.Add("DateTo", to_date);
                    ReportHelper.PrintReport("OverDueAgeing.rpt", alldata.Tables[0], reportParam);
                }

                else if (qType == "6")
                {
                    alldata = groupwiseReportService.GetOverDueAgeing(param, "getOverDueAgeingMemberWiseReport");
                    reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateFrom", from_date);
                    reportParam.Add("DateTo", to_date);
                    ReportHelper.PrintReport("OverDueAgeingLoneeWise.rpt", alldata.Tables[0], reportParam);

                }
                //else
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateOvedueAgeingReport_DSK(string from_date, string to_date, string emp, string prod, string center, string offc, string qType)
        {
            try
            {
                string CenterID = string.Empty; string CenterIDTo = string.Empty; string StaffID = string.Empty;
                string StaffIDTo = string.Empty; string productID = string.Empty; string ProductIDTo = string.Empty;


                var param = new
                {
                    OfficeID = LoginUserOfficeID,
                    OfficeIDTO = LoginUserOfficeID,
                    CenterID = 00000,
                    CenterIDTo = 99999,
                    StaffID = 00000,
                    StaffIDTo = 99999,
                    productID = 00000,
                    ProductIDTo = 99999,
                    DateFrom = from_date,
                    DateTo = to_date,
                    OrgID_PO = LoggedInOrganizationID,
                };


                DataSet alldata;
                var reportParam = new Dictionary<string, object>();
                if (qType == "2")
                {
                    alldata = groupwiseReportService.GetOverDueAgeing(param, "getOverDueAgeingReport");
                    reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateFrom", from_date);
                    reportParam.Add("DateTo", to_date);
                    ReportHelper.PrintReport("OverDueAgeing_DSK.rpt", alldata.Tables[0], reportParam);
                }
                else if (qType == "3")
                {
                    alldata = groupwiseReportService.GetOverDueAgeing(param, "getOverDueAgeingCenterWiseReport");
                    reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateFrom", from_date);
                    reportParam.Add("DateTo", to_date);
                    ReportHelper.PrintReport("OverDueAgeing_DSK.rpt", alldata.Tables[0], reportParam);
                }
                else if (qType == "4")
                {
                    alldata = groupwiseReportService.GetOverDueAgeing(param, "getOverDueAgeingStaffWiseReport");
                    reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateFrom", from_date);
                    reportParam.Add("DateTo", to_date);
                    ReportHelper.PrintReport("OverDueAgeing_DSK.rpt", alldata.Tables[0], reportParam);

                }


                else if (qType == "6")
                {
                    alldata = groupwiseReportService.GetOverDueAgeing(param, "getOverDueAgeingMemberWiseReport");
                    reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateFrom", from_date);
                    reportParam.Add("DateTo", to_date);
                    ReportHelper.PrintReport("OverDueAgeingLoneeWise.rpt", alldata.Tables[0], reportParam);

                }
                //else
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateOvedueAgeingReport_JCF_OverDueType(string from_date, string to_date, string emp, string prod, string center, string offc, string qType, string due)
        {
            try
            {
                //'84','84','00','99999999','00','99999999','00','99999999','01 Jul 2015','31 Jul 2015',3
                string CenterID = string.Empty; string CenterIDTo = string.Empty; string StaffID = string.Empty;
                string StaffIDTo = string.Empty; string productID = string.Empty; string ProductIDTo = string.Empty;
                string LoanType = string.Empty;


                var paramSe = new
                {
                    OrgID = LoggedInOrganizationID,
                    OfficeID = LoginUserOfficeID,
                    OfficeIDTO = LoginUserOfficeID,
                    CenterID = 00000,
                    CenterIDTo = 99999,
                    StaffID = 00000,
                    StaffIDTo = 99999,
                    productID = 00000,
                    ProductIDTo = 99999,
                    DateFrom = from_date,
                    DateTo = to_date,
                    Qtype = 1,
                    LoanType = ""
                };

                var alldataData = groupwiseReportService.GetOverDueAgeing(paramSe, "OverdueClassificationConsolidationDueDateWise");

                if (qType == "3") // Cneter wise
                {
                    if (Convert.ToInt32(center) != 0)
                    {
                        CenterID = center;
                        CenterIDTo = center;
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                else if (qType == "4") // Staff wise
                {
                    if (Convert.ToInt32(emp) != 0)
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = emp;
                        StaffIDTo = emp;
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                else if (qType == "2") // prod wise
                {
                    if (Convert.ToInt32(prod) != 0)
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = prod;
                        ProductIDTo = prod;
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                else if (qType == "5") // prod wise
                {


                    if (Convert.ToInt32(prod) != 0)
                    {
                        //OfficeID = LoggedInOrganizationID;
                        //OfficeIDTO = LoginUserOfficeID;
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                else if (qType == "6") // prod wise
                {
                    if (Convert.ToInt32(prod) != 0)
                    {
                        //OfficeID = LoggedInOrganizationID;
                        //OfficeIDTO = LoginUserOfficeID;
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }

                else if (qType == "7" || qType == "8") // Due type wise
                {
                    if (Convert.ToInt32(due) != 0)
                    {
                        //OfficeID = LoggedInOrganizationID;
                        //OfficeIDTO = LoginUserOfficeID;
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                        LoanType = due;
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                        LoanType = "";
                    }
                }

                var param = new
                {
                    OrgID = LoggedInOrganizationID,
                    OfficeID = LoginUserOfficeID,
                    OfficeIDTO = LoginUserOfficeID,
                    CenterID = CenterID,
                    CenterIDTo = CenterIDTo,
                    StaffID = StaffID,
                    StaffIDTo = StaffIDTo,
                    productID = productID,
                    ProductIDTo = ProductIDTo,
                    DateFrom = from_date,
                    DateTo = to_date,
                    Qtype = qType,
                    loanType = LoanType
                };

                var alldata = groupwiseReportService.GetOverDueAgeing(param, "OverdueClassificationConsolidationDueDateWise");

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", from_date);
                reportParam.Add("DateTo", to_date);
                if (qType == "7" || qType == "8")
                {
                    ReportHelper.PrintReport("OverDueAgeingLoneeWise_JCF_OverDueType.rpt", alldata.Tables[0], reportParam);
                }
                else if (qType == "6")
                {
                    ReportHelper.PrintReport("OverDueAgeingLoneeWise.rpt", alldata.Tables[0], reportParam);
                }
                else
                    ReportHelper.PrintReport("OverDueAgeingWithNoOfMembers.rpt", alldata.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateOvedueAgeingReport_NoOFBorrowers(string from_date, string to_date, string emp, string prod, string center, string offc, string qType)
        {
            try
            {
                //'84','84','00','99999999','00','99999999','00','99999999','01 Jul 2015','31 Jul 2015',3
                string CenterID = string.Empty; string CenterIDTo = string.Empty; string StaffID = string.Empty;
                string StaffIDTo = string.Empty; string productID = string.Empty; string ProductIDTo = string.Empty;
                string LoanType = string.Empty;
                string due = "0";

                var paramSe = new
                {
                    OrgID = LoggedInOrganizationID,
                    OfficeID = LoginUserOfficeID,
                    OfficeIDTO = LoginUserOfficeID,
                    CenterID = 00000,
                    CenterIDTo = 99999,
                    StaffID = 00000,
                    StaffIDTo = 99999,
                    productID = 00000,
                    ProductIDTo = 99999,
                    DateFrom = from_date,
                    DateTo = to_date,
                    Qtype = 1,
                    LoanType = ""
                };

                var alldataData = groupwiseReportService.GetOverDueAgeing(paramSe, "OverdueClassificationDisbursedate");

                if (qType == "3") // Cneter wise
                {
                    if (Convert.ToInt32(center) != 0)
                    {
                        CenterID = center;
                        CenterIDTo = center;
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                else if (qType == "4") // Staff wise
                {
                    if (Convert.ToInt32(emp) != 0)
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = emp;
                        StaffIDTo = emp;
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                else if (qType == "2") // prod wise
                {
                    if (Convert.ToInt32(prod) != 0)
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = prod;
                        ProductIDTo = prod;
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                else if (qType == "5") // prod wise
                {


                    if (Convert.ToInt32(prod) != 0)
                    {
                        //OfficeID = LoggedInOrganizationID;
                        //OfficeIDTO = LoginUserOfficeID;
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                else if (qType == "6") // prod wise
                {
                    if (Convert.ToInt32(prod) != 0)
                    {
                        //OfficeID = LoggedInOrganizationID;
                        //OfficeIDTO = LoginUserOfficeID;
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }

                else if (qType == "7" || qType == "8") // Due type wise
                {
                    if (Convert.ToInt32(due) != 0)
                    {
                        //OfficeID = LoggedInOrganizationID;
                        //OfficeIDTO = LoginUserOfficeID;
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                        LoanType = due;
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                        LoanType = "";
                    }
                }



                var param = new
                {
                    OrgID = LoggedInOrganizationID,
                    OfficeID = LoginUserOfficeID,
                    OfficeIDTO = LoginUserOfficeID,
                    CenterID = CenterID,
                    CenterIDTo = CenterIDTo,
                    StaffID = StaffID,
                    StaffIDTo = StaffIDTo,
                    productID = productID,
                    ProductIDTo = ProductIDTo,
                    DateFrom = from_date,
                    DateTo = to_date,
                    Qtype = qType,
                    loanType = LoanType
                };

                var alldata = groupwiseReportService.GetOverDueAgeing(param, "OverdueClassificationDisbursedate");

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", from_date);
                reportParam.Add("DateTo", to_date);
                if (qType == "7" || qType == "8")
                {
                    ReportHelper.PrintReport("OverDueAgeingWithNoOfBrowerss.rpt", alldata.Tables[0], reportParam);
                }
                else if (qType == "6")
                {
                    ReportHelper.PrintReport("OverDueAgeingWithNoOfBrowerss.rpt", alldata.Tables[0], reportParam);
                }
                else
                    ReportHelper.PrintReport("OverDueAgeingWithNoOfBrowerss.rpt", alldata.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateProvisionCalculationReport(string from_date, string to_date, string emp, string prod, string center, string offc, string qType)
        {
            try
            {
                //'84','84','00','99999999','00','99999999','00','99999999','01 Jul 2015','31 Jul 2015',3
                string CenterID = string.Empty; string CenterIDTo = string.Empty; string StaffID = string.Empty;
                string StaffIDTo = string.Empty; string productID = string.Empty; string ProductIDTo = string.Empty;

                //var paramSe = new
                //{
                //    OfficeID = LoginUserOfficeID,
                //    OfficeIDTO = LoginUserOfficeID,
                //    CenterID = 00000,
                //    CenterIDTo = 99999,
                //    StaffID = 00000,
                //    StaffIDTo = 99999,
                //    productID = 00000,
                //    ProductIDTo = 99999,
                //    DateFrom = from_date,
                //    DateTo = to_date,
                //    Qtype = 1
                //};

                //var alldataData = groupwiseReportService.GetDataProvisionCalculation_DataMarge(paramSe, "OverdueClassification");


                //if (qType == "3") // Cneter wise
                //{
                //    if (Convert.ToInt32(center) != 0)
                //    {
                //        CenterID = center;
                //        CenterIDTo = center;
                //        StaffID = "00";
                //        StaffIDTo = "99999999";
                //        productID = "00";
                //        ProductIDTo = "99999999";
                //    }
                //    else
                //    {
                //        CenterID = "00";
                //        CenterIDTo = "99999999";
                //        StaffID = "00";
                //        StaffIDTo = "99999999";
                //        productID = "00";
                //        ProductIDTo = "99999999";
                //    }
                //}
                //else if (qType == "4") // Staff wise
                //{
                //    if (Convert.ToInt32(emp) != 0)
                //    {
                //        CenterID = "00";
                //        CenterIDTo = "99999999";
                //        StaffID = emp;
                //        StaffIDTo = emp;
                //        productID = "00";
                //        ProductIDTo = "99999999";
                //    }
                //    else
                //    {
                //        CenterID = "00";
                //        CenterIDTo = "99999999";
                //        StaffID = "00";
                //        StaffIDTo = "99999999";
                //        productID = "00";
                //        ProductIDTo = "99999999";
                //    }
                //}
                //else if (qType == "2") // prod wise
                //{
                //    if (Convert.ToInt32(prod) != 0)
                //    {
                //        CenterID = "00";
                //        CenterIDTo = "99999999";
                //        StaffID = "00";
                //        StaffIDTo = "99999999";
                //        productID = prod;
                //        ProductIDTo = prod;
                //    }
                //    else
                //    {
                //        CenterID = "00";
                //        CenterIDTo = "99999999";
                //        StaffID = "00";
                //        StaffIDTo = "99999999";
                //        productID = "00";
                //        ProductIDTo = "99999999";
                //    }
                //}
                //else if (qType == "5") // prod wise
                //{


                //    if (Convert.ToInt32(prod) != 0)
                //    {
                //        //OfficeID = LoggedInOrganizationID;
                //        //OfficeIDTO = LoginUserOfficeID;
                //        CenterID = "00";
                //        CenterIDTo = "99999999";
                //        StaffID = "00";
                //        StaffIDTo = "99999999";
                //        productID = "00";
                //        ProductIDTo = "99999999";
                //    }
                //    else
                //    {
                //        CenterID = "00";
                //        CenterIDTo = "99999999";
                //        StaffID = "00";
                //        StaffIDTo = "99999999";
                //        productID = "00";
                //        ProductIDTo = "99999999";
                //    }
                //}
                //else if (qType == "6") // prod wise
                //{
                //    if (Convert.ToInt32(prod) != 0)
                //    {
                //        CenterID = "00";
                //        CenterIDTo = "99999999";
                //        StaffID = "00";
                //        StaffIDTo = "99999999";
                //        productID = "00";
                //        ProductIDTo = "99999999";
                //    }
                //    else
                //    {
                //        CenterID = "00";
                //        CenterIDTo = "99999999";
                //        StaffID = "00";
                //        StaffIDTo = "99999999";
                //        productID = "00";
                //        ProductIDTo = "99999999";
                //    }
                //}
                //var param = new
                //{
                //    OfficeID = LoginUserOfficeID,
                //    OfficeIDTO = LoginUserOfficeID,
                //    CenterID = CenterID,
                //    CenterIDTo = CenterIDTo,
                //    StaffID = StaffID,
                //    StaffIDTo = StaffIDTo,
                //    productID = productID,
                //    ProductIDTo = ProductIDTo,
                //    DateFrom = from_date,
                //    DateTo = to_date,
                //    Qtype = qType
                //};

                //var alldata = groupwiseReportService.GetDataRecoveryStatement(param, "OverdueClassification");
                var param = new
                {
                    OfficeID = LoginUserOfficeID,
                    OfficeIDTO = LoginUserOfficeID,
                    CenterID = 00000,
                    CenterIDTo = 99999,
                    StaffID = 00000,
                    StaffIDTo = 99999,
                    productID = 00000,
                    ProductIDTo = 99999,
                    DateFrom = from_date,
                    DateTo = to_date,
                    OrgID_PO = LoggedInOrganizationID
                };

                var alldata = groupwiseReportService.GetDataRecoveryStatement(param, "getProvisionReport");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", from_date);
                reportParam.Add("DateTo", to_date);
                if (qType == "6")
                {
                    ReportHelper.PrintReport("ProvisionCalculation.rpt", alldata.Tables[0], reportParam);
                }
                else
                    ReportHelper.PrintReport("ProvisionCalculation.rpt", alldata.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateStaffWiseSpecialSavingsReport(string from_date, string to_date, string emp, string prod, string center, string offc, string qType)
        {
            try
            {
                //'84','84','00','99999999','00','99999999','00','99999999','01 Jul 2015','31 Jul 2015',3
                string CenterID = string.Empty; string CenterIDTo = string.Empty; string StaffID = string.Empty;
                string StaffIDTo = string.Empty; string productID = string.Empty; string ProductIDTo = string.Empty;
                DataSet alldata;
                if (qType == "2") // Cneter wise
                {
                    if (Convert.ToInt32(center) != 0)
                    {
                        CenterID = center;
                        CenterIDTo = center;
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                else if (qType == "3") // Staff wise
                {
                    if (Convert.ToInt32(emp) != 0)
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = emp;
                        StaffIDTo = emp;
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                else if (qType == "4") // prod wise
                {
                    //if (LoggedInOrganizationID == 38 || LoggedInOrganizationID == 7 || LoggedInOrganizationID == 5 || LoggedInOrganizationID == 58 || LoggedInOrganizationID == 7 || LoggedInOrganizationID == 94)
                    //{

                  
                        if (prod != "0")
                        {
                            CenterID = "00";
                            CenterIDTo = "99999999";
                            StaffID = "00";
                            StaffIDTo = "99999999";
                            productID = prod;
                            ProductIDTo = prod;
                            qType = "6";
                        }
                        else
                        {
                            CenterID = "00";
                            CenterIDTo = "99999999";
                            StaffID = "00";
                            StaffIDTo = "99999999";
                            productID = "00000";
                            ProductIDTo = "ZZZZZ";
                            qType = "6";
                        }
                   
                  
                    //}
                    //else
                    //{
                    //    if (Convert.ToInt32(prod) != 0)
                    //    {
                    //        CenterID = "00";
                    //        CenterIDTo = "99999999";
                    //        StaffID = "00";
                    //        StaffIDTo = "99999999";
                    //        productID = prod;
                    //        ProductIDTo = prod;
                    //    }
                    //    else
                    //    {
                    //        CenterID = "00";
                    //        CenterIDTo = "99999999";
                    //        StaffID = "00";
                    //        StaffIDTo = "99999999";
                    //        productID = "00";
                    //        ProductIDTo = "99999999";
                    //    }
                    //}

                }
                else if (qType == "5") // prod wise
                {


                    if (Convert.ToInt32(prod) != 0)
                    {
                        //OfficeID = LoggedInOrganizationID;
                        //OfficeIDTO = LoginUserOfficeID;
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                // Due
                else if (qType == "6")
                {
                    CenterID = string.IsNullOrEmpty(center) | center == "0" ? "00" : center;
                    CenterIDTo = string.IsNullOrEmpty(center) | center == "0" ? "99999999" : center;
                    StaffID = string.IsNullOrEmpty(emp) | emp == "0" ? "00" : emp;
                    StaffIDTo = string.IsNullOrEmpty(emp) | emp == "0" ? "99999999" : emp;
                    productID = string.IsNullOrEmpty(prod) | prod == "0" ? "00" : prod;
                    ProductIDTo = string.IsNullOrEmpty(prod) | prod == "0" ? "99999999" : prod;
                }
                else if (qType == "1") // prod wise
                {
                    if (Convert.ToInt32(prod) != 0)
                    {
                        //OfficeID = LoggedInOrganizationID;
                        //OfficeIDTO = LoginUserOfficeID;
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }


                var param = new
                {

                    OfficeID = LoginUserOfficeID,
                    OfficeIDTO = LoginUserOfficeID,
                    CenterID = CenterID,
                    CenterIDTo = CenterIDTo,
                    StaffID = StaffID,
                    StaffIDTo = StaffIDTo,
                    productID = productID,
                    ProductIDTo = ProductIDTo,
                    DateFrom = from_date,
                    DateTo = to_date,
                    Qtype = qType
                };

                if (LoggedInOrganizationID == 38 || LoggedInOrganizationID == 7 || LoggedInOrganizationID == 5 || LoggedInOrganizationID == 58 || LoggedInOrganizationID == 7 || LoggedInOrganizationID == 94)
                {

                }
                if (LoggedInOrganizationID == 5)
                {
                    alldata = groupwiseReportService.GetDataStaffWiseSpecialSavings(param, "SpecialSavingsStaffWiseTermDepositWise");
                }
                else
                {
                    alldata = groupwiseReportService.GetDataStaffWiseSpecialSavings(param, "SpecialSavingsStaffWise");
                }
                

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", from_date);
                reportParam.Add("DateTo", to_date);
                //if (LoggedInOrganizationID == 5 || LoggedInOrganizationID == 58 || LoggedInOrganizationID == 7 || LoggedInOrganizationID == 94 || LoggedInOrganizationID == 5 || LoggedInOrganizationID == 7)
                //{
                if (qType == "1")
                {
                    ReportHelper.PrintReport("StaffWiseSpecialSavings.rpt", alldata.Tables[0], reportParam);
                }
                else if (qType == "6")
                {
                    ReportHelper.PrintReport("StaffWiseSpecialSavingsProductWise.rpt", alldata.Tables[0], reportParam);
                }
                else
                    ReportHelper.PrintReport("StaffWiseSpecialSavingsGroup.rpt", alldata.Tables[0], reportParam);


                //}
                //else
                //{
                //    if (qType == "1" || qType == "6")
                //    {
                //        ReportHelper.PrintReport("StaffWiseSpecialSavings.rpt", alldata.Tables[0], reportParam);
                //    }
                //    else
                //        ReportHelper.PrintReport("StaffWiseSpecialSavingsGroup.rpt", alldata.Tables[0], reportParam);
                //}

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateStaffWiseSpecialSavingsReportExport(string from_date, string to_date, string emp, string prod, string center, string offc, string qType)
        {
            try
            {
                //'84','84','00','99999999','00','99999999','00','99999999','01 Jul 2015','31 Jul 2015',3
                string CenterID = string.Empty; string CenterIDTo = string.Empty; string StaffID = string.Empty;
                string StaffIDTo = string.Empty; string productID = string.Empty; string ProductIDTo = string.Empty;

                if (qType == "2") // Cneter wise
                {
                    if (Convert.ToInt32(center) != 0)
                    {
                        CenterID = center;
                        CenterIDTo = center;
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                else if (qType == "3") // Staff wise
                {
                    if (Convert.ToInt32(emp) != 0)
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = emp;
                        StaffIDTo = emp;
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                else if (qType == "4") // prod wise
                {
                    if (LoggedInOrganizationID == 58 || LoggedInOrganizationID == 7)
                    {
                        if (prod != "0")
                        {
                            CenterID = "00";
                            CenterIDTo = "99999999";
                            StaffID = "00";
                            StaffIDTo = "99999999";
                            productID = prod;
                            ProductIDTo = prod;
                            qType = "6";
                        }
                        else
                        {
                            CenterID = "00";
                            CenterIDTo = "99999999";
                            StaffID = "00";
                            StaffIDTo = "99999999";
                            productID = "00000";
                            ProductIDTo = "ZZZZZ";
                            qType = "6";
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(prod) != 0)
                        {
                            CenterID = "00";
                            CenterIDTo = "99999999";
                            StaffID = "00";
                            StaffIDTo = "99999999";
                            productID = prod;
                            ProductIDTo = prod;
                        }
                        else
                        {
                            CenterID = "00";
                            CenterIDTo = "99999999";
                            StaffID = "00";
                            StaffIDTo = "99999999";
                            productID = "00";
                            ProductIDTo = "99999999";
                        }
                    }

                }
                else if (qType == "5") // prod wise
                {


                    if (Convert.ToInt32(prod) != 0)
                    {
                        //OfficeID = LoggedInOrganizationID;
                        //OfficeIDTO = LoginUserOfficeID;
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                else if (qType == "1") // prod wise
                {
                    if (Convert.ToInt32(prod) != 0)
                    {
                        //OfficeID = LoggedInOrganizationID;
                        //OfficeIDTO = LoginUserOfficeID;
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                if (LoggedInOrganizationID == 58 || LoggedInOrganizationID == 7)
                {

                }

                var param = new
                {

                    OfficeID = LoginUserOfficeID,
                    OfficeIDTO = LoginUserOfficeID,
                    CenterID = CenterID,
                    CenterIDTo = CenterIDTo,
                    StaffID = StaffID,
                    StaffIDTo = StaffIDTo,
                    productID = productID,
                    ProductIDTo = ProductIDTo,
                    DateFrom = from_date,
                    DateTo = to_date,
                    Qtype = qType
                };



                GridView gv = new GridView();
                var allRepaymentSchedule = groupwiseReportService.GetDataStaffWiseSpecialSavings(param, "SpecialSavingsStaffWiseExport");

                var detail = allRepaymentSchedule.Tables[0];
                gv.DataSource = detail;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=GenerateStaffWiseSpecialSavingsReport.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return RedirectToAction("GenerateStaffWiseSpecialSavingsReport");

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateStaffWiseSpecialSavingsReportJCF(string from_date, string to_date, string emp, string prod, string center, string offc, string qType)
        {
            try
            {
                //'84','84','00','99999999','00','99999999','00','99999999','01 Jul 2015','31 Jul 2015',3
                string CenterID = string.Empty; string CenterIDTo = string.Empty; string StaffID = string.Empty;
                string StaffIDTo = string.Empty; string productID = string.Empty; string ProductIDTo = string.Empty;

                if (qType == "2") // Cneter wise
                {
                    if (Convert.ToInt32(center) != 0)
                    {
                        CenterID = center;
                        CenterIDTo = center;
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                else if (qType == "3") // Staff wise
                {
                    if (Convert.ToInt32(emp) != 0)
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = emp;
                        StaffIDTo = emp;
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                else if (qType == "4") // prod wise
                {
                    if (Convert.ToInt32(prod) != 0)
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = prod;
                        ProductIDTo = prod;
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                else if (qType == "5") // prod wise
                {


                    if (Convert.ToInt32(prod) != 0)
                    {
                        //OfficeID = LoggedInOrganizationID;
                        //OfficeIDTO = LoginUserOfficeID;
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                else if (qType == "1") // prod wise
                {
                    if (Convert.ToInt32(prod) != 0)
                    {
                        //OfficeID = LoggedInOrganizationID;
                        //OfficeIDTO = LoginUserOfficeID;
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                var param = new
                {
                    OfficeID = LoginUserOfficeID,
                    OfficeIDTO = LoginUserOfficeID,
                    CenterID = CenterID,
                    CenterIDTo = CenterIDTo,
                    StaffID = StaffID,
                    StaffIDTo = StaffIDTo,
                    productID = productID,
                    ProductIDTo = ProductIDTo,
                    DateFrom = from_date,
                    DateTo = to_date,
                    Qtype = qType
                };
                //var param = new
                //{
                //    OfficeID = 84,
                //    OfficeIDTO = 84,
                //    CenterID = "00",
                //    CenterIDTo = "99999999",
                //    StaffID = "00",
                //    StaffIDTo = "99999999",
                //    productID = "00",
                //    ProductIDTo = "99999999",
                //    DateFrom = "01 Jul 2015",
                //    DateTo = "31 Jul 2015",
                //    Qtype = 3
                //};
                var alldata = groupwiseReportService.GetDataStaffWiseSpecialSavings(param, "SpecialSavingsStaffWise");

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", from_date);
                reportParam.Add("DateTo", to_date);
                if (qType == "1")
                {
                    // ReportHelper.PrintReport("StaffWiseSpecialSavingsJCF.rpt", alldata.Tables[0], reportParam);
                    ReportHelper.PrintReport("StaffWiseSpecialSavingsJCFCenterWiseGroup.rpt", alldata.Tables[0], reportParam);
                }
                else
                    ReportHelper.PrintReport("StaffWiseSpecialSavingsGroup.rpt", alldata.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateLoanAndSavingsBalanceSW(string offc, string center, string emp, string prod, string from_date, string to_date, string qType)
        {
            try
            {

                string OfficeID = string.Empty; string OfficeIDTo = string.Empty;
                string CenterID = string.Empty; string CenterIDTo = string.Empty;
                string StaffID = string.Empty; string StaffIDTo = string.Empty;
                string productID = string.Empty; string ProductIDTo = string.Empty;

                if (qType == "1") // Cneter wise
                {
                    if (Convert.ToInt32(center) != 0)
                    {
                        OfficeID = "00";
                        OfficeIDTo = "99999";
                        CenterID = center;
                        CenterIDTo = center;
                        StaffID = "00";
                        StaffIDTo = "99999";
                        productID = "00";
                        ProductIDTo = "99999";
                    }
                    else
                    {
                        OfficeID = "00";
                        OfficeIDTo = "99999";
                        CenterID = "00";
                        CenterIDTo = "99999";
                        StaffID = "00";
                        StaffIDTo = "99999";
                        productID = "00";
                        ProductIDTo = "99999";
                    }
                }
                else if (qType == "2") // Staff wise
                {
                    if (Convert.ToInt32(emp) != 0)
                    {
                        OfficeID = "00";
                        OfficeIDTo = "99999";
                        CenterID = "00";
                        CenterIDTo = "99999";
                        StaffID = emp;
                        StaffIDTo = emp;
                        productID = "00";
                        ProductIDTo = "99999";
                    }
                    else
                    {
                        OfficeID = "00";
                        OfficeIDTo = "99999";
                        CenterID = "00";
                        CenterIDTo = "99999";
                        StaffID = "00";
                        StaffIDTo = "99999";
                        productID = "00";
                        ProductIDTo = "99999";
                    }
                }
                else if (qType == "3") // prod wise
                {
                    if (Convert.ToInt32(prod) != 0)
                    {
                        OfficeID = "00";
                        OfficeIDTo = "99999";
                        CenterID = "00";
                        CenterIDTo = "99999";
                        StaffID = "00";
                        StaffIDTo = "99999";
                        productID = prod;
                        ProductIDTo = prod;
                    }
                    else
                    {
                        OfficeID = "00";
                        OfficeIDTo = "99999";
                        CenterID = "00";
                        CenterIDTo = "99999";
                        StaffID = "00";
                        StaffIDTo = "99999";
                        productID = "00";
                        ProductIDTo = "99999";
                    }
                }

                var param = new
                {

                    OfficeID = LoginUserOfficeID,
                    OfficeIDTO = LoginUserOfficeID,
                    CenterID = CenterID,
                    CenterIDTo = CenterIDTo,
                    StaffID = StaffID,
                    StaffIDTo = StaffIDTo,
                    productID = productID,
                    ProductIDTo = ProductIDTo,
                    DateFrom = from_date,
                    DateTo = to_date,
                    Qtype = qType
                };

                var alldata = unlimitedReportService.GetDataLoanAndSavingsBalanceSWReport(param);

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", from_date);
                reportParam.Add("DateTo", to_date);

                if (qType == "3") // Overall
                {
                    ReportHelper.PrintReport("rptLoanAndSavingsBalanceSW_GroupWise_forProduct.rpt", alldata.Tables[0], reportParam);
                }
                else
                    ReportHelper.PrintReport("rptLoanAndSavingsBalanceSW_GroupWise.rpt", alldata.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateOrganizerWiseRecoveryReport(string offc, string center, string prod, string emp, string from_date, string to_date, string qType)
        {
            try
            {
                //'84','84','00','99999999','00','99999999','00','99999999','01 Jul 2015','31 Jul 2015',3
                string CenterID = string.Empty; string CenterIDTo = string.Empty;
                string productID = string.Empty; string ProductIDTo = string.Empty;
                string StaffID = string.Empty; string StaffIDTo = string.Empty;

                var param = new
                {
                    Office = LoginUserOfficeID,

                    DateFrom = from_date,
                    DateTo = to_date

                };


                //  var alldata = groupwiseReportService.GetDataOrganizerWiseRecoveryStatement(param, "OrganizerWiseRecoveryStatemnt");
                var alldata = groupwiseReportService.GetDataOrganizerWiseRecoveryStatement(param, "Rpt_StaffwiseStatement");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", from_date);
                reportParam.Add("DateTo", to_date);
                if (qType == "1")
                {

                    ReportHelper.PrintReport("rptStaffWiseStatement.rpt", alldata.Tables[0], reportParam);

                }
                else

                    ReportHelper.PrintReport("rptStaffWiseStatement.rpt", alldata.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult StaffWiseStatement()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult GenerateStaffWiseStatement(string OfficeId, string Qtype, string DateTo)
        {
            try
            {
                var param = new { officeID = OfficeId, QType = Convert.ToInt32(Qtype), DateTo = DateTo };

                var alldata = groupwiseReportService.GetDataOrganizerWiseRecoveryStatement(param, "getStaffWiseStatement");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateTo);
                reportParam.Add("DateTo", DateTo);

                ReportHelper.PrintReport("rptStaffWiseStatementConsolidation.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateDailySavingCollectionReport(string Qtype, string Center)
        {
            try
            {
                var param = new { Qtype = Qtype, Office = SessionHelper.LoginUserOfficeID, Center = Center, Org = SessionHelper.LoginUserOrganizationID };
                var alldata = groupwiseReportService.GetDataDailyReport(param, "Rpt_DailySavingsCollection");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("rptDailySavingsCollection.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateSamitywiseRecoveryReportExport(string Qtype, string DateFrom, string DateTo)
        {
            //var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center };
            //groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_DailySavingsCollection");
            var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo };
            //var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_Recovery_Monthly_Report");

            GridView gv = new GridView();
            //var allRepaymentSchedule = creditscoreService.GetAll();
            var allRepaymentSchedule = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_Recovery_Monthly_Report");
            var detail = allRepaymentSchedule.Tables[0];
            gv.DataSource = detail;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=GenerateSamitywiseRecovery.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("GenerateSamitywiseRecovery");
        }
        public ActionResult GenerateDailySavingCollectionReportExport(string Qtype, string Center)
        {
            var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center };
            groupwiseReportService.GetDataDailyReport(param, "Rpt_DailySavingsCollection");

            GridView gv = new GridView();
            //var allRepaymentSchedule = creditscoreService.GetAll();
            var allRepaymentSchedule = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_DailySavingsCollection");
            var detail = allRepaymentSchedule.Tables[0];
            gv.DataSource = detail;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=rptDailySavingsCollection.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("DailySavingCollectionReport");
        }
        public ActionResult GenerateTodaysSummaryGroupWiseReport(string from_date, string to_date, string emp, string prod, string center, string offc, string qType)
        {
            try
            {
                //'84','84','00','99999999','00','99999999','00','99999999','01 Jul 2015','31 Jul 2015',3
                string CenterID = string.Empty; string CenterIDTo = string.Empty; string StaffID = string.Empty;
                string StaffIDTo = string.Empty; string productID = string.Empty; string ProductIDTo = string.Empty;
                String OfficeID = string.Empty; string OfficeIDTO = string.Empty;

                if (qType == "4") // Cneter wise
                {
                    if (Convert.ToInt32(center) != 0)
                    {
                        OfficeID = "00";
                        OfficeIDTO = "99999999";
                        CenterID = center;
                        CenterIDTo = center;
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        OfficeID = "00";
                        OfficeIDTO = "99999999";
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                else if (qType == "3") // Staff wise
                {
                    if (Convert.ToInt32(emp) != 0)
                    {
                        OfficeID = "00";
                        OfficeIDTO = "99999999";
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = emp;
                        StaffIDTo = emp;
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        OfficeID = "00";
                        OfficeIDTO = "99999999";
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                else if (qType == "2") // prod wise
                {
                    if (Convert.ToInt32(prod) != 0)
                    {
                        OfficeID = "00";
                        OfficeIDTO = "99999999";
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = prod;
                        ProductIDTo = prod;
                    }
                    else
                    {
                        OfficeID = "00";
                        OfficeIDTO = "99999999";
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                else if (qType == "5") // Offc wise
                {
                    if (Convert.ToInt32(offc) != 0)
                    {
                        OfficeID = offc; // LoginUserOfficeID.ToString();
                        OfficeIDTO = offc; //LoginUserOfficeID.ToString();
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                    else
                    {
                        OfficeID = "00";
                        OfficeIDTO = "99999999";
                        CenterID = "00";
                        CenterIDTo = "99999999";
                        StaffID = "00";
                        StaffIDTo = "99999999";
                        productID = "00";
                        ProductIDTo = "99999999";
                    }
                }
                var param = new
                {
                    Org = SessionHelper.LoginUserOrganizationID,
                    OfficeID = offc, //LoginUserOfficeID,
                    OfficeIDTO = offc, //LoginUserOfficeID,
                    CenterID = CenterID,
                    CenterIDTo = CenterIDTo,
                    StaffID = StaffID,
                    StaffIDTo = StaffIDTo,
                    productID = productID,
                    ProductIDTo = ProductIDTo,
                    DateFrom = from_date,
                    DateTo = to_date,
                    Qtype = qType
                };
                //var param = new
                //{
                //    OfficeID = 84,
                //    OfficeIDTO = 84,
                //    CenterID = "00",
                //    CenterIDTo = "99999999",
                //    StaffID = "00",
                //    StaffIDTo = "99999999",
                //    productID = "00",
                //    ProductIDTo = "99999999",
                //    DateFrom = "01 Jul 2015",
                //    DateTo = "31 Jul 2015",
                //    Qtype = 3
                //};
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_TodaySummary");

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", from_date);
                reportParam.Add("DateTo", to_date);
                if (qType == "1")
                {
                    ReportHelper.PrintReport("rptTodaySummaryLoaneeWise.rpt", alldata.Tables[0], reportParam);
                }
                else
                    ReportHelper.PrintReport("rptTodaySummaryGroupWise.rpt", alldata.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerategetOfficeDashBoardReport(string qType, string OfficeIDFrom, string Office_Name, string fromDate, string toDate)
        {
            try
            {
                string OfficeTo = string.Empty;
                string OfficeFrom = string.Empty;
                string officeNa_me = string.Empty;

                if (Convert.ToInt32(OfficeIDFrom) != 0)
                {
                    OfficeFrom = OfficeIDFrom;
                    OfficeTo = OfficeIDFrom;
                    officeNa_me = Office_Name;
                }
                else
                {
                    if (qType == "1")
                    {
                        OfficeFrom = "00";
                        OfficeTo = "99999999";
                        officeNa_me = "Head Office :" + Office_Name;
                    }
                    if (qType == "2")
                    {

                        OfficeFrom = "00";
                        OfficeTo = "99999999";
                        officeNa_me = "Zone Office";
                    }
                    if (qType == "3")
                    {
                        OfficeFrom = "00";
                        OfficeTo = "99999999";
                        officeNa_me = "Area Office";
                    }
                    if (qType == "4")
                    {
                        OfficeFrom = "00";
                        OfficeTo = "99999999";
                        officeNa_me = "Branch Office";
                    }

                }

                if (qType == "1")
                {
                    var param = new { Qtype = Convert.ToInt32(qType), OfficeIDFrom = Convert.ToInt32(OfficeFrom), officeIdto = Convert.ToInt32(OfficeTo), dateFrom = Convert.ToDateTime(fromDate), dateTo = Convert.ToDateTime(toDate) };
                    var alldata = groupwiseReportService.GetOfficeDashBoard(param, "proc_getOfficeDashBoard");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("officeName", officeNa_me);
                    reportParam.Add("dateFrom", fromDate);
                    reportParam.Add("toDate", toDate);
                    ReportHelper.PrintReport("rpt_getOfficeDashBoardrptforHeadOffice.rpt", alldata.Tables[0], reportParam);
                }
                else
                {
                    var param = new { Qtype = Convert.ToInt32(qType), OfficeIDFrom = Convert.ToInt32(OfficeFrom), officeIdto = Convert.ToInt32(OfficeTo), dateFrom = Convert.ToDateTime(fromDate), dateTo = Convert.ToDateTime(toDate) };
                    var alldata = groupwiseReportService.GetOfficeDashBoard(param, "proc_getOfficeDashBoard");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("officeName", officeNa_me);
                    reportParam.Add("DateFrom", fromDate);
                    reportParam.Add("ToDate", toDate);
                    ReportHelper.PrintReport("rpt_getOfficeDashBoardrpt.rpt", alldata.Tables[0], reportParam);
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateRecoveryReport(string Qtype, string DateTo)
        {
            try
            {
                var param = new { Qtype = Qtype, Office = SessionHelper.LoginUserOfficeID, DateTo = DateTo, Org = SessionHelper.LoginUserOrganizationID };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_Recoverable");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("Loan_Recoverable_Report.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateMiscellaneousMemberWise(string Qtype, string DateTo)
        {
            try
            {
                //var param = new { Office = SessionHelper.LoginUserOfficeID, Date = DateTo, QType=1 };
                //var alldata = groupwiseReportService.GenerateMiscellaneousMemberWise(param, "RPT_Miscellaneous");
                //var reportParam = new Dictionary<string, object>();
                //reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("DateTo", DateTo);
                //ReportHelper.PrintReport("Loan_Recoverable_Register.rpt", alldata.Tables[0], reportParam);
                //return Content(string.Empty);

                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Office", Value = SessionHelper.LoginUserOfficeID.ToString() });
                paramValues.Add(new ParameterValue() { Name = "Date", Value = DateTo });
                paramValues.Add(new ParameterValue() { Name = "QType", Value = "1" });
                PrintSSRSReport("/gBanker_Reports/Miscellaneous_Transaction_Report", paramValues.ToArray(), "gBankerDbContext");
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateRecoveryReportGUK(string Qtype, string DateTo)
        {
            try
            {
                var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_RecoverableRegister");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("Loan_Recoverable_Register.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateMRADBMS4(string Qtype, string DateTo)
        {
            try
            {
                var param = new { Date = DateTo, OfficeID = SessionHelper.LoginUserOfficeID, OfficeIDTo = SessionHelper.LoginUserOfficeID };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Proc_get_MRA4");
                var reportParam = new Dictionary<string, object>();
                // reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptMRADBMS4.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateMRADBMS2(string Qtype, string DateTo)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, OfficeID = SessionHelper.LoginUserOfficeID, Date = DateTo, };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_WorkingArea_DBMS_2");
                var reportParam = new Dictionary<string, object>();
                // reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptMRADBMS2.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateTodays_Comperative(string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Org = LoggedInOrganizationID, OfficeID = SessionHelper.LoginUserOfficeID, TrxDate = DateFrom, TrxDateDato = DateTo };
                var alldata = groupwiseReportService.GetDataGenerateTodays_Comperative(param, "Rpt_Todays_Comperative");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("Comperative.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //public ActionResult GenerateInactiveMemberList(string DateTo)
        //{
        //    try
        //    {
        //        var param = new { OfficeID = SessionHelper.LoginUserOfficeID };
        //        var alldata = groupwiseReportService.GetDataUltimateReleaseReportDSK(param, "Proc_Get_InActiveMemberList");
        //        var reportParam = new Dictionary<string, object>();
        //        reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);

        //        //reportParam.Add("DateTo", DateTo);
        //        ReportHelper.PrintReport("InActiveMemberList.rpt", alldata.Tables[0], reportParam);
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        public ActionResult GenerateInactiveMemberListDrop(string DateTo)
        {
            try
            {
                var param = new { OfficeID = SessionHelper.LoginUserOfficeID };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportDSK(param, "Proc_Get_InActiveMemberListDrop");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);

                //reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("InActiveMemberListDropable.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GeneratePostWriteOffReport(string DateFrom, string DateTo)
        {
            try
            {
                var param = new { officeID = SessionHelper.LoginUserOfficeID, StartDate = DateFrom, EndDate = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Proc_get_PostWriteOffRegister");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("PostWriteOffRegister.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GeneratePostWriteOffReportCollectionDate(string DateFrom, string DateTo)
        {
            try
            {
                var param = new { officeID = SessionHelper.LoginUserOfficeID, StartDate = DateFrom, EndDate = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Proc_get_PostWriteOffRegisterCollectionDateWise");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("PostWriteOffRegister.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateReconCileReport(string DateFrom, string DateTo)
        {
            try
            {
                var param = new { officeID = 0000, officeIDTo = 99999, TrxDateFrom = DateFrom, TrxDateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Proc_RPT_AccReconcile");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptAccountReconcile.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateLoanApprovalReport(string Qtype, string DateFrom, string DateTo, string Product, string Center)
        {
            try
            {
                var param = new { Qtype = Qtype, Office = SessionHelper.LoginUserOfficeID, Center = Center, Product = Product, DateFrom = DateFrom, DateTo = DateTo, Org = SessionHelper.LoginUserOrganizationID };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_LoanApproval");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptLoanApproval.rpt", alldata.Tables[0], new Dictionary<string, object>());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateMemberwiseSavingInterestReport(string Qtype, string DateTo)
        {
            try
            {
                var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportMemberwiseSavingInterestReport(param, "Rpt_MonthWiseInterest");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                // ReportHelper.PrintReport("rptMemberWiseSavingsInterestReport.rpt", alldata.Tables[0], reportParam);
                ReportHelper.PrintReport("rptMemberWiseSavingInterest.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //public ActionResult GenerateMemberCenterwiseSavingInterestReport(string Qtype, string DateTo, string center, string member)
        //{
        //    try
        //    {

        //        if (center == null)
        //        {
        //            center = "0";
        //        }
        //        if (member == null)
        //        {
        //            member = "0";
        //        }
        //        DataSet alldata;
        //        var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = DateTo, Center = center, member = member };
        //        if (LoggedInOrganizationID == 4)
        //        {
        //            alldata = groupwiseReportService.GetDataUltimateReleaseReportMemberCenterwiseSavingInterestReportDSK(param, "Rpt_MonthWiseInterestMemberWise");
        //        }
        //        else
        //            alldata = groupwiseReportService.GetDataUltimateReleaseReportMemberCenterwiseSavingInterestReport(param, "Rpt_MonthWiseInterestMemberWise");


        //        var reportParam = new Dictionary<string, object>();
        //        reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //        reportParam.Add("DateTo", DateTo);
        //        if (LoggedInOrganizationID == 99)
        //        {
        //            ReportHelper.PrintReport("rptMemberWiseSavingInterest_Proshika.rpt", alldata.Tables[0], reportParam);
        //        }
        //        else
        //            // ReportHelper.PrintReport("rptMemberWiseSavingsInterestReport.rpt", alldata.Tables[0], reportParam);
        //            ReportHelper.PrintReport("rptMemberWiseSavingInterest.rpt", alldata.Tables[0], reportParam);
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        public ActionResult GenerateMemberCenterwiseSavingInterestReport(string Qtype, string DateTo, string center, string member, string ExportType)
        {
            try
            {

                if (center == null)
                {
                    center = "0";
                }
                if (member == null)
                {
                    member = "0";
                }
                DataSet alldata;
                var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = DateTo, Center = center, member = member };
                if (LoggedInOrganizationID == 4)
                {
                    alldata = groupwiseReportService.GetDataUltimateReleaseReportMemberCenterwiseSavingInterestReportDSK(param, "Rpt_MonthWiseInterestMemberWise");
                }
                else
                    alldata = groupwiseReportService.GetDataUltimateReleaseReportMemberCenterwiseSavingInterestReport(param, "Rpt_MonthWiseInterestMemberWise");


                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);

                if (LoggedInOrganizationID == 99)
                {
                    if (ExportType == "pdf")
                        ReportHelper.PrintReport("rptMemberWiseSavingInterest_Proshika.rpt", alldata.Tables[0], reportParam);
                    else
                        ReportHelper.ExportExcelReport("rptMemberWiseSavingInterest_Proshika.rpt", alldata.Tables[0], reportParam);

                }
                else {
                    if (ExportType == "pdf")
                        ReportHelper.PrintReport("rptMemberWiseSavingInterest.rpt", alldata.Tables[0], reportParam);
                    else
                        ReportHelper.ExportExcelReport("rptMemberWiseSavingInterest.rpt", alldata.Tables[0], reportParam);
                }

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult UpdateJson() //Getting data from database and save in Json File    
        {
            //using (var context = new gBankerDbContext())
            //{
            try
            {
                List<AccTrxMasterViewModel> List_AccTrxMasterViewModel = new List<AccTrxMasterViewModel>();
                var param = new { OrgID = LoggedInOrganizationID, OfficeID = LoginUserOfficeID, TrxDate = TransactionDate };
                var empList = accReportService.GetAccDataForReport(param, "Proc_Get_AccountDetails");

                List_AccTrxMasterViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new AccTrxMasterViewModel
                {
                    TrxMasterID = row.Field<long>("TrxMasterID"),
                    VoucherNo = row.Field<string>("VoucherNo"),
                    TrxDtMsg = row.Field<string>("TrxDtMsg"),
                    VoucherType = row.Field<string>("VoucherType"),
                    VoucherDesc = row.Field<string>("VoucherDesc"),
                    Reference = row.Field<string>("Reference"),
                    TotDebit = row.Field<decimal>("TotDebit"),
                    TotCredit = row.Field<decimal>("TotCredit"),
                    IsAutoVoucherMsg = row.Field<string>("IsAutoVoucherMsg"),
                    IsReconcileVoucherMSG = row.Field<string>("IsReconcileVoucherMSG")

                }).ToList();
                //var bindInstaData = div_items.Tables[0].Rows.ToString();
                var Contract_Details = new
                {
                    header = new { DataType = "H", MfiCode = "12345", AccountingDate = DateTime.Now, ProductionDate = DateTime.Now },
                    Details = new { DataType = "C", values = List_AccTrxMasterViewModel },
                    footer = new { DataType = "F", List_AccTrxMasterViewModel.Count },
                };



                var Subject = new
                {
                    header = new { DataType = "H", MfiCode = "12345", AccountingDate = DateTime.Now, ProductionDate = DateTime.Now },
                    Details = new { DataType = "C", values = List_AccTrxMasterViewModel },
                    footer = new { DataType = "F", List_AccTrxMasterViewModel.Count },
                };
                //var members = new
                //{
                //    header = new { DataType = "H", MfiCode = "12345", AccountingDate = DateTime.Now, ProductionDate = DateTime.Now },
                //    Details = new { DataType = "C", values = List_AccTrxMasterViewModel },
                //    footer = new { DataType = "F", List_AccTrxMasterViewModel.Count },
                //};
                var mraCIBData = new { Contract_Details, Subject };
                var jsonString = JsonConvert.SerializeObject(mraCIBData);

                byte[] fileBytes = Encoding.ASCII.GetBytes(jsonString);
                string fileName = "filename.json";
                return File(fileBytes, "application/json", fileName);

            }
            catch (Exception)
            {
                throw;
            }
            //}
        }
        //[HttpPost]
        //[AllowMultipleButton(Name = "action", Argument = "ExportToJson")]
        //public ActionResult ExportToJson(int? pageNumber)
        //{
        //    DataTable dtProduct = GetProductsDetail(pageNumber);
        //    var listProduct = (from DataRow row in dtProduct.Rows

        //                       select new Product()
        //                       {
        //                           ProductID = row["ProductID"] != null ? Convert.ToInt32(row["ProductID"]) : 0,
        //                           ProductName = Convert.ToString(row["ProductName"]),
        //                           Price = row["Price"] != null ? Convert.ToInt32(row["Price"]) : 0,
        //                           ProductDescription = Convert.ToString(row["ProductDescription"])
        //                       }).ToList();
        //    string jsonProductList = new JavaScriptSerializer().Serialize(listProduct);

        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    Response.Buffer = true;
        //    Response.ContentType = "application/json";
        //    Response.AddHeader("Content-Length", jsonProductList.Length.ToString());
        //    Response.AddHeader("Content-Disposition", "attachment; filename=ProductDetails.json;");
        //    Response.Output.Write(jsonProductList);
        //    Response.Flush();
        //    Response.End();

        //    return View("Index");
        //}
        public ActionResult GenerateMonthwiseSavingInterestReport(string Qtype, string DateTo)
        {
            try
            {

                // var exporrt = UpdateJson();
                DataSet alldata;
                var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = DateTo };
                if (LoggedInOrganizationID == 4)
                {
                    alldata = groupwiseReportService.GetDataUltimateReleaseReportMonthWiseSavingReportDSK(param, "Rpt_MonthWiseInterest");
                }
                else
                    alldata = groupwiseReportService.GetDataUltimateReleaseReportMonthWiseSavingReport(param, "Rpt_MonthWiseInterest");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptMonthWiseSavingInterest.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
                //return UpdateJson();
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateYearlyLoanClosingReport(string Qtype, string Center)
        {
            try
            {
                var param = new { Qtype = Qtype, Office = SessionHelper.LoginUserOfficeID, Center = Center, Org = SessionHelper.LoginUserOrganizationID };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_YearlyStatement");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("YearlyLoanclosing.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateYearlySavingClosingReport(string Qtype, string Center)
        {
            try
            {
                var param = new { Qtype = Qtype, Office = SessionHelper.LoginUserOfficeID, Center = Center, Org = SessionHelper.LoginUserOrganizationID };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_YearlyStatement");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("YearlySavingclosing.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateSamitywiseRecoveryReport(string Qtype, string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_Recovery_Monthly_Report");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("MonthlyLoanSavingsCollectionSamityWise.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateOrganizerRecoveryReportExport(string Qtype, string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo };
                //var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_Recovery_Monthly_Report");
                GridView gv = new GridView();
                //var allRepaymentSchedule = creditscoreService.GetAll();
                var allRepaymentSchedule = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_Recovery_Monthly_Report");
                var detail = allRepaymentSchedule.Tables[0];
                gv.DataSource = detail;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=GenerateOrganizerRecoveryReport.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return RedirectToAction("DailySavingCollectionReport");
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateOrganizerRecoveryReport(string Qtype, string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_Recovery_Monthly_Report");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("MonthlySavings_And_LoanReport.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //public ActionResult GenerateExpireListReport(string Qtype, string Center)
        //{
        //    try
        //    {
        //        var param = new { Qtype = Qtype, Office = SessionHelper.LoginUserOfficeID, Center = Center, Org = SessionHelper.LoginUserOrganizationID };
        //        var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_Expirelist");
        //        var reportParam = new Dictionary<string, object>();
        //        reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //        ReportHelper.PrintReport("rptExpireList.rpt", alldata.Tables[0], reportParam);
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        public ActionResult GeneratePOMIS1HQConsolidateReport()
        {
            try
            {

                var param1 = new { Qtype = 1, Org = SessionHelper.LoginUserOrganizationID };
                var param2 = new { Qtype = 2, Org = SessionHelper.LoginUserOrganizationID };
                var param3 = new { Qtype = 3, Org = SessionHelper.LoginUserOrganizationID };
                var param4 = new { Qtype = 4, Org = SessionHelper.LoginUserOrganizationID };
                var POMIS1As = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param1, "Rpt_POMIS_Consolidation");
                var POMIS1Bs = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param2, "Rpt_POMIS_Consolidation");
                var POMIS1Cs = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param4, "Rpt_POMIS_Consolidation");
                var POMIS1Ds = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param3, "Rpt_POMIS_Consolidation");
                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("SubRptSavingsStatement", POMIS1Bs.Tables[0]);
                subReportDB.Add("ItemWiseTotal", POMIS1Cs.Tables[0]);
                subReportDB.Add("SubRptMemberAdmission", POMIS1Ds.Tables[0]);
                ReportHelper.PrintWithSubReport("rpt_POMIS1_SavingsStatementHQ.rpt", POMIS1As.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_POMIS1_SavingsStatementHQ());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GetWorkingLogInfoInfo(string Qtype, string OfficeId, string DateTo)
        {
            try
            {
                //var param = new { Qtype = Convert.ToInt32(1) };
                var param = new { Qtype = Convert.ToInt32(Qtype), OfficeId = OfficeId, DateTo = DateTo };
                groupwiseReportService.GetWorkingLogInfomation(param, "getWorkingLogInfoDateWise");
                var param1 = new { Qtype = Convert.ToInt32(Qtype), OfficeId = OfficeId, DateTo = DateTo };
                var POMIS1Bs = groupwiseReportService.GetDataPOMIS1_SavingsStatementConsolidationOfficewise(param1, "getWorkingLogInfoDateWise");
                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("WorkingAreaSub", POMIS1Bs.Tables[0]);
                ReportHelper.PrintWithSubReport("WorkingLog.rpt", POMIS1Bs.Tables[0], new Dictionary<string, object>(), subReportDB, new WorkingLog());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GeneratePOMIS1HQConsolidateOfficewise(string Qtype, string OfficeId, string DateTo)
        {
            try
            {
                //var param = new { Qtype = Convert.ToInt32(1) };
                var param = new { Qtype = Convert.ToInt32(Qtype), OfficeId = OfficeId, DateTo = DateTo };
                var vOrgid = SessionHelper.LoginUserOrganizationID;
                if (vOrgid == 9)
                {
                    groupwiseReportService.GetDataPOMIS1_DataMarge(param, "POMIS2_SavingStatement_ConsolidationWithDate");
                }
                else
                {
                    groupwiseReportService.GetDataPOMIS1_DataMarge(param, "POMIS2_SavingStatement_ConsolidationWithDate");
                }

                if (vOrgid == 4) //DSK
                {
                    var param1 = new { Qtype = Convert.ToInt32(1), DateTo = DateTo, OfficeId = Convert.ToInt32(OfficeId) };
                    var param2 = new { Qtype = Convert.ToInt32(2), DateTo = DateTo, OfficeId = Convert.ToInt32(OfficeId) };
                    var param3 = new { Qtype = Convert.ToInt32(4), DateTo = DateTo, OfficeId = Convert.ToInt32(OfficeId) };
                    var param4 = new { Qtype = Convert.ToInt32(3), DateTo = DateTo, OfficeId = Convert.ToInt32(OfficeId) };

                    var POMIS1As = groupwiseReportService.GetDataPOMIS1_GroupAndMembersInfoConsolidationOfficewise(param1, "Proc_getPomis_1");
                    var POMIS1Bs = groupwiseReportService.GetDataPOMIS1_SavingsStatementConsolidationOfficewise(param2, "Proc_getPomis_1");
                    var POMIS1Cs = groupwiseReportService.GetDataPOMIS1_SavingsStatementItemWiseTotalConsolidationOfficewise(param3, "Proc_getPomis_1");
                    var POMIS1Ds = groupwiseReportService.GetDataPOMIS1_AdmisionWithdrawanAttendanceConsolidationOfficewise(param4, "Proc_getPomis_1");
                    var subReportDB = new Dictionary<string, DataTable>();
                    subReportDB.Add("SubRptSavingsStatement", POMIS1Bs.Tables[0]);
                    subReportDB.Add("ItemWiseTotal", POMIS1Cs.Tables[0]);
                    subReportDB.Add("SubRptMemberAdmission", POMIS1Ds.Tables[0]);
                    ReportHelper.PrintWithSubReport("POMIS_1HQ.rpt", POMIS1As.Tables[0], new Dictionary<string, object>(), subReportDB, new POMIS_1HQ());
                    return Content(string.Empty);
                }
                else
                {
                    var param1 = new { Qtype = Convert.ToInt32(1), DateTo = DateTo };
                    var param2 = new { Qtype = Convert.ToInt32(2), DateTo = DateTo };
                    var param3 = new { Qtype = Convert.ToInt32(4), DateTo = DateTo };
                    var param4 = new { Qtype = Convert.ToInt32(3), DateTo = DateTo };

                    var POMIS1As = groupwiseReportService.GetDataPOMIS1_GroupAndMembersInfoConsolidationOfficewise(param1, "Proc_getPomis_1");
                    var POMIS1Bs = groupwiseReportService.GetDataPOMIS1_SavingsStatementConsolidationOfficewise(param2, "Proc_getPomis_1");
                    var POMIS1Cs = groupwiseReportService.GetDataPOMIS1_SavingsStatementItemWiseTotalConsolidationOfficewise(param3, "Proc_getPomis_1");
                    var POMIS1Ds = groupwiseReportService.GetDataPOMIS1_AdmisionWithdrawanAttendanceConsolidationOfficewise(param4, "Proc_getPomis_1");
                    var subReportDB = new Dictionary<string, DataTable>();
                    subReportDB.Add("SubRptSavingsStatement", POMIS1Bs.Tables[0]);
                    subReportDB.Add("ItemWiseTotal", POMIS1Cs.Tables[0]);
                    subReportDB.Add("SubRptMemberAdmission", POMIS1Ds.Tables[0]);
                    ReportHelper.PrintWithSubReport("POMIS_1HQ.rpt", POMIS1As.Tables[0], new Dictionary<string, object>(), subReportDB, new POMIS_1HQ());
                    return Content(string.Empty);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult ProvisionConsolidate(string Qtype, string OfficeId, string DateTo)
        {
            try
            {
                string from_date = "01 Jun 2019";
                //var param = new { Qtype = Convert.ToInt32(1) };
                var param = new { OfficeId = OfficeId, ProcessDate = DateTo, OrgID_PO = LoggedInOrganizationID, Qtype = Convert.ToInt32(Qtype), };
                var vOrgid = SessionHelper.LoginUserOrganizationID;

                var alldata = groupwiseReportService.ProvisionCalculationConsolidate(param, "generateHOProvision");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", from_date);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("ProvisionConsolidate.rpt", alldata.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateMRA4HQConsolidateOfficewise(string Qtype, string OfficeId, string DateTo)
        {
            try
            {
                var param = new { Office = SessionHelper.LoginUserOfficeID, DateTo = DateTo };
                var alldata = groupwiseReportService.MRA4ConsolidateReport(param, "getMRA4ConsolidateReport");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("CompanyName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptMRADBMS4Consolidate.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateStaffProfileConsolidateOfficewise(string Qtype, string OfficeId, string DateTo)
        {
            try
            {
                var param = new { Qtype = Convert.ToInt32(Qtype), OfficeId = OfficeId, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "getStaffProfileConsolidate");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rpt_POMIS3B_StaffProfileConsolidate.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }
        }
        ActionResult GenerateLoanDisburseConsolidation(string Qtype, string OfficeId, string DateFrom, string DateTo)
        {
            try
            {
                Qtype = "4";
                var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = "", Member = "", DateFrom = DateFrom, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_LoanDisburse");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptLoanDisburseAllOffice.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult MonthlySummaryReportProcess()
        {
            int orgid = (int)LoggedInOrganizationID;
            if (orgid == 99)
            {
                IEnumerable<SelectListItem> items = new SelectList(" ");
                ViewData["YearList"] = items;
                return View();
            }
            else
            {
                return View("sorry, you do not have permission to access this report");
            }
        }
        public ActionResult GenerateMonthlySummaryReportProcess(int ProcessYear, int ProcessMonth)
        {
            try
            {
                var reportParam = new Dictionary<string, object>();
                int OfficeId = Convert.ToInt32(LoginUserOfficeID);
                var param1 = new { ProcessYear = ProcessYear, ProcessMonth = ProcessMonth, OfficeId = OfficeId };
                var alldata = groupwiseReportService.GetMonthlySummaryReportPorcess(param1, "Proc_Set_MonthlySummaryReportProcess");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }
        }
        public ActionResult StaffWiseConsolidation()
        {
            OfficeViewModel model = new OfficeViewModel();
            if (!IsDayInitiated)
                ViewData["TrxDate"] = DateTime.Now.Date.ToString("dd-MMM-yyyy");

            else
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");

            MapDropDownForStaffwiseConsolidation(model);

            return View(model);
        }

        [HttpGet]
        public JsonResult GenerateStaffWiseConsolidation(string OfficeIds, string FromDate, string ToDate)
        {
            try
            {
                var serializer = new JavaScriptSerializer();
                var offices = serializer.Deserialize<string[]>(OfficeIds);

                string OfficeIdCombine = String.Join("_", offices);

                var param = new { @OfficeIDs = OfficeIdCombine, @DateFrom = FromDate, @DateTo = ToDate };
                ultimateReportService.GetDataWithParameter(param, "GenerateStaffWiseComsolidation_1");
                return Json(new { IsSuccess = true, Msg = "Process completed successfully." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private void MapDropDownForStaffwiseConsolidation(OfficeViewModel model)
        {
            var OfficeList = officeService.GetAll()
                .Where(o => o.OrgID == LoggedInOrganizationID && o.OfficeLevel == 4)
                .OrderBy(x => x.OfficeCode);

            var listItems = OfficeList
                .Select(x => new SelectListItem
                {
                    Value = x.OfficeID.ToString(),
                    Text = x.OfficeCode + '-' + x.OfficeName
                });

            var List = new List<SelectListItem>();
            List.Add(new SelectListItem() { Text = "Select All", Value = "0" });
            List.AddRange(listItems);
            model.OfficeList = List;
        }
        public ActionResult GenerateOfficeWiseProgramMISReport(string Qtype, string OfficeId, string DateFrom, string DateTo)
        {
            try
            {

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);

                if (LoggedInOrganizationID == 5)
                {
                    if (Qtype == "6")
                    {
                        var param1 = new { Office = OfficeId, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype };
                        var alldata1 = groupwiseReportService.GetProgramMISReportJCF(param1, "Proc_RPT_MIS_LoanReportZoneWise");
                        ReportHelper.PrintReport("RPT_MIS_LoanStatementCrossTabHOConLogoWithZone.rpt", alldata1.Tables[0], reportParam);
                    }
                    else
                    {

                        var param = new { Office = OfficeId, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype };
                        var alldata = groupwiseReportService.GetProgramMISReportJCF(param, "Proc_RPT_MIS_LoanReport");
                        ReportHelper.PrintReport("RPT_MIS_LoanStatementCrossTabHOConWithLogo.rpt", alldata.Tables[0], reportParam);

                    }
                }
                else
                {
                    if (Qtype == "6")
                    {
                        var param1 = new { Office = OfficeId, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype };
                        var alldata1 = groupwiseReportService.GetProgramMISReport(param1, "Proc_RPT_MIS_LoanReportZoneWise");
                        ReportHelper.PrintReport("RPT_MIS_LoanStatementCrossTabHOConLogoWithZone.rpt", alldata1.Tables[0], reportParam);

                    }
                    else
                    {
                        var param = new { Office = OfficeId, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype };
                        var alldata = groupwiseReportService.GetProgramMISReport(param, "Proc_RPT_MIS_LoanReport");
                        ReportHelper.PrintReport("RPT_MIS_LoanStatementCrossTabHOConWithLogo.rpt", alldata.Tables[0], reportParam);
                    }

                }

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }
        }
        public ActionResult GenerateZO_OfficeWiseProgramMISReport(string Qtype, string OfficeId, string DateFrom, string DateTo)
        {
            try
            {

                var param = new { Office = OfficeId, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype };
                var alldata = groupwiseReportService.GetProgramMISReport(param, "Proc_RPT_MIS_LoanReport");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("RPT_MIS_LoanStatementCrossTabHOConWithLogo.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }
        }
        public ActionResult GenerateHOOfficeWiseProgramMISReport(string Qtype, string OfficeId, string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Office = OfficeId, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype };
                var alldata = groupwiseReportService.GetProgramMISReport(param, "Proc_RPT_MIS_LoanReport");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("RPT_MIS_LoanStatementCrossTabHOConWithLogo.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateProgramMISReport(string Qtype, string OfficeId, string DateFrom, string DateTo)
        {
            try
            {
                if (Qtype == "2")
                {
                    var param = new { Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype };
                    var alldata = groupwiseReportService.GetProgramMISReport(param, "Proc_RPT_MIS_LoanReport");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    ReportHelper.PrintReport("RPT_MIS_LoanStatementCrossTabCOwise.rpt", alldata.Tables[0], reportParam);
                }
                else if (Qtype == "1")
                {
                    var param = new { Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype };
                    var alldata = groupwiseReportService.GetProgramMISReport(param, "Proc_RPT_MIS_LoanReport");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    ReportHelper.PrintReport("RPT_MIS_LoanStatement.rpt", alldata.Tables[0], reportParam);
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateChangesReport(string Qtype, string OfficeId, string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Office = OfficeId, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype };
                var alldata = groupwiseReportService.GetOfficeWiseChangesReport(param, "Proc_RPT_ChangesReport");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("ChangesReportOfficeWise.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //public ActionResult GenerateFOWiseChangesReport(string Qtype, string OfficeId, string DateFrom, string DateTo)
        //{
        //    try
        //    {
        //        var param = new { Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype };
        //        var alldata = groupwiseReportService.GetFOWiseChangesReport(param, "Proc_RPT_ChangesReport");
        //        var subReportDB = new Dictionary<string, DataTable>();
        //        subReportDB.Add("RPT_ChangesReport1", alldata.Tables[0]);
        //        ReportHelper.PrintWithSubReport("RPT_ChangesReportCOwise.rpt", alldata.Tables[0], new Dictionary<string, object>(), subReportDB, new RPT_ChangesReportCOwise());
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        public ActionResult GenerateFOWiseChangesReport(string Qtype, string OfficeId, string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Qtype = 1 };
                var alldata = groupwiseReportService.GetFOWiseChangesReport(param, "getChangesReport01");
                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("RPT_ChangesReport1", alldata.Tables[0]);
                ReportHelper.PrintWithSubReport("RPT_ChangesReportCOwise.rpt", alldata.Tables[0], new Dictionary<string, object>(), subReportDB, new RPT_ChangesReportCOwise());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateNewFOWiseChangesReport(string Qtype, string OfficeId, string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype };
                var alldata = groupwiseReportService.GetFOWiseChangesReport(param, "Proc_RPT_ChangesReport__New");
                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("RPT_ChangesReport1", alldata.Tables[0]);
                ReportHelper.PrintWithSubReport("RPT_ChangesReportCOwise.rpt", alldata.Tables[0], new Dictionary<string, object>(), subReportDB, new RPT_ChangesReportCOwise());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }
        }
        public ActionResult GeneratePOMIS1HQPKSFNonPKSF(string Qtype, string OfficeId, string DateTo)
        {
            try
            {
                //var param = new { Qtype = Convert.ToInt32(1) };
                var param = new { Qtype = Convert.ToInt32(Qtype), OfficeId = OfficeId };

                groupwiseReportService.GetDataPOMIS1_DataMarge(param, "POMIS2_SavingStatement_ConsolidationPKSFNonPKSF");

                var param1 = new { Qtype = Convert.ToInt32(1), DateTo = DateTo };
                var param2 = new { Qtype = Convert.ToInt32(2), DateTo = DateTo };
                var param3 = new { Qtype = Convert.ToInt32(4), DateTo = DateTo };
                var param4 = new { Qtype = Convert.ToInt32(3), DateTo = DateTo };

                var POMIS1As = groupwiseReportService.GetDataPOMIS1_GroupAndMembersInfoConsolidationOfficewise(param1, "Proc_getPomis_1PKSFnonPKSF");
                var POMIS1Bs = groupwiseReportService.GetDataPOMIS1_SavingsStatementConsolidationOfficewise(param2, "Proc_getPomis_1PKSFnonPKSF");
                var POMIS1Cs = groupwiseReportService.GetDataPOMIS1_SavingsStatementItemWiseTotalConsolidationOfficewise(param3, "Proc_getPomis_1PKSFnonPKSF");
                var POMIS1Ds = groupwiseReportService.GetDataPOMIS1_AdmisionWithdrawanAttendanceConsolidationOfficewise(param4, "Proc_getPomis_1PKSFnonPKSF");

                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("SubRptSavingsStatement", POMIS1Bs.Tables[0]);
                subReportDB.Add("ItemWiseTotal", POMIS1Cs.Tables[0]);
                subReportDB.Add("SubRptMemberAdmission", POMIS1Ds.Tables[0]);
                ReportHelper.PrintWithSubReport("POMIS_1PKSFNonPKSF.rpt", POMIS1As.Tables[0], new Dictionary<string, object>(), subReportDB, new POMIS_1PKSFNonPKSF());

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GeneratePOMIS2HQConsolidateReport()
        {
            try
            {
                var param1 = new { Qtype = 5, Org = SessionHelper.LoginUserOrganizationID };
                var param2 = new { Qtype = 5, Org = SessionHelper.LoginUserOrganizationID };
                var POMIS1As = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param1, "Rpt_POMIS_Consolidation");
                var POMIS1Bs = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param2, "Rpt_POMIS_Consolidation");
                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("SubRptLoanStatementOverdue", POMIS1Bs.Tables[0]);
                ReportHelper.PrintWithSubReport("rpt_POMIS2_LoanStatementHQ.rpt", POMIS1As.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_POMIS2_LoanStatementHQ());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult ShowReport()
        {
            CrystalDecisions.Shared.ExportOptions crExportOptions = new CrystalDecisions.Shared.ExportOptions();
            CrystalDecisions.Shared.DiskFileDestinationOptions crDiskFileDestination = new CrystalDecisions.Shared.DiskFileDestinationOptions();
            string strFName;

            ReportClass reportClass = new rpt_POMIS2_LoanStatementHQ();

            var param1 = new { Qtype = 5, Org = SessionHelper.LoginUserOrganizationID };
            var param2 = new { Qtype = 5, Org = SessionHelper.LoginUserOrganizationID };
            var POMIS1As = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param1, "Rpt_POMIS_Consolidation");
            var POMIS1Bs = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param2, "Rpt_POMIS_Consolidation");
            var subReportDB = new Dictionary<string, DataTable>();
            subReportDB.Add("SubRptLoanStatementOverdue", POMIS1Bs.Tables[0]);

            string reportName = "rpt_POMIS2_LoanStatementHQ.rpt";
            DataTable dataSource = POMIS1As.Tables[0];

            string strReportPathAbsolute = System.Web.HttpContext.Current.Server.MapPath("~/Reports/" + reportName);
            reportClass.Load(strReportPathAbsolute);
            reportClass.SetDataSource(dataSource);

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> kvp in parameters)
            {
                reportClass.SetParameterValue(kvp.Key, kvp.Value);

            }

            Dictionary<string, DataTable> subReportDatasources = subReportDB;

            if (subReportDatasources != null)
            {
                foreach (KeyValuePair<string, DataTable> kvp in subReportDatasources)
                {
                    reportClass.OpenSubreport(kvp.Key).SetDataSource(kvp.Value);
                }
            }


            strFName = System.Web.HttpContext.Current.Server.MapPath("~/") + string.Format("{0}.pdf", Guid.NewGuid());
            crDiskFileDestination.DiskFileName = strFName;
            crExportOptions = reportClass.ExportOptions;
            crExportOptions.DestinationOptions = crDiskFileDestination;
            crExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
            crExportOptions.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
            reportClass.Export();
            Session["ReportDocument"] = reportClass;
            reportClass.Dispose();
            reportClass.Close();

            System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
            System.Web.HttpContext.Current.Response.WriteFile(strFName);
            System.Web.HttpContext.Current.Response.End();

            System.IO.File.Delete(strFName);
            reportClass.Close();
            reportClass.Dispose();

            return Redirect("/CrystalReport/ReportViewer.aspx");
        }

        public ActionResult GeneratePOMIS2HQConsolidateOfficewise(string Qtype, string OfficeId, string DateTo)
        {
            try
            {
                
                var param1 = new { Qtype = Convert.ToInt32(Qtype), OfficeID = Convert.ToInt32(OfficeId), Org = SessionHelper.LoginUserOrganizationID, DateTo = DateTo };
                var param2 = new { Qtype = Convert.ToInt32(Qtype), OfficeID = Convert.ToInt32(OfficeId), Org = SessionHelper.LoginUserOrganizationID, DateTo = DateTo };

                var POMIS1As = groupwiseReportService.POMISConsolidationOfficewise(param1, "Rpt_POMIS_ConsolidationOfficewise");
                var POMIS1Bs = groupwiseReportService.POMISConsolidationOfficewise(param2, "Rpt_POMIS_ConsolidationOfficewise");

                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("SubRptLoanStatementOverdue", POMIS1Bs.Tables[0]);


                ReportHelper.PrintWithSubReport("rpt_POMIS2_LoanStatementHQOfficewise.rpt", POMIS1As.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_POMIS2_LoanStatementHQOfficewise());

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }
        }
        public ActionResult GeneratePOMIS2HQConsolidateOfficewiseOnlyMaincategory(string Qtype, string OfficeId, string DateTo)
        {
            try
            {
                var param1 = new { Qtype = Convert.ToInt32(Qtype), OfficeID = Convert.ToInt32(OfficeId), Org = SessionHelper.LoginUserOrganizationID, DateTo = DateTo };
                var param2 = new { Qtype = Convert.ToInt32(Qtype), OfficeID = Convert.ToInt32(OfficeId), Org = SessionHelper.LoginUserOrganizationID, DateTo = DateTo };
                var POMIS1As = groupwiseReportService.POMISConsolidationOfficewise(param1, "Rpt_POMIS_ConsolidationOfficewiseOnlyMaincategory");
                var POMIS1Bs = groupwiseReportService.POMISConsolidationOfficewise(param2, "Rpt_POMIS_ConsolidationOfficewiseOnlyMaincategory");
                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("SubRptLoanStatementOverdue", POMIS1Bs.Tables[0]);
                ReportHelper.PrintWithSubReport("rpt_POMIS2_LoanStatementHQOfficewiseOnlyMaincategory.rpt", POMIS1As.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_POMIS2_LoanStatementHQOfficewiseOnlyMaincategory());

                return Content(string.Empty);

                //var param1 = new { Qtype = Convert.ToInt32(Qtype), OfficeID = Convert.ToInt32(OfficeId), Org = SessionHelper.LoginUserOrganizationID, DateTo = DateTo };
                //var param2 = new { Qtype = Convert.ToInt32(Qtype), OfficeID = Convert.ToInt32(OfficeId), Org = SessionHelper.LoginUserOrganizationID, DateTo = DateTo };
                //var POMIS1As = groupwiseReportService.POMISConsolidationOfficewise(param1, "Rpt_POMIS_ConsolidationOfficewiseOnlyMaincategory");
                //var POMIS1Bs = groupwiseReportService.POMISConsolidationOfficewise(param2, "Rpt_POMIS_ConsolidationOfficewiseOnlyMaincategory");
                //var subReportDB = new Dictionary<string, DataTable>();
                //subReportDB.Add("SubRptLoanStatementOverdue", POMIS1Bs.Tables[0]);
                //ReportHelper.PrintWithSubReport("rpt_POMIS2_LoanStatementHQOfficewiseOnlyMaincategory.rpt", POMIS1As.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_POMIS2_LoanStatementHQOfficewiseOnlyMaincategory());

                //return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }
        }
        public ActionResult GeneratePOMIS2HQConsolidatePKSFNonPKSF(string Qtype, string OfficeId, string DateTo)
        {
            try
            {

                var param1 = new { Qtype = Convert.ToInt32(Qtype), OfficeID = Convert.ToInt32(OfficeId), Org = SessionHelper.LoginUserOrganizationID, DateTo = DateTo };
                var param2 = new { Qtype = Convert.ToInt32(Qtype), OfficeID = Convert.ToInt32(OfficeId), Org = SessionHelper.LoginUserOrganizationID, DateTo = DateTo };
                var POMIS1As = groupwiseReportService.POMISConsolidationOfficewise(param1, "Rpt_POMIS_ConsolidationOfficewiseOnlyMaincategoryPKSFNonPKSF");
                var POMIS1Bs = groupwiseReportService.POMISConsolidationOfficewise(param2, "Rpt_POMIS_ConsolidationOfficewiseOnlyMaincategoryPKSFNonPKSF");
                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("SubRptLoanStatementOverdue", POMIS1Bs.Tables[0]);
                ReportHelper.PrintWithSubReport("POMIS_2PKSFNonPKSF.rpt", POMIS1As.Tables[0], new Dictionary<string, object>(), subReportDB, new POMIS_2PKSFNonPKSF());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }
        }
        public ActionResult GeneratePOMIS2HQConsolidateOfficewiseUsingBranchreport(string Qtype, string OfficeId, string DateTo)
        {
            try
            {
                var param1 = new { Qtype = Convert.ToInt32(Qtype), OfficeID = Convert.ToInt32(OfficeId), Org = SessionHelper.LoginUserOrganizationID, DateTo = DateTo };
                var param2 = new { Qtype = Convert.ToInt32(Qtype), OfficeID = Convert.ToInt32(OfficeId), Org = SessionHelper.LoginUserOrganizationID, DateTo = DateTo };

                var POMIS1As = groupwiseReportService.POMISConsolidationOfficewise(param1, "Rpt_POMIS_ConsolidationOfficewise");
                var POMIS1Bs = groupwiseReportService.POMISConsolidationOfficewise(param2, "Rpt_POMIS_ConsolidationOfficewise");

                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("SubRptLoanStatementOverdue", POMIS1Bs.Tables[0]);


                ReportHelper.PrintWithSubReport("rpt_POMIS2_LoanStatementHQOfficewise.rpt", POMIS1As.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_POMIS2_LoanStatementHQOfficewise());

                return Content(string.Empty);

                //var param1 = new { Qtype = Convert.ToInt32(Qtype), OfficeID = Convert.ToInt32(OfficeId), Org = SessionHelper.LoginUserOrganizationID, DateTo = DateTo };
                //var param2 = new { Qtype = Convert.ToInt32(Qtype), OfficeID = Convert.ToInt32(OfficeId), Org = SessionHelper.LoginUserOrganizationID, DateTo = DateTo };
                //var POMIS1As = groupwiseReportService.POMISConsolidationOfficewise(param1, "Rpt_POMIS_ConsolidationOfficewise");
                //var POMIS1Bs = groupwiseReportService.POMISConsolidationOfficewise(param2, "Rpt_POMIS_ConsolidationOfficewise");
                //var subReportDB = new Dictionary<string, DataTable>();
                //subReportDB.Add("SubRptLoanStatementOverdue", POMIS1Bs.Tables[0]);
                //ReportHelper.PrintWithSubReport("rpt_POMIS2_LoanStatement_UsingBranchreport.rpt", POMIS1As.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_POMIS2_LoanStatementHQOfficewise());

                //return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }
        }
        public ActionResult GeneratePOMIS_5_A_3_HQConsolidateReport(string Qtype, string DateTo)
        {
            try
            {
                var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, OfficeTo = SessionHelper.LoginUserOfficeID, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "Proc_Get_Rpt_POMIS_5_AConsolidation");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("POMIS_5_A_3_FinalHQ.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GeneratePOMIS_5_A_4_HQConsolidateReport(string Qtype, string DateTo)
        {
            try
            {
                var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, OfficeTo = SessionHelper.LoginUserOfficeID, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "Proc_Get_Rpt_POMIS_5_AConsolidation");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("POMIS_5_A_4_FinalHQ.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //POMIS 1 Excel Export ----------- (Mostak : 20 Dec 2023) 

        public ActionResult GenerateExcel_POMIS1HQConsolidateOfficewise(string Qtype, string OfficeId, string DateTo)
        {
            try
            {

                var param = new { Qtype = Convert.ToInt32(Qtype), OfficeId = OfficeId, DateTo = DateTo };
                var vOrgid = SessionHelper.LoginUserOrganizationID;
                if (vOrgid == 9)
                {
                    groupwiseReportService.GetDataPOMIS1_DataMarge(param, "POMIS2_SavingStatement_ConsolidationWithDate");
                }
                else
                {
                    groupwiseReportService.GetDataPOMIS1_DataMarge(param, "POMIS2_SavingStatement_ConsolidationWithDate");
                }

                if (vOrgid == 4) //DSK
                {
                    var param1 = new { Qtype = Convert.ToInt32(1), DateTo = DateTo, OfficeId = Convert.ToInt32(OfficeId) };
                    var param2 = new { Qtype = Convert.ToInt32(2), DateTo = DateTo, OfficeId = Convert.ToInt32(OfficeId) };
                    var param3 = new { Qtype = Convert.ToInt32(4), DateTo = DateTo, OfficeId = Convert.ToInt32(OfficeId) };
                    var param4 = new { Qtype = Convert.ToInt32(3), DateTo = DateTo, OfficeId = Convert.ToInt32(OfficeId) };

                    DataSet POMIS1As = groupwiseReportService.GetDataPOMIS1_GroupAndMembersInfoConsolidationOfficewise(param1, "Proc_getPomis_1");
                    DataSet POMIS1Bs = groupwiseReportService.GetDataPOMIS1_SavingsStatementConsolidationOfficewise(param2, "Proc_getPomis_1");
                    DataSet POMIS1Cs = groupwiseReportService.GetDataPOMIS1_SavingsStatementItemWiseTotalConsolidationOfficewise(param3, "Proc_getPomis_1");
                    DataSet POMIS1Ds = groupwiseReportService.GetDataPOMIS1_AdmisionWithdrawanAttendanceConsolidationOfficewise(param4, "Proc_getPomis_1");

                    var subReportDB = new Dictionary<string, DataTable>();
                    subReportDB.Add("SubRptSavingsStatement", POMIS1Bs.Tables[0]);
                    subReportDB.Add("ItemWiseTotal", POMIS1Cs.Tables[0]);
                    subReportDB.Add("SubRptMemberAdmission", POMIS1Ds.Tables[0]);
                    //ReportHelper.PrintWithSubReport("POMIS_1HQ.rpt", POMIS1As.Tables[0], new Dictionary<string, object>(), subReportDB, new POMIS_1HQ());
                    ReportHelper.ExportExcelWithSubReportDataOnly("POMIS_1HQ_ForExcel.rpt", POMIS1As.Tables[0], new Dictionary<string, object>(), subReportDB);
                    return Content(string.Empty);
                }
                else
                {
                    var param1 = new { Qtype = Convert.ToInt32(1), DateTo = DateTo };
                    var param2 = new { Qtype = Convert.ToInt32(2), DateTo = DateTo };
                    var param3 = new { Qtype = Convert.ToInt32(4), DateTo = DateTo };
                    var param4 = new { Qtype = Convert.ToInt32(3), DateTo = DateTo };

                    var POMIS1As = groupwiseReportService.GetDataPOMIS1_GroupAndMembersInfoConsolidationOfficewise(param1, "Proc_getPomis_1");
                    var POMIS1Bs = groupwiseReportService.GetDataPOMIS1_SavingsStatementConsolidationOfficewise(param2, "Proc_getPomis_1");
                    var POMIS1Cs = groupwiseReportService.GetDataPOMIS1_SavingsStatementItemWiseTotalConsolidationOfficewise(param3, "Proc_getPomis_1");
                    var POMIS1Ds = groupwiseReportService.GetDataPOMIS1_AdmisionWithdrawanAttendanceConsolidationOfficewise(param4, "Proc_getPomis_1");
                    var subReportDB = new Dictionary<string, DataTable>();
                    subReportDB.Add("SubRptSavingsStatement", POMIS1Bs.Tables[0]);
                    subReportDB.Add("ItemWiseTotal", POMIS1Cs.Tables[0]);
                    subReportDB.Add("SubRptMemberAdmission", POMIS1Ds.Tables[0]);
                    ReportHelper.ExportExcelWithSubReportDataOnly("POMIS_1HQ_ForExcel.rpt", POMIS1As.Tables[0], new Dictionary<string, object>(), subReportDB);
                    //ReportHelper.PrintWithSubReport("POMIS_1HQ.rpt", POMIS1As.Tables[0], new Dictionary<string, object>(), subReportDB, new POMIS_1HQ());
                    return Content(string.Empty);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        //POMIS 2 Excel Export ----------- (Mostak : 20 Dec 2023)

        public ActionResult GenerateExcel_POMIS2HQConsolidateOfficewise(string Qtype, string OfficeId, string DateTo)
        {
            try
            {

                var param1 = new { Qtype = Convert.ToInt32(Qtype), OfficeID = Convert.ToInt32(OfficeId), Org = SessionHelper.LoginUserOrganizationID, DateTo = DateTo };
                var param2 = new { Qtype = Convert.ToInt32(Qtype), OfficeID = Convert.ToInt32(OfficeId), Org = SessionHelper.LoginUserOrganizationID, DateTo = DateTo };

                var POMIS1As = groupwiseReportService.POMISConsolidationOfficewise(param1, "Rpt_POMIS_ConsolidationOfficewise");
                var POMIS1Bs = groupwiseReportService.POMISConsolidationOfficewise(param2, "Rpt_POMIS_ConsolidationOfficewise");

                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("SubRptLoanStatementOverdue", POMIS1Bs.Tables[0]);
                ReportHelper.ExportExcelWithSubReportDataOnly("rpt_POMIS2_LoanStatementHQOfficewise__ForExcel.rpt", POMIS1As.Tables[0], new Dictionary<string, object>(), subReportDB);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }
        }


        //POMIS 3 Excel Export ----------- (Mostak : 20 Dec 2023)

        public ActionResult GenerateExcel_POMIS3_OfficeWiseReport(string Qtype, string OfficeId, string DateTo)
        {
            try
            {
                var param = new { Qtype = Convert.ToInt32(Qtype), OfficeID = Convert.ToInt32(OfficeId), Org = SessionHelper.LoginUserOrganizationID, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "Rpt_POMIS3_ConsolidationOfficewise");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.ExportExcelDataOnly("OverDueAgeingOfficewise__ForExcel.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        public ActionResult QuarterlyBalanceReportDecline_Passbook()
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

            return View();
        }



        public ActionResult GenerateQuarterlyBalanceReportDecline_Passbook(string Qtype, string DateTo)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = DateTo };

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                if (Qtype == "1")
                {
                    var alldata = groupwiseReportService.GetDataBalanceCompareMemberWise(param, "Rpt_LoanAndSavingsBalanceCompareMemberWise_WithDUE_PASSBOOK");
                    ReportHelper.PrintReport("rptLoan_SavingsBalanceDecline_Passbook.rpt", alldata.Tables[0], reportParam);
                }
                else if (Qtype == "2")
                {
                    //var alldata = groupwiseReportService.GetDataBalanceCompareCenterWise(param, "Rpt_LoanAndSavingsBalanceCompareSamityWise");
                    var alldata = groupwiseReportService.GetDataBalanceCompareCenterWise(param, "Rpt_LoanAndSavingsBalanceCompareSamityWise_WithDUE_PASSBOOK");
                    ReportHelper.PrintReport("rptLoan_SavingsBalanceDeclineSamitywise_Passbook.rpt", alldata.Tables[0], reportParam);
                }
                else if (Qtype == "3")
                {
                    //var alldata = groupwiseReportService.GetDataBalanceCompareStaffWise(param, "Rpt_LoanAndSavingsBalanceCompareStaffWise");
                    var alldata = groupwiseReportService.GetDataBalanceCompareStaffWise(param, "Rpt_LoanAndSavingsBalanceCompareStaffWise_WithDUE_PASSBOOK");
                    ReportHelper.PrintReport("rptLoan_SavingsBalanceDeclineStaffwise_Passbook.rpt", alldata.Tables[0], reportParam);
                }
                else if (Qtype == "4")
                {
                    //var alldata = groupwiseReportService.GetDataBalanceCompareOfficeWise(param, "Rpt_LoanAndSavingsBalanceCompareOfficeWise");
                    var alldata = groupwiseReportService.GetDataBalanceCompareOfficeWise(param, "Rpt_LoanAndSavingsBalanceCompareOfficeWise_WithDUE_PASSBOOK");
                    ReportHelper.PrintReport("rptLoan_SavingsBalanceDeclineBranchwise_Passbook.rpt", alldata.Tables[0], reportParam);
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //string Qtype, string DateTo, int center
        //public ActionResult GenerateQuarterlyBalanceReportDecline(string Qtype, string DateTo)
        public ActionResult GenerateQuarterlyBalanceReportDecline(string Qtype, string DateTo, int center)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Center = center, Office = SessionHelper.LoginUserOfficeID, DateTo = DateTo };

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                if (Qtype == "1")
                {
                    var alldata = groupwiseReportService.GetDataBalanceCompareMemberWise_MemberWise(param, "Rpt_LoanAndSavingsBalanceCompareMemberWise_WithDUECenter");
                    ReportHelper.PrintReport("rptLoan_SavingsBalanceDeclineCenter.rpt", alldata.Tables[0], reportParam);
                }
                else if (Qtype == "2")
                {
                    //var alldata = groupwiseReportService.GetDataBalanceCompareCenterWise(param, "Rpt_LoanAndSavingsBalanceCompareSamityWise");
                    var alldata = groupwiseReportService.GetDataBalanceCompareCenterWise(param, "Rpt_LoanAndSavingsBalanceCompareSamityWise_WithDUECenter");
                    ReportHelper.PrintReport("rptLoan_SavingsBalanceDeclineSamitywiseCenter.rpt", alldata.Tables[0], reportParam);
                }
                else if (Qtype == "3")
                {
                    //var alldata = groupwiseReportService.GetDataBalanceCompareStaffWise(param, "Rpt_LoanAndSavingsBalanceCompareStaffWise");
                    var alldata = groupwiseReportService.GetDataBalanceCompareStaffWise(param, "Rpt_LoanAndSavingsBalanceCompareStaffWise_WithDUECenter");
                    ReportHelper.PrintReport("rptLoan_SavingsBalanceDeclineStaffwiseCenter.rpt", alldata.Tables[0], reportParam);
                }
                else if (Qtype == "4")
                {
                    //var alldata = groupwiseReportService.GetDataBalanceCompareOfficeWise(param, "Rpt_LoanAndSavingsBalanceCompareOfficeWise");
                    var alldata = groupwiseReportService.GetDataBalanceCompareOfficeWise(param, "Rpt_LoanAndSavingsBalanceCompareOfficeWise_WithDUECenter");
                    ReportHelper.PrintReport("rptLoan_SavingsBalanceDeclineBranchwiseCenter.rpt", alldata.Tables[0], reportParam);
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateDailyRecoverableReceipt(string Qtype, string DateTo)
        {
            try
            {
                var param = new { Office = SessionHelper.LoginUserOfficeID, Date = DateTo, QType = Qtype, EmployeeID = LoggedInEmployeeID };
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("DateTo", DateTo);
                var orgID = SessionHelper.LoginUserOrganizationID;
                var param1 = new { @EmpID = LoggedInEmployeeID };
                var LoanInstallMent = ultimateReportService.GetCenterROleWise(param1);
                DataSet alldata;
                if (orgID == 4)
                {


                    alldata = groupwiseReportService.GetDataDSKDailyReceipt(param, "Rpt_DailyCollectionReceiptFoWise_New");
                    if (Qtype == "1")
                    {
                        ReportHelper.PrintReport("rpt_CollectionReceiptEmployeeWise_DSK.rpt", alldata.Tables[0], reportParam);
                    }
                    else if (Qtype == "2")
                    {
                        ReportHelper.PrintReport("rpt_DailyCollectionReceiptSamityWise_DSK.rpt", alldata.Tables[0], reportParam);
                    }

                }
                else
                {
                    alldata = groupwiseReportService.GetDataDSKDailyReceipt(param, "Rpt_DailyCollectionReceiptFoWise");
                    if (Qtype == "1")
                    {
                        ReportHelper.PrintReport("rpt_CollectionReceiptEmployeeWise.rpt", alldata.Tables[0], reportParam);
                    }
                    else if (Qtype == "2")
                    {
                        ReportHelper.PrintReport("rpt_DailyCollectionReceiptSamityWise.rpt", alldata.Tables[0], reportParam);
                    }
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        // FDR - NextInterest payable
        public ActionResult NextInterestPayable()
        {
            return View();
        }

        public ActionResult GenerateNextInterestPayableReport(int qType, string TrxDate, string DateFrom, string DateTo)
        {
            try
            {
                var officeId = SessionHelper.LoginUserOfficeID;

                var param = new { OfficeID = officeId, TransDate = TrxDate, QType = qType, DateFrom = DateFrom, DateTo = DateTo };
                var alldata = ultimateReportService.GetDataWithParameter(param, "Proc_NextInterestPayable");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("TrxDate", TrxDate);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("NextInterestPayableReport.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateDailyRecoverableReceipt_FOWise(string Qtype, string DateTo)
        {
            try
            {
                var param = new { Office = SessionHelper.LoginUserOfficeID, Date = DateTo, QType = Qtype, EmployeeID = LoggedInEmployeeID };
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("DateTo", DateTo);
                var orgID = SessionHelper.LoginUserOrganizationID;
                var param1 = new { @EmpID = LoggedInEmployeeID };
                var LoanInstallMent = ultimateReportService.GetCenterROleWise(param1);
                DataSet alldata;
                if (orgID == 4)
                {
                    var empType = LoanInstallMent.Tables[0].Rows[0]["Name"].ToString();
                    if (empType == "FO")
                    {
                        alldata = groupwiseReportService.GetDataDSKDailyReceipt(param, "FOCollectionSummary");
                        if (Qtype == "1")
                        {

                            ReportHelper.PrintReport("rpt_CollectionReceiptEmployeeWise_DSKFOWise.rpt", alldata.Tables[0], reportParam);
                        }
                        else if (Qtype == "2")
                        {

                            ReportHelper.PrintReport("rpt_DailyCollectionReceiptSamityWise_DSK_FOWise.rpt", alldata.Tables[0], reportParam);
                        }
                    }
                    else
                    {
                        alldata = groupwiseReportService.GetDataDSKDailyReceipt(param, "FOCollectionSummary");
                        if (Qtype == "1")
                        {

                            ReportHelper.PrintReport("rpt_CollectionReceiptEmployeeWise_DSKFOWise.rpt", alldata.Tables[0], reportParam);
                        }
                        else if (Qtype == "2")
                        {

                            ReportHelper.PrintReport("rpt_DailyCollectionReceiptSamityWise_DSK_FOWise.rpt", alldata.Tables[0], reportParam);
                        }
                    }
                }
                else
                {
                    alldata = groupwiseReportService.GetDataDSKDailyReceipt(param, "Rpt_DailyCollectionReceiptFoWise");
                    if (Qtype == "1")
                    {
                        ReportHelper.PrintReport("rpt_CollectionReceiptEmployeeWise.rpt", alldata.Tables[0], reportParam);
                    }
                    else if (Qtype == "2")
                    {
                        ReportHelper.PrintReport("rpt_DailyCollectionReceiptSamityWise.rpt", alldata.Tables[0], reportParam);
                    }
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateQuarterlyBalanceReportDeclineConsolidate(string Qtype, string DateTo)
        {
            try
            {
                var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = 00, OfficeTo = 99999, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "Rpt_LoanAndSavingsBalanceCompareConsolidate");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptLoan_SavingsBalanceDeclineBranchWiseConsolidate.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateQuarterlyBalanceReportFlat(string Qtype, string DateTo)
        {
            try
            {
                var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "Rpt_LoanAndSavingsBalanceCompare ");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptLoan_SavingsBalanceFlat.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //public ActionResult GenerateMonthlySavingsCollectionSheetReport(string DateTo)
        //{
        //    try
        //    {
        //        var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = DateTo };
        //        var alldata = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "Rpt_MonthlySavingsCollectionSheet");
        //        var reportParam = new Dictionary<string, object>();
        //        reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //        reportParam.Add("DateTo", DateTo);
        //        ReportHelper.PrintReport("rptMonthlySavingsCollectionSheet.rpt", alldata.Tables[0], reportParam);
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}

        public ActionResult GenerateMonthlySavingsCollectionSheetReport(string DateTo, string ProductCode)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, @ItemCode = ProductCode, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "Rpt_MonthlySavingsCollectionSheet");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                reportParam.Add("ItemCode", ProductCode);
                ReportHelper.PrintReport("rptMonthlySavingsCollectionSheet.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateStaffProfileReport(string DateTo)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "Rpt_StaffProfile");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rpt_POMIS3B_StaffProfile.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateAdvanceAdjustmentReport()
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Proc_GetRpt_FullyRepaid_Advance");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("rptAdvanceAdjustment.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateAdvanceAdjustmentDateRangeReport(string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Proc_GetRpt_FullyRepaid_Advance_DateRange");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptAdvanceAdjustmentDateRange.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateServiceChargeStatement(string DateFrom, string DateTo)
        {
            try
            {
                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataServiceChargeStatement(param, "Proc_RPT_BuroMonthlyLoanAndSchargeInformation");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("rptMonthlyServiceChargeStatement.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateSavingPortfolioStatement(string DateFrom, string DateTo, string Qtype)
        {
            try
            {
                Qtype = "1";
                var reportParam = new Dictionary<string, object>();
                var param = new { Qtype = Qtype, OfficeID = SessionHelper.LoginUserOfficeID, EmpID = 0, DateFrom = DateFrom, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportSavingStatement(param, "Proc_RPT_BuroSavimngsPortfolioStatement");
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                var paramB = new { OrgId = LoggedInOrganizationID, OfficeId = SessionHelper.LoginUserOfficeID, DateTo = DateTo, QType = 1 };
                var POMIS1Bs = groupwiseReportService.GetDataResizeReport(paramB, "RPT_SavingsSize_Buro");
                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("DistributionOfSavingsSize", POMIS1Bs.Tables[0]);
                ReportHelper.PrintWithSubReport("rptBranchWiseSavingsPortfolioStatement.rpt", alldata.Tables[0], new Dictionary<string, object>(), subReportDB, new rptBranchWiseSavingsPortfolioStatement());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateAccountCloseInformationReport(string Qtype, string Center, string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, DateFrom = DateFrom, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_AccountCloseInformation");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptAccountCloseInformation.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateMonthWiseCollectionSheetFortnightlyReport(string DateTo)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "Rpt_CollectionSheetFortnightly");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptMonthWiseCollectionSheetFortnightly.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateLoanApprovalReportGroupwise(string Qtype, string Center, string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, DateFrom = DateFrom, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_LoanApproval");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptLoanApprovalGroupwise.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateMemberBalanceInfoReport(string Center, string Member, string DateTo)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, Member = Member, DateTo = DateTo };
                var alldata = groupwiseReportService.GetMemberBalanceInfoReport(param, "Rpt_MemberBalanceInfo");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptMemberBalanceInfo.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateQuarterlySupplimentary2ReportConsolidationNew(string DateFrom, string DateTo, string Qtype, string OfficeId)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = OfficeId, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_Supplementary_Consolidation_2");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rpt_Supplementary_ConsolidationReplica_2.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult QuarterlySupplimentaryConsolidationNew_2()
        {

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;

            //var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoggedInEmployee.OfficeID));
            //var HeadOfficeCode = offcdetail.FirstLevel;
            //var Headoffcdetail = officeService.GetByOfficeCode(HeadOfficeCode);
            //var Secondoffcdetail = officeService.GetByOfficeCode(offcdetail.SecondLevel);
            //var Thirdoffcdetail = officeService.GetByOfficeCode(offcdetail.ThirdLevel);
            //var Fourthoffcdetail = officeService.GetByOfficeCode(offcdetail.FourthLevel);

            //ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
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
            var param = new { @OfficeID = SessionHelper.LoggedInEmployee.OfficeID };
            var allProducts = accReportService.GetLastInitialDate(param);
            var detail = allProducts.ToString();
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
        public ActionResult BranchWiseProfitLoss()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public JsonResult GetBranchWiseProfitLoss(int jtStartIndex, int jtPageSize, string jtSorting, int officeId, string dateFrom, string dateTo)
        {
            try
            {
                var param = new { OfficeID = officeId, DateFrom = dateFrom, DateTo = dateTo };

                var queryResponse = ultimateReportService.GetDataWithParameter(param, "Proc_GetBranchWiseProfitLoss");

                var assignSpToVM = queryResponse.Tables[0].AsEnumerable()
                    .Select(row => new ProfitLossViewModel
                    {
                        OfficeID = row.Field<int>("OfficeID"),
                        ZoneCode = row.Field<string>("ZoneCode"),
                        AreaCode = row.Field<string>("AreaCode"),
                        AreaName = row.Field<string>("AreaName"),
                        BranchCode = row.Field<string>("BranchCode"),
                        BranchName = row.Field<string>("BranchName"),
                        ProfitLossCurrentMonth = row.Field<decimal>("ProfitLossCurrentMonth"),
                        ProfitLossThisYear = row.Field<decimal>("ProfitLossThisYear"),
                        TrxDateMax = row.Field<DateTime>("TrxDateMax"),

                    }).ToList();
                var currentPageRecords = assignSpToVM.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = assignSpToVM.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public ActionResult GenerateMemberCardReport(string Center, string Member, string DateTo)
        {
            try
            {
                var org = SessionHelper.LoginUserOrganizationID;
                int Office_TO = Convert.ToInt16(SessionHelper.LoginUserOfficeID);
                var paramValues = new List<ParameterValue>();
                paramValues.Add(new ParameterValue() { Name = "Org", Value = Convert.ToString(org) });
                paramValues.Add(new ParameterValue() { Name = "Office", Value = Convert.ToString(Office_TO) });
                paramValues.Add(new ParameterValue() { Name = "Center", Value = Convert.ToString(Center) });
                paramValues.Add(new ParameterValue() { Name = "Member", Value = Convert.ToString(Member) });
                paramValues.Add(new ParameterValue() { Name = "DateTo", Value = DateTo });
                PrintSSRSReport("/gBanker_Reports/MemberInfoWithLoanAndSavingsBalance", paramValues.ToArray(), "gBankerReport");
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateGroupLedgerLoanSamitywise(string Qtype, string Center, string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, DateFrom = DateFrom, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportGroupLedgerSavingSamitywise(param, "Rpt_GroupLedgerLoanSamitywise");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptLoanLedgerSamityWise.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateSavingBalanceInfoReport(string Product, string DateFrom, string DateTo)
        {
            try
            {
                var setProd = "0";
                var setProdTo = "0";
                if (Product == "0")
                {
                    setProd = "00";
                    setProdTo = "ZZ";
                }
                else
                {
                    setProd = Product;
                    setProdTo = Product;
                }
                //var param = new { Qtype = Product, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, DateFrom = DateFrom, DateTo = DateTo };
                //var alldata = groupwiseReportService.GetDataUltimateReleaseReportGroupLedgerSavingSamitywise(param, "Rpt_GroupLedgerLoanSamitywise");
                //var reportParam = new Dictionary<string, object>();
                //reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("DateFrom", DateFrom);
                //reportParam.Add("DateTo", DateTo);
                // ReportHelper.PrintReport("rptLoanLedgerSamityWise.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateGroupLedgerSavingSamitywise(string Qtype, string Center, string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, DateFrom = DateFrom, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportGroupLedgerSavingSamitywise(param, "Rpt_GroupLedgerSavingSamitywise");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptSavingLedgerSamityWise.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //public ActionResult GenerateLoanLedgerMemberwise(string Center, string Member, string DateFrom, string DateTo)
        //{
        //    try
        //    {
        //        var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, Member = Member, DateFrom = DateFrom, DateTo = DateTo };
        //        var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_LoanLedgerMemberwise");
        //        var reportParam = new Dictionary<string, object>();
        //        reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //        reportParam.Add("DateFrom", DateFrom);
        //        reportParam.Add("DateTo", DateTo);
        //        ReportHelper.PrintReport("rptLoanLedgerLoaneeWise.rpt", alldata.Tables[0], reportParam);
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        public ActionResult GenerateLoanLedgerMemberwise(string Qtype, string Center, string Member, string ProductID, string LoanTerm, string DateFrom, string DateTo)
        {
            try
            {
                DataSet alldata;
                var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, Member = Member, ProductID = ProductID, LoanTerm = LoanTerm, DateFrom = DateFrom, DateTo = DateTo };

                if (LoggedInOrganizationID == 218)
                {
                    alldata = groupwiseReportService.GetDataUltimateReleaseReportGroupLedgerSavingSamitywise_loan(param, "Proc_Rpt_LoanLedgerMemberwise_SouthAfrica");
                }
               else
                {
                    alldata = groupwiseReportService.GetDataUltimateReleaseReportGroupLedgerSavingSamitywise_loan(param, "Rpt_LoanLedgerMemberwise_24052016");
                }
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                if (LoggedInOrganizationID == 218)
                {
                    ReportHelper.PrintReport("rptLoanLedgerLoaneeWise_SouthAfrica.rpt", alldata.Tables[0], reportParam);
                }
                else
                {
                    ReportHelper.PrintReport("rptLoanLedgerLoaneeWise.rpt", alldata.Tables[0], reportParam);
                }
               
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateSpecialLoanCollectionInfo(string DateFrom, string DateTo)
        {
            try
            {
                var param = new { officeID = SessionHelper.LoginUserOfficeID, StartDate = DateFrom, EndDate = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportGroupLedgerSavingSamitywise(param, "Proc_Rpt_SpecialLoanCollection");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("DateFrom", DateFrom);
                //reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("SpecialLoanCollectionInfo.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateSpecialSavingCollectionInfo(string DateFrom, string DateTo)
        {
            try
            {
                var param = new { officeID = SessionHelper.LoginUserOfficeID, StartDate = DateFrom, EndDate = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportGroupLedgerSavingSamitywise(param, "Proc_Rpt_SpecialSavingsCollection");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("DateFrom", DateFrom);
                //reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("SpecialSavingCollection.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //public ActionResult GenerateSavingLedgerMemberwise(string Center, string Member, string DateFrom, string DateTo)
        //{
        //    try
        //    {
        //        var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, Member = Member, DateFrom = DateFrom, DateTo = DateTo };
        //        var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_SavingLedgerMemberwise");
        //        var reportParam = new Dictionary<string, object>();
        //        reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //        reportParam.Add("DateFrom", DateFrom);
        //        reportParam.Add("DateTo", DateTo);
        //        ReportHelper.PrintReport("rptSavingLedgerLoaneeWise.rpt", alldata.Tables[0], reportParam);
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        public ActionResult GenerateSavingLedgerMemberwise(string Qtype, string Center, string Member, string ProductID, string NoOfAccount, string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, Member = Member, ProductID = ProductID, NoOfAccount = NoOfAccount, DateFrom = DateFrom, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportGroupLedgerSavingSamitywise_loan(param, "Rpt_SavingLedgerMemberwise_24052016");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptSavingLedgerLoaneeWise.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateSavingsRecoveryRegisterCenterWise(int Center, int product, string DateFrom, string DateTo, string Qtype)
        {
            try
            {
                if (Qtype == "1")
                {
                    var param = new { Center = Center, product = product, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Org = SessionHelper.LoginUserOrganizationID, Qtype = Qtype };
                    var alldata = groupwiseReportService.GetDataUltimateLedgerSavingCenterwise(param, "getSavingBalanceInfo_CenterWise");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("DateFrom", DateFrom);
                    reportParam.Add("DateTo", DateTo);
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    ReportHelper.PrintReport("rptSavingLedgerCenterWiseNew.rpt", alldata.Tables[0], reportParam);
                    return Content(string.Empty);
                }
                else
                {
                    var param = new { Center = Center, product = product, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Org = SessionHelper.LoginUserOrganizationID, Qtype = Qtype };
                    var alldata = groupwiseReportService.GetDataUltimateLedgerSavingCenterwise(param, "getSavingBalanceInfo_CenterWise");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("DateFrom", DateFrom);
                    reportParam.Add("DateTo", DateTo);
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    ReportHelper.PrintReport("rptSavingLedgerWithoutCenterWiseNew.rpt", alldata.Tables[0], reportParam);
                    return Content(string.Empty);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateLoanSavingAccountInfoCenterWise(int Center, string Qtype)
        {
            try
            {
                var param = new { Center = Center, Office = SessionHelper.LoginUserOfficeID, Org = SessionHelper.LoginUserOrganizationID, Qtype = Qtype };
                var alldata = groupwiseReportService.GetDataUltimateLoanSavingAccountInfoCenterWise(param, "LoanSavingAccountInfoCenterWise");
                var reportParam = new Dictionary<string, object>();
                //reportParam.Add("DateFrom", DateFrom);
                //reportParam.Add("DateTo", DateTo);
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("rptLoanSavingAccountInfoCenterWise.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateProcessInfoReport()

        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID };
                var alldata = groupwiseReportService.GetDataProcessLOg(param, "Rpt_ProcessInfo");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("rptProcessList.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateProcessInfoBranchwiseReport()
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = 00, OfficeTo = 9999 };
                // var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_ProcessInfoBranchwiseGenerateHOOfficeWiseProgramMISReport");
                var alldata = groupwiseReportService.GetDataProcessLOg(param, "Rpt_ProcessInfoBranchwise");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("rptProcessListBranchwise.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GenerateMemberAllLoanInformationReport(string Center, string Member)
        {
            try
            {
                var param = new { Office = SessionHelper.LoginUserOfficeID, Center = Center, Member = Member, Org = SessionHelper.LoginUserOrganizationID };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_MembersLoanInformationReport");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("MembersAllLoanInformation.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //Added By Shakhawat  
        public ActionResult GenerateSavingWithdrawalMemberwise(int Qtype, string Center, string Member, string DateFrom, string DateTo, string ExportType)
        {
            try
            {
                int? Org = SessionHelper.LoginUserOrganizationID;
                if (Org == 94)
                {
                    var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, Member = Member, DateFrom = DateFrom, DateTo = DateTo };
                    var alldata = groupwiseReportService.GetDataUltimateReleaseReportFromReportServer(param, "Rpt_SavingsWithdrawalLoaneeWiseTranserWise");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateFrom", DateFrom);
                    reportParam.Add("DateTo", DateTo);
                    if (ExportType == "pdf")
                        ReportHelper.PrintReport("rptSavingsWithdrawalLoaneeTranseWise.rpt", alldata.Tables[0], reportParam);
                    else
                        ReportHelper.ExportExcelReportFormated("rptSavingsWithdrawalLoaneeTranseWise.rpt", alldata.Tables[0], reportParam, "SavingsWithdrawalLoaneeTranseWise");

                    return Content(string.Empty);
                }
                else
                {

                    var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, Member = Member, DateFrom = DateFrom, DateTo = DateTo };
                    var alldata = groupwiseReportService.GetDataUltimateReleaseReportFromReportServer(param, "Rpt_SavingsWithdrawalLoaneeWise");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateFrom", DateFrom);
                    reportParam.Add("DateTo", DateTo);
                    if (ExportType == "pdf")
                        ReportHelper.PrintReport("rptSavingsWithdrawalLoaneeWise.rpt", alldata.Tables[0], reportParam);
                    else
                        ReportHelper.ExportExcelReportFormated("rptSavingsWithdrawalLoaneeWise.rpt", alldata.Tables[0], reportParam, "SavingsWithdrawalLoaneeWise");

                    return Content(string.Empty);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //public ActionResult GenerateSavingWithdrawalMemberwise(int Qtype, string Center, string Member, string DateFrom, string DateTo)
        //{
        //    try
        //    {
        //        int? Org = SessionHelper.LoginUserOrganizationID;
        //        if (Org == 94)
        //        {
        //            var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, Member = Member, DateFrom = DateFrom, DateTo = DateTo };
        //            var alldata = groupwiseReportService.GetDataUltimateReleaseReportFromReportServer(param, "Rpt_SavingsWithdrawalLoaneeWiseTranserWise");
        //            var reportParam = new Dictionary<string, object>();
        //            reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //            reportParam.Add("DateFrom", DateFrom);
        //            reportParam.Add("DateTo", DateTo);
        //            ReportHelper.PrintReport("rptSavingsWithdrawalLoaneeTranseWise.rpt", alldata.Tables[0], reportParam);
        //            return Content(string.Empty);
        //        }
        //        else
        //        {

        //            var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, Member = Member, DateFrom = DateFrom, DateTo = DateTo };
        //            var alldata = groupwiseReportService.GetDataUltimateReleaseReportFromReportServer(param, "Rpt_SavingsWithdrawalLoaneeWise");
        //            var reportParam = new Dictionary<string, object>();
        //            reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //            reportParam.Add("DateFrom", DateFrom);
        //            reportParam.Add("DateTo", DateTo);
        //            ReportHelper.PrintReport("rptSavingsWithdrawalLoaneeWise.rpt", alldata.Tables[0], reportParam);
        //            return Content(string.Empty);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        public ActionResult GenerateSavingCloseMemberwise(string Center, string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, DateFrom = DateFrom, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportFromReportServer(param, "Rpt_SavingsCloseInformationLoaneeWise");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptSavingsCloseLoaneeWise.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateSavingWithdrawalMemberwiseBeforeDayEnd(int Qtype, string Center, string Member)
        {
            try
            {
                string DateFrom = "01 Jan 1900";
                string DateTo = "01 Jan 1900";
                var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, Member = Member, DateFrom = DateFrom, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_SavingsWithdrawalLoaneeWiseBeforeDayEnd");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptSavingsWithdrawalLoaneeWise.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateSavingWithdrawal(int Qtype, string Center, string Member, string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, Member = Member, DateFrom = DateFrom, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportFromReportServer(param, "Rpt_SavingsWithdrawalLoaneeWise");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptSavingsWithdrawal.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateLoanDisburseProductWise(int Qtype, string Center, string product, string DateFrom, string DateTo)
        {
            try
            {
                var OrgID = SessionHelper.LoginUserOrganizationID;
                if (OrgID == 4)
                {
                    var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, product = product, DateFrom = DateFrom, DateTo = DateTo };
                    var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GenerateStaffWiseSpecialSavingsReportJCF_ProductWise");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateFrom", DateFrom);
                    reportParam.Add("DateTo", DateTo);
                    ReportHelper.PrintReport("rptLoanDisburseNewProductWise.rpt", alldata.Tables[0], reportParam);
                    return Content(string.Empty);
                }
                else
                {
                    var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, product = product, DateFrom = DateFrom, DateTo = DateTo };
                    var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GenerateStaffWiseSpecialSavingsReportJCF_ProductWise");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateFrom", DateFrom);
                    reportParam.Add("DateTo", DateTo);
                    ReportHelper.PrintReport("rptLoanDisburseProductWise.rpt", alldata.Tables[0], reportParam);
                    return Content(string.Empty);
                }


            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateLoanDisburse(int Qtype, string Center, string Member, string DateFrom, string DateTo,string MainProductCode)
        {
            try
            {
                var OrgID = SessionHelper.LoginUserOrganizationID;
                if (OrgID == 4)
                {
                    var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, Member = Member, DateFrom = DateFrom, DateTo = DateTo };
                    var alldata = groupwiseReportService.GetLoanDisburse(param, "GenerateStaffWiseSpecialSavingsReportJCF");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateFrom", DateFrom);
                    reportParam.Add("DateTo", DateTo);
                    ReportHelper.PrintReport("rptLoanDisburseNew.rpt", alldata.Tables[0], reportParam);
                    return Content(string.Empty);
                }
                else if (OrgID == 105)
                {
                    var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, Member = Member, DateFrom = DateFrom, DateTo = DateTo, MainProductCode = MainProductCode };
                    //var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GenerateStaffWiseSpecialSavingsReportJCF");
                    var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GenerateStaffWiseSpecialSavingsReportJCFNew");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateFrom", DateFrom);
                    reportParam.Add("DateTo", DateTo);
                    reportParam.Add("MainProductCode", MainProductCode);
                    ReportHelper.PrintReport("rptLoanDisburse_SDC.rpt", alldata.Tables[0], reportParam);
                    //  ReportHelper.PrintExcelReport("rptLoanDisburse.rpt", alldata.Tables[0], reportParam);
                    return Content(string.Empty);

                }
                else if (OrgID == 12) //=== GUK: Mostak : 29 Feb 2024=====//
                {
                    var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, Member = Member, DateFrom = DateFrom, DateTo = DateTo, MainProductCode = MainProductCode };
                    var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GenerateStaffWiseLoanDisbursementRegister");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateFrom", DateFrom);
                    reportParam.Add("DateTo", DateTo);
                    reportParam.Add("MainProductCode", MainProductCode);
                    ReportHelper.PrintReport("rptLoanDisburse_SDC__ApproveBy.rpt", alldata.Tables[0], reportParam);
                    return Content(string.Empty);
                }

                else
                {
                    var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, Member = Member, DateFrom = DateFrom, DateTo = DateTo, MainProductCode = MainProductCode };
                    //var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GenerateStaffWiseSpecialSavingsReportJCF");
                    var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GenerateStaffWiseSpecialSavingsReportJCFNew");
                    var reportParam = new Dictionary<string, object>();
                    reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    reportParam.Add("DateFrom", DateFrom);
                    reportParam.Add("DateTo", DateTo);
                    reportParam.Add("MainProductCode", MainProductCode);
                    ReportHelper.PrintReport("rptLoanDisburse_SDC.rpt", alldata.Tables[0], reportParam);
                    //  ReportHelper.PrintExcelReport("rptLoanDisburse.rpt", alldata.Tables[0], reportParam);
                    return Content(string.Empty);

                    //var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, Member = Member, DateFrom = DateFrom, DateTo = DateTo };
                    //var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "GenerateStaffWiseSpecialSavingsReportJCF");
                    //var reportParam = new Dictionary<string, object>();
                    //reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                    //reportParam.Add("DateFrom", DateFrom);
                    //reportParam.Add("DateTo", DateTo);
                    //ReportHelper.PrintReport("rptLoanDisburse.rpt", alldata.Tables[0], reportParam);
                    //return Content(string.Empty);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //========================= Changes Report Process (HO) ==========================//

        public ActionResult ChangesReportProcessHO()
        {
            if (!IsDayInitiated)
            {
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }

        //public JsonResult Run_ChangesReportProcessHO(string fromDate, string toDate)
        //{
        //    try
        //    {
        //        var param = new { FromDate = fromDate, ToDate = toDate };
        //        var process = ultimateReportService.GetDataWithParameter(param, "Proc_ChangesReportHO_Process");
        //        var msg = process.Tables[0].Rows[0]["Msg"].ToString();

        //        return Json(msg, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return GetErrorMessageResult(ex.Message);
        //    }
        //}
        public JsonResult Run_ChangesReportProcessHO(string fromDate, string toDate)
        {
            try
            {
                // Convert fromDate to the first day of the month
                DateTime fromDateValue = DateTime.Parse(fromDate);
                DateTime firstDayOfMonth = new DateTime(fromDateValue.Year, fromDateValue.Month, 1);
                string formattedFromDate = firstDayOfMonth.ToString("dd-MMM-yyyy");

                // Convert toDate to the last day of the month
                DateTime toDateValue = DateTime.Parse(toDate);
                DateTime lastDayOfMonth = new DateTime(toDateValue.Year, toDateValue.Month, DateTime.DaysInMonth(toDateValue.Year, toDateValue.Month));
                string formattedToDate = lastDayOfMonth.ToString("dd-MMM-yyyy");

                var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoggedInEmployee.OfficeID));
                var HeadOfficeCode = offcdetail.FirstLevel;
                var Headoffcdetail = officeService.GetByOfficeCode(HeadOfficeCode);

                var headOfficeID = Headoffcdetail.OfficeID;

                var param = new { @Office = headOfficeID, @DateFrom = formattedFromDate, @DateTo = formattedToDate, @Qtype = 0 };
                var process = ultimateReportService.GetDataWithParameter(param, "getChangesReportHO");

                return Json(new { Msg = "Success: Process Successfully Completed" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //public ActionResult LoanDisburseExeclReport(int Qtype, string Center, string Member, string DateFrom, string DateTo, string MainProductCode)
        //{
        //    var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, Member = Member, DateFrom = DateFrom, DateTo = DateTo, MainProductCode = MainProductCode };
        //    GridView gv = new GridView();
        //    var allRepaymentSchedule = groupwiseReportService.GetDataUltimateReleaseReport(param, "GenerateStaffWiseSpecialSavingsReportJCFNew");
        //    var detail = allRepaymentSchedule.Tables[0];
        //    gv.DataSource = detail;
        //    gv.DataBind();
        //    Response.ClearContent();
        //    Response.Buffer = true;
        //    Response.AddHeader("content-disposition", "attachment; filename=Rpt_LoanDisburse.xls");
        //    Response.ContentType = "application/ms-excel";
        //    Response.Charset = "";
        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter htw = new HtmlTextWriter(sw);
        //    gv.RenderControl(htw);
        //    Response.Output.Write(sw.ToString());
        //    Response.Flush();
        //    Response.End();
        //    //var reportParam = new Dictionary<string, object>();
        //    //reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //    //reportParam.Add("DateFrom", DateFrom);
        //    //reportParam.Add("DateTo", DateTo);
        //    //reportParam.Add("MainProductCode", MainProductCode);
        //    ////ReportHelper.PrintReport("rptDailyLoanCollectionInReportMenu.rpt", allRepaymentSchedule.Tables[0], reportParam);
        //    //ReportHelper.PrintReport("rptLoanDisburse.rpt", allRepaymentSchedule.Tables[0], reportParam);
        //    return Content(string.Empty);

        //}

        public ActionResult LoanDisburseExeclReport(int Qtype, string Center, string Member, string DateFrom, string DateTo, string MainProductCode)
        {
            try
            {
                var OrgID = SessionHelper.LoginUserOrganizationID;
                var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center, Member = Member, DateFrom = DateFrom, DateTo = DateTo, MainProductCode = MainProductCode };
                DataSet allRepaymentSchedule;

                var reportParam = new Dictionary<string, object>();

                if (OrgID == 12) //==== GUK : Mostak : 29 feb 2024 ====//
                {
                    allRepaymentSchedule = groupwiseReportService.GetDataUltimateReleaseReport(param, "GenerateStaffWiseLoanDisbursementRegister");
                    ReportHelper.ExportExcelReport("rptLoanDisburse_SDC__ForExcel_ApproveBy.rpt", allRepaymentSchedule.Tables[0], reportParam);
                    return Content(string.Empty);
                }
                else
                {
                    allRepaymentSchedule = groupwiseReportService.GetDataUltimateReleaseReport(param, "GenerateStaffWiseSpecialSavingsReportJCFNew");
                    ReportHelper.ExportExcelReport("rptLoanDisburse_SDC__ForExcel.rpt", allRepaymentSchedule.Tables[0], reportParam);
                    return Content(string.Empty);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        public JsonResult loadTransferCollection([DataSourceRequest] Kendo.Mvc.UI.DataSourceRequest request, int OfficeID, int CenterID, DateTime CollectionDate)
        {
            try
            {
                var param = new { OfficeID = OfficeID, CenterID = CenterID, CollectionDate = CollectionDate };
                var TransferCollectionList = groupwiseReportService.GetTabCollection(param, "getcollection_LoansavingsDate");  //employeeSPService.GetDataWithParameter(param, "dbo.getcollection_Loansavings");
                var TransferCollectionListViewModel = TransferCollectionList.Tables[0].AsEnumerable()
                    .Select((row, index) => new TransferCollectionViewModel
                    {
                        RowSl = index + 1,
                        //ReportType = row.Field<string>("ReportType"),
                        OfficeId = row.Field<int>("OfficeID"),
                        //OfficeCode = row.Field<string>("OfficeCode"),
                        SamityName = row.Field<string>("SamityName"),
                        SamityCode = row.Field<string>("SamityCode"),
                        MemberCode = row.Field<string>("MemberCode"),
                        MemberName = row.Field<string>("MemberName"),
                        ProductCode = row.Field<string>("ProductCode"),

                        LoanPaid = row.Field<decimal>("LoanPaid"),
                        IntPaid = row.Field<decimal>("IntPaid"),
                        Deposit = row.Field<decimal>("Deposit"),
                        IsUploaded = row.Field<int>("isUploaded"),
                        SavingsCS = row.Field<decimal>("SavingsCS"),
                        TotalLoan = row.Field<decimal>("TotalLoan"),
                    }).ToList();

                DataSourceResult result = TransferCollectionListViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", Message = ex.Message });
            }

        }
        private void MapDropDownList(BalanceCompareViewModel model)
        {
            var compareItem = new List<SelectListItem>();
            compareItem.Add(new SelectListItem() { Text = "Select One", Value = "", Selected = true });
            compareItem.Add(new SelectListItem() { Text = "Loan", Value = "1" });
            compareItem.Add(new SelectListItem() { Text = "Savings", Value = "2" });
            //compareItem.Add(new SelectListItem() { Text = "All", Value = "2" });
            model.ComareList = compareItem;
        }
        public JsonResult GetBalanceCompare(int jtStartIndex, int jtPageSize, string jtSorting, int Qtype)
        {
            try
            {

                if (Qtype == 1)
                {
                    var param = new { QType = Qtype , OfficeID = LoginUserOfficeID };
                    var sp_response = ultimateReportService.GetDataWithParameter(param, "Proc_GetBalanceCompare");

                    var assignSpToVM = sp_response.Tables[0].AsEnumerable()
                        .Select(x => new BalanceCompareViewModel
                        {
                            RowSl = x.Field<int>("RowSl"),
                            OfficeID = x.Field<int>("OfficeID"),
                            OfficeCode = x.Field<string>("OfficeCode"),
                            OfficeName = x.Field<string>("OfficeName"),
                            LoanBalR = x.Field<decimal>("LoanBalR"),
                            LoanBalS = x.Field<decimal>("LoanBalS"),
                            BalanceA = x.Field<decimal>("BalanceA"),
                            Diff_R_S = x.Field<decimal>("Diff_R_S"),
                            Diff_R_A = x.Field<decimal>("Diff_R_A"),
                            Diff_S_A = x.Field<decimal>("Diff_S_A"),
                            AccCode = x.Field<string>("AccCode"),
                            AccName = x.Field<string>("AccName"),
                            CompleteWorkDate = x.Field<DateTime>("CompleteWorkDate")
                        }).ToList();

                    var currentPageRecords = assignSpToVM.Skip(jtStartIndex).Take(jtPageSize);
                    return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = assignSpToVM.LongCount(), JsonRequestBehavior.AllowGet });
                }
                else if (Qtype == 2)
                {
                    var param = new { QType = Qtype, OfficeID = LoginUserOfficeID };
                    var sp_response = ultimateReportService.GetDataWithParameter(param, "Proc_GetBalanceCompare");

                    var assignSpToVM = sp_response.Tables[0].AsEnumerable()
                        .Select(x => new BalanceCompareViewModel
                        {
                            RowSl = x.Field<int>("RowSl"),
                            OfficeID = x.Field<int>("OfficeID"),
                            OfficeCode = x.Field<string>("OfficeCode"),
                            OfficeName = x.Field<string>("OfficeName"),
                            BalanceR = x.Field<decimal>("BalanceR"),
                            BalanceS = x.Field<decimal>("BalanceS"),
                            BalanceA = x.Field<decimal>("BalanceA"),
                            Diff_R_S = x.Field<decimal>("Diff_R_S"),
                            Diff_R_A = x.Field<decimal>("Diff_R_A"),
                            Diff_S_A = x.Field<decimal>("Diff_S_A"),
                            AccCode = x.Field<string>("AccCode"),
                            AccName = x.Field<string>("AccName"),
                            CompleteWorkDate = x.Field<DateTime>("CompleteWorkDate")
                        }).ToList();

                    var currentPageRecords = assignSpToVM.Skip(jtStartIndex).Take(jtPageSize);
                    return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = assignSpToVM.LongCount(), JsonRequestBehavior.AllowGet });
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult BalanceCompare()
        {
            var model = new BalanceCompareViewModel();
            MapDropDownList(model);
            return View(model);
        }
        public JsonResult loadTransferCollectionHistory([DataSourceRequest] Kendo.Mvc.UI.DataSourceRequest request, int OfficeID, int EmployeeID, int CenterID, DateTime? CollectionDate)
        {
            try
            {
                var param = new { OfficeID = OfficeID, EmployeeID = EmployeeID, CenterID = CenterID, CollectionDate = CollectionDate };
                var TransferCollectionList = groupwiseReportService.GetTabCollectionHistory(param, "getcollection_Loansavings_History");  //employeeSPService.GetDataWithParameter(param, "dbo.getcollection_Loansavings");
                var TransferCollectionListViewModel = TransferCollectionList.Tables[0].AsEnumerable()
                    .Select((row, index) => new TransferCollectionViewModel
                    {
                        RowSl = index + 1,
                        //ReportType = row.Field<string>("ReportType"),
                        OfficeId = row.Field<int>("OfficeID"),
                        //OfficeCode = row.Field<string>("OfficeCode"),
                        EmployeeCode = row.Field<string>("EmployeeCode"),
                        SamityName = row.Field<string>("SamityName"),
                        SamityCode = row.Field<string>("SamityCode"),
                        MemberCode = row.Field<string>("MemberCode"),
                        MemberName = row.Field<string>("MemberName"),
                        ProductCode = row.Field<string>("ProductCode"),

                        LoanPaid = row.Field<decimal>("LoanPaid"),
                        IntPaid = row.Field<decimal>("IntPaid"),
                        Deposit = row.Field<decimal>("Deposit"),

                    }).ToList();

                DataSourceResult result = TransferCollectionListViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", Message = ex.Message });
            }

        }
        public ActionResult UpdateTabCollection(string officeId, string CenterID)
        {
            if (CenterID == "")
            {
                CenterID = "0";
            }
            loanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);
            var param = new { OfficeID = LoginUserOfficeID, loggedInUser = LoggedInEmployeeID, CollectionDate = TransactionDate, CenterID = CenterID };
            var result = groupwiseReportService.UploadTabCollection(param, "UpLoanTabCollection");  //employeeSPService.GetDataWithParameter(param, "dbo.getcollection_Loansavings");
            int resul;
            if (result.Tables[0].Rows.Count > 0)
            {
                resul = Convert.ToInt16(result.Tables[0].Rows[0]["Success"].ToString());
            }
            else
            {
                resul = 0;
            }
            //var result = loanCollectionService.setLoanAndSavingingLessFiftyPercent(Convert.ToInt16(LoggedInOrganizationID), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(CenterID), 1);
            return Json(resul, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateTabCollectionbKash(string officeId, string CenterID)
        {
            if (CenterID == "")
            {
                CenterID = "0";
            }
            loanCollectionService.delVoucher(LoginUserOfficeID, TransactionDate, LoggedInOrganizationID);
            var param = new { OfficeID = LoginUserOfficeID, loggedInUser = LoggedInEmployeeID, CollectionDate = TransactionDate, CenterID = CenterID };
            var result = groupwiseReportService.UploadTabCollectionbKash(param, "bKashUpLoanTabCollection");  //UploadTabCollectionbKash
            int resul;
            if (result.Tables[0].Rows.Count > 0)
            {
                resul = Convert.ToInt16(result.Tables[0].Rows[0]["Success"].ToString());
            }
            else
            {
                resul = 0;
            }
            //var result = loanCollectionService.setLoanAndSavingingLessFiftyPercent(Convert.ToInt16(LoggedInOrganizationID), Convert.ToInt16(LoginUserOfficeID), Convert.ToInt32(CenterID), 1);
            return Json(resul, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult getEmployeeCodeAutoComplete(string Prefix, int? OfficeId)
        {
            int Result = 0; string Message = ""; object Data = "";
            try
            {
                int? _OfficeId = 0;
                if (OfficeId.HasValue)
                {
                    _OfficeId = LoginUserOfficeID;
                }

                var totalList = EmployeeWiseAutoComplete(_OfficeId, Prefix);
                var EntInfoList = totalList.Select(b => new { EmployeeID = b.EmployeeID, EmployeeCode = b.EmployeeCode, EmployeeName = b.EmployeeName.ToString() });
                Data = EntInfoList;


                return Json(new { Result = Result, Message = Message, Data = Data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Result = 0; Message = e.Message;
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);

        }
        public IEnumerable<TransferCollectionViewModel> EmployeeWiseAutoComplete(int? OfficeId, string Prefix)
        {
            List<TransferCollectionViewModel> rReturnList = new List<TransferCollectionViewModel>();
            try
            {
                var pram = new { OfficeId = OfficeId, Prefix = Prefix };
                var ShortInfo = groupwiseReportService.GetTabCollectionHistory(pram, "SP_GET_EmployeeAutoComplete"); //employeeSPService.GetDataWithParameter(pram, "SP_GET_CenterAutoComplete");
                rReturnList = ShortInfo.Tables[0].AsEnumerable()
                   .Select((row, index) => new TransferCollectionViewModel
                   {
                       EmployeeID = row.Field<short>("EmployeeID"),
                       EmployeeCode = row.Field<string>("EmployeeCode"),
                       EmployeeName = row.Field<string>("EmployeeName"),
                   }).ToList();
            }
            catch (Exception e)
            {
                rReturnList = new List<TransferCollectionViewModel>();
            }
            return rReturnList;
        }
        [HttpPost]
        public JsonResult getCenterCodeAutoComplete(int? OfficeId, int? EmployeeID, string Prefix)
        {
            int Result = 0; string Message = ""; object Data = "";
            try
            {
                int? _OfficeId = 0;
                if (OfficeId.HasValue)
                {
                    _OfficeId = LoginUserOfficeID;
                }

                var totalList = CenterWiseAutoComplete(_OfficeId, EmployeeID, Prefix);
                var EntInfoList = totalList.Select(b => new { CenterID = b.CenterID, CenterCode = b.CenterCode, CenterName = b.CenterName.ToString() });
                Data = EntInfoList;


                return Json(new { Result = Result, Message = Message, Data = Data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Result = 0; Message = e.Message;
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);

        }
        public IEnumerable<TransferCollectionViewModel> CenterWiseAutoComplete(int? OfficeId, int? EmployeeID, string Prefix)
        {
            List<TransferCollectionViewModel> rReturnList = new List<TransferCollectionViewModel>();
            try
            {
                var Employeeid = EmployeeID;
                if (Employeeid == null) { Employeeid = 0; }
                var pram = new { OfficeId = OfficeId, EmployeeID = Employeeid, Prefix = Prefix };
                var ShortInfo = groupwiseReportService.GetTabCollection(pram, "SP_GET_CenterAutoComplete"); //employeeSPService.GetDataWithParameter(pram, "SP_GET_CenterAutoComplete");
                rReturnList = ShortInfo.Tables[0].AsEnumerable()
                   .Select((row, index) => new TransferCollectionViewModel
                   {
                       CenterID = row.Field<int>("CenterID"),
                       CenterCode = row.Field<string>("CenterCode"),
                       CenterName = row.Field<string>("CenterName"),
                   }).ToList();
            }
            catch (Exception e)
            {
                rReturnList = new List<TransferCollectionViewModel>();
            }
            return rReturnList;
        }
        public void mapForTargetAchievementBuroLatest(TargetAchievementBuroLatestViewModel model)
        {
            var OfficeID = LoginUserOfficeID;
            var pram = new { OfficeID = OfficeID };
            var ViewemployeeList = groupwiseReportService.GetActiveEmployee(pram, "SP_GetActiveEmployee");
            var employeeList = ViewemployeeList.Tables[0].AsEnumerable()
               .Select((row, index) => new SelectListItem
               {
                   Text = row.Field<string>("EmployeeCode") + " " + row.Field<string>("EmpName"),
                   Value = row.Field<short>("EmployeeID").ToString()
               }).ToList();

            var Elist = new List<SelectListItem>();
            Elist.Add(new SelectListItem { Text = "Please Select", Value = "" });
            Elist.AddRange(employeeList);
            model.EmployeeList = Elist;
        }
        //public void mapForTransferCollection(TransferCollectionViewModel model)
        //{
        //    var centerList = centerService.GetAll().Where(p => p.IsActive == true && p.CenterID == 973);
        //    var viewList = centerList.AsEnumerable().Select(p => new SelectListItem()
        //    {
        //        Text = p.CenterCode + " " + p.CenterName,
        //        Value = p.CenterID.ToString()
        //    }).ToList();
        //    var list = new List<SelectListItem>();
        //    list.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
        //    list.AddRange(viewList);
        //    model.CenterList = list;
        //}
        public void mapForTransferCollection(TransferCollectionViewModel model)
        {
            var CenterID = 973;
            var pram = new { CenterID = CenterID };
            var centerList = groupwiseReportService.GetActiveCenter(pram, "SP_GET_GetActiveCenter");
            var viewList = centerList.Tables[0].AsEnumerable()
                 .Select((row, index) => new SelectListItem
                 {
                     Text = row.Field<string>("CenterCode") + " " + row.Field<string>("CenterName"),
                     Value = row.Field<int>("CenterID").ToString()
                 }).ToList();
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            list.AddRange(viewList);
            model.CenterList = list;
        }
        //public void mapForTargetAchievementBuroLatest(TargetAchievementBuroLatestViewModel model)
        //{
        //    var ViewemployeeList = employeeService.GetAll().Where(p => p.IsActive == true && p.OfficeID == LoginUserOfficeID && p.EmployeeStatus == 1).ToList();
        //    var employeeList = ViewemployeeList.AsEnumerable().Select(p => new SelectListItem()
        //    {
        //        Text = string.Format("{0} - {1}", p.EmployeeCode.ToString(), p.EmpName.ToString()),
        //        Value = p.EmployeeID.ToString()
        //    }).ToList();
        //    var Elist = new List<SelectListItem>();
        //    Elist.Add(new SelectListItem { Text = "Please Select", Value = "" });
        //    Elist.AddRange(employeeList);
        //    model.EmployeeList = Elist;
        //}






        //public void mapForStatisticsReportDetails(StatisticsReportDetailsViewModel model)
        //{
        //    var statisticsReportList = statisticsReportService.GetAll().Where(p => p.IsActive == true);
        //    var statisticsReportListviewList = statisticsReportList.AsEnumerable().Select(p => new SelectListItem()
        //    {
        //        Text = p.StatisticsReportName.ToString(),
        //        Value = p.StatisticsReportId.ToString()
        //    }).ToList();
        //    var statisticsReportList_list = new List<SelectListItem>();
        //    statisticsReportList_list.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
        //    statisticsReportList_list.AddRange(statisticsReportListviewList);
        //    model.StatisticsReportList = statisticsReportList_list;


        //    var statisticsDescriptionList = statisticsDescriptionService.GetAll().Where(p => p.IsActive == true);
        //    var statisticsDescriptionListviewList = statisticsDescriptionList.AsEnumerable().Select(p => new SelectListItem()
        //    {
        //        Text = p.StatisticsDescriptionName.ToString(),
        //        Value = p.StatisticsDescriptionID.ToString()
        //    }).ToList();
        //    var statisticsDescriptionList_list = new List<SelectListItem>();
        //    statisticsDescriptionList_list.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
        //    statisticsDescriptionList_list.AddRange(statisticsDescriptionListviewList);
        //    model.StatisticsDescriptionList = statisticsDescriptionList_list;
        //}
        public JsonResult loadStatisticsReportDetails([DataSourceRequest] Kendo.Mvc.UI.DataSourceRequest request, DateTime? DateFrom)
        {
            try
            {
                var OfficeID = SessionHelper.LoginUserOfficeID;
                var param = new { DateFrom = DateFrom, OfficeID = OfficeID };
                var statisticsReportDetailsList = groupwiseReportService.GetStatisticsReportDetails(param, "getStatisticsReportDetails");  //employeeSPService.GetDataWithParameter(param, "dbo.getcollection_Loansavings");
                var statisticsReportDetailsListViewModel = statisticsReportDetailsList.Tables[0].AsEnumerable()
                    .Select((row, index) => new StatisticsReportDetailsViewModel
                    {
                        RowSl = index + 1,
                        StatisticsReportDetailsID = row.Field<long>("StatisticsReportDetailsID"),
                        StatisticsReportId = row.Field<int>("StatisticsReportId"),
                        statisticsDespritionID = row.Field<long>("statisticsDespritionID"),
                        OfficeID = row.Field<int?>("OfficeID"),
                        ColumnShow = row.Field<int?>("ColumnShow"),
                        AmountM = row.Field<decimal>("AmountM"),
                        AmountF = row.Field<decimal>("AmountF"),
                        StatisticsReportDetailsDateStr = row.Field<string>("StatisticsReportDetailsDateStr"),
                        StatisticsReportDetailsDate = row.Field<DateTime?>("StatisticsReportDetailsDate"),
                        IsActive = row.Field<bool>("IsActive"),
                        ReportType = row.Field<int?>("ReportType"),
                        ItemSubID = row.Field<int?>("ItemSubID"),
                        ItemHeadName = row.Field<string>("ItemHeadName"),
                        //StatisticsDescriptionName = row.Field<string>("StatisticsDescriptionName"),
                        statisticsDesprition = row.Field<string>("statisticsDesprition"),
                        StatisticsReportName = row.Field<string>("StatisticsReportName"),
                    }).ToList();

                DataSourceResult result = statisticsReportDetailsListViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", Message = ex.Message });
            }

        }
        public JsonResult loadTargetAchievementBuroLatest([DataSourceRequest] Kendo.Mvc.UI.DataSourceRequest request, DateTime? DateFrom, int EmployeeID)
        {
            try
            {
                var OfficeID = SessionHelper.LoginUserOfficeID;//1321
                //var EmployeeID = 1341;
                var param = new { DateFrom = DateFrom, OfficeID = OfficeID, EmployeeID = EmployeeID };
                var targetAchievementBuroLatestList = groupwiseReportService.GetTargetAchievementBuroLatest(param, "getTargetAchievementBuroLatest");
                var targetAchievementBuroLatestListViewModel = targetAchievementBuroLatestList.Tables[0].AsEnumerable()
                    .Select((row, index) => new TargetAchievementBuroLatestViewModel
                    {
                        RowSl = index + 1,
                        TargetId = row.Field<int?>("TargetId"),
                        ParticularId = row.Field<int?>("ParticularId"),
                        ParticularName = row.Field<string>("ParticularName"),
                        Balance = row.Field<decimal?>("Balance"),
                        TargetCurrentYear = row.Field<decimal?>("TargetCurrentYear"),
                        Target = row.Field<decimal?>("Target"),
                        Achievement = row.Field<decimal?>("Achievement"),
                        Date = row.Field<DateTime?>("Date"),
                        DateStr = row.Field<String>("DateStr"),
                        IsActive = row.Field<bool?>("IsActive"),
                        OfficeID = row.Field<int?>("OfficeID"),
                        ProductID = row.Field<int?>("ProductID"),
                        EmployeeID = row.Field<int?>("EmployeeID")
                    }).ToList();

                DataSourceResult result = targetAchievementBuroLatestListViewModel.ToDataSourceResult(request);
                return Json(new { data = result.Data, total = result.Total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "ERROR", Message = ex.Message });
            }

        }
        //[HttpPost]
        //public JsonResult UpdateCollectionAmountsAmountM(StatisticsReportDetailsViewModel statisticsReportDetails)
        //{
        //    var result = 0;
        //    var message = "";
        //    try
        //    {
        //        var model = statisticsReportDetailsService.GetById(Convert.ToInt32(statisticsReportDetails.StatisticsReportDetailsID));
        //        model.StatisticsReportDetailsID = statisticsReportDetails.StatisticsReportDetailsID;
        //        model.StatisticsReportDetailsDate = Convert.ToDateTime(statisticsReportDetails.StatisticsReportDetailsDateStr);
        //        model.AmountM = statisticsReportDetails.AmountM;
        //        model.UpdateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
        //        model.UpdateDate = DateTime.UtcNow;
        //        statisticsReportDetailsService.Update(model);
        //        result = 1;
        //        message = "Updated successfully";
        //    }
        //    catch (Exception)
        //    {
        //        result = 0;
        //        message = "Update failed";
        //    }
        //    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        //}


        //[HttpPost]
        //public JsonResult UpdateCollectionAmountsAll(List<StatisticsReportDetailsViewModel> ProposalList)
        //{
        //    var result = 0;
        //    var message = "";
        //    try
        //    {

        //        if (ProposalList != null && ProposalList.Count() > 0)
        //        {
        //            foreach (var proposal in ProposalList)
        //            {
        //                var model = statisticsReportDetailsService.GetById(Convert.ToInt32(proposal.StatisticsReportDetailsID));

        //                var prm = new
        //                {
        //                    StatisticsReportDetailsID = proposal.StatisticsReportDetailsID,
        //                    OfficeID = SessionHelper.LoginUserOfficeID,
        //                    StatisticsReportDetailsDate = proposal.StatisticsReportDetailsDate,
        //                    AmountM = proposal.AmountM,
        //                    AmountF = proposal.AmountF
        //                };
        //                //DataSet DetailsInfo;
        //                var DetailsInfo = groupwiseReportService.getStatisticsReportDetailsInfo(prm, "getStatisticsReportDetailsInfo");

        //                if (DetailsInfo.Tables[0].Rows.Count > 0)
        //                {
        //                    var OfficeID = SessionHelper.LoginUserOfficeID;
        //                    model.StatisticsReportDetailsID = proposal.StatisticsReportDetailsID;
        //                    model.AmountM = proposal.AmountM;
        //                    model.AmountF = proposal.AmountF;
        //                    model.OfficeID = OfficeID;
        //                    model.StatisticsReportDetailsDate = Convert.ToDateTime(proposal.StatisticsReportDetailsDate);
        //                    model.IsActive = true;
        //                    model.UpdateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
        //                    model.UpdateDate = DateTime.UtcNow;
        //                    statisticsReportDetailsService.Update(model);
        //                    result = 1;

        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = 0;
        //        //message = "Update failed";
        //        return Json(new { result = result, Message = ex.Message });

        //    }
        //    return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public JsonResult UpdateCollectionAmountsAll(List<StatisticsReportDetailsViewModel> ProposalList)
        {
            var result = 0;
            var message = "";
            try
            {

                if (ProposalList != null && ProposalList.Count() > 0)
                {
                    foreach (var proposal in ProposalList)
                    {
                        var model = statisticsReportDetailsService.GetById(Convert.ToInt32(proposal.StatisticsReportDetailsID));

                        var prm = new
                        {
                            StatisticsReportDetailsID = proposal.StatisticsReportDetailsID,
                            OfficeID = SessionHelper.LoginUserOfficeID,
                            StatisticsReportDetailsDate = proposal.StatisticsReportDetailsDate,
                            AmountM = proposal.AmountM,
                            AmountF = proposal.AmountF
                        };
                        //DataSet DetailsInfo;
                        var DetailsInfo = groupwiseReportService.getStatisticsReportDetailsInfo(prm, "getStatisticsReportDetailsInfo");

                        if (DetailsInfo.Tables[0].Rows.Count > 0)
                        {
                            //vLoanDue = Convert.ToDecimal(LoanInstallMent.Tables[0].Rows[0]["LoanDue"].ToString());

                            //var OfficeID = SessionHelper.LoginUserOfficeID;
                            //model.StatisticsReportDetailsID = proposal.StatisticsReportDetailsID;
                            //model.AmountM = proposal.AmountM;
                            //model.AmountF = proposal.AmountF;
                            //model.OfficeID = OfficeID;
                            //model.StatisticsReportDetailsDate = Convert.ToDateTime(proposal.StatisticsReportDetailsDate);
                            //model.IsActive = true;
                            //model.UpdateUser = Convert.ToString(SessionHelper.LoggedInEmployeeID);
                            //model.UpdateDate = DateTime.UtcNow;
                            //statisticsReportDetailsService.Update(model);


                            var IIDD = proposal.StatisticsReportDetailsID;
                            var param = new
                            {
                                IIDD = IIDD,
                                OfficeID = SessionHelper.LoginUserOfficeID,
                                StatisticsReportDetailsDate = proposal.StatisticsReportDetailsDate,
                                AmountM = proposal.AmountM,
                                AmountF = proposal.AmountF,
                                LoggedInEmployeeID = Convert.ToString(SessionHelper.LoggedInEmployeeID)
                            };
                            var statisticsReportDetailsUpdate = groupwiseReportService.GetStatisticsReportDetailsUpdate(param, "getStatisticsReportDetailsUpdate");
                            result = 1;
                        }



                        //var editable = statisticsReportDetailsService.GetAll().Where(p => p.StatisticsReportDetailsID == proposal.StatisticsReportDetailsID && p.IsActive == true && (p.AmountM != proposal.AmountM || p.AmountF != proposal.AmountF)).ToList();
                        //if (editable.Any())
                        //{


                        //}





                    }
                }
            }
            catch (Exception ex)
            {
                result = 0;
                //message = "Update failed";
                return Json(new { result = result, Message = ex.Message });

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateTargetAchievementBuroLatestAll(List<TargetAchievementBuroLatestViewModel> ProposalList)
        {
            int result = 0;
            var message = "";
            try
            {
                if (ProposalList != null && ProposalList.Count() > 0)
                {
                    foreach (var proposal in ProposalList)
                    {
                        //if (proposal.TargetId > 100000000 && (proposal.Target != null))
                        if (proposal.TargetId > 100000000)
                        {
                            var proposalTarget = proposal.Target; if (proposalTarget == null || proposalTarget == 0) { proposalTarget = 0; }
                            var entity = new TargetAchievementBuro();
                            //entity.ParticularId = proposal.ParticularId;
                            //entity.Target = proposalTarget;
                            //entity.Date = proposal.Date;
                            //entity.OfficeID = SessionHelper.LoginUserOfficeID;
                            //entity.EmployeeID = proposal.EmployeeID;
                            //entity.IsActive = true;
                            //entity.CreateUser = SessionHelper.LoginUserOfficeID;
                            //entity.CreateDate = DateTime.Now;
                            //targetAchievementBuroService.Create(entity);

                            var paramInsert = new
                            {
                                ParticularId = proposal.ParticularId,
                                Target = proposalTarget,
                                Date = proposal.Date,
                                OfficeID = SessionHelper.LoginUserOfficeID,
                                EmployeeID = proposal.EmployeeID,
                                IsActive = true,
                                CreateUser = SessionHelper.LoginUserOfficeID,
                                CreateDate = DateTime.Now
                            };
                            var targetAchievementInsert = groupwiseReportService.GetgetTargetAchievementBuroLatestInsert(paramInsert, "getTargetAchievementBuroLatestInsert");
                            result = 1;
                        }

                        var model = targetAchievementBuroService.GetById(Convert.ToInt32(proposal.TargetId));
                        var prm = new
                        {
                            TargetId = proposal.TargetId,
                            ParticularId = proposal.ParticularId,
                            Target = proposal.Target,
                            Date = proposal.Date,
                            OfficeID = SessionHelper.LoginUserOfficeID,
                            ProductID = proposal.ProductID,
                            EmployeeID = proposal.EmployeeID
                        };
                        var DetailsInfo = groupwiseReportService.getTargetAchievementBuroLatestInfo(prm, "getTargetAchievementBuroLatestInfo"); //getStatisticsReportDetailsInfo

                        if (DetailsInfo.Tables[0].Rows.Count > 0)
                        {
                            var param = new
                            {
                                TargetId = proposal.TargetId,
                                ParticularId = proposal.ParticularId,
                                Target = proposal.Target,
                                Date = proposal.Date,
                                OfficeID = SessionHelper.LoginUserOfficeID,
                                EmployeeID = proposal.EmployeeID
                            };
                            var targetAchievementUpdate = groupwiseReportService.GetgetTargetAchievementBuroLatestUpdate(param, "getTargetAchievementBuroLatestUpdate");  //getStatisticsReportDetailsUpdate
                            result = 2;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = 0;
                //message = "Update failed";
                return Json(new { result = result, Message = ex.Message });

            }
            return Json(new { result = result, message = message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TargetAchievementBuroLatest()
        {
            var model = new TargetAchievementBuroLatestViewModel();
            mapForTargetAchievementBuroLatest(model);
            return View(model);
        }
        public ActionResult StatisticsReportDetails()
        {
            var model = new StatisticsReportDetailsViewModel();
            //mapForStatisticsReportDetails(model);
            return View(model);
        }
        public ActionResult TabCollection()
        {
            ViewData["OfficeID"] = SessionHelper.LoginUserOfficeID;
            ViewData["TransactionDate"] = SessionHelper.TransactionDate;
            var model = new TransferCollectionViewModel();
            mapForTransferCollection(model);
            return View(model);
        }
        public ActionResult TabCollectionbKash()
        {
            ViewData["OfficeID"] = SessionHelper.LoginUserOfficeID;
            var model = new TransferCollectionViewModel();
            mapForTransferCollection(model);
            return View(model);
        }
        public ActionResult TabCollectionHistory()
        {
            ViewData["OfficeID"] = SessionHelper.LoginUserOfficeID;
            var model = new TransferCollectionViewModel();
            mapForTransferCollection(model);
            return View(model);
        }
        public ActionResult GeneratePOMIS3_OfficeWiseReport(string Qtype, string OfficeId, string DateTo)
        {
            try
            {
                var param = new { Qtype = Convert.ToInt32(Qtype), OfficeID = Convert.ToInt32(OfficeId), Org = SessionHelper.LoginUserOrganizationID, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "Rpt_POMIS3_ConsolidationOfficewise");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("OverDueAgeingOfficewise.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);

                //var param = new { Qtype = Convert.ToInt32(Qtype), OfficeID = Convert.ToInt32(OfficeId), Org = SessionHelper.LoginUserOrganizationID, DateTo = DateTo };
                //var alldata = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "Rpt_POMIS3_ConsolidationOfficewise");
                //var reportParam = new Dictionary<string, object>();
                //reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                //ReportHelper.PrintReport("OverDueAgeingOfficewise.rpt", alldata.Tables[0], reportParam);
                //return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GeneratePOMIS3_OfficeWiseReportOverDue(string Qtype, string OfficeId, string DateTo)
        {
            try
            {
                var param = new { Qtype = Convert.ToInt32(Qtype), OfficeID = Convert.ToInt32(OfficeId), Org = SessionHelper.LoginUserOrganizationID, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "Rpt_POMIS3_ConsolidationOfficewise_OverDueType");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("OverDueAgeingWithNoOfMembersOverDueType.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GeneratePOMIS3_PKSFNonPKSF(string Qtype, string OfficeId, string DateTo)
        {
            try
            {
                var param = new { Qtype = Convert.ToInt32(Qtype), OfficeID = Convert.ToInt32(OfficeId), Org = SessionHelper.LoginUserOrganizationID, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "Rpt_POMIS3_ConsolidationOfficewisePKSFNonPKSF");

                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);

                ReportHelper.PrintReport("POMIS3_PKSFNonPKSF.rpt", alldata.Tables[0], reportParam);

                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateOverdueMemberListNewBeforeDayEndtReport(string DateTo)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Proc_Get_DueLoan_Before_DayEnd");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptNewOverDueMemberListBeforeDayEnd.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateQuarterlySupplimentary1Report(string DateFrom, string DateTo, string Qtype)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_Supplementary");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("Supplementary_1_Final.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //public ActionResult GenerateQuarterlySupplimentary1Report(string DateFrom, string DateTo, string Qtype)
        //{
        //    try
        //    {
        //        var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype };

        //        var paramValues = new List<ParameterValue>();
        //        paramValues.Add(new ParameterValue() { Name = "param_orgName", Value = ApplicationSettings.OrganiztionName.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "Org", Value = SessionHelper.LoginUserOrganizationID.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "Office", Value = SessionHelper.LoginUserOfficeID.ToString() });

        //        paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = DateFrom.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "DateTo", Value = DateTo.ToString() });
        //        paramValues.Add(new ParameterValue() { Name = "Qtype", Value = Qtype.ToString() });
        //        PrintSSRSReport("/gBanker_Reports/Rpt_Supplementary_Product", paramValues.ToArray(), "gBankerReport");


        //        return Content(string.Empty);

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        public ActionResult GenerateQuarterlySupplimentary2Report(string DateFrom, string DateTo, string Qtype)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_Supplementary");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("Supplementary_2_Final.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult SessionClear()
        {
            var OrgID = SessionHelper.LoginUserOrganizationID;
            var roleID = SessionHelper.LoginUserRoleId;
            var EmployeeId = SessionHelper.LoginUserEmployeeID;

            if (OrgID == 3 && roleID == 1 && EmployeeId == 520)
            {
                return View();
            }
            return null;
        }

        public JsonResult SessionClearProcess()
        {
            try
            {
                var OrgID = SessionHelper.LoginUserOrganizationID;
                if (OrgID == 3)
                {
                    var param = new { @DataBaseName = "", @UserName = "282" };
                    var process = ultimateReportService.GetDataWithParameter(param, "SP_SESSION_CLEAR");
                }
                return Json(new { Msg = "Success: Session Clear Process  Successfully Completed" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SessionClearButton()
        {
            try
            {

                var process = ultimateReportService.GetDataWithoutParameter("SP_SESSION_CLEAR_BUTTON");
                var msg = process.Tables[0].Rows[0]["RESULT"].ToString();

                return Json(new { Msg = msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult SessionRollBack()
        {
            try
            {
                var OrgID = SessionHelper.LoginUserOrganizationID;
                if (OrgID == 3)
                {
                    var param = new { @DataBaseName = "", @UserName = "282" };
                    var process = ultimateReportService.GetDataWithParameter(param, "SP_SESSION_ROLLBACK");
                }
                return Json(new { Msg = "Success: Session Roll Back Process  Successfully Completed" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GenerateQuarterlySupplimentary3Report(string DateFrom, string DateTo, string Qtype)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_Supplementary");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("Supplementary_3_Final.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateCPSPmonthlyProgressReport(string Qtype, string OfficeId, string DateTo, string ZoneID, string AreaID, string BranchID, string EmployeeID)
        {
            try
            {
                var reportParam = new Dictionary<string, object>();
                //reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("FromDate", from_date);

                DateTime dt = Convert.ToDateTime(DateTo);
                int Year = dt.Year;
                int Month = dt.Month;

                var param1 =
                    new
                    {
                        ReportingYear = Year,
                        ReportingMonth = Month,
                        OrgID = Convert.ToInt32(LoggedInOrganizationID),
                        ZoneID = Convert.ToInt32(ZoneID),
                        AreaID = Convert.ToInt32(AreaID),
                        BranchID = Convert.ToInt32(BranchID),
                        EmployeeID = Convert.ToInt32(EmployeeID),
                        QType = Convert.ToInt32(Qtype)
                    };


                var alldata1 = groupwiseReportService.GetDataProcessLOg(param1, "Proc_Get_MonthlyCPSPProgressReportRowWiseShow");
                if (alldata1.Tables.Count > 0)
                {
                    ReportHelper.PrintReport("rptMonthlyCPSPProgressReportRowWise.rpt", alldata1.Tables[0], reportParam);
                    return Content(string.Empty);
                }
                else
                {
                    return Json(new { Result = "ERROR", Message = "No Data Found!" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }
        }

        public ActionResult GetMemberList_CPSP(string memberid, string OfficeId)
        {
            var MemberByCenterSessionKey = string.Format("MemberByCenterSessionKey_{0}", OfficeId);
            var memberList = new List<Member>();
            if (Session[MemberByCenterSessionKey] != null)
                memberList = Session[MemberByCenterSessionKey] as List<Member>;
            else
            {
                var mbr = memberService.SearchMember(int.Parse(OfficeId), Convert.ToInt16(LoggedInOrganizationID)).ToList();
                Session[MemberByCenterSessionKey] = mbr;
                memberList = mbr;
            }
            //var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, m.FirstName).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, m1.FirstName) }).ToList();
            var members = memberList.Where(m => string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName)).ToLower().Contains(memberid.ToLower())).Select(m1 => new { m1.MemberID, MemberName = string.Format("{0} - {1}", m1.MemberCode, (string.IsNullOrEmpty(m1.FirstName) ? "" : m1.FirstName) + ' ' + (string.IsNullOrEmpty(m1.MiddleName) ? "" : m1.MiddleName) + ' ' + (string.IsNullOrEmpty(m1.LastName) ? "" : m1.LastName)) }).ToList();
            return Json(members, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerateCPSPmonthlyProgressReportProcess(string DateTo)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(DateTo);
                int Year = dt.Year;
                int Month = dt.Month;

                var param = new { ProcessYear = Year, ProcessMonth = Month };
                var alldata = groupwiseReportService.GetDataProcessLOg(param, "Proc_Set_MonthlyCPSPProgressReportProcessRowWise");
                return Json(new { Result = "SUCCESS", Message = "SUCCESS" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CPSPmonthlyProgressReportProcess()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
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

            return View();
        }

        //public ActionResult CPSPmonthlyProgressReport()
        //{

        //    IEnumerable<SelectListItem> items = new SelectList(" ");
        //    ViewData["HOList"] = items;
        //    ViewData["ZoneList"] = items;
        //    ViewData["AreaList"] = items;
        //    ViewData["OfficeList"] = items;
        //    ViewData["AccCodeList"] = items;
        //    var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
        //    //ViewData["OfficeLevel"] = offcdetail.OfficeLevel;
        //    ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];

        //    if (offcdetail.OfficeLevel == 1)
        //    {
        //        ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
        //        ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
        //        ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
        //        ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
        //    }
        //    else
        //    {
        //        ViewData["FirstLevel"] = officeService.GetByOfficeCode(offcdetail.FirstLevel).OfficeID;
        //        ViewData["SecondLevel"] = officeService.GetByOfficeCode(offcdetail.SecondLevel).OfficeID;
        //        ViewData["ThirdLevel"] = officeService.GetByOfficeCode(offcdetail.ThirdLevel).OfficeID;
        //        ViewData["FourthLevel"] = officeService.GetByOfficeCode(offcdetail.FourthLevel).OfficeID;
        //    }

        //    ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");

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
        //    //ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
        //    var model = new MemberViewModel();
        //    model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
        //    var blnk_items = new List<SelectListItem>();
        //    model.CenterList = blnk_items;
        //    var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID));
        //    var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName) + ' ' + (string.IsNullOrEmpty(m.RefereeName) ? "" : m.RefereeName)), Value = m.MemberID.ToString() });
        //    var MemberList = new List<Member>();
        //    ViewData["Member"] = viewMember;

        //    return View(model);
        //}





        //public ActionResult GenerateCPSPmonthlyProgressReportNew(string Qtype, string OfficeId, string DateTo, string ZoneID, string AreaID, string BranchID, string EmployeeID, string memberID, string centerID, string ReportType, string ReportName)
        //{
        //    try
        //    {

        //        var reportParam = new Dictionary<string, object>();
        //        //reportParam.Add("OrgName", ApplicationSettings.OrganiztionName);
        //        //reportParam.Add("FromDate", from_date);

        //        DateTime dt = Convert.ToDateTime(DateTo);
        //        int Year = dt.Year;
        //        int Month = dt.Month;

        //        if (memberID == "" || memberID == "null")
        //            memberID = "0";

        //        if (EmployeeID == "")
        //            EmployeeID = "0";


        //        if (ZoneID == "null")
        //            ZoneID = "0";


        //        if (AreaID == "null")
        //            AreaID = "0";


        //        if (BranchID == "null")
        //            BranchID = "0";


        //        var param22 =
        //          new
        //          {
        //              ProcessYear = Year,
        //              ProcessMonth = Month,
        //              OrgID = LoggedInOrganizationID,
        //              LoginUserOfficeID = LoginUserOfficeID,
        //              getOfficeName = SessionHelper.getOfficeName,
        //              LoggedInEmployeeID = SessionHelper.LoggedInEmployeeID,
        //              LoginUserRoleId = SessionHelper.LoginUserRoleId,
        //              OrganizationName = SessionHelper.OrganizationName,
        //              LoggedInOrganizationCode = SessionHelper.LoggedInOrganizationCode,
        //              OrganizationAddress = SessionHelper.OrganizationAddress,
        //              TransactionDay = SessionHelper.TransactionDay,
        //              TransactionDate = SessionHelper.TransactionDate,
        //              TransactionDashBoardString = SessionHelper.TransactionDashBoardString,
        //              ZoneID = Convert.ToInt32(ZoneID),
        //              AreaID = Convert.ToInt32(AreaID),
        //              BranchID = Convert.ToInt32(BranchID),
        //              EmployeeID = Convert.ToInt32(EmployeeID),
        //              QType = Convert.ToInt32(Qtype),
        //              OfficeId = Convert.ToInt32(OfficeId),
        //              centerID = Convert.ToInt32(centerID),
        //              memberID = Convert.ToInt32(memberID),
        //              ReportType = Convert.ToInt32(ReportType),
        //              ReportName = Convert.ToInt32(ReportName)
        //          };

        //        var reportQTYPE = groupwiseReportService.GetDataProcessLOg(param22, "CUSTOM_SP_CPSP_MONTHLY_REPORT_GET_QTYPE");

        //        var QT = reportQTYPE.Tables[0].Rows[0]["QTYPE"].ToString();


        //        var param1 =
        //            new
        //            {
        //                ProcessYear = Year,
        //                ProcessMonth = Month,
        //                OrgID = LoggedInOrganizationID,
        //                LoginUserOfficeID = LoginUserOfficeID,
        //                getOfficeName = SessionHelper.getOfficeName,
        //                LoggedInEmployeeID = SessionHelper.LoggedInEmployeeID,
        //                LoginUserRoleId = SessionHelper.LoginUserRoleId,
        //                OrganizationName = SessionHelper.OrganizationName,
        //                LoggedInOrganizationCode = SessionHelper.LoggedInOrganizationCode,
        //                OrganizationAddress = SessionHelper.OrganizationAddress,
        //                TransactionDay = SessionHelper.TransactionDay,
        //                TransactionDate = SessionHelper.TransactionDate,
        //                TransactionDashBoardString = SessionHelper.TransactionDashBoardString,
        //                ZoneID = Convert.ToInt32(ZoneID),
        //                AreaID = Convert.ToInt32(AreaID),
        //                BranchID = Convert.ToInt32(BranchID),
        //                EmployeeID = Convert.ToInt32(EmployeeID),
        //                QType = Convert.ToInt32(QT),
        //                OfficeId = Convert.ToInt32(OfficeId),
        //                centerID = Convert.ToInt32(centerID),
        //                memberID = Convert.ToInt32(memberID),
        //                ReportType = Convert.ToInt32(ReportType),
        //                ReportName = Convert.ToInt32(ReportName)
        //            };

        //        var reportConfig = groupwiseReportService.GetDataProcessLOg(param1, "CUSTOM_SP_CPSP_MONTHLY_REPORT_SET");

        //        if (reportConfig.Tables.Count > 0)
        //        {
        //            if (reportConfig.Tables[0].Rows.Count > 0)
        //            {
        //                var Id = reportConfig.Tables[0].Rows[0]["Id"].ToString();

        //                var SL = reportConfig.Tables[0].Rows[0]["SL"].ToString();

        //                var Qtype22 = reportConfig.Tables[0].Rows[0]["Qtype"].ToString();

        //                var ReportType22 = reportConfig.Tables[0].Rows[0]["ReportType"].ToString();

        //                var ReportName22 = reportConfig.Tables[0].Rows[0]["ReportName"].ToString();

        //                var SPName = reportConfig.Tables[0].Rows[0]["SPName"].ToString();

        //                var rptName = reportConfig.Tables[0].Rows[0]["rptName"].ToString();

        //                var IsActive = reportConfig.Tables[0].Rows[0]["IsActive"].ToString();

        //                var CreateDate = reportConfig.Tables[0].Rows[0]["CreateDate"].ToString();

        //                var alldata1 = groupwiseReportService.GetDataProcessLOg(param1, SPName);

        //                if (alldata1.Tables.Count > 0)
        //                {
        //                    ReportHelper.PrintReport(rptName, alldata1.Tables[0], reportParam);
        //                    return Content(string.Empty);
        //                }

        //                return Content(string.Empty);
        //            }
        //        }
        //        else
        //        {
        //            return Json(new { Result = "ERROR", Message = "No Data Found!" });
        //        }
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });

        //    }
        //}


        public JsonResult GetReportTypeList()
        {
            var param = new { OfficeID = SessionHelper.LoginUserOfficeID, OrgID = LoggedInOrganizationID, LoginUserEmployeeID = SessionHelper.LoginUserEmployeeID, LoginUserRoleId = SessionHelper.LoginUserRoleId, UserFullName = SessionHelper.UserFullName, LoggedInEmployeeID = SessionHelper.LoggedInEmployeeID };
            var alldata = ultimateReportService.GetDataWithParameter(param, "Proc_Get_CPSP_ReportType");

            var listItems = alldata.Tables[0].AsEnumerable()
                .Select(x => new SelectListItem
                {
                    Value = x.Field<string>("ReportTypeID"),
                    Text = x.Field<string>("ReportTypeName")
                });

            return Json(listItems, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetReportNameList(string ReportType)
        {
            int ReportTypes = 0;
            if (string.IsNullOrEmpty(ReportType))
                ReportTypes = 0;
            else
                ReportTypes = Convert.ToInt32(ReportType);

            var param = new { OfficeID = SessionHelper.LoginUserOfficeID, OrgID = LoggedInOrganizationID, ReportType = ReportTypes, LoginUserEmployeeID = SessionHelper.LoginUserEmployeeID, LoginUserRoleId = SessionHelper.LoginUserRoleId, UserFullName = SessionHelper.UserFullName, LoggedInEmployeeID = SessionHelper.LoggedInEmployeeID };
            var alldata = ultimateReportService.GetDataWithParameter(param, "Proc_Get_CPSP_ReportName");

            var listItems = alldata.Tables[0].AsEnumerable()
                .Select(x => new SelectListItem
                {
                    Value = x.Field<string>("ReportID"),
                    Text = x.Field<string>("ReportName")
                });

            return Json(listItems, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CPSPmonthlyProgressReport()
        {

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["AccCodeList"] = items;
            ViewData["ReportList"] = items;
            ViewData["ReportNameList"] = items;

            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            //ViewData["OfficeLevel"] = offcdetail.OfficeLevel;
            ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];

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
            var allmember = memberService.SearchMember(Convert.ToInt16(LoginUserOfficeID), Convert.ToInt16(LoggedInOrganizationID));
            var viewMember = allmember.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.MemberCode, (string.IsNullOrEmpty(m.FirstName) ? "" : m.FirstName) + ' ' + (string.IsNullOrEmpty(m.MiddleName) ? "" : m.MiddleName) + ' ' + (string.IsNullOrEmpty(m.LastName) ? "" : m.LastName) + ' ' + (string.IsNullOrEmpty(m.RefereeName) ? "" : m.RefereeName)), Value = m.MemberID.ToString() });
            var MemberList = new List<Member>();
            ViewData["Member"] = viewMember;

            return View(model);
        }
        public ActionResult GenerateQuarterlySupplimentary1ReportConsolidation(string DateFrom, string DateTo, string Qtype)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype };
                //var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_Supplementary_Consolidation");
                var alldata = groupwiseReportService.GetDataSupplimentaryReport(param, "Rpt_Supplementary_Consolidation");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rpt_Supplementary_Consolidation_1.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateQuarterlySupplimentary2ReportConsolidation(string DateFrom, string DateTo, string Qtype)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_Supplementary_Consolidation");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rpt_Supplementary_Consolidation_2.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateQuarterlySupplimentary3ReportConsolidation(string DateFrom, string DateTo, string Qtype)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_Supplementary_Consolidation");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rpt_Supplementary_Consolidation_3.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateDailyLoanCollectionNewReport(string Qtype, string Center)
        {
            try
            {
                var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = (string.IsNullOrEmpty(Center) ? "0" : Center) };

                // var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center };
                var alldata = groupwiseReportService.GetDataDailyReport(param, "Rpt_DailyLoanCollection");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("rptDailyLoanCollectionInReportMenu.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateDailyLoanCollectionNewReportExport(string Qtype, string Center)
        {
            var param = new { Qtype = Qtype, Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, Center = Center };
            GridView gv = new GridView();
            var allRepaymentSchedule = groupwiseReportService.GetDataDailyReport(param, "Rpt_DailyLoanCollection");
            var detail = allRepaymentSchedule.Tables[0];
            gv.DataSource = detail;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Rpt_DailyLoanCollection.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            var reportParam = new Dictionary<string, object>();
            reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
            ReportHelper.PrintReport("rptDailyLoanCollectionInReportMenu.rpt", allRepaymentSchedule.Tables[0], reportParam);
            return Content(string.Empty);

        }
        public ActionResult GenerateMonthlyInstallmentScheduleReport(string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo };
                var alldata = groupwiseReportService.GetMonthlyInstallmentScheduleReport(param, "Rpt_MonthlyInstallmentSchedule");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptRpt_MonthlyInstallmentSchedule.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateMonthlyInstallmentScheduleReportExport(string DateFrom, string DateTo)
        {
            var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo };
            groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_MonthlyInstallmentSchedule");

            GridView gv = new GridView();
            var allRepaymentSchedule = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_MonthlyInstallmentSchedule");
            var detail = allRepaymentSchedule.Tables[0];
            gv.DataSource = detail;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=rptRpt_MonthlyInstallmentSchedule.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("MonthlyInstallmentScheduleReport");
        }
        //public ActionResult GenerateEmployeeListReport(string EmployeeStatus)
        //{
        //    try
        //    {
        //        var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, EmployeeStatus = EmployeeStatus };
        //        var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_Employeelist");
        //        var reportParam = new Dictionary<string, object>();
        //        reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //        ReportHelper.PrintReport("RptEmployeeList.rpt", alldata.Tables[0], reportParam);
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        public ActionResult GenerateEmployeeListReport(string EmployeeStatus)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, EmployeeStatus = EmployeeStatus };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_Employeelist_All");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                if (EmployeeStatus == "2")
                    ReportHelper.PrintReport("RptEmployeeListAll.rpt", alldata.Tables[0], reportParam);
                else
                    ReportHelper.PrintReport("RptEmployeeListAllNew.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateSamityListReportWithDay(string CenterStatus, string DaySelect, string ExportType)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, CenterStatus = CenterStatus, DaySelect = DaySelect };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_CenterlistNew");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                //ReportHelper.PrintReport("RptCenterList.rpt", alldata.Tables[0], reportParam);
                // ReportHelper.PrintReport("RptCenterList_Emp.rpt", alldata.Tables[0], reportParam);

                if (ExportType == "pdf")
                    ReportHelper.PrintReport("RptCenterList_Emp.rpt", alldata.Tables[0], reportParam);
                else
                    ReportHelper.ExportExcelReportFormated("RptCenterList_Emp.rpt", alldata.Tables[0], reportParam, "SamityListReportWithDay");

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //public ActionResult GenerateSamityListReportWithDay(string CenterStatus, string DaySelect)
        //{
        //    try
        //    {
        //        var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, CenterStatus = CenterStatus, DaySelect = DaySelect };
        //        var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_CenterlistNew");
        //        var reportParam = new Dictionary<string, object>();
        //        reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //        //ReportHelper.PrintReport("RptCenterList.rpt", alldata.Tables[0], reportParam);
        //       // ReportHelper.PrintReport("RptCenterList_Emp.rpt", alldata.Tables[0], reportParam);
        //        ReportHelper.PrintReport("RptCenterList_Emp.rpt", alldata.Tables[0], reportParam);
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}

        //public ActionResult GenerateSamityListReport(string CenterStatus)
        //{
        //    try
        //    {
        //        var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, CenterStatus = CenterStatus };
        //        var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_Centerlist");
        //        var reportParam = new Dictionary<string, object>();
        //        reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //        ReportHelper.PrintReport("RptCenterList_Emp.rpt", alldata.Tables[0], reportParam);
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        public ActionResult GenerateSamityListReport(string CenterStatus, string ExportType)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = SessionHelper.LoginUserOfficeID, CenterStatus = CenterStatus };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_Centerlist");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);

                if (ExportType == "pdf")
                    ReportHelper.PrintReport("RptCenterList_Emp.rpt", alldata.Tables[0], reportParam);
                else
                    ReportHelper.ExportExcelReport("RptCenterList_Emp.rpt", alldata.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult MemberAgeReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            return View();
        }
        public ActionResult GenerateMemberAgeReport(int StartValue, int EndValue, string ExportType)
        {
            try
            {
                var param = new { StartValue = StartValue, EndValue = EndValue, OfficeID = SessionHelper.LoginUserOfficeID };
                var alldata = groupwiseReportService.GetDataUltimateMemberAgeReport(param, "Rpt_MemberAgeList");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);

                if (ExportType == "pdf")
                    ReportHelper.PrintReport("rptMemberAge.rpt", alldata.Tables[0], reportParam);
                else
                    ReportHelper.ExportExcelReportFormated("rptMemberAge.rpt", alldata.Tables[0], reportParam, "Member_Age_Report");

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //public ActionResult GenerateMemberAgeReport(int StartValue, int EndValue)
        //{
        //    try
        //    {
        //        var param = new { StartValue = StartValue, EndValue = EndValue, OfficeID = SessionHelper.LoginUserOfficeID };
        //        var alldata = groupwiseReportService.GetDataUltimateMemberAgeReport(param, "Rpt_MemberAgeList");
        //        var reportParam = new Dictionary<string, object>();
        //        reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //        ReportHelper.PrintReport("rptMemberAge.rpt", alldata.Tables[0], reportParam);
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        public ActionResult MemberInfo()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            return View();
        }
        public ActionResult GenerateMemberInfoReport(int center, long member)
        {
            try
            {
                var param = new { CenterID = center, MemberID = member };
                var alldata = groupwiseReportService.GetDataMemberInfoReport(param, "Rpt_MemberInfoReport");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("rptMemberInfoReport.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult MonthlySummaryReport()
        {
            int orgid = (int)LoggedInOrganizationID;
            if (orgid == 99)
            //if (orgid == 44)
            {
                IEnumerable<SelectListItem> items = new SelectList(" ");
                ViewData["HOList"] = items;
                ViewData["ZoneList"] = items;
                ViewData["AreaList"] = items;
                ViewData["OfficeList"] = items;

                ViewData["YearList"] = items;

                return View();
            }
            else
            {
                return View("sorry, you do not have permission to access this report");
            }
        }
        public ActionResult GenerateMonthlySummaryReport(int ProcessYear, int ProcessMonth, int OfficeId, int qType)
        {
            try
            {
                var reportParam = new Dictionary<string, object>();
                //reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                var param1 = new { ProcessYear = ProcessYear, ProcessMonth = ProcessMonth, OfficeId = OfficeId, qType = qType };
                var alldata1 = groupwiseReportService.GetProgramMISReportJCF(param1, "Proc_Get_MonthlySummaryReportData");
                ReportHelper.PrintReport("rptMonthlySummaryReport.rpt", alldata1.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }
        }
        //public ActionResult GenerateStaffwiseStatementReport(string DateFrom, string DateTo)
        //{
        //    try
        //    {
        //        var param = new { Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo };
        //        DataSet alldata;
        //        var reportParam = new Dictionary<string, object>();
        //        if (LoggedInOrganizationID == 4 || LoggedInOrganizationID == 5 || LoggedInOrganizationID==20 || LoggedInOrganizationID==53)
        //        {
        //            if (LoggedInOrganizationID==5 || LoggedInOrganizationID == 20)
        //            {
        //                alldata = groupwiseReportService.GetDataUltimateReleaseStaffWiseStatementJCF(param, "Rpt_StaffwiseStatementDSKNew");
        //                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //                reportParam.Add("DateFrom", DateFrom);
        //                reportParam.Add("DateTo", DateTo);
        //                ReportHelper.PrintReport("rptStaffWiseStatementDSKNew.rpt", alldata.Tables[0], reportParam);
        //                return Content(string.Empty);
        //            }
        //            else
        //            {
        //                alldata = groupwiseReportService.GetDataUltimateReleaseStaffWiseStatement(param, "Rpt_StaffwiseStatementDSKNew");
        //                //alldata = groupwiseReportService.GetDataUltimateReleaseStaffWiseStatementDSK(param, "Rpt_StaffwiseStatementDSKNew");
        //                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //                reportParam.Add("DateFrom", DateFrom);
        //                reportParam.Add("DateTo", DateTo);
        //                ReportHelper.PrintReport("rptStaffWiseStatementDSKNew.rpt", alldata.Tables[0], reportParam);
        //                return Content(string.Empty);
        //            }
        //        }
        //        else
        //        {
        //            if (LoggedInOrganizationID == 5)
        //            {
        //                alldata = groupwiseReportService.GetDataUltimateReleaseStaffWiseStatementJCF(param, "Rpt_StaffwiseStatement");
        //            }
        //            else if(LoggedInOrganizationID == 99)
        //            {
        //                alldata = groupwiseReportService.GetDataUltimateReleaseStaffWiseStatementJCF(param, "Rpt_StaffwiseStatementProshika");
        //            }
        //            else
        //                alldata = groupwiseReportService.GetDataUltimateReleaseStaffWiseStatement(param, "Rpt_StaffwiseStatement");

        //            if (LoggedInOrganizationID == 99)
        //            {
        //                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //                reportParam.Add("DateFrom", DateFrom);
        //                reportParam.Add("DateTo", DateTo);
        //                ReportHelper.PrintReport("rptStaffWiseStatementProshika.rpt", alldata.Tables[0], reportParam);
        //                return Content(string.Empty);
        //            }
        //            else
        //            {
        //                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //                reportParam.Add("DateFrom", DateFrom);
        //                reportParam.Add("DateTo", DateTo);
        //                ReportHelper.PrintReport("rptStaffWiseStatementNew.rpt", alldata.Tables[0], reportParam);
        //                return Content(string.Empty);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        [HttpGet]
        public JsonResult GetGroupwiseReportData(string DateFrom, string DateTo)
        {
            try
            {
                var filter = new SearchFilterData { OfficeID = (int)SessionHelper.LoginUserOfficeID, DateFrom = Convert.ToDateTime(DateFrom), DateTo = Convert.ToDateTime(DateTo) };
                var listings = employeespService.GetStaffwiseStatementDSKNewByFilter(filter);
                return Json(listings, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateStaffwiseStatementReport(string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo };
                DataSet alldata;
                var reportParam = new Dictionary<string, object>();
                if (LoggedInOrganizationID == 4 || LoggedInOrganizationID == 20 || LoggedInOrganizationID == 53)
                {
                    if (LoggedInOrganizationID == 20)
                    {
                        alldata = groupwiseReportService.GetDataUltimateReleaseStaffWiseStatementJCF(param, "Rpt_StaffwiseStatementDSKNew");
                        reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                        reportParam.Add("DateFrom", DateFrom);
                        reportParam.Add("DateTo", DateTo);
                        ReportHelper.PrintReport("rptStaffWiseStatementDSKNew.rpt", alldata.Tables[0], reportParam);
                        return Content(string.Empty);

                        //var paramValues = new List<ParameterValue>();
                        //paramValues.Add(new ParameterValue() { Name = "OrgName", Value = ApplicationSettings.OrganiztionName });
                        //paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = DateFrom });
                        //paramValues.Add(new ParameterValue() { Name = "DateTo", Value = DateTo });
                        //paramValues.Add(new ParameterValue() { Name = "Office", Value = SessionHelper.LoginUserOfficeID.ToString() });
                        //PrintSSRSReport("/gBanker_Reports/StaffwiseStatementDSKNew", paramValues.ToArray(), "gBankerReport");
                        //return Content(string.Empty);
                    }
                    else
                    {
                        alldata = groupwiseReportService.GetDataUltimateReleaseStaffWiseStatementDSK(param, "Rpt_StaffwiseStatementDSKNew");
                        reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                        reportParam.Add("DateFrom", DateFrom);
                        reportParam.Add("DateTo", DateTo);
                        ReportHelper.PrintReport("rptStaffWiseStatementDSKNew.rpt", alldata.Tables[0], reportParam);
                        return Content(string.Empty);

                        //var paramValues = new List<ParameterValue>();
                        //paramValues.Add(new ParameterValue() { Name = "OrgName", Value = ApplicationSettings.OrganiztionName });
                        //paramValues.Add(new ParameterValue() { Name = "DateFrom", Value = DateFrom });
                        //paramValues.Add(new ParameterValue() { Name = "DateTo", Value = DateTo });
                        //paramValues.Add(new ParameterValue() { Name = "Office", Value = SessionHelper.LoginUserOfficeID.ToString() });
                        //PrintSSRSReport("/gBanker_Reports/StaffwiseStatementDSKNew", paramValues.ToArray(), "gBankerReport");
                        //return Content(string.Empty);
                    }

                }
                else
                {
                    if (LoggedInOrganizationID == 5)
                    {
                        alldata = groupwiseReportService.GetDataUltimateReleaseStaffWiseStatementJCF(param, "Rpt_StaffwiseStatement");
                    }
                    else if (LoggedInOrganizationID == 99)
                    {
                        alldata = groupwiseReportService.GetDataUltimateReleaseStaffWiseStatementJCF(param, "Rpt_StaffwiseStatementProshika");
                    }
                    else if (LoggedInOrganizationID==12)
                    {
                        alldata = groupwiseReportService.GetDataUltimateReleaseStaffWiseStatementGUK(param, "Rpt_StaffwiseStatement");
                    }
                    else
                        alldata = groupwiseReportService.GetDataUltimateReleaseStaffWiseStatement(param, "Rpt_StaffwiseStatement");

                    if (LoggedInOrganizationID == 99)
                    {
                        reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                        reportParam.Add("DateFrom", DateFrom);
                        reportParam.Add("DateTo", DateTo);
                        ReportHelper.PrintReport("rptStaffWiseStatementProshika.rpt", alldata.Tables[0], reportParam);
                        return Content(string.Empty);
                    }
                    else
                    {

                        reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                        reportParam.Add("DateFrom", DateFrom);
                        reportParam.Add("DateTo", DateTo);
                        ReportHelper.PrintReport("rptStaffWiseStatementNew.rpt", alldata.Tables[0], reportParam);
                        return Content(string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        //public ActionResult GenerateCtegoryTransferReport(string DateFrom, string DateTo)
        //{
        //    try
        //    {
        //        var param = new { Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo };
        //        var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_CategoryTransfer");
        //        var reportParam = new Dictionary<string, object>();
        //        reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
        //        reportParam.Add("DateFrom", DateFrom);
        //        reportParam.Add("DateTo", DateTo);
        //        ReportHelper.PrintReport("rptCtegoryTransfer.rpt", alldata.Tables[0], reportParam);
        //        return Content(string.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message });
        //    }
        //}
        public ActionResult GenerateCtegoryTransferReport(int centerID = 0, long memberID = 0, string DateFrom = "", string DateTo = "")
        {
            try
            {
                var param = new { centerID = centerID, memberID = memberID, Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_CategoryTransferNew");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", DateFrom);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rptCtegoryTransfer.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateMemberListReportDependsOnCollection(string Qtype, string dateFrom, string dateTo)
        {
            try
            {
                var param = new { Office = SessionHelper.LoginUserOfficeID, Qtype = Qtype, EmpID = LoggedInEmployeeID, DateFrom = dateFrom, DateTo = dateTo };
                var alldata = groupwiseReportService.GenerateMemberListReportDependsOnCollection(param, "Rpt_Collectionwise_MemberInfoEmpWise_New");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("dateFrom", dateFrom);
                reportParam.Add("dateTo", dateTo);
                ReportHelper.PrintReport("RptMemberListDepensOnInstallment.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

       



//------------------------------------------------------------------------------------------------------------
 public ActionResult Supplementary_ConsolidationProcess()
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
            return View();
        }


//-----------------------------------------------------------------------------------------------------------

 public JsonResult GenerateSupplementary_ConsolidationProcess(string DateFrom, string DateTo, string Qtype, string officeId)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = officeId, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_Supplementary_ConsolidationProcess_1");
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


//------------------------------------------------------------------------------------------------------------



public ActionResult GenerateSupplementary_ConsolidationProcessView(string DateFrom, string DateTo, string Qtype, string officeId)
        {
            try
            {
                var param = new { Org = SessionHelper.LoginUserOrganizationID, Office = officeId, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_Supplementary_ConsolidationProcess_1");
                //var reportParam = new Dictionary<string, object>();
                //reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("DateFrom", DateFrom);
                //reportParam.Add("DateTo", DateTo);
                //ReportHelper.PrintReport("Supplementary_1_Final.rpt", alldata.Tables[0], reportParam);
                //return Content(string.Empty);

                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        //----------------------------------------------------------------------------------------------------------

        public ActionResult MRAMonthlyInformation()
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
            return View();
        }

//-------------------------------------------------------------------------------------------------------------

public JsonResult GenerateMRAMonthlyInformation(string DateFrom, string DateTo, string Qtype, string officeId)
        {
            try
            {
                var param = new { OfficeID = officeId, FromDate = DateFrom, ToDate = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "SP_RPT_MRAMonthlyInformationHOCon");

                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


//-----------------------------------------------------------------------------------------------------------


  public ActionResult GenerateMRAMonthlyInformationView(string DateFrom, string DateTo, string Qtype, string officeId)
        {
            try
            {
                var param = new { OfficeID = officeId, FromDate = DateFrom, ToDate = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "SP_RPT_MRAMonthlyInformationHOCon");
                //var reportParam = new Dictionary<string, object>();
                //reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                //reportParam.Add("DateFrom", DateFrom);
                //reportParam.Add("DateTo", DateTo);
                //ReportHelper.PrintReport("Supplementary_1_Final.rpt", alldata.Tables[0], reportParam);
                //return Content(string.Empty);

                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult GenerateMemberListReportDependsOnCollectionByFieldStuff(string Qtype, string dateFrom, string dateTo)
        {
            try
            {
                var param = new { Office = SessionHelper.LoginUserOfficeID, Qtype = Qtype, EmpID = LoggedInEmployeeID, DateFrom = dateFrom, DateTo = dateTo };
                var alldata = groupwiseReportService.GenerateMemberListReportDependsOnCollection(param, "Rpt_Collectionwise_MemberInfoEmpWise_New");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("dateFrom", dateFrom);
                reportParam.Add("dateTo", dateTo);
                ReportHelper.PrintReport("RptMemberListDepensOnInstallmentFieldStuff.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public JsonResult GetEmployeeList()
        {
            var getEmployee = employeeService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.OrgID == LoggedInOrganizationID).OrderBy(e => e.EmployeeCode);
            var viewEmployee = getEmployee.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.EmployeeID.ToString(),
                Text = x.EmployeeCode+'-'+x.EmpName.ToString()
            });
            var emp_items = new List<SelectListItem>();
            if (viewEmployee.ToList().Count > 0)
            {
                emp_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            }
            emp_items.AddRange(viewEmployee);
            return Json(emp_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProductList()
        {

            var getProduct = productService.GetAll().Where(s => s.IsActive == true && s.OrgID == LoggedInOrganizationID).OrderBy(e => e.ProductCode);
            var viewProduct = getProduct.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.ProductID.ToString(),
                Text = x.ProductName.ToString()
            });
            var prod_items = new List<SelectListItem>();
            if (viewProduct.ToList().Count > 0)
            {
                prod_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            }
            prod_items.AddRange(viewProduct);
            return Json(prod_items, JsonRequestBehavior.AllowGet);


        }
        public JsonResult GetOverDueProductList()
        {
            //if (LoggedInOrganizationID == 5 || LoggedInOrganizationID == 58 || LoggedInOrganizationID == 7 || LoggedInOrganizationID == 94 || LoggedInOrganizationID == 5 || LoggedInOrganizationID == 7)
            //{
            List<ProductViewModel> List_MemberPassBookRegisterViewModel = new List<ProductViewModel>();
            var param = new { PaymentFrq = "A", OfficeID = LoginUserOfficeID };
            var div_items = ultimateReportService.GetMainProductListAccordingToOffice(param);
            var viewProduct = div_items.Tables[0].AsEnumerable().Select(row => new SelectListItem
            {
                Value = row.Field<string>("mainproductcode").ToString(),
                Text = row.Field<string>("mainproductcode") + ' ' + row.Field<string>("mainitemname")
            });
            var d_items = new List<SelectListItem>();
            d_items.Add(new SelectListItem() { Text = "Please Select", Value = "0" });
            d_items.AddRange(viewProduct);
            return Json(d_items, JsonRequestBehavior.AllowGet);



        }
        public JsonResult GetMainProductList()
        {
            try
            {
                List<ProductViewModel> List_ProductViewModel = new List<ProductViewModel>();
                var productList = weeklyReportService.GetMainProductList();

                List_ProductViewModel = productList.Tables[0].AsEnumerable()
                .Select(row => new ProductViewModel
                {
                    MainProductCode = row.Field<string>("MainProductCode"),
                    MainItemName = row.Field<string>("MainItemName")
                }).ToList();

                return Json(List_ProductViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetSavingMainProductList()
        {
            try
            {
                List<ProductViewModel> List_ProductViewModel = new List<ProductViewModel>();
                var productList = weeklyReportService.GetSavingMainProductList();

                List_ProductViewModel = productList.Tables[0].AsEnumerable()
                .Select(row => new ProductViewModel
                {
                    MainProductCode = row.Field<string>("MainProductCode"),
                    MainItemName = row.Field<string>("MainItemName")

                }).ToList();

                return Json(List_ProductViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //public JsonResult GetCenterList()
        //{

        //    var param1 = new { @EmpID = LoggedInEmployeeID };
        //    var LoanInstallMent = unlimitedReportService.GetCenterROleWise(param1);
        //    IEnumerable<Center> getCenter;
        //    if (LoanInstallMent != null)
        //    {
        //        var empType = LoanInstallMent.Tables[0].Rows[0]["Name"].ToString();
        //        if (empType == "FO")
        //        {
        //            getCenter = centerService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.OrgID == LoggedInOrganizationID && s.EmployeeId == LoggedInEmployeeID).OrderBy(e => e.CenterCode);
        //        }
        //        else
        //            getCenter = centerService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.OrgID == LoggedInOrganizationID).OrderBy(e => e.CenterCode);
        //    }
        //    else
        //            getCenter = centerService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.OrgID == LoggedInOrganizationID).OrderBy(e => e.CenterCode);


        //    var viewCenter = getCenter.Select(x => x).ToList().Select(x => new SelectListItem
        //    {
        //        Value = x.CenterID.ToString(),
        //        Text = x.CenterCode.ToString() + ", " + x.CenterName.ToString()
        //    });
        //    var center_items = new List<SelectListItem>();
        //    if (viewCenter.ToList().Count > 0)
        //    {
        //        center_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
        //    }
        //    center_items.AddRange(viewCenter);
        //    return Json(center_items, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult GetCenterList()
        {
            var param1 = new { @EmpID = LoggedInEmployeeID };
            var LoanInstallMent = unlimitedReportService.GetCenterROleWise(param1);
            List<CenterViewModel> List_CenterViewModel = new List<CenterViewModel>();
            DataSet div_items;
            var param = new { OfficeId = LoginUserOfficeID };

            if (LoanInstallMent != null)
            {
                var empType = LoanInstallMent.Tables[0].Rows[0]["Name"].ToString();
                if (empType == "FO")
                {
                    var paramFOWISE = new { OfficeId = LoginUserOfficeID, EmpID = LoggedInEmployeeID, empType = empType };
                    div_items = groupwiseReportService.GetDataDataseAccess(paramFOWISE, "GetOnlyCenterFOWISE");
                }
                else
                    div_items = groupwiseReportService.GetDataDataseAccess(param, "GetOnlyCenter");
            }
            else
            {
                param = new { OfficeId = LoginUserOfficeID };
                div_items = groupwiseReportService.GetDataDataseAccess(param, "GetOnlyCenter");
            }

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
                Text = x.CenterCode.ToString() + ", " + x.CenterName.ToString()
            });
            var center_items = new List<SelectListItem>();
            if (viewCenter.ToList().Count > 0)
            {
                center_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            }
            center_items.AddRange(viewCenter);
            return Json(center_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOfficeList()
        {
            var getOffice = officeService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.OrgID == LoggedInOrganizationID && s.OfficeLevel == 4).OrderBy(e => e.OfficeCode);
            var viewOffice = getOffice.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeName.ToString()
            });
            var office_items = new List<SelectListItem>();
            if (viewOffice.ToList().Count > 0)
            {
                office_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            }
            office_items.AddRange(viewOffice);
            return Json(office_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMemberList(int centerId)
        {
            try
            {
                List<GetMemberListViewModel> List_Members = new List<GetMemberListViewModel>();

                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, CenterID = centerId };
                var alldata = groupwiseReportService.GetDataLedgerReport(param, "GetMemberList_Dropdown");

                List_Members = alldata.Tables[0].AsEnumerable()
                .Select(row => new GetMemberListViewModel
                {
                    MemberID = row.Field<string>("MemberID"),
                    //LoanTerm = row.Field<string>("LoanTerm"),
                    MemberName = row.Field<string>("MemberName")
                }).ToList();

                return Json(List_Members, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetMemberListByMemberCode(int centerId)
        {
            var getMember = memberService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.MemberStatus == "1" && s.OrgID == LoggedInOrganizationID && s.CenterID == centerId).OrderBy(e => e.MemberCode);


            var viewMember = getMember.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.MemberID.ToString(),
                //Text = x.MemberCode.ToString() + ", " + x.FirstName.ToString() + " " + (string.IsNullOrEmpty(x.MiddleName) ? " " : x.MiddleName.ToString()) + " " + (string.IsNullOrEmpty(x.LastName) ? " " : x.LastName.ToString())
                Text = x.MemberCode.ToString() + ", " + (string.IsNullOrEmpty(x.FirstName) ? " " : x.FirstName.ToString()) + " " + (string.IsNullOrEmpty(x.MiddleName) ? " " : x.MiddleName.ToString()) + " " + (string.IsNullOrEmpty(x.LastName) ? " " : x.LastName.ToString())
                //Text = x.MemberCode.ToString() + ", " + x.MiddleName.ToString()
                //Text = x.CenterName.ToString()
            });
            var member_items = new List<SelectListItem>();
            if (viewMember.ToList().Count > 0)
            {
                member_items.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            }
            member_items.AddRange(viewMember);
            return Json(member_items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOverduetypeList()
        {
            try
            {
                List<OverDueTypeViewModel> List_ProductViewModel = new List<OverDueTypeViewModel>();
                var param = new { OrgID = SessionHelper.LoginUserOrganizationID, OfficeID = SessionHelper.LoginUserOfficeID, OfficeIDTO = SessionHelper.LoginUserOfficeID };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "OverduetypeList");
                List_ProductViewModel = alldata.Tables[0].AsEnumerable()
                .Select(row => new OverDueTypeViewModel
                {
                    LoanType = row.Field<string>("LoanType"),
                    OverdueType = row.Field<string>("OverdueType")

                }).ToList();

                return Json(List_ProductViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult MRA4ConsolidateProcess()
        {
            return View();
        }
        public ActionResult Generate__MRA4ConsolidateProcess(string processDate)
        {
            try
            {
                DateTime dateObj = DateTime.ParseExact(processDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                DateTime lastDateOfMonth = new DateTime(dateObj.Year, dateObj.Month, 1).AddMonths(1).AddDays(-1);
                string lastDateOfMonthString = lastDateOfMonth.ToString("dd-MMM-yyyy");

                var param = new { @OfficeID = SessionHelper.LoginUserOfficeID, @OfficeFrom = 00, @OfficeTo = 999999, @ProcessDate = lastDateOfMonthString, @EmployeeID = LoggedInEmployeeID };
                var processCheck = ultimateReportService.GetDataWithParameter(param, "Proc_MRA4_ConsolidateProcess_Check");

                var processResult = processCheck.Tables[0].AsEnumerable()
                    .Select(row => new ProcessResponseViewModel
                    {
                        Message = row.Field<string>("Msg"),
                        IsRunning = row.Field<string>("IsRunning"),
                    }).FirstOrDefault();

                return Json(new { Response = processResult }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
       
        public JsonResult GetProductListByMemberWithProcedure(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<MemberwiseProductAndLoanTermViewModel> List_MemberwiseProduct = new List<MemberwiseProductAndLoanTermViewModel>();
                var param = new { Qtype = 1, MemberID = MemberID, ProductID = 0 };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "MemberwiseProductAndLoanTermforDropDown");

                List_MemberwiseProduct = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndLoanTermViewModel
                {
                    ProductID = row.Field<string>("ProductID"),
                    //LoanTerm = row.Field<string>("LoanTerm"),
                    ProductName = row.Field<string>("ProductName")

                }).ToList();

                return Json(List_MemberwiseProduct, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetLoanTermListByProductandMemberWithProcedure(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<MemberwiseProductAndLoanTermViewModel> List_LoanTermMemberandProductwise = new List<MemberwiseProductAndLoanTermViewModel>();

                var param = new { Qtype = 2, MemberID = MemberID, ProductID = ProductID };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "MemberwiseProductAndLoanTermforDropDown");

                List_LoanTermMemberandProductwise = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndLoanTermViewModel
                {
                    ProductID = row.Field<string>("ProductID"),
                    LoanTerm = row.Field<string>("LoanTerm"),
                }).ToList();
                return Json(List_LoanTermMemberandProductwise, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetProductListByMemberFromSavingTrx(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<MemberwiseProductAndAccountNoViewModel> List_MemberwiseProduct = new List<MemberwiseProductAndAccountNoViewModel>();
                var param = new { Qtype = 1, MemberID = MemberID, ProductID = 0 };
                var alldata = groupwiseReportService.GetDataLedgerReport(param, "MemberwiseProductAndAccountNoforDropDown");
                List_MemberwiseProduct = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndAccountNoViewModel
                {
                    ProductID = row.Field<string>("ProductID"),
                    ProductName = row.Field<string>("ProductName")

                }).ToList();

                return Json(List_MemberwiseProduct, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetProductListByMemberFromSavingTrxByMemberCode(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<MemberwiseProductAndAccountNoViewModel> List_MemberwiseProduct = new List<MemberwiseProductAndAccountNoViewModel>();
                var param = new { Qtype = 1, MemberID = MemberID, ProductID = 0 };
                var alldata = groupwiseReportService.GetDataLedgerReport(param, "MemberwiseProductAndAccountNoforDropDown");
                List_MemberwiseProduct = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndAccountNoViewModel
                {
                    ProductID = row.Field<string>("ProductID"),
                    //LoanTerm = row.Field<string>("LoanTerm"),
                    ProductName = row.Field<string>("ProductName")
                }).ToList();

                return Json(List_MemberwiseProduct, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetNoOfAccountListByProductandMemberFromSavingTrx(int Qtype, string MemberID, string ProductID)
        {
            try
            {
                List<MemberwiseProductAndAccountNoViewModel> List_AccountNoMemberandProductwise = new List<MemberwiseProductAndAccountNoViewModel>();
                var param = new { Qtype = 2, MemberID = MemberID, ProductID = ProductID };
                var alldata = groupwiseReportService.GetDataLedgerReport(param, "MemberwiseProductAndAccountNoforDropDown");
                List_AccountNoMemberandProductwise = alldata.Tables[0].AsEnumerable()
                .Select(row => new MemberwiseProductAndAccountNoViewModel
                {
                    ProductID = row.Field<string>("ProductID"),
                    NoOfAccount = row.Field<string>("NoOfAccount"),
                    //ProductName = row.Field<string>("ProductName")
                }).ToList();

                //return Json(List_MemberwiseProduct.ToList(), JsonRequestBehavior.AllowGet);
                return Json(List_AccountNoMemberandProductwise, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GeneratePOMIS_5AConsolidationReport(int OfficeId, string DateTo, int? zone_id, int? area_id)
        {
            try
            {
                var officeCode = "";
                officeCode = officeService.GetById(OfficeId).OfficeCode;
                if (zone_id == 200000 || area_id == 300000)
                {
                    var param = new { OfficeCode = officeCode, DateTo = DateTo, ZoneCode = zone_id, AreaCode = area_id };
                    var data = groupwiseReportService.GetDataUltimateReleaseReport(param, "SP_GroupWisePOMIS_5A_Consolidation");
                    var reportParam = new Dictionary<string, object>();
                    ReportHelper.PrintReport("rpt_POMIS5aNote4_New.rpt", data.Tables[0], reportParam);
                }
                else
                {
                    var param = new { OfficeCode = officeCode, DateTo = DateTo, ZoneCode = zone_id, AreaCode = area_id };
                    var data = groupwiseReportService.GetDataUltimateReleaseReport(param, "SP_GroupWisePOMIS_5A_Consolidation");
                    var reportParam = new Dictionary<string, object>();
                    ReportHelper.PrintReport("rpt_POMIS5aNote4_New.rpt", data.Tables[0], reportParam);
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Events
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RecoveryStatement()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["EmployeeList"] = items;
            ViewData["ProductList"] = items;
            ViewData["CenterList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult OverDueAgeing()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["EmployeeList"] = items;
            ViewData["ProductList"] = items;
            ViewData["CenterList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult OverDueAgeing_DSK()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["EmployeeList"] = items;
            ViewData["ProductList"] = items;
            ViewData["CenterList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult OvedueAgeingReport_JCF_OverDueType()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["EmployeeList"] = items;
            ViewData["ProductList"] = items;
            ViewData["CenterList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["OverduetypeList"] = items;
            return View();
        }
        public ActionResult OvedueAgeingReport_NoOfBorrowers()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["EmployeeList"] = items;
            ViewData["ProductList"] = items;
            ViewData["CenterList"] = items;
            ViewData["OfficeList"] = items;
            ViewData["OverduetypeList"] = items;
            return View();
        }
        public ActionResult ProvisionCalculation()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["EmployeeList"] = items;
            ViewData["ProductList"] = items;
            ViewData["CenterList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult StaffWiseSpecialSaving()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["EmployeeList"] = items;
            ViewData["ProductList"] = items;
            ViewData["CenterList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult StaffWiseSpecialSavingDue()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["EmployeeList"] = items;
            ViewData["ProductList"] = items;
            ViewData["CenterList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult StaffWiseSpecialSavingJCF()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["EmployeeList"] = items;
            ViewData["ProductList"] = items;
            ViewData["CenterList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult LoanAndSavingsBalance()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["EmployeeList"] = items;
            ViewData["ProductList"] = items;
            ViewData["CenterList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult OrganizerWiseRecoveryStatement()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["EmployeeList"] = items;
            ViewData["ProductList"] = items;
            ViewData["CenterList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult DailySavingCollectionReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            return View();
        }
        public ActionResult TodaysSummaryGroupWiseReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["EmployeeList"] = items;
            ViewData["ProductList"] = items;
            ViewData["CenterList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult RecoveryReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            return View();
        }
        public ActionResult RecoveryReportGUK()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            return View();
        }
        public ActionResult MiscellaneousNew()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            return View();
        }
        public ActionResult MRADBMS4()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            return View();
        }
        public ActionResult MRADBMS2()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            return View();
        }
        public ActionResult LoanApprovalReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            ViewData["ProductList"] = items;
            return View();
        }
        public ActionResult MemberwiseSavingInterestReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            return View();
        }
        public ActionResult MonthwiseSavingInterestReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            return View();
        }
        public ActionResult YearlyLoanClosingReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            return View();
        }
        public ActionResult YearlySavingClosingReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            return View();
        }
        public ActionResult SamitywiseRecoveryReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            return View();
        }
        public ActionResult SavingBalanceInfo()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["ProductList"] = items;
            return View();
        }
        public ActionResult OrganizerwiseRecoveryReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            return View();
        }
        public ActionResult ExpireListReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            return View();
        }
        public ActionResult POMIS1HQConsolidateReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            return View();
        }
        public ActionResult POMIS2HQConsolidateReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            return View();
        }
        //public ActionResult GeneratePOMIS2HQConsolidateOfficewise()
        //{
        //    IEnumerable<SelectListItem> items = new SelectList(" ");
        //    ViewData["HOList"] = items;
        //    ViewData["ZoneList"] = items;
        //    ViewData["AreaList"] = items;
        //    ViewData["OfficeList"] = items;
        //    return View();
        //}
        public ActionResult POMIS2HQConsolidateReportOfficewiseOnlyMaincategory()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult POMIS2HQConsolidateReportPKSFNonPKSF()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult POMIS2HQConsolidateReportOfficewise()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;

            //var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoggedInEmployee.OfficeID));
            //var HeadOfficeCode = offcdetail.FirstLevel;
            //var Headoffcdetail = officeService.GetByOfficeCode(HeadOfficeCode);
            //var Secondoffcdetail = officeService.GetByOfficeCode(offcdetail.SecondLevel);
            //var Thirdoffcdetail = officeService.GetByOfficeCode(offcdetail.ThirdLevel);
            //var Fourthoffcdetail = officeService.GetByOfficeCode(offcdetail.FourthLevel);

            //ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
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
            var param = new { @OfficeID = SessionHelper.LoggedInEmployee.OfficeID };
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
            return View();
            //IEnumerable<SelectListItem> items = new SelectList(" ");
            //ViewData["HOList"] = items;
            //ViewData["ZoneList"] = items;
            //ViewData["AreaList"] = items;
            //ViewData["OfficeList"] = items;
            //return View();
        }
        public ActionResult POMIS2HQConsolidateReportOfficewiseUsingBranchreport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult OfficeAtAGalance()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["dateFrom"] = TransactionDate.ToString("dd-MMM-yyyy");
                ViewData["dateTo"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["dateFrom"] = VDate.ToString("dd-MMM-yyyy");
                ViewData["dateTo"] = VDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }
        public ActionResult POMIS_5_A_3_HQConsolidateReport()
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

            return View();
        }
        public ActionResult POMIS_5_A_4_HQConsolidateReport()
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

            return View();
        }
        public ActionResult QuarterlyBalanceReportDecline()
        {
            DateTime VDate;
            IEnumerable<SelectListItem> items = new SelectList(" ");

            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            }
            ViewData["CenterList"] = items;

           

            return View();
        }
        public ActionResult DailyRecoverableRecipt()
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

            return View();
        }
        public ActionResult DailyRecoverableRecipt_FOWise()
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

            return View();
        }
        public ActionResult QuarterlyBalanceReportDeclineConsolidate()
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

            return View();
        }
        public ActionResult QuarterlyBalanceReportFlat()
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

            return View();
        }
        public ActionResult MonthlySavingsCollectionSheetReport()
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

        public JsonResult GetSavingsProductList()
        {
            try
            {
                var result_set = ultimateReportService.GetDataWithoutParameter("Proc_GetSavingsProductList");
                var resul_List = result_set.Tables[0].AsEnumerable()
                    .Select(x => new SelectListItem()
                    {
                        Text = x.Field<string>("ItemName"),
                        Value = x.Field<string>("ItemCode")
                    }).ToList();

                var List = new List<SelectListItem>();
                List.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
                List.AddRange(resul_List);

                return Json(List.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult StaffProfileReport()
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
            return View();
        }
        public ActionResult AdvanceAdjustmentReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            return View();
        }
        public ActionResult AdvanceAdjustmentDateRangeReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            return View();
        }
        public ActionResult ServiceChargeStatement()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            return View();
        }
        public ActionResult SavingsPortFolioStatement()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            return View();
        }
        public ActionResult AccountCloseInformationReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
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
        public ActionResult MonthWiseCollectionSheetFortnightlyReport()
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
            ViewData["CenterList"] = items;

            //IEnumerable<SelectListItem> items = new SelectList(" ");

            return View();
        }
        public ActionResult LoanApprovalReportGroupwise()
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
            ViewData["CenterList"] = items;

            return View();
        }
        public ActionResult MemberBalanceInfoReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            return View();
        }
        public ActionResult MemberCard()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            return View();
        }
        public ActionResult GroupLedgerLoanSamitywise()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            return View();
        }
        public ActionResult GroupLedgerSavingSamitywise()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            return View();
        }
        public ActionResult LoanLedgerMemberwise()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            ViewData["ProductListByMember"] = items;
            ViewData["LoanTermList"] = items;

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
        public ActionResult GenerateFOWiseChangesReport_RDRS(string Qtype, string OfficeId, string DateFrom, string DateTo, string CenterID)
        {
            try
            {
                var param = new { Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo, Qtype = Qtype, CenterId = Convert.ToInt32(CenterID) };
                var alldata = groupwiseReportService.GetDataProcessLOg(param, "Proc_RPT_ChangesReport_program");
                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("RPT_ChangesReport1", alldata.Tables[0]);
                ReportHelper.PrintWithSubReport("RPT_ChangesReportCOwise.rpt", alldata.Tables[0], new Dictionary<string, object>(), subReportDB, new RPT_ChangesReportCOwise());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult FOWiseChangesReport_RDRS()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var model = new MemberViewModel();
            model.OfficeID = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            var blnk_items = new List<SelectListItem>();
            model.CenterList = blnk_items;
            return View(model);
        }

        public ActionResult SavingLedgerMemberwise()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            ViewData["ProductListByMember"] = items;
            ViewData["NoOfAccountList"] = items;
            return View();
        }
        public ActionResult SavingLedgerCenterwise()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            ViewData["MainProductView"] = items;
            return View();
        }
        public ActionResult LoanSavingAccountInfoCenterWise()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            //ViewData["MainProductView"] = items;
            return View();
        }
        public JsonResult GetMainSavingProductList()
        {
            try
            {
                List<ProductViewModel> List_ProductViewModel = new List<ProductViewModel>();
                var productList = weeklyReportService.GetSavingMainProductListForCenterWise();

                List_ProductViewModel = productList.Tables[0].AsEnumerable()
                .Select(row => new ProductViewModel
                {
                    MainProductCode = row.Field<string>("MainProductCode"),
                    MainItemName = row.Field<string>("MainItemName")

                }).ToList();

                return Json(List_ProductViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ProcessInfoReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            return View();
        }
        public ActionResult ProcessInfoBranchwiseReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            return View();
        }
        public ActionResult MemberAllLoanInformationReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            return View();
        }
        public ActionResult SavingWithdrawalMemberwise()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            return View();
        }
        public ActionResult SavingWithdrawalMemberwiseBeforeDayEnd()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            return View();
        }
        public ActionResult SavingWithdrawal()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            return View();
        }
        public ActionResult SavingsClose()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            return View();
        }
        public ActionResult LoanDisburse()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            ViewData["ProductList"] = items;
            return View();
        }
        public ActionResult LoanDisburseProductWise()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            ViewData["MainProductView"] = items;
            return View();
        }
        public ActionResult POMIS3_OfficeWiseReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;

            //var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoggedInEmployee.OfficeID));
            //var HeadOfficeCode = offcdetail.FirstLevel;
            //var Headoffcdetail = officeService.GetByOfficeCode(HeadOfficeCode);
            //var Secondoffcdetail = officeService.GetByOfficeCode(offcdetail.SecondLevel);
            //var Thirdoffcdetail = officeService.GetByOfficeCode(offcdetail.ThirdLevel);
            //var Fourthoffcdetail = officeService.GetByOfficeCode(offcdetail.FourthLevel);

            //ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
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
            var param = new { @OfficeID = SessionHelper.LoggedInEmployee.OfficeID };
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
            return View();

            //IEnumerable<SelectListItem> items = new SelectList(" ");
            //ViewData["HOList"] = items;
            //ViewData["ZoneList"] = items;
            //ViewData["AreaList"] = items;
            //ViewData["OfficeList"] = items;

            //var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            //var HeadOfficeCode = offcdetail.FirstLevel;
            //var Headoffcdetail = officeService.GetByOfficeCode(HeadOfficeCode);
            //var Secondoffcdetail = officeService.GetByOfficeCode(offcdetail.SecondLevel);
            //var Thirdoffcdetail = officeService.GetByOfficeCode(offcdetail.ThirdLevel);
            //var Fourthoffcdetail = officeService.GetByOfficeCode(offcdetail.FourthLevel);

            //ViewData["OfficeLevel"] = offcdetail.OfficeLevel;

            //if (offcdetail.OfficeLevel == 1)
            //{
            //    ViewData["FirstLevel"] = Headoffcdetail.OfficeID;
            //    ViewData["SecondLevel"] = "";
            //    ViewData["ThirdLevel"] = "";
            //    ViewData["FourthLevel"] = "";
            //}
            //else if (offcdetail.OfficeLevel == 2)
            //{
            //    ViewData["FirstLevel"] = Headoffcdetail.OfficeID;
            //    ViewData["SecondLevel"] = Secondoffcdetail.OfficeID;
            //    ViewData["ThirdLevel"] = "";
            //    ViewData["FourthLevel"] = "";
            //}
            //else if (offcdetail.OfficeLevel == 3)
            //{
            //    ViewData["FirstLevel"] = Headoffcdetail.OfficeID;
            //    ViewData["SecondLevel"] = Secondoffcdetail.OfficeID;
            //    ViewData["ThirdLevel"] = Thirdoffcdetail.OfficeID;
            //    ViewData["FourthLevel"] = "";
            //}
            //else
            //{
            //    ViewData["FirstLevel"] = Headoffcdetail.OfficeID;
            //    ViewData["SecondLevel"] = Secondoffcdetail.OfficeID;
            //    ViewData["ThirdLevel"] = Thirdoffcdetail.OfficeID;
            //    ViewData["FourthLevel"] = Fourthoffcdetail.OfficeID;

            //}
            //var param = new { @OfficeID = SessionHelper.LoggedInEmployee.OfficeID };
            //var allProducts = accReportService.GetLastInitialDate(param);
            //var detail = allProducts.ToString();

            //if (!IsDayInitiated)
            //{
            //    ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            //}
            //else
            //{
            //    ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            //}
            //return View();
            //IEnumerable<SelectListItem> items = new SelectList(" ");
            //ViewData["HOList"] = items;
            //ViewData["ZoneList"] = items;
            //ViewData["AreaList"] = items;
            //ViewData["OfficeList"] = items;
            //return View();
        }
        public ActionResult POMIS3_OverDueType()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult POMIS3_PKSFNonPKSF()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult MRA4HQConsolidateOfficewise()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult ProvisionCalculationConsolidate()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult POMIS1HQConsolidateOfficewise()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;

            //var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoggedInEmployee.OfficeID));
            //var HeadOfficeCode = offcdetail.FirstLevel;
            //var Headoffcdetail = officeService.GetByOfficeCode(HeadOfficeCode);
            //var Secondoffcdetail = officeService.GetByOfficeCode(offcdetail.SecondLevel);
            //var Thirdoffcdetail = officeService.GetByOfficeCode(offcdetail.ThirdLevel);
            //var Fourthoffcdetail = officeService.GetByOfficeCode(offcdetail.FourthLevel);

            //ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];
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
            var param = new { @OfficeID = SessionHelper.LoggedInEmployee.OfficeID };
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
            return View();
            //IEnumerable<SelectListItem> items = new SelectList(" ");
            //ViewData["HOList"] = items;
            //ViewData["ZoneList"] = items;
            //ViewData["AreaList"] = items;
            //ViewData["OfficeList"] = items;
            //return View();
        }
        public ActionResult StaffProfileConsolidate()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult LoanDisburseConsolidation()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult POMIS1HQPKSFNONPKSF()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult ChangesReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID));
            var HeadOfficeCode = offcdetail.FirstLevel;
            var Headoffcdetail = officeService.GetByOfficeCode(HeadOfficeCode);
            var Secondoffcdetail = officeService.GetByOfficeCode(offcdetail.SecondLevel);
            var Thirdoffcdetail = officeService.GetByOfficeCode(offcdetail.ThirdLevel);
            var Fourthoffcdetail = officeService.GetByOfficeCode(offcdetail.FourthLevel);

            ViewData["OfficeLevel"] = offcdetail.OfficeLevel;
            //var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoggedInEmployee.OfficeID));
            //var HeadOfficeCode = offcdetail.FirstLevel;
            //var Headoffcdetail = officeService.GetByOfficeCode(HeadOfficeCode);
            //var Secondoffcdetail = officeService.GetByOfficeCode(offcdetail.SecondLevel);
            //var Thirdoffcdetail = officeService.GetByOfficeCode(offcdetail.ThirdLevel);
            //var Fourthoffcdetail = officeService.GetByOfficeCode(offcdetail.FourthLevel);

            //ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];

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
                ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            return View();
            //IEnumerable<SelectListItem> items = new SelectList(" ");
            //ViewData["HOList"] = items;
            //ViewData["ZoneList"] = items;
            //ViewData["AreaList"] = items;
            //ViewData["OfficeList"] = items;

            //var offcdetail = officeService.GetById(Convert.ToInt32(SessionHelper.LoggedInEmployee.OfficeID));
            //var HeadOfficeCode = offcdetail.FirstLevel;
            //var Headoffcdetail = officeService.GetByOfficeCode(HeadOfficeCode);
            //var Secondoffcdetail = officeService.GetByOfficeCode(offcdetail.SecondLevel);
            //var Thirdoffcdetail = officeService.GetByOfficeCode(offcdetail.ThirdLevel);
            //var Fourthoffcdetail = officeService.GetByOfficeCode(offcdetail.FourthLevel);

            //ViewData["OfficeLevel"] = Session[SessionKeys.LOGGED_IN_Employee_Office_Level];

            //if (offcdetail.OfficeLevel == 1)
            //{
            //    ViewData["FirstLevel"] = Headoffcdetail.OfficeID;
            //    ViewData["SecondLevel"] = "";
            //    ViewData["ThirdLevel"] = "";
            //    ViewData["FourthLevel"] = "";
            //}
            //else if (offcdetail.OfficeLevel == 2)
            //{
            //    ViewData["FirstLevel"] = Headoffcdetail.OfficeID;
            //    ViewData["SecondLevel"] = Secondoffcdetail.OfficeID;
            //    ViewData["ThirdLevel"] = "";
            //    ViewData["FourthLevel"] = "";
            //}
            //else if (offcdetail.OfficeLevel == 3)
            //{
            //    ViewData["FirstLevel"] = Headoffcdetail.OfficeID;
            //    ViewData["SecondLevel"] = Secondoffcdetail.OfficeID;
            //    ViewData["ThirdLevel"] = Thirdoffcdetail.OfficeID;
            //    ViewData["FourthLevel"] = "";
            //}
            //else
            //{
            //    ViewData["FirstLevel"] = Headoffcdetail.OfficeID;
            //    ViewData["SecondLevel"] = Secondoffcdetail.OfficeID;
            //    ViewData["ThirdLevel"] = Thirdoffcdetail.OfficeID;
            //    ViewData["FourthLevel"] = Fourthoffcdetail.OfficeID;

            //}
            //var param = new { @OfficeID = SessionHelper.LoggedInEmployee.OfficeID };
            //var allProducts = accReportService.GetLastInitialDate(param);
            //var detail = allProducts.ToString();

            //if (!IsDayInitiated)
            //{
            //    ViewData["TrxDate"] = Convert.ToString("dd-MMM-yyyy");
            //}
            //else
            //{
            //    ViewData["TrxDate"] = TransactionDate.ToString("dd-MMM-yyyy");
            //}
            //return View();

        }
        public ActionResult FOWiseChangesReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult ProgramMISReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult OfficeWiseProgramMISReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult WorkingLogInfo()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult OverdueMemberListNewBeforeDayEndReport()
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
            ViewData["CenterList"] = items;

            //IEnumerable<SelectListItem> items = new SelectList(" ");

            return View();
        }
        public ActionResult QuarterlySupplimentary1()
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
            return View();
        }
        public ActionResult QuarterlySupplimentary2()
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
            return View();
        }
        public ActionResult QuarterlySupplimentary3()
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
            return View();
        }
        public ActionResult QuarterlySupplimentaryConsolidation_1()
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
            return View();
        }
        public ActionResult QuarterlySupplimentaryConsolidation_2()
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
            return View();
        }
        public ActionResult PostWriteOffRegister()
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
            return View();
        }
        public ActionResult AccountReconcileInfo()
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
            return View();
        }
        public ActionResult SpecialLoanCollectionInfo()
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
            return View();
        }
        public ActionResult SpecialSavingCollectionInfo()
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
            return View();
        }
        public ActionResult BalanceCompareCheck()
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
            return View();
        }
        public ActionResult QuarterlySupplimentaryConsolidation_3()
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
            return View();
        }
        public ActionResult DailyLoanCollectionNewReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            return View();
        }
        public ActionResult MonthlyInstallmentScheduleReport()
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
            //ViewData["CenterList"] = items;

            //IEnumerable<SelectListItem> items = new SelectList(" ");

            return View();
        }
        public ActionResult EmployeeListReport()
        {

            return View();
        }
        public ActionResult InactiveMemberListReport()
        {

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["EmployeeList"] = items;

            return View();
        }
        public ActionResult SamityListReport()
        {

            return View();
        }
        public ActionResult StaffwiseStatementReport()
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
            //ViewData["CenterList"] = items;

            //IEnumerable<SelectListItem> items = new SelectList(" ");

            return View();
        }
        public ActionResult HTML_StaffwiseStatementReport()
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
            //ViewData["CenterList"] = items;

            //IEnumerable<SelectListItem> items = new SelectList(" ");

            return View();
        }
        public ActionResult CategoryTransferReport()
        {
            DateTime VDate;
            VDate = System.DateTime.Now;

            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["CenterList"] = items;
            ViewData["MemberList"] = items;
            if (IsDayInitiated)
            {
                ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            }
            //IEnumerable<SelectListItem> items = new SelectList(" ");
            //ViewData["CenterList"] = items;

            //IEnumerable<SelectListItem> items = new SelectList(" ");

            return View();
        }
        public ActionResult MemberListReportDependsOnCollection()
        {
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["DateFrom"] = TransactionDate.ToString("dd-MMM-yyyy");
                ViewData["DateTo"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["DateFrom"] = VDate.ToString("dd-MMM-yyyy");
                ViewData["DateTo"] = VDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }
        public ActionResult Details(int id)
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Delete(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult LoanBalanceReport()
        {
            //DateTime VDate;
            //VDate = System.DateTime.Now;
            //if (IsDayInitiated)
            //{
            //    ViewData["Trxdate"] = TransactionDate.ToString("dd-MMM-yyyy");
            //}
            //else
            //{
            //    ViewData["Trxdate"] = VDate.ToString("dd-MMM-yyyy");
            //}
            //IEnumerable<SelectListItem> items = new SelectList(" ");
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["MainProductView"] = items;
            return View();
        }
        public ActionResult GenerateLoanBalanceReport(string MainProductCode, string DateFrom, string DateTo, string Qtype)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var officeId = SessionHelper.LoginUserOfficeID;
                var param = new { Office = officeId, DateFrom = DateFrom, DateTo = DateTo, Org = orgId, ProductCode = MainProductCode, Qtype = Qtype };
                var data = ultimateReportService.GetDataWithParameter(param, "getLoanBalanceInfo");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("CompanyName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rpt_LoanBalance.rpt", data.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult SavingBalanceReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["MainProductView"] = items;
            return View();
        }
        public ActionResult GenerateSavingBalanceReport(string MainProductCode, string DateFrom, string DateTo, string Qtype)
        {
            try
            {
                var orgId = SessionHelper.LoginUserOrganizationID;
                var officeId = SessionHelper.LoginUserOfficeID;
                var param = new { Office = officeId, DateFrom = DateFrom, DateTo = DateTo, Org = orgId, ProductCode = MainProductCode, Qtype = Qtype };
                var data = ultimateReportService.GetDataWithParameter(param, "getSavingBalanceInfo");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("CompanyName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("rpt_SavingBalance.rpt", data.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult POMIS_5AConsolidation()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult POMIS3_Employment()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult HTML_GenerateDailyRecoverableReceipt()
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


            return View();
        }
        [HttpGet]
        public JsonResult GetDailyRecoverableReceiptData(string Qtype, string DateTo)
        {
            try
            {
                var orgID = SessionHelper.LoginUserOrganizationID;

                orgID = 4;
                if (orgID == 4)
                {
                    var filter = new SearchFilterData { OfficeID = (int)SessionHelper.LoginUserOfficeID, DateTo = Convert.ToDateTime(DateTo), QType = Convert.ToInt32(Qtype), EmployeeId = LoggedInEmployeeID };
                    var listings = employeespService.GetDailyRecoverableReceiptByFilter_NEW(filter);
                    //return Json(listings, JsonRequestBehavior.AllowGet);

                    var Employees = listings.GroupBy(test => test.Employee)
                   .Select(grp => grp.First())
                   .ToList();

                    List<Rpt_DailyCollectionReceiptFoWise_New_JSON> objEmployeeDataList = new List<Rpt_DailyCollectionReceiptFoWise_New_JSON>();

                    foreach (var cur in Employees)
                    {
                        Rpt_DailyCollectionReceiptFoWise_New_JSON objEmployeeData = new Rpt_DailyCollectionReceiptFoWise_New_JSON();
                        objEmployeeData.Employee = cur.Employee;

                        //objEmployeeData.SavingsCollections
                        var SavingsCollections = from s in listings
                                                 where s.Item.Trim() == "Savings Collection".Trim() && s.Employee.Trim() == cur.Employee.Trim()
                                                 select new
                                                 {
                                                     ProductCode = s.Product,
                                                     Recoverable = s.Amount,
                                                     Collection = s.Amount
                                                 };

                        List<GCollectionC> objList = new List<GCollectionC>();
                        foreach (var Col in SavingsCollections)
                        {
                            GCollectionC obj = new GCollectionC();
                            obj.ProductCode = Col.ProductCode;
                            obj.Collection = Col.Collection;
                            obj.Recoverable = Col.Recoverable;
                            objList.Add(obj);
                        }
                        objEmployeeData.SavingsCollections = objList;



                        var LoanCollections = from s in listings
                                              where s.Item.Trim() == "Loan Collection".Trim() && s.Employee.Trim() == cur.Employee.Trim()
                                              select new
                                              {
                                                  ProductCode = s.Product,
                                                  Recoverable = s.Amount,
                                                  Collection = s.Amount
                                              };

                        List<GCollectionC> objList_LoanCollections = new List<GCollectionC>();
                        foreach (var Col in LoanCollections)
                        {
                            GCollectionC obj = new GCollectionC();
                            obj.ProductCode = Col.ProductCode;
                            obj.Collection = Col.Collection;
                            obj.Recoverable = Col.Recoverable;
                            objList_LoanCollections.Add(obj);
                        }
                        objEmployeeData.LoanCollections = objList_LoanCollections;



                        var ServiceChargeCollection = from s in listings
                                                      where s.Item.Trim() == "Service Charge Collection".Trim() && s.Employee.Trim() == cur.Employee.Trim()
                                                      select new
                                                      {
                                                          ProductCode = s.Product,
                                                          Recoverable = s.Amount,
                                                          Collection = s.Amount
                                                      };

                        List<GCollectionC> objList_ServiceChargeCollection = new List<GCollectionC>();
                        foreach (var Col in ServiceChargeCollection)
                        {
                            GCollectionC obj = new GCollectionC();
                            obj.ProductCode = Col.ProductCode;
                            obj.Collection = Col.Collection;
                            obj.Recoverable = Col.Recoverable;
                            objList_LoanCollections.Add(obj);
                        }
                        objEmployeeData.ServiceChargeCollections = objList_ServiceChargeCollection;


                        objEmployeeDataList.Add(objEmployeeData);

                    }// END FOREach


                    var jsonStringDeatils = JsonConvert.SerializeObject(objEmployeeDataList);

                    return Json(jsonStringDeatils, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var filter2 = new SearchFilterData { OfficeID = (int)SessionHelper.LoginUserOfficeID, DateTo = Convert.ToDateTime("2018-02-27"), QType = 1, EmployeeId = 3697 };

                    var listings2 = employeespService.GetDailyRecoverableReceiptByFilter(filter2);



                    var Employees = listings2.GroupBy(test => test.Employee)
                   .Select(grp => grp.First())
                   .ToList();

                    List<Rpt_DailyCollectionReceiptFoWise_New_JSON> objEmployeeDataList = new List<Rpt_DailyCollectionReceiptFoWise_New_JSON>();


                    foreach (var cur in Employees)
                    {
                        Rpt_DailyCollectionReceiptFoWise_New_JSON objEmployeeData = new Rpt_DailyCollectionReceiptFoWise_New_JSON();

                        objEmployeeData.Employee = cur.Employee;


                        //objEmployeeData.SavingsCollections
                        var SavingsCollections = from s in listings2
                                                 where s.Item.Trim() == "Savings Collection".Trim() && s.Employee.Trim() == cur.Employee.Trim()
                                                 select new
                                                 {
                                                     Recoverable = s.Amount,
                                                     Collection = s.Amount
                                                 };

                        List<GCollectionC> objList = new List<GCollectionC>();
                        foreach (var Col in SavingsCollections)
                        {
                            GCollectionC obj = new GCollectionC();
                            obj.Collection = Col.Collection;
                            obj.Recoverable = Col.Recoverable;
                            objList.Add(obj);
                        }
                        objEmployeeData.SavingsCollections = objList;



                        var LoanCollections = from s in listings2
                                              where s.Item.Trim() == "Loan Collection".Trim() && s.Employee.Trim() == cur.Employee.Trim()
                                              select new
                                              {
                                                  Recoverable = s.Amount,
                                                  Collection = s.Amount
                                              };

                        List<GCollectionC> objList_LoanCollections = new List<GCollectionC>();
                        foreach (var Col in LoanCollections)
                        {
                            GCollectionC obj = new GCollectionC();
                            obj.Collection = Col.Collection;
                            obj.Recoverable = Col.Recoverable;
                            objList_LoanCollections.Add(obj);
                        }
                        objEmployeeData.LoanCollections = objList_LoanCollections;



                        var ServiceChargeCollection = from s in listings2
                                                      where s.Item.Trim() == "Service Charge Collection".Trim() && s.Employee.Trim() == cur.Employee.Trim()
                                                      select new
                                                      {
                                                          Recoverable = s.Amount,
                                                          Collection = s.Amount
                                                      };

                        List<GCollectionC> objList_ServiceChargeCollection = new List<GCollectionC>();
                        foreach (var Col in ServiceChargeCollection)
                        {
                            GCollectionC obj = new GCollectionC();
                            obj.Collection = Col.Collection;
                            obj.Recoverable = Col.Recoverable;
                            objList_LoanCollections.Add(obj);
                        }
                        objEmployeeData.ServiceChargeCollections = objList_ServiceChargeCollection;


                        objEmployeeDataList.Add(objEmployeeData);

                    }// END FOREach


                    var jsonStringDeatils = JsonConvert.SerializeObject(objEmployeeDataList);




                    return Json(jsonStringDeatils, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        [HttpGet]
        public JsonResult GetProductListData()
        {
            try
            {
                var orgID = SessionHelper.LoginUserOrganizationID;

                var filter = new SearchFilterData { };
                var listings = employeespService.GetProduct_NEW(filter);

                //Rpt_Product_tbl 
                var jsonStringDeatils = JsonConvert.SerializeObject(listings);
                return Json(jsonStringDeatils, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        /*
        public ActionResult ExcelJson(DataTable dataTable)
        {
            try
            {

                var FileName = "gBankerData_Con";
                var contracttDetailListing = new List<MRA_Contract_DetailsViewModel>();
                var ContractDetailsParam = new { OrgID = LoggedInOrganizationID, OfficeID = LoginUserOfficeID, ACCOUNTINGDATE = ACCOUNTINGDATE };
                var ContractDetails = accreportService.GetAccDataForReport(ContractDetailsParam, "Proc_Get_ContractDetails");

                var listContractDetails = ContractDetails.Tables[0].AsEnumerable()
                .Select(row => new MRA_Contract_DetailsViewModel
                {
                    DATATYPE = "C",
                    RECORDNO = row.Field<string>("RECORDNO"),
                    BRANCH_CODE = row.Field<string>("BRANCH_CODE"),
                    MEMBERID = row.Field<string>("MEMBERID"),
                    LOAN_CODE = row.Field<string>("LOAN_CODE"),
                    LOAN_TYPE = row.Field<string>("LOAN_TYPE"),
                    LOAN_DISBURSEMENT_DATE = row.Field<string>("LOAN_DISBURSEMENT_DATE"),
                    END_DATE_CONTRACT = row.Field<string>("END_DATE_CONTRACT"),
                    LAST_INSTALLMENT_PAID_DATE = row.Field<string>("LAST_INSTALLMENT_PAID_DATE"),
                    DISBURSED_AMOUNT = row.Field<string>("DISBURSED_AMOUNT"),
                    TOTAL_OUTSTANDING_AMT = row.Field<string>("TOTAL_OUTSTANDING_AMT"),
                    PERIODICITY_PAYMENT = row.Field<string>("PERIODICITY_PAYMENT"),
                    TOTAL_NUM_INSTALLMENT = row.Field<string>("TOTAL_NUM_INSTALLMENT"),
                    INSTALLMENT_AMT = row.Field<string>("INSTALLMENT_AMT"),
                    NUM_REMAINING_INSTALLMENT = row.Field<string>("NUM_REMAINING_INSTALLMENT"),
                    NUM_OVERDUE_INSTALLMENT = row.Field<string>("NUM_OVERDUE_INSTALLMENT"),
                    OVERDUE_AMT = row.Field<string>("OVERDUE_AMT"),
                    LOAN_STATUS = row.Field<string>("LOAN_STATUS"),
                    RESCHEDULE_NO = row.Field<string>("RESCHEDULE_NO"),
                    LAST_RESCHEDULE_DATE = row.Field<string>("LAST_RESCHEDULE_DATE"),
                    WRITE_OFF_AMT = row.Field<string>("WRITE_OFF_AMT"),
                    WRITE_OFF_DATE = row.Field<string>("WRITE_OFF_DATE"),
                    CONTRACT_PHASE = row.Field<string>("CONTRACT_PHASE"),
                    LOAN_DURATION = row.Field<string>("LOAN_DURATION"),
                    ACTUAL_END_DATE_CONTRACT = row.Field<string>("ACTUAL_END_DATE_CONTRACT"),
                    ECONOMIC_PURPOSE_CODE = row.Field<string>("ECONOMIC_PURPOSE_CODE"),
                    COMPULSORY_SAVING_AMT = row.Field<string>("COMPULSORY_SAVING_AMT"),
                    VOLUNTARY_SAVING_AMT = row.Field<string>("VOLUNTARY_SAVING_AMT"),
                    TERM_SAVING_AMT = row.Field<string>("TERM_SAVING_AMT"),
                    SUBSIDIZED_CREDIT_FLAG = row.Field<string>("SUBSIDIZED_CREDIT_FLAG"),
                    SERVICE_CHARGE_RATE = row.Field<string>("SERVICE_CHARGE_RATE"),
                    PAYMENT_MODE = row.Field<string>("PAYMENT_MODE"),
                    ADVANCE_PAYMENT_AMT = row.Field<string>("ADVANCE_PAYMENT_AMT"),
                    LAW_SUIT = row.Field<string>("LAW_SUIT"),
                    ME = row.Field<string>("ME"),
                    MEMBER_WELFARE_FUND = row.Field<string>("MEMBER_WELFARE_FUND"),
                    INSURENCE_COVERAGE = row.Field<string>("INSURENCE_COVERAGE"),

                }).ToList();

                var MFICODE = "";
                var ProductionDate = "";

                if (listContractDetails.Count > 0)
                {
                    MFICODE = ContractDetails.Tables[0].Rows[0]["MFICode"].ToString();  //ListContractDetails.Select(l => l.MFICODE).FirstOrDefault();
                    FileName = ContractDetails.Tables[0].Rows[0]["FileNameJSON"].ToString();
                    ACCOUNTINGDATE = ContractDetails.Tables[0].Rows[0]["ACCOUNTINGDATE"].ToString(); //ListContractDetails.Select(l => l.ACCOUNTINGDATE).FirstOrDefault();
                    ProductionDate = ContractDetails.Tables[0].Rows[0]["ProductionDate"].ToString();

                }

                var header = new
                {
                    DATATYPE = "H",
                    MFICODE = MFICODE,
                    ACCOUNTINGDATE = ACCOUNTINGDATE,
                    PRODUCTIONDATE = ProductionDate
                };


                var footer = new
                {
                    DATATYPE = "F",
                    TOTALRECORD = listContractDetails.Count.ToString()
                };

                if (listContractDetails.Any())
                    contracttDetailListing.AddRange(listContractDetails);

                //let's serial as json formatted data 
                var jsonStringHeader = JsonConvert.SerializeObject(header);
                var jsonStringFooter = JsonConvert.SerializeObject(footer);

                var jsonStringDeatils = JsonConvert.SerializeObject(contracttDetailListing);
                jsonStringDeatils = jsonStringDeatils.Replace("[", "");
                jsonStringDeatils = jsonStringDeatils.Replace("]", "");

                var jsonStringFinal = "[" + jsonStringHeader + "," + jsonStringDeatils + "," + jsonStringFooter + "]";

                byte[] fileBytes = Encoding.ASCII.GetBytes(jsonStringFinal);

                string fileName = FileName + ".json";

                return File(fileBytes, "application/json", fileName);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        */
        #endregion

        #region SSS POMIS Report

        public ActionResult POMIS1HQConsolidateOfficeDateWise()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult GeneratePOMIS1HQConsolidateOfficeDateWise(string Qtype, string OfficeId, string DateFrom, string DateTo)
        {
            try
            {

                var param = new { Qtype = Convert.ToInt32(Qtype), OfficeId = OfficeId, DateFrom = DateFrom, DateTo = DateTo };
                var vOrgid = SessionHelper.LoginUserOrganizationID;
                if (vOrgid == 9)
                {
                    groupwiseReportService.GetDataPOMIS1_DataMarge(param, "POMIS2_SavingStatement_ConsolidationWithDate_SSS");
                }
                else
                {
                    groupwiseReportService.GetDataPOMIS1_DataMarge(param, "POMIS2_SavingStatement_ConsolidationWithDate_SSS");
                }

                var param1 = new { Qtype = Convert.ToInt32(1), DateFrom = DateFrom, DateTo = DateTo };
                var param2 = new { Qtype = Convert.ToInt32(2), DateFrom = DateFrom, DateTo = DateTo };
                var param3 = new { Qtype = Convert.ToInt32(4), DateFrom = DateFrom, DateTo = DateTo };
                var param4 = new { Qtype = Convert.ToInt32(3), DateFrom = DateFrom, DateTo = DateTo };
                var POMIS1As = groupwiseReportService.GetDataPOMIS1_GroupAndMembersInfoConsolidationOfficewise(param1, "Proc_getPomis_1_SSS");
                var POMIS1Bs = groupwiseReportService.GetDataPOMIS1_SavingsStatementConsolidationOfficewise(param2, "Proc_getPomis_1_SSS");
                var POMIS1Cs = groupwiseReportService.GetDataPOMIS1_SavingsStatementItemWiseTotalConsolidationOfficewise(param3, "Proc_getPomis_1_SSS");
                var POMIS1Ds = groupwiseReportService.GetDataPOMIS1_AdmisionWithdrawanAttendanceConsolidationOfficewise(param4, "Proc_getPomis_1_SSS");
                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("SubRptSavingsStatement", POMIS1Bs.Tables[0]);
                subReportDB.Add("ItemWiseTotal", POMIS1Cs.Tables[0]);
                subReportDB.Add("SubRptMemberAdmission", POMIS1Ds.Tables[0]);
                ReportHelper.PrintWithSubReport("POMIS_1HQ_SSS.rpt", POMIS1As.Tables[0], new Dictionary<string, object>(), subReportDB, new POMIS_1HQ());
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }



        public ActionResult POMIS2HQConsolidateReportOfficewiseWithDateMaincategory()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }

        public ActionResult GeneratePOMIS2HQConsolidateOfficewiseOnlyMaincategory_SSS(string Qtype, string OfficeId, string DateFrom, string DateTo)
        {
            try
            {
                var param1 = new { Qtype = Convert.ToInt32(Qtype), OfficeID = Convert.ToInt32(OfficeId), Org = SessionHelper.LoginUserOrganizationID, DateTo = DateTo };
                var param2 = new { Qtype = Convert.ToInt32(Qtype), OfficeID = Convert.ToInt32(OfficeId), Org = SessionHelper.LoginUserOrganizationID, DateTo = DateTo };
                var POMIS1As = groupwiseReportService.POMISConsolidationOfficewise(param1, "Rpt_POMIS_ConsolidationOfficewiseOnlyMaincategory_SSS");
                var POMIS1Bs = groupwiseReportService.POMISConsolidationOfficewise(param2, "Rpt_POMIS_ConsolidationOfficewiseOnlyMaincategory_SSS");
                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("SubRptLoanStatementOverdue", POMIS1Bs.Tables[0]);
                ReportHelper.PrintWithSubReport("rpt_POMIS2_LoanStatementHQOfficewiseOnlyMaincategory_SSS.rpt", POMIS1As.Tables[0], new Dictionary<string, object>(), subReportDB, new rpt_POMIS2_LoanStatementHQOfficewiseOnlyMaincategory());

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });

            }
        }



        public ActionResult POMIS3_OfficeWiseWithDateReport()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }

        public ActionResult GeneratePOMIS3_OfficeWiseReport_SSS(string Qtype, string OfficeId, string DateFrom, string DateTo)
        {
            try
            {
                var param = new { Qtype = Convert.ToInt32(Qtype), OfficeID = Convert.ToInt32(OfficeId), Org = SessionHelper.LoginUserOrganizationID, DateFrom = DateFrom, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "Rpt_POMIS3_ConsolidationOfficewise_SSS");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("OverDueAgeingOfficewise_SSS.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        public ActionResult InactiveMemberListReport_DateRange()
        {

            return View();
        }


        public ActionResult GenerateInactiveMemberList_SSS(string DateFrom, string DateTo)
        {
            try
            {
                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportDSK(param, "Proc_Get_InActiveMemberList_DateWise");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);

                //reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("InActiveMemberList_SSS.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateInactiveMemberListDrop_SSS(string DateFrom, string DateTo)
        {
            try
            {
                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportDSK(param, "Proc_Get_InActiveMemberListDrop_SSS");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);

                //reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("InActiveMemberListDropable_SSS.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        #endregion

       
        public ActionResult GenerateInactiveMemberList(string DateTo, string EmployeeId)
        {
            try
            {
                var param = new { OfficeID = SessionHelper.LoginUserOfficeID, EmployeeID = EmployeeId };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReportDSK(param, "Proc_Get_InActiveMemberListEmployeeWise");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);

                //reportParam.Add("DateTo", DateTo);
                ReportHelper.PrintReport("InActiveMemberList.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult MemberWiseSavingsData()
        {
            DateTime VDate;
            VDate = System.DateTime.Now;
            if (IsDayInitiated)
            {
                ViewData["DateFrom"] = TransactionDate.ToString("dd-MMM-yyyy");
                ViewData["DateTo"] = TransactionDate.ToString("dd-MMM-yyyy");
            }
            else
            {
                ViewData["DateFrom"] = VDate.ToString("dd-MMM-yyyy");
                ViewData["DateTo"] = VDate.ToString("dd-MMM-yyyy");
            }
            return View();
        }
        public ActionResult GenerateMemberWiseSavingsData(int Qtype, string dateFrom, string dateTo)
        {
            try
            {
                var param = new { Qtype = Qtype, OfficeID = SessionHelper.LoginUserOfficeID, DateFrom = dateFrom, DateTo = dateTo };
                var alldata = groupwiseReportService.GenerateMemberListReportDependsOnCollection(param, "rpt_MemberWiseSavingsData");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("CompanyName", ApplicationSettings.OrganiztionName);
                reportParam.Add("DateFrom", dateFrom);
                reportParam.Add("DateTo", dateTo);
                if (Qtype == 1)
                    ReportHelper.PrintReport("RptMemberWiseSavingsData_Deposit.rpt", alldata.Tables[0], reportParam);
                else if (Qtype == 2)
                    ReportHelper.PrintReport("RptMemberWiseSavingsData_Withdrawal.rpt", alldata.Tables[0], reportParam);
                else
                    ReportHelper.PrintReport("RptMemberWiseSavingsData_Drop.rpt", alldata.Tables[0], reportParam);

                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        public ActionResult GenerateWorkingArea()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult GenerateExpireListReport(string Qtype, string Center, string DateFromValue, string DateToValue)
        {
            try
            {
                var param = new { Qtype = Qtype, Office = SessionHelper.LoginUserOfficeID, Center = Center, Org = SessionHelper.LoginUserOrganizationID, DateFrom = DateFromValue, DateTo = DateToValue };
                var alldata = groupwiseReportService.GetDataUltimateReleaseReport(param, "Rpt_Expirelist");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                reportParam.Add("FromDate", DateFromValue);
                reportParam.Add("ToDate", DateToValue);
                //ReportHelper.PrintReport("rptExpireList.rpt", alldata.Tables[0], reportParam);
                ReportHelper.PrintReport("rptExpireListDateWise.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        
        #region GenerateCIBdata
        public ActionResult MRACIBInformation()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }

        public ActionResult MRACIBMultiselect()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }

        public JsonResult GenerateMRASubjectDetails(int jtStartIndex, int jtPageSize, string jtSorting, string callType, string officeId, string dateTo, string branch, string filterColumn, string filterValue)
        {
            try
            {
                var param = new object();

                if (callType == "generate_MRASubjectDetails")
                {
                    param = new { param0 = callType, OfficeID = officeId, DateTo = dateTo };
                }
                if (callType == "get_MRASubjectDetails")
                {
                    param = new { param0 = callType, OfficeID = officeId, DateTo = dateTo, param1 = branch, param2 = filterColumn, param3 = filterValue };
                }

                var queryResponse = ultimateReportService.GetDataWithParameter(param, "usp_Generate_MRASubjectDetails");

                var assignSpToVM = queryResponse.Tables[0].AsEnumerable()
                    .Select(row => new MRACIBSubjectDetailsVM
                    {
                        ACCOUNTINGDATE = row.Field<string>("ACCOUNTINGDATE"),
                        PRODUCTIONDATE = row.Field<string>("PRODUCTIONDATE"),
                        MFICODE = row.Field<string>("MFICODE"),
                        BRANCH_CODE = row.Field<string>("BRANCH_CODE"),
                        MEMBERID = row.Field<string>("MEMBERID"),
                        Name = row.Field<string>("Name"),
                        Occupation = row.Field<int>("Occupation"),
                        FATHERS_NAME = row.Field<string>("FATHERS_NAME"),
                        MOTHERS_NAME = row.Field<string>("MOTHERS_NAME"),
                        MARITAL_STATUS = row.Field<int>("MARITAL_STATUS"),
                        SPOUSE_NAME = row.Field<string>("SPOUSE_NAME"),
                        Gender = row.Field<string>("Gender"),
                        DOB = row.Field<string>("DOB"),
                        NID = row.Field<string>("NID"),
                        SMARTCARD_NO = row.Field<string>("SMARTCARD_NO"),
                        BIRTH_CERTIFICATE_NO = row.Field<string>("BIRTH_CERTIFICATE_NO"),
                        TIN = row.Field<string>("TIN"),
                        Other_ID_Type = row.Field<int>("Other_ID_Type"),
                        OTHER_ID_NO = row.Field<string>("OTHER_ID_NO"),
                        EXPIRY_DATE = row.Field<string>("EXPIRY_DATE"),
                        ISSUE_COUNTRY = row.Field<string>("ISSUE_COUNTRY"),
                        ContactNo = row.Field<string>("ContactNo"),
                        P_ADDRESS = row.Field<string>("P_ADDRESS"),
                        P_THANA = row.Field<string>("P_THANA"),
                        P_DISTRICT = row.Field<string>("P_DISTRICT"),
                        P_COUNTRY = row.Field<string>("P_COUNTRY"),
                        PR_ADDRESS = row.Field<string>("PR_ADDRESS"),
                        PR_THANA = row.Field<string>("PR_THANA"),
                        PR_DISTRICT = row.Field<string>("PR_DISTRICT"),
                        PR_COUNTRY = row.Field<string>("PR_COUNTRY"),
                        ACADEMIC_QUALIFICATION = row.Field<int>("ACADEMIC_QUALIFICATION"),
                        OfficeId = row.Field<int>("OfficeId"),
                        ReportDate = row.Field<DateTime>("ReportDate").ToString("dd-MM-yyyy"),
                        Data_Status = row.Field<int>("Data_Status")

                    }).ToList();
                var currentPageRecords = assignSpToVM.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = assignSpToVM.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public JsonResult GenerateMRAContractDetails(int jtStartIndex, int jtPageSize, string jtSorting, string callType, string officeId, string dateTo, string branch, string filterColumn, string filterValue)
        {
            try
            {
                var param = new object();

                if (callType == "generate_MRAContractDetails")
                {
                    param = new { param0 = callType, OfficeID = officeId, DateTo = dateTo };
                }
                if (callType == "get_MRAContractDetails")
                {
                    param = new { param0 = callType, OfficeID = officeId, DateTo = dateTo, param1 = branch, param2 = filterColumn, param3 = filterValue };
                }

                var queryResponse = ultimateReportService.GetDataWithParameter(param, "usp_Generate_MRAContractDetails");

                var assignSpToVM = queryResponse.Tables[0].AsEnumerable()
                    .Select(row => new MRACIBContractDetailsVM
                    {
                        ACCOUNTINGDATE = row.Field<string>("ACCOUNTINGDATE"),
                        PRODUCTIONDATE = row.Field<string>("PRODUCTIONDATE"),
                        MFICODE = row.Field<string>("MFICODE"),
                        BRANCH_CODE = row.Field<string>("BRANCH_CODE"),
                        MEMBERID = row.Field<string>("MEMBERID"),
                        LOAN_CODE = row.Field<string>("LOAN_CODE"),
                        LOAN_TYPE = row.Field<string>("LOAN_TYPE"),
                        LOAN_DISBURSEMENT_DATE = row.Field<string>("LOAN_DISBURSEMENT_DATE"),
                        END_DATE_CONTRACT = row.Field<string>("END_DATE_CONTRACT"),
                        LAST_INSTALLMENT_PAID_DATE = row.Field<string>("LAST_INSTALLMENT_PAID_DATE"),
                        DISBURSED_AMOUNT = row.Field<decimal?>("DISBURSED_AMOUNT") ?? 0,
                        TOTAL_OUTSTANDING_AMT = row.Field<decimal?>("TOTAL_OUTSTANDING_AMT") ?? 0,
                        PERIODICITY_PAYMENT = row.Field<string>("PERIODICITY_PAYMENT"),
                        TOTAL_NUM_INSTALLMENT = row.Field<int>("TOTAL_NUM_INSTALLMENT"),
                        INSTALLMENT_AMT = row.Field<decimal?>("INSTALLMENT_AMT") ?? 0,
                        NUM_REMAINING_INSTALLMENT = row.Field<decimal?>("NUM_REMAINING_INSTALLMENT") ?? 0,
                        NUM_OVERDUE_INSTALLMENT = row.Field<decimal?>("NUM_OVERDUE_INSTALLMENT") ?? 0,
                        OVERDUE_AMT = row.Field<decimal?>("OVERDUE_AMT") ?? 0,
                        LOAN_STATUS = row.Field<string>("LOAN_STATUS"),
                        RESCHEDULE_NO = row.Field<int>("RESCHEDULE_NO"),
                        LAST_RESCHEDULE_DATE = row.Field<string>("LAST_RESCHEDULE_DATE"),
                        WRITE_OFF_AMT = row.Field<decimal?>("WRITE_OFF_AMT") ?? 0,
                        WRITE_OFF_DATE = row.Field<string>("WRITE_OFF_DATE"),
                        CONTRACT_PHASE = row.Field<string>("CONTRACT_PHASE"),
                        LOAN_DURATION = row.Field<int>("LOAN_DURATION"),
                        ACTUAL_END_DATE_CONTRACT = row.Field<string>("ACTUAL_END_DATE_CONTRACT"),
                        ECONOMIC_PURPOSE_CODE = row.Field<string>("ECONOMIC_PURPOSE_CODE"),
                        COMPULSORY_SAVING_AMT = row.Field<decimal?>("COMPULSORY_SAVING_AMT") ?? 0,
                        VOLUNTARY_SAVING_AMT = row.Field<decimal?>("VOLUNTARY_SAVING_AMT") ?? 0,
                        TERM_SAVING_AMT = row.Field<decimal?>("TERM_SAVING_AMT") ?? 0,
                        SUBSIDIZED_CREDIT_FLAG = row.Field<string>("SUBSIDIZED_CREDIT_FLAG"),
                        SERVICE_CHARGE_RATE = row.Field<decimal?>("SERVICE_CHARGE_RATE") ?? 0,
                        PAYMENT_MODE = row.Field<string>("PAYMENT_MODE"),
                        ADVANCE_PAYMENT_AMT = row.Field<decimal?>("ADVANCE_PAYMENT_AMT") ?? 0,
                        LAW_SUIT = row.Field<string>("LAW_SUIT"),
                        ME = row.Field<string>("ME"),
                        MEMBER_WELFARE_FUND = row.Field<string>("MEMBER_WELFARE_FUND"),
                        INSURENCE_COVERAGE = row.Field<string>("INSURENCE_COVERAGE"),
                        OfficeId = row.Field<int>("OfficeId"),
                        ReportDate = row.Field<DateTime>("ReportDate").ToString("dd-MM-yyyy"),
                        Data_Status = row.Field<int>("Data_Status")

                    }).ToList();
                var currentPageRecords = assignSpToVM.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = assignSpToVM.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult DeleteMRASubjectDetails(string memberid)
        {
            try
            {
                var param = new { @param0 = "delete_MRASubjectDetails", @param1 = memberid };
                int isInserted = ultimateReportService.DeleteMRASubjectDetails(param);

                if (isInserted > 0)
                    return Json(new { success = true, message = "Delete Successful" }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, message = "Delete Failed" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteMRAContractDetails(string memberid)
        {
            try
            {
                var param = new { @param0 = "delete_MRAContractDetails", @param1 = memberid };
                int isInserted = ultimateReportService.DeleteMRAContractDetails(param);

                if (isInserted > 0)
                    return Json(new { success = true, message = "Delete Successful" }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, message = "Delete Failed" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region GenerateMicrofinanceKPI

        public ActionResult FOWiseChangesReportNew()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
            return View();
        }
        public ActionResult GenerateFOWiseChangesReportNew(string DateFrom, string DateTo, string ddlReportType)
        {
            try
            {
                var param = new { Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo };
                var alldata = groupwiseReportService.GetFOWiseChangesReport(param, "SP_Get_MicrofinanceKPIReport");
                var subReportDB = new Dictionary<string, DataTable>();
                subReportDB.Add("RPT_ChangesReport1", alldata.Tables[0]);
                if (ddlReportType == "pdf")
                {
                    ReportHelper.PrintWithSubReport("RPT_MFKPIReport.rpt", alldata.Tables[0], new Dictionary<string, object>(), subReportDB, new RPT_ChangesReportCOwise());
                }
                else if (ddlReportType == "xls")
                {
                    ReportHelper.ExportExcelDataOnly("RPT_MFKPIReport.rpt", alldata.Tables[0], new Dictionary<string, object>(), subReportDB, new RPT_ChangesReportCOwise());
                
                }
                return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public ActionResult MicrofinanceKPI()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["HOList"] = items;
            ViewData["ZoneList"] = items;
            ViewData["AreaList"] = items;
            ViewData["OfficeList"] = items;
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
            var param = new { @OfficeID = SessionHelper.LoggedInEmployee.OfficeID };
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
            return View();

        }
        public ActionResult GenerateMicrofinanceKPI(string OfficeId,string DateFrom, string DateTo)
        {
            try
            {
                //var param = new { Office = SessionHelper.LoginUserOfficeID, DateFrom = DateFrom, DateTo = DateTo };
                //var alldata = groupwiseReportService.GetDataUltimateReleaseReportWithReportServer(param, "SP_Get_MicrofinanceKPIReport");
                //var subReportDB = new Dictionary<string, DataTable>();
                //subReportDB.Add("RPT_MFKPISubReport1", alldata.Tables[0]);           
                //ReportHelper.PrintWithSubReport("RPT_MFKPIReport.rpt", alldata.Tables[0], new Dictionary<string, object>(), subReportDB, new RPT_ChangesReportCOwise());

                var param = new { Office = OfficeId, DateFrom = DateFrom, DateTo = DateTo};
                var alldata = groupwiseReportService.GetOfficeWiseChangesReport(param, "SP_Get_MicrofinanceKPIReport");
                var reportParam = new Dictionary<string, object>();
                reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                ReportHelper.PrintReport("RPT_MFKPIReport.rpt", alldata.Tables[0], reportParam);
                return Content(string.Empty);
                //var alldata = groupwiseReportService.GetOfficeWiseChangesReport(param, "SP_Get_MicrofinanceKPIReport");
                //var reportParam = new Dictionary<string, object>();
                //reportParam.Add("param_orgName", ApplicationSettings.OrganiztionName);
                //ReportHelper.PrintReport("RPT_MFKPIReport.rpt", alldata.Tables[0], reportParam);
                //return Content(string.Empty);
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
        #endregion 

    }
}
