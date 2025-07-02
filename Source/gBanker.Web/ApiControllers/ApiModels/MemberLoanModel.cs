using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Web.ApiControllers.ApiModels
{
    public class MemberLoanModel
    {
        public long MemberID { get; set; }
        public string MemberCode
        {
            get; set;
        }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int ProductType { get; set; }
        public decimal LoanRecovery { get; set; }
        public decimal Recoverable { get; set; }
        public decimal Balance { get; set; }
        public int InstallmentNo { get; set; }
       

    }
}
