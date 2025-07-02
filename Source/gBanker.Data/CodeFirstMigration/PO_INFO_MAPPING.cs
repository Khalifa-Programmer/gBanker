namespace gBanker.Data.CodeFirstMigration
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("pksf.PO_INFO_MAPPING")]
    public partial class PO_INFO_MAPPING
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "MFI  PO CODE")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(10, ErrorMessage = "Maximum length is {1}")]
        public string MFI_PO_CODE { get; set; }

        [Display(Name = "PKSF PO CODE")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(10, ErrorMessage = "Maximum length is {1}")]
        public string PKSF_PO_CODE { get; set; }

        [Display(Name = "PO NAME")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(150, ErrorMessage = "Maximum length is {1}")]
        public string PO_NAME { get; set; }
    }
}
