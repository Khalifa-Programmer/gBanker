namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SurveyMemberExpenditure")]
    public partial class SurveyMemberExpenditure
    {
        [Key]
        public long SurveyExpenditureId { get; set; }

        public long SurveyId { get; set; }

        public int ExpenditureId { get; set; }

        public decimal ExpendetureAmount { get; set; }

        [Required]
        [StringLength(500)]
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
