using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class MemberwiseProductAndLoanTermViewModel
    {
        public String ProductID { get; set; }
        public String ProductName { get; set; }
        public String LoanTerm { get; set; }
        public long LoanSummaryID { get; set; }
        public long LoanSummaryIDPre { get; set; }
        public decimal LoanPaid { get; set; }
        public decimal IntPaid { get; set; }
        public decimal PrincipalLoan { get; set; }
        public decimal LoanRepaid { get; set; }
        public String InstallmentDate { get; set; }
        public String noAccount { get; set; }




    }
}



