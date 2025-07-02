namespace gBanker.Data.CodeFirstMigration.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InterbranchTransactionTracking")]
    public partial class InterbranchTransactionTracking
    {
        [Key]
        public int InterbranchTransactionTrackingID { get; set; }
        public int OfficeID { get; set; }
        public Int64 SummaryID { get; set; }
        public decimal LoanPaid { get; set; }
        public decimal IntPaid { get; set; }
        public decimal Deposit { get; set; }
        public decimal Withdrawal { get; set; }
        public string LoanType { get; set; }
        public DateTime TrxDate { get; set; } 
        public bool? IsActive { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
