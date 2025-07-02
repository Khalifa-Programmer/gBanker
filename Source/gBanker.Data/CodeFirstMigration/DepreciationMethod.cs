using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("fix.DepreciationMethod")]
    public partial class DepreciationMethod
    {
        [Key]
        public int Id { get; set; }
        public string DepriciationName { get; set; }
        public bool IsActive { get; set; }
    }
}
