namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("inv.InventoryDailyVoucher")]
    public partial class InventoryDailyVoucher
    {
        [Key]
        public long ID { get; set; }
        public DateTime TransactionDate { get; set; }
        public string VoucherNo { get; set; }
        public int OfficeID { get; set; }
        public int AccID { get; set; }
        public string NarationEng { get; set; }
        public string NarationBng { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string VoucherType { get; set; }
        public string ReconPurposeCode { get; set; }
        public string Reference { get; set; }
        public bool? IsVoucherChange { get; set; }
        public int? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}
