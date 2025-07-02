namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SurveyMemberAccomodationInformation")]
    public partial class SurveyMemberAccomodationInformation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SMAccomodationId { get; set; }

        public long SurveyId { get; set; }

        public bool IsOwnHome { get; set; }

        [Required]
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

        public bool? IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
