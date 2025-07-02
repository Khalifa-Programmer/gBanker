using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class AccReconcileViewModel :BaseModel
    {
        public long AccReconcileID { get; set; }

        public long TrxMasterID { get; set; }

       
        public DateTime TrxDate { get; set; }

        public int SenderOfficeId { get; set; }

        public int ReceiverOfficeId { get; set; }

        public string ReffNo { get; set; }

        [StringLength(200)]
        public string Purpose { get; set; }

        public decimal? Credit { get; set; }
        public decimal? Debit { get; set; }
        public bool? IsReconcile { get; set; }
        public int? OrgID { get; set; }
        public string VoucherID { get; set; }
        public string VoucherName { get; set; }
    }
}