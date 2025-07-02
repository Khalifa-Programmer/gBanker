namespace gBanker.Data.DBDetailModels.OLRSHubs.Reports
{
    public class PRA_MN_RPT_TAB_XL_FD_Report_Model
    {
        public string PO_CODE { get; set; }       
        public string MNYR { get; set; }      
        public string IND_CODE { get; set; }       
        public string M_F_FLAG { get; set; }
        public decimal? FD_PKSF_FUND { get; set; }
        public decimal? FD_NON_PKSF_FUND { get; set; }
        public decimal? FD_TOTAL_FUND { get; set; }
        public string INS_USER { get; set; }
        public string INS_ON_DATE { get; set; }
        public string SYNCED_STATUS { get; set; }
        public int TotalCount { get; set; }
    }
}
