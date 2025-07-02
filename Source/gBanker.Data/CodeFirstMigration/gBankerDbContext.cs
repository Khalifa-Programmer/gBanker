namespace gBanker.Data.CodeFirstMigration.Db
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using gBankerCodeFirstMigration.Db;


    public partial class gBankerDbContext : DbContext
    {
        public gBankerDbContext()
            : base("name=gBankerDbContext")
        {
        }
        public virtual DbSet<RDOffice> RDOffice { get; set; }
        public DbSet<BuroCenterInfo> BuroCenterInfos { get; set; }
        // public DbSet<BuroCustomerInfo> BuroCustomerInfos { get; set; }
        public DbSet<BuroStaffInfo> BuroStaffInfos { get; set; }
        public DbSet<ReportSpName> ReportSpNames { get; set; }
        public virtual DbSet<MobileErrorLog> MobileErrorLogs { get; set; }
        public virtual DbSet<MobileSyncLog> MobileSyncLogs { get; set; }
        public virtual DbSet<MobileSyncLogDetail> MobileSyncLogDetails { get; set; }

        public virtual DbSet<ReconPurpose> ReconPurposes { get; set; }
        public virtual DbSet<AccReconcile> AccReconciles { get; set; }
        public virtual DbSet<WorkingAreaLog> WorkingAreaLogs { get; set; }
        public virtual DbSet<Miscellaneou> Miscellaneous { get; set; }
        public virtual DbSet<MemberPassBookStock> MemberPassBookStocks { get; set; }
        public virtual DbSet<FileUpload> FileUploads { get; set; }
        public virtual DbSet<FamilyGrace> FamilyGraces { get; set; }
        public virtual DbSet<AccCategory> AccCategories { get; set; }
        public virtual DbSet<AccChart> AccCharts { get; set; }
        public virtual DbSet<AccLastVoucher> AccLastVouchers { get; set; }
        public virtual DbSet<AccNote> AccNotes { get; set; }
        public virtual DbSet<AccTrxDetail> AccTrxDetails { get; set; }
        public virtual DbSet<AccTrxMaster> AccTrxMasters { get; set; }
        public virtual DbSet<ApplicationSetting> ApplicationSettings { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<AspNetRoleModule> AspNetRoleModules { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetSecurityLevel> AspNetSecurityLevels { get; set; }
        public virtual DbSet<AspNetSecurityModule> AspNetSecurityModules { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Budget> Budgets { get; set; }
        public virtual DbSet<Center> Centers { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<DailyLoanTrx> DailyLoanTrxes { get; set; }
        public virtual DbSet<DailySavingTrx> DailySavingTrxes { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<ExpireInfo> ExpireInfoes { get; set; }
        public virtual DbSet<GeoLocation> GeoLocations { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Holiday> Holidays { get; set; }
        public virtual DbSet<Investor> Investors { get; set; }
        public virtual DbSet<LoanSummary> LoanSummaries { get; set; }
        public virtual DbSet<LgVillage> LgVillages { get; set; }
        public virtual DbSet<LoanTrx> LoanTrxes { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<MemberAssetInfo> MemberAssetInfoes { get; set; }
        public virtual DbSet<MemberCategory> MemberCategories { get; set; }
        public virtual DbSet<MemberFamilyInfo> MemberFamilyInfoes { get; set; }
        public virtual DbSet<MemberHouseInfo> MemberHouseInfoes { get; set; }
        public virtual DbSet<MemberLandInfo> MemberLandInfoes { get; set; }
        public virtual DbSet<RepaymentSchedule> RepaymentSchedules { get; set; }
        public virtual DbSet<MemberLastCode> MemberLastCodes { get; set; }
        public virtual DbSet<Office> Offices { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<PNMConfirm> PNMConfirms { get; set; }
        public virtual DbSet<PNMOrder> PNMOrders { get; set; }
        public virtual DbSet<ProcessInfo> ProcessInfoes { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Purpose> Purposes { get; set; }
        //public virtual DbSet<RepaymentSchedule> RepaymentSchedules { get; set; }
        public virtual DbSet<SavingSummary> SavingSummaries { get; set; }
        public virtual DbSet<SavingTrx> SavingTrxes { get; set; }
        public virtual DbSet<Scheduler> Schedulers { get; set; }
        public virtual DbSet<SmsConfig> SmsConfigs { get; set; }
        public virtual DbSet<SmsLog> SmsLogs { get; set; }
        public virtual DbSet<SchedulerDetail> SchedulerDetails { get; set; }
        public virtual DbSet<EmployeeOfficeMapping> EmployeeOfficeMappings { get; set; }
        public virtual DbSet<TransferHistory> TransferHistories { get; set; }
        public virtual DbSet<WeekNo> WeekNoes { get; set; }
        public virtual DbSet<ApplicationLog> ApplicationLogs { get; set; }
        public virtual DbSet<DashBoard> DashBoards { get; set; }
        public virtual DbSet<AspNetOrgModule> AspNetOrgModules { get; set; }
        public virtual DbSet<MemberPassBookRegister> MemberPassBookRegisters { get; set; }
        public virtual DbSet<CumAI> CumAIS { get; set; }
        public virtual DbSet<CumMi> CumMis { get; set; }
        public virtual DbSet<ApproveCelling> ApproveCellings { get; set; }
        public virtual DbSet<SurveyMemberMaster> SurveyMemberMasters { get; set; }
        public virtual DbSet<SurveyMemberVerification> SurveyMemberVerifications { get; set; }
        public virtual DbSet<SurveyMemberFamilyInformation> SurveyMemberFamilyInformations { get; set; }
        public virtual DbSet<SurveyMemberAccomodationInformation> SurveyMemberAccomodationInformation { get; set; }
        public virtual DbSet<View_SurveyMemberMaster> View_SurveyMemberMasters { get; set; }
        public virtual DbSet<SurveyMemberAsset> SurveyMemberAssets { get; set; }
        public virtual DbSet<SurveyMemberExpenditure> SurveyMemberExpenditures { get; set; }
        public virtual DbSet<SurveyMemberOperationwithOtherNGOInformation> SurveyMemberOperationwithOtherNGOInformations { get; set; }
        public virtual DbSet<InstituteType> InstituteTypes { get; set; }
        public virtual DbSet<Institute> Institutes { get; set; }
        public virtual DbSet<SurveyMemberFamilyEducationInformation> SurveyMemberFamilyEducationInformations { get; set; }
        public virtual DbSet<SurveyKnownMember> SurveyKnownMembers { get; set; }
        public virtual DbSet<View_SurveyMemberFamilyInformation> View_SurveyMemberFamilyInformations { get; set; }
        public virtual DbSet<View_SurveyMemberAccomodationInformation> View_SurveyMemberAccomodationInformations { get; set; }
        public virtual DbSet<View_SurveyMemberAsset> View_SurveyMemberAssets { get; set; }
        public virtual DbSet<View_SurveyMemberExpenditure> View_SurveyMemberExpenditures { get; set; }
        public virtual DbSet<View_SurveyMemberOperationwithOtherNGOInformation> View_SurveyMemberOperationwithOtherNGOInformations { get; set; }
        public virtual DbSet<View_SurveyMemberFamilyEducationInformation> View_SurveyMemberFamilyEducationInformations { get; set; }
        public virtual DbSet<View_SurveyKnownMember> View_SurveyKnownMembers { get; set; }
        public virtual DbSet<CumMisItem> CumMisItems { get; set; }
        public virtual DbSet<BatchPostingProcess> BatchPostingProcesss { get; set; }
        public virtual DbSet<MRAActivityList> MRAActivityList { get; set; }
        public virtual DbSet<WelfareActivityDetail> WelfareActivityDetail { get; set; }
        public virtual DbSet<RemittanceActivity> RemittanceActivity { get; set; }
        public virtual DbSet<Training> Training { get; set; }
        public virtual DbSet<MFIInformation> MFIInformation { get; set; }
        public virtual DbSet<PrimaryRegistration> PrimaryRegistration { get; set; }
        public virtual DbSet<Particular> Particular { get; set; }
        public virtual DbSet<TargetAchievement> TargetAchievement { get; set; }
        public virtual DbSet<TargetAchievementBuro> TargetAchievementBuroes { get; set; }
        public virtual DbSet<AssetGroupInfo> AssetGroupInfo { get; set; }
        public virtual DbSet<LastAssetCodeInfo> LastAssetCodeInfo { get; set; }
        public virtual DbSet<AssetInfo> AssetInfo { get; set; }
        public virtual DbSet<AssetClientInfo> AssetClientInfo { get; set; }
        public virtual DbSet<TransactionType> TransactionType { get; set; }
        public virtual DbSet<FixAssetUpdates> FixAssetUpdates { get; set; }
        public virtual DbSet<DailyTransaction> DailyTransaction { get; set; }
        public virtual DbSet<AssetRegister> AssetRegister { get; set; }
        public virtual DbSet<DepreciationMethod> DepreciationMethod { get; set; }
        public virtual DbSet<ClientType> ClientType { get; set; }
        public virtual DbSet<ProjectInfo> ProjectInfo { get; set; }
        public virtual DbSet<AssetValuer> AssetValuer { get; set; }
        public virtual DbSet<AssetTransfer> AssetTransfer { get; set; }
        public virtual DbSet<AssetRevaluation> AssetRevaluation { get; set; }
        public virtual DbSet<AssetOut> AssetOut { get; set; }
        public virtual DbSet<AssetPartialOut> AssetPartialOut { get; set; }
        public virtual DbSet<AssetOverhauling> AssetOverhauling { get; set; }
        public virtual DbSet<DepriciationRateChange> DepriciationRateChange { get; set; }
        public virtual DbSet<AssetProcessInfo> AssetProcessInfo { get; set; }
        public virtual DbSet<AssetUserDepartment> AssetUserDepartment { get; set; }
        public virtual DbSet<AssetUserDesignation> AssetUserDesignation { get; set; }
        public virtual DbSet<AssetUser> AssetUser { get; set; }
        public virtual DbSet<WriteOffHistory> WriteOffHistory { get; set; }

        public virtual DbSet<StatisticsReport> StatisticsReport { get; set; }
        public virtual DbSet<StatisticsDescription> StatisticsDescription { get; set; }
        public virtual DbSet<StatisticsReportDetails> StatisticsReportDetails { get; set; }
        public virtual DbSet<MemberNominee> MemberNominee { get; set; }
        public virtual DbSet<MemberOtherInfo> MemberOtherInfo { get; set; }
        public virtual DbSet<SMS_SentLog> SMS_SentLog { get; set; }
        public virtual DbSet<PO_INFO> PO_INFOs { get; set; }


        public virtual DbSet<PO_INFO_MAPPING> PO_INFO_MAPPINGs { get; set; }
        public virtual DbSet<POLoanCode> POLoanCodes { get; set; }
        public virtual DbSet<LOAN_PRODUCT> LOAN_PRODUCTs { get; set; }
        public virtual DbSet<POProductMapping> POProductMappings { get; set; }
        public virtual DbSet<Union> Unions { get; set; }
        public virtual DbSet<District> Districts { get; set; }
     
        
        public virtual DbSet<Upozilla> Upozillas { get; set; }
        public virtual DbSet<Division> Divisions { get; set; }
        public virtual DbSet<ReportType> ReportType { get; set; }
        public virtual DbSet<ReportTypeMapping> ReportTypeMapping { get; set; }
        public virtual DbSet<ConsentForm> ConsentForm { get; set; }
        public virtual DbSet<ArchiveDbMapping> ArchiveDbMapping { get; set; }
        public virtual DbSet<StopInterest> StopInterest { get; set; }
        public virtual DbSet<LegalInfo> LegalInfo { get; set; }
        public virtual DbSet<MISConsolidationProcess> MISConsolidationProcess { get; set; }
        public virtual DbSet<FamilyMemberSameHousehold> FamilyMemberSameHousehold { get; set; }
        public virtual DbSet<PKSFFundLoan> PKSFFundLoan { get; set; }
        public virtual DbSet<ProductXEmploymentProductMapping> ProductXEmploymentProductMapping { get; set; }
        public virtual DbSet<MemberWisePassbookBalanceEntry> MemberWisePassbookBalanceEntry { get; set; }
        public virtual DbSet<FundTransferMapping> FundTransferMapping { get; set; }
        public virtual DbSet<ProMiniBalanSet> ProMiniBalanSet { get; set; }
        public virtual DbSet<InterbranchTransactionTracking> InterbranchTransactionTracking { get; set; }
        public virtual DbSet<MemberBlackList> MemberBlackList{ get; set; }

        // mahfuz
        public virtual DbSet<AccMappingForFundTransfer> AccMappingForFundTransfers { get; set; }

        #region Inventory
        public virtual DbSet<Inv_CategoryOrSubCategory> InvCategoryOrSubCategory { get; set; }
        public virtual DbSet<Inv_ItemPriceDetails> InvItemPriceDetails { get; set; }
        public virtual DbSet<Inv_Items> InvItems { get; set; }
        public virtual DbSet<InvWarehouse> InvWarehouses { get; set; }
        public virtual DbSet<InvStoreItem> InvStoreItems { get; set; }
        public virtual DbSet<Inv_Store> Inv_Stores { get; set; }
        public virtual DbSet<Inv_Vendor> Inv_Vendors { get; set; }
        public virtual DbSet<Inv_RequsitionMaster> Inv_RequsitionMasters { get; set; }
        public virtual DbSet<Inv_RequsitionDetails> Inv_RequsitionDetail { get; set; }
        public virtual DbSet<Inv_RequisitionConsulateMaster> Inv_RequisitionConsulateMasters { get; set; }
        public virtual DbSet<Inv_RequisitionConsulateDetails> Inv_RequisitionConsulateDetail { get; set; }
        public virtual DbSet<Inv_TempStore> Inv_TempStores { get; set; }
        public virtual DbSet<Inv_TrxDetail> InvTrxDetails { get; set; }
        public virtual DbSet<Inv_TrxMaster> InvTrxMasters { get; set; }
        public virtual DbSet<InventoryDailyVoucher> InventoryDailyVouchers { get; set; }
        public virtual DbSet<InventoryDailyVoucherHistory> InventoryDailyVoucherHistorys { get; set; }
        public virtual DbSet<Inv_Settings> inv_Settings { get; set; }
        public virtual DbSet<Inv_RequsitionDispose> inv_RequsitionDisposes { get; set; }
        public virtual DbSet<Inv_ConsolidateDisposeRequest> Inv_ConsolidateDisposeRequests { get; set; }
        #endregion Inventory

        #region OLRS

        public virtual DbSet<OLRSAccChartMapping> OLRSAccChartMappings { get; set; }       
        public virtual DbSet<Indicator> Indicators { get; set; }
        public virtual DbSet<DistrictThanaMapping> DistrictThanaMappings { get; set; }        

        #endregion

        public virtual ObjectResult<getSavingStopInterestAccountInfo_Result> getSavingStopInterestAccountInfo(Nullable<int> orgID, Nullable<int> officeID, Nullable<System.DateTime> tranDate)
        {
            var orgIDParameter = orgID.HasValue ?
                new ObjectParameter("OrgID", orgID) :
                new ObjectParameter("OrgID", typeof(int));

            var officeIDParameter = officeID.HasValue ?
                new ObjectParameter("OfficeID", officeID) :
                new ObjectParameter("OfficeID", typeof(int));

            var tranDateParameter = tranDate.HasValue ?
                new ObjectParameter("TranDate", tranDate) :
                new ObjectParameter("TranDate", typeof(System.DateTime));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getSavingStopInterestAccountInfo_Result>("getSavingStopInterestAccountInfo", orgIDParameter, officeIDParameter, tranDateParameter);
        }
        public virtual int delWriteOffList(Nullable<long> office, Nullable<long> memberID, Nullable<int> centerID, Nullable<int> productID, Nullable<int> loanTerm, Nullable<System.DateTime> trandate, Nullable<decimal> writeOffLOan, Nullable<decimal> writeOffInterest)
        {
            var officeParameter = office.HasValue ?
                new ObjectParameter("Office", office) :
                new ObjectParameter("Office", typeof(long));

            var memberIDParameter = memberID.HasValue ?
                new ObjectParameter("MemberID", memberID) :
                new ObjectParameter("MemberID", typeof(long));

            var centerIDParameter = centerID.HasValue ?
                new ObjectParameter("centerID", centerID) :
                new ObjectParameter("centerID", typeof(int));

            var productIDParameter = productID.HasValue ?
                new ObjectParameter("productID", productID) :
                new ObjectParameter("productID", typeof(int));

            var loanTermParameter = loanTerm.HasValue ?
                new ObjectParameter("LoanTerm", loanTerm) :
                new ObjectParameter("LoanTerm", typeof(int));

            var trandateParameter = trandate.HasValue ?
                new ObjectParameter("Trandate", trandate) :
                new ObjectParameter("Trandate", typeof(System.DateTime));

            var writeOffLOanParameter = writeOffLOan.HasValue ?
                new ObjectParameter("writeOffLOan", writeOffLOan) :
                new ObjectParameter("writeOffLOan", typeof(decimal));

            var writeOffInterestParameter = writeOffInterest.HasValue ?
                new ObjectParameter("writeOffInterest", writeOffInterest) :
                new ObjectParameter("writeOffInterest", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("delWriteOffList", officeParameter, memberIDParameter, centerIDParameter, productIDParameter, loanTermParameter, trandateParameter, writeOffLOanParameter, writeOffInterestParameter);
        }
        public virtual int setWriteOffList(Nullable<long> office, Nullable<long> memberID, Nullable<int> centerID, Nullable<int> productID, Nullable<int> loanTerm, Nullable<System.DateTime> trandate, Nullable<decimal> writeOffLOan, Nullable<decimal> writeOffInterest)
        {
            var officeParameter = office.HasValue ?
                new ObjectParameter("Office", office) :
                new ObjectParameter("Office", typeof(long));

            var memberIDParameter = memberID.HasValue ?
                new ObjectParameter("MemberID", memberID) :
                new ObjectParameter("MemberID", typeof(long));

            var centerIDParameter = centerID.HasValue ?
                new ObjectParameter("centerID", centerID) :
                new ObjectParameter("centerID", typeof(int));

            var productIDParameter = productID.HasValue ?
                new ObjectParameter("productID", productID) :
                new ObjectParameter("productID", typeof(int));

            var loanTermParameter = loanTerm.HasValue ?
                new ObjectParameter("LoanTerm", loanTerm) :
                new ObjectParameter("LoanTerm", typeof(int));

            var trandateParameter = trandate.HasValue ?
                new ObjectParameter("Trandate", trandate) :
                new ObjectParameter("Trandate", typeof(System.DateTime));

            var writeOffLOanParameter = writeOffLOan.HasValue ?
                new ObjectParameter("writeOffLOan", writeOffLOan) :
                new ObjectParameter("writeOffLOan", typeof(decimal));

            var writeOffInterestParameter = writeOffInterest.HasValue ?
                new ObjectParameter("writeOffInterest", writeOffInterest) :
                new ObjectParameter("writeOffInterest", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("setWriteOffList", officeParameter, memberIDParameter, centerIDParameter, productIDParameter, loanTermParameter, trandateParameter, writeOffLOanParameter, writeOffInterestParameter);
        }
        public virtual ObjectResult<getGetLoanLedgerMemberWise_Result> getGetLoanLedgerMemberWise(Nullable<int> officeId, Nullable<int> memberId)
        {
            var officeIdParameter = officeId.HasValue ?
                new ObjectParameter("officeId", officeId) :
                new ObjectParameter("officeId", typeof(int));

            var memberIdParameter = memberId.HasValue ?
                new ObjectParameter("memberId", memberId) :
                new ObjectParameter("memberId", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getGetLoanLedgerMemberWise_Result>("getGetLoanLedgerMemberWise", officeIdParameter, memberIdParameter);
        }
        //public virtual ObjectResult<Proc_get_LoanDisburse_Result> Proc_get_LoanDisburse(Nullable<int> officeID, Nullable<System.DateTime> date)
        //{
        //    var officeIDParameter = officeID.HasValue ?
        //        new ObjectParameter("OfficeID", officeID) :
        //        new ObjectParameter("OfficeID", typeof(int));

        //    var dateParameter = date.HasValue ?
        //        new ObjectParameter("Date", date) :
        //        new ObjectParameter("Date", typeof(System.DateTime));

        //    return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Proc_get_LoanDisburse_Result>("Proc_get_LoanDisburse", officeIDParameter, dateParameter);
        //}
        public virtual ObjectResult<Proc_get_LoanDisburse_Result> Proc_get_LoanDisburse(Nullable<int> officeID, Nullable<System.DateTime> date, string filterColumnName, string filterValue)
        {
            var officeIDParameter = officeID.HasValue ?
                new ObjectParameter("OfficeID", officeID) :
                new ObjectParameter("OfficeID", typeof(int));

            var dateParameter = date.HasValue ?
                new ObjectParameter("Date", date) :
                new ObjectParameter("Date", typeof(System.DateTime));

            var filterColumnNameParameter = filterColumnName != null ?
                new ObjectParameter("filterColumnName", filterColumnName) :
                new ObjectParameter("filterColumnName", typeof(string));

            var filterValueParameter = filterValue != null ?
                new ObjectParameter("filterValue", filterValue) :
                new ObjectParameter("filterValue", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Proc_get_LoanDisburse_Result>("Proc_get_LoanDisburse", officeIDParameter, dateParameter, filterColumnNameParameter, filterValueParameter);
        }

        //public virtual ObjectResult<proc_get_SpecialLoanCollection_Result> proc_get_SpecialLoanCollection(Nullable<int> officeId, string collectionDay)
        //{
        //    var officeIdParameter = officeId.HasValue ?
        //        new ObjectParameter("officeId", officeId) :
        //        new ObjectParameter("officeId", typeof(int));

        //    var collectionDayParameter = collectionDay != null ?
        //        new ObjectParameter("CollectionDay", collectionDay) :
        //        new ObjectParameter("CollectionDay", typeof(string));

        //    return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_get_SpecialLoanCollection_Result>("proc_get_SpecialLoanCollection", officeIdParameter, collectionDayParameter);
        //}
        public virtual ObjectResult<proc_get_SpecialLoanCollection_Result> proc_get_SpecialLoanCollection(Nullable<int> officeId, string collectionDay, string filterColumnName, string filterValue)
        {
            var officeIdParameter = officeId.HasValue ?
                new ObjectParameter("officeId", officeId) :
                new ObjectParameter("officeId", typeof(int));

            var collectionDayParameter = collectionDay != null ?
                new ObjectParameter("CollectionDay", collectionDay) :
                new ObjectParameter("CollectionDay", typeof(string));

            var filterColumnNameParameter = filterColumnName != null ?
                new ObjectParameter("filterColumnName", filterColumnName) :
                new ObjectParameter("filterColumnName", typeof(string));

            var filterValueParameter = filterValue != null ?
                new ObjectParameter("filterValue", filterValue) :
                new ObjectParameter("filterValue", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_get_SpecialLoanCollection_Result>("proc_get_SpecialLoanCollection", officeIdParameter, collectionDayParameter, filterColumnNameParameter, filterValueParameter);
        }
        public virtual int Proc_Set_RepaymentSchedule(Nullable<long> loanSummaryID, Nullable<int> officeID, Nullable<long> memberID, Nullable<short> productID, Nullable<int> centerID, Nullable<byte> memberCategoryID, Nullable<int> loanTerm, Nullable<int> duration, Nullable<System.DateTime> installmentStartDate, string createUser, Nullable<System.DateTime> createDate)
        {
            var loanSummaryIDParameter = loanSummaryID.HasValue ?
                new ObjectParameter("LoanSummaryID", loanSummaryID) :
                new ObjectParameter("LoanSummaryID", typeof(long));

            var officeIDParameter = officeID.HasValue ?
                new ObjectParameter("OfficeID", officeID) :
                new ObjectParameter("OfficeID", typeof(int));

            var memberIDParameter = memberID.HasValue ?
                new ObjectParameter("MemberID", memberID) :
                new ObjectParameter("MemberID", typeof(long));

            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(short));

            var centerIDParameter = centerID.HasValue ?
                new ObjectParameter("CenterID", centerID) :
                new ObjectParameter("CenterID", typeof(int));

            var memberCategoryIDParameter = memberCategoryID.HasValue ?
                new ObjectParameter("MemberCategoryID", memberCategoryID) :
                new ObjectParameter("MemberCategoryID", typeof(byte));

            var loanTermParameter = loanTerm.HasValue ?
                new ObjectParameter("LoanTerm", loanTerm) :
                new ObjectParameter("LoanTerm", typeof(int));

            var durationParameter = duration.HasValue ?
                new ObjectParameter("Duration", duration) :
                new ObjectParameter("Duration", typeof(int));

            var installmentStartDateParameter = installmentStartDate.HasValue ?
                new ObjectParameter("InstallmentStartDate", installmentStartDate) :
                new ObjectParameter("InstallmentStartDate", typeof(System.DateTime));

            var createUserParameter = createUser != null ?
                new ObjectParameter("CreateUser", createUser) :
                new ObjectParameter("CreateUser", typeof(string));

            var createDateParameter = createDate.HasValue ?
                new ObjectParameter("CreateDate", createDate) :
                new ObjectParameter("CreateDate", typeof(System.DateTime));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Proc_Set_RepaymentSchedule", loanSummaryIDParameter, officeIDParameter, memberIDParameter, productIDParameter, centerIDParameter, memberCategoryIDParameter, loanTermParameter, durationParameter, installmentStartDateParameter, createUserParameter, createDateParameter);
        }
        //public virtual ObjectResult<proc_get_SpecialSavingCollection_Result> proc_get_SpecialSavingCollection(Nullable<int> officeId, string collectionDay)
        //{
        //    var officeIdParameter = officeId.HasValue ?
        //        new ObjectParameter("officeId", officeId) :
        //        new ObjectParameter("officeId", typeof(int));

        //    var collectionDayParameter = collectionDay != null ?
        //        new ObjectParameter("CollectionDay", collectionDay) :
        //        new ObjectParameter("CollectionDay", typeof(string));

        //    return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_get_SpecialSavingCollection_Result>("proc_get_SpecialSavingCollection", officeIdParameter, collectionDayParameter);
        //}
        public virtual ObjectResult<proc_get_SpecialSavingCollection_Result> proc_get_SpecialSavingCollection(Nullable<int> officeId, string collectionDay, string filterColumnName, string filterValue)
        {
            var officeIdParameter = officeId.HasValue ?
                new ObjectParameter("officeId", officeId) :
                new ObjectParameter("officeId", typeof(int));

            var collectionDayParameter = collectionDay != null ?
                new ObjectParameter("CollectionDay", collectionDay) :
                new ObjectParameter("CollectionDay", typeof(string));

            var filterColumnNameParameter = filterColumnName != null ?
                new ObjectParameter("filterColumnName", filterColumnName) :
                new ObjectParameter("filterColumnName", typeof(string));

            var filterValueParameter = filterValue != null ?
                new ObjectParameter("filterValue", filterValue) :
                new ObjectParameter("filterValue", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_get_SpecialSavingCollection_Result>("proc_get_SpecialSavingCollection", officeIdParameter, collectionDayParameter, filterColumnNameParameter, filterValueParameter);
        }
        public virtual int Prcs_DayInitial(Nullable<int> officeId, Nullable<System.DateTime> businessDate)
        {
            var officeIdParameter = officeId.HasValue ?
                new ObjectParameter("OfficeId", officeId) :
                new ObjectParameter("OfficeId", typeof(int));

            var businessDateParameter = businessDate.HasValue ?
                new ObjectParameter("BusinessDate", businessDate) :
                new ObjectParameter("BusinessDate", typeof(System.DateTime));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Prcs_DayInitial", officeIdParameter, businessDateParameter);
        }
        public virtual int Prcs_DayEnd(Nullable<int> officeId, Nullable<System.DateTime> businessDate)
        {
            var officeIdParameter = officeId.HasValue ?
                new ObjectParameter("OfficeId", officeId) :
                new ObjectParameter("OfficeId", typeof(int));

            var businessDateParameter = businessDate.HasValue ?
                new ObjectParameter("BusinessDate", businessDate) :
                new ObjectParameter("BusinessDate", typeof(System.DateTime));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Prcs_DayEnd", officeIdParameter, businessDateParameter);
        }
        public virtual int AccountClose(Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID, Nullable<int> noAccount, Nullable<System.DateTime> tranDate)
        {
            var officeIDParameter = officeID.HasValue ?
                new ObjectParameter("OfficeID", officeID) :
                new ObjectParameter("OfficeID", typeof(int));

            var centerIDParameter = centerID.HasValue ?
                new ObjectParameter("CenterID", centerID) :
                new ObjectParameter("CenterID", typeof(int));

            var memberIDParameter = memberID.HasValue ?
                new ObjectParameter("MemberID", memberID) :
                new ObjectParameter("MemberID", typeof(long));

            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));

            var noAccountParameter = noAccount.HasValue ?
                new ObjectParameter("NoAccount", noAccount) :
                new ObjectParameter("NoAccount", typeof(int));

            var tranDateParameter = tranDate.HasValue ?
                new ObjectParameter("TranDate", tranDate) :
                new ObjectParameter("TranDate", typeof(System.DateTime));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AccountClose", officeIDParameter, centerIDParameter, memberIDParameter, productIDParameter, noAccountParameter, tranDateParameter);
        }
        public virtual ObjectResult<Proc_Rpt_LoanLedger_Result> Proc_Rpt_LoanLedger(string qtype, string branchCode, string centerCode)
        {
            var qtypeParameter = qtype != null ?
                new ObjectParameter("Qtype", qtype) :
                new ObjectParameter("Qtype", typeof(string));

            var branchCodeParameter = branchCode != null ?
                new ObjectParameter("BranchCode", branchCode) :
                new ObjectParameter("BranchCode", typeof(string));

            var centerCodeParameter = centerCode != null ?
                new ObjectParameter("CenterCode", centerCode) :
                new ObjectParameter("CenterCode", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Proc_Rpt_LoanLedger_Result>("Proc_Rpt_LoanLedger", qtypeParameter, branchCodeParameter, centerCodeParameter);
        }
        public virtual ObjectResult<GetSetSLCTxtKeyPress_Result> GetSetSLCTxtKeyPress(Nullable<int> orgID, Nullable<int> officeID, Nullable<int> centerID, Nullable<int> productID, Nullable<long> memberID, Nullable<int> loanTerm, Nullable<System.DateTime> collectionDate, Nullable<int> transType)
        {
            var orgIDParameter = orgID.HasValue ?
                new ObjectParameter("orgID", orgID) :
                new ObjectParameter("orgID", typeof(int));

            var officeIDParameter = officeID.HasValue ?
                new ObjectParameter("officeID", officeID) :
                new ObjectParameter("officeID", typeof(int));

            var centerIDParameter = centerID.HasValue ?
                new ObjectParameter("CenterID", centerID) :
                new ObjectParameter("CenterID", typeof(int));

            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));

            var memberIDParameter = memberID.HasValue ?
                new ObjectParameter("MemberID", memberID) :
                new ObjectParameter("MemberID", typeof(long));

            var loanTermParameter = loanTerm.HasValue ?
                new ObjectParameter("LoanTerm", loanTerm) :
                new ObjectParameter("LoanTerm", typeof(int));

            var collectionDateParameter = collectionDate.HasValue ?
                new ObjectParameter("CollectionDate", collectionDate) :
                new ObjectParameter("CollectionDate", typeof(System.DateTime));

            var transTypeParameter = transType.HasValue ?
                new ObjectParameter("TransType", transType) :
                new ObjectParameter("TransType", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetSetSLCTxtKeyPress_Result>("GetSetSLCTxtKeyPress", orgIDParameter, officeIDParameter, centerIDParameter, productIDParameter, memberIDParameter, loanTermParameter, collectionDateParameter, transTypeParameter);
        }

        public virtual int monthlyProcess(Nullable<int> officeID, Nullable<System.DateTime> monthEndDate)
        {
            var officeIDParameter = officeID.HasValue ?
                new ObjectParameter("OfficeID", officeID) :
                new ObjectParameter("OfficeID", typeof(int));

            var monthEndDateParameter = monthEndDate.HasValue ?
                new ObjectParameter("MonthEndDate", monthEndDate) :
                new ObjectParameter("MonthEndDate", typeof(System.DateTime));



            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("monthlyProcess", officeIDParameter, monthEndDateParameter);
        }

        public virtual int monthlyProcessAverageMethod(Nullable<int> officeID, Nullable<System.DateTime> processDate)
        {
            var officeIDParameter = officeID.HasValue ?
                new ObjectParameter("officeID", officeID) :
                new ObjectParameter("officeID", typeof(int));

            var processDateParameter = processDate.HasValue ?
                new ObjectParameter("ProcessDate", processDate) :
                new ObjectParameter("ProcessDate", typeof(System.DateTime));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("monthlyProcessAverageMethod", officeIDParameter, processDateParameter);
        }
        public virtual ObjectResult<string> validateDayIntial(Nullable<int> officeId)
        {
            var officeIdParameter = officeId.HasValue ?
                new ObjectParameter("OfficeId", officeId) :
                new ObjectParameter("OfficeId", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("validateDayIntial", officeIdParameter);
        }
        public virtual int AccAutoVoucherCollection(Nullable<int> officeID, Nullable<System.DateTime> businessDate)
        {
            var officeIDParameter = officeID.HasValue ?
                new ObjectParameter("OfficeID", officeID) :
                new ObjectParameter("OfficeID", typeof(int));

            var businessDateParameter = businessDate.HasValue ?
                new ObjectParameter("BusinessDate", businessDate) :
                new ObjectParameter("BusinessDate", typeof(System.DateTime));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AccAutoVoucherCollection", officeIDParameter, businessDateParameter);
        }
        public virtual int Proc_Set_OpeningLoanEntry(Nullable<int> officeID, Nullable<System.DateTime> businessDate)
        {
            var officeIDParameter = officeID.HasValue ?
                new ObjectParameter("OfficeID", officeID) :
                new ObjectParameter("OfficeID", typeof(int));

            var businessDateParameter = businessDate.HasValue ?
                new ObjectParameter("BusinessDate", businessDate) :
                new ObjectParameter("BusinessDate", typeof(System.DateTime));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Proc_Set_OpeningLoanEntry", officeIDParameter, businessDateParameter);
        }

        public virtual int Proc_set_OpeningSaving(Nullable<int> officeID, Nullable<System.DateTime> businessDate)
        {
            var officeIDParameter = officeID.HasValue ?
                new ObjectParameter("OfficeID", officeID) :
                new ObjectParameter("OfficeID", typeof(int));

            var businessDateParameter = businessDate.HasValue ?
                new ObjectParameter("BusinessDate", businessDate) :
                new ObjectParameter("BusinessDate", typeof(System.DateTime));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Proc_set_OpeningSaving", officeIDParameter, businessDateParameter);
        }
        public virtual int SpecialCollection(Nullable<int> officeID, Nullable<int> centerID, Nullable<int> productID, Nullable<int> memberID, Nullable<int> loanTerm, string collectionDay, Nullable<System.DateTime> collectionDate, Nullable<int> qType, Nullable<int> transType, Nullable<decimal> loanPaid, Nullable<decimal> intPaid)
        {
            var officeIDParameter = officeID.HasValue ?
                new ObjectParameter("OfficeID", officeID) :
                new ObjectParameter("OfficeID", typeof(int));

            var centerIDParameter = centerID.HasValue ?
                new ObjectParameter("CenterID", centerID) :
                new ObjectParameter("CenterID", typeof(int));

            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));

            var memberIDParameter = memberID.HasValue ?
                new ObjectParameter("MemberID", memberID) :
                new ObjectParameter("MemberID", typeof(int));

            var loanTermParameter = loanTerm.HasValue ?
                new ObjectParameter("LoanTerm", loanTerm) :
                new ObjectParameter("LoanTerm", typeof(int));

            var collectionDayParameter = collectionDay != null ?
                new ObjectParameter("CollectionDay", collectionDay) :
                new ObjectParameter("CollectionDay", typeof(string));

            var collectionDateParameter = collectionDate.HasValue ?
                new ObjectParameter("CollectionDate", collectionDate) :
                new ObjectParameter("CollectionDate", typeof(System.DateTime));

            var qTypeParameter = qType.HasValue ?
                new ObjectParameter("qType", qType) :
                new ObjectParameter("qType", typeof(int));

            var transTypeParameter = transType.HasValue ?
                new ObjectParameter("TransType", transType) :
                new ObjectParameter("TransType", typeof(int));

            var loanPaidParameter = loanPaid.HasValue ?
                new ObjectParameter("LoanPaid", loanPaid) :
                new ObjectParameter("LoanPaid", typeof(decimal));

            var intPaidParameter = intPaid.HasValue ?
                new ObjectParameter("intPaid", intPaid) :
                new ObjectParameter("intPaid", typeof(decimal));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SpecialCollection", officeIDParameter, centerIDParameter, productIDParameter, memberIDParameter, loanTermParameter, collectionDayParameter, collectionDateParameter, qTypeParameter, transTypeParameter, loanPaidParameter, intPaidParameter);
        }
        public virtual int updateDisburseCharge(Nullable<long> loanSummaryID, Nullable<int> officeID, Nullable<int> centerID, Nullable<int> memberId, Nullable<int> productID, Nullable<int> loanterm, Nullable<decimal> principal, Nullable<System.DateTime> installmentStartDate, Nullable<System.DateTime> disburseDate)
        {
            var loanSummaryIDParameter = loanSummaryID.HasValue ?
                new ObjectParameter("LoanSummaryID", loanSummaryID) :
                new ObjectParameter("LoanSummaryID", typeof(long));

            var officeIDParameter = officeID.HasValue ?
                new ObjectParameter("OfficeID", officeID) :
                new ObjectParameter("OfficeID", typeof(int));

            var centerIDParameter = centerID.HasValue ?
                new ObjectParameter("CenterID", centerID) :
                new ObjectParameter("CenterID", typeof(int));

            var memberIdParameter = memberId.HasValue ?
                new ObjectParameter("MemberId", memberId) :
                new ObjectParameter("MemberId", typeof(int));

            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));

            var loantermParameter = loanterm.HasValue ?
                new ObjectParameter("Loanterm", loanterm) :
                new ObjectParameter("Loanterm", typeof(int));

            var principalParameter = principal.HasValue ?
                new ObjectParameter("Principal", principal) :
                new ObjectParameter("Principal", typeof(decimal));

            var installmentStartDateParameter = installmentStartDate.HasValue ?
                new ObjectParameter("InstallmentStartDate", installmentStartDate) :
                new ObjectParameter("InstallmentStartDate", typeof(System.DateTime));

            var disburseDateParameter = disburseDate.HasValue ?
                new ObjectParameter("DisburseDate", disburseDate) :
                new ObjectParameter("DisburseDate", typeof(System.DateTime));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("updateDisburseCharge", loanSummaryIDParameter, officeIDParameter, centerIDParameter, memberIdParameter, productIDParameter, loantermParameter, principalParameter, installmentStartDateParameter, disburseDateParameter);
        }
        public virtual int delVoucher(Nullable<int> officeID, Nullable<System.DateTime> businessDate)
        {
            var officeIDParameter = officeID.HasValue ?
                new ObjectParameter("OfficeID", officeID) :
                new ObjectParameter("OfficeID", typeof(int));

            var businessDateParameter = businessDate.HasValue ?
                new ObjectParameter("BusinessDate", businessDate) :
                new ObjectParameter("BusinessDate", typeof(System.DateTime));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("delVoucher", officeIDParameter, businessDateParameter);
        }
        public virtual ObjectResult<getTransferHistory_Result> getTransferHistory(Nullable<int> officeId)
        {
            var officeIdParameter = officeId.HasValue ?
                new ObjectParameter("officeId", officeId) :
                new ObjectParameter("officeId", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getTransferHistory_Result>("getTransferHistory", officeIdParameter);
        }
        public virtual int CateGoryTransfer(Nullable<int> officeID, Nullable<int> centerID, Nullable<int> memberID, Nullable<int> memberCategoryID, Nullable<int> productID, Nullable<decimal> savBalance, Nullable<int> newMemberCategoryID, Nullable<System.DateTime> transdate, Nullable<int> newProductID)
        {
            var officeIDParameter = officeID.HasValue ?
                new ObjectParameter("OfficeID", officeID) :
                new ObjectParameter("OfficeID", typeof(int));

            var centerIDParameter = centerID.HasValue ?
                new ObjectParameter("CenterID", centerID) :
                new ObjectParameter("CenterID", typeof(int));

            var memberIDParameter = memberID.HasValue ?
                new ObjectParameter("MemberID", memberID) :
                new ObjectParameter("MemberID", typeof(int));

            var memberCategoryIDParameter = memberCategoryID.HasValue ?
                new ObjectParameter("MemberCategoryID", memberCategoryID) :
                new ObjectParameter("MemberCategoryID", typeof(int));

            var productIDParameter = productID.HasValue ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(int));

            var savBalanceParameter = savBalance.HasValue ?
                new ObjectParameter("SavBalance", savBalance) :
                new ObjectParameter("SavBalance", typeof(decimal));

            var newMemberCategoryIDParameter = newMemberCategoryID.HasValue ?
                new ObjectParameter("NewMemberCategoryID", newMemberCategoryID) :
                new ObjectParameter("NewMemberCategoryID", typeof(int));

            var transdateParameter = transdate.HasValue ?
                new ObjectParameter("Transdate", transdate) :
                new ObjectParameter("Transdate", typeof(System.DateTime));

            var newProductIDParameter = newProductID.HasValue ?
                new ObjectParameter("NewProductID", newProductID) :
                new ObjectParameter("NewProductID", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CateGoryTransfer", officeIDParameter, centerIDParameter, memberIDParameter, memberCategoryIDParameter, productIDParameter, savBalanceParameter, newMemberCategoryIDParameter, transdateParameter, newProductIDParameter);
        }
        public virtual DbSet<LoanCorrectionTrx> LoanCorrectionTrxes { get; set; }
        public virtual DbSet<SavingCorrectionTrx> SavingCorrectionTrxes { get; set; }
        public virtual ObjectResult<proc_get_LoanCorrection_Result> proc_get_LoanCorrection(Nullable<int> officeId, Nullable<System.DateTime> trxDate)
        {
            var officeIdParameter = officeId.HasValue ?
                new ObjectParameter("officeId", officeId) :
                new ObjectParameter("officeId", typeof(int));

            var trxDateParameter = trxDate.HasValue ?
                new ObjectParameter("TrxDate", trxDate) :
                new ObjectParameter("TrxDate", typeof(System.DateTime));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_get_LoanCorrection_Result>("proc_get_LoanCorrection", officeIdParameter, trxDateParameter);
        }
        public virtual ObjectResult<getSavingsCorrection_Result> getSavingsCorrection(Nullable<int> officeId, Nullable<System.DateTime> transDate)
        {
            var officeIdParameter = officeId.HasValue ?
                new ObjectParameter("officeId", officeId) :
                new ObjectParameter("officeId", typeof(int));

            var transDateParameter = transDate.HasValue ?
                new ObjectParameter("TransDate", transDate) :
                new ObjectParameter("TransDate", typeof(System.DateTime));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getSavingsCorrection_Result>("getSavingsCorrection", officeIdParameter, transDateParameter);
        }
        public virtual ObjectResult<GetRepaymentSchedule_Result> GetRepaymentSchedule(Nullable<int> officeId, Nullable<int> memberID, Nullable<int> productId)
        {
            var officeIdParameter = officeId.HasValue ?
                new ObjectParameter("officeId", officeId) :
                new ObjectParameter("officeId", typeof(int));

            var memberIDParameter = memberID.HasValue ?
                new ObjectParameter("MemberID", memberID) :
                new ObjectParameter("MemberID", typeof(int));

            var productIdParameter = productId.HasValue ?
                new ObjectParameter("ProductId", productId) :
                new ObjectParameter("ProductId", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetRepaymentSchedule_Result>("GetRepaymentSchedule", officeIdParameter, memberIDParameter, productIdParameter);
        }
        public virtual int usp_rpt_credit_score(Nullable<long> officeID, Nullable<System.DateTime> start_date, Nullable<System.DateTime> end_date)
        {
            var officeIDParameter = officeID.HasValue ?
                new ObjectParameter("officeID", officeID) :
                new ObjectParameter("officeID", typeof(long));

            var start_dateParameter = start_date.HasValue ?
                new ObjectParameter("start_date", start_date) :
                new ObjectParameter("start_date", typeof(System.DateTime));

            var end_dateParameter = end_date.HasValue ?
                new ObjectParameter("end_date", end_date) :
                new ObjectParameter("end_date", typeof(System.DateTime));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("usp_rpt_credit_score", officeIDParameter, start_dateParameter, end_dateParameter);
        }
        public virtual ObjectResult<GetCreditScore_Result> GetCreditScore(Nullable<int> officeId)
        {
            var officeIdParameter = officeId.HasValue ?
                new ObjectParameter("officeId", officeId) :
                new ObjectParameter("officeId", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetCreditScore_Result>("GetCreditScore", officeIdParameter);
        }
        public virtual ObjectResult<getFamilyGrace_Result> getFamilyGrace(Nullable<long> officeId)
        {
            var officeIdParameter = officeId.HasValue ?
                new ObjectParameter("officeId", officeId) :
                new ObjectParameter("officeId", typeof(long));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getFamilyGrace_Result>("getFamilyGrace", officeIdParameter);
        }
        public virtual ObjectResult<getTodaysTransaction_Result> getTodaysTransaction(Nullable<long> office, Nullable<System.DateTime> tranDateFrom, Nullable<System.DateTime> tranDateTo)
        {
            var officeParameter = office.HasValue ?
                new ObjectParameter("Office", office) :
                new ObjectParameter("Office", typeof(long));

            var tranDateFromParameter = tranDateFrom.HasValue ?
                new ObjectParameter("TranDateFrom", tranDateFrom) :
                new ObjectParameter("TranDateFrom", typeof(System.DateTime));

            var tranDateToParameter = tranDateTo.HasValue ?
                new ObjectParameter("TranDateTo", tranDateTo) :
                new ObjectParameter("TranDateTo", typeof(System.DateTime));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getTodaysTransaction_Result>("getTodaysTransaction", officeParameter, tranDateFromParameter, tranDateToParameter);
        }
        public virtual ObjectResult<getWriteOffList_Result> getWriteOffList(Nullable<long> office, Nullable<long> memberID, Nullable<System.DateTime> trandate)
        {
            var officeParameter = office.HasValue ?
                new ObjectParameter("Office", office) :
                new ObjectParameter("Office", typeof(long));

            var memberIDParameter = memberID.HasValue ?
                new ObjectParameter("MemberID", memberID) :
                new ObjectParameter("MemberID", typeof(long));

            var trandateParameter = trandate.HasValue ?
                new ObjectParameter("Trandate", trandate) :
                new ObjectParameter("Trandate", typeof(System.DateTime));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getWriteOffList_Result>("getWriteOffList", officeParameter, memberIDParameter, trandateParameter);
        }
        public virtual ObjectResult<getCenterMeetingSchedule_Result> getCenterMeetingSchedule(Nullable<int> officeID, string collectionDay)
        {
            var officeIDParameter = officeID.HasValue ?
                new ObjectParameter("OfficeID", officeID) :
                new ObjectParameter("OfficeID", typeof(int));

            var collectionDayParameter = collectionDay != null ?
                new ObjectParameter("CollectionDay", collectionDay) :
                new ObjectParameter("CollectionDay", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getCenterMeetingSchedule_Result>("getCenterMeetingSchedule", officeIDParameter, collectionDayParameter);
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApproveCelling>()
                .Property(e => e.MinRange)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ApproveCelling>()
                .Property(e => e.MaxRange)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ApproveCelling>()
                .Property(e => e.RoleName)
                .IsUnicode(false);

            modelBuilder.Entity<MobileErrorLog>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<MobileErrorLog>()
                .Property(e => e.ActionName)
                .IsUnicode(false);

            modelBuilder.Entity<MobileErrorLog>()
                .Property(e => e.InputParams)
                .IsUnicode(false);

            modelBuilder.Entity<MobileErrorLog>()
                .Property(e => e.ErrorDetail)
                .IsUnicode(false);

            modelBuilder.Entity<MobileErrorLog>()
                .Property(e => e.ErrorType)
                .IsUnicode(false);

            modelBuilder.Entity<MobileErrorLog>()
                .Property(e => e.PhoneModel)
                .IsUnicode(false);

            modelBuilder.Entity<MobileErrorLog>()
                .Property(e => e.OSVersion)
                .IsUnicode(false);

            modelBuilder.Entity<MobileErrorLog>()
                .Property(e => e.PhoneBuildNumber)
                .IsUnicode(false);

            modelBuilder.Entity<MobileErrorLog>()
                .Property(e => e.GBAppVersion)
                .IsUnicode(false);

            modelBuilder.Entity<MobileSyncLog>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<MobileSyncLog>()
                .Property(e => e.RequestDetail)
                .IsUnicode(false);

            modelBuilder.Entity<MobileSyncLog>()
                .Property(e => e.ErrorLog)
                .IsUnicode(false);

            modelBuilder.Entity<MobileSyncLog>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<MobileSyncLog>()
                .HasMany(e => e.MobileSyncLogDetails)
                .WithRequired(e => e.MobileSyncLog)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MobileSyncLogDetail>()
                .Property(e => e.RequestParam)
                .IsUnicode(false);

            modelBuilder.Entity<MobileSyncLogDetail>()
                .Property(e => e.ErrorLog)
                .IsUnicode(false);

            modelBuilder.Entity<MobileSyncLogDetail>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<MobileSyncLogDetail>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<CumAI>()
              .Property(e => e.VoucherNo)
              .IsUnicode(false);

            modelBuilder.Entity<CumAI>()
                .Property(e => e.OfficeID)
                .IsUnicode(false);

            modelBuilder.Entity<CumAI>()
                .Property(e => e.AccCode)
                .IsUnicode(false);

            modelBuilder.Entity<CumAI>()
                .Property(e => e.Naration)
                .IsUnicode(false);

            modelBuilder.Entity<CumAI>()
                .Property(e => e.ReconPurposeCode)
                .IsUnicode(false);

            modelBuilder.Entity<CumAI>()
                .Property(e => e.Reference)
                .IsUnicode(false);

            modelBuilder.Entity<CumAI>()
                .Property(e => e.VoucherType)
                .IsUnicode(false);

            modelBuilder.Entity<CumMi>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<CumMi>()
                .Property(e => e.NoOfLoanee)
                .HasPrecision(18, 0);

            modelBuilder.Entity<CumMi>()
                .Property(e => e.UpToLoanDis)
                .HasPrecision(18, 0);

            modelBuilder.Entity<CumMi>()
                .Property(e => e.UptoDisburseMent)
                .HasPrecision(18, 0);

            modelBuilder.Entity<CumMi>()
                .Property(e => e.UpToRecovery)
                .HasPrecision(18, 0);

            modelBuilder.Entity<CumMi>()
                .Property(e => e.UptoAdmission)
                .HasPrecision(18, 0);

            modelBuilder.Entity<CumMi>()
                .Property(e => e.UpToDropOut)
                .HasPrecision(18, 0);

            modelBuilder.Entity<CumMi>()
                .Property(e => e.UptoFullyRepaid)
                .HasPrecision(18, 0);

            modelBuilder.Entity<CumMi>()
                .Property(e => e.UptoDeposit)
                .HasPrecision(18, 0);

            modelBuilder.Entity<CumMi>()
                .Property(e => e.UptoInterest)
                .HasPrecision(18, 0);

            modelBuilder.Entity<CumMi>()
                .Property(e => e.uptowithdrawal)
                .HasPrecision(18, 0);

            modelBuilder.Entity<WorkingAreaLog>()
               .Property(e => e.MainProductCode)
               .IsUnicode(false);

            modelBuilder.Entity<WorkingAreaLog>()
                .Property(e => e.WorkingArea)
                .IsUnicode(false);

            modelBuilder.Entity<WorkingAreaLog>()
                .Property(e => e.Upzilla)
                .HasPrecision(18, 0);

            modelBuilder.Entity<WorkingAreaLog>()
                .Property(e => e.Municipality)
                .HasPrecision(18, 0);

            modelBuilder.Entity<WorkingAreaLog>()
                .Property(e => e.Village)
                .HasPrecision(18, 0);

            modelBuilder.Entity<WorkingAreaLog>()
                .Property(e => e.SelfEnterprenuerMale)
                .HasPrecision(18, 0);

            modelBuilder.Entity<WorkingAreaLog>()
                .Property(e => e.SelfEnterprenuerFeMale)
                .HasPrecision(18, 0);

            modelBuilder.Entity<WorkingAreaLog>()
                .Property(e => e.PaidEnterPrenuerOwnFamilyMale)
                .HasPrecision(18, 0);

            modelBuilder.Entity<WorkingAreaLog>()
                .Property(e => e.PaidEnterPrenuerOwnFamilyFeMale)
                .HasPrecision(18, 0);

            modelBuilder.Entity<WorkingAreaLog>()
                .Property(e => e.PaidEnterPrenuerOutSideMale)
                .HasPrecision(18, 0);

            modelBuilder.Entity<WorkingAreaLog>()
                .Property(e => e.PaidEnterPrenuerOutSideFeMale)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Miscellaneou>()
               .Property(e => e.CreateUser)
               .IsUnicode(false);

            modelBuilder.Entity<MemberPassBookStock>()
               .Property(e => e.Qty)
               .HasPrecision(9, 0);

            modelBuilder.Entity<MemberPassBookStock>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            //modelBuilder.Entity<MemberPassBookRegister>()
            //    .Property(e => e.MemberPassBookNO)
            //    .IsUnicode(false);

            modelBuilder.Entity<MemberPassBookRegister>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<ExpireInfo>()
             .Property(e => e.ExpiryName)
             .IsUnicode(false);

            modelBuilder.Entity<ExpireInfo>()
                .Property(e => e.Relation)
                .IsUnicode(false);

            modelBuilder.Entity<ExpireInfo>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<ExpireInfo>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<Organization>()
              .Property(e => e.OrganizationCode)
              .IsUnicode(false);

            modelBuilder.Entity<Organization>()
                .Property(e => e.OrganizationName)
                .IsUnicode(false);

            modelBuilder.Entity<Organization>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<Organization>()
               .Property(e => e.OrgAddress)
               .IsUnicode(false);

            modelBuilder.Entity<Organization>()
             .Property(e => e.MemberAge);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.AccCharts)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.AccNotes)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.ApplicationSettings)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.AspNetRoles)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.Centers)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.DailyLoanTrxes)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.DailySavingTrxes)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.Employees)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.EmployeeOfficeMappings)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.Groups)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.Holidays)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.Investors)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.LoanCorrectionTrxes)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.LoanSummaries)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.LoanTrxes)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.Members)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.MemberCategories)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.MemberLastCodes)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.Offices)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.AccountingInterfaces)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.MonthWiseSavingInterests)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.ProcessInfoes)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.Products)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.Purposes)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.RepaymentSchedules)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.SavingCorrectionTrxes)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.SavingSummaries)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.SavingTrxes)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.SchedulerDetails)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(e => e.TransferHistories)
            //    .WithRequired(e => e.Organization)
            //    .WillCascadeOnDelete(false);


            modelBuilder.Entity<FamilyGrace>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<FamilyGrace>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<RepaymentSchedule>()
               .Property(e => e.LoanInstallment)
               .HasPrecision(9, 2);

            modelBuilder.Entity<RepaymentSchedule>()
                .Property(e => e.IntInstallment)
                .HasPrecision(9, 2);

            modelBuilder.Entity<RepaymentSchedule>()
                .Property(e => e.IntCharge)
                .HasPrecision(9, 2);

            modelBuilder.Entity<RepaymentSchedule>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<CreditScore>()
               .Property(e => e.CenterID)
               .IsUnicode(false);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.ProductID)
                .IsUnicode(false);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.MemberID)
                .IsUnicode(false);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.OfficeName)
                .IsUnicode(false);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.EmpName)
                .IsUnicode(false);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.MemberName)
                .IsUnicode(false);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.RefereeName)
                .IsUnicode(false);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.MemberAddress)
                .IsUnicode(false);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.SSN)
                .IsUnicode(false);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.PrincipalLoan)
                .HasPrecision(9, 2);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.LoanPaid)
                .HasPrecision(9, 2);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.LoanBalance)
                .HasPrecision(9, 2);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.InterestPaid)
                .HasPrecision(9, 2);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.LoanInstallment)
                .HasPrecision(9, 2);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.IntInstallment)
                .HasPrecision(9, 2);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.LoanItem)
                .IsUnicode(false);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.PurposeName)
                .IsUnicode(false);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.LoanPaid_ThisMonth)
                .HasPrecision(9, 2);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.IntPaid_ThisMonth)
                .HasPrecision(9, 2);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.LastLoanPaid)
                .HasPrecision(9, 2);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.LastIntPaid)
                .HasPrecision(9, 2);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.MemberFirstName)
                .IsUnicode(false);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.MemberMiddleName)
                .IsUnicode(false);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.MemberLastName)
                .IsUnicode(false);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.StreetAddress1)
                .IsUnicode(false);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.StreetAddress2)
                .IsUnicode(false);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.State)
                .IsUnicode(false);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.ZipCode)
                .IsUnicode(false);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.TypeofID)
                .IsUnicode(false);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.IDComments)
                .IsUnicode(false);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.Race)
                .IsUnicode(false);

            modelBuilder.Entity<CreditScore>()
                .Property(e => e.Ethnicity)
                .IsUnicode(false);

            modelBuilder.Entity<DashBoard>()
               .Property(e => e.TotalTodaysMember)
               .HasPrecision(9, 0);

            modelBuilder.Entity<DashBoard>()
                .Property(e => e.TotalTodaysBorrower)
                .HasPrecision(9, 0);

            modelBuilder.Entity<DashBoard>()
                .Property(e => e.Ratio)
                .HasPrecision(9, 2);

            modelBuilder.Entity<DashBoard>()
                .Property(e => e.OverDueAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<DashBoard>()
                .Property(e => e.DueAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<DashBoard>()
                .Property(e => e.PaidAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<DashBoard>()
                .Property(e => e.Otr)
                .HasPrecision(5, 2);

            modelBuilder.Entity<DashBoard>()
                .Property(e => e.ItemCode)
                .IsUnicode(false);

            modelBuilder.Entity<DashBoard>()
                .Property(e => e.ItemName)
                .IsUnicode(false);

            modelBuilder.Entity<DashBoard>()
                .Property(e => e.Members)
                .HasPrecision(9, 0);

            modelBuilder.Entity<DashBoard>()
                .Property(e => e.BarYear)
                .IsUnicode(false);


            modelBuilder.Entity<Scheduler>()
            .Property(e => e.SchedulerName)
            .IsUnicode(false);

            modelBuilder.Entity<Scheduler>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Scheduler>()
                .Property(e => e.Frequency)
                .IsUnicode(false);

            modelBuilder.Entity<Scheduler>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<Scheduler>()
                .HasMany(e => e.SchedulerDetails)
                .WithRequired(e => e.Scheduler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SchedulerDetail>()
                .Property(e => e.ErrorDescription)
                .IsUnicode(false);

            modelBuilder.Entity<SchedulerDetail>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<SmsConfig>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<SmsLog>()
                .Property(e => e.SmsType)
                .IsUnicode(false);

            modelBuilder.Entity<SmsLog>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
               .Property(e => e.ActionURL)
               .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.ClientIP)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.RequestUser)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.RequestDetail)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.ErrorDetail)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationLog>()
                .Property(e => e.UserAgent)
                .IsUnicode(false);

            modelBuilder.Entity<LoanCorrectionTrx>()
              .Property(e => e.MemberCode)
              .IsUnicode(false);

            modelBuilder.Entity<LoanCorrectionTrx>()
                .Property(e => e.MemberName)
                .IsUnicode(false);

            modelBuilder.Entity<LoanCorrectionTrx>()
                .Property(e => e.ProductCode)
                .IsUnicode(false);

            modelBuilder.Entity<LoanCorrectionTrx>()
                .Property(e => e.ProductName)
                .IsUnicode(false);

            modelBuilder.Entity<LoanCorrectionTrx>()
                .Property(e => e.InterestCalculationMethod)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<LoanCorrectionTrx>()
                .Property(e => e.PrincipalLoan)
                .HasPrecision(9, 2);

            modelBuilder.Entity<LoanCorrectionTrx>()
                .Property(e => e.LoanRepaid)
                .HasPrecision(9, 2);

            modelBuilder.Entity<LoanCorrectionTrx>()
                .Property(e => e.LoanDue)
                .HasPrecision(9, 2);

            modelBuilder.Entity<LoanCorrectionTrx>()
                .Property(e => e.LoanPaid)
                .HasPrecision(9, 2);

            modelBuilder.Entity<LoanCorrectionTrx>()
                .Property(e => e.CumIntCharge)
                .HasPrecision(9, 2);

            modelBuilder.Entity<LoanCorrectionTrx>()
                .Property(e => e.IntCharge)
                .HasPrecision(9, 2);

            modelBuilder.Entity<LoanCorrectionTrx>()
                .Property(e => e.IntDue)
                .HasPrecision(9, 2);

            modelBuilder.Entity<LoanCorrectionTrx>()
                .Property(e => e.IntPaid)
                .HasPrecision(9, 2);

            modelBuilder.Entity<LoanCorrectionTrx>()
                .Property(e => e.Advance)
                .HasPrecision(9, 2);

            modelBuilder.Entity<LoanCorrectionTrx>()
                .Property(e => e.DueRecovery)
                .HasPrecision(9, 2);

            modelBuilder.Entity<LoanCorrectionTrx>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<SavingCorrectionTrx>()
                .Property(e => e.MemberCode)
                .IsUnicode(false);

            modelBuilder.Entity<SavingCorrectionTrx>()
                .Property(e => e.MemberName)
                .IsUnicode(false);

            modelBuilder.Entity<SavingCorrectionTrx>()
                .Property(e => e.ProductCode)
                .IsUnicode(false);

            modelBuilder.Entity<SavingCorrectionTrx>()
                .Property(e => e.ProductName)
                .IsUnicode(false);

            modelBuilder.Entity<SavingCorrectionTrx>()
                .Property(e => e.DueSavingInstallment)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingCorrectionTrx>()
                .Property(e => e.SavingInstallment)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingCorrectionTrx>()
                .Property(e => e.Deposit)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingCorrectionTrx>()
                .Property(e => e.Withdrawal)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingCorrectionTrx>()
                .Property(e => e.Balance)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingCorrectionTrx>()
                .Property(e => e.Penalty)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingCorrectionTrx>()
                .Property(e => e.MonthlyInterest)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingCorrectionTrx>()
                .Property(e => e.TransferDeposit)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingCorrectionTrx>()
                .Property(e => e.TransferWithdrawal)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingCorrectionTrx>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);
            modelBuilder.Entity<LoanCorrection>()
             .Property(e => e.PrincipalLoan)
             .HasPrecision(9, 2);

            modelBuilder.Entity<LoanCorrection>()
                .Property(e => e.LoanDue)
                .HasPrecision(9, 2);

            modelBuilder.Entity<LoanCorrection>()
                .Property(e => e.LoanPaid)
                .HasPrecision(9, 2);

            modelBuilder.Entity<LoanCorrection>()
                .Property(e => e.IntCharge)
                .HasPrecision(9, 2);

            modelBuilder.Entity<LoanCorrection>()
                .Property(e => e.IntDue)
                .HasPrecision(9, 2);

            modelBuilder.Entity<LoanCorrection>()
                .Property(e => e.IntPaid)
                .HasPrecision(9, 2);

            modelBuilder.Entity<LoanCorrection>()
                .Property(e => e.Advance)
                .HasPrecision(9, 2);

            modelBuilder.Entity<LoanCorrection>()
                .Property(e => e.DueRecovery)
                .HasPrecision(9, 2);

            modelBuilder.Entity<LoanCorrection>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<SavingCorrection>()
                .Property(e => e.Deposit)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingCorrection>()
                .Property(e => e.Withdrawal)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingCorrection>()
                .Property(e => e.Balance)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingCorrection>()
                .Property(e => e.Penalty)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingCorrection>()
                .Property(e => e.MonthlyInterest)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingCorrection>()
                .Property(e => e.TransferDeposit)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingCorrection>()
                .Property(e => e.TransferWithdrawal)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingCorrection>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<AccCategory>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<AccChart>()
         .Property(e => e.AccCode)
         .IsUnicode(false);

            modelBuilder.Entity<AccChart>()
                .Property(e => e.AccName)
                .IsUnicode(false);

            modelBuilder.Entity<AccChart>()
                .Property(e => e.Nature)
                .IsUnicode(false);

            modelBuilder.Entity<AccChart>()
                .Property(e => e.FirstLevel)
                .IsUnicode(false);

            modelBuilder.Entity<AccChart>()
                .Property(e => e.SecondLevel)
                .IsUnicode(false);

            modelBuilder.Entity<AccChart>()
                .Property(e => e.ThirdLevel)
                .IsUnicode(false);

            modelBuilder.Entity<AccChart>()
                .Property(e => e.FourthLevel)
                .IsUnicode(false);

            modelBuilder.Entity<AccChart>()
                .Property(e => e.FifthLevel)
                .IsUnicode(false);

            modelBuilder.Entity<AccChart>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<AccLastVoucher>()
                .Property(e => e.VoucherNo)
                .IsUnicode(false);

            modelBuilder.Entity<AccNote>()
               .Property(e => e.CreateUser)
               .IsUnicode(false);

            modelBuilder.Entity<AccTrxDetail>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<AccTrxMaster>()
                .Property(e => e.VoucherNo)
                .IsUnicode(false);

            modelBuilder.Entity<AccTrxMaster>()
                .Property(e => e.VoucherDesc)
                .IsUnicode(false);

            modelBuilder.Entity<AccTrxMaster>()
                .Property(e => e.VoucherType)
                .IsUnicode(false);

            modelBuilder.Entity<AccTrxMaster>()
                .Property(e => e.Reference)
                .IsUnicode(false);

            modelBuilder.Entity<AccTrxMaster>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<AccTrxMaster>()
                .HasMany(e => e.AccTrxDetails)
                .WithRequired(e => e.AccTrxMaster)
                //  .HasForeignKey(e => e.TrxMasterID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationSetting>()
                .Property(e => e.PLAccount)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationSetting>()
                .Property(e => e.BankAccount)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationSetting>()
                .Property(e => e.OrganizationAddress)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationSetting>()
                .Property(e => e.PhoneNo)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationSetting>()
                .Property(e => e.CellNo)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationSetting>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationSetting>()
                .Property(e => e.LicenseNo)
                .IsUnicode(false);

            modelBuilder.Entity<ApplicationSetting>()
               .Property(e => e.ProcessType)
               .IsUnicode(false);

            modelBuilder.Entity<ApplicationSetting>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<Area>()
                .Property(e => e.AreaCode)
                .IsUnicode(false);

            modelBuilder.Entity<Area>()
                .Property(e => e.AreaName)
                .IsUnicode(false);

            modelBuilder.Entity<Area>()
                .Property(e => e.ZoneName)
                .IsUnicode(false);

            modelBuilder.Entity<Area>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            //modelBuilder.Entity<AspNetRole>()
            //    .HasMany(e => e.AspNetRoleModules)
            //    .WithRequired(e => e.AspNetRole)
            //    .HasForeignKey(e => e.RoleId)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<AspNetRole>()
            //    .HasMany(e => e.AspNetUsers)
            //    .WithMany(e => e.AspNetRoles)
            //    .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            //modelBuilder.Entity<AspNetSecurityLevel>()
            //    .Property(e => e.SecurityLevelCode)
            //    .IsUnicode(false);

            //modelBuilder.Entity<AspNetSecurityLevel>()
            //    .Property(e => e.SecurityLevelName)
            //    .IsUnicode(false);

            //modelBuilder.Entity<AspNetSecurityLevel>()
            //    .HasMany(e => e.AspNetRoleModules)
            //    .WithRequired(e => e.AspNetSecurityLevel)
            //    .HasForeignKey(e => e.SecurityLevelId)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<AspNetSecurityModule>()
            //    .Property(e => e.SecurityModuleCode)
            //    .IsUnicode(false);

            //modelBuilder.Entity<AspNetSecurityModule>()
            //    .Property(e => e.SecurityModuleName)
            //    .IsUnicode(false);

            //modelBuilder.Entity<AspNetSecurityModule>()
            //    .HasMany(e => e.AspNetRoleModules)
            //    .WithRequired(e => e.AspNetSecurityModule)
            //    .HasForeignKey(e => e.ModuleId)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<AspNetUser>()
            //    .HasMany(e => e.AspNetUserClaims)
            //    .WithRequired(e => e.AspNetUser)
            //    .HasForeignKey(e => e.UserId);

            //modelBuilder.Entity<AspNetUser>()
            //    .HasMany(e => e.AspNetUserLogins)
            //    .WithRequired(e => e.AspNetUser)
            //    .HasForeignKey(e => e.UserId);
            modelBuilder.Entity<AspNetRole>()
              .Property(e => e.DefaultLinkURL)
              .IsUnicode(false);

            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetRoleModules)
                .WithRequired(e => e.AspNetRole)
                .HasForeignKey(e => e.RoleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetSecurityLevel>()
                .Property(e => e.SecurityLevelCode)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetSecurityLevel>()
                .Property(e => e.SecurityLevelName)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetSecurityLevel>()
                .HasMany(e => e.AspNetRoleModules)
                .WithRequired(e => e.AspNetSecurityLevel)
                .HasForeignKey(e => e.SecurityLevelId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetSecurityModule>()
                .Property(e => e.SecurityModuleCode)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetSecurityModule>()
                .Property(e => e.LinkText)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetSecurityModule>()
                .Property(e => e.ControllerName)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetSecurityModule>()
                .Property(e => e.ActionName)
                .IsUnicode(false);

            modelBuilder.Entity<AspNetSecurityModule>()
                .HasMany(e => e.AspNetRoleModules)
                .WithRequired(e => e.AspNetSecurityModule)
                .HasForeignKey(e => e.ModuleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Branch>()
                .Property(e => e.BranchCode)
                .IsUnicode(false);

            modelBuilder.Entity<Branch>()
                .Property(e => e.BranchName)
                .IsUnicode(false);

            modelBuilder.Entity<Branch>()
                .Property(e => e.BranchAddress)
                .IsUnicode(false);

            modelBuilder.Entity<Branch>()
                .Property(e => e.PostCode)
                .IsUnicode(false);

            modelBuilder.Entity<Branch>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Branch>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Branch>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<Budget>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            /*
            modelBuilder.Entity<Center>()
                .Property(e => e.CenterCode)
                .IsUnicode(false);

            modelBuilder.Entity<Center>()
                .Property(e => e.CenterName)
                .IsUnicode(false);

            modelBuilder.Entity<Center>()
                .Property(e => e.CenterAddress)
                .IsUnicode(false);

            modelBuilder.Entity<Center>()
                .Property(e => e.CollectionDay)
                .IsUnicode(false);

            modelBuilder.Entity<Center>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);
            */

            modelBuilder.Entity<Center>()
                .HasMany(e => e.DailyLoanTrxes)
                .WithRequired(e => e.Center)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Center>()
                .HasMany(e => e.Holidays)
                .WithRequired(e => e.Center)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Center>()
                .HasMany(e => e.LoanSummaries)
                .WithRequired(e => e.Center)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Center>()
                .HasMany(e => e.LoanTrxes)
                .WithRequired(e => e.Center)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Center>()
                .HasMany(e => e.Members)
                .WithRequired(e => e.Center)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Center>()
                .HasMany(e => e.SavingSummaries)
                .WithRequired(e => e.Center)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DailyLoanTrx>()
                .Property(e => e.MemberCode)
                .IsUnicode(false);

            modelBuilder.Entity<DailyLoanTrx>()
                .Property(e => e.MemberName)
                .IsUnicode(false);

            modelBuilder.Entity<DailyLoanTrx>()
                .Property(e => e.ProductCode)
                .IsUnicode(false);

            modelBuilder.Entity<DailyLoanTrx>()
                .Property(e => e.ProductName)
                .IsUnicode(false);

            modelBuilder.Entity<DailyLoanTrx>()
                .Property(e => e.InterestCalculationMethod)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DailyLoanTrx>()
                .Property(e => e.PrincipalLoan)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DailyLoanTrx>()
                .Property(e => e.LoanRepaid)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DailyLoanTrx>()
                .Property(e => e.LoanDue)
                .HasPrecision(12, 2);

            modelBuilder.Entity<DailyLoanTrx>()
                .Property(e => e.LoanPaid)
                .HasPrecision(12, 2);

            modelBuilder.Entity<DailyLoanTrx>()
                .Property(e => e.CumIntCharge)
                .HasPrecision(12, 2);

            modelBuilder.Entity<DailyLoanTrx>()
                .Property(e => e.IntCharge)
                .HasPrecision(12, 2);

            modelBuilder.Entity<DailyLoanTrx>()
                .Property(e => e.IntDue)
                .HasPrecision(12, 2);

            modelBuilder.Entity<DailyLoanTrx>()
                .Property(e => e.IntPaid)
                .HasPrecision(12, 2);

            modelBuilder.Entity<DailyLoanTrx>()
                .Property(e => e.Advance)
                .HasPrecision(12, 2);

            modelBuilder.Entity<DailyLoanTrx>()
                .Property(e => e.DueRecovery)
                .HasPrecision(12, 2);

            modelBuilder.Entity<DailyLoanTrx>()
                .Property(e => e.DurationOverLoanDue)
                .HasPrecision(12, 0);

            modelBuilder.Entity<DailyLoanTrx>()
                .Property(e => e.DurationOverIntDue)
                .HasPrecision(12, 0);
            modelBuilder.Entity<DailyLoanTrx>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<DailySavingTrx>()
                .Property(e => e.MemberCode)
                .IsUnicode(false);

            modelBuilder.Entity<DailySavingTrx>()
                .Property(e => e.MemberName)
                .IsUnicode(false);

            modelBuilder.Entity<DailySavingTrx>()
                .Property(e => e.ProductCode)
                .IsUnicode(false);

            modelBuilder.Entity<DailySavingTrx>()
                .Property(e => e.ProductName)
                .IsUnicode(false);

            modelBuilder.Entity<DailySavingTrx>()
                .Property(e => e.DueSavingInstallment)
                .HasPrecision(14, 2);

            modelBuilder.Entity<DailySavingTrx>()
                .Property(e => e.SavingInstallment)
                .HasPrecision(14, 2);

            modelBuilder.Entity<DailySavingTrx>()
                .Property(e => e.Deposit)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DailySavingTrx>()
                .Property(e => e.Withdrawal)
                .HasPrecision(14, 2);

            modelBuilder.Entity<DailySavingTrx>()
                .Property(e => e.Balance)
                .HasPrecision(18, 2);

            modelBuilder.Entity<DailySavingTrx>()
                .Property(e => e.Penalty)
                .HasPrecision(14, 2);

            modelBuilder.Entity<DailySavingTrx>()
                .Property(e => e.MonthlyInterest)
                .HasPrecision(14, 2);

            modelBuilder.Entity<DailySavingTrx>()
                .Property(e => e.TransferDeposit)
                .HasPrecision(14, 2);

            modelBuilder.Entity<DailySavingTrx>()
                .Property(e => e.TransferWithdrawal)
                .HasPrecision(14, 2);

            modelBuilder.Entity<DailySavingTrx>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<EmployeeOfficeMapping>()
             .Property(e => e.CreateUser)
             .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.EmployeeCode)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.EmpName)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.GuardianName)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.EmpAddress)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.PhoneNo)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Designation)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<GeoLocation>()
                .Property(e => e.LocationName)
                .IsUnicode(false);

            modelBuilder.Entity<GeoLocation>()
                .Property(e => e.FirstLevel)
                .IsUnicode(false);

            modelBuilder.Entity<GeoLocation>()
                .Property(e => e.SecondLevel)
                .IsUnicode(false);

            modelBuilder.Entity<GeoLocation>()
                .Property(e => e.ThirdLevel)
                .IsUnicode(false);

            modelBuilder.Entity<GeoLocation>()
                .Property(e => e.FourthLevel)
                .IsUnicode(false);

            modelBuilder.Entity<GeoLocation>()
                .Property(e => e.FifthLevel)
                .IsUnicode(false);

            modelBuilder.Entity<GeoLocation>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .Property(e => e.GroupCode)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.Members)
                .WithRequired(e => e.Group)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Holiday>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Holiday>()
                .Property(e => e.HolidayType)
                .IsUnicode(false);

            modelBuilder.Entity<Holiday>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<Investor>()
                .Property(e => e.InvestorCode)
                .IsUnicode(false);

            modelBuilder.Entity<Investor>()
                .Property(e => e.InvestorName)
                .IsUnicode(false);

            modelBuilder.Entity<Investor>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            //modelBuilder.Entity<LoanSummary>()
            //    .Property(e => e.PrincipalLoan)
            //    .HasPrecision(9, 2);

            //modelBuilder.Entity<LoanSummary>()
            //    .Property(e => e.LoanRepaid)
            //    .HasPrecision(9, 2);

            //modelBuilder.Entity<LoanSummary>()
            //    .Property(e => e.IntCharge)
            //    .HasPrecision(9, 2);

            //modelBuilder.Entity<LoanSummary>()
            //    .Property(e => e.IntPaid)
            //    .HasPrecision(9, 2);

            //modelBuilder.Entity<LoanSummary>()
            //    .Property(e => e.LoanInstallment)
            //    .HasPrecision(9, 2);

            //modelBuilder.Entity<LoanSummary>()
            //    .Property(e => e.IntInstallment)
            //    .HasPrecision(9, 2);

            //modelBuilder.Entity<LoanSummary>()
            //    .Property(e => e.InterestRate)
            //    .HasPrecision(5, 2);

            //modelBuilder.Entity<LoanSummary>()
            //    .Property(e => e.Balance)
            //    .HasPrecision(9, 2);

            //modelBuilder.Entity<LoanSummary>()
            //    .Property(e => e.Advance)
            //    .HasPrecision(9, 2);

            //modelBuilder.Entity<LoanSummary>()
            //    .Property(e => e.DueRecovery)
            //    .HasPrecision(9, 2);

            //modelBuilder.Entity<LoanSummary>()
            //    .Property(e => e.ExcessPay)
            //    .HasPrecision(5, 2);

            //modelBuilder.Entity<LoanSummary>()
            //    .Property(e => e.CurLoan)
            //    .HasPrecision(9, 2);

            //modelBuilder.Entity<LoanSummary>()
            //    .Property(e => e.PreLoan)
            //    .HasPrecision(9, 2);

            //modelBuilder.Entity<LoanSummary>()
            //    .Property(e => e.CumLoanDue)
            //    .HasPrecision(9, 2);

            //modelBuilder.Entity<LoanSummary>()
            //    .Property(e => e.WriteOffLoan)
            //    .HasPrecision(9, 2);

            //modelBuilder.Entity<LoanSummary>()
            //    .Property(e => e.WriteOffInterest)
            //    .HasPrecision(9, 2);

            //modelBuilder.Entity<LoanSummary>()
            //    .Property(e => e.CreateUser)
            //    .IsUnicode(false);

            //modelBuilder.Entity<LoanSummary>()
            //    .HasMany(e => e.DailyLoanTrxes)
            //    .WithRequired(e => e.LoanSummary)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<LoanSummary>()
            //    .HasMany(e => e.LoanTrxes)
            //    .WithRequired(e => e.LoanSummary)
            //    .WillCascadeOnDelete(false);
            modelBuilder.Entity<LoanSummary>()
               .Property(e => e.PrincipalLoan)
               .HasPrecision(12, 2);

            modelBuilder.Entity<LoanSummary>()
                .Property(e => e.LoanRepaid)
                .HasPrecision(12, 2);

            modelBuilder.Entity<LoanSummary>()
                .Property(e => e.IntCharge)
                .HasPrecision(12, 2);

            modelBuilder.Entity<LoanSummary>()
                .Property(e => e.IntPaid)
                .HasPrecision(12, 2);

            modelBuilder.Entity<LoanSummary>()
                .Property(e => e.LoanInstallment)
                .HasPrecision(12, 2);

            modelBuilder.Entity<LoanSummary>()
                .Property(e => e.IntInstallment)
                .HasPrecision(12, 2);

            modelBuilder.Entity<LoanSummary>()
                .Property(e => e.InterestRate)
                .HasPrecision(5, 2);

            modelBuilder.Entity<LoanSummary>()
                .Property(e => e.Balance)
                .HasPrecision(9, 2);

            modelBuilder.Entity<LoanSummary>()
                .Property(e => e.Advance)
                .HasPrecision(12, 2);

            modelBuilder.Entity<LoanSummary>()
                .Property(e => e.DueRecovery)
                .HasPrecision(12, 2);

            modelBuilder.Entity<LoanSummary>()
                .Property(e => e.ExcessPay)
                .HasPrecision(12, 2);

            modelBuilder.Entity<LoanSummary>()
                .Property(e => e.CurLoan)
                .HasPrecision(12, 2);

            modelBuilder.Entity<LoanSummary>()
                .Property(e => e.PreLoan)
                .HasPrecision(12, 2);

            modelBuilder.Entity<LoanSummary>()
                .Property(e => e.CumLoanDue)
                .HasPrecision(12, 2);

            modelBuilder.Entity<LoanSummary>()
                .Property(e => e.WriteOffLoan)
                .HasPrecision(12, 2);

            modelBuilder.Entity<LoanSummary>()
                .Property(e => e.WriteOffInterest)
                .HasPrecision(12, 2);

            modelBuilder.Entity<LoanSummary>()
               .Property(e => e.BankName)
               .IsUnicode(false);

            modelBuilder.Entity<LoanSummary>()
                .Property(e => e.ChequeNo)
                .IsUnicode(false);

            modelBuilder.Entity<LoanSummary>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);
            //////////new field////////////////
            modelBuilder.Entity<LoanSummary>()
                 .Property(e => e.ChequeNo)
                 .IsUnicode(false);

            modelBuilder.Entity<LoanSummary>()
                .Property(e => e.Guarantor)
                .IsUnicode(false);


            //////////new field////////////////
            modelBuilder.Entity<LoanSummary>()
                .HasMany(e => e.DailyLoanTrxes)
                .WithRequired(e => e.LoanSummary)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LoanSummary>()
                .HasMany(e => e.LoanTrxes)
                .WithRequired(e => e.LoanSummary)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<LoanTrx>()
                .Property(e => e.PrincipalLoan)
                .HasPrecision(12, 2);

            modelBuilder.Entity<LoanTrx>()
                .Property(e => e.LoanDue)
                .HasPrecision(12, 2);

            modelBuilder.Entity<LoanTrx>()
                .Property(e => e.LoanPaid)
                .HasPrecision(12, 2);

            modelBuilder.Entity<LoanTrx>()
                .Property(e => e.IntCharge)
                .HasPrecision(12, 2);

            modelBuilder.Entity<LoanTrx>()
                .Property(e => e.IntDue)
                .HasPrecision(12, 2);

            modelBuilder.Entity<LoanTrx>()
                .Property(e => e.IntPaid)
                .HasPrecision(12, 2);

            modelBuilder.Entity<LoanTrx>()
                .Property(e => e.Advance)
                .HasPrecision(12, 2);

            modelBuilder.Entity<LoanTrx>()
                .Property(e => e.DueRecovery)
                .HasPrecision(12, 2);

            modelBuilder.Entity<LoanTrx>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            //modelBuilder.Entity<LoanTrx>()
            //    .HasOptional(e => e.LoanTrx1)
            //    .WithRequired(e => e.LoanTrx2);

            /*
            modelBuilder.Entity<Member>()
                .Property(e => e.MemberCode)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.FirstName)
                .IsUnicode(true);

            modelBuilder.Entity<Member>()
                .Property(e => e.MiddleName)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.AddressLine1)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.AddressLine2)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.RefereeName)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.NationalID)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.StateName)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.ZipCode)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.CountryOfIssue)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.NIDComments)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.IDType)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.Race)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.Ethnicity)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.PhoneNo)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.nsAccountNo)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);
            */

            modelBuilder.Entity<Member>()
                .HasMany(e => e.DailyLoanTrxes)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.LoanSummaries)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.LoanTrxes)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.SavingSummaries)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            /*
            modelBuilder.Entity<MemberCategory>()
                .Property(e => e.MemberCategoryCode)
                .IsUnicode(false);

            modelBuilder.Entity<MemberCategory>()
                .Property(e => e.CategoryName)
                .IsUnicode(false);

            modelBuilder.Entity<MemberCategory>()
                .Property(e => e.CategoryShortName)
                .IsUnicode(false);

            modelBuilder.Entity<MemberCategory>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);
            */

            modelBuilder.Entity<MemberCategory>()
              .Property(e => e.AdmissionFee)
              .HasPrecision(18, 0);

            modelBuilder.Entity<MemberCategory>()
                .Property(e => e.PassBookFee)
                .HasPrecision(18, 0);

            modelBuilder.Entity<MemberCategory>()
                .Property(e => e.LoanFormFee)
                .HasPrecision(18, 0);
            //modelBuilder.Entity<MemberLastCode>()
            //    .Property(e => e.LastCode)
            //    .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.OfficeCode)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.FirstLevel)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.SecondLevel)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.ThirdLevel)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.FourthLevel)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.OfficeAddress)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.PostCode)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<Office>()
                .HasMany(e => e.Centers)
                .WithRequired(e => e.Office)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Office>()
                .HasMany(e => e.DailyLoanTrxes)
                .WithRequired(e => e.Office)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Office>()
                .HasMany(e => e.Employees)
                .WithRequired(e => e.Office)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Office>()
                .HasMany(e => e.Groups)
                .WithRequired(e => e.Office)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Office>()
                .HasMany(e => e.Holidays)
                .WithRequired(e => e.Office)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Office>()
                .HasMany(e => e.LoanSummaries)
                .WithRequired(e => e.Office)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Office>()
                .HasMany(e => e.LoanTrxes)
                .WithRequired(e => e.Office)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Office>()
                .HasMany(e => e.Members)
                .WithRequired(e => e.Office)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Office>()
                .HasMany(e => e.SavingSummaries)
                .WithRequired(e => e.Office)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Office>()
                .HasMany(e => e.SavingTrxes)
                .WithRequired(e => e.Office)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Office>()
             .HasMany(e => e.EmployeeOfficeMappings)
             .WithRequired(e => e.Office)
             .WillCascadeOnDelete(false);

            modelBuilder.Entity<PNMConfirm>()
                .Property(e => e.due_to_site_currency)
                .IsUnicode(false);

            modelBuilder.Entity<PNMConfirm>()
                .Property(e => e.net_payment_currency)
                .IsUnicode(false);

            modelBuilder.Entity<PNMConfirm>()
                .Property(e => e.payment_currency)
                .IsUnicode(false);

            modelBuilder.Entity<PNMConfirm>()
                .Property(e => e.pnm_withheld_currency)
                .IsUnicode(false);

            modelBuilder.Entity<PNMConfirm>()
                .Property(e => e.status)
                .IsUnicode(false);

            modelBuilder.Entity<PNMConfirm>()
                .Property(e => e.version)
                .IsUnicode(false);
            modelBuilder.Entity<PNMOrder>()
               .Property(e => e.pnm_customer_identifier)
               .IsUnicode(false);

            modelBuilder.Entity<PNMOrder>()
                .Property(e => e.pnm_customer_postal_code)
                .IsUnicode(false);

            modelBuilder.Entity<PNMOrder>()
                .Property(e => e.site_customer_identifier)
                .IsUnicode(false);

            modelBuilder.Entity<PNMOrder>()
                .Property(e => e.latitude)
                .IsUnicode(false);

            modelBuilder.Entity<PNMOrder>()
                .Property(e => e.longitude)
                .IsUnicode(false);

            modelBuilder.Entity<PNMOrder>()
                .Property(e => e.minimum_payment_currency)
                .IsUnicode(false);

            modelBuilder.Entity<PNMOrder>()
                .Property(e => e.order_currency)
                .IsUnicode(false);

            modelBuilder.Entity<PNMOrder>()
                .Property(e => e.order_status)
                .IsUnicode(false);

            modelBuilder.Entity<PNMOrder>()
                .Property(e => e.order_type)
                .IsUnicode(false);

            modelBuilder.Entity<PNMOrder>()
                .Property(e => e.pnm_balance_due_currency)
                .IsUnicode(false);

            modelBuilder.Entity<PNMOrder>()
                .Property(e => e.pnm_customer_language)
                .IsUnicode(false);

            modelBuilder.Entity<PNMOrder>()
                .Property(e => e.pnm_order_crid)
                .IsUnicode(false);

            modelBuilder.Entity<PNMOrder>()
                .Property(e => e.pnm_order_identifier)
                .IsUnicode(false);

            modelBuilder.Entity<PNMOrder>()
                .Property(e => e.pnm_order_short_identifier)
                .IsUnicode(false);

            modelBuilder.Entity<ProcessInfo>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.ProductCode)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.ProductName)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.ProductFullNameEng)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.InterestRate)
                .HasPrecision(10, 5);

            modelBuilder.Entity<Product>()
                .Property(e => e.LoanInstallment)
                .HasPrecision(12, 7);

            modelBuilder.Entity<Product>()
                .Property(e => e.InterestInstallment)
                .HasPrecision(12, 7);

            modelBuilder.Entity<Product>()
                .Property(e => e.SavingsInstallment)
                .HasPrecision(9, 2);

            modelBuilder.Entity<Product>()
                .Property(e => e.MinLimit)
                .HasPrecision(9, 2);

            modelBuilder.Entity<Product>()
                .Property(e => e.MaxLimit)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(e => e.InterestCalculationMethod)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.PaymentFrequency)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.InsuranceItemRate)
                .HasPrecision(7, 0);

            modelBuilder.Entity<Product>()
                .Property(e => e.MainItemName)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.DailyLoanTrxes)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.LoanSummaries)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.LoanTrxes)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.SavingSummaries)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Purpose>()
                .Property(e => e.PurposeCode)
                .IsUnicode(false);

            modelBuilder.Entity<Purpose>()
                .Property(e => e.PurposeName)
                .IsUnicode(false);

            modelBuilder.Entity<Purpose>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<SavingSummary>()
                .Property(e => e.Deposit)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingSummary>()
                .Property(e => e.Withdrawal)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingSummary>()
                .Property(e => e.Balance)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingSummary>()
                .Property(e => e.InterestRate)
                .HasPrecision(5, 2);

            modelBuilder.Entity<SavingSummary>()
                .Property(e => e.SavingInstallment)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingSummary>()
                .Property(e => e.CumInterest)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingSummary>()
                .Property(e => e.MonthlyInterest)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingSummary>()
                .Property(e => e.Penalty)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingSummary>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<SavingTrx>()
                .Property(e => e.Deposit)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingTrx>()
                .Property(e => e.Withdrawal)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingTrx>()
                .Property(e => e.Balance)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingTrx>()
                .Property(e => e.Penalty)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingTrx>()
                .Property(e => e.MonthlyInterest)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingTrx>()
                .Property(e => e.TransferDeposit)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingTrx>()
                .Property(e => e.TransferWithdrawal)
                .HasPrecision(9, 2);

            modelBuilder.Entity<SavingTrx>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);
            modelBuilder.Entity<Scheduler>()
               .Property(e => e.SchedulerName)
               .IsUnicode(false);

            modelBuilder.Entity<Scheduler>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Scheduler>()
                .Property(e => e.Frequency)
                .IsUnicode(false);

            modelBuilder.Entity<Scheduler>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<Scheduler>()
                .HasMany(e => e.SchedulerDetails)
                .WithRequired(e => e.Scheduler)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TransferHistory>()
               .Property(e => e.Status)
               .IsUnicode(false);

            modelBuilder.Entity<Center>()
                .HasMany(e => e.RepaymentSchedules)
                .WithRequired(e => e.Center)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LoanSummary>()
                .HasMany(e => e.RepaymentSchedules)
                .WithRequired(e => e.LoanSummary)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Member>()
                .HasMany(e => e.RepaymentSchedules)
                .WithRequired(e => e.Member)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.RepaymentSchedules)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FileUpload>()
                 .Property(e => e.UploadType)
                 .IsUnicode(false);

            modelBuilder.Entity<FileUpload>()
                .Property(e => e.FileType)
                .IsUnicode(false);

            modelBuilder.Entity<FileUpload>()
                .Property(e => e.FileName)
                .IsUnicode(false);

            modelBuilder.Entity<FileUpload>()
                .Property(e => e.FileLocation)
                .IsUnicode(false);

            modelBuilder.Entity<FileUpload>()
                .Property(e => e.UploadBy)
                .HasPrecision(18, 5);

            modelBuilder.Entity<FileUpload>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);

            modelBuilder.Entity<CumMisItem>()
               .Property(e => e.CumMisItemName)
               .IsUnicode(false);

            modelBuilder.Entity<CumMisItem>()
                .Property(e => e.CreateUser)
                .IsUnicode(false);
        }
    }
}
