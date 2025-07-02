namespace gBanker.Data.CodeFirstMigration
{
    using gBanker.Data.CodeFirstMigration.Db;
    using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RepaymentSchedule")]
    public partial class RepaymentSchedule
    {
        public long RepaymentScheduleID { get; set; }

        public long LoanSummaryID { get; set; }

        public int OfficeID { get; set; }

        public long MemberID { get; set; }

        public short ProductID { get; set; }

        public int CenterID { get; set; }

        public byte MemberCategoryID { get; set; }

        public int LoanTerm { get; set; }

        [Column(TypeName = "date")]
        public DateTime RepaymentDate { get; set; }

        public int InstallmentNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal LoanInstallment { get; set; }

        [Column(TypeName = "numeric")]
        public decimal IntInstallment { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? IntCharge { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PrincipalLoan { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LoanBalnce { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? InterestBalance { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
        public int OrgID { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Center Center { get; set; }
        public virtual Member Member { get; set; }
        public virtual Product Product { get; set; }
        public virtual LoanSummary LoanSummary { get; set; }
        //public virtual LoanTrx LoanTrx { get; set; }
    }
}
