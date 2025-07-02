using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class LoanUpdateViewModel
    {
        // ========== Loan Summary ==============//
        public Int64 LoanSummaryID { get; set; }

        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        public Int64 MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public short ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int CenterID { get; set; }
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public byte MemberCategoryID { get; set; }
        public string MemberCategoryCode { get; set; }
        public string CategoryName { get; set; }
        public Int16 PurposeID { get; set; }
        public string PurposeCode { get; set; }
        public string PurposeName { get; set; }
        public int LoanTerm { get; set; }
        public decimal PrincipalLoan { get; set; }
        public string ApproveDate { get; set; }
        public string DisburseDate { get; set; } = string.Empty;
        public int? Duration { get; set; }
        public decimal? LoanRepaid { get; set; }
        public decimal? IntCharge { get; set; }
        public decimal? IntPaid { get; set; }
        public decimal? LoanInstallment { get; set; }
        public decimal? IntInstallment { get; set; }
        public decimal? InterestRate { get; set; }
        public int? InstallmentNo { get; set; }
        public string InstallmentDate { get; set; } = string.Empty;
        public byte LoanStatus { get; set; }
        public decimal? PartialAmount { get; set; }
        public decimal? PartialIntCharge { get; set; }
        public decimal? PartialIntPaid { get; set; }

        // ========== Loan Trx ==============//

        public long LoanTrxID { get; set; }
        public int rowSl { get; set; }
        public string TrxDate { get; set; }
        public decimal LoanDue { get; set; }
        public decimal LoanPaid { get; set; }
        public decimal IntDue { get; set; }
        public decimal Advance { get; set; }
        public decimal DueRecovery { get; set; }
        public byte TrxType { get; set; }
        public short EmployeeID { get; set; }
        public byte InvestorID { get; set; }
        public int OrgID { get; set; }
        public bool IsActive { get; set; }
        public DateTime InActiveDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}