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
    public class MembersLoanBalanceViewModel : BaseModel
    {
        [Required(ErrorMessage = "Member Category is required.")]
        [Display(Name = "Member Category")]
        public byte MemberCategoryID { get; set; }


        [Required(ErrorMessage = "Main Product Code is required.")]

        [Display(Name = "Main Product Code")]
         public short ProductID { get; set; }
        public long LoanBalanceID { get; set; }


        [Display(Name = "General Savings")]
        [Required(ErrorMessage = "General Savings is required")]
        public decimal GS { get; set; }

        [Display(Name = "Voluntary Savings")]
        [Required(ErrorMessage = "Voluntary Savings is required")]
        public decimal VS { get; set; }


        [Display(Name = "LTS")]
        [Required(ErrorMessage = "LTS is required")]
        public decimal LTS { get; set; }

        [Display(Name = "Loan Balance")]
        [Required(ErrorMessage = "Loan Balance is required")]
        public decimal LoanBalance { get; set; }

        [Display(Name = "SC Balance")]
        [Required(ErrorMessage = "SC Balance is required")]
        public decimal SCBalance { get; set; }


        [Display(Name = "PassBook Entry Date")]
        [Required(ErrorMessage = "PassBook Entry Date is required")]
        public DateTime? PassBookEntryDate { get; set; }



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

        // [GlobalizedDisplayName("Main Product Code")]
        //[Display(Name = "Main Product Code")]
        //[Required(ErrorMessage = "Product is required")]
        //public short ProductID { get; set; }
        [Required(ErrorMessage = "Samity is required")]
        public int CenterID { get; set; }
        [GlobalizedDisplayName("LoanTerm")]
        [Required(ErrorMessage = "Loan Term is required")]
        public byte? LoanTerm { get; set; }
        [GlobalizedDisplayName("PurposeID")]
        [Required(ErrorMessage = "Purpose is required")]
        public short? PurposeID { get; set; }
        public string LoanNo { get; set; }
        //[GlobalizedDisplayName("PrincipalLoan")]
        [Display(Name = "Applied Amount(Principal Loan)")]
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
        [Column(TypeName = "numeric")]
        public decimal? PartialIntCharge { get; set; }
        [NotMapped]
        public int? InstallmentNo { get; set; }

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
         [NotMapped]
        [Display(Name = "Frequency Mode")]
        public string frequencyMode { get; set; }
        public string LoanAccountNo { get; set; }
        [StringLength(65)]
        [Display(Name = "Bank Name")]
        public string SecurityBankName { get; set; }
        [StringLength(65)]
        [Display(Name = "Branch Name")]
        public string SecurityBankBranchName { get; set; }
        [StringLength(50)]
        [Display(Name = "Check No")]
        public string SecurityBankCheckNo { get; set; }
        public string PurposeName { get; set; }
        public string MyGuid {
            get; set;
        }
        public string MainSector { get; set; }
        public string MainSectorName { get; set; }

        public IEnumerable<SelectListItem> MemberCategoryList { get; set; }

        public IEnumerable<SelectListItem> MainProductList { get; set; }
        public IEnumerable<SelectListItem> MainSectorList { get; set; }
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
        public IEnumerable<SelectListItem> freqMode { get; set; }

        public int?   txtMaleFullTimeP1  { get; set; }
        public int?   txtFeMaleFullTimeP1{ get; set; }
        public int?   txtMalePartTimeP1  { get; set; }
        public int?   txtFeMalePartTimeP1{ get; set; }
        public int?   txtMaleFullTimeP2  { get; set; }
        public int?   txtFeMaleFullTimeP2{ get; set; }
        public int?   txtMalePartTimeP2  { get; set; }
        public int?   txtFeMalePartTimeP2{ get; set; }
        public int?   txtMaleFullTimeP3  { get; set; }
        public int?   txtFeMaleFullTimeP3{ get; set; }
        public int?   txtMalePartTimeP3  { get; set; }
        public int?   txtFeMalePartTimeP3 { get; set; }






    }


    public class MembersLoanBalanceKendoViewModel : BaseModel
    {
        public long MembersLoanBalanceID { get; set; }

        public int OfficeID { get; set; }

        public long MemberID { get; set; }   
        
        public string MembersInfo { get; set; }

        public int CenterID { get; set; }

        public int MembersCategoryID { get; set; }

        public decimal? GeneralSavings { get; set; }

        public decimal? VoluntarySavings { get; set; }

        public decimal? LTS { get; set; }

        public decimal? LoanBalance { get; set; }

        public decimal? SCbalance { get; set; }

        public string PassBookEntryDate { get; set; }

    
        public bool IsActive { get; set; }    

        public long? rowSl { get; set; }

        public string CreateUser { get; set; }

        public string CreateDate { get; set; }

        public string UpdateUser { get; set; }

        public string UpdateDate { get; set; }
    }
}