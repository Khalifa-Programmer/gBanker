using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("inv.Inv_Store")]
    public class Inv_Store
    {
        [Key]
        public Int64 ID { get; set; }
        public int WarehouseID { get; set; }
        public int ItemID { get; set; }
        public string ChallanNo { get; set; }
        public string StoreNo { get; set; }
        public long RequisitionID { get; set; }
        public long TrxMasterID { get; set; }
        public string RequestPage { get; set; }
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsActive { get; set; }
        public DateTime StockInOrOutDate { get; set; }
        public string StockType { get; set; }
        public int EmployeeID { get; set; }
        
        public string Remarks { get; set; }
        public int StockBalance { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? VendorID { get; set; }
        public Int64? RefStoreID { get; set; }
        
        [NotMapped]
        public string EmployeeCode { get; set; }
        [NotMapped]
        public string StockDate { get; set; }
        [NotMapped]
        public int? ExistsQty { get; set; }
        [NotMapped]
        public decimal? ExistsPrice { get; set; }

    }
}
