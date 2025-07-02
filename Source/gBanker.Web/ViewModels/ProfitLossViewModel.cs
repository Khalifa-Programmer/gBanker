using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class ProfitLossViewModel
    {
        public int OfficeID { get; set; }
        public string ZoneCode { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public decimal ProfitLossCurrentMonth { get; set; }
        public decimal ProfitLossThisYear { get; set; }
        public DateTime TrxDateMax { get; set; }
    }
}
