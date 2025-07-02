using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.ResponseModels
{
    public class RebateCalcResult
    {
        public Decimal PrincipalLoan { get; set; }
        public Decimal LoanRepaid { get; set; }
        public Decimal LoanBalance { get; set; }
        public Decimal IntCharge { get; set; }
        public Decimal IntPaid { get; set; }
        public Decimal IntBalance { get; set; }
        public Decimal CumIntDue { get; set; }
        public Decimal NewRebate { get; set; }
        public Decimal TotalRebate { get; set; }
        public Decimal IntCollection { get; set; }
    }
}