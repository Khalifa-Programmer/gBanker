using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.RestApi.Models.Entity
{
    public class LoanProposal
    {
        public long LoanSummaryId { get; set; }
        public int OfficeID { get; set; }
        public int CenterID { get; set; }
        public long MemberID { get; set; }
        public string MemberName { get; set; }
        public string MemberCode { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public string Guarantor { get; set; }
        public byte MemberCategoryID { get; set; }
        public short ProductID { get; set; }
        public byte InvestorID { get; set; }
        public short PurposeID { get; set; }
        public int Duration { get; set; }
        public byte TransType { get; set; }
        public string BankName { get; set; }
        public string ChequeNo { get; set; }
        public byte LoanTerm { get; set; }
        public Decimal PrincipalLoan { get; set; }
        public Decimal LoanInstallment { get; set; }
        public Decimal IntInstallment { get; set; }
        public byte DisbursementType { get; set; }
        public bool IsApproved { get; set; }
        public string CoApplicantName { get; set; }
        public DateTime ChequeIssueDate { get; set; }
        public Decimal ProposalAmount { get; set; }
        public string SecurityBankName { get; set; }
        public string SecurityBankBranchName { get; set; }
        public string SecurityBankCheckNo { get; set; }
        public long MemberPassBookRegisterID { get; set; }
        public DateTime PropsalDate { get; set; }
        public Decimal InterestRate { get; set; }
        public Decimal IntCharge { get; set; }
        public DateTime? DisburseDate { get; set; }
        public DateTime? InstallmentStartDate { get; set; }
        public string ProposalNo { get; set; }
        public int ProductInstallmentMethodId { get; set; }
        public string ProductInstallmentMethodName { get; set; }
        public Decimal ProductInstallmentLoanInstallment { get; set; }
        public Decimal ProductInstallmentIntInstallment { get; set; } 
    }
}