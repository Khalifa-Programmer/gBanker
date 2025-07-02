namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class View_SurveyMemberFamilyInformation
    {
        public int? RowSl { get; set; }

        [Key]
        [Column(Order = 0)]
        public long SurveyFamilyId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SurveyId { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RelationshipId { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OccupationId { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NoOfPerson { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool IsEarningCapable { get; set; }

        [Key]
        [Column(Order = 6)]
        public decimal IncomeMonthly { get; set; }

        [Key]
        [Column(Order = 7)]
        public bool IsAnyOtherPrivateBusiness { get; set; }

        [Key]
        [Column(Order = 8)]
        public decimal IncomeMonthlyFromPrivateBusiness { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(500)]
        public string Remarks { get; set; }
        public string SurveyCode { get; set; }

        [Key]
        [Column(Order = 10)]
        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(15)]
        public string CreateUser { get; set; }

        [Key]
        [Column(Order = 12, TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
    }
}
