using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels
{
    public class FinancialDataModel
    {

        public string POCode { get; set; }

        public string Ind_code { get; set; }

        public string MNYR { get; set; }

        public decimal FD_PKSF_FUND { get; set; }
        public int CreatedBy { get; set; }
    }
}
