using gBanker.Service.ReportServies;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace gBanker.Web.Controllers
{
    public class DepartmentController : BaseController
    {
        #region Private Variabls
        private readonly IUltimateReportService ultimateReportService;
        #endregion

        #region Variables        

        public DepartmentController(
            IUltimateReportService ultimateReportService)
        {
            this.ultimateReportService = ultimateReportService;
        }

        #endregion
        // GET: Department
        public ActionResult ManageDepartment()
        {
            return View();
        }

        //Create Department
        public JsonResult CreateDepartment(string departmentName)
        {
            string result = "Ok";
            try
            {
                bool IsActive = true;
                Int64 CreateUser = Convert.ToInt64(LoggedInEmployeeID.ToString());
                DateTime CreateDate = DateTime.Now;

                var param = new { DepartmentName = departmentName, IsActive = IsActive, CreateUser = CreateUser, CreateDate = CreateDate };
                var val = ultimateReportService.GetDataWithParameter(param, "SP_PR_CreateDepartment");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateDepartment(int departmentId, string departmentName)
        {
            string result = "Ok";
            try
            {
                bool IsActive = true;
                Int64 UpdateUser = Convert.ToInt64(LoggedInEmployeeID.ToString());
                DateTime UpdateDate = DateTime.Now;

                var param = new { DepartmentID = departmentId, DepartmentName = departmentName, IsActive = IsActive, UpdateUser = UpdateUser, UpdateDate = UpdateDate };
                var val = ultimateReportService.GetDataWithParameter(param, "SP_PR_UpdateDepartment");
            }
            catch(Exception ex)
            {
                Response.StatusCode = 403;
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Delete Department
        public JsonResult DeleteDepartment(int departmentID)
        {
            string result = "OK";
            try
            {
                bool IsActive = false;
                Int64 UpdateUser = Convert.ToInt64(LoggedInEmployeeID.ToString());
                DateTime InActiveDate = DateTime.Now;
                DateTime UpdateDate = DateTime.Now;

                List<DepartmentViewModel> List_ViewModel = new List<DepartmentViewModel>();
                var param = new { DepartmentID = departmentID };
                var depList = ultimateReportService.GetDataWithParameter(param, "SP_PR_Check_IsDepartmentUsed");

                List_ViewModel = depList.Tables[0].AsEnumerable()
                .Select(row => new DepartmentViewModel
                {
                    DepartmentID = row.Field<int>("DepartmentID"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    IsActive = row.Field<bool>("IsActive"),
                    InActiveDate = row.Field<DateTime>("InActiveDate"),

                }).ToList();

                if (List_ViewModel.Count > 0)
                {
                    Response.StatusCode = 403;
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }
                //   End of Check

                var param2 = new { DepartmentID = departmentID, IsActive = IsActive ,UpdateUser = UpdateUser, InActiveDate = InActiveDate, UpdateDate = UpdateDate };
                var val = ultimateReportService.GetDataWithParameter(param2, "SP_PR_DeleteDepartment");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Department List
        public JsonResult GetDepartmentList(string DepartmentID, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                string DepartmentIds = Convert.ToString(DepartmentID);

                if (DepartmentID != null) //"0"
                    sb.Append(" AND  DepartmentID =" + DepartmentIds);

                List<DepartmentViewModel> List_ViewModel = new List<DepartmentViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var depList = ultimateReportService.GetDataWithParameter(param, "SP_PR_Get_Department_List");

                List_ViewModel = depList.Tables[0].AsEnumerable()
                    .Select(row => new DepartmentViewModel
                    {
                        RowSl = row.Field<Int64>("rowSl"),
                        DepartmentID = row.Field<int>("DepartmentID"),
                        DepartmentName = row.Field<string>("DepartmentName")
                    }).ToList();
                if (DepartmentID != null)
                {
                    return Json(List_ViewModel.ToList(), JsonRequestBehavior.AllowGet);
                }


                var currentPageRecords = List_ViewModel.Skip(jtStartIndex).Take(jtPageSize);

                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = List_ViewModel.LongCount(), JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}