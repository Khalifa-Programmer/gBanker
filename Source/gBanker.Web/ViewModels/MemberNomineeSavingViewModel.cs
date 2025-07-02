using gBanker.Web.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class MemberNomineeSavingViewModel : BaseModel
    {
        
        public long MemberNomineeId { get; set; }
        public long MemberId { get; set; }
        [Display(Name = "Nominee Name")]
        public string NomineeName { get; set; }
        [Display(Name = "Nominee Father")]
        public string NomineeFather { get; set; }
        [Display(Name = "Nominee Mother")]
        public string NomineeMother { get; set; }
        [Display(Name = "Husband/Wife")]
        public string NomineeHusbandWife { get; set; }
        [Display(Name = "Birthdate")]
        public DateTime? NomineeBirthdate { get; set; }
        public string NomineeBirthdateStr { get; set; }
        [Display(Name = "Mobile No")]
        public string NomineeMobileNo { get; set; }

        [Display(Name = "Nominee Relation")]
        public string NomineeRelation { get; set; }
        [Display(Name = "Id Type")]
        public int? IdType { get; set; }

        [Display(Name = "Nominee National Id")]
        public string NomineeNationalId { get; set; }
        
        [Display(Name = "Country")]
        public int? CountryID { get; set; }

        [Display(Name = "Division")]
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        [Display(Name = "District")]
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        [Display(Name = "Upzilla")]
        //[Required(ErrorMessage = "UpzillaCode is required.")]
        public string UpozillaCode { get; set; }
        public string UpozillaName { get; set; }
        [Display(Name = "Union")]
        public string UnionCode { get; set; }
        public string UnionName { get; set; }
        [Display(Name = "Village")]
        [Required(ErrorMessage = "বর্তমান ঠিকানা Village / Street  is required.")]
        public string VillageCode { get; set; }
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        public string AddressLine1 { get; set; }
        public string SNO { get; set; }

        [Display(Name = "Image")]
        public string NomineeImagePath { get; set; }
        public string NomineeImageMsg { get; set; } 

        [Display(Name = "Nominee Image")]
        public HttpPostedFileBase ImgFile_NomineeImage { get; set; }


        [Display(Name = "Image")]
        public string NomineeSignaturePath { get; set; }
        public string NomineeSignatureImageMsg { get; set; }

        [Display(Name = "Nominee Signature")]
        public HttpPostedFileBase ImgFile_NomineeSignatureImage { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public int OfficeID { get; set; }
        public string OfficeName { get; set; }
        public new string CreateUser { get; set; }
        public new DateTime CreateDate { get; set; }

        public IEnumerable<SelectListItem> CountryList { get; set; }
        public IEnumerable<SelectListItem> DivisionList { get; set; }
        public IEnumerable<SelectListItem> DistrictList { get; set; }
        public IEnumerable<SelectListItem> UpozillaList { get; set; }
        public IEnumerable<SelectListItem> UnionList { get; set; }
        public IEnumerable<SelectListItem> VillageList { get; set; }
        public IEnumerable<SelectListItem> IdTypeList { get; set; }
        public IEnumerable<SelectListItem> GenderTypeList { get; set; }

        public int? NomId { get; set; }
        public int? NAlocation { get; set; }
        public long? SavingSummaryID { get; set; }
        public string Gender { get; set; }


    }
    public class MemberNomineeSavingListViewModel
    {
        public long MemberId { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public int OfficeID { get; set; }
        public string OfficeName { get; set; }
        public bool IsActive { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }


        public int? CountryID { get; set; }
        public string DivisionCode { get; set; }
        public string DistrictCode { get; set; }
        public string UpozillaCode { get; set; }
        public string UnionCode { get; set; }
        public string VillageCode { get; set; }
        public string ZipCode { get; set; }
        public string AddressLine1 { get; set; }
    }
}