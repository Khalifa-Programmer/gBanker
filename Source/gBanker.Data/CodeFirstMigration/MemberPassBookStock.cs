using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("MemberPassBookStock")]
    public partial class MemberPassBookStock
    {
        public long MemberPassBookStockID { get; set; }

        public int? OfficeID { get; set; }

        public long? LotNo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Qty { get; set; }

        public long? StartingNo { get; set; }

        public long? LastIssue { get; set; }

        public int OrgID { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
    }
}
