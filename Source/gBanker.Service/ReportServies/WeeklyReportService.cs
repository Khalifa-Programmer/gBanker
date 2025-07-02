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
    public interface IWeeklyReportService
    {
        DataSet GetDataSamityWiseWeeklyReportJCF<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataSamityWiseWeeklyReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataSamityWiseWeeklyDSKReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataSamityWiseWeeklyDSK_WithoutCategoryGroupReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataWeeklyCollectionSheetReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataMonthlyCollectionSheet_NewReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataMonthlyCollectionSheet_NewReportCCDB<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataMonthlyCollectionSheet_ForAllReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataCollectionSheetWeeklyMonthlyReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataCollectionSheetWeeklyMonthlyReportSagorica<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataCollectionSheetWeeklyMonthlyReportMemberwise<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataCollectionSheetWeeklyMonthlyReportMemberwiseSouthAfrica<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataCollectionSheetWeeklyMonthlyReportMemberwisePrayas<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataCollectionSheetWeeklyMonthlyReportMemberwiseSeperateSavings<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataMonthlyCollectionSheet_AmericaReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetKhatWaryReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetKhatWaryReportExport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet LoanDisburseExport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet CenterExport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetRebateReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetNegativeLoanLedgerBalance<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetStaffWiseSpecialSavingReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet DailyRecoverableAndRecoveryRegisterCenterDateWise<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet DailyRecoverableAndRecoveryRegisterCenterDateWiseFROMPKSD<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetMemberTransfer<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet RecoverableAndRecoveryRegisterDSKFormat<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetMainProductList();
        DataSet GetSavingMainProductList();
        DataSet GetSavingMainProductListForCenterWise();
        //DataSet GetMemberwiseProductAndLoanTermforDropDownList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet DayEndProcess<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet HalYearlySavingInterest<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet RepaymentScheduleProcess<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet MonthProcessAverageMethod<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet AutoVoucherCollectionProcess<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet CheckAutoVoucher<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet PortFOlioYearClosingProcess<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet LoanInstallmentCorrectionLedgerPost<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet SavingInstallmentCorrectionLedgerPost<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet HalyYearlyProcess<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDualLoanReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataMonthlyCollectionSheet_NewReportChina<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet MonthProcessLLPVoucher<TParamOType>(TParamOType target) where TParamOType : class;
        // DataSet RepaymentScheduleProcessSpecial<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet DailyRecoverableAndRecoveryRegisterProductWise<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet DailyRecoverableAndRecoveryRegisterProductWiseFROMPKSF<TParamOType>(TParamOType target) where TParamOType : class;

        DataSet MonthlyInstallmentProcess<TParamOType>(TParamOType target) where TParamOType : class;

        DataSet DailyRecoverableAndRecoveryRegisterAreaDateWise<TParamOType>(TParamOType target) where TParamOType : class;

        DataSet ProcessRecoverableAndRecoveryRegisterAreaDateWise<TParamOType>(TParamOType target) where TParamOType : class;
        //DataSet DailyRecoverableAndRecoveryRegisterAreaDateWise<TParamOType>(TParamOType target) where TParamOType : class;

        //DataSet ProcessRecoverableAndRecoveryRegisterAreaDateWise<TParamOType>(TParamOType target) where TParamOType : class;


        DataSet CheckProcessRecoverableAndRecoveryRegisterAreaDateWise<TParamOType>(TParamOType target) where TParamOType : class;

        DataSet GetCollectionSheetWeeklyMonthlyReportMemberwise_SouthAfrica<TParamOType>(TParamOType target) where TParamOType : class;

    }
    public class WeeklyReportService : IWeeklyReportService
    {
        public DataSet GetCollectionSheetWeeklyMonthlyReportMemberwise_SouthAfrica<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "MonthlyCollectionSheet_ForAll__SouthAfrica";

            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet ProcessRecoverableAndRecoveryRegisterAreaDateWise<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SP_RecoverableRecoveryRegisterDateWiseAreaZone";

            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet DailyRecoverableAndRecoveryRegisterAreaDateWise<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SP_RecoverableRecoveryRegisterDateWiseAreaZone_Report";

            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet CheckProcessRecoverableAndRecoveryRegisterAreaDateWise<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SP_Check_Process_RecoverableRecoveryRegisterDateWiseAreaZone";

            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        //public DataSet ProcessRecoverableAndRecoveryRegisterAreaDateWise<TParamOType>(TParamOType target) where TParamOType : class
        //{
        //    var storeProcedureName = "SP_RecoverableRecoveryRegisterDateWiseAreaZone";

        //    using (var gbData = new gBankerReportDataAccess())
        //    {
        //        return gbData.GetDataOnDateset(storeProcedureName, target);
        //    }
        //}
        //public DataSet DailyRecoverableAndRecoveryRegisterAreaDateWise<TParamOType>(TParamOType target) where TParamOType : class
        //{
        //    var storeProcedureName = "SP_RecoverableRecoveryRegisterDateWiseAreaZone_Report";

        //    using (var gbData = new gBankerReportDataAccess())
        //    {
        //        return gbData.GetDataOnDateset(storeProcedureName, target);
        //    }
        //}
        public DataSet GetDataSamityWiseWeeklyReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_MFI_Weekly_Statement_New";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerWeeklyDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataSamityWiseWeeklyDSKReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_MFI_Weekly_Statement_New_Dsk";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerWeeklyDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataSamityWiseWeeklyDSK_WithoutCategoryGroupReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_MFI_Weekly_Statement_New_Dsk_WithoutCategoryGroup";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerWeeklyDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataWeeklyCollectionSheetReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_WeeklyCollectionSheet";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataMonthlyCollectionSheet_NewReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Monthly_Collection_SheetWeeklyMonthly";
            using (var gbData = new gBankerDataAccess())
            //using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataMonthlyCollectionSheet_ForAllReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "MonthlyCollectionSheet_ForAll";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
         public DataSet GetDataCollectionSheetWeeklyMonthlyReportSagorica<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Monthly_Collection_SheetWeeklyMonthly";
            using (var gbData = new gBankerDataAccess())
            //using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        
        public DataSet GetDataCollectionSheetWeeklyMonthlyReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Monthly_Collection_SheetWeeklyMonthly";
            using (var gbData = new gBankerPKSF())
            //using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataCollectionSheetWeeklyMonthlyReportMemberwise<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Monthly_Collection_SheetWeeklyMonthlyMeberwise";

            using (var gbData = new gBankerJCFCollectionSheet())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataMonthlyCollectionSheet_AmericaReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetRpt_LoanCollectionSheetAmerica";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetKhatWaryReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "KhatwariRreport";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetRebateReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "RebateInformation";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetNegativeLoanLedgerBalance<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "NegativeLoanLedgerBalance";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetStaffWiseSpecialSavingReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SpecialSavingsStaffwiseReport";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet DailyRecoverableAndRecoveryRegisterCenterDateWise<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "RecoverableRecoveryRegisterDateWiseNew";
            //var storeProcedureName = "RecoverableRecoveryRegisterDateWise";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetMainProductList()
        {
            var storeProcedureName = "MainProductList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDatesetWithoutParam(storeProcedureName);
            }
        }
        public DataSet DayEndProcess<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Prcs_DayEnd";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet MonthProcessAverageMethod<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "monthlyProcessAverageMethod";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        } 
        
        public DataSet MonthProcessLLPVoucher<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "generateLLPVoucher";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet RepaymentScheduleProcess<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Set_RepaymentSchedule";
            using (var gbData = new gBankerSpecialDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet AutoVoucherCollectionProcess<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "AccAutoVoucherCollection";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet HalYearlySavingInterest<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "AddHalfYearlySavingInterest";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet CheckAutoVoucher<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "CheckAUtoVoucher";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet PortFOlioYearClosingProcess<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "PortFOlioYearClosingProcess";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet LoanInstallmentCorrectionLedgerPost<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "ProcLoanInstallmentLedgerPost";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet SavingInstallmentCorrectionLedgerPost<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "ProcSavingInstallmentLedgerPost";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet HalyYearlyProcess<TParamOType>(TParamOType target) where TParamOType : class
        {
            
           var storeProcedureName = "AddHalfYearlySavingInterest";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDualLoanReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_RPT_Duel_LoanInformation";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataMonthlyCollectionSheet_NewReportChina<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Monthly_Collection_SheetWeeklyMonthlyChina";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet RecoverableAndRecoveryRegisterDSKFormat<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Rpt_RecoverableRegisterDaywise";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetMemberTransfer<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Rpt_MemberTransfer";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataMonthlyCollectionSheet_NewReportCCDB<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Monthly_Collection_SheetWeeklyMonthlyCCB";
            using (var gbData = new gBankerDataAccess())
            //using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetSavingMainProductList()
        {
            var storeProcedureName = "getSavingMainProductList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDatesetWithoutParam(storeProcedureName);
            }
        }
        public DataSet GetSavingMainProductListForCenterWise()
        {
            var storeProcedureName = "getSavingMainProductListNew";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDatesetWithoutParam(storeProcedureName);
            }
        }
        public DataSet GetDataCollectionSheetWeeklyMonthlyReportMemberwiseSeperateSavings<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Monthly_Collection_SheetWeeklyMonthlyMeberwiseSpecial";

            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetKhatWaryReportExport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "KhatwariRreportExport";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet LoanDisburseExport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "RPT_LoanDisburseRegister_BuroExport";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet CenterExport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Rpt_CenterlistExport";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataCollectionSheetWeeklyMonthlyReportMemberwisePrayas<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Monthly_Collection_SheetWeeklyMonthlyMeberwiseAccount";

            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataSamityWiseWeeklyReportJCF<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_MFI_Weekly_Statement_New";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerReportMIS())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet DailyRecoverableAndRecoveryRegisterProductWise<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "RecoverableRecoveryRegisterProductWiseNew";
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet MonthlyInstallmentProcess<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GenerateAppendMonthlyInstallment";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet DailyRecoverableAndRecoveryRegisterProductWiseFROMPKSF<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "RecoverableRecoveryRegisterProductWiseNew";
            using (var gbData = new gBankerWeeklyDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet DailyRecoverableAndRecoveryRegisterCenterDateWiseFROMPKSD<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "RecoverableRecoveryRegisterDateWiseNew";
            //var storeProcedureName = "RecoverableRecoveryRegisterDateWise";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerWeeklyDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataCollectionSheetWeeklyMonthlyReportMemberwiseSouthAfrica<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_Monthly_Collection_SheetWeeklyMonthly_SouthAfrica";

            using (var gbData = new gBankerJCFCollectionSheet())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
    }
}
