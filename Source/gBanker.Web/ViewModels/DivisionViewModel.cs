using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class DivisionViewModel
    {
        public byte? DivisionID { get; set; }
        public int? CountryId { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string DivisionAddress { get; set; }
        public string ContactNo { get; set; }
        public int? OfficeID { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string ZoneCode { get; set; }
        public string AreaCode { get; set; }
        public string HOCode { get; set; }


        
        public string CountryName { get; set; }
        public IEnumerable<SelectListItem> CountryList { get; set; }
    }
}