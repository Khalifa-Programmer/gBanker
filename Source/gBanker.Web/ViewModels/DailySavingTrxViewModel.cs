using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class DailySavingTrxViewModel
    {
        public long DailySavingTrxID { get; set; }
        public long SavingSummaryID { get; set; }
        public int OfficeID { get; set; }
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public short ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int CenterID { get; set; }
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

        public string BankName { get; set; }
        public string ChequeNo { get; set; }
        public Nullable<short> EmployeeID { get; set; }
        public Nullable<byte> MemberCategoryID { get; set; }
        public int IsActive { get; set; }
        public Nullable<System.DateTime> InActiveDate { get; set; }
        public string CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int OrgID { get; set; }
        //public string BankName { get; set; }
        //public string ChequeNo { get; set; }
        //public virtual Organization Organization { get; set; }
        //public virtual Center Center { get; set; }



        //public virtual Member Member { get; set; }

        //public virtual Office Office { get; set; }

        //public virtual Product Product { get; set; }
    }
}
