using gBanker.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class PartialLoanDisbursementViewModel : BaseModel
    {
        [GlobalizedDisplayName("LoanSummaryID")]
        public long LoanSummaryID { get; set; }
        [GlobalizedDisplayName("OfficeID")]
        [Required(ErrorMessage = "Office Code is required")]
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public string MemberCode { get; set; }
        public string ProductCode { get; set; }
        public string CenterCode { get; set; }
        [GlobalizedDisplayName("MemberID")]
        [Required(ErrorMessage = "Member is required")]
        public long MemberID { get; set; }
        [GlobalizedDisplayName("ProductID")]
        [Required(ErrorMessage = "Product is required")]
        public short ProductID { get; set; }
        [GlobalizedDisplayName("CenterID")]
        [Required(ErrorMessage = "Samity is required")]
        public int CenterID { get; set; }
        [GlobalizedDisplayName("LoanTerm")]
        [Required(ErrorMessage = "Loan Term is required")]
        public byte? LoanTerm { get; set; }
        [GlobalizedDisplayName("PurposeID")]
        [Required(ErrorMessage = "Purpose is required")]
        public short? PurposeID { get; set; }
        public string LoanNo { get; set; }
        [GlobalizedDisplayName("PrincipalLoan")]
        [Required(ErrorMessage = "Principal Loan is required")]
        public decimal PrincipalLoan { get; set; }
        public decimal LoanRepaid { get; set; }
        [GlobalizedDisplayName("IntPaid")]
        public decimal IntPaid { get; set; }
        [Column(TypeName = "numeric")]
        public decimal IntCharge { get; set; }
        [GlobalizedDisplayName("ApproveDate")]
        [Required(ErrorMessage = "Approve Date is required")]
        public System.DateTime ApproveDate { get; set; }
         [Display(Name = "Partial Sc.Charge")]
        [Column(TypeName = "numeric")]
        public decimal? PartialIntCharge { get; set; }
         [Display(Name = "Partial Sc.Paid")]
        [Column(TypeName = "numeric")]
        public decimal? PartialIntPaid { get; set; }

        [GlobalizedDisplayName("Duration")]
        [Required(ErrorMessage = "Duration is required")]
        public int Duration { get; set; }
        public byte InvestorID { get; set; }
        public byte TransType { get; set; }
        [GlobalizedDisplayName("LoanInstallment")]
        [Required(ErrorMessage = "Loan Installment is required")]
        public decimal LoanInstallment { get; set; }
        [GlobalizedDisplayName("IntInstallment")]
        [Required(ErrorMessage = "Interest Installment is required")]
        public decimal IntInstallment { get; set; }
        [GlobalizedDisplayName("InterestRate")]
        [Required(ErrorMessage = "Interest Rate is required")]
        public decimal InterestRate { get; set; }
        public string BankName { get; set; }
        public string ChequeNo { get; set; }
        public bool? IsApproved { get; set; }
        public string DisburseDate { get; set; }
        public string LoanCloseDate { get; set; }
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

        public byte? FinalDisbursement { get; set; }
        [Display(Name = "Disbursement Type")]
        public byte? DisbursementType { get; set; }
        public string InstallmentStartDate { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "Total Paid Amount")]
        public decimal? TotalAmount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FirstInstallmentDate { get; set; }
        public string InterestCalMethod { get; set; }
       
        public IEnumerable<SelectListItem> MemberPassBookNOListItems { get; set; }
        public IEnumerable<SelectListItem> productListItems { get; set; }
        public IEnumerable<SelectListItem> investorListItems { get; set; }
        public IEnumerable<SelectListItem> centerListItems { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
        public IEnumerable<SelectListItem> membercategoryListItems { get; set; }
        public IEnumerable<SelectListItem> purposeListItems { get; set; }
        public IEnumerable<SelectListItem> memberListItems { get; set; }
        public string memName { get; set; }
        public string vMaxLoanTerm { get; set; }
        public IEnumerable<ProductViewModel> MemberProductItemsSelected { get; set; }
        public IEnumerable<SelectListItem> paymentMode { get; set; }
       
        public IEnumerable<SelectListItem> disType { get; set; }
        public IEnumerable<SelectListItem> PDis { get; set; }
        public IEnumerable<SelectListItem> AccListListItems { get; set; }
    }
}