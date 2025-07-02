namespace gBanker.Data.CodeFirstMigration.Db
{
    using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Center")]
    public partial class Center
    {
        public Center()
        {
            DailyLoanTrxes = new HashSet<DailyLoanTrx>();
            LoanSummaries = new HashSet<LoanSummary>();
            LoanTrxes = new HashSet<LoanTrx>();
            Members = new HashSet<Member>();
            SavingSummaries = new HashSet<SavingSummary>();
        }

        public int CenterID { get; set; }

        [Required]
        [StringLength(50)]
        public string CenterCode { get; set; }

        public int OfficeID { get; set; }

        [Required]
        [StringLength(50)]
        public string CenterName { get; set; }

        [StringLength(155)]
        public string CenterAddress { get; set; }

        [StringLength(50)]
        public string CenterNameBng { get; set; }
        public string Organizer { get; set; }

        public short EmployeeId { get; set; }

        [Required]
        [StringLength(10)]
        public string CollectionDay { get; set; }

        [Column(TypeName = "date")]
        public DateTime CollectionDate { get; set; }

        public int? GeoLocationID { get; set; }

        [Column(TypeName = "date")]
        public DateTime OperationStartDate { get; set; }

        public byte CenterStatus { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
        public int OrgID { get; set; }

        public byte CenterTypeID { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual GeoLocation GeoLocation { get; set; }

        public virtual Office Office { get; set; }

        public virtual ICollection<Holiday> Holidays { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<DailyLoanTrx> DailyLoanTrxes { get; set; }

        public virtual ICollection<LoanSummary> LoanSummaries { get; set; }

        public virtual ICollection<LoanTrx> LoanTrxes { get; set; }

        public virtual ICollection<Member> Members { get; set; }

        public virtual ICollection<SavingSummary> SavingSummaries { get; set; }
        public virtual ICollection<RepaymentSchedule> RepaymentSchedules { get; set; }
        public DateTime? CenterTime { get; set; }

        public string CenterDistance { get; set; }

        ///Address Fields
        ///
        // Address Added By KHALID ON 15 March, 2022

        public int? CountryID { get; set; }

        public string DivisionCode { get; set; }

        public string DistrictCode { get; set; }

        public string UpozillaCode { get; set; }

        public string UnionCode { get; set; }

        public string VillageCode { get; set; }

        public string ZipCode { get; set; }

        public string AddressLine1 { get; set; }

        public long? CenterChief { get; set; }
        public long? AssoCenterChief { get; set; }
        public long? PanelMember { get; set; }

        public int? CenterCategoryID { get; set; } //_25

        /*public virtual Member Member{ get; set; } *///CenterChief_dtl 
        //public virtual Member AssoCenterChief_dtl { get; set; }
    }
}
