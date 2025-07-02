using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace gBanker.Web.ViewModels
{
    public class CumMISViewModel:BaseModel
    {
        public int CumMisID { get; set; }
        [Display(Name = "MIS Date")]
        [Column(TypeName = "smalldatetime")]
        public DateTime MisDate { get; set; }
         [Display(Name = "Office")]
        public int OfficeID { get; set; }
         [Display(Name = "Office Code")]
         public String OfficeCode { get; set; }
         [Display(Name = "Center Code")]
         public String CenterCode { get; set; }
         [Display(Name = "Product Code")]
         public String ProductCode { get; set; }
         [Display(Name = "Center")]
        public int CenterID { get; set; }
         [Display(Name = "Product")]
        public int ProductID { get; set; }
         [Display(Name = "Investor")]
        public int InvestorID { get; set; }

        [Required]
        [StringLength(2)]
        public string Gender { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "No.Of Loanee")]
        public decimal? NoOfLoanee { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "No.Of Loan")]
        public decimal? UpToLoanDis { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "UptoDisbursement")]
        public decimal? UptoDisburseMent { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "UpToRecovery")]
        public decimal? UpToRecovery { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "UptoAdmission")]
        public decimal? UptoAdmission { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "UpToDropOut")]
        public decimal? UpToDropOut { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "UptoFullyRepaid")]
        public decimal? UptoFullyRepaid { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "UptoDeposit")]
        public decimal? UptoDeposit { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "UptoInterest")]
        public decimal? UptoInterest { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "uptowithdrawal")]
        public decimal? uptowithdrawal { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "WriteOffLoan")]
        public decimal? WriteOffLoan { get; set; }

        [Column(TypeName = "numeric")]
        [Display(Name = "WriteOffInterest")]
        public decimal? WriteOffInterest { get; set; }
        [Display(Name = "ReportItem")]
        public int? CumMisItemID { get; set; }
        [Display(Name = "ReportItemName")]
        public string CumMisItemName { get; set; }

        public IEnumerable<SelectListItem> OfficeList { get; set; }
        public IEnumerable<SelectListItem> productListItems { get; set; }
        public IEnumerable<SelectListItem> investorListItems { get; set; }
        public IEnumerable<SelectListItem> centerListItems { get; set; }
        public IEnumerable<SelectListItem> GenderList { get; set; }
        public IEnumerable<SelectListItem> CumMisItemList { get; set; }
    }
}