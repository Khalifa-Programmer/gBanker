namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SurveyKnownMember")]
    public partial class SurveyKnownMember
    {
        [Key]
        public long KnownMemberId { get; set; }

        public long SurveryId { get; set; }

        [Required]
        [StringLength(20)]
        public string MemberCode { get; set; }

        public long IsBloodRelated { get; set; }

        [Required]
        [StringLength(50)]
        public string Remarks { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(15)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
    }
}
