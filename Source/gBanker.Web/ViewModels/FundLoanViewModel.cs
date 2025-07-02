using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class FundLoanViewModel
    {
        public int FundLoanID { get; set; }

        [Display(Name = "Fund Loan Name")]
        public string FundLoanCode { get; set; }

        [Display(Name = "Principal Amount")]
        public decimal? PrincipalAmount { get; set; }

        [Display(Name = "Loan Sanction No")]
        public int? LoanSanctionNo { get; set; }

        [Display(Name = "Loan Sanction Term")]
        public int? LoanSanctionTerm { get; set; }

        [Display(Name = "Loan Sanction Approve Date")]
        public string LoanSanctionApproveDate { get; set; }

        [Display(Name = "Loan Disbursement Date")]
        public string LoanDisbursementDate { get; set; }

        [Display(Name = "Disbursed Amount")]
        public decimal? DisbursedAmount { get; set; }

        [Display(Name = "Interest Rate(%)")]
        public decimal? InterestRate { get; set; }

        [Display(Name = "Loan Duration")]
        public int? LoanDuration { get; set; }

        [Display(Name = "Grace Period")]
        public int? GracePeriod { get; set; }

        [Display(Name = "Total Installment No")]
        public int? TotalInstallmentNo { get; set; }

        [Display(Name = "Service Charge")]
        public decimal? ServiceCharge { get; set; }

        [Display(Name = "Total Payable Amount")]
        public decimal? TotalPayableAmount { get; set; }

        [Display(Name = "Loan Installment Amount")]
        public decimal? LoanInstallmentAmount { get; set; }

        [Display(Name = "Installment Date")]
        public string InstallmentDate { get; set; }

        [Display(Name = "No of Installment")]
        public int? NoOfInstallment { get; set; }

        public IEnumerable<SelectListItem> FundLoanCodeList { get; set; }
        public List<LoanRepaymentDetail> LoanRepaymentDetailList { get; set; }
    }
    public partial class LoanRepaymentDetail
    {
        public int FundLoanID { get; set; }
        public decimal LoanInstallmentAmount { get; set; }
        public decimal ServiceCharge { get; set; }
        public string InstallmentDate { get; set; }
    }
}