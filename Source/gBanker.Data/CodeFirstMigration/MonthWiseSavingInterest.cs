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
    [Table("MonthWiseSavingInterest")]
    public partial class MonthWiseSavingInterest
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long MonthWiseSavingInterestID { get; set; }

        public int? OfficeID { get; set; }

        public int? CenterID { get; set; }

        public long? MemberID { get; set; }

        public short? ProductID { get; set; }

        public int? NoOfAccount { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "numeric")]
        public decimal Balance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Interest { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? InterestRate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TransactionDate { get; set; }

        [StringLength(50)]
        public string ProcessType { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrgID { get; set; }

        public virtual Organization Organization { get; set; }
    }
}
