using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels.OLRSHubs.OLRSHubPayloads
{
    public class PRA_MN_RPT_TAB_XL_FD_Model
    {
        public string PO_CODE { get; set; }
       
        public string MNYR { get; set; }
      
        public string IND_CODE { get; set; }
       
        public string M_F_FLAG { get; set; }

        public decimal? FD_PKSF_FUND { get; set; }

        public decimal? FD_NON_PKSF_FUND { get; set; }

        public decimal? FD_TOTAL_FUND { get; set; }

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
