using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class EmployeeHistoryUpdateViewModel: BaseModel
    {
        public int EmployeeHistoryId { get; set; }

        public int EmployeeID { get; set; }

        public string EmployeeCode { get; set; }

        public int OfficeID { get; set; }
        [Display(Name = "Office Code")]
        public string OfficeCode { get; set; }

        [Display(Name = "Old Joining Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public string OldJoinDate { get; set; }

        public int OldOfficeId { get; set; }
        [Display(Name = "Old Office Code")]
        public string OldOfficeCode { get; set; }

        public IEnumerable<SelectListItem> OldOfficeCodeList { get; set; }

        //public IEnumerable<SelectListItem> EmployeeList { get; set; }

        //public IEnumerable<SelectListItem> OfficeList { get; set; }
    }
}