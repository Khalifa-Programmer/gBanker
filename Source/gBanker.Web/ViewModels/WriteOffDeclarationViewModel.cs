using gBanker.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace gBanker.Web.ViewModels
{
    public class WriteOffDeclarationViewModel
    {
        public long LoanSummaryID { get; set; }
        [Display(Name = "Office")]
        [Required(ErrorMessage = "Office Code is required")]
        public int OfficeID { get; set; }

        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        [Display(Name = "Member")]
        [Required(ErrorMessage = "Member Code is required")]
        public long MemberID { get; set; }


        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        [Display(Name = "Product")]
        [Required(ErrorMessage = "Product Code is required")]
        public short ProductID { get; set; }

        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        [Display(Name = "Samity")]
        [Required(ErrorMessage = "Samity Code is required")]
        public int CenterID { get; set; }
        [Display(Name = "Samity Code")]
        public string CenterCode { get; set; }
        [Display(Name = "Samity Name")]
        public string CenterName { get; set; }
        [Display(Name = "Member Category")]
        public byte MemberCategoryID { get; set; }
        [Display(Name = "Loan Term")]
        [Required(ErrorMessage = "Loan Term is required")]
        public byte LoanTerm { get; set; }
        [Display(Name = "Purpose")]
        public short PurposeID { get; set; }
        public string Purpose { get; set; }
        [Display(Name = "Principal Loan")]
        public decimal PrincipalLoan { get; set; }
        [Display(Name = "Approve Date")]
        public System.DateTime ApproveDate { get; set; }
        [Display(Name = "Disburse Date")]
        [Required(ErrorMessage = "Disburse Date is required")]
        public Nullable<System.DateTime> DisburseDate { get; set; }
        public int Duration { get; set; }
        [Display(Name = "Loan Installment")]
        public int LoanInstallment { get; set; }
        [Display(Name = "Interest Installment")]
        public int IntInstallment { get; set; }
        [Display(Name = "Interest Rate")]
        public decimal InterestRate { get; set; }
        [Display(Name = "Installment StartDate")]
        public Nullable<System.DateTime> InstallmentStartDate { get; set; }
        [Display(Name = "Installment Date")]
        public System.DateTime InstallmentDate { get; set; }
        [Display(Name = "Transaction Type")]
        public byte TransType { get; set; }
        [Display(Name = "Loan Status")]
        public byte LoanStatus { get; set; }
        [Display(Name = "Employee")]
        public short EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        [Display(Name = "Loan Repaid")]
        public decimal LoanRepaid { get; set; }
        [Display(Name = "Interest Charge")]
        public decimal IntCharge { get; set; }
        [Display(Name = "Interest Paid")]
        public decimal IntPaid { get; set; }
        [Display(Name = "Installment No")]
        public int InstallmentNo { get; set; }
        [Display(Name = "Drop Installment")]
        public int DropInstallment { get; set; }
        [Display(Name = "Holi Days")]
        public int Holidays { get; set; }
        [Display(Name = "Continuous Drop")]
        public short ContinuousDrop { get; set; }
        public decimal Balance { get; set; }
        public decimal Advance { get; set; }
        [Display(Name = "Due Recovery")]
        public decimal DueRecovery { get; set; }
        [Display(Name = "Loan CloseDate")]
        public Nullable<System.DateTime> LoanCloseDate { get; set; }
        [Display(Name = "Overdue Date")]
        public Nullable<System.DateTime> OverdueDate { get; set; }
        [Display(Name = "Excess Pay")]
        public decimal ExcessPay { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Cur. Loan ")]
        public Nullable<decimal> CurLoan { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Pre. Loan ")]
        public Nullable<decimal> PreLoan { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Cummulative Loan Due")]
        public Nullable<decimal> CumLoanDue { get; set; }
        [DefaultValue(0)]
        [Display(Name = "WriteOff Loan")]
        public Nullable<decimal> WriteOffLoan { get; set; }
        [DefaultValue(0)]
        [Display(Name = "WriteOff Interest")]
        public Nullable<decimal> WriteOffInterest { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Additional Pay")]
        public decimal AddPay { get; set; }
        [Display(Name = "Investor")]
        public byte InvestorID { get; set; }

        public IEnumerable<SelectListItem> investorListItems { get; set; }
        public IEnumerable<SelectListItem> productListItems { get; set; }
        public IEnumerable<SelectListItem> centerListItems { get; set; }
        public IEnumerable<SelectListItem> branchListItems { get; set; }
        public IEnumerable<SelectListItem> membercategoryListItems { get; set; }
        public IEnumerable<SelectListItem> purposeListItems { get; set; }
        public IEnumerable<SelectListItem> memberListItems { get; set; }
        public string memName { get; set; }
        public IEnumerable<ProductViewModel> MemberProductItemsSelected { get; set; }
        public IEnumerable<SelectListItem> paymentMode { get; set; }
        public int? searchData { get; set; }
        public IEnumerable<SelectListItem> SearchOption { get; set; }
    }
}