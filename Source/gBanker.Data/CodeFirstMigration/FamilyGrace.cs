
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("FamilyGrace")]
    public partial class FamilyGrace
    {
        public int FamilyGraceID { get; set; }

        public int? OrgID { get; set; }

        public int? OfficeID { get; set; }

        public int? CenterID { get; set; }

        public long? MemberID { get; set; }
        [Column(TypeName = "date")]
        public DateTime? GraceStartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? GraceEndDate { get; set; }

        [StringLength(50)]
        public string Description { get; set; }
        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }

        [Required]
        [StringLength(35)]
        public string CreateUser { get; set; }

    
    }
}
