using gBanker.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class AccAccountDetailsViewModel
    {
        [GlobalizedDisplayName("TrxMaster")]
        public long TrxMasterID { get; set; }

        [GlobalizedDisplayName("Office")]
        public int OfficeID { get; set; }

        [GlobalizedDisplayName("OfficeLevel")]
        public int OfficeLevel { get; set; }

        [GlobalizedDisplayName("TrxDate")]
        public System.DateTime TrxDate { get; set; }

        [GlobalizedDisplayName("LastWorkingDate")]
        public DateTime LastWorkingDate { get; set; }

        [GlobalizedDisplayName("TrxDtMsg")]
        public string TrxDtMsg { get; set; }
        public string VoucherNo { get; set; }
        public string VoucherType { get; set; }
        public string VoucherDesc { get; set; }
        public string Reference { get; set; }
        public Nullable<decimal> TotDebit { get; set; }
        public Nullable<decimal> TotCredit { get; set; }
        public Nullable<bool> IsAutoVoucher { get; set; }
       
       
       
        [Display(Name = "Rectify")]
        public bool? IsRectify { get; set; }
        public bool? IsPosted { get; set; }
        public bool? IsYearlyClosing { get; set; }
        
    }
}