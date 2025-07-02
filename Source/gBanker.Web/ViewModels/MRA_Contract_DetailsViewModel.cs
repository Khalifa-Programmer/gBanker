using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class MRA_Contract_DetailsViewModel
    {
        public string DATATYPE { get; set; }
        public string RECORDNO { get; set; }
        public string BRANCH_CODE { get; set; }
        public string MEMBERID { get; set; }
        public string LOAN_CODE { get; set; }
        public string LOAN_TYPE { get; set; }
        public string LOAN_DISBURSEMENT_DATE { get; set; }
        public string END_DATE_CONTRACT { get; set; }
        public string LAST_INSTALLMENT_PAID_DATE { get; set; }
        public string  DISBURSED_AMOUNT { get; set; }
        public string TOTAL_OUTSTANDING_AMT { get; set; }
        public string PERIODICITY_PAYMENT { get; set; }
        public string TOTAL_NUM_INSTALLMENT { get; set; }
        public string INSTALLMENT_AMT { get; set; }
        public string NUM_REMAINING_INSTALLMENT { get; set; }
        public string NUM_OVERDUE_INSTALLMENT { get; set; }
        public string OVERDUE_AMT { get; set; }
        public string LOAN_STATUS { get; set; }
        public string RESCHEDULE_NO { get; set; }
        public string LAST_RESCHEDULE_DATE { get; set; }
        public string WRITE_OFF_AMT { get; set; }
        public string WRITE_OFF_DATE { get; set; }
        public string CONTRACT_PHASE { get; set; }
        public string LOAN_DURATION { get; set; }
        public string ACTUAL_END_DATE_CONTRACT { get; set; }
        public string ECONOMIC_PURPOSE_CODE { get; set; }
        public string COMPULSORY_SAVING_AMT { get; set; }
        public string VOLUNTARY_SAVING_AMT { get; set; }
        public string TERM_SAVING_AMT { get; set; }
        public string SUBSIDIZED_CREDIT_FLAG { get; set; }
        public string SERVICE_CHARGE_RATE { get; set; }
        public string PAYMENT_MODE { get; set; }
        public string ADVANCE_PAYMENT_AMT { get; set; }
        public string LAW_SUIT { get; set; }
        public string ME { get; set; }
        public string MEMBER_WELFARE_FUND { get; set; }
        public string INSURENCE_COVERAGE { get; set; }

        //public string MFICODE { get; set; }
        //public string ACCOUNTINGDATE { get; set; }
        //public string PRODUCTIONDATE { get; set; }
        //public string TOTALRECORD { get; set; }
    }// END Class

    public class MRA_CONTRACT_HEADER
    {
        public string DATATYPE { get; set; }
        public string MFICODE { get; set; }
        public string ACCOUNTINGDATE { get; set; }
        public string PRODUCTIONDATE { get; set; }
        
    }

    public class MRA_CONTRACT_FOOTER
    {
        public string DATATYPE { get; set; }
        public string TOTALRECORD { get; set; }
    }

}// END Namespace