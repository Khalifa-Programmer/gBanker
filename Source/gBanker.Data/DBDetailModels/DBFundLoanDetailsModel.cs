using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
    public class DBFundLoanDetailsModel
    {
        public string FundLoanCode { get; set; }
        public int AccID { get; set; }
        public decimal PrincipalAmount { get; set; }
        public int LoanSanctionNo { get; set; }
        public int LoanSanctionTerm { get; set; }
        public DateTime LoanSanctionApproveDate { get; set; }
        public DateTime LoanDisbursementDate { get; set; }
        public decimal DisbursedAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int LoanDuration { get; set; }
        public int GracePeriod { get; set; }
        public int TotalInstallmentNo { get; set; }
    }
}
