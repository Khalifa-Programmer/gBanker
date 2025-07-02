using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.DBDetailModels.OLRSHubs.OLRSHubPayloads
{
    public class PRA_LN_DIST_WISE_DISB_Model
    {
        public string PO_CODE { get; set; }

        public string MNYR { get; set; }

        public string DIST_CODE { get; set; }

        public string THANA_CODE { get; set; }

        public string LOAN_CODE { get; set; }

        public decimal? CUM_DISB { get; set; }

        public decimal? MEMBER { get; set; }

        public decimal? BORROWER { get; set; }

        public decimal? LOAN_FY { get; set; }

        public decimal? CUM_BORR { get; set; }


        public decimal? CUM_DISB_HDN { get; set; }

        public decimal? MEMBER_HDN { get; set; }

        public decimal? BORROWER_HDN { get; set; }

        public decimal? LOAN_FY_HDN { get; set; }

        public decimal? CUM_BORR_HDN { get; set; }


        public string INS_USER { get; set; }

        public DateTime? INS_DATE { get; set; }

        public string UPD_USER { get; set; }

        public DateTime? UPD_DATE { get; set; }

        public string POSTING_FLAG { get; set; }

        public DateTime? STATUS_DATE { get; set; }

        public string INS_DATE_IN_TEXT { get; set; }

        public string STATUS_DATE_IN_TEXT { get; set; }

        //additional
        public int TotalCount { get; set; }
        public string SYNCED_STATUS { get; set; }
        public string MFI_DISTRICT_NAME { get; set; }    
        public string MFI_THANA_NAME { get; set; }
        public string PRODUCTNAME { get; set; }


    }
}
