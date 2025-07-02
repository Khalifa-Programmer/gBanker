using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("inv.Inv_Items")]
    public class Inv_Items
    {
        [Key]
        public int ItemID { get; set; }
        public int? CategoryID { get; set; }
        public int? SubCatagoryID { get; set; }
        public string ItemName { get; set; }
        public string ItemNameInBangle { get; set; }
        public string ItemShortName { get; set; }
        public string ItemCode { get; set; }
        public string Unit { get; set; }
        public string ItemDetails { get; set; }
        public bool? IsActive { get; set; }
        public int MinStockLevel { get; set; }
        public int MaxStockLevel { get; set; }
        public int ReOrderLevel { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}