using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("fix.AssetPartialOut")]
    public class AssetPartialOut
    {
        [Key]
        public int AssetPartialOutID { get; set; }
        public int AssetGroupID { get; set; }
        public long AssetID { get; set; }
        public string AssetSerial { get; set; }
        public decimal? DepriciationRate { get; set; }
        public decimal CurrAssetCost { get; set; }
        public decimal DisposalAmount { get; set; }        
        public decimal CurrCostAfterDisposal { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal PreviousBookValue { get; set; }
        public decimal AccuDeprWholeAsset { get; set; }
        public decimal AccuDeprDisposedAsset { get; set; }
        public decimal NewBookValue { get; set; }
        public int OrgID { get; set; }
        public int OfficeID { get; set; }
        public bool IsActive { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public long DailyTransactionId { get; set; }

    }
}
