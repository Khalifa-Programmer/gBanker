using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class LoanInstallmentCorrectionViewmodel
    {
        public long DailyLoanTrxID { get; set; }
        public long LoanSummaryID { get; set; }
        public string LoanNo { get; set; }
        public int OfficeID { get; set; }
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public Int16 ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string InterestCalculationMethod { get; set; }
        public int CenterID { get; set; }
        public Int16 MemberCategoryID { get; set; }
        public int LoanTerm { get; set; }
        public Int16 PurposeID { get; set; }
        public decimal PrincipalLoan { get; set; }
        public decimal LoanRepaid { get; set; }
        public decimal LoanDue { get; set; }
        public decimal CumIntCharge { get; set; }
        public decimal IntCharge { get; set; }
        public decimal IntDue { get; set; }
        public decimal Advance { get; set; }
        public decimal DueRecovery { get; set; }
        public Int16 TrxType { get; set; }
        public Int16 InstallmentNo { get; set; }
        public Int16 InvestorID { get; set; }
        public decimal TotalPaid { get; set; }
        public Int16 CollectionStatus { get; set; }
        public int OrgID { get; set; }
        public Boolean IsActive { get; set; }
        public string CreateUser { get; set; }
        public int Duration { get; set; }
        public decimal DurationOverLoanDue { get; set; }
        public decimal DurationOverIntDue { get; set; }

    }
}