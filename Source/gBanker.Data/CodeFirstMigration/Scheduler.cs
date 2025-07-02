using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class Scheduler
    {
        public Scheduler()
        {
            SchedulerDetails = new HashSet<SchedulerDetail>();
        }

        public long SchedulerID { get; set; }

        [Required]
        [StringLength(100)]
        public string SchedulerName { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? LastRun { get; set; }

        public int? RunEvery { get; set; }

        [StringLength(5)]
        public string Frequency { get; set; }

        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        public virtual ICollection<SchedulerDetail> SchedulerDetails { get; set; }
    }
}
