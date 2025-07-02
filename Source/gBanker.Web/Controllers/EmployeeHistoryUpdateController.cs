using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.Controllers
{
    public class EmployeeHistoryUpdateController : BaseController
    {
        #region Private Variabls
        private readonly IUltimateReportService ultimateReportService;
        #endregion      

        public EmployeeHistoryUpdateController(
            IUltimateReportService ultimateReportService)
        {
            this.ultimateReportService = ultimateReportService;
        }
        // GET: EmployeeHistoryUpdate
        public ActionResult Index()
        {
            return View();
        }



        //Get Employee List
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
                List<EmployeeHistoryUpdateViewModel> List_ViewModel = new List<EmployeeHistoryUpdateViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList = ultimateReportService.GetDataWithParameter(param, "SP_GetEmployeeHistoryUpdateList");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new EmployeeHistoryUpdateViewModel
                {
                    EmployeeHistoryId = row.Field<int>("EmployeeHistoryId"),
                    EmployeeCode = row.Field<string>("EmployeeCode"),
                    OfficeCode = row.Field<string>("OfficeCode"),
                    OldOfficeCode = row.Field<string>("OldOfficeCode"),
                    OldJoinDate = row.Field<string>("OldJoinDate")

                }).ToList();

                var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }// End of Function

        // Edit Employee History Update

        public ActionResult Edit(int id)
        {
            try
            {
                EmployeeHistoryUpdateViewModel model = new EmployeeHistoryUpdateViewModel();

                //Old Office Code DropDown
                var oldOfficeList = ultimateReportService.GetDataWithoutParameter("spGet_OldOfficeList");
                var OldOfficeCodeList = oldOfficeList.Tables[0].AsEnumerable()
                    .Select(x => x).ToList().Select(x => new SelectListItem
                    {
                        Value = x.Field<int>("OfficeID").ToString(),
                        Text = x.Field<string>("OfficeCode") + '-' + x.Field<string>("OfficeName")
                    });
                var List = new List<SelectListItem>();
                List.Add(new SelectListItem() { Text = "Select Old Office Code", Value = "0", Selected = true });
                List.AddRange(OldOfficeCodeList);
                model.OldOfficeCodeList = List;

                var param = new { EmployeeHistoryID = id };
                var employeeHistorydata = ultimateReportService.GetDataWithParameter(param, "spGet_EmployeeHistoryUpdate");



                var employeeHistoryList = employeeHistorydata.Tables[0].AsEnumerable()
                .Select(x => new EmployeeHistoryUpdateViewModel
                {
                    EmployeeHistoryId = x.Field<int>("EmployeeHistoryId"),
                    EmployeeCode = x.Field<string>("EmployeeCode")+ '-' + x.Field<string>("EmpName"),
                    OfficeCode = x.Field<string>("OfficeCode") + '-' + x.Field<string>("OfficeName"),
                    OldOfficeId = x.Field<int>("OldOfficeCode"),
                    OldJoinDate = x.Field<string>("OldJoinDate")
                }).ToList().FirstOrDefault();

                model.EmployeeHistoryId = employeeHistoryList.EmployeeHistoryId;
                model.EmployeeCode = employeeHistoryList.EmployeeCode;
                model.OfficeCode = employeeHistoryList.OfficeCode;
                model.OldOfficeId = employeeHistoryList.OldOfficeId;
                model.OldJoinDate = employeeHistoryList.OldJoinDate;
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Edit(EmployeeHistoryUpdateViewModel model)
        {
            try
            {

                var param = new { EmployeeHistoryId = model.EmployeeHistoryId, OldOfficeId = model.OldOfficeId, OldJoinDate = model.OldJoinDate };

                var val = ultimateReportService.GetDataWithParameter(param, "spUpdate_EmployeeHistory");


            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return GetSuccessMessageResult("Data Saved Successfully");
        }
    }
}