namespace gBanker.Data.CodeFirstMigration.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Area")]
    public partial class Area
    {
        public int AreaID { get; set; }

        [StringLength(10)]
        public string AreaCode { get; set; }

        [Required]
        [StringLength(50)]
        public string AreaName { get; set; }

        public int ZoneCode { get; set; }

        [Required]
        [StringLength(50)]
        public string ZoneName { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
    }
}
