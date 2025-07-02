using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("inv.Inv_RequisitionConsulateDetails")]
    public class Inv_RequisitionConsulateDetails
    {
        [Key]
        public Int64 ConsulateDetailID { get; set; }
        public int ConsulateRequisitionMasterID { get; set; }
        public int ItemID { get; set; }
        public int Qty { get; set; }
        public int SendingQty { get; set; }
        public int ApprovedQty { get; set; }
        public string AprovedStatus { get; set; }
        public int ReConsulateRequisitionID { get; set; }
        public int? StatusChangeBy { get; set; }
        public DateTime? StatusChangeDate { get; set; }
    }
    public class RequisitionConsulateDetailsViewModel
    {
        public string DetailsIDs { get; set; }
        public int ItemID { get; set; }
        public int Qty { get; set; }
        public int SendingQty { get; set; }
        public int ApprovedQty { get; set; }
        public string Remarks { get; set; }
    }
}
