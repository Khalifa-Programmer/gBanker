namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MemberOtherInfo")]
    public partial class MemberOtherInfo
    {
        [Key]
        public long MemberOtherInfoID { get; set; }

        public int OrgID { get; set; }

        public int OfficeID { get; set; }

        public int CenterID { get; set; }

        public long MemberID { get; set; }

        public bool? Tubewel { get; set; }
        public bool? Nodi { get; set; }
        public bool? Khal { get; set; }
        public bool? Pukur { get; set; }
        public bool? Filter { get; set; }
        public bool? PSF { get; set; }
        public bool? BristyrPani { get; set; }
        public bool? Saplai { get; set; }
        public bool? Others { get; set; }
        public bool? Lattin { get; set; }

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
