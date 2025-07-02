namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StatisticsReportDetails")]
    public partial class StatisticsReportDetails
    {
        [Key]
        public long StatisticsReportDetailsID { get; set; }

        public int? StatisticsReportId { get; set; }

        public long? StatisticsDescriptionID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AmountM { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AmountF { get; set; }

        public DateTime? StatisticsReportDetailsDate { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }

        [StringLength(50)]
        public string UpdateUser { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int? OfficeID { get; set; }

    }
}
