using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace gBanker.Web.ViewModels
{
    public class TodaysSummaryViewModel : BaseModel
    {
        public Nullable<int> OfficeID { get; set; }
        public string OfficeName { get; set; }
        public Nullable<short> ProductID { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> InvestorID { get; set; }
        public Nullable<decimal> OpeningBalance { get; set; }
        public Nullable<decimal> DueInstallment { get; set; }
        public Nullable<decimal> LoanRepaid { get; set; }
        public Nullable<decimal> SavingsDeposit { get; set; }
        public Nullable<decimal> TaxDeposit { get; set; }
        public Nullable<decimal> LoanDisburse { get; set; }
        public Nullable<decimal> SavingsWithdrawal { get; set; }
        public Nullable<decimal> ClosingBalance { get; set; }
        public Nullable<decimal> InterestBalance { get; set; }
        public Nullable<decimal> DueInterestInstallment { get; set; }
        public Nullable<decimal> CurrentInterestPaid { get; set; }
        public Nullable<decimal> PreviousInerestPaid { get; set; }
        public Nullable<decimal> InterestClosingBalance { get; set; }
        public Nullable<int> RegularLoanee { get; set; }
        public Nullable<int> DropLoanee { get; set; }
        public Nullable<int> TokenLoanee { get; set; }
        public Nullable<int> Member { get; set; }
        public decimal GuranterOpBalance { get; set; }
        public decimal GuranterDueSerCharge { get; set; }
        public decimal GuranterPaidSerCharge { get; set; }
        public decimal GuranterClBalance { get; set; }
        public decimal Penalty { get; set; }
        public Nullable<int> Absent { get; set; }
        public decimal BankInterest { get; set; }
        public decimal AddCharge { get; set; }
        public int NonDepositer { get; set; }
        public DateTime? BusinessDate { get; set; }
        public IEnumerable<SelectListItem> officeListItems { get; set; }
    }
}