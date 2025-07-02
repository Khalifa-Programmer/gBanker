using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class LoanSavingsViewModel
    {
        public List<LoanModel> loanArray { get; set; }
        public List<SavingsModel> savingsArray { get; set; }
    }

    public class LoanModel
    {
        public int LoanProductId { get; set; }
        public decimal LoanINstallment { get; set; }
        public decimal InterestInstallment { get; set; }
        public int LoanSummaryId { get; set; }
        public int LoanTerm { get; set; }
        
    }

    public class SavingsModel
    {
        public int CenterID { get; set; }
        public int MemberId { get; set; }
        public int SavingProductId { get; set; }
        public int NoOfAccount { get; set; }
        public decimal SavingInstallment { get; set; }
        public decimal WithdrawalAmount { get; set; }
        public decimal BalanceAmount { get; set; }
    }




}// End Namespace