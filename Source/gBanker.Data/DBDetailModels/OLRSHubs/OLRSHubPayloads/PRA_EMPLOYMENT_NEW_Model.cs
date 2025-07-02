using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels.OLRSHubs.OLRSHubPayloads
{
    public class PRA_EMPLOYMENT_NEW_Model
    {
        public string PO_CODE { get; set; }

        public string MNYR { get; set; }

        public string LOAN_CODE { get; set; }

        public decimal? SELF_FULL_M { get; set; }

        public decimal? SELF_FULL_F { get; set; }

        public decimal? SELF_PART_M { get; set; }

        public decimal? SELF_PART_F { get; set; }

        public decimal? WAGE_FULL_M { get; set; }

        public decimal? WAGE_FULL_F { get; set; }

        public decimal? WAGE_PART_M { get; set; }

        public decimal? WAGE_PART_F { get; set; }

        public string INS_USER { get; set; }

        public DateTime? INS_DATE { get; set; }

        public string UPD_USER { get; set; }

        public DateTime? UPD_DATE { get; set; }

        public string POSTING_FLAG { get; set; }

        public DateTime? STATUS_DATE { get; set; }
                
        public string INS_DATE_IN_TEXT { get; set; }
                
        public string STATUS_DATE_IN_TEXT { get; set; }
    }
}
