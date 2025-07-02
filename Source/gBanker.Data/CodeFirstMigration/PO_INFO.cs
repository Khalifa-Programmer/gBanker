using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("pksf.PO_INFO")]
    public partial class PO_INFO
    {
        [Key]
        [Display(Name = "po_code")]
        [Required(ErrorMessage = "{0} is Required")]
        public string po_code { get; set; }

        [Display(Name = "po name")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(300, ErrorMessage = "Maximum length is {1}")]
        public string po_name { get; set; }
    }
}
