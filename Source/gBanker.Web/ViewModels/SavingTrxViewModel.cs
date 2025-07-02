using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class SavingTrxViewModel : BaseModel
    {
        public int rowSl { get; set; }
        public long SavingTrxID { get; set; }
        public long SavingSummaryID { get; set; }
        public int OfficeID { get; set; }
        public long MemberID { get; set; }
        public short ProductID { get; set; }
        public int CenterID { get; set; }
        public int NoOfAccount { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Deposit { get; set; }
        public decimal Withdrawal { get; set; }
        public decimal Balance { get; set; }
        public decimal Penalty { get; set; }
        public byte TransType { get; set; }
        public decimal MonthlyInterest { get; set; }
        public bool PresenceInd { get; set; }
        public decimal TransferDeposit { get; set; }
        public decimal TransferWithdrawal { get; set; }
        public Int16? EmployeeID { get; set; }
        public byte MemberCategoryID { get; set; }
        public long IsActive { get; set; }
        public DateTime? InActiveDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public int OrgID { get; set; }
        

    }// ENd Class
}// End NameSpace