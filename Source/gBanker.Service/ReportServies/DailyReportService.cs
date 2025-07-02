using BasicDataAccess;
using BasicDataAccess.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Service.ReportServies
{
    public interface IDailyReportService
    {
        DataSet GetDataNewOverduePeriodListAllReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetLoanOutstandingSavingsReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataLoanBalanceReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataSavingLedgerReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataSavingBalanceReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataLoanLedgerReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataFullyRepaidReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataFullyRepaid_DateRangeReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataNewOverdueMemberListReport<TParamOType>(TParamOType target) where TParamOType : class;

        DataSet GetDataNewOverdueMemberListReportDisburseDateWise<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataNewOverdueNewMemberListReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataNewOverdueMemberListAllReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataNewOverdueMemberListAllReportPrayas<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataNewOverdueMemberListAllReportRDRS<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataNewOverdueMemberListAllReportDisburseDateWise<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataTodaysSummaryReportNew<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataTodaysSummaryReportNewSouthAfrica<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataDailyTransactionTopsheet<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataMemberlistReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataCenterwiseTransactionReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataDisburseReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataCenterwise_loan_SC<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetPomisTargetAchivement<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetProductivityRatioReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetQualityRatioAnalysisReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetSustainabilityAndProfitabilityRatioReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetFinancialManagmentSolvencyRatioReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetBranchWiseRatioAnalysisReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetPreWriteOffListReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetWaitingForLoanList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetTrialBalance_AccountCodeWise<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet getRepaymentScheduleReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataNewOverdueMemberListAllReportProductWise<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataNewOverdueMemberListAllReportProductWiseRDRS<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetPreWriteOffListReportAllOffice<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataNewOverdueMemberListAllReportByFilter<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataExpiredForcast_DateRangeReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataMemberlistMemCategoryWiseReport<TParamOType>(TParamOType target) where TParamOType : class;
    }
    //Test
    public class DailyReportService : IDailyReportService
    {

        public DataSet GetDataNewOverduePeriodListAllReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "OverduePeriodType";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataExpiredForcast_DateRangeReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetRpt_ExpiredForcastDateRangeEmpWise";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetLoanOutstandingSavingsReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "LoanOutstandingSavings";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataLoanLedgerReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_LoanLedger";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataLoanBalanceReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_LoanBalance";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataSavingLedgerReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_SavingLedger";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataSavingBalanceReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_SavingsBalance";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        
        public DataSet GetDataFullyRepaidReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetRpt_FullyRepaidEmpWise";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataFullyRepaid_DateRangeReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetRpt_FullyRepaidDateRangeEmpWise";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataNewOverdueMemberListReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Get_DueLoan";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataNewOverdueMemberListAllReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Rpt_OverDueLoaneeList_All";
            using (var gbData = new gBankerPKSF())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataNewOverdueMemberListAllReportByFilter<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Rpt_OverDueLoaneeList_All_By_Filter";
            using (var gbData = new gBankerPKSF())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataTodaysSummaryReportNew<TParamOType>(TParamOType target) where TParamOType : class
        {
            //var storeProcedureName = "Proc_GetRptTodaySummary";  changed by mahbub 
            var storeProcedureName = "Proc_GetRptTodaySummaryOTR";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataDailyTransactionTopsheet<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetRptDailyTransactionTopsheet";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataMemberlistReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Memberlist";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataCenterwiseTransactionReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetTodaySummary";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataDisburseReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "GetDisburseList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataCenterwise_loan_SC<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Centerwise_loan_SC";
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        
        public DataSet GetPomisTargetAchivement<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Rpt_POMISTargetAndAchievement";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetProductivityRatioReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            //var storeProcedureName = "Proc_RPT_ProductivityRatioAnalysis";
            var storeProcedureName = "Proc_RPT_ProductivityRatioAnalysis_new";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetQualityRatioAnalysisReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            //var storeProcedureName = "Proc_RPT_PortfolioQualityRatio";
            var storeProcedureName = "Proc_RPT_PortfolioQualityRatioNew";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetSustainabilityAndProfitabilityRatioReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_ProfitabilitySustainabilityRatioNew";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        

        public DataSet GetFinancialManagmentSolvencyRatioReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_rptFinancialManagmentSolvencyRatioNew";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }



        public DataSet GetBranchWiseRatioAnalysisReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_RPT_At_a_Glance";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetPreWriteOffListReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getWriteOffListReport";
            using (var gbData = new gBankerPKSF())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetWaitingForLoanList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "RPT_Waiting_for_Loan_Member_List_EmployeeWise";
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetTrialBalance_AccountCodeWise<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_TrialBalance_AccountCodeWise";
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet getRepaymentScheduleReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getRepaymentScheduleReportHistory";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetDataNewOverdueMemberListAllReportProductWise<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SP_RPT_OverduePeriodType";
            using (var gbData = new gBankerPKSF())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetDataNewOverdueMemberListAllReportDisburseDateWise<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Rpt_OverDueLoaneeList_All_DisburseDateWise";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetPreWriteOffListReportAllOffice<TParamOType>(TParamOType target) where TParamOType : class
        {

            var storeProcedureName = "getWriteOffListReportAllOffice";
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataNewOverdueNewMemberListReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Get_DueLoaneeThisMonth";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataMemberlistMemCategoryWiseReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Memberlist_MemberCategoryWise";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataNewOverdueMemberListAllReportRDRS<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Rpt_OverDueLoaneeList_All";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataNewOverdueMemberListAllReportProductWiseRDRS<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Rpt_OverDueLoaneeList_ProductWise";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataNewOverdueMemberListAllReportPrayas<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Rpt_OverDueLoaneeList_All";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataTodaysSummaryReportNewSouthAfrica<TParamOType>(TParamOType target) where TParamOType : class
        {
            //var storeProcedureName = "Proc_GetRptTodaySummary";  changed by mahbub 
            var storeProcedureName = "Proc_GetRptTodaySummaryOTR_SouthAfrica";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataNewOverdueMemberListReportDisburseDateWise<TParamOType>(TParamOType target) where TParamOType : class
        {
            //var storeProcedureName = "Proc_GetRptTodaySummary";  changed by mahbub 
            var storeProcedureName = "PROC_RPT_OverDueLoaneeList_DisburseDateWise";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
    }
}