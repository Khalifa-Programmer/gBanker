namespace gBanker.Data.CodeFirstMigration.Db
{
    using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SavingTrx")]
    public partial class SavingTrx
    {
        [Key]
        public long SavingTrxID { get; set; }

        public long SavingSummaryID { get; set; }

        public int OfficeID { get; set; }

        public long MemberID { get; set; }

        public short ProductID { get; set; }

        public int CenterID { get; set; }

        public int NoOfAccount { get; set; }

        [Column(TypeName = "date")]
        public DateTime TransactionDate { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Deposit { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Withdrawal { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Balance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Penalty { get; set; }

        public byte TransType { get; set; }

        [Column(TypeName = "numeric")]
        public decimal MonthlyInterest { get; set; }

        public bool PresenceInd { get; set; }

        [Column(TypeName = "numeric")]
        public decimal TransferDeposit { get; set; }

        [Column(TypeName = "numeric")]
        public decimal TransferWithdrawal { get; set; }

        public Int16? EmployeeID { get; set; }

        public byte MemberCategoryID { get; set; }

        public long IsActive { get; set; }

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

        public virtual Office Office { get; set; }

        public virtual Product Product { get; set; }
    }
}
