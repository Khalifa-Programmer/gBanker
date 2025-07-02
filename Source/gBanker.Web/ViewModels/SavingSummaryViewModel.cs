using gBanker.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class SavingSummaryViewModel : BaseModel
    {
        [GlobalizedDisplayName("SavingSummaryID")]
        public long SavingSummaryID { get; set; }
        [Display(Name = "Office Code")]
        [Required(ErrorMessage = "Office Code is required")]
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        [Display(Name = "Member Code")]
        [Required(ErrorMessage = "Member Code is required")]
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        [Display(Name = "Product Code")]
        [Required(ErrorMessage = "Product Code is required")]
        public short ProductID { get; set; }
        public string ProductCode { get; set; }

        [Display(Name = "Samity Code")]
        [Required(ErrorMessage = "Samity Code is required")]
        public int CenterID { get; set; }
        [Display(Name = "Samity Code")]
        public string CenterCode { get; set; }
        [Display(Name = "NoOfAccount")]
        [Required(ErrorMessage = "Account No. is required")]
        public int NoOfAccount { get; set; }
        [Display(Name = "Transaction Date")]
        [Required(ErrorMessage = "Transaction Date is required")]
        public System.DateTime TransactionDate { get; set; }
        public decimal Deposit { get; set; }
        public decimal Withdrawal { get; set; }
        public decimal Balance { get; set; }
        [Display(Name = "Interest Rate")]
        [Required(ErrorMessage = "Interest Rate is required")]
        public decimal InterestRate { get; set; }
        [Display(Name = "Saving Installment")]
        public decimal SavingInstallment { get; set; }
        [Display(Name = "Cummulative Interest")]
        public decimal CumInterest { get; set; }
        [Display(Name = "Monthly Interest")]
        public decimal MonthlyInterest { get; set; }
        public decimal Penalty { get; set; }
        [Display(Name = "Opening Date")]
        public System.DateTime OpeningDate { get; set; }
        [Display(Name = "Matured Date")]
        public Nullable<System.DateTime> MaturedDate { get; set; }
        public Nullable<System.DateTime> ClosingDate { get; set; }
        public byte TransType { get; set; }
        public byte SavingStatus { get; set; }
        public short EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public byte MemberCategoryID { get; set; }
        public bool IsActive { get; set; }
        [Display(Name = "Ref Employee")]
        public int Ref_EmployeeID { get; set; }
        public Nullable<System.DateTime> InActiveDate { get; set; }

        //[GlobalizedDisplayName("SavingSummaryID")]
        //public long SavingSummaryID { get; set; }
        //[Required(ErrorMessage = "Office Code Required")]
        //[Display(Name = "Office Code")]
        //public int OfficeID { get; set; }
        //[Required(ErrorMessage = "Member Code Required")]
        //[Display(Name = "Member Code")]
        //public long MemberID { get; set; }
        //[Required(ErrorMessage = "Product Code Required")]
        //[Display(Name = "Product Code")]
        //public short ProductID { get; set; }
        //[Required(ErrorMessage = "Center Code Required")]
        //[Display(Name = "Center Code")]
        //public int CenterID { get; set; }
        //[Required(ErrorMessage = "NoOfAccount Required")]
        //[Display(Name = "NoOf Account")]
        //public int NoOfAccount { get; set; }
        //[Required(ErrorMessage = "TransactionDate Required")]
        //[Display(Name = "Transaction Date")]
        //public System.DateTime TransactionDate { get; set; }
       
        //[Display(Name = "Deposit")]
        //public decimal Deposit { get; set; }
      
        //[Display(Name = "Withdrawal")]
        //public decimal Withdrawal { get; set; }
       
        //[Display(Name = "Balance")]
        //public decimal Balance { get; set; }
     
        //[Display(Name = "Interest Rate")]
        //public decimal InterestRate { get; set; }
        // [Display(Name = "Saving Installment")]
        //public decimal SavingInstallment { get; set; }
        // [Display(Name = "Cummulative Interest")]
        //public decimal CumInterest { get; set; }
        // [Display(Name = "Monthly  Interest")]
        //public decimal MonthlyInterest { get; set; }
        // [Display(Name = "Penalty")]
        //public decimal Penalty { get; set; }
        // [Display(Name = "Opening Date")]
        //public System.DateTime OpeningDate { get; set; }
        // [Display(Name = "Matured Date")]
        //public Nullable<System.DateTime> MaturedDate { get; set; }
        // [Display(Name = "Closing Date")]
        //public Nullable<System.DateTime> ClosingDate { get; set; }
        //public byte TransType { get; set; }
        //public byte SavingStatus { get; set; }
   
        //public short EmployeeId { get; set; }
        //public byte MemberCategoryID { get; set; }




       
        public IEnumerable<SelectListItem> productListItems { get; set; }
        public IEnumerable<SelectListItem> centerListItems { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
        public IEnumerable<SelectListItem> membercategoryListItems { get; set; }
        public IEnumerable<SelectListItem> purposeListItems { get; set; }
        public IEnumerable<SelectListItem> memberListItems { get; set; }
        public IEnumerable<SelectListItem> paymentMode { get; set; }

      
    }
}