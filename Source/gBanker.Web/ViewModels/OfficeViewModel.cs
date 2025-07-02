using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class OfficeViewModel:BaseModel
    {
        public int OfficeID { get; set; }
        [Display(Name = "Code")]
        public string OfficeCode { get; set; }
        [Display(Name = "Name")]
        public string OfficeName { get; set; }

        [Display(Name = "Office Level")]
        public byte OfficeLevel { get; set; }

        [Display(Name = "First Level")]
        public string FirstLevel { get; set; }

        [Display(Name = "Second Level")]
        public string SecondLevel { get; set; }

        [Display(Name = "Third Level")]
        public string ThirdLevel { get; set; }

        [Display(Name = "Fourth Level")]
        public string FourthLevel { get; set; }

        [Display(Name = "Operation Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime OperationStartDate { get; set; }
        [Display(Name = "Address")]
        public string OfficeAddress { get; set; }
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }
        [Display(Name = "Location")]
        public Nullable<int> GeoLocationID { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [Display(Name = "Parent Code")]
        public string ParentId { get; set; }
        public IEnumerable<SelectListItem> GeoLocationList { get; set; }

        [Display(Name = "Investor")]
        public int? InvestorID { get; set; } // public byte? InvestorID { get; set; }

        [Display(Name ="Union")]
        public int UnionID { get; set; }
        public IEnumerable<SelectListItem> OfficeList { get; set; }
        public IEnumerable<SelectListItem> InvestorList { get; set; }
        public IEnumerable<SelectListItem> UnionList { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Country is required.")]
        public int CountryID { get; set; }
        public int ProvidedByCountryID { get; set; }

        [Display(Name = "Division")]
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        [Display(Name = "District")]
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        [Display(Name = "Upazilla")]
        public string UpozillaCode { get; set; }
        public string UpozillaName { get; set; }
        [Display(Name = "Union")]
        public string UnionCode { get; set; }

        public string txtParentCode { get; set; }
        public IEnumerable<SelectListItem> CountryList { get; set; }
        public IEnumerable<SelectListItem> DivisionList { get; set; }
        public IEnumerable<SelectListItem> DistrictList { get; set; }
        public IEnumerable<SelectListItem> UpozillaList { get; set; }

        [NotMapped]
        public string Sl { get; set; }
    }
}