using gBanker.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class EmployeeViewModel : BaseModel
    {
        [GlobalizedDisplayName("EmployeeID")]
        public int EmployeeID { get; set; }

        [GlobalizedDisplayName("EmployeeCode")]
        [Required(ErrorMessage = "Employee Code is required")]
        public string EmployeeCode { get; set; }

        [GlobalizedDisplayName("OfficeID")]
        public int OfficeID { get; set; }
        [StringLength(40)]
        [GlobalizedDisplayName("EmpName")]
        [Required(ErrorMessage = "Employee Name is required")]
        public string EmpName { get; set; }
        [StringLength(50)]
        [GlobalizedDisplayName("EmpNameBen")]
        public string EmpNameBen { get; set; }
        [StringLength(40)]
        [GlobalizedDisplayName("GuardianName")]
        public string GuardianName { get; set; }
        [StringLength(155)]
        [GlobalizedDisplayName("EmpAddress")]
        [Required(ErrorMessage = "Address is required")]
        public string EmpAddress { get; set; }
        [StringLength(35)]
        [GlobalizedDisplayName("PhoneNo")]
        public string PhoneNo { get; set; }
        [StringLength(55)]
        [GlobalizedDisplayName("Email")]
        public string Email { get; set; }
        [StringLength(7)]
        [GlobalizedDisplayName("Gender")]
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [GlobalizedDisplayName("BirthDate")]
        public Nullable<System.DateTime> BirthDate { get; set; }
       
        public string Designation { get; set; }
        [GlobalizedDisplayName("JoiningDate")]
        public System.DateTime JoiningDate { get; set; }
        [GlobalizedDisplayName("EmployeeStatus")]
        public byte EmployeeStatus { get; set; }


        public int OrgID { get; set; }
        public int? DesignationID { get; set; }


        [GlobalizedDisplayName("ReleaseDate")]

        public Nullable<System.DateTime> ReleaseDate { get; set; }
        public string OfficeCode { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> InActiveDate { get; set; }
        public IEnumerable<SelectListItem> branchListItems { get; set; }
        public IEnumerable<SelectListItem> empstatusListItems { get; set; }
        public IEnumerable<SelectListItem> genderListItems { get; set; }
    }
}