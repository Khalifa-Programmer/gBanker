using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class AccCodeViewModel : BaseModel
    {
        
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public long TrxMasterID { get; set; }
        public string TrxDate { get; set; }

        public string VoucherNo { get; set; }
        public string VoucherType { get; set; }

        public int trxDay { get; set; }
        public int trxMonth { get; set; }
        public int trxYear { get; set; }

        public string TrxDateFormated { get; set; }
        public long TrxDetailsID { get; set; }
        public int AccID { get; set; }
        public string AccCodes { get; set; }
        public string AccName { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }

        public string Narration { get; set; }
        public string ReconPurpose { get; set; }



    }// End Class
}// End NameSpace