using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("fix.FixAssetUpdates")]
    public class FixAssetUpdates
    {
        [Key]
        public int FixAssetUpdateID { get; set; }
        public DateTime TransactionDate { get; set; }
        public int AssetGroupID { get; set; }
        public long AssetID { get; set; }        
        public string AssetSerial { get; set; }
        public string VoucherNo { get; set; }
        public decimal UnitPrice { get; set; }        
        public decimal AccumulatedDepri { get; set; }
        public decimal CurrentDepri { get; set; }
        public decimal? BookValue { get; set; }
        public long AssetClientId { get; set; }
        public string TransactionType { get; set; }
        public string AssetUser { get; set; }
        public bool Usable { get; set; }
        public int? OrgID { get; set; }
        public int OfficeID { get; set; }
        //public DateTime DepCalcDate { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? OperationDate { get; set; }
        public decimal? InstallationCost { get; set; }
        public decimal? CarringCost { get; set; }
        public decimal? OtherCost { get; set; }
        public decimal? TotalCost { get; set; }
        public decimal? TotalOpeningBalanceCost { get; set; }
        public int? ProjectID { get; set; }
        public decimal? DepriciationRate { get; set; }
        public int? DepriciationMethod { get; set; }
        public bool? IsCapitalizedAsset { get; set; }
        public decimal? OpeningDepriciationBalance { get; set; }
        public decimal? OpeningBookValue { get; set; }        
        public decimal? InsuranceValue { get; set; }
        public DateTime? InsuranceExpDate { get; set; }
        public string WarrantyGurantee { get; set; }
        public int? UsefulLifeYear { get; set; }
        public string PurchaseOrderNo { get; set; }
        public DateTime? PurchaseOrderDate { get; set; }
        public bool? IsActive { get; set; }        
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

        public DateTime? DepriciationClosingDate { get; set; }
    }
}
