using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("CumMisItem")]
    public partial class CumMisItem
    {
        [Key]
        [Column(Order = 0)]
        public int CumMisItemID { get; set; }

        [StringLength(100)]
        public string CumMisItemName { get; set; }

        public int? ItemType { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(35)]
        public string CreateUser { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
    }
}
