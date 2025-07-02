using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
    public class InvStoreViewModel
    {
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
        public int? UpdaeBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? VendorID { get; set; }

        public string EmployeeCode { get; set; }
        public string StockDate { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }

    }
}
