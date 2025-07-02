using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("ArchiveDbMapping")]
    public class ArchiveDbMapping
    {        
        [Key]
        public int Id { get; set; }

        [Display(Name = "Organization")]
        [Required(ErrorMessage = "{0} is Required")]
        public int OrgId { get; set; }

        [Display(Name = "Db Name")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string DbName { get; set; }

        [Display(Name = "Archive Db Name")]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        public string ArchiveDbName { get; set; }

    }
}
