namespace gBanker.Data.CodeFirstMigration.Db
{
    using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LoanTrx")]
    public partial class LoanTrx
    {
        public long LoanTrxID { get; set; }

        [Column(TypeName = "date")]
        public DateTime TrxDate { get; set; }

        public long LoanSummaryID { get; set; }

        public int OfficeID { get; set; }

        public long MemberID { get; set; }

        public short ProductID { get; set; }

        public int CenterID { get; set; }

        public byte MemberCategoryID { get; set; }

        public int LoanTerm { get; set; }

        [Column(TypeName = "date")]
        public DateTime InstallmentDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal PrincipalLoan { get; set; }

        [Column(TypeName = "numeric")]
        public decimal LoanDue { get; set; }

        [Column(TypeName = "numeric")]
        public decimal LoanPaid { get; set; }

        [Column(TypeName = "numeric")]
        public decimal IntCharge { get; set; }

        [Column(TypeName = "numeric")]
        public decimal IntDue { get; set; }

        [Column(TypeName = "numeric")]
        public decimal IntPaid { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Advance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DueRecovery { get; set; }

        public byte TrxType { get; set; }

        public short InstallmentNo { get; set; }

        public short EmployeeID { get; set; }
        public byte InvestorID { get; set; }
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

        public virtual LoanSummary LoanSummary { get; set; }

        //public virtual LoanTrx LoanTrx1 { get; set; }

        //public virtual LoanTrx LoanTrx2 { get; set; }

        public virtual Member Member { get; set; }

        public virtual Office Office { get; set; }

        public virtual Product Product { get; set; }
    }
}
