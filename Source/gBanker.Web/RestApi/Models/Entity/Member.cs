using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class Member
    {
        public long MemberID { get; set; }
        public long? MemberPassBookNo { get; set; }
        public long? MemberPassBookRegisterID { get; set; }
        public int CenterID { get; set; }
        public short GroupID { get; set; }
        public string MemberCode { get; set; }
        public byte MemberCategoryID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string MaritalStatus { get; set; }
        public string SpouseName { get; set; }

        public int? CountryID { get; set; }
        public string DivisionCode { get; set; }
        public string DistrictCode { get; set; }
        public string UpozillaCode { get;set; }
        public string UnionCode { get; set; }
        public string VillageCode { get; set; }
        public string AddressLine1 { get; set; }
        public string ZipCode { get; set; }

        public int? PerCountryID { get; set; }
        public string PerDivisionCode { get; set; }
        public string PerDistrictCode { get; set; }
        public string PerUpozillaCode { get; set; }
        public string PerUnionCode { get; set; }

        public string PerVillageCode { get; set; }
        public string PerAddressLine1 { get; set; }
        public string PerZipCode { get; set; }


        public string NationalID { get; set; }
        public string SmartCard { get; set; }
        public int? IdentTypeID { get; set; }
        public DateTime? CardIssueDate { get; set; }
        public string OtherIdNo { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int? ProvidedByCountryID { get; set; }

        public DateTime? BirthDate { get; set; }
        public string AsOnDateAge { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Cityzenship { get; set; }
        public string Gender { get; set; }
        public DateTime? JoinDate { get; set; }

        public string HomeType { get; set; }
        public string GroupType { get; set; }
        public string Education { get; set; }
        public int? FamilyMember { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string FamilyContactNo { get; set; }
        public string RefereeName { get; set; }
        public string CoApplicantName { get; set; }
        public string EconomicActivity { get; set; }
        public string TotalWealth { get; set; }
        public byte MemberType { get; set; }
        public string TIN { get; set; }
        public Decimal? TaxAmount { get; set; }
        public bool? IsAnyFS { get; set; }
        public string FServiceName { get; set; }
        public int? FinServiceChoiceId { get; set; }
        public int? TransactionChoiceId { get; set; }
        public int OfficeID { get; set; }
        public int OrgID { get; set; }
        public byte[] MemberImg { get; set; }
        public byte[] ThumbImg { get; set; }
        public bool ImgSync { get; set; }
        public bool SigImgSync { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }

        public string MemberStatus { get; set; }
    }
}