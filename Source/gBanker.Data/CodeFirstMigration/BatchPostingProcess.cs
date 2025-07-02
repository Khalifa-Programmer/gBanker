namespace gBanker.Data.CodeFirstMigration.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BatchPostingProcess")]
    public partial class BatchPostingProcess
    {
        [Key]
        public long BatchId { get; set; }

        [Column(TypeName = "date")]
        public DateTime TransactionDate { get; set; }

        [StringLength(12)]
        public string OfficeCode { get; set; }

        [Required]
        [StringLength(20)]
        public string AccountCode { get; set; }

        [Required]
        [StringLength(300)]
        public string AccountName { get; set; }

        [Required]
        [StringLength(10)]
        public string VoucherType { get; set; }

        [Required]
        public string Narration { get; set; }

        public decimal Credit { get; set; }

        public decimal Debit { get; set; }

        public bool IsRemoved { get; set; }

        public int CreateBy { get; set; }

        public DateTime CreateDate { get; set; }

        public bool? IsPosted { get; set; }

        public DateTime? PostedDate { get; set; }

        public int? PostedBy { get; set; }

        public int? UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        [StringLength(50)]
        public string BatchFileNo { get; set; }
    }
}
