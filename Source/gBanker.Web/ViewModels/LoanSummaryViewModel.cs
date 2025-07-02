using gBanker.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class LoanSummaryViewModel:BaseModel
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

        public short SavingProductID { get; set; }  // KhalidAdd
        public short LOANProductID { get; set; }  // KhalidAdd

        public string ProductCode { get; set; }
        public string ProductName { get; set; }
          [Display(Name = "Samity")]
          [Required(ErrorMessage = "Center Code is required")]
        public int CenterID { get; set; }
        [Display(Name = "Samity Code")]
        public string CenterCode { get; set; }
        [Display(Name = "Samity Name")]
        public string CenterName { get; set; }
         [Display(Name = "Member Category")]
        public byte MemberCategoryID { get; set; }
          [Display(Name = "Loan Term")]
          [Required(ErrorMessage = "Loan Term is required")]
        public short LoanTerm { get; set; }
        [Display(Name = "Purpose")]
        public short PurposeID { get; set; }
        public string LoanNo { get; set; }
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
        [Display(Name = "SC Installment")]
        public int IntInstallment { get; set; }
        [Display(Name = "SC Rate")]
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
         [Display(Name = "SC Charge")]
        public decimal IntCharge { get; set; }
         [Display(Name = "SC Paid")]
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

        public decimal LoanINstallment { get; set; }
        public decimal InterestInstallment { get; set; }

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
        [Display(Name = "WriteOff SC")]
        public Nullable<decimal> WriteOffInterest { get; set; }
        [DefaultValue(0)]
        [Display(Name = "Additional Pay")]
        public decimal AddPay { get; set; }
         [Display(Name = "Investor")]
        public byte InvestorID { get; set; }
        
         public string BankName { get; set; }

         public string ChequeNo { get; set; }

         public bool? IsApproved { get; set; }


         [StringLength(100)]
         public string CoApplicantName { get; set; }

         [StringLength(50)]
         public string Guarantor { get; set; }
         [Display(Name = "MemberPassBook No.")]
         public long? MemberPassBookRegisterID { get; set; }

         public DateTime? ChequeIssueDate { get; set; }

         [Column(TypeName = "numeric")]
         [Display(Name = "Approved Amount")]
         public decimal? ApprovedAmount { get; set; }

         [Column(TypeName = "numeric")]
         [Display(Name = "Partial Amount")]
         public decimal? PartialAmount { get; set; }

         public bool? FinalDisbursement { get; set; }
         [Display(Name = "Disbursement Type")]
         public byte? DisbursementType { get; set; }

         

        public IEnumerable<SelectListItem> MemberPassBookNOListItems { get; set; }
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
        public IEnumerable<SelectListItem> disType { get; set; }
        public int? searchData { get; set; }
        public IEnumerable<SelectListItem> SearchOption { get; set; }
    }
}