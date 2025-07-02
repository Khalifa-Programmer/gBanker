using gBanker.Web.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class ProductViewModel : BaseModel
    {
        public int Sl { get; set; }
        public Int16 MyProductID { get; set; }
        public ProductViewModel()
        {

            LateFeePercentage = 0;
        }
        [GlobalizedDisplayName("ProductID")]
        public short ProductID { get; set; }
        [StringLength(10)]
        [Required(ErrorMessage = "Product Code is required")]
        [GlobalizedDisplayName("ProductCode")]
        public string ProductCode { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Product Name is required")]
        [GlobalizedDisplayName("ProductName")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [StringLength(100)]
        [GlobalizedDisplayName("ProductFullNameEng")]
        [Display(Name = "Product Full Name")]
        public string ProductFullNameEng { get; set; }
        [StringLength(10)]
        [GlobalizedDisplayName("ProductShortNameBng")]
        [Display(Name = "Product Short Name")]
        public string ProductShortNameBng { get; set; }
        [StringLength(100)]
        [GlobalizedDisplayName("ProductFullNameBng")]
        [Display(Name = "Product Name(Unicode)")]
        public string ProductFullNameBng { get; set; }
        [Required(ErrorMessage = "Product Type is required")]
        [GlobalizedDisplayName("ProductType")]
        public byte? ProductType { get; set; }
        [Required(ErrorMessage = "Interest Rate is required")]

        [GlobalizedDisplayName("InterestRate")]
        public Nullable<decimal> InterestRate { get; set; }
        [Required(ErrorMessage = "Duration is required")]
        [GlobalizedDisplayName("Duration")]
        public Nullable<short> Duration { get; set; }
        [GlobalizedDisplayName("MainProductCode")]
        [Required(ErrorMessage = "Main ProductCode is required")]
        public string MainProductCode { get; set; }
        //[GlobalizedDisplayName("MainProductName")]
        public string MainItemName { get; set; }

        [StringLength(50)]
        [Display(Name = "Sub Main Category")]
        public string SubMainCategory { get; set; }
        
        [Display(Name = "Late Fee Percentage")]
        public decimal LateFeePercentage { get; set; }

        [Display(Name = "Next Disbursement Duration")]
        public byte? NextDisbursementDuration { get; set; }

        [Display(Name = "Yearly Interest Payable")]
        public byte? YearlyInterestPayable { get; set; }

        public IEnumerable<SelectListItem> YearlyInterestPayable_List { get; set; }




        [Required(ErrorMessage = "Loan Installment is required")]
        //[Range(0, 9999999999999999.99999)]
        //[DisplayFormat(DataFormatString = "{9999999999999999.99999}", ApplyFormatInEditMode = true)]

        [DisplayFormat(DataFormatString = "{0:0.0000000}")]
        [GlobalizedDisplayName("LoanInstallment")]
        public decimal? LoanInstallment { get; set; }
        [Required(ErrorMessage = "Interest Installment is required")]
        [DisplayFormat(DataFormatString = "{0:0.0000000}")]
        //[Range(0, 999999.99999)]
        //[DisplayFormat(DataFormatString = "{9999999999999999.99999}", ApplyFormatInEditMode = true)]
        [GlobalizedDisplayName("InterestInstallment")]
        public decimal? InterestInstallment { get; set; }
        [Required(ErrorMessage = "Savings Installment is required")]
        [GlobalizedDisplayName("SavingsInstallment")]
        public decimal SavingsInstallment { get; set; }
        [Required(ErrorMessage = "Minimun Limit is required")]
        [GlobalizedDisplayName("MinLimit")]
        public Nullable<decimal> MinLimit { get; set; }
        [Required(ErrorMessage = "Maximun Limit is required")]
        [GlobalizedDisplayName("MaxLimit")]
        public Nullable<decimal> MaxLimit { get; set; }
        [Required(ErrorMessage = "Interest Calculation Method is required")]
        [GlobalizedDisplayName("InterestCalculationMethod")]
        public string InterestCalculationMethod { get; set; }
        [Required(ErrorMessage = "Payment Frequency is required")]
        [GlobalizedDisplayName("PaymentFrequency")]
        public string PaymentFrequency { get; set; }
        [Required(ErrorMessage = "Insurance ItemCode is required")]
        [GlobalizedDisplayName("InsuranceItemCode")]
        public string InsuranceItemCode { get; set; }
        [Required(ErrorMessage = "Insurance Rate is required")]
        [GlobalizedDisplayName("InsuranceItemRate")]
        public decimal InsuranceItemRate { get; set; }
        [GlobalizedDisplayName("GracePeriod")]
        public int GracePeriod { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> InActiveDate { get; set; }

      
        public bool IsSelected { get; set; }

        public IEnumerable<SelectListItem> PInvestorListItems { get; set; }
        public IEnumerable<SelectListItem> PFrequencyListItems { get; set; }
        public IEnumerable<SelectListItem> PCalcuationMethodListItems { get; set; }
        public List<SelectionViewModel> MemberCategoryList { get; set; }
    }
}