using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class LoanCorrectionViewModel : BaseModel
    {
        public long LoanCorrectionTrxID { get; set; }

      //  [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
         [Required(ErrorMessage = "Transaction Date is required")]
        public DateTime TrxDate { get; set; }
      

        public long LoanSummaryID { get; set; }
         [Required(ErrorMessage = "Office is required")]
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
         [Required(ErrorMessage = "Member is required")]
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
         [Required(ErrorMessage = "Product is required")]
        public short ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string InterestCalculationMethod { get; set; }
        [Display(Name = "Samity ID")]
         [Required(ErrorMessage = "Samity is required")]
        public int CenterID { get; set; }
        [Display(Name = "Samity Code")]
        public string CenterCode { get; set; }

        public byte MemberCategoryID { get; set; }

        public int LoanTerm { get; set; }

        public short PurposeID { get; set; }

        public DateTime InstallmentDate { get; set; }

        public decimal PrincipalLoan { get; set; }

        public decimal LoanRepaid { get; set; }

        public decimal LoanDue { get; set; }

        public decimal LoanPaid { get; set; }

        public decimal CumIntCharge { get; set; }

        public decimal IntCharge { get; set; }

        public decimal IntDue { get; set; }

        public decimal IntPaid { get; set; }

        public decimal Advance { get; set; }

        public decimal DueRecovery { get; set; }

        public byte TrxType { get; set; }

        public short InstallmentNo { get; set; }

        public short EmployeeID { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? InActiveDate { get; set; }


       
        public decimal TotalPaid { get; set; }


        public IEnumerable<SelectListItem> cashListItems { get; set; }
        public IEnumerable<SelectListItem> productListItems { get; set; }

        public IEnumerable<SelectListItem> centerListItems { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
        public IEnumerable<SelectListItem> membercategoryListItems { get; set; }
        public IEnumerable<SelectListItem> purposeListItems { get; set; }
        public IEnumerable<SelectListItem> memberListItems { get; set; }
        public string memName { get; set; }
        public string vMaxLoanTerm { get; set; }
        public IEnumerable<ProductViewModel> MemberProductItemsSelected { get; set; }
    }
}