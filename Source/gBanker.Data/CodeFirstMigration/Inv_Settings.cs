namespace gBanker.Data.CodeFirstMigration
{
    using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("inv.Inv_Settings")]
    public partial class Inv_Settings
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Purpose { get; set; }
        [StringLength(50)]
        public string PurposeKey { get; set; }
        public int? PurposeValue { get; set; }
        public bool IsActive { get; set; }
    }
}
