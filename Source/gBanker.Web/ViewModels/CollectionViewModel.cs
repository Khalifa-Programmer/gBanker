using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class CollectionViewModel
    {
        public long SummaryID { get; set; }
        [Display(Name = "Office")]
        public int OfficeID { get; set; }
        [Display(Name = "Member")]
        public long MemberID { get; set; }

       
        public int ProductId { get; set; }

        public long Id { get; set; }

        public decimal LoadPaid { get; set; }
        public decimal Balance { get; set; }


        [Display(Name = "Center Code")]
        public string CenterCode { get; set; }

        [Display(Name = "Member Code")]
        public string MemberCode { get; set; }

        [Display(Name = "Member Name")]
        public string MemberName { get; set; }
        [Display(Name = "Product")]
        public short ProductID { get; set; }

        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Display(Name = "Samity")]
        public int CenterID { get; set; }
        [Display(Name = "Loan Due")]
        public decimal LoanDue { get; set; }

        [Display(Name = "Total Paid")]
        public decimal TotalPaid { get; set; }

        [Display(Name = "Loan Paid")]
        public decimal LoanPaid { get; set; }
        public decimal Deposit { get; set; }
        public decimal IntDue { get; set; }
        public decimal WithDrawal { get; set; }

        [Display(Name = "Interest Paid")]
        public decimal IntPaid { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DueSavingInstallment { get; set; }

        [Column(TypeName = "numeric")]
        public decimal SavingInstallment { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Withdrawal { get; set; }

        [Column(TypeName = "numeric")]
       // public decimal Balance { get; set; }
        public decimal TotalDue { get; set; }
        public int installmentNo { get; set; }
        public bool IsLoanProduct { get; set; }


        public int Duration { get; set; }
        public decimal DurationOverLoanDue { get; set; }
        public decimal DurationOverIntDue { get; set; }
        public decimal CumInterestPaid { get; set; }
        public decimal CumIntCharge { get; set; }
        public decimal CumLoanDue { get; set; }
        public decimal CumIntDue { get; set; }
        public decimal DOC { get; set; }
        public int OrgID { get; set; }
        public decimal principalLoan { get; set; }
        public decimal loanRepaid { get; set; }





        public IEnumerable<SelectListItem> CenterListItems { get; set; }
        public IEnumerable<SelectListItem> memberListItems { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
    }
}