using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial  class Proc_GetZeroBalance_Result
    {
        public int OfficeID { get; set; }
        public long SavingSummaryID { get; set; }
        public string OfficeCode { get; set; }
        public string CenterCode { get; set; }
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public string ProductCode { get; set; }
        public int NoOfAccount { get; set; }
        public decimal Deposit { get; set; }
        public decimal Withdrawal { get; set; }
        public decimal SavingInstallment { get; set; }
        public decimal InterestRate { get; set; }
        public decimal CumInterest { get; set; }
        public decimal MonthlyInterest { get; set; }
        public decimal Penalty { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public System.DateTime OpeningDate { get; set; }
        public Nullable<System.DateTime> MaturedDate { get; set; }
        public Nullable<System.DateTime> ClosingDate { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public decimal LoanBalance { get; set; }
        public decimal ScBalance { get; set; }
        public decimal TotalBalance { get; set; }
        public string MemberStatus { get; set; }
    }
}
