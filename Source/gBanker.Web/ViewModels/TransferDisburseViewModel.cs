using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class TransferDisburseViewModel: BaseModel
    {
        public long LoanSummaryID { get; set; }
        [Display(Name = "Office")]
        public int OfficeID { get; set; }
        public string OfficeCode { get; set; }
        public string OfficeName { get; set; }
        [Display(Name = "Member")]
        public long MemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberName { get; set; }
        public byte MemberCategoryID { get; set; }
        [Display(Name = "Product Code")]
        public short ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        [Display(Name = "Tr. Product Code")]
        public short TrProductID { get; set; }
        [Display(Name = "Samity")]
        public int CenterID { get; set; }
        [Display(Name = "Samity Code")]
        public string CenterCode { get; set; }
        [Display(Name = "Samity Name")]
        public string CenterName { get; set; }
        [Display(Name = "Loan Term")]
        public byte LoanTerm { get; set; }
        [Display(Name = "Purpose")]
        public short PurposeID { get; set; }
        public string PurposeName { get; set; }
        [Display(Name = "Principal Loan")]
        public decimal PrincipalLoan { get; set; }
        public System.DateTime ApproveDate { get; set; }

        [Display(Name = "Disburse Date")]
        public Nullable<System.DateTime> DisburseDate { get; set; }
        public int Duration { get; set; }

        [Display(Name = "Loan Installment")]
        public decimal LoanInstallment { get; set; }

        [Display(Name = "Interest Installment")]
        public decimal IntInstallment { get; set; }
        public decimal InterestRate { get; set; }

        [Display(Name = "Installment Start Date")]
        public Nullable<System.DateTime> InstallmentStartDate { get; set; }
        public System.DateTime InstallmentDate { get; set; }
        public byte TransType { get; set; }
        public byte LoanStatus { get; set; }      


        public decimal LoanRepaid { get; set; }

        [Display(Name = "Interest Charge")]
        public decimal IntCharge { get; set; }

        [Display(Name = "Interest Paid")]
        public decimal IntPaid { get; set; }


        public int InstallmentNo { get; set; }
        public int DropInstallment { get; set; }
        public int Holidays { get; set; }
        public short ContinuousDrop { get; set; }
        public decimal Balance { get; set; }
        public decimal Advance { get; set; }
        public decimal DueRecovery { get; set; }
        public Nullable<System.DateTime> LoanCloseDate { get; set; }
        public Nullable<System.DateTime> OverdueDate { get; set; }

        public decimal ExcessPay { get; set; }
        public Nullable<decimal> CurLoan { get; set; }
        public Nullable<decimal> PreLoan { get; set; }
        public Nullable<decimal> CumLoanDue { get; set; }
        public Nullable<decimal> WriteOffLoan { get; set; }
        public Nullable<decimal> WriteOffInterest { get; set; }

        public IEnumerable<SelectListItem> officeList { get; set; }
        public IEnumerable<SelectListItem> productList { get; set; }
        public IEnumerable<SelectListItem> trProductList { get; set; }
        public IEnumerable<SelectListItem> centerList { get; set; }
        public IEnumerable<SelectListItem> purposeList { get; set; }
        public IEnumerable<SelectListItem> memberList { get; set; }
        public IEnumerable<SelectListItem> memberCategoryList { get; set; }

    }
}