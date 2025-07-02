using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class DepartmentViewModel
    {
        public long RowSl { get; set; }

        [Display(Name = "Department ID")]
        [Required(ErrorMessage = "{0} is Required")]
        public int DepartmentID { get; set; }

        [Display(Name = "Department Name")]
        [StringLength(150, ErrorMessage = "Maximum length is {1}")]
        public string DepartmentName { get; set; }

        [Display(Name = "Is Active")]
        public bool? IsActive { get; set; }

        [Display(Name = "In Active Date")]
        public DateTime? InActiveDate { get; set; }

        [Display(Name = "Create User")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(35, ErrorMessage = "Maximum length is {1}")]
        public string CreateUser { get; set; }

        [Display(Name = "Create Date")]
        [Required(ErrorMessage = "{0} is Required")]
        public DateTime CreateDate { get; set; }
    }
}