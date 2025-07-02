namespace gBanker.Data.CodeFirstMigration.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("pksf.Indicator")]
    public partial class Indicator
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Indicator Code")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string IndicatorCode { get; set; }

        [Display(Name = "Indicator Name")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(500, ErrorMessage = "Maximum length is {1}")]
        public string IndicatorName { get; set; }

        [Display(Name = "Associated Table")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        public string AssociatedTable { get; set; }

        [Display(Name = "AssociatedAccCodeFD")]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        public string AssociatedAccCodeFD { get; set; }

        public bool? IsManual { get; set; }

        [Display(Name = "Create User")]
        public int? CreateUser { get; set; }

        [Display(Name = "Create Date")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "Update User")]
        public int? UpdateUser { get; set; }

        [Display(Name = "Update Date")]
        public DateTime? UpdateDate { get; set; }

        [Display(Name = "Is Active")]
        [Required(ErrorMessage = "{0} is Required")]
        public bool IsActive { get; set; }

    }
}
