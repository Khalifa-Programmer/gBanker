using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class EmployeeHistoryViewModel:BaseModel
    {
        public int EmployeeHistoryId { get; set; }

        public int EmployeeID { get; set; } 

        public string EmployeeCode { get; set; }

        public int OfficeID { get; set; }

        [Display(Name = "Employee Name")]
        public string EmpName { get; set; }
        public string OfficeName { get; set; }

        public string EmpNameBen { get; set; }

        public string GuardianName { get; set; }

        public string EmpAddress { get; set; }

        [Display(Name = "Joining Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string JoiningDate { get; set; }

        public int EmployeeStatus { get; set; }

        [Display(Name = "Release Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string ReleaseDate { get; set; }

        public int OrgID { get; set; }

        public int OldOfficeId { get; set; }
        public string OldOfficeCode { get; set; }

        [Display(Name = "New Office")]
        public int NewOfficeId { get; set; }

        public IEnumerable<SelectListItem> EmployeeList { get; set; }

        public IEnumerable<SelectListItem> OfficeList { get; set; }


    }//End Class
}//End Namespace