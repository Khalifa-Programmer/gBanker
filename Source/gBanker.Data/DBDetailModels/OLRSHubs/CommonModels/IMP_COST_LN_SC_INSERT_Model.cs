using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels.OLRSHubs.CommonModels
{
    public class IMP_COST_LN_SC_INSERT_Model
    {
        public string PO_CODE { get; set; }
        public string MNYR { get; set; }
        public string Sc_Rate { get; set; }
        public string LoanCode { get; set; }
        public decimal LoanServiceAmount { get; set; }
        public string INS_USER { get; set; }
    }
}
