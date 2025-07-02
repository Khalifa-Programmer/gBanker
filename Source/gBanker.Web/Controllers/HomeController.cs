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
using System.Data;
namespace gBanker.Web.Controllers
{
    //[Authorize]    
    public class HomeController : BaseController
    {

        private readonly IEmployeeOfficeMappingService employeeOfficeService;
        private readonly IOfficeService officeService;
        private readonly IDayInitialService dayInitialService;
        private readonly IMemberService memberService;
        private readonly IDashBoardService dashBoardService;
        private readonly IAccReportService accReportService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly IEmployeeService employeeService;
        public HomeController(IEmployeeOfficeMappingService employeeOfficeService, 
            IOfficeService officeService, IDayInitialService dayInitialService, 
            IMemberService memberService, IDashBoardService dashBoardService, 
            IAccReportService accReportService, IUltimateReportService ultimateReportService, IEmployeeService employeeService)
        {
            this.employeeOfficeService = employeeOfficeService;
            this.officeService = officeService;
            this.dayInitialService = dayInitialService;
            this.memberService = memberService;
            this.dashBoardService = dashBoardService;
            this.accReportService = accReportService;
            this.ultimateReportService = ultimateReportService;
            this.employeeService = employeeService;
        }
        public ActionResult Index()
        {
            ViewBag.ShowPopup = false;
            if (SessionHelper.LoginUserOfficeID == default(int?))
            {
                ViewBag.ShowPopup = true;
                if (SessionHelper.LoggedInEmployee != null)
                {
                    SessionHelper.LoginUserOfficeID = SessionHelper.LoggedInEmployee.OfficeID;
                    var office = officeService.GetById(SessionHelper.LoginUserOfficeID.Value);
                    var entity = Mapper.Map<Office, OfficeViewModel>(office);
                    SessionHelper.LoggedInOfficeDetail = entity;
                }

            }
            if (SessionHelper.LoggedInEmployee != null)
            {
               // ViewBag.ShowPopup = true;
                var officeList = employeeOfficeService.GetEmployeeOfficeMappings(SessionHelper.LoggedInEmployee.EmployeeCode).Select(s => new SelectListItem() { Value = s.OfficeID.ToString(), Text = string.Format("{0} - {1}", s.Office.OfficeCode, s.Office.OfficeName) }).ToList();
              

                if(officeList.Count==1)
                {
                    SessionHelper.LoginUserOfficeID = SessionHelper.LoggedInEmployee.OfficeID;
                    var office = officeService.GetById(SessionHelper.LoginUserOfficeID.Value);
                    var entity = Mapper.Map<Office, OfficeViewModel>(office);
                    SessionHelper.LoggedInOfficeDetail = entity;
                    SelectOffice(SessionHelper.LoggedInEmployee.OfficeID);
                    ViewBag.ShowPopup = false;
                }
                else
                {
                    ViewBag.EmployeeOfficeMappings = officeList;
                    ViewBag.ShowPopup = true;
                }
                    

            }
            ViewBag.OrgId = LoggedInOrganizationID;
            if (TempData.ContainsKey("Expire_Msg"))
                ViewBag.Expire_Msg = TempData["Expire_Msg"].ToString();
            return View();
        }
        public JsonResult GetOfficeListTree()
        {
            var empID = SessionHelper.LoggedInEmployeeID;
            var employee = employeeService.GetById(empID ?? 0);
            var msg = "";
            var param = new { EmployeeCode = employee.EmployeeCode };
            var empList = ultimateReportService.GetDataWithParameter(param, "getEmployeeOfficeTreeView");
            //var empList = accReportService.GetEmployeeMapping(param, "getEmployeeOfficeListTreeView");
            var newOfficeList = empList.Tables[0].AsEnumerable()
            .Select(row => new
            {
                Text = string.Format("{0} - {1}", row.Field<string>("OfficeCode"), row.Field<string>("OfficeName")),
                Value = row.Field<int>("OfficeID").ToString(),
                ZoneName = row.Field<string>("ZoneName").ToString(),
                AreaName = row.Field<string>("AreaName").ToString(),
                HoName = row.Field<string>("HeadOffice").ToString()

            }).ToList();

            return Json(new { msg, officeList = newOfficeList }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public JsonResult SelectOffice(int officeId)
        {
            var officeFullName = "";
            var OrgName = "";
            var PreAnotation = "";
            PreAnotation = "Welcome to";
            if (officeId > 0)
            {
                SessionHelper.LoginUserOfficeID = officeId;
                var office = officeService.GetById(SessionHelper.LoginUserOfficeID.Value);                
                var entity = Mapper.Map<Office, OfficeViewModel>(office);
                SessionHelper.LoggedInOfficeDetail = entity;
                try
                {
                    DateTime? transactionDate;
                    string OrginizationName;
                    string Processtype;
                    DateTime? LastDayEndDate;
                    DateTime? dDAte; 
                    var dayInitialStatus = dayInitialService.ValidateDayInitial(SessionHelper.LoginUserOfficeID, out transactionDate, out OrginizationName, out Processtype, out LastDayEndDate,Convert.ToInt16(LoggedInOrganizationID));
                    SessionHelper.TransactionDay = dayInitialStatus;
                    SessionHelper.TransactionDate = transactionDate.HasValue ? transactionDate.Value : default(DateTime);
                    SessionHelper.OrganizationName = OrginizationName;
                    SessionHelper.ProcessType = Processtype;
                    SessionHelper.LastDayEndDate = LastDayEndDate;
                    SessionHelper.IsDayInitiated = !string.IsNullOrEmpty(dayInitialStatus);
                    
                    //officeService.GetById(Convert.ToInt32(SessionHelper.LoginUserOfficeID))
                    //officeFullName = office.OfficeCode + ", " + office.OfficeName;
                    officeFullName = office.OfficeCode + ", " + office.OfficeName + ", DashBoardDate:" + SessionHelper.LastDayEndDate.Value.ToString("dd-MMM-yyyy");
                    //OrgName = PreAnotation + " " + SessionHelper.OrganizationName + "-" + officeFullName;
                    OrgName = SessionHelper.OrganizationName + "-" + officeFullName;
                   // OrgName = PreAnotation + " " + SessionHelper.OrganizationName ;
                }
                catch (Exception ex)
                {
                    SessionHelper.IsDayInitiated = false;
                }
            }
            var resultObj = new { TransactionDashBoardString = SessionHelper.TransactionDashBoardString, OfficeName = officeFullName, OrgName = OrgName };
            return new JsonResult() { Data = resultObj };
        }
        [HttpPost]
        public JsonResult getOrgName()
        {
           
            var OrgName = "";
            var PreAnotation = "";
            PreAnotation = "Welcome to";
            
                try
                {
                    OrgName = SessionHelper.OrganizationName;
                    //OrgName = PreAnotation + " " + SessionHelper.OrganizationName;
                }
                catch (Exception ex)
                {
                    SessionHelper.IsDayInitiated = false;
                }
           
            var resultObj = new { OrgName = OrgName };
            return new JsonResult() { Data = resultObj };
        }
        public JsonResult GetDashboardItems()
        {

            List<DashboardViewModel> List_MemberViewModel = new List<DashboardViewModel>();
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var ArrearItem = accReportService.GetDashboardItems(param);
            if (ArrearItem.Tables[0] !=null && ArrearItem.Tables[0].Rows.Count > 0)            
            {
               List_MemberViewModel = ArrearItem.Tables[0].AsEnumerable()
              .Select(row => new DashboardViewModel
              {
                  DueAmount = row.Field<decimal>("DueAmount"),
                  PaidAmount = row.Field<decimal>("PaidAmount"),
                  Otr = row.Field<decimal>("Otr"),
                  Income = row.Field<decimal>("Income"),
                  Expense = row.Field<decimal>("Expense"),
                  ProfitOrLoss = row.Field<decimal>("ProfitOrLoss"),
                  TotalTodaysMember = row.Field<decimal>("TotalTodaysMember"),
                  TotalTodaysBorrower = row.Field<decimal>("TotalTodaysBorrower"),
                  Ratio = row.Field<decimal>("Ratio"),
                  TotalMember = row.Field<long>("TotalMember"),
                  TotalBorrower = row.Field<long>("TotalBorrower"),
                  DormantMember = row.Field<long>("DormantMember")
                 }).ToList();
            }
            else
            {
                 
            }
            return Json(List_MemberViewModel.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPieChartData()
        {

             List<DashBoardPieChart> ds = new List<DashBoardPieChart>();
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var allProducts = accReportService.GetPieChartData(param);
            String strTotalBorrower = "";
            String strDormantMember = "";
            if (allProducts.Tables[0].Rows.Count>0)
            {
                strTotalBorrower = allProducts.Tables[0].Rows[0]["TotalBorrower"].ToString();
                strDormantMember = allProducts.Tables[0].Rows[0]["DormantMember"].ToString();
            }
            else
            {
                strTotalBorrower = "0";
                strDormantMember = "0";
            }
            DashBoardPieChart oDashBoardPieChart2 = new DashBoardPieChart();
            oDashBoardPieChart2.PieNode = "Borrowers";
            if (string.IsNullOrEmpty(strTotalBorrower) == true)
            {
                oDashBoardPieChart2.PieValue = 0;
            }
            else
                oDashBoardPieChart2.PieValue = Convert.ToInt64(strTotalBorrower);
            ds.Add(oDashBoardPieChart2);

            DashBoardPieChart oDashBoardPieChart3 = new DashBoardPieChart();
            oDashBoardPieChart3.PieNode = "Dorment Members";
            if (string.IsNullOrEmpty(strDormantMember) == true)
            {
                oDashBoardPieChart3.PieValue = 0;
            }
            else
            oDashBoardPieChart3.PieValue = Convert.ToInt64(strDormantMember);
            ds.Add(oDashBoardPieChart3);
            var chartData = ds.ToList();           
            return Json(chartData.ToList(),JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetArrearAging()
        {

            List<DashboardViewModel> List_MemberViewModel = new List<DashboardViewModel>();
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var ArrearItem = accReportService.GetArrearAging(param);



            List_MemberViewModel = ArrearItem.Tables[0].AsEnumerable()
            .Select(row => new DashboardViewModel
            {
                ItemName = row.Field<string>("ItemName"),
                Members = row.Field<decimal>("Members")
            }).ToList();

            return Json(List_MemberViewModel.ToList(), JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetBarChartData()
        {

            List<DashboardViewModel> List_MemberViewModel = new List<DashboardViewModel>();
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var ArrearItem = accReportService.GetBarChartData(param);
            List_MemberViewModel = ArrearItem.Tables[0].AsEnumerable()
            .Select(row => new DashboardViewModel
            {
                BarYear = row.Field<string>("BarYear"),
                BarDisbursements = row.Field<decimal>("BarDisbursements"),
                BarLoanRepaid = row.Field<decimal>("BarLoanRepaid"),
                BarBadLoans = row.Field<decimal>("BarBadLoans"),
                BarOverDueAmount= row.Field<decimal>("BarOverDueAmount"),
                BarSavings=row.Field<decimal>("BarSavings")
            }).ToList();


            return Json(List_MemberViewModel, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult GetRapayment()
        //{

        //    List<DashboardViewModel> List_MemberViewModel = new List<DashboardViewModel>();
        //    var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
        //    var ArrearItem = accReportService.GetRapayment(param);

        //    List_MemberViewModel = ArrearItem.Tables[0].AsEnumerable()
        //    .Select(row => new DashboardViewModel
        //    {
        //        ItemName = row.Field<string>("ItemName"),
        //        Members = row.Field<decimal>("Members")
        //    }).ToList();

        //    return Json(List_MemberViewModel.ToList(), JsonRequestBehavior.AllowGet);

        //}

        public JsonResult GetDashboardCenterMemmberMiscSummary()
        {

            List<DashboardViewModel> List_MemberViewModel = new List<DashboardViewModel>();
            var param = new { @OfficeID = SessionHelper.LoginUserOfficeID };
            var ArrearItem = accReportService.GetDashboardCenterMemmberMiscSummary(param);

            List_MemberViewModel = ArrearItem.Tables[0].AsEnumerable()
            .Select(row => new DashboardViewModel
            {
                ItemName = row.Field<string>("Title"),
                SummaryValue = row.Field<decimal>("SummaryValue")
            }).ToList();

            return Json(List_MemberViewModel.ToList(), JsonRequestBehavior.AllowGet);

        }

    }
}
