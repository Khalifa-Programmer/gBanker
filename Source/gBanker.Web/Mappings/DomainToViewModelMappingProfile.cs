using AutoMapper;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
using gBanker.Web.Models;
using gBanker.Web.ViewModels;
using gBankerCodeFirstMigration.Db;

namespace gBanker.Web.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<DailySavingTrx, DailySavingTrxViewModel>();
            Mapper.CreateMap<MemberCategory, MemberCategoryViewModel>();
            Mapper.CreateMap<Investor, InvestorViewModel>();
            Mapper.CreateMap<Product, ProductViewModel>();
            Mapper.CreateMap<Product, ProductViewModelEditMode>();
            Mapper.CreateMap<LoanSummary, LoanSummaryViewModel>();
            Mapper.CreateMap<DBLoanApproveDetailModel, LoanSummaryViewModel>();
            Mapper.CreateMap<DBSavingSummaryDetails, SavingsAccountOpeningViewModel>();
            Mapper.CreateMap<Employee, EmployeeViewModel>();
            //Mapper.CreateMap<Purpose, PurposeViewModel>();
            Mapper.CreateMap<Branch, BranchViewModel>();
            Mapper.CreateMap<Purpose, PurposeViewModel>();
            Mapper.CreateMap<Area, AreaViewModel>();
            Mapper.CreateMap<LoanSummary, LoanApprovalViewModel>();
            Mapper.CreateMap<LoanSummary, LoanDisburseViewModel>();
            Mapper.CreateMap<Proc_get_LoanDisburse_Result, LoanDisburseViewModel>();
           // Mapper.CreateMap<getEmployeeInfo_Result, getEmployeeViewModel>();
            Mapper.CreateMap<SavingSummary, SavingSummaryViewModel>();
            Mapper.CreateMap<DBSavingSummaryDetails, SavingSummaryViewModel>();
            Mapper.CreateMap<DBLoanApproveDetailModel, LoanApprovalViewModel>();
            Mapper.CreateMap<Center, CenterViewModel>();
            Mapper.CreateMap<Holiday, HolidayViewModel>();
            Mapper.CreateMap<Office, OfficeViewModel>();
            Mapper.CreateMap<Member, MemberViewModel>();
            Mapper.CreateMap<DailyLoanTrx, DailyLoanTrxViewModel>();
            Mapper.CreateMap<DailySavingTrx, DailySavingCollectionViewModel>();
            Mapper.CreateMap<DailySavingTrx, SavingCorrectionViewModel>();
            Mapper.CreateMap<DbSpecialLoanCollectionDetailModel, SpecialLoanCollectionViewModel>();
            Mapper.CreateMap<DailyLoanTrx, SpecialLoanCollectionViewModel>();
            Mapper.CreateMap<LoanSummary, SpecialLoanCollectionViewModel>();
            Mapper.CreateMap<DailySavingTrx, SpecialSavingCollectionViewModel>();
            Mapper.CreateMap<proc_get_SpecialLoanCollection_Result, SpecialLoanCollectionViewModel>();
            Mapper.CreateMap<proc_get_SpecialSavingCollection_Result, SpecialSavingCollectionViewModel>();
            Mapper.CreateMap<Prcs_DayInitial_Result, DayInitialViewModel>();
            Mapper.CreateMap<Prcs_DayEnd_Result, DayEndViewModel>();
            Mapper.CreateMap<AccChart, AccChartViewModel>();
            Mapper.CreateMap<Proc_GetRptTodaySummary_Result, TodaysSummaryViewModel>();
            Mapper.CreateMap<SavingSummary, AccountCloseViewModel>();
            Mapper.CreateMap<Proc_Rpt_LoanLedger_Result, LoanLedgerViewModel>();
            Mapper.CreateMap<AccTrxMaster, AccTrxMasterViewModel>();
            Mapper.CreateMap<AccTrxMaster, AccVoucherEntryViewModel>();
            Mapper.CreateMap<AspNetRoleModule, AspNetRoleModuleViewModel>();
            Mapper.CreateMap<AspNetSecurityModule, AspNetSecurityModuleViewModel>();
            Mapper.CreateMap<ApplicationSetting, ApplicationSettingViewModel>();
            Mapper.CreateMap<DBApplicationSettingsDetail, ApplicationSettingViewModel>();
            Mapper.CreateMap<Group, GroupViewModel>();
            Mapper.CreateMap<DBGroupDeatil, GroupViewModel>();
            Mapper.CreateMap<SavingSummary, SavingsAccountOpeningViewModel>();
            Mapper.CreateMap<TransferHistory, CategoryTransferViewModel>();
            Mapper.CreateMap<getTransferHistory_Result, CategoryTransferViewModel>();
            Mapper.CreateMap<AccNote, AccNoteViewModel>();
            Mapper.CreateMap<LoanCorrectionTrx, SpecialLoanCollectionViewModel>();
            Mapper.CreateMap<SavingCorrectionTrx, SpecialSavingCollectionViewModel>();
            Mapper.CreateMap<LoanCorrectionTrx, LoanCorrectionViewModel>();
            Mapper.CreateMap<proc_get_LoanCorrection_Result, LoanCorrectionViewModel>();
            Mapper.CreateMap<getSavingsCorrection_Result, SavingCorrectionViewModel>();
            Mapper.CreateMap<SavingCorrectionTrx, SavingCorrectionViewModel>();
            Mapper.CreateMap<getGetLoanLedgerMemberWise_Result, MemberLedgerViewModel>();
            Mapper.CreateMap<DBSavingSummaryDetails, AccountCloseViewModel>();
            Mapper.CreateMap<RepaymentSchedule, RepaymentScheduleViewModel>();
            Mapper.CreateMap<DashBoard, DashboardViewModel>();
            Mapper.CreateMap<GetRepaymentSchedule_Result, RepaymentScheduleViewModel>();
            Mapper.CreateMap<CreditScore, CreditScoreViewModel>();
            Mapper.CreateMap<FamilyGrace, FamilyGraceViewModel>();
            Mapper.CreateMap<getFamilyGrace_Result, FamilyGraceViewModel>();
            Mapper.CreateMap<SmsConfig, SmsConfigViewModel>();
            Mapper.CreateMap<SmsLog, SmsLogViewModel>();
            Mapper.CreateMap<LoanSummary, WriteOffDeclarationViewModel>();
            Mapper.CreateMap<ExpireInfo, ExpireInfoViewModel>();
            Mapper.CreateMap<getExpireInfo_Result, ExpireInfoViewModel>();
            Mapper.CreateMap<Budget, BudgetViewModel>();
            Mapper.CreateMap<MemberPassBookRegister, MemberPassBookRegisterViewModel>();
            Mapper.CreateMap<getPassBookRegister_Result, MemberPassBookRegisterViewModel>();
            Mapper.CreateMap<getSavingCloseAccountInfo_Result, AccountCloseViewModel>();
            Mapper.CreateMap<FileUpload, FileUploadViewModel>();
            Mapper.CreateMap<MemberPassBookStock, MemberPassBookStockViewModel>();
            Mapper.CreateMap<getMemberPassBookStock_Result, MemberPassBookStockViewModel>();
            Mapper.CreateMap<Proc_Get_AccountDetails_Result, AccAccountDetailsViewModel>();
            Mapper.CreateMap<Proc_get_Miscellaneou_Result, MiscellaneouViewModel>();
            Mapper.CreateMap<Miscellaneou, MiscellaneouViewModel>();
            Mapper.CreateMap<WorkingAreaLog, WorkingAreaLogViewModel>();
            Mapper.CreateMap<AccReconcile, AccReconcileViewModel>();
            Mapper.CreateMap<DBLoanApproveDetailModel, PartialLoanDisbursementViewModel>();
            Mapper.CreateMap<LoanSummary, PartialLoanDisbursementViewModel>();
            Mapper.CreateMap<CumMi, CumMISViewModel>();
            Mapper.CreateMap<Proc_Get_CUMMIS_Result, CumMISViewModel>();
            Mapper.CreateMap<CumAI, CumAISViewModel>();
            Mapper.CreateMap<Proc_Get_CUMAIS_Result, CumAISViewModel>();
            Mapper.CreateMap<Organization, OrganizationViewModel>();
            Mapper.CreateMap<getLoanProposalListRoleWise_Result, LoanProposalListViewModel>();
            Mapper.CreateMap<ApproveCelling, ApproveCellingViewModel>();
            Mapper.CreateMap<Member, MemberCorrectionForMemberViewModel>();
            Mapper.CreateMap<SurveyMemberMaster, SurveyMemberMasterViewModel>();
            Mapper.CreateMap<MFIInformation, MFIInfoViewModel>();
            Mapper.CreateMap<WelfareActivityDetail, WelfareActivityViewModel>();
            Mapper.CreateMap<RemittanceActivity, RemittanceActivityViewModel>();
            Mapper.CreateMap<Training, TrainingViewModel>();
            Mapper.CreateMap<MFIInformation, MFIInfoViewModel>();
            Mapper.CreateMap<EmployeeDetail, EmployeeViewModel>();
            Mapper.CreateMap<LoanSummary, LoanApprovalEligibleViewModel>();
            Mapper.CreateMap<SavingTrx, SavingTrxViewModel>();
            Mapper.CreateMap<LoanTrx, LoanTrxViewModel>();
            Mapper.CreateMap<DailyLoanTrx, RebateViewModel>();
            Mapper.CreateMap<LoanSummary, RebateViewModel>();
            Mapper.CreateMap<proc_get_SpecialLoanCollection_Result, RebateViewModel>();


            Mapper.CreateMap<StatisticsReportDetails, StatisticsReportDetailsViewModel>();

            Mapper.CreateMap<Member, CIBViewModel>();
            Mapper.CreateMap<Member, BuroMemberViewModel>();
            Mapper.CreateMap<AspNetUser, RegisterModel>();
            Mapper.CreateMap<StopInterest, StopInterestViewModel>();
            Mapper.CreateMap<LegalInfo, LegalInfoViewModel>();
            Mapper.CreateMap<ProductXEmploymentProductMapping, ProductXEmploymentProductMappingViewModel>();
            Mapper.CreateMap<MemberWisePassbookBalanceEntry, MemberWisePassbookBalanceEntryViewModel>();
            Mapper.CreateMap<FundTransferMapping, FundTransferMappingViewModel>();

        }
    }
}