namespace gBanker.Data.CodeFirstMigration.Db
{
    using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Office")]
    public partial class Office
    {
        public Office()
        {
            ApplicationSettings = new HashSet<ApplicationSetting>();
            Centers = new HashSet<Center>();
            DailyLoanTrxes = new HashSet<DailyLoanTrx>();
            Employees = new HashSet<Employee>();
            EmployeeOfficeMappings = new HashSet<EmployeeOfficeMapping>();
            Groups = new HashSet<Group>();
            Holidays = new HashSet<Holiday>();
            LoanCorrectionTrxes = new HashSet<LoanCorrectionTrx>();
            LoanSummaries = new HashSet<LoanSummary>();
            LoanTrxes = new HashSet<LoanTrx>();
            Members = new HashSet<Member>();
            FamilyGraces = new HashSet<FamilyGrace>();
            SavingSummaries = new HashSet<SavingSummary>();
            SavingTrxes = new HashSet<SavingTrx>();
            SchedulerDetails = new HashSet<SchedulerDetail>();
            //Centers = new HashSet<Center>();
            //DailyLoanTrxes = new HashSet<DailyLoanTrx>();
            //DailySavingTrxes = new HashSet<DailySavingTrx>();
            //Employees = new HashSet<Employee>();
            //Groups = new HashSet<Group>();
            //LoanSummaries = new HashSet<LoanSummary>();
            //LoanTrxes = new HashSet<LoanTrx>();
            //Members = new HashSet<Member>();
            //SavingSummaries = new HashSet<SavingSummary>();
            //SavingTrxes = new HashSet<SavingTrx>();
            //EmployeeOfficeMappings = new HashSet<EmployeeOfficeMapping>();
        }

        public int OfficeID { get; set; }

        [Required]
        [StringLength(10)]
        public string OfficeCode { get; set; }

        [Required]
        [StringLength(40)]
        public string OfficeName { get; set; }

        public byte OfficeLevel { get; set; }

        [Required]
        [StringLength(10)]
        public string FirstLevel { get; set; }

        [StringLength(10)]
        public string SecondLevel { get; set; }

        [StringLength(10)]
        public string ThirdLevel { get; set; }

        [StringLength(10)]
        public string FourthLevel { get; set; }

        [Column(TypeName = "date")]
        public DateTime OperationStartDate { get; set; }

        [StringLength(155)]
        public string OfficeAddress { get; set; }

        [StringLength(10)]
        public string PostCode { get; set; }

        public int? GeoLocationID { get; set; }

        [StringLength(45)]
        public string Email { get; set; }

        [StringLength(35)]
        public string Phone { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
        public int OrgID { get; set; }
        public virtual ICollection<ApplicationSetting> ApplicationSettings { get; set; }

        public virtual ICollection<Center> Centers { get; set; }

        public virtual ICollection<DailyLoanTrx> DailyLoanTrxes { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        public virtual ICollection<EmployeeOfficeMapping> EmployeeOfficeMappings { get; set; }

        public virtual GeoLocation GeoLocation { get; set; }

        public virtual ICollection<Group> Groups { get; set; }

        public virtual ICollection<Holiday> Holidays { get; set; }

        public virtual ICollection<LoanCorrectionTrx> LoanCorrectionTrxes { get; set; }

        public virtual ICollection<LoanSummary> LoanSummaries { get; set; }

        public virtual ICollection<LoanTrx> LoanTrxes { get; set; }

        public virtual ICollection<Member> Members { get; set; }

        public virtual ICollection<FamilyGrace> FamilyGraces { get; set; }

        public virtual Organization Organization { get; set; }

        public virtual ICollection<SavingSummary> SavingSummaries { get; set; }

        public virtual ICollection<SavingTrx> SavingTrxes { get; set; }

        public virtual ICollection<SchedulerDetail> SchedulerDetails { get; set; }
        //public virtual Organization Organization { get; set; }
        //public virtual ICollection<Center> Centers { get; set; }

        //public virtual ICollection<DailyLoanTrx> DailyLoanTrxes { get; set; }

        //public virtual ICollection<DailySavingTrx> DailySavingTrxes { get; set; }

        //public virtual ICollection<Employee> Employees { get; set; }

        //public virtual GeoLocation GeoLocation { get; set; }

        //public virtual ICollection<Group> Groups { get; set; }

        //public virtual ICollection<Holiday> Holidays { get; set; }

        //public virtual ICollection<LoanSummary> LoanSummaries { get; set; }

        //public virtual ICollection<LoanTrx> LoanTrxes { get; set; }

        //public virtual ICollection<Member> Members { get; set; }

        //public virtual ICollection<SavingSummary> SavingSummaries { get; set; }

        //public virtual ICollection<SavingTrx> SavingTrxes { get; set; }
        //public virtual ICollection<EmployeeOfficeMapping> EmployeeOfficeMappings { get; set; }
        //public int? InvestorID { get; set; }

        public int? InvestorID { get; set; }
        //public byte? InvestorID { get; set; }

        public int? UnionID { get; set; }
    }
}
