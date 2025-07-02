using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class getSavingLastBalance_Result
    {
        public int officeid { get; set; }
        public int Centerid { get; set; }
        public long MemberID { get; set; }
        public short ProductID { get; set; }
        public int NoOfAccount { get; set; }
        public long SavingSummaryID { get; set; }
        public Nullable<decimal> Balance { get; set; }
    }
}
