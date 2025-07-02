using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels.OLRSHubs.CommonModels
{
    public class ProgramDataManualDataModel
    {
        public string OrganizationCode { get; set; }
        public string MNYR { get; set; }
        public string IndCode { get; set; }
        public string LoanCode { get; set; }
        public string M_F_FLAG { get; set; }
        public decimal Amount { get; set; }
        public string InsertUser { get; set; }
    }

    public class POLoanCodeWithProductModel
    {
        public string LoanCode { get; set; }
        public string ProductName { get; set; }
    }
}
