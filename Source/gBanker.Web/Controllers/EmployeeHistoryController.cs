using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace gBanker.Web.Controllers
{
    public class EmployeeHistoryController : BaseController
    {

        #region Variables
        private readonly IOfficeService officeService;
        private readonly IGeoLocationService geoLocationService;
        private readonly IEmployeeService employeeService;
        private readonly IGroupwiseReportService groupwiseReportService;

        public EmployeeHistoryController(IOfficeService officeService, IGeoLocationService geoLocationService, IEmployeeService employeeService, IGroupwiseReportService groupwiseReportService)
        {
              this.officeService            = officeService;
              this.geoLocationService       = geoLocationService;
              this.employeeService          = employeeService;
              this.groupwiseReportService   = groupwiseReportService;
        }
        #endregion

        #region Events

        //
        // GET: /EmployeeHistory/
        public ActionResult Index()
        {
            EmployeeHistoryViewModel employeeHistory = new EmployeeHistoryViewModel();

            //Employee DropDown
            var AllEmployee = employeeService.GetAll();

            var EmployeeList = AllEmployee.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.EmployeeID.ToString(),
                Text = x.EmployeeCode.ToString() + "-" + x.EmpName.ToString()
            });
            var List = new List<SelectListItem>();
            List.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            List.AddRange(EmployeeList);
            employeeHistory.EmployeeList = List;

            return View(employeeHistory);
        }

        public ActionResult Create()
        {
            EmployeeHistoryViewModel employeeHistory = new EmployeeHistoryViewModel();
            //LoggedInOrganizationID

            var office = officeService.GetById((int) LoginUserOfficeID);

            employeeHistory.OldOfficeId = office.OfficeID;
            employeeHistory.OldOfficeCode = office.OfficeCode;

            //Employee DropDown
            var AllEmployee = employeeService.GetAll().Where(x => x.OfficeID == office.OfficeID);

            var EmployeeList = AllEmployee.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.EmployeeID.ToString(),
                Text = x.EmployeeCode.ToString() + "-" + x.EmpName.ToString()  
            });
            var List = new List<SelectListItem>();
            List.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            List.AddRange(EmployeeList);
            employeeHistory.EmployeeList = List;

            //Office Dropdown
            var AllOffice = officeService.GetAll().Where(x => x.OfficeID != office.OfficeID);

            var OfficeList = AllOffice.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.OfficeID.ToString(),
                Text = x.OfficeCode.ToString() + "-" + x.OfficeName.ToString()
            });
            var Listoffice = new List<SelectListItem>();
            Listoffice.Add(new SelectListItem() { Text = "Select All", Value = "0", Selected = true });
            Listoffice.AddRange(OfficeList);
            employeeHistory.OfficeList = Listoffice;
            return View(employeeHistory);


        }
        #endregion Events

        #region Methods

        public JsonResult CreateNew( int OldOfficeId, int EmployeeID, int NewOfficeId, string ReleaseDate, string JoiningDate )
        {
            string result = "OK";
            try
            {

                var employeeData = employeeService.GetById(EmployeeID);
                var param = new // CreateEmpHistory 2, 2 ,'14 Jan,2014', '14 Jan,2015'
                {
                    EmployeeID = EmployeeID
                   ,
                    OfficeID = NewOfficeId
                   ,
                    JoiningDate = JoiningDate
                   ,
                    ReleaseDate = ReleaseDate
                    ,
                    OldOfficeId = OldOfficeId
                    ,
                    OldJoinDate = employeeData.JoiningDate
                };
                var updateParam = new // CreateEmpHistory 2, 2 ,'14 Jan,2014', '14 Jan,2015'
                {
                    EmployeeID = EmployeeID
               ,
                    OfficeID = NewOfficeId
               ,
                    JoiningDate = JoiningDate
               ,
                    ReleaseDate = ReleaseDate
                };
                // Insert in Employee History
                var val = groupwiseReportService.GetEmployeeTransfer(param, "SP_CreateEmpHistory");
                var val2 = groupwiseReportService.GetEmployeeTransfer(updateParam, "SP_UpdateEmployee");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }//End of Function.


        public JsonResult GetList(string EmployeeID, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (EmployeeID != null)
                {
                    if (EmployeeID != "")
                        sb.Append(" AND eh.employeeId ='" + EmployeeID.Trim() + "'");
                }
                List<EmployeeHistoryViewModel> List_ViewModel = new List<EmployeeHistoryViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = groupwiseReportService.GetEmployeeTransfer(param, "SP_GetEmployeeHistoryList");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeHistoryViewModel
                {
                    EmployeeHistoryId = row.Field<int>("EmployeeHistoryId"),
                    EmpName     = row.Field<string>("EmpName"),
                    OfficeName  = row.Field<string>("OfficeName"),
                    JoiningDate = row.Field<string>("JoiningDate"),
                    ReleaseDate = row.Field<string>("ReleaseDate")

                }).ToList();

                var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End of Function




        #endregion Methods

    }//End Class
}//End Namespace