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
    public class DesignationController : BaseController
    {
        #region Private Variabls
        private readonly IUltimateReportService ultimateReportService;




        #endregion


        #region Variables        

        public DesignationController( 
            IUltimateReportService ultimateReportService )
        { 
            this.ultimateReportService = ultimateReportService;
        }

        #endregion

        // GET: Designation
        public ActionResult ManageDesignation()
        {
            DesignationViewModel designation = new DesignationViewModel();

            //Department List
            var depList = ultimateReportService.GetDataWithoutParameter("SP_PR_Get_Department");

            var deptList = depList.Tables[0].AsEnumerable()
                .Select(x => x).ToList().Select(x => new SelectListItem
                {
                    Value = x.Field<int>("DepartmentID").ToString(),
                    Text = x.Field<string>("DepartmentName")
                });
            var Departmentlist = new List<SelectListItem>();
            Departmentlist.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            Departmentlist.AddRange(deptList);
            designation.Department = Departmentlist;

            //Designation Type Dropdown

            var designationTypeList = ultimateReportService.GetDataWithoutParameter("SP_PR_Get_DesignationType");

            var desigtypeList = designationTypeList.Tables[0].AsEnumerable()
                .Select(x => x).ToList().Select(x => new SelectListItem 
                {
                    Value = x.Field<int>("DesignationTypeID").ToString(),
                    Text = x.Field<string>("DesignationTypeName")
                });
            var DesTypeList = new List<SelectListItem>();
            DesTypeList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            DesTypeList.AddRange(desigtypeList);
            designation.DesignationType = DesTypeList;


            return View(designation);
        }

        public JsonResult CreateDesignation(string DesignationName, int DepartmentID, int DesignationTypeID )
        {
            string result = "OK";
            try
            {
                //Check If Same Work area Name
                List<DesignationViewModel> List_ViewModel = new List<DesignationViewModel>();
                var param2 = new { @DesignationName = DesignationName, @DepartmentID = DepartmentID, @DesignationTypeID = DesignationTypeID };
                var empList = ultimateReportService.GetDataWithParameter(param2, "SP_PR_Get_Designation_ByName");

                //Depatment DropDwon

                DesignationViewModel designation = new DesignationViewModel();
                var depList = ultimateReportService.GetDataWithoutParameter("SP_PR_Get_Department");

                var deptList = depList.Tables[0].AsEnumerable()
                    .Select(x => x).ToList().Select(x => new SelectListItem
                    {
                        Value = x.Field<int>("DepartmentID").ToString(),
                        Text = x.Field<string>("DepartmentName")
                    });
                var list = new List<SelectListItem>();
                list.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                list.AddRange(deptList);

                designation.Department = list;

                //Designation Type Dropdown

                var designationTypeList = ultimateReportService.GetDataWithoutParameter("SP_PR_Get_DesignationType");

                var desigtypeList = designationTypeList.Tables[0].AsEnumerable()
                    .Select(x => x).ToList().Select(x => new SelectListItem
                    {
                        Value = x.Field<int>("DesignationTypeID").ToString(),
                        Text = x.Field<string>("DesignationTypeName")
                    });
                var DesTypeList = new List<SelectListItem>();
                DesTypeList.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
                DesTypeList.AddRange(desigtypeList);
                designation.DesignationType = DesTypeList;


                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new DesignationViewModel
                {
                    DesignationID = row.Field<int>("DesignationID"),
                    DesignationName = row.Field<string>("DesignationName"),
                    DepartmentID = row.Field<int>("DepartmentID"),
                    DesignationTypeID = row.Field<int>("DesignationTypeID")
                }).ToList();


                if (List_ViewModel.Count > 0)
                {
                    Response.StatusCode = 403;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                //End of Check

                Int64 CreateUser = Convert.ToInt64( LoggedInEmployeeID.ToString());
                DateTime CreateDate = DateTime.Now;
                //(@WorkAreaName varchar(200), @CreateUser varchar(100), @CreateDate datetime)
                var param = new { DesignationName = DesignationName, DepartmentID = DepartmentID, DesignationTypeID = DesignationTypeID, CreateUser = CreateUser, CreateDate = CreateDate };
                var val = ultimateReportService.GetDataWithParameter(param, "SP_PR_CreateDesignation");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateDeignation(string DesignationName, int DesignationID, int DepartmentID, int DesignationTypeID)
        {
            string result = "OK";
            try
            {
                Int64 UpdateUser = Convert.ToInt64( LoggedInEmployeeID.ToString());
                DateTime UpdateDate = DateTime.Now;
                
                var param = new { DesignationName = DesignationName, DesignationID = DesignationID, DepartmentID = DepartmentID, DesignationTypeID = DesignationTypeID, UpdateUser = UpdateUser, UpdateDate = UpdateDate };
                var val = ultimateReportService.GetDataWithParameter(param, "SP_PR_UpdateDesignation");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        // Delete Area
        public JsonResult DeleteDesignation(int DesignationID)
        {
            string result = "OK";
            try
            {
                Int64 UpdateUser = Convert.ToInt64(LoggedInEmployeeID.ToString());
                DateTime UpdateDate = DateTime.Now;
 
                List<DesignationViewModel> List_ViewModel = new List<DesignationViewModel>();
                var param = new { DesignationID = DesignationID };
                var empList = ultimateReportService.GetDataWithParameter(param, "SP_PR_Check_EmployeeDesignation");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new DesignationViewModel
                {
                    DesignationID = row.Field<int>("DesignationID"),
                    DesignationName = row.Field<string>("DesignationName"),
                    DepartmentID = row.Field<int>("DepartmentID"),
                    DesignationTypeID = row.Field<int>("DesignationTypeID"),

                }).ToList();

                if (List_ViewModel.Count > 0)
                {
                    Response.StatusCode = 403;
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }
                //End of Check

                var param2 = new { DesignationID = DesignationID, UpdateUser = UpdateUser, UpdateDate = UpdateDate };
                var val = ultimateReportService.GetDataWithParameter(param2, "SP_PR_DeleteDesignation");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Show List
        public JsonResult GetDesignationList(string DesignationID, int jtStartIndex, int jtPageSize, string jtSorting, string filterColumn, string filterValue)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                string DesignationIds = Convert.ToString(DesignationID);

                if (DesignationID != null) //"0"
                    sb.Append(" AND  DesignationID =" + DesignationIds);

                List<DesignationViewModel> List_ViewModel = new List<DesignationViewModel>();
                var param = new { AndCondition = sb.ToString() };
                var empList =  ultimateReportService.GetDataWithParameter(param, "SP_PR_Get_Designation_List");

                List_ViewModel = empList.Tables[0].AsEnumerable()
                .Select(row => new DesignationViewModel
                {
                    rowSl = row.Field<Int64>("rowSl"),
                    DesignationID = row.Field<int>("DesignationID"),
                    DesignationName = row.Field<string>("DesignationName"),
                    DepartmentID = row.Field<int>("DepartmentID"),
                    DepartmentName = row.Field<string>("DepartmentName"),
                    DesignationTypeID = row.Field<int>("DesignationTypeID"),
                    DesignationTypeName = row.Field<string>("DesignationTypeName")

                }).ToList();

                if (DesignationID != null)
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

        }// End Function


    }// END Class
}// END Namespace