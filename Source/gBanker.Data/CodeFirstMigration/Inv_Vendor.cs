using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("inv.Inv_Vendor")]
    public class Inv_Vendor
    {
        [Key]
        public int VendorID { get; set; }
        public int WharehouseID { get; set; }
        public string VendorName { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public bool? IsActive { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    public class VendorViewModel
    {
        public int VendorID { get; set; }
        public string VendorName { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string WarehouseName { get; set; }
    }
}
