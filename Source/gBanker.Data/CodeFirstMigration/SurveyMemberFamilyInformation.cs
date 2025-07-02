namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SurveyMemberFamilyInformation")]
    public partial class SurveyMemberFamilyInformation
    {
        [Key]
        public long SurveyFamilyId { get; set; }

        public long SurveyId { get; set; }

        public int RelationshipId { get; set; }

        public int OccupationId { get; set; }

        public int NoOfPerson { get; set; }

        public bool IsEarningCapable { get; set; }

        public decimal IncomeMonthly { get; set; }

        public bool IsAnyOtherPrivateBusiness { get; set; }

        public decimal IncomeMonthlyFromPrivateBusiness { get; set; }

        [Required]
        [StringLength(500)]
        public string Remarks { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [StringLength(15)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
        public DateTime? InActiveDate { get; set; }

        //[Required]
        //[StringLength(50)]
        //public string UpdateUser { get; set; }

        //public DateTime UpdateDate { get; set; }
    }
}
