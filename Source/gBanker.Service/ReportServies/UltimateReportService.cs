using BasicDataAccess;
using BasicDataAccess.Data;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using gBanker.Data.Repository;
using gBankerCodeFirstMigration.Db;
using System;

using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Service.ReportServies
{

    public interface IUltimateReportService
    {
        DataSet GetMemberBalanceInfoByProduct<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet PenaltyWithDailySavingBank_Adjustment<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet DataInsertintoDailyLoanTrx_Adjustment<TParamOType>(TParamOType target) where TParamOType : class;

        DataSet GenerateNIDStatusForAdminOnly<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GenerateNIDStatus<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GenerateNIDStatusEligible<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet DailyRecoverableAndRecoveryRegisterEmployeeCenterDateWise<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataWithParameterCollection<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetAccountCodeListbudgetAndParticularwise<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetAccCodeListAccordingToOfficeBudgetForProgram<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GenerateMemberLastCodeWriteOff<TParamOType>(TParamOType target) where TParamOType : class;

        DataSet UpdateMemberProshikha<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet UpdateMemberProshikhaMemberName<TParamOType>(TParamOType target) where TParamOType : class;

        DataSet GetOfficesForDropdownList<TParamOType>(TParamOType target) where TParamOType : class;
 
         DataSet Proc_Set_SavingOpeingWhenMemberEligible<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetSMSDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet UpdateLoanAccount<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetBkashTransactionLog<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet InsertLOANRegisterUpdateINFO<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetAccountCodeJCF<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet InsertSAVINGRegisterUpdateINFO<TParamOType>(TParamOType target) where TParamOType : class;
       DataSet GetAllAccountCodeList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetAccountCodeList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet setCateGoryTransfer<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataDailySavingCollectionReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataTodaysSummaryMemberwiseReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataRecoverableReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataMRACDBReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataMRACDB03Report<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataLoanAndSavingsBalanceSWReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataPOMISFiveAReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataPOMISTargetAndAchievement<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetLocationList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetMembersLoanInformation<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetMembersPassBookRegisterInformation<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetRepaymentSchedule<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetProductList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDailyCenterList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDailySavingCenterList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetZoneList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetProductListAccordingToOffice<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetAccCodeListAccordingToOffice<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet SetReconVoucher<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetAccCodeListAccordingToOfficeBudget<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetBankAccCodeListAccordingToOffice<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetAccCodetListAccordingToOffice<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetCenterTypeList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetProductSavingList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetProductSavingList_SDC<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetNoAccount<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetMemberPasBookList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetMemberCashBookList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetgetOrganizerList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetAccLevelByAccId<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet getAccLevelForBankBookByAccID<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetBankBookList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet ExecuteSPBankINterestRate<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet ExecuteSPMonthlySaving<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet getCategoryByCategoryID<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetProductListFromSavingSummary<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetNewcategoryList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet DeleteProcessCheck<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet Proc_GetSavingBalanceForCate<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDailyLoanTrxlist<TParamOType>(TParamOType target) where TParamOType : class;
       // DataSet GetLastCenterCodeList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet ValidateMainItemCode_21_list<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDailySavinglist<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetWorkingLogInfo<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetMainProductCode<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetAuditTrialo<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetriteOffSummarylist<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetCenterROleWise<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetPreWriteOffHistory<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetInsuranceSlot<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet AccessInsuranceSlot<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet Set_GenerateReffNo<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetReconPurposeList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet SetSpecialLoanCollection<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet AddApplicationSetting<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet updateSavingInstallmentSap<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet DelSavingSummary<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet SavingReinstate<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDailyLoanTrxID<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetLoanSummaryID<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet DataInsertintoDailyLoanTrx <TParamOType>(TParamOType target) where TParamOType : class;
        DataSet DataInsertintoEmployee<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet getAccBuroCode<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetTodaysCollection<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet Set_LoanSummaryProposalAPI<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataDistributeAmount<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet validateLOanProposal<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet validateMemberPassbook<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet UpdateSpecialLOan<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetKerPressValueForSpecialLoan<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetKerPressValueForSpecialLoanWeekly<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetKerPressValueForSpecialLoanWeeklyFlyTable<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetStopClaimableInterestValidationGUK<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetKerPressValueForSpecialLoanMonthly<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetKerPressValueForSpecialLoanMonthlyFlyTable<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataDailyLoantrx<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet AccountCloseMemberList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet UpdateCenterIDInAllRelatedTable<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataMemberCollectionAdd<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet validateSavingBalance<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetProductListByFrequencyMode<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetProductListByFrequencyModeAccoringTOOffice<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetProductListBySubCat<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetProductListBySubCatAccordingToOffice<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetMainProductList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetMainProductListAccordingToOffice<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetSubMainProductList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetLoanDisburseTypeList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetMaxLoanTermMainCodeWise<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetProductListT<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetProductListTAccordingToOffice<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetMemberDetails<TParamOType>(TParamOType target) where TParamOType : class;
        List<DBMemberDetailModel> GetMemberDetailsasList<TParamOType>(TParamOType target);
        List<Organization> GetOrganizationDetails<TParamOType>(TParamOType target);
        List<DBMemberDetailModel> GetMemberDetailsasListEmployeeWise<TParamOType>(TParamOType target);
        List<DBMemberDetailModel> GetMemberDetailsasListEmployeeDateWise<TParamOType>(TParamOType target);
        List<DBCenterDetailModel> GetCenterListEmployeeWise<TParamOType>(TParamOType target);
        List<DBMemberDetailModel> GetElegibleMemberDetailsasList<TParamOType>(TParamOType target);
        List<DBMemberDetailModel> GetElegibleMemberDetailsasListEmployeeWise<TParamOType>(TParamOType target);
        DataSet GetAccountCode<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet UpdateAccountInfo<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetEmployment<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet DelSpecialLoanCollection<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet DelSpecialLoanCollectionAdjustment<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet SetRebateCollection<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetAccountCorrectionLog<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDisbursementCorrectionLog<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetMemberCorrectionLog<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetLoanInstallmentmentCorrectionLog<TParamOType>(TParamOType target) where TParamOType : class;
        List<DBholidayDetailModel> GetHolidayListCenterWise<TParamOType>(TParamOType target);
        List<DBholidayDetailModel> GetHolidayListSearch<TParamOType>(TParamOType target);
        DataSet GetSavingInstallmentCorrectionLog<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetLastCenterCode<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetOfficeListByEmployee<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet RiskDataTransferIntoSavings<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet DelRiskDataTransferIntoSavings<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GenerateMemberLastCode<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GenerateMemberLastCodeMember<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GenerateMemberLastCodeMemberSSS<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet CheckDuplicateMember<TParamOType>(TParamOType target) where TParamOType : class;
        object GetLastInitialDate<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet ValidateSavingSummary<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet ValidateSavingSummaryMonthlyInterest<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet PenaltyWithDailySaving<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet getLoanProposalListRoleWise<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet getArroveCellingList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet PenaltyWithDailySavingBank<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet PenaltyWithDailySavingBank_Dormant<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetIdentityTypeList<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet SetSurvey<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet InsertTabUploadData<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataWithParameterMRA<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataWithParameterMRAJCF<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet SMSUpdate<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;
        DataSet GetDataWithoutParameter(string storeProcedureName);
        DataSet GetRebateInfo<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet SetRebateInfo<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetInsuranceRate<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetLTSPenaltySlot<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet AccessInsuranceRate<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet AccessLTSPenaltySlot<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet ExecuteSPBankINterestClear<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetSpecialSavingLastBalance<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet getMaxAccountNo<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet Proc_getSavingLastBalance<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GenerateLoanAndSavingAccount<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet UpdateAccountInfoChanged<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet UpdateAccountInfoChangedInsert<TParamOType>(TParamOType target) where TParamOType : class;
        // Start Mapping
        DataSet GetProductListForMapping<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetSelectedProductListForMapping<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet SaveProductMapping<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet DeleteProductMapping<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetAccChartListForMapping<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetSelectedAccChartListForMapping<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet SaveAccChartMapping<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet DeleteAccChartMapping<TParamOType>(TParamOType target) where TParamOType : class;

        DataSet GetOfficeByLevel<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetSelectedOfficeListForMapping<TParamOType>(TParamOType target) where TParamOType : class;

        // END Mapping

        // Start Mapping Cash

        DataSet GetOfficeByLevelCash<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetSelectedAccChartListForMappingCash<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetAccChartListForMappingCash<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet DeleteAccChartMappingCash<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet SaveAccChartMappingCash<TParamOType>(TParamOType target) where TParamOType : class;

        // END Mapping Cash

        // Start Employee Sal 
        DataSet GetEmployeeSalInfo<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet AccessEmployeeSalInfo<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetEmployeeInfo<TParamOType>(TParamOType target) where TParamOType : class;

        // End Employee Sal


        // Start AdvanceInfo 

        DataSet GetAdvanceInfo<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet AccessAdvanceInfo<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetEmployeeList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetSectorList<TParamOType>(TParamOType target) where TParamOType : class;


        // End AdvanceInfo

        // Start Advance Sector

        DataSet GetAdvanceSector<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet AccessAdvanceSector<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet UpdateSavingSummary<TParamOType>(TParamOType target) where TParamOType : class;

        // End Advance Sector
        // SMS
        DataSet GetMessageType<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetMessageSubCategory<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet InsertUpdateMessage<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet SMSGetProductList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet SMSGetOfficeList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet InsertUpdateGroup<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetGroupList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GETMessage<TParamOType>(TParamOType target) where TParamOType : class;

        DataSet SENDMessage<TParamOType>(TParamOType target) where TParamOType : class;

        //SMS

        //OLRS
        //Loan Details 
        DataSet GetLoanDetails<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetConsentProductSavingList<TParamOType>(TParamOType target) where TParamOType : class;

        // CENTER LOG
        DataSet SaveCenterLog<TParamOType>(TParamOType target) where TParamOType : class;

        DataSet GetMemberImage<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet DeleteEmpOfficeMapping<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetFundTransferMappedOfficeList<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetSavingSummaryID<TParamOType>(TParamOType target) where TParamOType : class;

        List<DBFundLoanDetailsModel> GetFundLoanDetailsAsDateWise<TParamOType>(TParamOType target);
        List<DBMemberDetailModel> GetMemberAsListAgeWise<TParamOType>(TParamOType target);
        DataSet RunVoiceDataProcess<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class;

        DataSet SaveEditMemberImage<TParamOType>(TParamOType target) where TParamOType : class;

        int DeleteMRASubjectDetails<TParamOType>(TParamOType target) where TParamOType : class;
        int DeleteMRAContractDetails<TParamOType>(TParamOType target) where TParamOType : class;
    }
    public class UltimateReportService : IUltimateReportService
    {
        private readonly ICenterRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public UltimateReportService(ICenterRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public DataSet GetMemberBalanceInfoByProduct<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "GetMemberBalanceInfoByProduct";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet RunVoiceDataProcess<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GenerateNIDStatus<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SP_NIDVerification";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetSavingSummaryID<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_getSavingSummaryID";

            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataDailySavingCollectionReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Rpt_DailySavingsCollection";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GenerateMemberLastCodeWriteOff<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "GenerateMemberLastCodeWriteoff";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataTodaysSummaryMemberwiseReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Rpt_TodaySummary";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataRecoverableReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Rpt_Recoverable";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataMRACDBReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_MRACDB";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataMRACDB03Report<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_MRA_CDB_03";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataLoanAndSavingsBalanceSWReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Rpt_LoanAndSavingsBalanceSW";

            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataPOMISFiveAReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Rpt_POMIS5A";
            using (var gbData = new gBankerReportMIS())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataPOMISTargetAndAchievement<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Rpt_POMISTargetAndAchievement";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetLocationList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetLocationList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetOfficesForDropdownList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Office_GetOfficesForDropdownList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetMembersLoanInformation<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Rpt_MembersLoanInformation";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetRepaymentSchedule<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "GetRepaymentSchedule";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetProductList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getProductByMember";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetProductSavingList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getProductByMemberSavings";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetFundTransferMappedOfficeList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_FundTransferMappedOffice";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetConsentProductSavingList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getProductByMemberConsent";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetMemberPasBookList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getMemberPassBookNoByMember";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetMemberCashBookList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getAccLevelForCashBookByOfficeId";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetgetOrganizerList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getOrganizer";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetAccLevelByAccId<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getAccLevelByAccID";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet getAccLevelForBankBookByAccID<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getAccLevelForBankBookByAccID";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetBankBookList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getBankBookByOfficeId";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetProductListFromSavingSummary<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getProductByMemberFromSavingSummary";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetNewcategoryList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getProductByMemberCategory";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet Proc_GetSavingBalanceForCate<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetSavingBalanceForCate";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetNoAccount<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_getAccountNo";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet setCateGoryTransfer<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "CateGoryTransfer";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDailyLoanTrxlist<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetDailyLoanTrx";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet DeleteProcessCheck<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_DeleteProcessCheck";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDailySavinglist<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetDailySavingCollection";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet ValidateMainItemCode_21_list<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_VAlidate_21";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetWorkingLogInfo<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getWorkingLogInfo";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetMainProductCode<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getMainProductCode";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetAuditTrialo<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_get_AuditTRail";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetriteOffSummarylist<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getWriteOffDetails";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetCenterROleWise<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_getUserRole";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetPreWriteOffHistory<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getWriteOffList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetInsuranceSlot<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getInsuranceSlotList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet AccessInsuranceSlot<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "AccessInsuranceSlotList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet Set_GenerateReffNo<TParamOType>(TParamOType target) where TParamOType : class
        {
            //var storeProcedureName = "SP_GET_GenerateReffNo";

            var storeProcedureName = "SP_ReffNo";

            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetReconPurposeList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_get_ReconPurpose";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet SetSpecialLoanCollection<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "UpdateSpecialLOan";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDailyLoanTrxID<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_getDailyLoanTrxID";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetTodaysCollection<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetMemberWise";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet Set_LoanSummaryProposalAPI<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Set_LoanSummaryProposalAPI";

            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataDistributeAmount<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "UpdateDailyLoan_SavingsTRX_UserWise";

            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet validateLOanProposal<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "validate_loanProposal";

            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet UpdateSpecialLOan<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "UpdateSpecialLOan";

            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetKerPressValueForSpecialLoan<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SetSLCTxtKeyPressTemp";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetKerPressValueForSpecialLoanWeekly<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SetSLCTxtKeyPressWeekly";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetKerPressValueForSpecialLoanMonthly<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SetSLCTxtKeyPressMonthly";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet AccountCloseMemberList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_AccountCloseMemberList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet UpdateCenterIDInAllRelatedTable<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "updateCenterIDinAllRelatedTables";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet getCategoryByCategoryID<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getCategoryByCategoryID";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataMemberCollectionAdd<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Set_CollectionWithDrawal";

            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet validateSavingBalance<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_CheckSavingBalance";

            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetProductListByFrequencyMode<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getProductByFrequencyMode";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetProductListBySubCat<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getProductBySubCat";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetMainProductList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getMainProductList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetSubMainProductList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getSubMainProductList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetProductListT<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getSubMainProductListT";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetMemberDetails<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetMemberDetails";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public List<DBMemberDetailModel> GetMemberDetailsasList<TParamOType>(TParamOType target)
        {
            var sql = "Proc_GetMemberDetailsDateWise @orgID,@OfficeID,@filterColumnName,@filterValue,@TypeFilterColumn,@DateFrom,@DateTo";
            return repository.GetSqlResult<DBMemberDetailModel, TParamOType>(sql, target);
        }
        public List<DBMemberDetailModel> GetMemberDetailsasListEmployeeWise<TParamOType>(TParamOType target)
        {
            var sql = "Proc_GetMemberDetailsEmployeeWise @orgID,@OfficeID,@filterColumnName,@filterValue,@TypeFilterColumn,@EmployeeID";
            return repository.GetSqlResult<DBMemberDetailModel, TParamOType>(sql, target);
        }
        public List<DBMemberDetailModel> GetElegibleMemberDetailsasList<TParamOType>(TParamOType target)
        {
            var sql = "Proc_GetElegibleMemberDetails @orgID,@OfficeID,@filterColumnName,@filterValue,@TypeFilterColumn,@EmployeeID";
            return repository.GetSqlResult<DBMemberDetailModel, TParamOType>(sql, target);
        }
        public List<DBMemberDetailModel> GetElegibleMemberDetailsasListEmployeeWise<TParamOType>(TParamOType target)
        {
            var sql = "Proc_GetElegibleMemberDetailsEmployeeWise @orgID,@OfficeID,@filterColumnName,@filterValue,@TypeFilterColumn,@EmployeeID";
            return repository.GetSqlResult<DBMemberDetailModel, TParamOType>(sql, target);
        }
        public DataSet GetAccountCode<TParamOType>(TParamOType target) where TParamOType : class
        {

            var storeProcedureName = "Proc_getAccountCode";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet UpdateAccountInfo<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_UpdateAccInfoCorrection";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetEmployment<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "GetEmployment";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet DelSpecialLoanCollection<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_DelSpecialLoan";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet SetRebateCollection<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "RebateTransferToSavings";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetAccountCorrectionLog<TParamOType>(TParamOType target) where TParamOType : class
        {

            var storeProcedureName = "GetAccountCorrectionLog";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDisbursementCorrectionLog<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "GetLoanSummaryCorrectionLog";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetMemberCorrectionLog<TParamOType>(TParamOType target) where TParamOType : class
        {

            var storeProcedureName = "GetMemberCorrectionLog";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public List<DBCenterDetailModel> GetCenterListEmployeeWise<TParamOType>(TParamOType target)
        {
            var sql = "getCenterInfo @OrgID,@OfficeID,@EmployeeID,@Qtype";
            return repository.GetSqlResult<DBCenterDetailModel, TParamOType>(sql, target);
        }

        public List<DBholidayDetailModel> GetHolidayListCenterWise<TParamOType>(TParamOType target)
        {
            var sql = "getHoliday @OfficeID,@OrgID,@Qtype";
            return repository.GetSqlResult<DBholidayDetailModel, TParamOType>(sql, target);
        }

        public List<Organization> GetOrganizationDetails<TParamOType>(TParamOType target)
        {
            var sql = "Proc_get_OrgDetails @OrgID";
            return repository.GetSqlResult<Organization, TParamOType>(sql, target);
        }

        public DataSet GetLoanInstallmentmentCorrectionLog<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "GetLoanInstallmentCorrectionLog";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetSavingInstallmentCorrectionLog<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "GetSavingInstallmentCorrectionLog";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetOfficeListByEmployee<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_getZoneEmployeeWise";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetLastCenterCode<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Set_LastCenterCode_CenterType";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet UpdateLoanAccount<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "upDateLOanAccount";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet RiskDataTransferIntoSavings<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "RiskFundDataTransfer";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet DelRiskDataTransferIntoSavings<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "DelRiskData";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GenerateMemberLastCode<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "GenerateMemberLastCode";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public object GetLastInitialDate<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getLastInitialDate";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetScalarResult(storeProcedureName, target);
            }
        }

        public DataSet ValidateSavingSummary<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "ValidateSavingSummary";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet PenaltyWithDailySaving<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "DailySavingTrxWithPenalty";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet getLoanProposalListRoleWise<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getLoanProposalListRoleWise";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet getArroveCellingList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getApprovedCelling";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet PenaltyWithDailySavingBank<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "DailySavingTrxWithPenaltyBankInfo";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetDataWithoutParameter(string storeProcedureName)
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDatesetWithoutParam(storeProcedureName);
            }
        }

        public DataSet GetRebateInfo<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getRebateInfo";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet SetRebateInfo<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "setRebateInf";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetInsuranceRate<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getInterestRateList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet AccessInsuranceRate<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "AccessInterestRateList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetLTSPenaltySlot<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getLTSPenaltySlotList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet AccessLTSPenaltySlot<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "AccessLTSPenaltySlotList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet ExecuteSPBankINterestRate<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "AppendDailySavingForBankInterest";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet ExecuteSPBankINterestClear<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "AppendDailySavingForBankInterestClear";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetSpecialSavingLastBalance<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Get_SpecialSavingLastBalance";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet getMaxAccountNo<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getMaxAccountNo";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet Proc_getSavingLastBalance<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_get_getSavingLastBalance";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GenerateLoanAndSavingAccount<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "generateLoanAndSavingAccountNo";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet UpdateAccountInfoChanged<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_UpdateAccInfoCorrectionChanged";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetAccountCodeList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_getAccountCodeList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetAllAccountCodeList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_getALLAccountCodeList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetAccountCodeJCF<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_getAccountCodeJCF";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet ExecuteSPMonthlySaving<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "setMonthSaving";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public List<DBholidayDetailModel> GetHolidayListSearch<TParamOType>(TParamOType target)
        {
            var sql = "getHolidayBySerch @OfficeID,@OrgID,@Qtype,@SearchDate";
            return repository.GetSqlResult<DBholidayDetailModel, TParamOType>(sql, target);

        }

        //Mapping Section Starts
        public DataSet GetProductListForMapping<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetProductList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetLoanDetails<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetLoanList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetSelectedProductListForMapping<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetSelectedProductList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet SaveProductMapping<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_insertProductMapping";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet DeleteProductMapping<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_DeleteProductMapping";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetAccChartListForMapping<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetAccChartList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetAccChartListForMappingCash<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetAccChartListCash";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetSelectedAccChartListForMapping<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetSelectedAccChartList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetSelectedAccChartListForMappingCash<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetSelectedAccChartListCash";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet SaveAccChartMapping<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_insertAccChartMapping";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet SaveAccChartMappingCash<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_insertAccChartMappingCash";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet DeleteAccChartMapping<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_DeleteAccChartMapping";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet DeleteAccChartMappingCash<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_DeleteAccChartMappingCash";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetCenterTypeList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getCenterType";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetProductListAccordingToOffice<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetProductAccordingtoOffice";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetProductListByFrequencyModeAccoringTOOffice<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getProductByFrequencyModeAccordingToOffice";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetProductListBySubCatAccordingToOffice<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getProductBySubCatAccordingTOOffice";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetAccCodetListAccordingToOffice<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetBankCodeAccordingTOOffice";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetAccCodeListAccordingToOffice<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetAccountCodeAccordingToOffice";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetBankAccCodeListAccordingToOffice<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetBankAccountCodeAccordingToOffice";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetMainProductListAccordingToOffice<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getMainProductListAccordingToOffice";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetProductListTAccordingToOffice<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getSubMainProductListTAccordingTOOffice";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetOfficeByLevel<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getAccountCodeByLevel";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetOfficeByLevelCash<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getAccountCodeByLevelCash";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetAccCodeListAccordingToOfficeBudget<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetAccountCodeAccordingToOfficeForBudget";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        // Mapping Section Ends

        // Start EmployeeSalInfo
        public DataSet GetEmployeeSalInfo<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getEmployeeSalInfoList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet AccessEmployeeSalInfo<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "AccessEmployeeSalInfo";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetEmployeeInfo<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getEmployeeInfoByCode";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        // End EmployeeSalInfo
        // Start AdvanceInfo
        public DataSet GetAdvanceInfo<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getAdvanceInfoList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet AccessAdvanceInfo<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "AccessAdvanceInfo";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetEmployeeList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getEmployeeList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetSectorList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getSectorList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        // End AdvanceInfo

        //Start Advance Sector

        public DataSet GetAdvanceSector<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getAdvanceSectorList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet AccessAdvanceSector<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "AccessAdvanceSector";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetLoanDisburseTypeList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getLoanPaymentFrequecyForLoanDisbursementType";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet UpdateSavingSummary<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "updateSavingSummary";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet SetReconVoucher<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SetReconVoucher";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet getAccBuroCode<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getAccBuroCode";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet updateSavingInstallmentSap<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "updateSavingInstallmentSap";

            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        //END Advance Sector
        /// SAVING REGISTER UPDATE INFO 

        public DataSet InsertSAVINGRegisterUpdateINFO<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_InsertSavingRegisterUpdateInfo";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        /// SAVING REGISTER UPDATE INFO
        /// /// LOAN REGISTER UPDATE INFO 

        public DataSet InsertLOANRegisterUpdateINFO<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_InsertLoanRegisterUpdateInfo";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet AddApplicationSetting<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "addApplicationSetting";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        /// LOAN REGISTER UPDATE INFO 
        /// //Bkash DATA

        public DataSet GetBkashTransactionLog<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getBkashLogDetails";
            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetMaxLoanTermMainCodeWise<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getMaxLoanTerm";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet UpdateAccountInfoChangedInsert<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_UpdateAccInfoCorrectionChangedInsert";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        //Bkash DATA
        // SMS

        public DataSet GetMessageType<TParamOType>(TParamOType target) where TParamOType : class
        {

            var storeProcedureName = "SMS_GETMessageType";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetMessageSubCategory<TParamOType>(TParamOType target) where TParamOType : class
        {

            var storeProcedureName = "SMS_GETMessageSubCategory";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet InsertUpdateMessage<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SMS_CreateUpdateMessage";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet SMSGetProductList<TParamOType>(TParamOType target) where TParamOType : class
        {

            var storeProcedureName = "SMS_GETProductList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }



        public DataSet SMSGetOfficeList<TParamOType>(TParamOType target) where TParamOType : class
        {

            var storeProcedureName = "SMS_GETOfficeList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet InsertUpdateGroup<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SMS_CreateUpdateGroup";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetGroupList<TParamOType>(TParamOType target) where TParamOType : class
        {

            var storeProcedureName = "SMS_GETGroupList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GETMessage<TParamOType>(TParamOType target) where TParamOType : class
        {

            var storeProcedureName = "SMS_GETMessage";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet SENDMessage<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SMS_SENDMessage";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetZoneList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getZoneList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetMembersPassBookRegisterInformation<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getPassBookRegisterDateBetween";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public List<DBMemberDetailModel> GetMemberDetailsasListEmployeeDateWise<TParamOType>(TParamOType target)
        {
            var sql = "Proc_GetMemberDetailsEmployeeDateWise @orgID,@OfficeID,@filterColumnName,@filterValue,@TypeFilterColumn,@EmployeeID,@DateFrom,@DateTo";
            return repository.GetSqlResult<DBMemberDetailModel, TParamOType>(sql, target);

        }

        public DataSet SMSUpdate<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet DelSavingSummary<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "DelSavingSummary";

            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetSMSDataWithParameter<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetLoanSummaryID<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_getLoanSummaryID";

            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet DataInsertintoDailyLoanTrx<TParamOType>(TParamOType target) where TParamOType : class
        {
            try
            {
                var storeProcedureName = "Proc_SetDailyLoanTrxInsertWithBank";

                using (var gbData = new gBankerDataAccess())
                {
                    return gbData.GetDataOnDateset(storeProcedureName, target);
                }
            }
            catch (Exception ex)

            {
                var storeProcedureName = "Proc_SetDailyLoanTrxInsertWithBank";
                using (var gbData = new gBankerDataAccess())
                {
                    return gbData.GetDataOnDateset(storeProcedureName, target);
                }
            }

        }

        public DataSet GetKerPressValueForSpecialLoanMonthlyFlyTable<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SetSLCTxtKeyPressMonthlyFlyTable";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetKerPressValueForSpecialLoanWeeklyFlyTable<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SetSLCTxtKeyPressWeeklyFlyTable";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataDailyLoantrx<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getFromDailyLoanTrx";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet Proc_Set_SavingOpeingWhenMemberEligible<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Set_SavingOpeingWhenMemberEligible";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataWithParameterMRA<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet SavingReinstate<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "ReInstate";

            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet InsertTabUploadData<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GenerateMemberLastCodeMember<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "GenerateMemberLastCode";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet SetSurvey<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDailyCenterList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getDailyCenterList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetIdentityTypeList<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet DataInsertintoEmployee<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "setEmployeeSync";

            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetStopClaimableInterestValidationGUK<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "stopClaimableValidationGuk";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDailySavingCenterList<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getDailySavingCenterList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet CheckDuplicateMember<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "CheckIDS";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet UpdateMemberProshikha<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "updateMemberName";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet UpdateMemberProshikhaMemberName<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "UpdateMemberName";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetAccountCodeListbudgetAndParticularwise<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetAccountCodeListbudgetAndParticularwise";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetAccCodeListAccordingToOfficeBudgetForProgram<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetAccountCodeAccordingToOfficeForBudgetForProgram";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GenerateMemberLastCodeMemberSSS<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "GenerateMemberLastCodeSSS";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet validateMemberPassbook<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getExistingMemberPassbookNo";

            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        // END SMS


        public DataSet SaveCenterLog<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_InsertCenterLog";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetMemberImage<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getDropMemberImage";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet GetSelectedOfficeListForMapping<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetSelectedOfficeList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet DeleteEmpOfficeMapping<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_DeleteEmpOfficeMapping";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        //FundLoanDetailsDateWise
        public List<DBFundLoanDetailsModel> GetFundLoanDetailsAsDateWise<TParamOType>(TParamOType target)
        {
            var sql = "Proc_FundLoanDetailsDateWise @DateFrom,@DateTo";
            return repository.GetSqlResult<DBFundLoanDetailsModel, TParamOType>(sql, target);
        }

        public List<DBMemberDetailModel> GetMemberAsListAgeWise<TParamOType>(TParamOType target)
        {
            var sql = "Proc_GetMemberListAgeWise @OfficeID,@CenterID,@AgeFrom,@AgeTo";
            return repository.GetSqlResult<DBMemberDetailModel, TParamOType>(sql, target);
        }

        public DataSet GetProductSavingList_SDC<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "getProductByMemberSavings_RoleWise_SDC";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataWithParameterCollection<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet PenaltyWithDailySavingBank_Dormant<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "DailySavingTrxWithPenaltyBankInfo_Dormant";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet DailyRecoverableAndRecoveryRegisterEmployeeCenterDateWise<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SP_RecoverableRecoveryRegisterDateWise";

            using (var gbData = new gBankerReportDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet SaveEditMemberImage<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_SaveEdit_MemberImage";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GenerateNIDStatusForAdminOnly<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "dbo.SP_NIDVerification_AdminOnly ";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataWithParameterMRAJCF<TParamOType>(TParamOType target, string storeProcedureName) where TParamOType : class
        {
            using (var gbData = new gBankerPKSF())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet ValidateSavingSummaryMonthlyInterest<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_SavingBankInterest_Validate";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet PenaltyWithDailySavingBank_Adjustment<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "DailySavingTrxWithPenaltyBankInfo_Adjustment";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
        public DataSet DataInsertintoDailyLoanTrx_Adjustment<TParamOType>(TParamOType target) where TParamOType : class
        {
            try
            {
                var storeProcedureName = "Proc_SetDailyLoanTrxInsertWithBank_Adjustment";

                using (var gbData = new gBankerDataAccess())
                {
                    return gbData.GetDataOnDateset(storeProcedureName, target);
                }
            }
            catch (Exception ex)
            {
                var storeProcedureName = "Proc_SetDailyLoanTrxInsertWithBank_Adjustment";
                using (var gbData = new gBankerDataAccess())
                {
                    return gbData.GetDataOnDateset(storeProcedureName, target);
                }
            }

        }

        public DataSet DelSpecialLoanCollectionAdjustment<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_DelSpecialLoanAdjustment";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GenerateNIDStatusEligible<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "SP_NIDVerificationEligible";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public int DeleteMRASubjectDetails<TParamOType>(TParamOType target) where TParamOType : class
        {
            try
            {
                var storeProcedureName = "usp_Generate_MRASubjectDetails";
                using (var gbData = new gBankerDataAccess())
                {
                    return gbData.ExecuteNonQuery(storeProcedureName, target);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int DeleteMRAContractDetails<TParamOType>(TParamOType target) where TParamOType : class
        {
            try
            {
                var storeProcedureName = "usp_Generate_MRAContractDetails";
                using (var gbData = new gBankerDataAccess())
                {
                    return gbData.ExecuteNonQuery(storeProcedureName, target);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}



    