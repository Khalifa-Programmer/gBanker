using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("fix.AssetOut")]
    public class AssetOut
    {
        [Key]
        public long AssetOutID { get; set; }
        public long DailyTransactionId { get; set; }
        public DateTime? OutDate { get; set; }
        public int TranType { get; set; }
        public int AssetGroupID { get; set; }
        public long AssetID { get; set; }
        public string AssetSerial { get; set; }        
        public decimal SellingPrice { get; set; }        
        public decimal? AssetCost { get; set; }
        public decimal? AccumulatedDepriciation { get; set; }
        public decimal? CurrentDepriciation { get; set; }    
        public decimal? BookValue { get; set; }
        public decimal? TotalProfitLoss { get; set; }
        public decimal? CapitalGain { get; set; }
        public decimal? BusinessGain { get; set; }
        public string Remarks { get; set; }
        public int OrgID { get; set; }
        public int OfficeID { get; set; }        
        public bool? IsActive { get; set; }        
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
