using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels.OLRSHubs.OLRSHubPayloads
{
    public class PRA_MN_IMP_COST_MASTER_Model
    {
        public string PO_CODE { get; set; }       
        public string MNYR { get; set; }
        public decimal? MARKET_RATE { get; set; }
        public decimal? INF_RATE { get; set; }       
        public Int16 FY_MONTH { get; set; }
        public decimal? IMP_FS_NPK { get; set; }
        public decimal? IMP_FS_PK { get; set; }
        public decimal? IMP_FS_TOTAL { get; set; }
        public decimal? IMP_FSI_NPK { get; set; }
        public decimal? IMP_FSI_PK { get; set; }
        public decimal? IMP_FSI_TOTAL { get; set; }       
        public string INS_USER { get; set; }       
        public DateTime? INS_DATE { get; set; }
        public string UPD_USER { get; set; }
        public DateTime? UPD_DATE { get; set; }
        public decimal? CAL_RATE { get; set; }
        public string POSTING_FLAG { get; set; }
        public DateTime? STATUS_DATE { get; set; }
        public string INS_DATE_IN_TEXT { get; set; }
        public string STATUS_DATE_IN_TEXT { get; set; }
    }
}
