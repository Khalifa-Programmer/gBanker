using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace gBanker.Web.ViewModels
{
    public class DailyLoanTrxViewModel : BaseModel
    {

        public long DailyLoanTrxID { get; set; }

        [Display(Name = "Transaction Date")]
        public DateTime TrxDate { get; set; }
        public string TrxDateMsg { get; set; }
        public long LoanSummaryID { get; set; }
        public string LoanNo { get; set; }

        [Required(ErrorMessage = "Office is required")]
        [Display(Name = "Office")]
        public int OfficeID { get; set; }

        [Required(ErrorMessage = "Member is required")]
        [Display(Name = "Member")]
        public long MemberID { get; set; }
        
        [Display(Name = "Member Code")]
        public string MemberCode { get; set; }
        
        [Display(Name = "Member Name")]
        public string MemberName { get; set; }

        [Display(Name = "Phone No")]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Product is required")]
        [Display(Name = "Product")]
        public short ProductID { get; set; }

        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Display(Name = "Interest Calculation Method")]
        public string InterestCalculationMethod { get; set; }

        [Display(Name = "Samity")]
        public int CenterID { get; set; }

        [Display(Name = "Member Category")]
        public byte MemberCategoryID { get; set; }

        [Required(ErrorMessage = "Loan Term is required")]
        [Display(Name = "Loan Term")]
        public int LoanTerm { get; set; }

        [Display(Name = "Purpose")]
        public short PurposeID { get; set; }

        [Required(ErrorMessage = "Installment Date is required")]
        [Display(Name = "Installment Date")]
        public DateTime InstallmentDate { get; set; }

        [Display(Name = "Principal Loan")]
        public decimal PrincipalLoan { get; set; }

        [Display(Name = "Loan Repaid")]
        public decimal LoanRepaid { get; set; }

        [Display(Name = "Loan Due")]
        public decimal LoanDue { get; set; }

        [Display(Name = "Loan Paid")]
        public decimal LoanPaid { get; set; }

        public decimal CumIntCharge { get; set; }
       // public decimal CumInterestPaid { get; set; }
        public decimal IntCharge { get; set; }

        public decimal IntDue { get; set; }

        [Display(Name = "Interest Paid")]
        public decimal IntPaid { get; set; }

        public decimal Advance { get; set; }

        public decimal DueRecovery { get; set; }

        public byte TrxType { get; set; }

        [Display(Name = "Installment No")]
        public short InstallmentNo { get; set; }

        [Display(Name = "Employee")]
        public short EmployeeID { get; set; }
        public string CollectionStatus { get; set; }
        //public bool? IsActive { get; set; }
        //public DateTime? InActiveDate { get; set; }

        [Display(Name = "Total Paid")]
        public decimal TotalPaid { get; set; }
        public byte InvestorID { get; set; }

        public int Duration { get; set; }
        public int OrgID { get; set; }

        public decimal DurationOverLoanDue { get; set; }
        public byte DurationOverCollection { get; set; }
        public string LoanAccountNo { get; set; }

        public decimal DurationOverIntDue { get; set; }
        public decimal CumLoanDue { get; set; }
        public decimal CumIntDue { get; set; }
        public string MainProductCode { get; set; }
        public int MainProductID { get; set; }
        public IEnumerable<SelectListItem> centerListItems { get; set; }
        public IEnumerable<SelectListItem> cashListItems { get; set; }
        public IEnumerable<SelectListItem> productListItems { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
        public IEnumerable<SelectListItem> membercategoryListItems { get; set; }
        public IEnumerable<SelectListItem> purposeListItems { get; set; }
        public IEnumerable<SelectListItem> memberListItems { get; set; }
        public string memName { get; set; }
        public string vMaxLoanTerm { get; set; }
        public IEnumerable<ProductViewModel> MemberProductItemsSelected { get; set; }
        public decimal DueLoanSummary { get; set; }
        public decimal LoanCollectionSummary { get; set; }
        public decimal DueInterestSummary { get; set; }
        public decimal InterestCollectionSummary { get; set; }
        public decimal TotalDueSummary { get; set; }
        public decimal TotalCollectionSummary { get; set; }
        public string SmsStatus { get; set; }
        public int rowSl { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }

        //public DateTime CreateDate { get; set; }      
    }
}