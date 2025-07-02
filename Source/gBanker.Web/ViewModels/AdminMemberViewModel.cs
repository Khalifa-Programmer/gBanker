using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class AdminMemberViewModel : BaseModel
    {
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        [Display(Name = "Office")]
        public int OfficeID { get; set; }
        [Display(Name = "Samity")]
        [Required(ErrorMessage = "Samity Name is required.")]
        public int CenterID { get; set; }
        [Display(Name = "Group")]
        // [Required(ErrorMessage = "Group is required.")]
        public short GroupID { get; set; }

        [Display(Name = "নাম (Applicant’s Name)")]
        //[Required(ErrorMessage = "নাম (Applicant’s Name)  is required.")]
        public string BanglaName { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Address Details")]
        //[Required(ErrorMessage = "AddressLine 1 is required.")]
        public string AddressLine1 { get; set; }
        [Display(Name = "Address 2(Location)")]
        public string AddressLine2 { get; set; }
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


        // For Permanent Address

        [Display(Name = "Address Details")]
        //[Required(ErrorMessage = "AddressLine 1 is required.")]
        public string PerAddressLine1 { get; set; }
        [Display(Name = "Address 2(Location)")]
        public string PerAddressLine2 { get; set; }
        [Display(Name = "Country")]
        //[Required(ErrorMessage = "Country is required.")]
        public int PerCountryID { get; set; }
        [Display(Name = "Division")]
        public string PerDivisionCode { get; set; }
        public string PerDivisionName { get; set; }
        [Display(Name = "District")]
        public string PerDistrictCode { get; set; }
        public string PerDistrictName { get; set; }
        [Display(Name = "Upzilla")]
        //[Required(ErrorMessage = "UpzillaCode is required.")]
        public string PerUpozillaCode { get; set; }
        public string PerUpozillaName { get; set; }
        [Display(Name = "Union")]
        public string PerUnionCode { get; set; }
        public string PerUnionName { get; set; }
        [Display(Name = "Village")]
        [Required(ErrorMessage = "স্থায়ী ঠিকানা Village / Street is required.")]
        public string PerVillageCode { get; set; }

        public string PerVillageName { get; set; }

        [Display(Name = "Zip Code")]
        public string PerZipCode { get; set; }
        // END


        public string VillageName { get; set; }
        [Display(Name = "Reference Name")]
        public string RefereeName { get; set; }
        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Date Of Birth is required.")]
        public Nullable<System.DateTime> BirthDate { get; set; }
        [Display(Name = "Place of Birth")]
        public string PlaceOfBirth { get; set; }
        [Display(Name = "Citizenship")]
        public string Cityzenship { get; set; }
        [Display(Name = "Admission Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime JoinDate { get; set; }

        [Display(Name = "ID Card Expire Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime? ExpireDate { get; set; }

        public System.DateTime ServerCurrentDate { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }
        [Display(Name = "National ID No.")]
        //[Required(ErrorMessage = "NID/ Birth Cer. is required.")]
        public string NationalID { get; set; }
        [Display(Name = "Smart Card No.")]
        public string SmartCard { get; set; }
        [Display(Name = "Card Number")]

        public string OtherIdNo { get; set; }

        public string Location { get; set; }

        [Required(ErrorMessage = "Home Type is required.")]
        [Display(Name = "Home Type")]
        public string HomeType { get; set; }

        [Display(Name = "Group Type")]
        public string GroupType { get; set; }

        [Display(Name = "Academic Qualification")]
        [Required(ErrorMessage = "Education is required.")]
        public string Education { get; set; }

        [Display(Name = "Family Member")]
        [Range(0, 100)]
        public int FamilyMember { get; set; }

        [Display(Name = "Total Wealth")]
        public string TotalWealth { get; set; }

        [Required(ErrorMessage = "Economic Activity is required.")]
        [Display(Name = "Occupation")]
        public string EconomicActivity { get; set; }
        [Required(ErrorMessage = "Father Name is required.")]
        [Display(Name = "Father Name")]
        public string FatherName { get; set; }

        [Display(Name = "Father Name")]
        //[Required(ErrorMessage = "পিতার নাম (Father’s Name) Bangla  is required.")]
        public string FatherNameBN { get; set; }

        [Required(ErrorMessage = "Mother Name is required.")]
        [Display(Name = "Mother Name")]
        public string MotherName { get; set; }

        //[Required(ErrorMessage = "Mother Name Bangla is required.")]
        [Display(Name = "Mother Name Bangla")]
        public string MotherNameBN { get; set; }

        public string SpouseName { get; set; }

        public string SpouseNameBN { get; set; }

        public int IdentTypeID { get; set; }
        public string TIN { get; set; }

        public decimal? TaxAmount { get; set; }

        //[Required(ErrorMessage = "Co Applicant Name is required.")]
        [Display(Name = "Co Applicant Name")]
        public string CoApplicantName { get; set; }
        [Required(ErrorMessage = "Member Category is required.")]
        [Display(Name = "Member Category")]
        public byte MemberCategoryID { get; set; }
        /// <summary>
        /// 0 = InActive, 1 = Active, 2 = Drop, 3 = Dead
        /// </summary>     
        [Display(Name = "Member Status")]
        public string MemberStatus { get; set; }
        [Display(Name = "Release Date")]
        public Nullable<System.DateTime> ReleaseDate { get; set; }
        public string City { get; set; }

        public int? FinServiceChoiceId { get; set; }

        public int? TransactionChoiceId { get; set; }

        public string AsOnDateAge { get; set; }
        public string FamilyContactNo { get; set; }
        public DateTime? CardIssueDate { get; set; }

        public string FServiceName { get; set; }

        public bool IsAnyFS { get; set; }

        [Display(Name = "State")]
        public string StateName { get; set; }
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        public string CountryOfIssue { get; set; }
        public string NIDComments { get; set; }
        public string IDType { get; set; }
        public string Race { get; set; }
        public string Ethnicity { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Contact No. is required")]
        [Display(Name = "Contact No")]
        public string PhoneNo { get; set; }
        public string nsAccountNo { get; set; }
        public Nullable<byte> MemberType { get; set; }

        [Display(Name = "Member Image(Max Size 100 kb)")]
        public byte[] MemberImg { get; set; }

        [Display(Name = "Member Image(Max Size 100 kb)")]
        public HttpPostedFileBase ImgFile { get; set; }

        [Display(Name = "Thumb Image(Max Size 100 kb)")]
        public byte[] ThumbImg { get; set; }

        [Display(Name = "Thumb Image(Max Size 100 kb)")]
        public HttpPostedFileBase ThumbImgFile { get; set; }
        public string PwdStatus { get; set; }

        [Display(Name = "Member Type")]
        public string MemCategory { get; set; }

        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }

        public short ProductID { get; set; }
        public string ProductName { get; set; }
        public string MemberImage64String { get; set; }
        public string MemberSignature64String { get; set; }
        public IEnumerable<SelectListItem> OfficeList { get; set; }
        public IEnumerable<SelectListItem> CenterList { get; set; }
        public IEnumerable<SelectListItem> GroupList { get; set; }
        public IEnumerable<SelectListItem> MemberCategoryList { get; set; }
        public IEnumerable<SelectListItem> GeoLocationList { get; set; }
        public IEnumerable<SelectListItem> GenderList { get; set; }
        public IEnumerable<SelectListItem> MemberStatusList { get; set; }
        public IEnumerable<SelectListItem> PlaceOfBirthList { get; set; }
        public IEnumerable<SelectListItem> CityzenshipList { get; set; }

        public IEnumerable<SelectListItem> CountryList { get; set; }
        public IEnumerable<SelectListItem> DivisionList { get; set; }
        public IEnumerable<SelectListItem> DistrictList { get; set; }
        public IEnumerable<SelectListItem> UpozillaList { get; set; }
        public IEnumerable<SelectListItem> UnionList { get; set; }
        public IEnumerable<SelectListItem> VillageList { get; set; }

        public IEnumerable<SelectListItem> HomeTypeList { get; set; }
        public IEnumerable<SelectListItem> GroupTypeList { get; set; }
        public IEnumerable<SelectListItem> EducationList { get; set; }
        public IEnumerable<SelectListItem> EconomicActivityList { get; set; }

        public IEnumerable<SelectListItem> MaritalStatusList { get; set; }
        public IEnumerable<SelectListItem> MemCategoryList { get; set; }
    }// End Class
}// ENd Namespace