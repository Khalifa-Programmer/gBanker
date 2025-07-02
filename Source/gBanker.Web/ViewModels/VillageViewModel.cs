using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class VillageViewModel
    {
        public long LgVillageID { get; set; }
        public string VillageCode { get; set; }
        public string VillageName { get; set; }
        public string UnionCode { get; set; }
        public string UnionName { get; set; }
        public string UpozillaCode { get; set; }
        public string UpozillaName { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public int? CountryID { get; set; }


        public bool? IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }




        public int? DivisionID { get; set; }
        public IEnumerable<SelectListItem> DivisionList { get; set; }

        public int? DistrictID { get; set; }
        public IEnumerable<SelectListItem> DistrictList { get; set; }

        public int? UpozillaID { get; set; }
        public IEnumerable<SelectListItem> UpozillaList { get; set; }

        public int? UnionID { get; set; }
        public IEnumerable<SelectListItem> UnionList { get; set; }

        public string CountryName { get; set; }
        public IEnumerable<SelectListItem> CountryList { get; set; }
    }
}