using gBankerCodeFirstMigration.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
     [Table("SchedulerDetail")]
    public partial class SchedulerDetail
    {
        [Key]
        [Column(Order = 0)]
        public long SchedulerRunDetailID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SchedulerID { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string ErrorDescription { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(35)]
        public string CreateUser { get; set; }
        public int OrgID { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Scheduler Scheduler { get; set; }
    }
}
