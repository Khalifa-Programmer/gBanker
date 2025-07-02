namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Division")]
    public partial class Division
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte DivisionID { get; set; }
        public int? CountryId { get; set; }

        [StringLength(50)]
        public string DivisionCode { get; set; }

        [StringLength(150)]
        public string DivisionName { get; set; }

        [StringLength(200)]
        public string DivisionAddress { get; set; }

        [StringLength(50)]
        public string ContactNo { get; set; }

        public int? OfficeID { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }

        [StringLength(50)]
        public string ZoneCode { get; set; }

        [StringLength(50)]
        public string AreaCode { get; set; }

        [StringLength(50)]
        public string HOCode { get; set; }
    }
}
