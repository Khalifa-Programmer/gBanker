using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class EmployeeOfficeMappingViewModel : BaseModel
    {
        public int EmployeeOfficeMappingID { get; set; }
        [Display(Name = "Employee Code")]
        [Required]
        [MaxLength(10)]
        public string EmployeeCode { get; set; }
        public short EmployeeID { get; set; }

        [Display(Name = "Office Names")]
        public int OfficeID { get; set; }

        [Display(Name = "Head Office")]
        public string HeadOfficeCode { get; set; }
        [Display(Name = "Zone Office")]
        public string ZoneCode { get; set; }
        [Display(Name = "Area Office")]
        public string AreaCode { get; set; }

        public int EmployeeId { get; set; }
        public IEnumerable<SelectListItem> SelectedOfficeList { get; set; }

    }
}