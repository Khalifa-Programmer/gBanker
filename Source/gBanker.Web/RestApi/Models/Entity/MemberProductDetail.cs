using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class MemberProductDetail
    {
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public int CenterID { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public string ProductID { get; set; }
        public string ProductCode { get; set; }
        //PrinBalance,SerBalance
        public string ProductName { get; set; }
        public byte ProductType { get; set; }
        public decimal LoanRecovery { get; set; }
        public decimal Recoverable { get; set; }
        public decimal Balance { get; set; }
        public DateTime InstallmentDate { get; set; }
        public int InstallmentNo { get; set; }
        public int TrxType { get; set; }
        public long SummaryID { get; set; }

        public decimal PrinBalance { get; set; }
        public decimal SerBalance { get; set; }
        public string InterestCalculationMethod { get; set; }
        public int Duration { get; set; }
        public decimal DurationOverLoanDue { get; set; }
        public decimal DurationOverIntDue { get; set; }
        public decimal LoanDue { get; set; }

        public decimal IntDue { get; set; }
        public decimal CumIntCharge { get; set; }
        public decimal CumInterestPaid { get; set; }

        public decimal PrincipalLoan { get; set; }
        public decimal LoanRepaid { get; set; }
        public decimal IntCharge { get; set; }
        // public string CollectionType { get; set; }
        public decimal NewDue { get; set; }
        public string MainProductCode { get; set; }
        public byte Doc { get; set; }
        
        public string accountNo { get; set; }
        public int fine { get; set; }

        public decimal SCPaid { get; set; }
        public decimal PersonalWithdraw { get; set; }

        public decimal PersonalSaving { get; set; }

        public decimal CumLoanDue { get; set; }
        public decimal CumIntDue { get; set; }

        public DateTime DisburseDate { get; set; }
        public decimal DisburseAmount { get; set; }

        public int orgID { get; set; }
        public int allowFine { get; set; }
    }
}