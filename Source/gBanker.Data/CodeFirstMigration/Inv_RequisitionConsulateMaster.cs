using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("inv.Inv_RequisitionConsulateMaster")]
    public class Inv_RequisitionConsulateMaster
    {
        [Key]
        public int ConsulateRequisitionID { get; set; }
        public string RequisitionNo { get; set; }
        public int SenderStoreID { get; set; }
        public int SenderBy { get; set; }
        public DateTime SenderDate { get; set; }
        public int ReceiverStoreID { get; set; }
        //public string ReceiverStoreStatus { get; set; }
        //public int ReceiverStatusChangeBy { get; set; }
       // public DateTime ReceiverStatusChangeDate { get; set; }
        //public DateTime ForwardDate { get; set; }
        //public int ForwardOfficeID { get; set; }
        //public string ForwardOfficeStatus { get; set; }
        //public int ForwardStatusChangeBy { get; set; }
        //public DateTime ForwardStatusChangeDate { get; set; }
    }
    public class InvRequisitionConsulateViewModel
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
        public int ConsulateRequisitionID { get; set; }
        public long ConsulateDetailID { get; set; }
        public string RequsitionNo { get; set; }
        public string RDetailsID { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class Inv_ConsulateRequisitionDetailsViewModel
    {
        public string RequsitionNo { get; set; }
        public string WarehouseName { get; set; }
        public string OfficeName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int RequestQty { get; set; }
        public int SendingQty { get; set; }
        public int ApprovedQty { get; set; }
        public long ConsulateDetailID { get; set; }
        public long RequsitionDetailsID { get; set; }
        public int ConsulateRequisitionID { get; set; }
    }

    public class ConsulateRequisitionMasterApproveViewModel
    {
        public string RequsitionNo { get; set; }
        public int ApproveStoreID { get; set; }
        //public int RequestStoreID { get; set; }
        public DateTime ApproveDate { get; set; }
    }
    public class ConsulateRequisitionDetailsApproveViewModel
    {
        public int? ItemID { get; set; }
        public int? ConsulateDetailID { get; set; }
        public int? ApproveQty { get; set; }
        public int? ModifyQty { get; set; }
        public int? StockBalance { get; set; }
        public int? MinStockLevel { get; set; }
    }
    public class ConsulateRequsitionAfterApprovedViewModel
    {
        public int ConsulateRequisitionID { get; set; }
        public string RequisitionNo { get; set; }
        public DateTime? StatusChangeDate { get; set; }
        public string WarehouseName { get; set; }
        public string OfficeName { get; set; }
        public string AprovedStatus { get; set; }
    }
}
