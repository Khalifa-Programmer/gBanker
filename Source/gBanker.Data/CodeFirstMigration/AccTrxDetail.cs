namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AccTrxDetail")]
    public partial class AccTrxDetail
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

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }

        public virtual AccChart AccChart { get; set; }

        public virtual AccTrxMaster AccTrxMaster { get; set; }
    }
}
