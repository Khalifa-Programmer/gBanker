using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels.OLRSHubs.OLRSHubPayloads
{
    public class PRA_MN_IMP_COST_SV_INT_Model
    {
        public string PO_CODE { get; set; }        
        public string MNYR { get; set; }        
        public decimal INT_RATE { get; set; }
        public decimal? NPK { get; set; }
        public decimal? REG { get; set; }
        public decimal? VOL { get; set; }
        public decimal? OTHER { get; set; }
        public decimal? TOTAL_PKSF { get; set; }
        public decimal? CAL_NPK { get; set; }
        public decimal? CAL_REG { get; set; }
        public decimal? CAL_VOL { get; set; }
        public decimal? CAL_OTHER { get; set; }
        public decimal? CAL_TOTAL_PKSF { get; set; }
        public string INS_USER { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string UPD_USER { get; set; }
        public DateTime? UPD_DATE { get; set; }
        public string POSTING_FLAG { get; set; }
        public DateTime? STATUS_DATE { get; set; }
    }
}
