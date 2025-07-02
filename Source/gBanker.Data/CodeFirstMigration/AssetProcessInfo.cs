using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("fix.AssetProcessInfo")]
    public class AssetProcessInfo
    {
        [Key]
        public int ProcessID { get; set; }
        public DateTime DeprDate { get; set; }
        public bool ProcessYN { get; set; }
        public int OrgID { get; set; }
        public int OfficeID { get; set; }
        public int CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
