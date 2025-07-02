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
using gBanker.Data.CodeFirstMigration;
namespace gBanker.Web.Controllers
{
    public class ApplicationLogController : BaseController
    {
        private readonly IApplicationLogService applicationLogService;
        private readonly IEmployeeService employeeService;
        private readonly IUltimateReportService ultimateReportService;
        public ApplicationLogController(IApplicationLogService applicationLogService, IEmployeeService employeeService, IUltimateReportService ultimateReportService)
        {
             this.employeeService = employeeService;
             this.applicationLogService = applicationLogService;
             this.ultimateReportService = ultimateReportService;
        }
        //
        // GET: /ApplicationLog/
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["EmpList"] = items;
            return View();
        }
        public ActionResult Detail(int id)
        {
            var log = applicationLogService.GetById(id);
            return View(log);
        }
        
      
        public JsonResult GetLogRecords(int jtStartIndex, int jtPageSize, string jtSorting, string dateFrom, string dateTo, string action, string user)
        {
       
            try
            {
               

                List<ApplicationLog> List_ProductViewModel = new List<ApplicationLog>();
                var param = new { DateFrom = Convert.ToDateTime(dateFrom), DateTO = Convert.ToDateTime(dateTo), Action = action, User = user };
                var model = ultimateReportService.GetAuditTrialo(param);

                List_ProductViewModel = model.Tables[0].AsEnumerable()
                .Select(row => new ApplicationLog
                {
                    ApplicationLogId = row.Field<long>("ApplicationLogId"),
                    LogDate = row.Field<DateTime>("LogDate"),
                    ClientIP = row.Field<string>("ClientIP"),
                    RequestUser = row.Field<string>("RequestUser"),
                    RequestDetail = row.Field<string>("RequestDetail"),
                    ControllerName = row.Field<string>("ControllerName"),
                    ActionName = row.Field<string>("ActionName"),
                    HttpMethod = row.Field<string>("HttpMethod"),
                    ActionURL = row.Field<string>("ActionURL"),
                    OfficeCode= row.Field<string>("OfficeCode")

                }).ToList();




                //var allloansummary = applicationLogService.GetAll();
                //if(dateFrom != "" && dateTo != "")
                //    allloansummary = allloansummary.Where(w => w.CreateDate >= Convert.ToDateTime(dateFrom) && w.CreateDate <= Convert.ToDateTime(dateTo));
                //if (action != "" && action !="V")
                //    allloansummary = allloansummary.Where(w => w.ActionName == action);
                //if (user != "")
                //    allloansummary = allloansummary.Where(w => w.RequestUser == user);

                //var detail = allloansummary.ToList();
                //var totCount = detail.Count();
                //var currentPageRecords = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                //return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totCount });




                var detail = List_ProductViewModel.ToList();
                var totCount = detail.Count();
                var currentPageRecords = detail.ToList().Skip(jtStartIndex).Take(jtPageSize);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totCount });

                
            }
            catch(Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        public JsonResult GetEmpList()
        {
            var offc_id = Convert.ToInt32(SessionHelper.LoginUserOfficeID);
            var empList = employeeService.GetAll().Where(c => c.OfficeID == offc_id);
            var viewEmp = empList.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.EmployeeCode.ToString(),
                Text = x.EmployeeCode.ToString() + " " + x.EmpName.ToString()
            });
            var emp_items = new List<SelectListItem>();
            if (viewEmp.ToList().Count > 0)
            {
                emp_items.Add(new SelectListItem() { Text = "Please Select", Value = "", Selected = true });
            }
            emp_items.AddRange(viewEmp);
            return Json(emp_items, JsonRequestBehavior.AllowGet);
        }
    }

}