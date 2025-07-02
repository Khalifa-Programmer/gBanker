using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.Helpers;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;


namespace gBanker.Web.Controllers
{
    public class EmployeeSalInfoController : BaseController
    {

        #region Variables
        private readonly ILoanTrxService loantrxService;
        private readonly IGroupwiseReportService groupwiseReportService;
        private readonly IMemberService memberService;
        private readonly IOfficeService officeService;
        private readonly IUltimateReportService ultimateReportService;
        private readonly ILoanSummaryService loanSummaryService;

        public EmployeeSalInfoController(ILoanTrxService loantrxService, IMemberService memberService, IOfficeService officeService, IUltimateReportService ultimateReportService, IGroupwiseReportService groupwiseReportService, ILoanSummaryService loanSummaryService)
        {
            this.loantrxService = loantrxService;
            this.memberService = memberService;
            this.officeService = officeService;
            this.ultimateReportService = ultimateReportService;
            this.groupwiseReportService = groupwiseReportService;
            this.loanSummaryService = loanSummaryService;

        }
        #endregion Variables

        // GET: EmployeeSalInfo
        /// <summary>
        /// KHALID
        /// #22 Oct 2018
        /// </summary>
        /// <returns>View</returns>
        /// 
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get List Of EmployeeSalInfo
        /// </summary>
        /// <param name="jtStartIndex"></param>
        /// <param name="jtPageSize"></param>
        /// <param name="jtSorting"></param>
        /// <param name="EmployeeId"></param>
        /// <returns>JsonObject</returns>
        public JsonResult GenerateEmployeeSalList(int jtStartIndex, int jtPageSize, string jtSorting, string hdnEmployeeID = "")
        {
            try
            {


                StringBuilder sb = new StringBuilder();
                if (hdnEmployeeID != "" && hdnEmployeeID != "0")
                {
                    sb.Append("AND esi.EmployeeID = " + hdnEmployeeID);
                }

                var OfficeId = Convert.ToInt32(SessionHelper.LoginUserOfficeID);

                sb.Append("AND o.OfficeID = " + OfficeId);


                var param1 = new { @AndCondition = sb.ToString() };

                List<EmployeeSalViewModel> List_EmployeeSalViewModel = new List<EmployeeSalViewModel>();
                var empList = ultimateReportService.GetEmployeeSalInfo(param1);

                List_EmployeeSalViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeSalViewModel
                {
                    EmployeeCode        = row.Field<string>("EmployeeCode"),
                    EmployeeName        = row.Field<string>("EmployeeName"),
                    EmployeeID          = row.Field<int>("EmployeeID"),
                   // OfficeID            = row.Field<int>("OfficeID"),
                    Basic               = row.Field<decimal>("Basic"),
                    HRent               = row.Field<decimal>("HRent"),
                    MA                  = row.Field<decimal>("MA"),
                    TA                  = row.Field<decimal>("TA"),
                    PFOwn               = row.Field<decimal>("PFOwn"),
                    PFOrg               = row.Field<decimal>("PFOrg"),
                    FestBonus           = row.Field<decimal>("FestBonus"),
                    SSF                 = row.Field<decimal>("SSF"),
                    special             = row.Field<decimal>("special"),
                    distance            = row.Field<decimal>("distance"),
                    dearness            = row.Field<decimal>("dearness"),
                    MobileBill          = row.Field<decimal>("MobileBill"),
                    GratuityOrg         = row.Field<decimal>("GratuityOrg"),
                    HealthFund          = row.Field<decimal>("HealthFund"),
                    CreateDate          = row.Field<DateTime>("CreateDate"),
                    CreateUser          = row.Field<string>("CreateUser")   ,
                    SalaryDate          = row.Field<string>("SalaryDate")

                }).ToList();

                var currentPageRecords = List_EmployeeSalViewModel.Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_EmployeeSalViewModel.LongCount() });

            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }

        public JsonResult GetEmpInfoByCode(string employee_code)
        {
            try
            {
                
               


                var param1 = new { @EmployeeCode = employee_code, @OfficeId = Convert.ToInt32(SessionHelper.LoginUserOfficeID) };

                List<EmployeeSalViewModel> List_EmployeeSalViewModel = new List<EmployeeSalViewModel>();
                var empList = ultimateReportService.GetEmployeeInfo(param1);


                List_EmployeeSalViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeSalViewModel
                {
                    EmployeeID = row.Field<int>("EmployeeID"),
                    EmployeeName = row.Field<string>("EmployeeName")
                     
                }).ToList();

                if (List_EmployeeSalViewModel.Count() == 0)
                {
                    Response.StatusCode = 403;
                }
                return Json(List_EmployeeSalViewModel.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }





        /// <summary>
        /// Access: Create Or Update EmployeeSalInfo
        /// </summary>
        /// <param name="hdnEmployeeID"></param>
        /// <param name="txtBasic"></param>
        /// <param name="txtHRent"></param>
        /// <param name="txtMA"></param>
        /// <param name="txtTA"></param>
        /// <param name="txtPFOwn"></param>
        /// <param name="txtPFOrg"></param>
        /// <param name="txtFestBonus"></param>
        /// <param name="txtSSF"></param>
        /// <param name="txtspecial"></param>
        /// <param name="txtdistance"></param>
        /// <param name="txtdearness"></param>
        /// <param name="txtMobileBill"></param>
        /// <param name="txtGratuityOrg"></param>
        /// <param name="txtHealthFund"></param>
        /// <returns>JsonObject</returns>
        public JsonResult CreateUpdate(

              string hdnEmployeeID
            , string txtBasic           = "0"
            , string txtHRent           = "0"
            , string txtMA              = "0"
            , string txtTA              = "0"
            , string txtPFOwn           = "0"
            , string txtPFOrg           = "0"
            , string txtFestBonus       = "0"
            , string txtSSF             = "0"
            , string txtspecial         = "0"
            , string txtdistance        = "0"
            , string txtdearness        = "0"
            , string txtMobileBill      = "0"
            , string txtGratuityOrg     = "0"
            , string txtHealthFund      = "0"
            , string txtSalaryDate      = ""


            )
        {
            string result = "Data Saved Successfully.";
            try
            {
                int EmpID = 0;
                if (hdnEmployeeID != "")
                {
                    if (hdnEmployeeID != null)
                    {
                        EmpID = Convert.ToInt32(hdnEmployeeID);
                    }
                }

                var param1 = new
                {
                    @hdnEmployeeID       =   EmpID                          ,
                    @txtBasic            =   txtBasic                       ,
                    @txtHRent            =   txtHRent                       ,
                    @txtMA               =   txtMA                          ,
                    @txtTA               =   txtTA                          ,
                    @txtPFOwn            =   txtPFOwn                       ,
                    @txtPFOrg            =   txtPFOrg                       ,
                    @txtFestBonus        =   txtFestBonus                   ,
                    @txtSSF              =   txtSSF                         ,
                    @txtspecial          =   txtspecial                     ,
                    @txtdistance         =   txtdistance                    ,
                    @txtdearness         =   txtdearness                    ,
                    @txtMobileBill       =   txtMobileBill                  ,
                    @txtGratuityOrg      =   txtGratuityOrg                 ,
                    @txtHealthFund       =   txtHealthFund                  ,
                    @txtCreateUser       =   LoggedInEmployee.EmployeeID    ,
                    @txtSalaryDate       =   txtSalaryDate

                };
                var empList = ultimateReportService.AccessEmployeeSalInfo(param1);

            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        } // END FUNCTION








    } //End Of Controller.
}// End Namespace