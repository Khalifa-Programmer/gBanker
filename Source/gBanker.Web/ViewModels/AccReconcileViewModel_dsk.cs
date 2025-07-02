using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class AccReconcileViewModel_dsk :BaseModel
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


        // Mahfuz
        public string TrxDtMsg { get; set; }
        public long? Receiver_TrxMasterID { get; set; }
        public long? HO_TrxMasterID { get; set; }
        public string SenderOffice { get; set; }
        public string ReceiverOffice { get; set; }
        public string Sndr_V_No { get; set; }
        public string Sndr_V_Type { get; set; }
        public string Rec_V_No { get; set; }
        public string Rec_V_Type { get; set; }
        public string HO_V_No { get; set; }
        public string HO_V_Type { get; set; }
    }
}