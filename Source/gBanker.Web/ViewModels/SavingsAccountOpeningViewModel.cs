using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class SavingsAccountOpeningViewModel : BaseModel

    {
        public long SavingSummaryID { get; set; }
         [Display(Name = "Office")]
         [Required(ErrorMessage = "Office Code is required")]
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
         [Display(Name = "Member")]
         [Required(ErrorMessage = "Member Code is required")]
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        [Display(Name = "Product")]
        [Required(ErrorMessage = "Product Code is required")]
        public short ProductID { get; set; }
        public string ProductCode { get; set; }
        [Display(Name = "Samity")]
        [Required(ErrorMessage = "Center Code is required")]
        public int CenterID { get; set; }
        [Display(Name = "Samity Code")]
        public string CenterCode { get; set; }
        [Display(Name = "NoOf Account")]
        [Required(ErrorMessage = "Account No. is required")]
        public int NoOfAccount { get; set; }
        [Display(Name = "Interest Rate")]
        [Required(ErrorMessage = "Interest Rate is required")]
        public decimal InterestRate { get; set; }
        [Display(Name = "Savings Installment")]
        [Required(ErrorMessage = "Savings Installment is required")]
        public decimal SavingInstallment { get; set; }
         [Display(Name = "Opening Date")]
         [Required(ErrorMessage = "Opening Date is required")]
        public System.DateTime OpeningDate { get; set; }
         [Display(Name = "Matured Date")]
        public Nullable<System.DateTime> MaturedDate { get; set; }
        public string SavingAccountNo { get; set; }

        public int Duration { get; set; }
        public int InstallmentNo { get; set; }
        [Display(Name = "Ref. Employee")]
        public int? Ref_EmployeeID { get; set; }
        public IEnumerable<SelectListItem> productListItems { get; set; }
        public IEnumerable<SelectListItem> centerListItems { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
        public IEnumerable<SelectListItem> memberListItems { get; set; }


        public int? MemberNomineeId { get; set; }

    }
}