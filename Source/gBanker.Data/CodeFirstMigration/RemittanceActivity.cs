using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("RemittanceActivity")]
    public class RemittanceActivity
    {
        [Key]
        public int RemittanceActivityId { get; set; }
        public int? OrgId { get; set; }
        public int? OfficeID { get; set; }
        public int? NoOfClient { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal? RemittedAmount { get; set; }
        public decimal? Commission { get; set; }
        public string LinkedBank { get; set; }
        public string Remark { get; set; }
        public bool? IsActive { get; set; }
        public int? CreateUser { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
