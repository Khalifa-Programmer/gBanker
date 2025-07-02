using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("inv.Inv_Temp_Store")]
    public class Inv_TempStore
    {
        [Key]
        public long ID { get; set; }
        public int StoreID { get; set; }
        public long ConsulateRequisitionID { get; set; }
        public long RequisitionID { get; set; }
        //public int StoreOutDate { get; set; }
        public int ItemID { get; set; }
        public long TrxMasterID { get; set; }
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        
    }
}
