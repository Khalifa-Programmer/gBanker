using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
using gBanker.Web.Models;
using gBanker.Web.ViewModels;
using gBankerCodeFirstMigration.Db;

namespace gBanker.Web.Mappings
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }
        protected override void Configure()
        {

            Mapper.CreateMap<DailySavingTrxViewModel, DailySavingTrx>();
            Mapper.CreateMap<MemberCategoryViewModel, MemberCategory>();
            Mapper.CreateMap<InvestorViewModel, gBanker.Data.CodeFirstMigration.Db.Investor>();
            Mapper.CreateMap<ProductViewModel, Product>();
            Mapper.CreateMap<ProductViewModelEditMode, Product>();
            Mapper.CreateMap<SavingsAccountOpeningViewModel, DBSavingSummaryDetails>();
            Mapper.CreateMap<LoanSummaryViewModel, LoanSummary>();
            Mapper.CreateMap<LoanSummaryViewModel, DBLoanApproveDetailModel>();
            Mapper.CreateMap<EmployeeViewModel, Employee>();
            //Mapper.CreateMap<PurposeViewModel, Purpose>();
            Mapper.CreateMap<BranchViewModel, Branch>();
            Mapper.CreateMap<PurposeViewModel, Purpose>();
            Mapper.CreateMap<AreaViewModel,Area>();
            Mapper.CreateMap<LoanApprovalViewModel, LoanSummary>();
            Mapper.CreateMap<LoanDisburseViewModel, LoanSummary>();
            Mapper.CreateMap<LoanDisburseViewModel, Proc_get_LoanDisburse_Result>();
           // Mapper.CreateMap<getEmployeeViewModel, getEmployeeInfo_Result>();
            Mapper.CreateMap<SavingSummaryViewModel, SavingSummary>();
            Mapper.CreateMap<SavingSummaryViewModel, DBSavingSummaryDetails>();
            Mapper.CreateMap<LoanApprovalViewModel, DBLoanApproveDetailModel>();
            Mapper.CreateMap<OfficeViewModel, Office>();
            Mapper.CreateMap<CenterViewModel, Center>();
            Mapper.CreateMap<MemberViewModel, Member>();
            Mapper.CreateMap<HolidayViewModel, Holiday>();
            Mapper.CreateMap<DailyLoanTrxViewModel, DailyLoanTrx>();
            Mapper.CreateMap<DailySavingCollectionViewModel, DailySavingTrx>();
            Mapper.CreateMap<SpecialLoanCollectionViewModel, DbSpecialLoanCollectionDetailModel>();
            Mapper.CreateMap<SpecialLoanCollectionViewModel, DailyLoanTrx>();
            Mapper.CreateMap<SpecialSavingCollectionViewModel, DailySavingTrx>();
            Mapper.CreateMap<SpecialLoanCollectionViewModel, proc_get_SpecialLoanCollection_Result>();
            Mapper.CreateMap<SpecialLoanCollectionViewModel, LoanSummary>();
            Mapper.CreateMap<SpecialSavingCollectionViewModel, proc_get_SpecialSavingCollection_Result>();
            Mapper.CreateMap<DayInitialViewModel, Prcs_DayInitial_Result>();
            Mapper.CreateMap<DayEndViewModel, Prcs_DayEnd_Result>();
            Mapper.CreateMap<AccChartViewModel, AccChart>();
            Mapper.CreateMap<TodaysSummaryViewModel,  Proc_GetRptTodaySummary_Result>();
           
            Mapper.CreateMap<AccountCloseViewModel, SavingSummary>();
            Mapper.CreateMap<LoanLedgerViewModel, Proc_Rpt_LoanLedger_Result>();
            Mapper.CreateMap<AccTrxMasterViewModel,AccTrxMaster>();
            Mapper.CreateMap<AccVoucherEntryViewModel, AccTrxMaster>();

            Mapper.CreateMap<AspNetRoleModuleViewModel, AspNetRoleModule>();
            Mapper.CreateMap<AspNetSecurityModuleViewModel, AspNetSecurityModule>();

            Mapper.CreateMap<ApplicationSettingViewModel, ApplicationSetting>();
            Mapper.CreateMap<ApplicationSettingViewModel, DBApplicationSettingsDetail>();
            Mapper.CreateMap<GroupViewModel, Group>();
            Mapper.CreateMap<GroupViewModel, DBGroupDeatil>();

            Mapper.CreateMap<SavingsAccountOpeningViewModel, SavingSummary>();
            Mapper.CreateMap<CategoryTransferViewModel, getTransferHistory_Result>();
            Mapper.CreateMap<CategoryTransferViewModel, TransferHistory>();
            Mapper.CreateMap<AccNoteViewModel, AccNote>();
            Mapper.CreateMap<SpecialLoanCollectionViewModel, LoanCorrectionTrx>();
            Mapper.CreateMap<SpecialSavingCollectionViewModel, SavingCorrectionTrx>();
            Mapper.CreateMap<LoanCorrectionViewModel, LoanCorrectionTrx>();
            Mapper.CreateMap<LoanCorrectionViewModel, DailyLoanTrx>();
            Mapper.CreateMap<LoanCorrectionViewModel, proc_get_LoanCorrection_Result>();
            Mapper.CreateMap<SavingCorrectionViewModel, getSavingsCorrection_Result>();
            Mapper.CreateMap<SavingCorrectionViewModel, SavingCorrectionTrx>();
            Mapper.CreateMap<MemberLedgerViewModel, getGetLoanLedgerMemberWise_Result>();
            Mapper.CreateMap<AccountCloseViewModel, DBSavingSummaryDetails>();
            Mapper.CreateMap<RepaymentScheduleViewModel, RepaymentSchedule>();
            Mapper.CreateMap<DashboardViewModel, DashBoard>();
            Mapper.CreateMap<RepaymentScheduleViewModel, GetRepaymentSchedule_Result>();
            Mapper.CreateMap<CreditScoreViewModel, CreditScore>();
            Mapper.CreateMap<FamilyGraceViewModel, FamilyGrace>();
            Mapper.CreateMap<FamilyGraceViewModel, getFamilyGrace_Result>();
            Mapper.CreateMap<SmsConfigViewModel, SmsConfig>();
            Mapper.CreateMap<SmsLogViewModel, SmsLog>();
            Mapper.CreateMap<WriteOffDeclarationViewModel, LoanSummary>();
            Mapper.CreateMap<ExpireInfoViewModel, ExpireInfo>();
            Mapper.CreateMap<ExpireInfoViewModel, getExpireInfo_Result>();
            Mapper.CreateMap<BudgetViewModel, Budget>();
            Mapper.CreateMap<MemberPassBookRegisterViewModel, MemberPassBookRegister>();
            Mapper.CreateMap<MemberPassBookRegisterViewModel, getPassBookRegister_Result>();
            Mapper.CreateMap<AccountCloseViewModel, getSavingCloseAccountInfo_Result>();
            Mapper.CreateMap<FileUploadViewModel, FileUpload>();
            Mapper.CreateMap<MemberPassBookStockViewModel, MemberPassBookStock>();
            Mapper.CreateMap<MemberPassBookStockViewModel, getMemberPassBookStock_Result>();

            Mapper.CreateMap<AccAccountDetailsViewModel, Proc_Get_AccountDetails_Result>();
            Mapper.CreateMap<MiscellaneouViewModel, Proc_get_Miscellaneou_Result>();
            Mapper.CreateMap<MiscellaneouViewModel, Miscellaneou>();
            Mapper.CreateMap<WorkingAreaLogViewModel, WorkingAreaLog>();
            Mapper.CreateMap<AccReconcileViewModel, AccReconcile>();
            Mapper.CreateMap<PartialLoanDisbursementViewModel, DBLoanApproveDetailModel>();
            Mapper.CreateMap<PartialLoanDisbursementViewModel, LoanSummary>();
            Mapper.CreateMap<CumMISViewModel, CumMi>();
            Mapper.CreateMap<CumMISViewModel, Proc_Get_CUMMIS_Result>();
            Mapper.CreateMap<CumAISViewModel, CumAI>();
            Mapper.CreateMap<CumAISViewModel, Proc_Get_CUMAIS_Result>();
            Mapper.CreateMap<OrganizationViewModel, Organization>();
            Mapper.CreateMap<LoanProposalListViewModel, getLoanProposalListRoleWise_Result>();
            Mapper.CreateMap<ApproveCellingViewModel, ApproveCelling>();
            Mapper.CreateMap<MemberCorrectionForMemberViewModel, Member>();
            Mapper.CreateMap<SurveyMemberMasterViewModel, SurveyMemberMaster>();
            Mapper.CreateMap<MFIInfoViewModel, MFIInformation>();
            Mapper.CreateMap<WelfareActivityViewModel, WelfareActivityDetail>();
            Mapper.CreateMap<RemittanceActivityViewModel, RemittanceActivity>();
            Mapper.CreateMap<TrainingViewModel, Training>();
            Mapper.CreateMap<MFIInfoViewModel, MFIInformation>();
            Mapper.CreateMap<EmployeeViewModel, EmployeeDetail>();
            Mapper.CreateMap<LoanApprovalEligibleViewModel, LoanSummary>();
            Mapper.CreateMap<SavingTrxViewModel, SavingTrx>();
            Mapper.CreateMap<LoanTrxViewModel, LoanTrx>();
            Mapper.CreateMap<RebateViewModel, DailyLoanTrx> ();
            Mapper.CreateMap<RebateViewModel, LoanSummary > ();
            Mapper.CreateMap<RebateViewModel, proc_get_SpecialLoanCollection_Result>();

            Mapper.CreateMap<StatisticsReportDetailsViewModel, StatisticsReportDetails>();

            Mapper.CreateMap<CIBViewModel, Member>();
            Mapper.CreateMap<BuroMemberViewModel, Member>();
            Mapper.CreateMap<RegisterModel, AspNetUser>();
            Mapper.CreateMap<StopInterestViewModel, StopInterest>();
            Mapper.CreateMap<LegalInfoViewModel, LegalInfo>();
            Mapper.CreateMap<ProductXEmploymentProductMappingViewModel, ProductXEmploymentProductMapping>();
            Mapper.CreateMap<MemberWisePassbookBalanceEntryViewModel, MemberWisePassbookBalanceEntry>();
            Mapper.CreateMap<FundTransferMappingViewModel, FundTransferMapping>();
        }
    }
}