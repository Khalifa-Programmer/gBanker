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
    [Table("AccountingInterface")]
    public partial class AccountingInterface
    {
        [Key]
        [Column(Order = 0)]
        public int AccountingInterfaceID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string AccCode { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(2)]
        public string voucher_category { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(2)]
        public string voucher_type { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(2)]
        public string trx_type { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(2)]
        public string trx_ind { get; set; }

        [StringLength(2)]
        public string office_type { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrgID { get; set; }

        public virtual Organization Organization { get; set; }
    }
}
