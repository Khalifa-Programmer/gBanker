using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class MemberwiseProductAndAccountNoViewModel
    {
        public String ProductID { get; set; }
        public String ProductName { get; set; }
        public String NoOfAccount { get; set; }
        public long SavingSummaryID { get; set; }
        public long SavingSummaryIDPre { get; set; }
        public decimal Deposit { get; set; }
        public decimal Withdrawal { get; set; }
        
    }
}

