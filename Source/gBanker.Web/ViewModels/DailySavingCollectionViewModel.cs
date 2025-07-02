using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class DailySavingCollectionViewModel : BaseModel
    {

        public long DailySavingTrxID { get; set; }

        public long SavingSummaryID { get; set; }

        public int OfficeID { get; set; }
         [Required(ErrorMessage = "Member is required")]
        public long MemberID { get; set; }

        [StringLength(20)]
        public string MemberCode { get; set; }

        [StringLength(110)]
        public string MemberName { get; set; }
         [Required(ErrorMessage = "Product is required")]
        public short ProductID { get; set; }

        [StringLength(10)]
        public string ProductCode { get; set; }

        [StringLength(50)]
        public string ProductName { get; set; }
         [Required(ErrorMessage = "Samity is required")]
        public int CenterID { get; set; }
         [Required(ErrorMessage = "Account No. is required")]
        public int NoOfAccount { get; set; }

        [Column(TypeName = "date")]
        [Required(ErrorMessage = "Transaction Date is required")]
        public DateTime TransactionDate { get; set; }
        public string TrxDateMsg { get; set; }
        [Column(TypeName = "numeric")]
        public decimal DueSavingInstallment { get; set; }

        [Column(TypeName = "numeric")]
        public decimal SavingInstallment { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Deposit { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Withdrawal { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Balance { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Penalty { get; set; }

        public byte TransType { get; set; }

        [Column(TypeName = "numeric")]
        public decimal MonthlyInterest { get; set; }

        public bool PresenceInd { get; set; }

        [Column(TypeName = "numeric")]
        public decimal TransferDeposit { get; set; }

        [Column(TypeName = "numeric")]
        public decimal TransferWithdrawal { get; set; }
        public int OrgID { get; set; }
        public long IsActive { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? InActiveDate { get; set; }
        public string SubMainCategory { get; set; }

        public string SavingAccountNo { get; set; }

        public IEnumerable<SelectListItem> centerListItems { get; set; }
        public IEnumerable<SelectListItem> productListItems { get; set; }
        public IEnumerable<SelectListItem> cashListItems { get; set; }
      
        public IEnumerable<SelectListItem> officeListItems { get; set; }
        public IEnumerable<SelectListItem> membercategoryListItems { get; set; }
        public IEnumerable<SelectListItem> purposeListItems { get; set; }
        public IEnumerable<SelectListItem> memberListItems { get; set; }
        public string memName { get; set; }
        public string vMaxLoanTerm { get; set; }
        public IEnumerable<ProductViewModel> MemberProductItemsSelected { get; set; }
        public decimal DueSavingSummary { get; set; }
        public decimal SavingCollectionSummary { get; set; }
      
        public decimal WithDrawalSummary { get; set; }
        public decimal PenaltySummary { get; set; }
        public int rowSl { get; set; }
       
    }
}