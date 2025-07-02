using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    [Table("inv.Inv_RequsitionDetails")]
    public class Inv_RequsitionDetails
    {
        [Key]
        public Int64 ID { get; set; }
        public Int64 RequsitionMasterID { get; set; }
        public int ItemID { get; set; }
        public int RequestQty { get; set; }
        public int SendingQty { get; set; }
        public int ApprovedQty { get; set; }
        public string Remarks { get; set; }
        public string AprovedStatus { get; set; }
        public int ConsulateRequisitionID { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
