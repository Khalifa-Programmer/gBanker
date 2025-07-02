using gBanker.Data.CodeFirstMigration.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("WorkingAreaLog")]
    public partial class WorkingAreaLog
    {
      
        public long WorkingAreaLogID { get; set; }

        public int? OfficeID { get; set; }

        [StringLength(10)]
        public string MainProductCode { get; set; }

        [StringLength(200)]
        public string WorkingArea { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Upzilla { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Municipality { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Village { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SelfEnterprenuerMale { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SelfEnterprenuerFeMale { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PaidEnterPrenuerOwnFamilyMale { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PaidEnterPrenuerOwnFamilyFeMale { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PaidEnterPrenuerOutSideMale { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PaidEnterPrenuerOutSideFeMale { get; set; }
        public DateTime? EntryDate { get; set; }
        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
        public virtual Office Office { get; set; }
    }
}
