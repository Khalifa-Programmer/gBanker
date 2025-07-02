using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("fix.DailyTransaction")]
    public class DailyTransaction
    {
        [Key]
        public long DailyTransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public int AssetGroupID { get; set; }
        public long AssetID { get; set; }
        public string AssetSerial { get; set; }
        public string AssetDescription { get; set; }
        public decimal PurchasePrice { get; set; }            
        public long AssetClientId { get; set; }
        public int TransactionType { get; set; }
        public string AssetUser { get; set; }
        public bool Usable { get; set; }
        public int? OrgID { get; set; }
        public int OfficeID { get; set; }
        public DateTime DepCalcDate { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? OperationDate { get; set; }
        public decimal? InstallationCost { get; set; }
        public decimal? CarringCost { get; set; }
        public decimal? OtherCost { get; set; }
        public decimal? TotalCost { get; set; }        
        public decimal? DepriciationRate { get; set; }
        public int? DepriciationMethod { get; set; }
        public int? ProjectID { get; set; }
        public bool? IsCapitalizedAsset { get; set; }
        public bool? IsInstallmentAsset { get; set; }        
        public decimal? InsuranceValue { get; set; }
        public DateTime? InsuranceExpDate { get; set; }
        public string WarrantyGurantee { get; set; }
        public int? UsefulLifeYear { get; set; }
        public string PurchaseOrderNo { get; set; }
        public DateTime? PurchaseOrderDate { get; set; }
        public string Remarks { get; set; }
        public bool? IsActive { get; set; }
        public int AssetStatus { get; set; }
        public DateTime? StatusDate { get; set; }        
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public decimal? DownPayment { get; set; }
        public int? InstallmentNumber { get; set; }
        public decimal? InstallmentAmount { get; set; }
        public string BankName { get; set; }
        public string AccCardNo { get; set; }
        public int selectedOfficeId { get; set; }
    }
}
