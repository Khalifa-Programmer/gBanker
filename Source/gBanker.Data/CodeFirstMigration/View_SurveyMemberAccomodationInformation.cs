namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class View_SurveyMemberAccomodationInformation
    {
        public int? RowSl { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SMAccomodationId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SurveyId { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool IsOwnHome { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(500)]
        public string ResidenceAddress { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ResideFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ResideTo { get; set; }

        [StringLength(100)]
        public string HomeOwnerName { get; set; }

        public int? HomeOwnerOccupationId { get; set; }

        public int? IsRentPaymentRegular { get; set; }

        public int? IsUseRentalMemberForLoanPurpose { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(15)]
        public string CreateUser { get; set; }
        public string ResideFromString { get; set; }
        public string ResideToString { get; set; }

        [Key]
        [Column(Order = 6, TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(100)]
        public string SurveyCode { get; set; }
    }
}
