using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class MobileSyncLogDetail
    {
        public long ID { get; set; }

        public long MobileSyncLogID { get; set; }

        [Required]
        public string RequestParam { get; set; }

        public string ErrorLog { get; set; }

        [StringLength(50)]
        public string UserID { get; set; }

        [StringLength(2)]
        public string Status { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual MobileSyncLog MobileSyncLog { get; set; }
    }
}
