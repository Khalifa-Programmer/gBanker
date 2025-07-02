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
    public interface IGroupwiseReportService
    {
        DataSet GetLoanDisbursedProductWiseReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetMonthlyProgressReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetMonthlyProgressLoanReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetrptMonthlyJCFReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;


        DataSet GetWeekNoDeclaration<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetWeeklyDataProcess<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetYearWiseWeekNoList<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;





        DataSet GetMonthlySummaryReportPorcess<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;

        DataSet GetDataMemberInfoReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataUltimateMemberAgeReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;



        DataSet GetCountry<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDivision<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDivisionInsert<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;

        DataSet GetDistrict<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDistrictInsert<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;

        DataSet GetUpozilla<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetUpozillaInsert<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;

        DataSet GetUnion<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetUnionInsert<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;

        DataSet GetVillage<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetVillageInsert<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;


        DataSet writeOffOldMemberCode<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;



        //DataSet GetActiveCenter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetWriteOffHistoryCheck<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetWriteOffHistoryInsert<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetWriteOffHistoryDelete<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetWriteOffHistory<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;

        DataSet ProvisionCalculationConsolidate<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataRecoveryStatement<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetProductListByMemberList<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetLoanTermList<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        
        DataSet GetActiveAccount<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataOverDueStatement<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataStaffWiseSpecialSavings<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataOrganizerWiseRecoveryStatement<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataUltimateReleaseReportFromReportServer<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataUltimateReleaseReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataDailyReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GenerateMemberListReportDependsOnCollection<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataLedgerReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataGenerateTodays_Comperative<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetProductListByMemberWithProcedure<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        
        DataSet GetDisbursementCorrectionData<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetMonthlyInstallmentScheduleReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        
        DataSet GetMemberBalanceInfoReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetLoanDisburse<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        
        DataSet GetDataProcessLOg<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataWithoutDaterange_AdayEmpWise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet ExportExcellData<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetActiveCenter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetActiveEmployee<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetTabCollection<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetTabCollectionbKash<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetTabCollectionHistory<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet UploadTabCollection<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet UploadTabCollectionbKash<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetStatisticsReportDetails<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetStatisticsReportDetailsUpdate<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet getStatisticsReportDetailsInfo<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;




        DataSet GetTargetAchievementBuroLatest<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet getTargetAchievementBuroLatestInfo<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetgetTargetAchievementBuroLatestUpdate<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetgetTargetAchievementBuroLatestInsert<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;

        

        DataSet GetDataSupplimentaryReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataServiceChargeStatement<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataUltimateReleaseReportSavingStatement<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataDataseAccess<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetEmployeeTransfer<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataUltimateReleaseReportDSK<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataResizeReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataUltimateReleaseStaffWiseStatement<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataUltimateReleaseStaffWiseStatementGUK<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;

        // DataSet GetDataUltimateReleaseStaffWiseStatementDSK<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataUltimateReleaseStaffWiseStatementDSK<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataUltimateReleaseStaffWiseStatementJCF<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataUltimateReleaseReportGroupLedgerSavingSamitywise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataUltimateReleaseReportWithReportServer<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataDSKDailyReceipt<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDaliyRecoverableReceipt<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetOverDueAgeing<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet POMISConsolidationOfficewise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataPOMIS1_GroupAndMembersInfoConsolidationOfficewise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataPOMIS1_SavingsStatementConsolidationOfficewise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataPOMIS1_SavingsStatementItemWiseTotalConsolidationOfficewise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataPOMIS1_AdmisionWithdrawanAttendanceConsolidationOfficewise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetOfficeDashBoard<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataPOMIS1_DataMarge<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetWorkingLogInfomation<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataProvisionCalculation_DataMarge<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetWorkingLogInfomationOfficeWise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataDisbursementTransferMemberList<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataUltimateReleaseReportGroupLedgerSavingSamitywise_loan<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        
        DataSet GetOfficeWiseChangesReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetProgramMISReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetProgramMISReportJCF<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetFOWiseChangesReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataUltimateReleaseReportMonthWiseSavingReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataUltimateReleaseReportMonthWiseSavingReportDSK<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataUltimateReleaseReportMemberwiseSavingInterestReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet MRA4ConsolidateReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataBalanceCompareMemberWise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataBalanceCompareMemberWise_MemberWise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataBalanceCompareCenterWise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataBalanceCompareStaffWise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataBalanceCompareOfficeWise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataUltimateReleaseReportMemberCenterwiseSavingInterestReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataUltimateReleaseReportMemberCenterwiseSavingInterestReportDSK<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetOverDueAgeingPIDIM<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;

        DataSet GenerateMiscellaneousMemberWise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataUltimateLedgerSavingCenterwise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataUltimateLoanSavingAccountInfoCenterWise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetMemberInfoByMemberCode<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetMemberNomineeList<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetMemberNomineeListForSaving<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GenerateMemberListReportAgeWise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;

        
    }
    public class GroupwiseReportService : IGroupwiseReportService
    {



        public DataSet GetDataMemberInfoReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportMIS())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataUltimateMemberAgeReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportMIS())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetCountry<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {

            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDivision<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {

            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDivisionInsert<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {

            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDistrict<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {

            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDistrictInsert<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {

            //using (var gbData = new gBankerReportDataAccess())
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetUpozilla<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {

            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetUpozillaInsert<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {

            //using (var gbData = new gBankerReportDataAccess())
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetUnion<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {

            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetUnionInsert<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {

            //using (var gbData = new gBankerReportDataAccess())
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetVillage<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {

            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetVillageInsert<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {

            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet writeOffOldMemberCode<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataRecoveryStatement<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {

            //var storeProcedureName = "RecoverableRecoveryRegister";
           // using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataOverDueStatement<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            //var storeProcedureName = "OverdueClassification";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataStaffWiseSpecialSavings<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            //var storeProcedureName = "SpecialSavingsStaffWise";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataOrganizerWiseRecoveryStatement<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            //var storeProcedureName = "OrganizerWiseRecoveryStatemnt";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataUltimateReleaseReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            //var storeProcedureName = "OrganizerWiseRecoveryStatemnt";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerPKSF())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataUltimateReleaseReportWithReportServer<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            //var storeProcedureName = "OrganizerWiseRecoveryStatemnt";
            using (var gbData = new gBankerDataAccess())
            //using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetOfficeDashBoard<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            //var storeProcedureName = "OrganizerWiseRecoveryStatemnt";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet POMISConsolidationOfficewise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            //var storeProcedureName = "OrganizerWiseRecoveryStatemnt";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetOverDueAgeing<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerReportMIS())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataPOMIS1_GroupAndMembersInfoConsolidationOfficewise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            //var storeProcedureName = "OrganizerWiseRecoveryStatemnt";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerDataAccess())
             {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataPOMIS1_SavingsStatementConsolidationOfficewise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            //var storeProcedureName = "OrganizerWiseRecoveryStatemnt";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataPOMIS1_SavingsStatementItemWiseTotalConsolidationOfficewise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            //var storeProcedureName = "OrganizerWiseRecoveryStatemnt";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataPOMIS1_AdmisionWithdrawanAttendanceConsolidationOfficewise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            //var storeProcedureName = "OrganizerWiseRecoveryStatemnt";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        //public DataSet GetOfficeDashBoard<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        //{
        //    //var storeProcedureName = "OrganizerWiseRecoveryStatemnt";
        //    using (var gbData = new gBankerDataAccess())
        //    {
        //        return gbData.GetDataOnDateset(storeProcedureName, target);
        //    }

        //}
        public DataSet GetDataPOMIS1_DataMarge<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataProvisionCalculation_DataMarge<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
           // using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataUltimateReleaseReportGroupLedgerSavingSamitywise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            //var storeProcedureName = "OrganizerWiseRecoveryStatemnt";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetWorkingLogInfomation<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetWorkingLogInfomationOfficeWise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataUltimateReleaseStaffWiseStatement<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerJCFCollectionSheet())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataDisbursementTransferMemberList<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataUltimateReleaseReportGroupLedgerSavingSamitywise_loan<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDaliyRecoverableReceipt<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        
        }
        public DataSet GetOfficeWiseChangesReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportMIS())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetProgramMISReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportMIS())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetFOWiseChangesReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportMIS())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataUltimateReleaseReportMonthWiseSavingReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            //var storeProcedureName = "OrganizerWiseRecoveryStatemnt";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataUltimateReleaseReportMemberwiseSavingInterestReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataBalanceCompareMemberWise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataBalanceCompareCenterWise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportMIS())
            //using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataBalanceCompareStaffWise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportMIS())
            //using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataBalanceCompareOfficeWise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportMIS())
            //using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataUltimateReleaseReportFromReportServer<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataUltimateReleaseReportMemberCenterwiseSavingInterestReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetOverDueAgeingPIDIM<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GenerateMiscellaneousMemberWise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
          
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataResizeReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
           // using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet MRA4ConsolidateReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            //using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataDSKDailyReceipt<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataUltimateReleaseReportDSK<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet ProvisionCalculationConsolidate<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetEmployeeTransfer<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            //using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataDataseAccess<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataUltimateReleaseReportSavingStatement<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataServiceChargeStatement<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataSupplimentaryReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetActiveAccount<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetTabCollection<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetTabCollectionbKash<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        

        public DataSet GetTabCollectionHistory<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetStatisticsReportDetails<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetStatisticsReportDetailsUpdate<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet getStatisticsReportDetailsInfo<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetTargetAchievementBuroLatest<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet getTargetAchievementBuroLatestInfo<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetgetTargetAchievementBuroLatestUpdate<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetgetTargetAchievementBuroLatestInsert<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet UploadTabCollection<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet UploadTabCollectionbKash<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        

        public DataSet GetProductListByMemberList<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetLoanTermList<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetActiveCenter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetActiveEmployee<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetWriteOffHistoryCheck<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetWriteOffHistoryInsert<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetWriteOffHistoryDelete<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetWriteOffHistory<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetDataUltimateLedgerSavingCenterwise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataUltimateLoanSavingAccountInfoCenterWise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetProgramMISReportJCF<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportMIS())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataUltimateReleaseStaffWiseStatementJCF<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportMIS())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetMemberInfoByMemberCode<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportMIS())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetMemberNomineeList<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportMIS())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetMemberNomineeListForSaving<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportMIS())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet ExportExcellData<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerPKSF())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataWithoutDaterange_AdayEmpWise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataProcessLOg<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetMemberBalanceInfoReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetLoanDisburse<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetMonthlyInstallmentScheduleReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDisbursementCorrectionData<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetProductListByMemberWithProcedure<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataGenerateTodays_Comperative<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataLedgerReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetMonthlySummaryReportPorcess<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetWeekNoDeclaration<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetWeeklyDataProcess<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetYearWiseWeekNoList<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetLoanDisbursedProductWiseReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportMIS())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetMonthlyProgressReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportMIS())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetMonthlyProgressLoanReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportMIS())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetrptMonthlyJCFReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportMIS())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GenerateMemberListReportDependsOnCollection<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GenerateMemberListReportAgeWise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataUltimateReleaseStaffWiseStatementDSK<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportMIS())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataUltimateReleaseReportMemberCenterwiseSavingInterestReportDSK<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataUltimateReleaseReportMonthWiseSavingReportDSK<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportMIS())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataUltimateReleaseStaffWiseStatementGUK<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportMIS())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataBalanceCompareMemberWise_MemberWise<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerReportMIS())
            //using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataDailyReport<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            //var storeProcedureName = "OrganizerWiseRecoveryStatemnt";
            //using (var gbData = new gBankerDataAccess())
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
    }
}
