using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class CenterViewModel:BaseModel
    {
        
        public int CenterID { get; set; }
        [Display(Name = "Samity Code")]
        public string CenterCode { get; set; }
        [Display(Name = "Office")]
        public int OfficeID { get; set; }
        [Display(Name = "Samity Name")]
        public string CenterName { get; set; }
        [Display(Name = "Samity Address")]
        public string CenterAddress { get; set; }
        [Display(Name = "Samity Collection Type")]
        public string CenterNameBng { get; set; }
        [Display(Name = "Samity Type")]
        public string Organizer { get; set; }
        [Required]
        [Display(Name = "Employee")]
        public short EmployeeId { get; set; }

        [Display(Name = "CenterCategory")]
        [Required]
        public Nullable<int> CenterCategoryID { get; set; }


        [Display(Name = "Collection Day")]
        [Required]
        public string CollectionDay { get; set; }
        [Display(Name = "Collection Date")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime CollectionDate { get; set; }
        [Display(Name = "Location")]
        [Required]
        public Nullable<int> GeoLocationID { get; set; }
        [Display(Name = "Opening Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime OperationStartDate { get; set; }
        [Display(Name = "Samity Status")]
        public byte CenterStatus { get; set; }
        [Display(Name = "CenterType")]
        
        public byte CenterTypeID { get; set; }
      
        public string CenterType { get; set; }
        public IEnumerable<SelectListItem> OfficeList { get; set; }
        public IEnumerable<SelectListItem> EmployeeList { get; set; }
        public IEnumerable<SelectListItem> CenterCategoryList { get; set; }
        public IEnumerable<SelectListItem> GeoLocationList { get; set; }
        public IEnumerable<SelectListItem> CollectionDayList { get; set; }
        public IEnumerable<SelectListItem> CenterStatusList { get; set; }
        public IEnumerable<SelectListItem> OrganizerList { get; set; }
        public IEnumerable<SelectListItem> CenterCollectionType { get; set; }

        public IEnumerable<SelectListItem> CenterTypeList { get; set; }



        [Display(Name = "Center Time")]
        //[DisplayFormat(DataFormatString = "{0:t}", ApplyFormatInEditMode = true)]    //{0:t}
        //[Required(ErrorMessage = "{0} is Required")]
        public string CenterTime { get; set; }

        public string CenterTimeOnly { get; set; }

        [Display(Name = "Center Distance")]
        public string CenterDistance { get; set; }


        // Address Added By KHALID ON 15 March, 2022
        [Display(Name = "Country")]
        [Required(ErrorMessage = "Country is required.")]
        public int? CountryID { get; set; }

        [Display(Name = "Division")]
        public string DivisionCode { get; set; }

        [Display(Name = "District")]
        public string DistrictCode { get; set; }

        [Display(Name = "Upzilla")]
        [Required]
        public string UpozillaCode { get; set; }
        [Required(ErrorMessage = "Upzilla is required.")]
        public string UnionCode { get; set; }

        [Display(Name = "Village")]
        [Required(ErrorMessage = "বর্তমান ঠিকানা Village / Street  is required.")]
        public string VillageCode { get; set; }


        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Display(Name = "Address Details")]
        public string AddressLine1 { get; set; }

        public IEnumerable<SelectListItem> CountryList { get; set; }
        public IEnumerable<SelectListItem> DivisionList { get; set; }
        public IEnumerable<SelectListItem> DistrictList { get; set; }
        public IEnumerable<SelectListItem> UpozillaList { get; set; }
        public IEnumerable<SelectListItem> UnionList { get; set; }
        public IEnumerable<SelectListItem> VillageList { get; set; }

        public long? CenterChief { get; set; }
        public long? AssoCenterChief { get; set; }
        public long? PanelMember { get; set; }

        

    }
}