namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AccLastVoucher")]
    public partial class AccLastVoucher
    {
        [Key]
        public int LastVoucherID { get; set; }

        public int OfficeID { get; set; }

        [Required]
        [StringLength(50)]
        public string VoucherNo { get; set; }
    }
}
