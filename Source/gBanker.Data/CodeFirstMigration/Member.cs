namespace gBanker.Data.CodeFirstMigration.Db
{
    using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Member")]
    public partial class Member
    {
        public Member()
        {
            DailyLoanTrxes = new HashSet<DailyLoanTrx>();
            LoanSummaries = new HashSet<LoanSummary>();
            LoanTrxes = new HashSet<LoanTrx>();
            SavingSummaries = new HashSet<SavingSummary>();
        }

        public long MemberID { get; set; }

        [Required]
        [StringLength(20)]
        public string MemberCode { get; set; }
        [StringLength(20)]
        public string OldMemberCode { get; set; }

        public int OfficeID { get; set; }

        public int CenterID { get; set; }

        public short GroupID { get; set; }

        [StringLength(350)]
        public string BanglaName { get; set; }

        [Display(Name = "First Name")]
        [StringLength(150, ErrorMessage = "Maximum length is {1}")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(150, ErrorMessage = "Maximum length is {1}")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        public string LastName { get; set; }

        [StringLength(75)]
        public string AddressLine1 { get; set; }

        [StringLength(75)]
        public string AddressLine2 { get; set; }
        public int? CountryID { get; set; }

        public int? ProvidedByCountryID { get; set; }

        public int? FinServiceChoiceId { get; set; }

        public int? TransactionChoiceId { get; set; }



        [StringLength(255)]
        public string DivisionCode { get; set; }

        [StringLength(255)]
        public string DistrictCode { get; set; }

        [StringLength(255)]
        public string UpozillaCode { get; set; }

        [StringLength(255)]
        public string UnionCode { get; set; }

        [StringLength(255)]
        public string VillageCode { get; set; }


        //FOR Permanent Address

        [StringLength(75)]
        public string PerAddressLine1 { get; set; }

        [StringLength(75)]
        public string PerAddressLine2 { get; set; }
        public int? PerCountryID { get; set; }

        [StringLength(255)]
        public string PerDivisionCode { get; set; }

        [StringLength(255)]
        public string PerDistrictCode { get; set; }

        [StringLength(255)]
        public string PerUpozillaCode { get; set; }

        [StringLength(255)]
        public string PerUnionCode { get; set; }

        [StringLength(255)]
        public string PerVillageCode { get; set; }

        [StringLength(5)]
        public string PerZipCode { get; set; }

        public int? EmployeeID { get; set; }

        //








        [StringLength(45)]
        public string RefereeName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BirthDate { get; set; }

        [StringLength(255)]
        public string PlaceOfBirth { get; set; }

        [StringLength(5)]
        public string Cityzenship { get; set; }

        [Column(TypeName = "date")]
        public DateTime JoinDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpireDate { get; set; }

        [Required]
        [StringLength(7)]
        public string Gender { get; set; }

        [StringLength(20)]
        public string SmartCard { get; set; }
        // [Required]
        [StringLength(20)]
        public string NationalID { get; set; }

        public string OtherIdNo { get; set; }

        public int? IdentTypeID { get; set; }

        [StringLength(12)]
        public string Location { get; set; }

        [StringLength(5)]
        public string HomeType { get; set; }

        [StringLength(5)]
        public string GroupType { get; set; }

        [StringLength(5)]
        public string Education { get; set; }

        public int? FamilyMember { get; set; }

        [StringLength(100)]
        public string TotalWealth { get; set; }

        [StringLength(5)]
        public string EconomicActivity { get; set; }

        [StringLength(200)]
        public string FatherName { get; set; }

        [StringLength(200)]
        public string FatherNameBN { get; set; }

        [StringLength(200)]
        public string SpouseName { get; set; }

        [StringLength(200)]
        public string SpouseNameBN { get; set; }

        [StringLength(200)]
        public string TIN { get; set; }

        public decimal? TaxAmount { get; set; }

        [StringLength(200)]
        public string MotherName { get; set; }

        [StringLength(200)]
        public string MotherNameBN { get; set; }

        [StringLength(200)]
        public string CoApplicantName { get; set; }

        public byte MemberCategoryID { get; set; }

        public string MemberStatus { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ReleaseDate { get; set; }

        [StringLength(35)]
        public string City { get; set; }

        [StringLength(35)]
        public string StateName { get; set; }

        [StringLength(5)]
        public string ZipCode { get; set; }

        [StringLength(50)]
        public string CountryOfIssue { get; set; }

        [StringLength(25)]
        public string NIDComments { get; set; }

        [StringLength(15)]
        public string IDType { get; set; }

        [StringLength(55)]
        public string Race { get; set; }

        [StringLength(55)]
        public string Ethnicity { get; set; }

        [StringLength(55)]
        public string Email { get; set; }

        [StringLength(35)]
        public string PhoneNo { get; set; }

        [StringLength(50)]
        public string nsAccountNo { get; set; }

        public byte? MemberType { get; set; }
        public byte[] MemberImg { get; set; }
        public byte[] ThumbImg { get; set; }

        [StringLength(2)]
        public string PwdStatus { get; set; }
        public string MemCategory { get; set; }
        public string MaritalStatus { get; set; }

        public string FServiceName { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsAnyFS { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(15)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
        public int OrgID { get; set; }
        [StringLength(255)]
        public string MemberNameBng { get; set; }
        public string AsOnDateAge { get; set; }
        public string FamilyContactNo { get; set; }
        public DateTime? CardIssueDate { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Center Center { get; set; }

        public virtual ICollection<DailyLoanTrx> DailyLoanTrxes { get; set; }

        public virtual Group Group { get; set; }

        public virtual ICollection<LoanSummary> LoanSummaries { get; set; }

        public virtual ICollection<LoanTrx> LoanTrxes { get; set; }

        public virtual Office Office { get; set; }

        public virtual ICollection<SavingSummary> SavingSummaries { get; set; }

        public virtual MemberCategory MemberCategory { get; set; }
        public virtual ICollection<RepaymentSchedule> RepaymentSchedules { get; set; }

    }
}
