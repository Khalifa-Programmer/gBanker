using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class Proc_get_SavingLastBalance
    {
        public int officeid { get; set; }
        public int Centerid { get; set; }
        public long MemberID { get; set; }
        public int ProductID { get; set; }
        public int NoOfAccount { get; set; }
        public long SavingSummaryID { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public decimal SavingInstallment { get; set; }
        public decimal Penalty { get; set; }
        public decimal DueSavingInstallment { get; set; }

        public decimal CumInterest { get; set; }
    }
}
