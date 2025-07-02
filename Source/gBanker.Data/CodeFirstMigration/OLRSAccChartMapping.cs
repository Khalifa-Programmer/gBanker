namespace gBanker.Data.CodeFirstMigration
{
    using gBankerCodeFirstMigration.Db;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("pksf.OLRSAccChartMapping")]
    public partial class OLRSAccChartMapping
    {
        [Key]
        public int Id { get; set; }

        public int POId { get; set; }

        public string POCode { get; set; }

        [Display(Name = "Acc Code")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string AccCode { get; set; }

        [Display(Name = "Acc Code O L R S")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string AccCodeOLRS { get; set; }

        [Display(Name = "l1_code")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(6, ErrorMessage = "Maximum length is {1}")]
        public string l1_code { get; set; }

        [Display(Name = "l2_code")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(6, ErrorMessage = "Maximum length is {1}")]
        public string l2_code { get; set; }

        [Display(Name = "l3_code")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(6, ErrorMessage = "Maximum length is {1}")]
        public string l3_code { get; set; }

        [Display(Name = "l4_code")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(6, ErrorMessage = "Maximum length is {1}")]
        public string l4_code { get; set; }

        [Display(Name = "l5_code")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(10, ErrorMessage = "Maximum length is {1}")]
        public string l5_code { get; set; }



        [Display(Name = "Is Active")]
        [Required(ErrorMessage = "{0} is Required")]
        public bool IsActive { get; set; }

        [Display(Name = "Create User")]
        public int? CreateUser { get; set; }

        [Display(Name = "Create Date")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "Update User")]
        public int? UpdateUser { get; set; }

        [Display(Name = "Update Date")]
        public DateTime? UpdateDate { get; set; }
    }
}

