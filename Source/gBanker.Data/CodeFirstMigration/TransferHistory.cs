using gBanker.Data.CodeFirstMigration.Db;
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
    [Table("TransferHistory")]
    public partial class TransferHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long TransferHistoryID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OfficeID { get; set; }

        public int? TrOfficeID { get; set; }

        public int? CenterID { get; set; }

        public int? TrCenterID { get; set; }

        public int? MemberID { get; set; }

        public int? TrMemberID { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TransferDate { get; set; }

        public int? MemberCategoryId { get; set; }

        public int? TrMemberCategoryID { get; set; }

        public int? ProductID { get; set; }

        public int? TrProductID { get; set; }

        [StringLength(2)]
        public string Status { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Charge { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Principal { get; set; }
        public int OrgID { get; set; }
        public virtual Organization Organization { get; set; }
       
    }
}
