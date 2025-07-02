using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class SavingUpdateViewModel
    {
        //======================== Saving Summary ========================//
        public long SavingSummaryID { get; set; }
        public int CenterID { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public short ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int NoOfAccount { get; set; }
        public string TransactionDate { get; set; }
        public decimal Deposit { get; set; }
        public decimal Withdrawal { get; set; }
        public decimal InterestRate { get; set; }
        public decimal SavingsInstallment { get; set; }
        public decimal CumInterest { get; set; }
        public decimal MonthlyInterest { get; set; }
        public decimal Penalty { get; set; }
        public string ClosingDate { get; set; }
        public byte SavingStatus { get; set; }

        //============= Saving Trx Colomns ================//
        public long SavingTrxID { get; set; }
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public decimal Balance { get; set; }
        public byte TransType { get; set; }
        public bool PresenceInd { get; set; }
        public decimal TransferDeposit { get; set; }
        public decimal TransferWithdrawal { get; set; }
        public short EmployeeID { get; set; }
        public byte MemberCategoryID { get; set; }
        public string MemberCategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string CreateUser { get; set; }
        public string CreateDate { get; set; }

        //============= No Of Account Dropdwon Property ============//
        public string NoOfAccountValue { get; set; }
        public string NoOfAccountText { get; set; }
    }
}