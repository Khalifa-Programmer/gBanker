using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels.OLRSHubs.OLRSHubPayloads
{
    public class PO_MONTHLY_ACC_BAL_Model
    {
        public string PO_CODE { get; set; }        
        public string COMPANY_CODE { get; set; }        
        public string COMPANY_BRANCH_CODE { get; set; }        
        public string FINANCE_CODE { get; set; }        
        public string PROJECT_CODE { get; set; }        
        public string COMPONENT_CODE { get; set; }        
        public string MNYR { get; set; }        
        public string COA_ID { get; set; }        
        public string L1_CODE { get; set; }        
        public string L2_CODE { get; set; }        
        public string L3_CODE { get; set; }        
        public string L4_CODE { get; set; }        
        public string L5_CODE { get; set; }
        public string ACCTYPE { get; set; }
        public string ACCGROUP { get; set; }
        public decimal? BAL_DR { get; set; }
        public decimal? BAL_CR { get; set; }
        public decimal? CUM_BAL_DR { get; set; }
        public decimal? CUM_BAL_CR { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_USER { get; set; }
        public DateTime? UPD_DATE { get; set; }
        public string UPD_USER { get; set; }
        public decimal? THIS_MONTH_CASH { get; set; }
        public decimal? THIS_FY_CASH { get; set; }
        public decimal? THIS_MONTH_NONCASH { get; set; }
        public decimal? THIS_FY_NONCASH { get; set; }
        public decimal? LAST_JUNE { get; set; }
        public string POSTING_FLAG { get; set; }
        public DateTime? STATUS_DATE { get; set; }
        public string INS_DATE_IN_TEXT { get; set; }
        public string STATUS_DATE_IN_TEXT { get; set; }

        //additional
        public int TotalCount { get; set; }
        public string SYNCED_STATUS { get; set; }
        public string ACCHEADNAME { get; set; }
    }

    public class PO_MONTHLY_ACC_BAL_SummaryModel
    {        
        public decimal? BAL_DR { get; set; }
        public decimal? BAL_CR { get; set; }
        public decimal? CUM_BAL_DR { get; set; }
        public decimal? CUM_BAL_CR { get; set; }      
        public decimal? THIS_MONTH_CASH { get; set; }
        public decimal? THIS_FY_CASH { get; set; }
        public decimal? THIS_MONTH_NONCASH { get; set; }
        public decimal? THIS_FY_NONCASH { get; set; }
        public decimal? LAST_JUNE { get; set; }
    }
}
