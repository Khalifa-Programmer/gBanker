using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class MemberCorrectionForMemberViewModel : BaseModel
    {
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        [Display(Name = "Office")]
        public int OfficeID { get; set; }
        [Display(Name = "Samity")]
        [Required(ErrorMessage = "Samity Name is required.")]
        public int CenterID { get; set; }
        [Display(Name = "Group")]
        [Required(ErrorMessage = "Group is required.")]
        public short GroupID { get; set; }
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Address 1")]
        [Required(ErrorMessage = "AddressLine 1 is required.")]
        public string AddressLine1 { get; set; }
        [Display(Name = "Address 2")]
        public string AddressLine2 { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "Country is required.")]
        public int CountryID { get; set; }

        [Display(Name = "Division")]
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }

        [Display(Name = "District")]
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }

        [Display(Name = "Upzilla")]
       // [Required(ErrorMessage = "UpzillaCode is required.")]
        public string UpozillaCode { get; set; }
        public string UpozillaName { get; set; }

        [Display(Name = "Union")]
        public string UnionCode { get; set; }
        public string UnionName { get; set; }

        [Display(Name = "Village")]
        public string VillageCode { get; set; }
        public string VillageName { get; set; }

        [Display(Name = "Referee Name")]
        public string RefereeName { get; set; }

        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Date Of Birth is required.")]
        public Nullable<System.DateTime> BirthDate { get; set; }

        [Display(Name = "Place of Birth")]
        public string PlaceOfBirth { get; set; }
        [Display(Name = "Citizenship")]
        public string Cityzenship { get; set; }

        [Display(Name = "Joining Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime JoinDate { get; set; }
        public System.DateTime ServerCurrentDate { get; set; }
        //[Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }
        [Display(Name = "NID/ Birth Certificate")]
        [Required(ErrorMessage = "NID/ Birth Cer. is required.")]
        public string NationalID { get; set; }
        public string Location { get; set; }
        //[Required(ErrorMessage = "Home Type is required.")]
        [Display(Name = "Home Type")]
        public string HomeType { get; set; }

        [Display(Name = "Group Type")]
        public string GroupType { get; set; }
        public string Education { get; set; }

        [Display(Name = "Family Member")]
        [Range(0, 100)]
        public int FamilyMember { get; set; }

        [Display(Name = "Total Wealth")]
        public string TotalWealth { get; set; }

        [Display(Name = "Economic Activity")]
        public string EconomicActivity { get; set; }
        [Required(ErrorMessage = "Father Name is required.")]
        [Display(Name = "Father Name")]
        public string FatherName { get; set; }
        [Required(ErrorMessage = "Mother Name is required.")]
        [Display(Name = "Mother Name")]
        public string MotherName { get; set; }
        [Required(ErrorMessage = "Co Applicant Name is required.")]
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

        [Display(Name = "Member Image")]
        public byte[] MemberImg { get; set; }

        [Display(Name = "Member Image")]
        public HttpPostedFileBase ImgFile { get; set; }

        [Display(Name = "Thumb Image")]
        public byte[] ThumbImg { get; set; }

        [Display(Name = "Thumb Image")]
        public HttpPostedFileBase ThumbImgFile { get; set; }
        public string PwdStatus { get; set; }

        [Display(Name = "Member Type")]
        public string MemCategory { get; set; }

        [Display(Name = "Marital Status")]
        public string MaritalStatus { get; set; }

        //[Range(0, int.MaxValue)]
        //public int X { get; set; }

        //[Range(0, int.MaxValue)]
        //public int Y { get; set; }

        //[Range(1, int.MaxValue)]
        //public int Width { get; set; }

        //[Range(1, int.MaxValue)]
        //public int Height { get; set; }

        public short ProductID { get; set; }
        public string ProductName { get; set; }
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
    }
}