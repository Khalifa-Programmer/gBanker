using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class LoanProposalListViewModel
    {
        public long LoanSummaryID { get; set; }
        public int OfficeID { get; set; }
        public long MemberID { get; set; }
        public int ProductID { get; set; }
        public int CenterID { get; set; }
        public byte MemberCategoryID { get; set; }
        public int LoanTerm { get; set; }
        public Nullable<int> PurposeID { get; set; }
        public string LoanNo { get; set; }
        public decimal PrincipalLoan { get; set; }
        public System.DateTime ApproveDate { get; set; }
        public Nullable<System.DateTime> DisburseDate { get; set; }
        public int Duration { get; set; }
        public decimal LoanRepaid { get; set; }
        public decimal IntCharge { get; set; }
        public decimal IntPaid { get; set; }
        public decimal LoanInstallment { get; set; }
        public decimal IntInstallment { get; set; }
        public decimal InterestRate { get; set; }
        public Nullable<System.DateTime> InstallmentStartDate { get; set; }
        public int InstallmentNo { get; set; }
        public int DropInstallment { get; set; }
        public int Holidays { get; set; }
        public Nullable<System.DateTime> InstallmentDate { get; set; }
        public byte TransType { get; set; }
        public short ContinuousDrop { get; set; }
        public byte LoanStatus { get; set; }
        public decimal Balance { get; set; }
        public decimal Advance { get; set; }
        public decimal DueRecovery { get; set; }
        public Nullable<System.DateTime> LoanCloseDate { get; set; }
        public Nullable<System.DateTime> OverdueDate { get; set; }
        public short EmployeeId { get; set; }
        public Nullable<byte> InvestorID { get; set; }
        public decimal ExcessPay { get; set; }
        public Nullable<decimal> CurLoan { get; set; }
        public Nullable<decimal> PreLoan { get; set; }
        public Nullable<decimal> CumLoanDue { get; set; }
        public Nullable<decimal> WriteOffLoan { get; set; }
        public Nullable<decimal> WriteOffInterest { get; set; }
        public bool Posted { get; set; }
        public int OrgID { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> InActiveDate { get; set; }
        public string CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string BankName { get; set; }
        public string ChequeNo { get; set; }
        public Nullable<bool> IsApproved { get; set; }
        public string CoApplicantName { get; set; }
        public string Guarantor { get; set; }
        public Nullable<long> MemberPassBookRegisterID { get; set; }
        public Nullable<System.DateTime> ChequeIssueDate { get; set; }
        public Nullable<decimal> CumIntDue { get; set; }
        public Nullable<decimal> ApprovedAmount { get; set; }
        public Nullable<decimal> PartialAmount { get; set; }
        public Nullable<byte> FinalDisbursement { get; set; }
        public Nullable<byte> DisbursementType { get; set; }
        public Nullable<decimal> PartialIntCharge { get; set; }
        public Nullable<decimal> PartialIntPaid { get; set; }
        public Nullable<System.DateTime> FirstInstallmentDate { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public string MemberCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public Nullable<int> RoleId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PurposeCode { get; set; }
        public string PurposeName { get; set; }
    }
}