using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class InterBranchTransactionViewModel : BaseModel
    {
        //Master Props
        [Required(ErrorMessage = "Office is required")]
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }


        [Display(Name = "Samity ID")]
        [Required(ErrorMessage = "Samity is required")]
        public int CenterID { get; set; }
        [Display(Name = "Samity Code")]
        public string CenterCode { get; set; }

        public string BranchTrxType { get; set; }


        [Required(ErrorMessage = "Member is required")]
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }


        [Required(ErrorMessage = "Product is required")]
        public short ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }

        


        //------------- Special Savings ----------------------
        public long DailySavingTrxID { get; set; }
        public long SavingSummaryID { get; set; }

        [Required(ErrorMessage = "Account No. is required")]
        [Range(1, 100000)]
        public int NoOfAccount { get; set; }
        public decimal SavingInstallment { get; set; }
        public decimal Withdrawal { get; set; }
        public decimal Penalty { get; set; }
        public decimal Balance { get; set; }
        [Display(Name = "SavingInstallment(Scheme)")]
        public decimal DueSavingInstallment { get; set; }
        public byte TransType { get; set; }
        public string BankName { get; set; }
        public string ChequeNo { get; set; }

        public string LoanType { get; set; }
        public DateTime TrxDate { get; set; }

        //DropDown
        public IEnumerable<SelectListItem> officeListItems { get; set; }
        public IEnumerable<SelectListItem> centerListItems { get; set; }
        public IEnumerable<SelectListItem> productListItems { get; set; }
        public IEnumerable<SelectListItem> cashListItems { get; set; }
        public IEnumerable<SelectListItem> memberListItems { get; set; }

        public IEnumerable<SelectListItem> membercategoryListItems { get; set; }
        public IEnumerable<SelectListItem> purposeListItems { get; set; }
        public IEnumerable<SelectListItem> branchTrxTypeListItems { get; set; }
        public IEnumerable<SelectListItem> GetAccountCodeList { get; set; }


        //------------- Special Loan ----------------------
        [Range(1, 10000)]
        public int LoanTerm { get; set; }
        public byte TrxType { get; set; }
        [Column(TypeName = "numeric")]
        public decimal TotalPaid { get; set; }
        public decimal LoanPaid { get; set; }
        [Display(Name = "SC Paid")]
        public decimal IntPaid { get; set; }
        public decimal LoanDue { get; set; }
        public decimal IntDue { get; set; }
        public decimal PrincipalLoan { get; set; }
        public decimal LoanRepaid { get; set; }
        [Display(Name = "Cumm.SC Charge")]
        public decimal CumIntCharge { get; set; }
        [Display(Name = "Cumm.SC Paid")]
        public decimal DueRecovery { get; set; }
        [Column(TypeName = "numeric")]
        public decimal LoanBal { get; set; }
        [Column(TypeName = "numeric")]
        public decimal SerBal { get; set; }
        public decimal IntCharge { get; set; }
        [Display(Name = "Fine")]
        [Column(TypeName = "numeric")]
        public decimal Fine { get; set; }
        public short InstallmentNo { get; set; }

        //--------- Other ------------
        public DateTime TransactionDate { get; set; }
    }
    public class Response
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}