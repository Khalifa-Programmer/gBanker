namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LgVillage")]
    public partial class LgVillage
    {
        public long LgVillageID { get; set; }

        [StringLength(255)]
        public string VillageCode { get; set; }

        [StringLength(255)]
        public string VillageName { get; set; }

        [StringLength(255)]
        public string UnionCode { get; set; }

        [StringLength(255)]
        public string UnionName { get; set; }

        [StringLength(255)]
        public string UpozillaCode { get; set; }

        [StringLength(255)]
        public string UpozillaName { get; set; }

        [StringLength(255)]
        public string DistrictCode { get; set; }

        [StringLength(255)]
        public string DistrictName { get; set; }

        [StringLength(255)]
        public string DivisionCode { get; set; }

        [StringLength(255)]
        public string DivisionName { get; set; }

        public int? CountryID { get; set; }
        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(15)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
    }
}
