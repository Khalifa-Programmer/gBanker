namespace gBanker.Data.CodeFirstMigration.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("pksf.LOAN_PRODUCT")]
    public partial class LOAN_PRODUCT
    {
        [Key]
        public string LOAN_CODE { get; set; }
        public string LOAN_NAME { get; set; }
        public string LOAN_SHORT_NAME { get; set; }
        public string LOAN_TYPE { get; set; }
        public string LOAN_GROUP { get; set; }
        public string LOAN_CAT_CODE { get; set; }
        public Int16? SC_INSTALL { get; set; }
        public Int16? PRN_INSTALL { get; set; }
        public string LOAN_PARENT_CODE { get; set; }
        public decimal? FIRST_INSTALL { get; set; }
        public decimal? NEXT_INSTALL { get; set; }
        public decimal? FIRST_INSTALL_PRN { get; set; }
        public decimal? FIRST_INSTALL_SC { get; set; }
        public decimal? NEXT_INSTALL_PRN { get; set; }
        public decimal? NEXT_INSTALL_SC { get; set; }
        public decimal? SC_MONTH_PRODUCT { get; set; }
        public string SC_CAL_METHOD { get; set; }
        public string SC_CAL_VARIANT { get; set; }
        public string INST_COLLECT_METHOD { get; set; }
        public string INST_COLLECT_VARIANT { get; set; }
        public string INST_INTERVAL { get; set; }
        public decimal? SC_RATE { get; set; }
        public string CORE_PROJECT_FLAG { get; set; }
        public string AGE_GROUP_CODE { get; set; }
        public string ADD_LOAN_FLAG { get; set; }
        public Int16? GRACE_PERIOD_IN_MONTH { get; set; }
        public Int16? LOAN_PERIOD_IN_MONTH { get; set; }
        public Int16? TTL_INSTALL { get; set; }
        public string PENAL_FLAG { get; set; }
        public string PENAL_MODE { get; set; }
        public decimal? PENAL_RATE { get; set; }
        public string RESCHEDULE_FLAG { get; set; }
        public string LN_DISB_REC_L1 { get; set; }
        public string LN_DISB_REC_L2 { get; set; }
        public string LN_DISB_REC_L3 { get; set; }
        public string LN_DISB_REC_L4 { get; set; }
        public string LN_DISB_REC_L5 { get; set; }
        public string LN_SC_L1 { get; set; }
        public string LN_SC_L2 { get; set; }
        public string LN_SC_L3 { get; set; }
        public string LN_SC_L4 { get; set; }
        public string LN_SC_L5 { get; set; }
        public string LN_PENAL_L1 { get; set; }
        public string LN_PENAL_L2 { get; set; }
        public string LN_PENAL_L3 { get; set; }
        public string LN_PENAL_L4 { get; set; }
        public string LN_PENAL_L5 { get; set; }
        public string DMR_L1 { get; set; }
        public string DMR_L2 { get; set; }
        public string DMR_L3 { get; set; }
        public string DMR_L4 { get; set; }
        public string DMR_L5 { get; set; }
        public string DMR_INV_L1 { get; set; }
        public string DMR_INV_L2 { get; set; }
        public string DMR_INV_L3 { get; set; }
        public string DMR_INV_L4 { get; set; }
        public string DMR_INV_L5 { get; set; }
        public string CASH_L1 { get; set; }
        public string CASH_L2 { get; set; }
        public string CASH_L3 { get; set; }
        public string CASH_L4 { get; set; }
        public string CASH_L5 { get; set; }
        public string BANK_L1 { get; set; }
        public string BANK_L2 { get; set; }
        public string BANK_L3 { get; set; }
        public string BANK_L4 { get; set; }
        public string BANK_L5 { get; set; }
        public string DMRE_L1 { get; set; }
        public string DMRE_L2 { get; set; }
        public string DMRE_L3 { get; set; }
        public string DMRE_L4 { get; set; }
        public string DMRE_L5 { get; set; }
        public Int16? REPORT_SL { get; set; }
        public decimal? FIRST_INST_PER_TH { get; set; }
        public int? NEXT_INST_PER_TH { get; set; }
        public Int64? PLAN_AVG_LN_DISB { get; set; }
        public Int16? PLAN_INST_PER_TH { get; set; }
        public decimal? PLAN_SC_RATE { get; set; }
        public decimal? PLAN_ADMISSION_FEE { get; set; }
        public Int16? PLAN_FORM_FEE { get; set; }
        public Int16? PLAN_PASS_BOOK_FEE { get; set; }
        public string PRN_SC_ENTRY_FLAG { get; set; }
        public string INSUSER { get; set; }
        public DateTime? INSDT { get; set; }
        public string UPDUSER { get; set; }
        public DateTime? UPDDT { get; set; }
        public Int16? RECOVERY_FREQ { get; set; }
        public string FINANCE_CODE { get; set; }
        public string PROJECT_CODE { get; set; }
        public string COMPONENT_CODE { get; set; }
        public string MRA_COMPONENT_CODE { get; set; }
        public string LOAN_REPORT_GROUP { get; set; }
        public Int16? LOAN_REPORT_GROUP_SL { get; set; }
        public string LOAN_RPT_GROUP_DESC { get; set; }
        public string PROJECT_CODE_ACC { get; set; }
        public string COMPONENT_CODE_ACC { get; set; }
    }
}
