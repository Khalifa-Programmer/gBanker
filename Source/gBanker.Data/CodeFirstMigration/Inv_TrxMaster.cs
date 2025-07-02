namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("inv.Inv_TrxMaster")]
    public partial class Inv_TrxMaster
    {
        [Key]
        public long TrxMasterID { get; set; }

        public int OfficeID { get; set; }

        [Column(TypeName = "date")]
        public DateTime TrxDate { get; set; }

        [Required]
        [StringLength(50)]
        public string VoucherNo { get; set; }

        [StringLength(200)]
        public string VoucherDesc { get; set; }

        [StringLength(3)]
        public string VoucherType { get; set; }

        [StringLength(125)]
        public string Reference { get; set; }

        public bool? IsPosted { get; set; }
        
        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
    }
}
