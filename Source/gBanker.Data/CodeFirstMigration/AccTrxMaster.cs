namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AccTrxMaster")]
    public partial class AccTrxMaster
    {
        public AccTrxMaster()
        {
            AccTrxDetails = new HashSet<AccTrxDetail>();
        }

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
        public bool? IsYearlyClosing { get; set; }
        public bool? IsAutoVoucher { get; set; }
        public bool? IsRectify { get; set; }
        public int? OrgID { get; set; }
        public bool? IsReconcileVoucher { get; set; }
        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }

        public virtual ICollection<AccTrxDetail> AccTrxDetails { get; set; }
    }
}
