using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels.OLRSHubs.OLRSHubPayloads
{
    public class PRA_MN_INF_EQUITY_Model
    {
        public string PO_CODE { get; set; }
        public string MNYR { get; set; }
        public decimal NPK_LAST_YR { get; set; }
        public decimal PK_LAST_YR { get; set; }
        public decimal NPK_THIS_MONTH { get; set; }
        public decimal PK_THIS_MONTH { get; set; }
        public decimal NPK_AVG { get; set; }
        public decimal PK_AVG { get; set; }
        public decimal NPK_INF_ADJ { get; set; }
        public decimal PK_INF_ADJ { get; set; }
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
