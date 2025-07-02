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
    [Table("ExpireInfo")]
    public partial class ExpireInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ExpireInfoID { get; set; }

        public int OfficeID { get; set; }

        public int CenterID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MemberID { get; set; }
        [StringLength(50)]
        public string Relation { get; set; }
        [StringLength(50)]
        public string ExpiryName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpireDate { get; set; }

        [StringLength(50)]
        public string Remarks { get; set; }

        public int OrgID { get; set; }

        public bool? IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }

        public decimal PrincipleBalance { get; set; }

        public decimal ServiceCharge { get; set; }

        public DateTime? LoanCloseDate { get; set; }



        [Required]
        [StringLength(15)]
        public string CreateUser { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime CreateDate { get; set; }
        public int? ExpireStatus { get; set; }
        public long? LoanSummaryID { get; set; }
        public virtual Center Center { get; set; }

        public virtual Member Member { get; set; }

        public virtual Office Office { get; set; }

        public virtual Organization Organization { get; set; }
        public string ApplicantName { get; set; }
        public string MobileNumber { get; set; }
    }
}
