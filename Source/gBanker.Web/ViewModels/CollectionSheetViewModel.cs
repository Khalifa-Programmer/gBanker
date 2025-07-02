using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gBanker.Web.ViewModels
{
    public class CollectionSheetViewModel
    {
        public long SummaryID { get; set; }
        [Display(Name = "Office")]
        public int OfficeID { get; set; }
        [Display(Name = "Member")]
        public long MemberID { get; set; }
        public int ProductId { get; set; }

        public long Id { get; set; }

        public Int64? TrxID { get; set; }

        public decimal DurationOverLoanDue { get; set; }

        public string TransType { get; set; }
        public string nType { get; set; }

        public Int32? Duration { get; set; }

        public decimal LoadPaid { get; set; }
        public decimal Balance { get; set; }
        public decimal DurationOverIntDue { get; set; }

        [Display(Name = "Center Code")]
        public string CenterCode { get; set; }

        [Display(Name = "Member Code")]
        public string MemberCode { get; set; }

        [Display(Name = "Member Name")]
        public string MemberName { get; set; }
        [Display(Name = "Product")]
        public short ProductID { get; set; }

        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Display(Name = "Samity")]
        public int CenterID { get; set; }
        [Display(Name = "Loan Due")]
        public decimal LoanDue { get; set; }

        [Display(Name = "Total Paid")]
        public decimal TotalPaid { get; set; }

        [Display(Name = "Loan Paid")]
        public decimal LoanPaid { get; set; }
        public decimal Deposit { get; set; }
        public decimal IntDue { get; set; }
        public decimal WithDrawal { get; set; }

        [Display(Name = "Interest Paid")]
        public decimal IntPaid { get; set; }

        [Column(TypeName = "numeric")]
        public decimal DueSavingInstallment { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SavingInstallment { get; set; }
        public string CollectionStatus { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Withdrawal { get; set; }

        [Column(TypeName = "numeric")]
        // public decimal Balance { get; set; }
        public decimal TotalDue { get; set; }
        public Int32? installmentNo { get; set; }
        public bool IsLoanProduct { get; set; }
        public decimal GSavings { get; set; }
        public decimal VS { get; set; }
        public decimal SSP { get; set; }
        public decimal PrincipalLoan { get; set; }
        public decimal LoanRepaid { get; set; }
        public decimal CumIntCharge { get; set; }
        public decimal CumIntPaid { get; set; }
        public decimal Recoverable
        {
            get
            {
                decimal recoverable = 0;
                switch (nType)
                {
                    case "L":
                        recoverable= LoanDue+IntDue;
                        break;

                    case "G":
                        recoverable= DueSavingInstallment;
                        break;

                    case "SSP":
                        recoverable= DueSavingInstallment;
                        break;

                    case "VS":
                        recoverable = DueSavingInstallment;
                        break;
                }

                return recoverable;
            }
        }


        //public decimal PrincipalLoan { get; set; }



        public string AccountNo { get; set; }



        //public int TotalCount { get; set; }        

        // custom properties
        public decimal Total
        {
            get
            {
                decimal total = 0;
                total = LoanPaid + IntPaid + GSavings + SSP + VS;
                return total;
            }
        }

        public decimal TotalLoanPaid
        {
            get
            {
                decimal total = 0;
                total = LoanPaid + IntPaid;
                return total;
            }
        }

        public decimal DefaultLoanPaid
        {
            get
            {
                decimal total = 0;
                if (nType == "L")
                {
                    total = CollectionStatus == "YES" ? LoanPaid + IntPaid : 
                            CollectionStatus == "NO" && (LoanPaid + IntPaid) >0 ? (LoanPaid + IntPaid) :  Recoverable;
                }
                else if (nType == "G")
                {
                    total = CollectionStatus == "YES" ? GSavings:
                        CollectionStatus == "NO" && GSavings > 0 ? GSavings : Recoverable;
                }
                else if (nType == "SSP")
                {
                    total = CollectionStatus == "YES" ? SSP:
                        CollectionStatus == "NO" && SSP > 0 ? SSP : Recoverable;
                }
                else if (nType == "VS")
                {
                    total = CollectionStatus == "YES" ? VS :
                         CollectionStatus == "NO" && VS > 0 ? VS : Recoverable; ;
                }
                return total;
            }
        }


        public IEnumerable<SelectListItem> CenterListItems { get; set; }
        public IEnumerable<SelectListItem> memberListItems { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
        public int? LoggedInOrganizationID { get; set; }
    }

    public class CollectionSheetListViewModel
    {
        public List<CollectionSheetViewModel> CollectionSheets { get; set; }
    }

    public class CollectionSheetStatusViewModel
    {
        public List<CSTrxIdXTransTypeViewModel> CSTrxIdXTransTypes { get; set; }
    }

    public class CSTrxIdXTransTypeViewModel
    {
        
        public int TrxId { get; set; }
        public int MemberID { get; set; }
        public string TransType { get; set; }
        public decimal TotalLPaid { get; set; }
        public decimal LoanPaid { get; set; }
        public decimal IntPaid { get; set; }
        public decimal GSavings { get; set; }
        public decimal SSP { get; set; }
        public decimal VS { get; set; }
        public string nType { get; set; }
    }
}