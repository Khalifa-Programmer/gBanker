namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProMiniBalanSet")]
    public partial class ProMiniBalanSet
    {
        [Key]
        public long MinimumBalanceID { get; set; }
       
        public Int16 ProductId { get; set; }

        public decimal MinimumBalance { get; set; }

        public DateTime EffectiveDateFrom { get; set; }

        public DateTime EffectiveDateTo { get; set; }

        [StringLength(50)]
        public string CreateUser { get; set; }
       
        public DateTime CreateDate { get; set; }
      
        [StringLength(50)]
        public string UpdateUser { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool? IsDefault { get; set; }
        public bool IsActive { get; set; }


    }
}
