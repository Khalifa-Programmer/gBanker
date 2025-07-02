using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class getTodaysTransaction_Result
    {
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
        public System.DateTime InstallmentDate { get; set; }
        public decimal PrincipalLoan { get; set; }
        public decimal LoanDue { get; set; }
        public decimal LoanPaid { get; set; }
        public decimal IntCharge { get; set; }
        public decimal IntDue { get; set; }
        public decimal IntPaid { get; set; }
        public short InstallmentNo { get; set; }
        public string CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public byte TrxType { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public string MemberName { get; set; }
        public int NoOfAccount { get; set; }
        public decimal Deposit { get; set; }
        public decimal Withdrawal { get; set; }
        public decimal Penalty { get; set; }
        public decimal MonthlyInterest { get; set; }
        public byte TransType { get; set; }
    }
}
