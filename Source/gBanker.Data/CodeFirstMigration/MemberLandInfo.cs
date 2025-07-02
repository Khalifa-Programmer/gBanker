namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MemberLandInfo")]
    public partial class MemberLandInfo
    {
        [Key]
        public long MemberLandID { get; set; }

        public int OrgID { get; set; }

        public int OfficeID { get; set; }

        public int CenterID { get; set; }

        public long MemberID { get; set; }

        public bool? LandVita { get; set; }

        public bool? LandKrishie { get; set; }

        public bool? LandBagan { get; set; }

        public bool? LandPukur { get; set; }

        public bool? LandOnabade { get; set; }

        public bool? LandBondhokDeya { get; set; }

        public bool? LandBorgaDeya { get; set; }

        public bool? LandBondhokNeya { get; set; }

        public bool? LandBorgaNeya { get; set; }

        public bool? KhashLand { get; set; }

        [StringLength(8)]
        public string BorgaCondition { get; set; }

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
