using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("inv.Inv_StoreItem")]
    public class InvStoreItem
    {
        [Key]
        public int StoreItemID { get; set; }
        public int WarehouseID { get; set; }
        public int ItemID { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
