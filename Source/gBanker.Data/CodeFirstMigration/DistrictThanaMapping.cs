namespace gBanker.Data.CodeFirstMigration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("pksf.DistrictThanaMapping")]
    public partial class DistrictThanaMapping
    {
        [Display(Name = "Id")]
        [Required(ErrorMessage = "{0} is Required")]
        public int Id { get; set; }

        [Display(Name = "District Code")]
        [StringLength(5, ErrorMessage = "Maximum length is {1}")]
        public string DistrictCode { get; set; }

        [Display(Name = "Olrs District Code")]
        [StringLength(5, ErrorMessage = "Maximum length is {1}")]
        public string OlrsDistrictCode { get; set; }

        [Display(Name = "Thana Code")]
        [StringLength(5, ErrorMessage = "Maximum length is {1}")]
        public string ThanaCode { get; set; }

        [Display(Name = "Olrs Thana Code")]
        [StringLength(5, ErrorMessage = "Maximum length is {1}")]
        public string OlrsThanaCode { get; set; }

        [Display(Name = "Created By")]
        public int? CreatedBy { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }
    }
}
