namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class View_SurveyMemberOperationwithOtherNGOInformation
    {
        public int? RowSl { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SMNGOId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SurveyId { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NGOId { get; set; }

        [Key]
        [Column(Order = 3)]
        public decimal LoanAmount { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(500)]
        public string Remarks { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(15)]
        public string CreateUser { get; set; }

        [Key]
        [Column(Order = 7, TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(100)]
        public string SurveyCode { get; set; }
    }
}
