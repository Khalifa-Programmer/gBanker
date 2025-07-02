using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class FundTransferViewModel
    {
        public string TransactionType { get; set; }
        public DateTime Date { get; set; }
        public string Narration { get; set; }
        public string Reference { get; set; }
        public List<InnerVoucherViewModel> Sender { get; set; }
        public List<InnerVoucherViewModel> Receiver { get; set; }
        public List<InnerVoucherViewModel> HO_vou { get; set; }
    }

    public class InnerVoucherViewModel
    {
        public int Sl { get; set; }
        public int OfficeID { get; set; }
        public string Narration { get; set; }
        public int AccID { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
}