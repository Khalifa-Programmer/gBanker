using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class RemittanceActivityViewModel
    {
        public int RemittanceActivityId { get; set; }
        public int? NoOfClient { get; set; }
        public string TransactionDate { get; set; }
        public decimal? RemittedAmount { get; set; }
        public decimal? Commission { get; set; }
        public string LinkedBank { get; set; }
        public string Remark { get; set; }
    }
}