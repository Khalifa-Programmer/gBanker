using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("fix.DepriciationRateChange")]
    public class DepriciationRateChange
    {
        [Key]
        public int DepRateChangeID { get; set; }
        public int AssetGroupID { get; set; }
        public long AssetID { get; set; }
        public decimal CurrDepRate { get; set; }        
        public decimal NewDepRate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Remarks { get; set; }
        public int OrgID { get; set; }
        public int OfficeID { get; set; }
        public bool IsActive { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
