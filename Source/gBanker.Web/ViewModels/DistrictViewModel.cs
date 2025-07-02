using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class DistrictViewModel
    {
        public int DistrictID { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }


        public int? DivisionID { get; set; }
        public string DivisionName { get; set; }
        public string DivisionCode { get; set; }
        public IEnumerable<SelectListItem> DivisionList { get; set; }
    }
}