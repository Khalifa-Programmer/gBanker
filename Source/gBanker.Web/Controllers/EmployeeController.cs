using AutoMapper;
using CrystalDecisions.CrystalReports.Engine;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Service;
using gBanker.Web.Models;
using gBanker.Web.ViewModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gBanker.Web.Core.Extensions;
using gBanker.Web.Helpers;
using gBanker.Service.ReportServies;
using gBanker.Data.CodeFirstMigration;
using System.Data.Entity.Validation;

namespace gBanker.Web.Controllers
{
    //  [Authorize]
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService employeeService;
        private readonly IOfficeService officeService;
        private readonly IUltimateReportService ultimateReportService;
        public EmployeeController(IEmployeeService employeeService, IOfficeService officeService, IUltimateReportService ultimateReportService)
        {
            this.employeeService = employeeService;
            this.officeService = officeService;
            this.ultimateReportService = ultimateReportService;
        }

        //public object GetEmployee(int v1, int v2, object p)
        //{
        //    throw new NotImplementedException();
        //}

        public EmployeeController(IEmployeeService employeeService, IOfficeService officeService)
        {
            this.employeeService = employeeService;
            this.officeService = officeService;
        }

        // GET: Employee
        public ActionResult Index()
        {
            IEnumerable<SelectListItem> items = new SelectList(" ");
            ViewData["EmpList"] = items;
            return View();
        }

        public JsonResult GetEmployee(int jtStartIndex, int jtPageSize, string jtSorting, string filterValue)
        {
            try
            {
                long TotCount;


                var param2 = new { @officeID = LoginUserOfficeID, @EmpID = filterValue };
                if (filterValue == null || filterValue == "")
                {
                    filterValue = "0";
                }
                var getMemberTolrecordEmp = employeeService.getEmployeeList(LoginUserOfficeID, filterValue);
                var detail = getMemberTolrecordEmp.ToList();
                var totalCount = detail.Count();
                var entities = detail.Skip(jtStartIndex).Take(jtPageSize);
                var currentPageRecords = Mapper.Map<IEnumerable<EmployeeDetail>, IEnumerable<EmployeeViewModel>>(entities);


                //var allemployee = employeeService.GetAll().Where(s => s.OfficeID == LoginUserOfficeID && s.IsActive == true && s.OrgID==LoggedInOrganizationID).OrderBy(e => e.EmployeeCode);
                //var totalCount = allemployee.Count();
                //var entities = allemployee.Skip(jtStartIndex).Take(jtPageSize);
                //var currentPageRecords = Mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(entities);
                //var detail = getMemberTolrecordEmp.ToList();

                //var currentPageRecords = Mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(getMemberEmp);
                return Json(new { Result = "OK", Records = currentPageRecords, TotalRecordCount = totalCount });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }

        }
        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Employee/Create
        public ActionResult Create()
        {


            var model = new EmployeeViewModel();
            MapDropDownList(model);
            ViewData["Designation"] = GetDesignationList(0);

            model.EmployeeStatus = 1;
            return View(model);
        }


        private void MapDropDownList(EmployeeViewModel model)
        {
            var employee = new List<SelectListItem>();
            employee.Add(new SelectListItem() { Text = "Active", Value = "1", Selected = true });
            employee.Add(new SelectListItem() { Text = "InActive", Value = "0" });

            var gender = new List<SelectListItem>();
            gender.Add(new SelectListItem() { Text = "Female", Value = "F", Selected = true });
            gender.Add(new SelectListItem() { Text = "Male", Value = "M" });

            var allbranch = officeService.GetAll().Where(o => o.OfficeID == LoginUserOfficeID);

            var viewbranch = allbranch.Select(m => new SelectListItem() { Text = string.Format("{0} - {1}", m.OfficeCode, m.OfficeName), Value = m.OfficeID.ToString() });

            model.branchListItems = viewbranch;
            model.empstatusListItems = employee.AsEnumerable();
            model.genderListItems = gender.AsEnumerable();

        }
        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(EmployeeViewModel model)
        {
            try
            {


                model.IsActive = true;
                model.EmployeeCode = model.EmployeeCode;
                model.Designation = Convert.ToString(model.DesignationID);
                var entity = Mapper.Map<EmployeeViewModel, Employee>(model);
                
               //Add Validlation Logic.
                if (ModelState.IsValid)
                {
                    var errors = employeeService.IsValidEmployee(entity);
                    if (errors.ToList().Count == 0)
                    {
                        if (entity.EmployeeStatus == 1)
                        {
                            model.ReleaseDate = null;

                        }
                        if (entity.EmployeeStatus == 0)
                        {
                            if (model.ReleaseDate == null)
                                return GetErrorMessageResult("Release Date found empty");


                        }
                        entity.OrgID = Convert.ToInt16(LoggedInOrganizationID);

                        //if (entity.OrgID != 54)
                        //{
                        //    employeeService.Create(entity);
                        //}
                        //else
                        //{

                        //    var paramD = new
                        //    {
                        //        @EmployeeID = 0,
                        //        @EmployeeCode = entity.EmployeeCode,
                        //        @OfficeID = entity.OfficeID,
                        //        @EmpName = entity.EmpName,
                        //        @EmpNameBen = entity.EmpNameBen,
                        //        @GuardianName = entity.GuardianName,
                        //        @EmpAddress = entity.EmpAddress,
                        //        @PhoneNo = entity.PhoneNo,
                        //        @Email = entity.Email,
                        //        @Gender = entity.Gender,
                        //        @BirthDate = entity.BirthDate,
                        //        @Designation = entity.Designation,
                        //        @JoiningDate = entity.JoiningDate,
                        //        @EmployeeStatus = entity.EmployeeStatus,
                        //       // @ReleaseDate = null,
                        //        @OrgID = entity.OrgID,
                        //        @CreateDate = entity.CreateDate,
                        //        @CreateUser = entity.CreateUser
                        //        //,@CompanyId =entity.,
                        //        //@BranchId int,
                        //        //@BirthPlace nvarchar(50),
                        //        //@GrossSalary Decimal(18, 4),
                        //        //@TotalEarnings Decimal(18, 4),
                        //        //@DesignationId int,
                        //        //@DepartmentId int,
                        //        //@FirstJoiningDate Date,
                        //        //@ConfirmationDate Date,
                        //        //@LoginTime Datetime,
                        //        //@LogoutTime datetime,
                        //        //@LastLoginTime Datetime,
                        //        //@BankAccountNo nvarchar(50),
                        //        //@EmployeeRank varchar(2),
                        //        //@IsOverTime bit,
                        //        //@OvertimeHour Decimal(18, 4),
                        //        //@IncrementMonth int,
                        //        //@IncrementYear int,
                        //        //@EffectiveStartDate date,
                        //        //@EffectiveEndDate date,
                        //        //@EmployeeTypeId int,
                        //        //@GradeId int,
                        //        //@Step int,
                        //        //@BankName varchar(50),
                        //        //@BankBranchName varchar(50),
                        //        //@IsPFApplicable bit,
                        //        //@IsPFClossed bit,
                        //        //@EmployeeStatusInString nvarchar(50),
                        //        //@OvertimeRate decimal(18, 4),
                        //        //@UpdateDate Datetime,
                        //        //@UpdateUser bigint
                        //    };
                        //    var loanser1 = ultimateReportService.DataInsertintoEmployee(paramD);
                        //}

                        employeeService.Create(entity);


                        return GetSuccessMessageResult();
                    }
                    else
                        return GetErrorMessageResult(errors);
                }
                return GetErrorMessageResult();

            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                ve.PropertyName, ve.ErrorMessage);
            //        }
            //    }
            //    throw;
            //}
        } 
   

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            if (employeeService.IsContinued(id))
            {
                var employee = employeeService.GetById(id);
                var entity = Mapper.Map<Employee, EmployeeViewModel>(employee);

                MapDropDownList(entity);
                if(entity.DesignationID != null)
                ViewData["Designation"] = GetDesignationList((int)entity.DesignationID);
                else
                    ViewData["Designation"] = GetDesignationList(0);

                return View(entity);
            }
            else
                ModelState.AddModelError("Validation", "Duplicate Product, please enter a diferent employee id and name.");
                return RedirectToAction("Index");
        }


        public List<SelectListItem> GetDesignationList(int DesignationID)
        {
            List<SelectListItem> Component_items_Designation;
            List<DesignationViewModel> List_ViewModel = new List<DesignationViewModel>();
            var param = new { AndCondition = "" };
            var List = ultimateReportService.GetDataWithParameter(param, "SP_PR_Get_Designation_List");
            List_ViewModel = List.Tables[0].AsEnumerable()
            .Select(row => new DesignationViewModel
            {  
                DesignationID = row.Field<int>("DesignationID"),
                DesignationName = row.Field<string>("DesignationName") 

            }).ToList();

            var Components = List_ViewModel.Select(x => x).ToList().Select(x => new SelectListItem
            {
                Value = x.DesignationID.ToString(),
                Text = string.Format("{0}", x.DesignationName),
                Selected = x.DesignationID == DesignationID ? true : false
            });

            Component_items_Designation = new List<SelectListItem>();
            if (Components.ToList().Count > 0)
            {
                Component_items_Designation.Add(new SelectListItem() { Text = "Please Select", Value = "0", Selected = true });
            }
            Component_items_Designation.AddRange(Components);
            return Component_items_Designation;

        }





        public JsonResult CheckDupliEmployee(string EmployeeCode)
        {
            string cnt_status = "No";
            var mem = employeeService.CheckDupliEmployee(EmployeeCode);
            if (mem.ToList().Count > 0)
            {
                cnt_status = "Yes";
            }
            //var x = DateTime.Now.ToString("dd-MMM-yyyy");

            return Json(cnt_status, JsonRequestBehavior.AllowGet);
        }
        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(EmployeeViewModel model)
        {
            try
            {
                
                var entity = Mapper.Map<EmployeeViewModel, Employee>(model);
                var getEmployeeDetails = employeeService.GetById(entity.EmployeeID);
                //// TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    if (entity.EmployeeStatus == 1)
                    {
                        model.ReleaseDate = null;

                    }
                    if (entity.EmployeeStatus == 0)
                    {
                        if (model.ReleaseDate == null)
                            return GetErrorMessageResult("Release Date found empty");
                    }
                    getEmployeeDetails.BirthDate = entity.BirthDate;
                    getEmployeeDetails.Designation = entity.Designation;
                    getEmployeeDetails.Email = entity.Email;
                    getEmployeeDetails.EmpAddress = entity.EmpAddress;
                    getEmployeeDetails.EmpName = entity.EmpNameBen;
                    getEmployeeDetails.EmployeeStatus = entity.EmployeeStatus;
                    getEmployeeDetails.EmpName = entity.EmpName;
                    getEmployeeDetails.EmpNameBen = entity.EmpNameBen;
                    getEmployeeDetails.Gender = entity.Gender;
                    getEmployeeDetails.GuardianName = entity.GuardianName;
                    getEmployeeDetails.JoiningDate = entity.JoiningDate;
                    getEmployeeDetails.PhoneNo = entity.PhoneNo;
                    getEmployeeDetails.ReleaseDate = entity.ReleaseDate;
                    getEmployeeDetails.EmployeeCode = entity.EmployeeCode;
                    getEmployeeDetails.DesignationID = entity.DesignationID;
                    var mem = employeeService.CheckDupliEmployee(entity.EmployeeCode,entity.EmployeeID);
                    if (mem.ToList().Count > 0)
                    {
                        return GetErrorMessageResult("EmployeeCode Already Exists");
                    }

                    if (getEmployeeDetails.OfficeID!=LoginUserOfficeID)
                    {
                        return GetErrorMessageResult("Invalid Office...........");
                    }

                    if (SessionHelper.LoginUserOrganizationID != 54)
                    {
                        employeeService.Update(getEmployeeDetails);

                    }
                    else
                    {

                        var paramD = new
                        {
                            @EmployeeID = entity.EmployeeID,
                            @EmployeeCode = entity.EmployeeCode,
                            @OfficeID = entity.OfficeID,
                            @EmpName = entity.EmpName,
                            @EmpNameBen = entity.EmpNameBen,
                            @GuardianName = entity.GuardianName,
                            @EmpAddress = entity.EmpAddress,
                            @PhoneNo = entity.PhoneNo,
                            @Email = entity.Email,
                            @Gender = entity.Gender,
                            @BirthDate = entity.BirthDate,
                            @Designation = entity.Designation,
                            @JoiningDate = entity.JoiningDate,
                            @EmployeeStatus = entity.EmployeeStatus,
                            // @ReleaseDate = null,
                            @OrgID = entity.OrgID,
                            @CreateDate = entity.CreateDate,
                            @CreateUser = entity.CreateUser
                            //,@CompanyId =entity.,
                            //@BranchId int,
                            //@BirthPlace nvarchar(50),
                            //@GrossSalary Decimal(18, 4),
                            //@TotalEarnings Decimal(18, 4),
                            //@DesignationId int,
                            //@DepartmentId int,
                            //@FirstJoiningDate Date,
                            //@ConfirmationDate Date,
                            //@LoginTime Datetime,
                            //@LogoutTime datetime,
                            //@LastLoginTime Datetime,
                            //@BankAccountNo nvarchar(50),
                            //@EmployeeRank varchar(2),
                            //@IsOverTime bit,
                            //@OvertimeHour Decimal(18, 4),
                            //@IncrementMonth int,
                            //@IncrementYear int,
                            //@EffectiveStartDate date,
                            //@EffectiveEndDate date,
                            //@EmployeeTypeId int,
                            //@GradeId int,
                            //@Step int,
                            //@BankName varchar(50),
                            //@BankBranchName varchar(50),
                            //@IsPFApplicable bit,
                            //@IsPFClossed bit,
                            //@EmployeeStatusInString nvarchar(50),
                            //@OvertimeRate decimal(18, 4),
                            //@UpdateDate Datetime,
                            //@UpdateUser bigint
                        };
                        var loanser1 = ultimateReportService.DataInsertintoEmployee(paramD);
                    }

                   
                    return GetSuccessMessageResult();
                    
                }
                return GetErrorMessageResult();
            }
            catch (Exception ex)
            {
                return GetErrorMessageResult(ex);
            }
        }
        public void UpdateMethod(int Id, DateTime newValue)
        {
           
        }
        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            employeeService.Inactivate(id, null);
            return RedirectToAction("Index");
        }

        // POST: Employee/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                employeeService.Inactivate(id, null);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
