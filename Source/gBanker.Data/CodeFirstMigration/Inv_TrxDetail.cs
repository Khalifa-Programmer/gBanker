namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("inv.Inv_TrxDetail")]
    public partial class Inv_TrxDetail
    {
        [Key]
        public long TrxDetailsID { get; set; }

        public long TrxMasterID { get; set; }

        public int? AccID { get; set; }

        public decimal? Credit { get; set; }

        public decimal? Debit { get; set; }

        [StringLength(200)]
        public string Narration { get; set; }

        public bool? IsActive { get; set; }


        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
    }
}
