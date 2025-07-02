using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("TargetAchievement")]
    public class TargetAchievement
    {
        [Key]
        public int TargetId { get; set; }
        public int? ParticularId { get; set; }
        public decimal? Balance { get; set; }
        public decimal? TargetCurrentYear { get; set; }
        public decimal? Target { get; set; }
        public decimal? Achievement { get; set; }
        public DateTime? Date { get; set; }
        public int? ProductID { get; set; }
        public bool? IsActive { get; set; }
        public int? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public int OfficeID { get; set; }
    }
}
