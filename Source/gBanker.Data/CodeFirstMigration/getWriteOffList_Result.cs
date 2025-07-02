using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class getWriteOffList_Result
    {
        public long LoanSummaryID { get; set; }

        public int OfficeID { get; set; }
        
        public string OfficeCode { get; set; }
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string FirstName { get; set; }
        public short ProductID { get; set; }
        public string ProductCode { get; set; }
        public int CenterID { get; set; }
        public string CenterCode { get; set; }
        public int LoanTerm { get; set; }
        public bool isProcessed { get; set; }
        public Nullable<decimal> PrincipalLoan { get; set; }
        public Nullable<decimal> LoanPaid { get; set; }
        public Nullable<decimal> LoanBalance { get; set; }
        public Nullable<decimal> intPaid { get; set; }
        public Nullable<decimal> IntCharge { get; set; }
        public Nullable<decimal> intBal { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public DateTime? DisburseDate { get; set; }
        public string DisburseDatestg { get; set; }
    }
}
