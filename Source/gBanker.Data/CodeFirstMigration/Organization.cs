namespace gBankerCodeFirstMigration.Db
{
    using gBanker.Data.CodeFirstMigration;
    using gBanker.Data.CodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Organization")]
    public partial class Organization
    {
        //public Organization()
        //{
        //    AccCharts = new HashSet<AccChart>();
        //    AccNotes = new HashSet<AccNote>();
        //    ApplicationSettings = new HashSet<ApplicationSetting>();
        //    AspNetRoles = new HashSet<AspNetRole>();
        //    Centers = new HashSet<Center>();
        //    DailyLoanTrxes = new HashSet<DailyLoanTrx>();
        //    DailySavingTrxes = new HashSet<DailySavingTrx>();
        //    Employees = new HashSet<Employee>();
        //    EmployeeOfficeMappings = new HashSet<EmployeeOfficeMapping>();
        //    GeoLocations = new HashSet<GeoLocation>();
        //    Groups = new HashSet<Group>();
        //    Holidays = new HashSet<Holiday>();
        //    Investors = new HashSet<Investor>();
        //    LoanCorrectionTrxes = new HashSet<LoanCorrectionTrx>();
        //    LoanSummaries = new HashSet<LoanSummary>();
        //    LoanTrxes = new HashSet<LoanTrx>();
        //    Members = new HashSet<Member>();
        //    MemberCategories = new HashSet<MemberCategory>();
        //    MemberLastCodes = new HashSet<MemberLastCode>();
        //    Offices = new HashSet<Office>();
        //    AccountingInterfaces = new HashSet<AccountingInterface>();
        //    FamilyGraces = new HashSet<FamilyGrace>();
        //    MonthWiseSavingInterests = new HashSet<MonthWiseSavingInterest>();
        //    ProcessInfoes = new HashSet<ProcessInfo>();
        //    Products = new HashSet<Product>();
        //    Purposes = new HashSet<Purpose>();
        //    RepaymentSchedules = new HashSet<RepaymentSchedule>();
        //    SavingCorrectionTrxes = new HashSet<SavingCorrectionTrx>();
        //    SavingSummaries = new HashSet<SavingSummary>();
        //    SavingTrxes = new HashSet<SavingTrx>();
        //    SchedulerDetails = new HashSet<SchedulerDetail>();
        //    TransferHistories = new HashSet<TransferHistory>();
        //}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrgID { get; set; }

        [StringLength(20)]
        public string OrganizationCode { get; set; }

        [StringLength(50)]
        public string OrganizationName { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
        [StringLength(150)]
        public string OrgAddress { get; set; }

        public byte[] OrgLOGO { get; set; }
        public int? MemberAge { get; set; }
        public int? GuarantorAge { get; set; }
        //public virtual ICollection<AccChart> AccCharts { get; set; }

        //public virtual ICollection<AccNote> AccNotes { get; set; }

        //public virtual ICollection<ApplicationSetting> ApplicationSettings { get; set; }

        //public virtual ICollection<AspNetRole> AspNetRoles { get; set; }

        //public virtual ICollection<Center> Centers { get; set; }

        //public virtual ICollection<DailyLoanTrx> DailyLoanTrxes { get; set; }

        //public virtual ICollection<DailySavingTrx> DailySavingTrxes { get; set; }

        //public virtual ICollection<Employee> Employees { get; set; }

        //public virtual ICollection<EmployeeOfficeMapping> EmployeeOfficeMappings { get; set; }

        //public virtual ICollection<GeoLocation> GeoLocations { get; set; }

        //public virtual ICollection<Group> Groups { get; set; }

        //public virtual ICollection<Holiday> Holidays { get; set; }

        //public virtual ICollection<Investor> Investors { get; set; }

        //public virtual ICollection<LoanCorrectionTrx> LoanCorrectionTrxes { get; set; }

        //public virtual ICollection<LoanSummary> LoanSummaries { get; set; }

        //public virtual ICollection<LoanTrx> LoanTrxes { get; set; }

        //public virtual ICollection<Member> Members { get; set; }

        //public virtual ICollection<MemberCategory> MemberCategories { get; set; }

        //public virtual ICollection<MemberLastCode> MemberLastCodes { get; set; }

        //public virtual ICollection<Office> Offices { get; set; }

        //public virtual ICollection<AccountingInterface> AccountingInterfaces { get; set; }

        //public virtual ICollection<FamilyGrace> FamilyGraces { get; set; }

        //public virtual ICollection<MonthWiseSavingInterest> MonthWiseSavingInterests { get; set; }

        //public virtual ICollection<ProcessInfo> ProcessInfoes { get; set; }

        //public virtual ICollection<Product> Products { get; set; }

        //public virtual ICollection<Purpose> Purposes { get; set; }

        //public virtual ICollection<RepaymentSchedule> RepaymentSchedules { get; set; }

        //public virtual ICollection<SavingCorrectionTrx> SavingCorrectionTrxes { get; set; }

        //public virtual ICollection<SavingSummary> SavingSummaries { get; set; }

        //public virtual ICollection<SavingTrx> SavingTrxes { get; set; }

        //public virtual ICollection<SchedulerDetail> SchedulerDetails { get; set; }

        //public virtual ICollection<TransferHistory> TransferHistories { get; set; }
    }
}
