using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Data.Entity;


namespace gBanker.Web.ViewModels
{
    public class SurveyMemberMasterViewModel : BaseModel
    {
        //public int MemberSurveyID { get; set; }
        //public string surveycode { get; set; }
        public string Center { get; set; }
        public IEnumerable<SelectListItem> CenterList { get; set; }

        //[Column(TypeName = "date")]
        //[Display(Name = "Survey Date")]
        //public DateTime SurveyDate { get; set; }

        //public string FirstName { get; set; }
        //public string MiddleName { get; set; }
        //public string LastName { get; set; }
        //public string FullName { get; set; }
        //[Column(TypeName = "date")]
        //[Display(Name = "Birth Date")]
        //public DateTime BirthDate { get; set; }
        public string Ocupation { get; set; }
        public string Education { get; set; }
        public IEnumerable<SelectListItem> EducationList { get; set; }

        public string MeritalStatus { get; set; }
        public IEnumerable<SelectListItem> MaritalStatusList { get; set; }


        public int SurveyId { get; set; }
        public string SurveyCode { get; set; }
        //public string Center { get; set; }
        public DateTime SurveyDate { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MemberFullName { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string PresentCountryId { get; set; }
        public string PermanentCountryId { get; set; }
        public string PresentAddressPOBCode { get; set; }
        public string PermanentAddressPOBCode { get; set; }
        public string RefereeName { get; set; }
        public DateTime BirthDate { get; set; }
        public string PlaceOfBirth { get; set; }
        public int CityzenshipId { get; set; }
        public bool IsAnyRelationwithOtherNGO { get; set; }
        




        [Display(Name = "Birth Division")]
        public string BirthDivision { get; set; }

        [Display(Name = "Birth District")]
        public string BirthDistrict { get; set; }

        public string BirthDivisionName { get; set; }

        public string BirthDivisionCode { get; set; }

        public string BirthDistrictName { get; set; }
        public string BirthDistrictCode { get; set; }

        public string BirthVillageName { get; set; }
        public string BirthVillageCode { get; set; }
        public string UnionCode { get; set; }
        public string UnionName { get; set; }

        [Required]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Display(Name = "Birth Union")]
        public string BirthUnion { get; set; }



        //varificationdocument
        public int SMVerificationId { get; set; }
        public int SurveryId { get; set; }
        public string VarificationNo { get; set; }
        public int VarificationTypeId { get; set; }
        [Display(Name = "A/C Form Scan Copy")]
        public byte[] VarificationDocument { get; set; }
        [Display(Name = "A/C Form Scan Copy")]
        public string VarifyDocument { get; set; }
        public HttpPostedFileBase VarificationDocumentImageFile { get; set; }
        public string Remarks { get; set; }
        // SurveyMemberFamilyInformation
        public long SurveyFamilyId { get; set; }
        public int RelationshipId { get; set; }
        [Display(Name = "Occupation")]
        public int OccupationId { get; set; }
        public int NoOfPerson { get; set; }
        public bool IsEarningCapable { get; set; }
        public decimal IncomeMonthly { get; set; }
        public bool IsAnyOtherPrivateBusiness { get; set; }
        public decimal IncomeMonthlyFromPrivateBusiness { get; set; }
        public IEnumerable<SelectListItem> RelationshipIdList { get; set; }
        public IEnumerable<SelectListItem> OccupationIdList { get; set; }
        public IEnumerable<SelectListItem> IsEarningCapableList { get; set; }
        public IEnumerable<SelectListItem> IsAnyOtherPrivateBusinessList { get; set; }
        //SurveyMemberAccomodationInformation
        public long SMAccomodationId { get; set; }
        public bool IsOwnHome { get; set; }
        public string ResidenceAddress { get; set; }
        public DateTime? ResideFrom { get; set; }
        public DateTime? ResideTo { get; set; }
        public string HomeOwnerName { get; set; }
        public int? HomeOwnerOccupationId { get; set; }
        public int? IsRentPaymentRegular { get; set; }
        public int? IsUseRentalMemberForLoanPurpose { get; set; }
        public IEnumerable<SelectListItem> IsOwnHomeList { get; set; }
        public IEnumerable<SelectListItem> IsRentPaymentRegularList { get; set; }
        public IEnumerable<SelectListItem> IsUseRentalMemberForLoanPurposeList { get; set; }
        //SurveyMemberAsset
        public long SMAssetId { get; set; }
        public int AssetId { get; set; }
        public decimal AssetAmount { get; set; }
        public IEnumerable<SelectListItem> AssetIdList { get; set; }
        //SurveyMemberExpenditure
        public long SurveyExpenditureId { get; set; }
        public int ExpenditureId { get; set; }
        public decimal ExpendetureAmount { get; set; }
        public IEnumerable<SelectListItem> ExpenditureIdList { get; set; }
        //SurveyMemberOperationwithOtherNGOInformation
        public long SMNGOId { get; set; }
        public int NGOId { get; set; }
        public decimal LoanAmount { get; set; }
        public IEnumerable<SelectListItem> NGOIdList { get; set; }
        //SurveyMemberFamilyEducationInformation
        public long SMEducationId { get; set; }
        public int InstituteId { get; set; }
        public int InstituteTypeId { get; set; }
        public string ClassName { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public IEnumerable<SelectListItem> InstituteIdList { get; set; }
        public IEnumerable<SelectListItem> InstituteTypeIdList { get; set; }
        //SurveyKnownMember
        public long KnownMemberId { get; set; }
        public string MemberCode { get; set; }
        public long IsBloodRelated { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public IEnumerable<SelectListItem> IsBloodRelatedList { get; set; }
        



        public bool? IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }

    }
}

