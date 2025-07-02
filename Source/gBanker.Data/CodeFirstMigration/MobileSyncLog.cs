using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class MobileSyncLog
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MobileSyncLog()
        {
            MobileSyncLogDetails = new HashSet<MobileSyncLogDetail>();
        }

        public long ID { get; set; }

        public short SyncType { get; set; }

        [Required]
        [StringLength(50)]
        public string UserID { get; set; }

        public string RequestDetail { get; set; }

        public int InputRecordCount { get; set; }

        public int OutputRecordCount { get; set; }

        public int PassedRecordCount { get; set; }

        public int FailedRecordCount { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string ErrorLog { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreateDate { get; set; }

        [Required]
        [StringLength(2)]
        public string Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MobileSyncLogDetail> MobileSyncLogDetails { get; set; }
    }
}
