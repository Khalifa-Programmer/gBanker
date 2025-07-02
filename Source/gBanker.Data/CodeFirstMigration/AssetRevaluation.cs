using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("fix.AssetRevaluation")]
    public class AssetRevaluation
    {
        [Key]
        public int AssetRevaluationID { get; set; }
        public int AssetGroupID { get; set; }
        public long AssetID { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal CurrAssetCost { get; set; }
        public int Valuer { get; set; }
        public decimal RevaluatedValue { get; set; }
        public decimal DeprRate { get; set; }
        public string Remarks { get; set; }
        public int OrgID { get; set; }
        public int OfficeID { get; set; }
        public bool IsActive { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }            
    }
}
