using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("fix.ReportTypeMapping")]
    public partial class ReportTypeMapping
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Required")]
        public int ReportTypeId { get; set; }
        [Required(ErrorMessage = "Required")]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Required")]
        public bool IsActive { get; set; }

    }
}
