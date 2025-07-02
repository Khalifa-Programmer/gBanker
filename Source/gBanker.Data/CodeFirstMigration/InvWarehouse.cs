using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace gBanker.Data.CodeFirstMigration
{
    [Table("inv.Inv_Warehouse")]
   public class InvWarehouse
   {
        [Key]
        public int WarehouseID { get; set; }
        public int OfficeID { get; set; }
        public int DepartmentID { get; set; }
        public string WarehouseName { get; set; }
        public bool IsActive { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
   }
}
