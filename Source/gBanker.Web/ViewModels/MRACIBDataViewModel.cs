using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gBanker.Web.ViewModels
{
    public class MRACIBSubjectDetailsVM
    {
        public string ACCOUNTINGDATE { get; set; }
        public string PRODUCTIONDATE { get; set; }
        public string MFICODE { get; set; }
        public string BRANCH_CODE { get; set; }
        public string MEMBERID { get; set; }
        public string Name { get; set; }
        public int Occupation { get; set; }
        public string FATHERS_NAME { get; set; }
        public string MOTHERS_NAME { get; set; }
        public int MARITAL_STATUS { get; set; }
        public string SPOUSE_NAME { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string NID { get; set; }
        public string SMARTCARD_NO { get; set; }
        public string BIRTH_CERTIFICATE_NO { get; set; }
        public string TIN { get; set; }
        public int Other_ID_Type { get; set; }
        public string OTHER_ID_NO { get; set; }
        public string EXPIRY_DATE { get; set; }
        public string ISSUE_COUNTRY { get; set; }
        public string ContactNo { get; set; }
        public string P_ADDRESS { get; set; }
        public string P_THANA { get; set; }
        public string P_DISTRICT { get; set; }
        public string P_COUNTRY { get; set; }
        public string PR_ADDRESS { get; set; }
        public string PR_THANA { get; set; }
        public string PR_DISTRICT { get; set; }
        public string PR_COUNTRY { get; set; }
        public int ACADEMIC_QUALIFICATION { get; set; }
        public int OfficeId { get; set; }
        public string ReportDate { get; set; }
        public int Data_Status { get; set; }
    }

    public class MRACIBContractDetailsVM
    {
        public string ACCOUNTINGDATE { get; set; }
        public string PRODUCTIONDATE { get; set; }
        public string MFICODE { get; set; }
        public string BRANCH_CODE { get; set; }
        public string MEMBERID { get; set; }
        public string LOAN_CODE { get; set; }
        public string LOAN_TYPE { get; set; }
        public string LOAN_DISBURSEMENT_DATE { get; set; }
        public string END_DATE_CONTRACT { get; set; }
        public string LAST_INSTALLMENT_PAID_DATE { get; set; }
        public decimal DISBURSED_AMOUNT { get; set; }
        public decimal TOTAL_OUTSTANDING_AMT { get; set; }
        public string PERIODICITY_PAYMENT { get; set; }
        public int TOTAL_NUM_INSTALLMENT { get; set; }
        public decimal INSTALLMENT_AMT { get; set; }
        public decimal NUM_REMAINING_INSTALLMENT { get; set; }
        public decimal NUM_OVERDUE_INSTALLMENT { get; set; }
        public decimal OVERDUE_AMT { get; set; }
        public string LOAN_STATUS { get; set; }
        public int RESCHEDULE_NO { get; set; }
        public string LAST_RESCHEDULE_DATE { get; set; }
        public decimal WRITE_OFF_AMT { get; set; }
        public string WRITE_OFF_DATE { get; set; }
        public string CONTRACT_PHASE { get; set; }
        public int LOAN_DURATION { get; set; }
        public string ACTUAL_END_DATE_CONTRACT { get; set; }
        public string ECONOMIC_PURPOSE_CODE { get; set; }
        public decimal COMPULSORY_SAVING_AMT { get; set; }
        public decimal VOLUNTARY_SAVING_AMT { get; set; }
        public decimal TERM_SAVING_AMT { get; set; }
        public string SUBSIDIZED_CREDIT_FLAG { get; set; }
        public decimal SERVICE_CHARGE_RATE { get; set; }
        public string PAYMENT_MODE { get; set; }
        public decimal ADVANCE_PAYMENT_AMT { get; set; }
        public string LAW_SUIT { get; set; }
        public string ME { get; set; }
        public string MEMBER_WELFARE_FUND { get; set; }
        public string INSURENCE_COVERAGE { get; set; }
        public int OfficeId { get; set; }
        public string ReportDate { get; set; }
        public int Data_Status { get; set; }
    }

}