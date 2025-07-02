using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class HolidayViewModel:BaseModel
    {
        public int HolidayID { get; set; }

        [Display(Name = "Business Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BusinessDate { get; set; }
        [Display(Name = "Office")]
        public int OfficeID { get; set; }
        [Display(Name = "Samity")]
        public int CenterID { get; set; }

        public string Description { get; set; }

        [Display(Name = "Holiday Type")]
        public string HolidayType { get; set; }
        public IEnumerable<SelectListItem> OfficeList { get; set; }
        public IEnumerable<SelectListItem> CenterList { get; set; }
        public IEnumerable<SelectListItem> HolidayTypeList { get; set; }
        [Display(Name = "Days")]
        public IEnumerable<SelectListItem> WeeklyList { get; set; }
    }
}