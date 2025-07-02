using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels.OLRSHubs.OLRSHubPayloads
{
    public class TOP_SHEET_Model
    {
        public string PO_CODE { get; set; }      
        public string MNYR { get; set; }
        public decimal? LOAN_OUT_PRN_OPB_YR { get; set; }
        public decimal? CUM_LOAN_DISB { get; set; }
        public decimal? CUM_LOAN_RCVD_PRN { get; set; }
        public decimal? LOAN_OUT_PRN { get; set; }
        public decimal? LOAN_OD_PRN { get; set; }
        public decimal? LOAN_ADV_PRN { get; set; }
        public decimal? MEMBER_MALE { get; set; }
        public decimal? MEMBER_FEMALE { get; set; }
        public decimal? BORROWER_MALE { get; set; }
        public decimal? BORROWER_FEMALE { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_USER { get; set; }
        public string UPD_USER { get; set; }
        public DateTime? UPD_DATE { get; set; }
        public string POSTING_FLAG { get; set; }
        public decimal? LOAN_OVERDUE { get; set; }
        public DateTime? STATUS_DATE { get; set; }
        public string INS_DATE_IN_TEXT { get; set; }
        public string STATUS_DATE_IN_TEXT { get; set; }

        //additional
        public int TotalCount { get; set; }
        public string SYNCED_STATUS { get; set; }
    }
}
