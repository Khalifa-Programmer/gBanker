using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class FundTransferVoucherViewModel
    {
        public string TrxDate { get; set; }
        public List<InnerVoucherDetailsViewModel> Sender { get; set; }
        public List<InnerVoucherDetailsViewModel> Receiver { get; set; }
        public List<InnerVoucherDetailsViewModel> HO_vou { get; set; }
    }

    public class InnerVoucherDetailsViewModel 
    {
    public string Office { get; set; }
    public string VoucherType { get; set; }
    public string VoucherDesc { get; set; }
    public string AccountHead { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    }
}