namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MemberAssetInfo")]
    public partial class MemberAssetInfo
    {
        [Key]
        public long MemberAssetID { get; set; }

        public int OrgID { get; set; }

        public int OfficeID { get; set; }

        public int CenterID { get; set; }

        public long MemberID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Cow { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Goat { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Sheep { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Duck { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Hen { get; set; }

        [StringLength(20)]
        public string Others { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Bed { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Chair { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AssetTable { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Cycle { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Radio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Ornament { get; set; }

        [StringLength(20)]
        public string OtherAsset { get; set; }

        public decimal? AgriculturalLandAmount { get; set; }
        public decimal? PondLandAmount { get; set; }
        public decimal? AbandonedLandAmount { get; set; }

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
