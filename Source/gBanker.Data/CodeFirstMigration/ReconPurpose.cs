using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("ReconPurpose")]
    public partial class ReconPurpose
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string ReconPurposeCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string ReconPurposeName { get; set; }
    }
}
