using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("inv.Inv_RequsitionMaster")]
    public class Inv_RequsitionMaster
    {
        [Key]
        public Int64 RequsitionID { get; set; }
        public string RequsitionNo { get; set; }
        public int FromStoreID { get; set; }
        public int ToStoreID { get; set; }
        public DateTime RequsitionDate { get; set; }
        public Int64 ForwardRequsitionID { get; set; }
        public string RequsitionStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsActive { get; set; }

        //public string FromStoreName { get; set; }
        //public string ToStoreName { get; set; }
    }
    public class Inv_RequsitionMasterViewModel
    {
        public Int64 RequsitionID { get; set; }
        public string RequsitionNo { get; set; }
        public int FromStoreID { get; set; }
        public int ToStoreID { get; set; }
        public DateTime RequsitionDate { get; set; }
        public Int64 ForwardRequsitionID { get; set; }
        public string RequsitionStatus { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsActive { get; set; }
        public string FromStoreName { get; set; }
        public string ToStoreName { get; set; }
    }
    public class Inv_RequisitionWiseTotalQtyInItemViewModel
    {
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int RequestQty { get; set; }
        public int SendingQty { get; set; }
        public int ApprovedQty { get; set; }
        public string AprovedStatus { get; set; }
        public int MinStockLevel { get; set; }
        public int StockBalance { get; set; }
        public string RequsitionNo { get; set; }
        public string RDetailsID { get; set; }
    }

    
}
