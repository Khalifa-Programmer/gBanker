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
    public interface IAccReportService
    {
        DataSet GetDataWithParameterFromGDA<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;

        DataSet GetBudgetWithParticularReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet IsReconAccdelVoucher<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet AccdelVoucher<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetRapayment<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetBarChartData<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetArrearAging<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetMemberDetail<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetMemberDetailBN<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDashboardItems<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetPieChartData<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetAccVoucherByVoucherType<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataVoucher<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataVoucherBuro<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataVoucherAll<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataVoucherAllBURO<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataTrialBalance<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataAdminCostTrialBalance<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataAdminCostTrialBalance5_B<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataAdminCostTrialBalanceRecv<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet ExportbKashDepositBalance<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetACCLedgerDataLedgerReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataLedgerReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetACCDataLedgerCodeWiseReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataLedgerCodeWiseReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataRcvPayReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataCashBookReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataCashBookReportRDRS<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataCashBookReportDateWise<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataCashBookReportBuro<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataCashBookBankReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataIncExpReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataIncExpReportJCF<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataIncExpReportGUK<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataBalanceSheetReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataCleanCashReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataLastWorkngDay<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet LastWorkingDate<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetNoteReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetBudgetReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetProductInterestReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetWeeklyCashFlowReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetStatementOfAffairsReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetStatementOfClosingAffairsReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetAccDataForReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet DelTargetBuro<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetNewAccDataForReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetNewAccDataForReportRDRS<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetNewAccDataForReportGUK<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        //DataSet GetNewAccDataForReportGUK<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetMemberProductList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetLastInitialDate<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GenerateStatiticsReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetMemberProductListCateWise<TParamOType>(TParamOType target) where TParamOType : class;
        int SaveLoaneeTransfer<TParamOType>(TParamOType target) where TParamOType : class;
        int SaveLoaneeTransferSelected<TParamOType>(TParamOType target) where TParamOType : class;
        int SaveTransferDisburse<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetVoucherDetail<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetMemberProductListCategoryWise<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataValidWorkngDay<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetAccChartList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetVoucherList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetReconcileList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetReconcileReffNo<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDashboardCenterMemmberMiscSummary<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataTrialBalanceArchive<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetLastClosingDate<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataLedgerReportArchive<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataLedgerCodeWiseReportArchive<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetAccChartListSearch<TParamOType>(TParamOType target) where TParamOType : class;
    }

    public class AccReportService : IAccReportService
    {
        public DataSet GetDataWithParameterFromGDA<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetAccChartListSearch<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getAccChartListOfficeWiseSearch";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetAccVoucherByVoucherType<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Get_Acc_Voucher_ByVoucherType";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataVoucher<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_Voucher";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataVoucherAll<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_Voucher_AllDateWise";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataTrialBalance<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_TrialBalance";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataTrialBalanceArchive<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_TrialBalance_Archive";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataLedgerReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_Ledger";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
                //irfan
            }
        }
        public DataSet GetDataLedgerReportArchive<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_Ledger_Archive";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);                
            }
        }
        public DataSet GetDataLedgerCodeWiseReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_Ledger_Codewise";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
                //irfan
            }
        }
        public DataSet GetDataLedgerCodeWiseReportArchive<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_Ledger_Codewise_Archive";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);                
            }
        }
        public DataSet GetDataRcvPayReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_ReceivePayment";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);                
            }
        }
        public DataSet GetDataCashBookReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_CashBook";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);                
            }
        }
        public DataSet GetDataCashBookBankReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_CashBook_Bank";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);                
            }
        }
        public DataSet GetDataIncExpReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_OffcInfo";
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);                
            }
        }
        public DataSet GetDataBalanceSheetReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_BalanceSheet";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);                
            }
        }
        public DataSet GetDataCleanCashReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_CleanCash";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);                
            }
        }
        public DataSet GetDataLastWorkngDay<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Get_Acc_LastWorkingDay";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetNoteReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_NoteDetails";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetBudgetReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_Budget";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetProductInterestReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_ProductInterest";
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetWeeklyCashFlowReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_WeeklyCashFlow";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetStatementOfAffairsReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_StatementOfAffairs";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetStatementOfClosingAffairsReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_StatementOfClosingAffairs";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetMemberProductList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SP_GET_Member_ProdList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetAccDataForReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetNewAccDataForReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
           // using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public int SaveLoaneeTransfer<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_MemberTransferToAnotherBranch";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.ExecuteNonQuery(storeProcedureName, target);
            }
        }
        public int SaveTransferDisburse<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_SET_ProductTransfer";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.ExecuteNonQuery(storeProcedureName, target);
            }
        }
        public DataSet GetMemberProductListCateWise<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SP_GET_Member_ProdListMember";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetLastInitialDate<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_getlastbusinessdate";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetLastClosingDate<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "GetLastClosingDate";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetVoucherDetail<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetPieChartData<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetPieData";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDashboardItems<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "GetDashboardItems";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetArrearAging<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetArrearAging";
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetBarChartData<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetBarChartData";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetRapayment<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetRapayment";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDashboardCenterMemmberMiscSummary<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Dashboard_GetDashboardCenterMemmberMiscSummary";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetMemberDetail<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_getMemberListDetail";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet AccdelVoucher<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "AccdelVoucher";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetMemberProductListCategoryWise<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SP_GET_Member_ProdList_MemberCategoryWise";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataValidWorkngDay<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "validateDayIntial";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataAdminCostTrialBalance<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_TrialBalance_AdminCost";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetAccChartList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getAccChartListOfficeWise";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet IsReconAccdelVoucher<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "AccdelISReconVoucher";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetVoucherList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getVoucherType";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public int SaveLoaneeTransferSelected<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_MemberTransferToAnotherSelectedBranch";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.ExecuteNonQuery(storeProcedureName, target);
            }
        }

        public DataSet LastWorkingDate<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "get_lastDayendDate";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet ExportbKashDepositBalance<TParamOType>(TParamOType target) where TParamOType : class
        {

            
            var storeProcedureName = "getBkashLogDetailsAllOffice";
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataIncExpReportGUK<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_OffcInfo";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerReportMISAccounts())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetNewAccDataForReportGUK<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportMISAccounts())
            // using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet DelTargetBuro<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            // using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetMemberDetailBN<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_getMemberListDetail_BNUpdate";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GenerateStatiticsReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "GenerateMonthlyStatisticsReport_Buro";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataCashBookReportBuro<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_CashBook";
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataVoucherBuro<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_Voucher";
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataVoucherAllBURO<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_Voucher_AllDateWise";
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataCashBookReportDateWise<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_CashBookDateWise";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetBudgetWithParticularReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_BudgetWithParticular";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataAdminCostTrialBalanceRecv<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_TrialBalance_AdminCostRecv";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataIncExpReportJCF<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_OffcInfo_JCF";
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataAdminCostTrialBalance5_B<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_POMIS5_B";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetACCLedgerDataLedgerReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_Ledger";
            using (var gbData = new gBankerPKSF())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
                //irfan
            }
        }

        public DataSet GetACCDataLedgerCodeWiseReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_Ledger_Codewise";
            using (var gbData = new gBankerPKSF())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
                //irfan
            }
        }

        public DataSet GetDataCashBookReportRDRS<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Acc_CashBook";
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetNewAccDataForReportRDRS<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            // using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetReconcileList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getReconcileRefList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetReconcileReffNo<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getReffNoWiseOffice";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
    }
}
