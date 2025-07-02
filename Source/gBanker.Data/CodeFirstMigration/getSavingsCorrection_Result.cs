using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration
{
    public partial class getSavingsCorrection_Result
    {
        public long SavingCorrectionTrxID { get; set; }
        public long SavingSummaryID { get; set; }
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public short ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int CenterID { get; set; }
        public string CenterCode { get; set; }
        public int NoOfAccount { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public decimal DueSavingInstallment { get; set; }
        public decimal SavingInstallment { get; set; }
        public decimal Deposit { get; set; }
        public decimal Withdrawal { get; set; }
        public decimal Balance { get; set; }
        public decimal Penalty { get; set; }
        public byte TransType { get; set; }
        public decimal MonthlyInterest { get; set; }
        public bool PresenceInd { get; set; }
        public decimal TransferDeposit { get; set; }
        public decimal TransferWithdrawal { get; set; }
        public long IsActive { get; set; }
        public Nullable<System.DateTime> InActiveDate { get; set; }
        public string CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}
