using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface ISavingSummaryRepository : IRepository<SavingSummary>
    {

        int updateSavingInstallment(Nullable<int> office, Nullable<long> memberID, Nullable<int> prodID, Nullable<int> orgID, Nullable<int> CenterID, Nullable<System.DateTime> TransactionDate, Nullable<System.DateTime> OpeningDate, Nullable<int> EmployeeId, Nullable<int> MemberCategoryID, string CreateUser);
        IEnumerable<Proc_GetSavingBalanceForCate_Result> GetSavingBalanceForCate(Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID);
        IEnumerable<Proc_get_MaxNoOfAccount_Result> Get_MaxNoOfAccount(Nullable<int> officeID, Nullable<int> orgID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID);
        int MaxAccountNo(SavingSummary savingsummary);
        int SetOpeningSavingEntry(int? orgID, int? officeID);
        IEnumerable<DBSavingSummaryDetails> GetSavingSummaryDetail(int? officeID);
        IEnumerable<DBSavingSummaryDetails> GetSavingSummaryDetailAccountClose(int? OrgID,int? officeID);
        IEnumerable<getSavingCloseAccountInfo_Result> GetSavingAccountCloseInfo(int? OrgID, int? officeID, Nullable<System.DateTime> tranDate);
        IEnumerable<getSavingCloseAccountInfo_Result> GetSavingStopInterestInfo(int? OrgID, int? officeID, Nullable<System.DateTime> tranDate);
        IEnumerable<getSavingCloseAccountInfo_Result> GetSavingClaimableInterestInfo(int? OrgID, int? officeID, Nullable<System.DateTime> tranDate);
        int AccountClose(Nullable<int> OrgID, Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID, Nullable<int> noAccount, Nullable<System.DateTime> tranDate);
        int StopInterestAccount(Nullable<int> OrgID, Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID, Nullable<int> noAccount, Nullable<System.DateTime> tranDate, Nullable<int> Qtype, Nullable<long> SavingSummaryID,Nullable<int> StopOrClaimable,string CreateUser);
        int ClaimableInterestAccount(Nullable<int> OrgID, Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID, Nullable<int> noAccount, Nullable<System.DateTime> tranDate, Nullable<int> Qtype, Nullable<long> SavingSummaryID, Nullable<int> StopOrClaimable, string CreateUser);
        int StartInterestAccount(Nullable<int> OrgID, Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID, Nullable<int> noAccount, Nullable<System.DateTime> tranDate, Nullable<int> Qtype, Nullable<long> SavingSummaryID, Nullable<int> StopOrClaimable,string CreateUser);
        IEnumerable<DBSavingSummaryDetails> GetSavingSummaryDetail(int? officeID, string filterColumnName, string filterValue);
        IEnumerable<DBSavingSummaryDetails> GetSavingSummaryDetailPaged(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount);
        IEnumerable<DBSavingSummaryDetails> GetSavingSummarySavingInterestUpdate(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount);

        int Proc_Set_SavingOpeingWhenMemberEligible(Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID, Nullable<int> noOfAccount, Nullable<System.DateTime> transactionDate, Nullable<decimal> interestRate, Nullable<System.DateTime> openingDate, Nullable<int> employeeId, Nullable<int> memberCategoryID, Nullable<int> orgID, string createUser, Nullable<System.DateTime> createDate);
        IEnumerable<DBSavingSummaryDetails> GetSavingReinstate(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount, DateTime DateFromValue, DateTime DateToValue);
        int InsertConsentForm(int? OrgID, int? officeID, int? centerID, long? memberID, int? productID, int? noAccount, DateTime? tranDate, int? Qtype, long? SavingSummaryID, int? StopOrClaimable, string CreateUser);
    }
    public class SavingSummaryRepository : RepositoryBaseCodeFirst<SavingSummary>,ISavingSummaryRepository
    {
        public SavingSummaryRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public int SetOpeningSavingEntry(int? orgID, int? officeID)
        {
            var orgIdParameter = new SqlParameter("@OrgID", orgID);
            var officeIdParameter = new SqlParameter("@OfficeId", officeID);

            return DataContext.Database.ExecuteSqlCommand("Proc_set_OpeningSaving @OrgID,@OfficeId", orgIdParameter,officeIdParameter);
           
        }
        public IEnumerable<DBSavingSummaryDetails> GetSavingSummaryDetail(int? officeID)
        {

            var obj = DataContext.SavingSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OfficeID == officeID && x.TransType == 0)
                .Select(s => new DBSavingSummaryDetails()
                {
                    SavingSummaryID = s.SavingSummaryID,
                    OfficeID = s.OfficeID,
                    OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                    CenterCode = s.Center.CenterCode,
                    MemberCode = s.Member.MemberCode,
                    MemberName = s.Member.FirstName,
                    ProductCode = s.Product.ProductCode,
                    NoOfAccount = s.NoOfAccount,
                    Deposit = s.Deposit,
                    Withdrawal = s.Withdrawal,
                    SavingInstallment = s.SavingInstallment,
                    InterestRate = s.InterestRate,
                    CumInterest = s.CumInterest,
                    MonthlyInterest = s.MonthlyInterest,
                    Penalty = s.Penalty,
                    OpeningDate = s.OpeningDate,
                    MaturedDate = s.MaturedDate,
                    ClosingDate = s.ClosingDate,
                    TransactionDate = s.TransactionDate

                }).OrderBy(x => x.CenterCode).ThenBy(x => x.MemberCode);

            return obj;
        }
        public IEnumerable<DBSavingSummaryDetails> GetSavingSummaryDetail(int? officeID, string filterColumnName, string filterValue)
        {
            IQueryable<SavingSummary> results = null;
            if (filterColumnName == "MemberCode")
                results = DataContext.SavingSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OfficeID == officeID && x.TransType == 0 && x.Member.MemberCode.Contains(filterValue));
            else if (filterColumnName == "CenterCode")
                results = DataContext.SavingSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OfficeID == officeID && x.TransType == 0 && x.Center.CenterCode.Contains(filterValue));
            else if (filterColumnName == "ProductCode")
                results = DataContext.SavingSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OfficeID == officeID && x.TransType == 0 && x.Product.ProductCode == filterValue);

            else

                results = DataContext.SavingSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OfficeID == officeID && x.TransType == 0);


            var obj = results.Select(s => new DBSavingSummaryDetails()
            {
                    SavingSummaryID = s.SavingSummaryID,
                    OfficeID = s.OfficeID,
                    OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                    CenterCode = s.Center.CenterCode,
                    MemberCode = s.Member.MemberCode,
                    MemberName = s.Member.FirstName,
                    ProductCode = s.Product.ProductCode,
                    NoOfAccount = s.NoOfAccount,
                    Deposit = s.Deposit,
                    Withdrawal = s.Withdrawal,
                    SavingInstallment = s.SavingInstallment,
                    InterestRate = s.InterestRate,
                    CumInterest = s.CumInterest,
                    MonthlyInterest = s.MonthlyInterest,
                    Penalty = s.Penalty,
                    OpeningDate = s.OpeningDate,
                    MaturedDate = s.MaturedDate,
                    ClosingDate = s.ClosingDate,
                    TransactionDate = s.TransactionDate

                }).OrderBy(x => x.CenterCode).ThenBy(x => x.MemberCode);

            return obj;
        }
        public int AccountClose(int? OrgID,int? officeID, int? centerID, long? memberID, int? productID, int? noAccount, DateTime? tranDate)
        {
            var OrgIDIdParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var centerParameter = new SqlParameter("@CenterID", centerID);
            var memberParameter = new SqlParameter("@MemberID", memberID);
            var productParameter = new SqlParameter("@ProductID", productID);
            var noAccountParameter = new SqlParameter("@NoAccount", noAccount);
            var dateParameter = new SqlParameter("@TranDate", tranDate);
            return DataContext.Database.ExecuteSqlCommand("AccountClose @OrgID,@OfficeID,@CenterID,@MemberID,@ProductID,@NoAccount,@TranDate", OrgIDIdParameter, officeIdParameter, centerParameter, memberParameter, productParameter, noAccountParameter, dateParameter);

        }
        public IEnumerable<DBSavingSummaryDetails> GetSavingSummaryDetailPaged(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount)
        {
            IQueryable<SavingSummary> results = null;
            if (filterColumnName == "MemberCode" && !string.IsNullOrWhiteSpace(filterValue))
                results = DataContext.SavingSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OfficeID == officeID && x.TransType == 0 && x.Member.MemberCode.Contains(filterValue));
            else if (filterColumnName == "CenterCode")
                results = DataContext.SavingSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OfficeID == officeID && x.TransType == 0 && x.Center.CenterCode.Contains(filterValue));
            else if (filterColumnName == "ProductCode")
                results = DataContext.SavingSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OfficeID == officeID && x.TransType == 0 && x.Product.ProductCode == filterValue);

            else

                results = DataContext.SavingSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == false && x.OfficeID == officeID && x.TransType == 0);

            totalCount = results.LongCount();
            var obj = results.OrderBy(x => x.Center.CenterCode).ThenBy(x => x.Member.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBSavingSummaryDetails()
            {
                SavingSummaryID = s.SavingSummaryID,
                OfficeID = s.OfficeID,
                OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                CenterCode = s.Center.CenterCode,
                MemberCode = s.Member.MemberCode,
                MemberName = s.Member.FirstName,
                ProductCode = s.Product.ProductCode,
                NoOfAccount = s.NoOfAccount,
                Deposit = s.Deposit,
                Withdrawal = s.Withdrawal,
                SavingInstallment = s.SavingInstallment,
                InterestRate = s.InterestRate,
                CumInterest = s.CumInterest,
                MonthlyInterest = s.MonthlyInterest,
                Penalty = s.Penalty,
                OpeningDate = s.OpeningDate,
                MaturedDate = s.MaturedDate,
                ClosingDate = s.ClosingDate,
                TransactionDate = s.TransactionDate

            });

            return obj;
        }
        public IEnumerable<DBSavingSummaryDetails> GetSavingSummaryDetailAccountClose(int? OrgID, int? officeID)
        {
            var obj = DataContext.SavingSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.OrgID==OrgID  && x.Posted == true && x.OfficeID == officeID && x.SavingStatus == 0 )
               .Select(s => new DBSavingSummaryDetails()
               {
                   SavingSummaryID = s.SavingSummaryID,
                   OfficeID = s.OfficeID,
                   OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                   CenterCode = s.Center.CenterCode,
                   MemberCode = s.Member.MemberCode,
                   MemberName = s.Member.FirstName,
                   ProductCode = s.Product.ProductCode,
                   NoOfAccount = s.NoOfAccount,
                   Deposit = s.Deposit,
                   Withdrawal = s.Withdrawal,
                   SavingInstallment = s.SavingInstallment,
                   InterestRate = s.InterestRate,
                   CumInterest = s.CumInterest,
                   MonthlyInterest = s.MonthlyInterest,
                   Penalty = s.Penalty,
                   OpeningDate = s.OpeningDate,
                   MaturedDate = s.MaturedDate,
                   ClosingDate = s.ClosingDate,
                   TransactionDate = s.TransactionDate

               }).OrderBy(x => x.CenterCode).ThenBy(x => x.MemberCode);

            return obj;
        }
        public IEnumerable<DBSavingSummaryDetails> GetSavingSummarySavingInterestUpdate(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount)
        {

            IQueryable<SavingSummary> results = null;
            if (filterColumnName == "MemberCode" && !string.IsNullOrWhiteSpace(filterValue))
                results = DataContext.SavingSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == true && x.OfficeID == officeID  && x.Member.MemberCode.Contains(filterValue));
            else if (filterColumnName == "CenterCode")
                results = DataContext.SavingSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == true && x.OfficeID == officeID  && x.Center.CenterCode.Contains(filterValue));
            else if (filterColumnName == "ProductCode")
                results = DataContext.SavingSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == true && x.OfficeID == officeID  && x.Product.ProductCode == filterValue);

            else

                results = DataContext.SavingSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == true && x.OfficeID == officeID );

            totalCount = results.LongCount();
            var obj = results.OrderBy(x => x.Center.CenterCode).ThenBy(x => x.Member.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBSavingSummaryDetails()
            {
                SavingSummaryID = s.SavingSummaryID,
                OfficeID = s.OfficeID,
                OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                CenterCode = s.Center.CenterCode,
                MemberCode = s.Member.MemberCode,
                MemberName = s.Member.FirstName,
                ProductCode = s.Product.ProductCode,
                NoOfAccount = s.NoOfAccount,
                Deposit = s.Deposit,
                Withdrawal = s.Withdrawal,
                SavingInstallment = s.SavingInstallment,
                InterestRate = s.InterestRate,
                CumInterest = s.CumInterest,
                MonthlyInterest = s.MonthlyInterest,
                Penalty = s.Penalty,
                OpeningDate = s.OpeningDate,
                MaturedDate = s.MaturedDate,
                ClosingDate = s.ClosingDate,
                TransactionDate = s.TransactionDate

            });

            return obj;
        }
        public IEnumerable<getSavingCloseAccountInfo_Result> GetSavingAccountCloseInfo(int? OrgID, int? officeID, DateTime? tranDate)
        {
            var OrgIDParameter = new SqlParameter("@OrgID", OrgID);
            var officeIDParameter = new SqlParameter("@OfficeID", officeID);
            var dateParameter = new SqlParameter("@TranDate", tranDate);
            return DataContext.Database.SqlQuery<getSavingCloseAccountInfo_Result>("getSavingCloseAccountInfo @OrgID,@officeId,@TranDate", OrgIDParameter, officeIDParameter,dateParameter);
        }
        public int MaxAccountNo(SavingSummary savingsummary)
        {

            //var obj = DataContext.SavingSummaries.Where(x => x.OrgID == savingsummary.OrgID && x.OfficeID == savingsummary.OfficeID && x.MemberID == savingsummary.MemberID && x.ProductID == savingsummary.ProductID && x.IsActive == true)
            var obj = DataContext.SavingSummaries.Where(x => x.OrgID == savingsummary.OrgID && x.OfficeID == savingsummary.OfficeID && x.ProductID == savingsummary.ProductID && x.IsActive == true)
             .Max(u => (int?)u.NoOfAccount) ?? 0;
            return Convert.ToInt16(obj);
        
        }
        public IEnumerable<Proc_get_MaxNoOfAccount_Result> Get_MaxNoOfAccount(int? officeID, int? orgID, int? centerID, long? memberID, int? productID)
        {
            var officeIDParameter = new SqlParameter("@OfficeID", officeID);
            var OrgIDParameter = new SqlParameter("@OrgID", orgID);
            var centerParameter = new SqlParameter("@CenterID", centerID);
            var memberParameter = new SqlParameter("@MemberID", memberID);
            var productParameter = new SqlParameter("@ProductID", productID);
            return DataContext.Database.SqlQuery<Proc_get_MaxNoOfAccount_Result>("Proc_get_MaxNoOfAccount @officeId,@OrgID,@CenterID,@MemberID,@ProductID", officeIDParameter, OrgIDParameter, centerParameter,memberParameter,productParameter);
 
        }
        public IEnumerable<Proc_GetSavingBalanceForCate_Result> GetSavingBalanceForCate(int? officeID, int? centerID, long? memberID, int? productID)
        {
            var officeIDParameter = new SqlParameter("@OfficeID", officeID);
           
            var centerParameter = new SqlParameter("@CenterID", centerID);
            var memberParameter = new SqlParameter("@MemberID", memberID);
            var productParameter = new SqlParameter("@ProductID", productID);
            return DataContext.Database.SqlQuery<Proc_GetSavingBalanceForCate_Result>("Proc_GetSavingBalanceForCate @officeId,@CenterID,@MemberID,@ProductID", officeIDParameter, centerParameter, memberParameter, productParameter);
 
        }



        public int updateSavingInstallment(int? office, long? memberID, int? prodID, int? orgID, int? CenterID, DateTime? TransactionDate, DateTime? OpeningDate, int? EmployeeId, int? MemberCategoryID, string CreateUser)
        {

            var officeIdParameter = new SqlParameter("@Office", office);
            var memberParameter = new SqlParameter("@memberID", memberID);
            var productParameter = new SqlParameter("@ProdID", prodID);
            var orgIdParameter = new SqlParameter("@OrgID", orgID);
            var CenterIDParameter = new SqlParameter("@CenterID", CenterID);
            var TransactionDateParameter = new SqlParameter("@TransactionDate", TransactionDate);
            var OpeningDateParameter = new SqlParameter("@OpeningDate", OpeningDate);
            var EmployeeIdParameter = new SqlParameter("@EmployeeId", EmployeeId);
            var MemberCategoryIDParameter = new SqlParameter("@MemberCategoryID", MemberCategoryID);
            var CreateUserParameter = new SqlParameter("@CreateUser", CreateUser);
            return DataContext.Database.ExecuteSqlCommand("updateSavingInstallment @Office,@memberID,@ProdID,@OrgID,@CenterID,@TransactionDate,@OpeningDate,@EmployeeId,@MemberCategoryID,@CreateUser", officeIdParameter, memberParameter, productParameter, orgIdParameter,CenterIDParameter,TransactionDateParameter,OpeningDateParameter,EmployeeIdParameter,MemberCategoryIDParameter,CreateUserParameter);
  
        }


        public int Proc_Set_SavingOpeingWhenMemberEligible(int? officeID, int? centerID, long? memberID, int? productID, int? noOfAccount, DateTime? transactionDate, decimal? interestRate, DateTime? openingDate, int? employeeId, int? memberCategoryID, int? orgID, string createUser, DateTime? createDate)
        {
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var CenterIDParameter = new SqlParameter("@CenterID", centerID);
            var memberParameter = new SqlParameter("@MemberID", memberID);
            var productParameter = new SqlParameter("@ProductID", productID);
            var NoOfAccountParameter = new SqlParameter("@NoOfAccount", noOfAccount);
          
            
            var TransactionDateParameter = new SqlParameter("@TransactionDate", transactionDate);
            var intrestRateParameter = new SqlParameter("@InterestRate", interestRate);


            var OpeningDateParameter = new SqlParameter("@OpeningDate", openingDate);
            var EmployeeIdParameter = new SqlParameter("@EmployeeId", employeeId);
            var MemberCategoryIDParameter = new SqlParameter("@MemberCategoryID", memberCategoryID);
            var orgIdParameter = new SqlParameter("@OrgID", orgID);

            var CreateUserParameter = new SqlParameter("@CreateUser", createUser);
            var CreatedateParameter = new SqlParameter("@CreateDate", createDate);

            return DataContext.Database.ExecuteSqlCommand("Proc_Set_SavingOpeingWhenMemberEligible @OfficeID,@CenterID,@MemberID,@ProductID,@NoOfAccount,@TransactionDate,@InterestRate,@OpeningDate,@EmployeeId,@MemberCategoryID,@OrgID,@CreateUser,@CreateDate", officeIdParameter, CenterIDParameter, memberParameter, productParameter, NoOfAccountParameter, TransactionDateParameter, intrestRateParameter, OpeningDateParameter, EmployeeIdParameter, MemberCategoryIDParameter, orgIdParameter, CreateUserParameter, CreatedateParameter);
  
        }

        public IEnumerable<getSavingCloseAccountInfo_Result> GetSavingStopInterestInfo(int? OrgID, int? officeID, DateTime? tranDate)
        {
            var OrgIDParameter = new SqlParameter("@OrgID", OrgID);
            var officeIDParameter = new SqlParameter("@OfficeID", officeID);
            var dateParameter = new SqlParameter("@TranDate", tranDate);
            return DataContext.Database.SqlQuery<getSavingCloseAccountInfo_Result>("getSavingStopInterestAccountInfo @OrgID,@officeId,@TranDate", OrgIDParameter, officeIDParameter, dateParameter);

        }

        public int StopInterestAccount(int? OrgID, int? officeID, int? centerID, long? memberID, int? productID, int? noAccount, DateTime? tranDate,int?  Qtype,long? SavingSummaryID,int? StopOrClaimable,string CreateUser)
        {
            var OrgIDIdParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var centerParameter = new SqlParameter("@CenterID", centerID);
            var memberParameter = new SqlParameter("@MemberID", memberID);
            var productParameter = new SqlParameter("@ProductID", productID);
            var noAccountParameter = new SqlParameter("@NoAccount", noAccount);
            var dateParameter = new SqlParameter("@TranDate", tranDate);
            var qTypeParameter = new SqlParameter("@Qtype", Qtype);
            var savingSummaryIDParameter = new SqlParameter("@SavingSummaryID", SavingSummaryID);
            var StopOrClaimableIDParameter = new SqlParameter("@StopOrClaimable", StopOrClaimable);
            var CreateUserParameter = new SqlParameter("@CreateUser", CreateUser);
            
            return DataContext.Database.ExecuteSqlCommand("setStopInterest @OrgID,@OfficeID,@CenterID,@MemberID,@ProductID,@NoAccount,@TranDate,@Qtype,@SavingSummaryID,@StopOrClaimable,@CreateUser", OrgIDIdParameter, officeIdParameter, centerParameter, memberParameter, productParameter, noAccountParameter, dateParameter,qTypeParameter,savingSummaryIDParameter,StopOrClaimableIDParameter,CreateUserParameter);

        }
        public int InsertConsentForm(int? OrgID, int? officeID, int? centerID, long? memberID, int? productID, int? noAccount, DateTime? tranDate, int? Qtype, long? SavingSummaryID, int? StopOrClaimable, string CreateUser)
        {
            var OrgIDIdParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var centerParameter = new SqlParameter("@CenterID", centerID);
            var memberParameter = new SqlParameter("@MemberID", memberID);
            var productParameter = new SqlParameter("@ProductID", productID);
            var noAccountParameter = new SqlParameter("@NoAccount", noAccount);
            var dateParameter = new SqlParameter("@TranDate", tranDate);
            var qTypeParameter = new SqlParameter("@Qtype", Qtype);
            var savingSummaryIDParameter = new SqlParameter("@SavingSummaryID", SavingSummaryID);
            var StopOrClaimableIDParameter = new SqlParameter("@StopOrClaimable", StopOrClaimable);
            var CreateUserParameter = new SqlParameter("@CreateUser", CreateUser);

            return DataContext.Database.ExecuteSqlCommand("InsertConsentForm @OrgID,@OfficeID,@CenterID,@MemberID,@ProductID,@NoAccount,@TranDate,@Qtype,@SavingSummaryID,@StopOrClaimable,@CreateUser", OrgIDIdParameter, officeIdParameter, centerParameter, memberParameter, productParameter, noAccountParameter, dateParameter, qTypeParameter, savingSummaryIDParameter, StopOrClaimableIDParameter, CreateUserParameter);

        }
        public int StartInterestAccount(int? OrgID, int? officeID, int? centerID, long? memberID, int? productID, int? noAccount, DateTime? tranDate, int? Qtype, long? SavingSummaryID,int? StopOrClaimable,string CreateUser)
        {
            var OrgIDIdParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var centerParameter = new SqlParameter("@CenterID", centerID);
            var memberParameter = new SqlParameter("@MemberID", memberID);
            var productParameter = new SqlParameter("@ProductID", productID);
            var noAccountParameter = new SqlParameter("@NoAccount", noAccount);
            var dateParameter = new SqlParameter("@TranDate", tranDate);
            var qTypeParameter = new SqlParameter("@Qtype", Qtype);
            var savingSummaryIDParameter = new SqlParameter("@SavingSummaryID", SavingSummaryID);
            var StopOrClaimableIDParameter = new SqlParameter("@StopOrClaimable", StopOrClaimable);
            var CreateUserParameter = new SqlParameter("@CreateUser", CreateUser);

            return DataContext.Database.ExecuteSqlCommand("setStopInterest @OrgID,@OfficeID,@CenterID,@MemberID,@ProductID,@NoAccount,@TranDate,@Qtype,@SavingSummaryID,@StopOrClaimable,@CreateUser", OrgIDIdParameter, officeIdParameter, centerParameter, memberParameter, productParameter, noAccountParameter, dateParameter, qTypeParameter, savingSummaryIDParameter, StopOrClaimableIDParameter, CreateUserParameter);

        }

        public IEnumerable<getSavingCloseAccountInfo_Result> GetSavingClaimableInterestInfo(int? OrgID, int? officeID, DateTime? tranDate)
        {
            var OrgIDParameter = new SqlParameter("@OrgID", OrgID);
            var officeIDParameter = new SqlParameter("@OfficeID", officeID);
            var dateParameter = new SqlParameter("@TranDate", tranDate);
            return DataContext.Database.SqlQuery<getSavingCloseAccountInfo_Result>("getSavingClaimableInterestAccountInfo @OrgID,@officeId,@TranDate", OrgIDParameter, officeIDParameter, dateParameter);

        }

        public int ClaimableInterestAccount(int? OrgID, int? officeID, int? centerID, long? memberID, int? productID, int? noAccount, DateTime? tranDate, int? Qtype, long? SavingSummaryID, int? StopOrClaimable, string CreateUser)
        {
            var OrgIDIdParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var centerParameter = new SqlParameter("@CenterID", centerID);
            var memberParameter = new SqlParameter("@MemberID", memberID);
            var productParameter = new SqlParameter("@ProductID", productID);
            var noAccountParameter = new SqlParameter("@NoAccount", noAccount);
            var dateParameter = new SqlParameter("@TranDate", tranDate);
            var qTypeParameter = new SqlParameter("@Qtype", Qtype);
            var savingSummaryIDParameter = new SqlParameter("@SavingSummaryID", SavingSummaryID);
            var StopOrClaimableIDParameter = new SqlParameter("@StopOrClaimable", StopOrClaimable);
            var CreateUserParameter = new SqlParameter("@CreateUser", CreateUser);

            return DataContext.Database.ExecuteSqlCommand("setClaimableInterest @OrgID,@OfficeID,@CenterID,@MemberID,@ProductID,@NoAccount,@TranDate,@Qtype,@SavingSummaryID,@StopOrClaimable,@CreateUser", OrgIDIdParameter, officeIdParameter, centerParameter, memberParameter, productParameter, noAccountParameter, dateParameter, qTypeParameter, savingSummaryIDParameter, StopOrClaimableIDParameter, CreateUserParameter);

        }

        public IEnumerable<DBSavingSummaryDetails> GetSavingReinstate(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount, DateTime DateFromValue, DateTime DateToValue)
        {
            IQueryable<SavingSummary> results = null;
            if (filterColumnName == "MemberCode" && !string.IsNullOrWhiteSpace(filterValue))
                results = DataContext.SavingSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == true && x.OfficeID == officeID && x.Member.MemberCode.Contains(filterValue) && x.ClosingDate>= DateFromValue && x.ClosingDate<=DateToValue && x.SavingStatus==0);
            else if (filterColumnName == "CenterCode")
                results = DataContext.SavingSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == true && x.OfficeID == officeID && x.Center.CenterCode.Contains(filterValue) && x.ClosingDate >= DateFromValue && x.ClosingDate <= DateToValue && x.SavingStatus == 0);
            else if (filterColumnName == "ProductCode")
                results = DataContext.SavingSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == true && x.OfficeID == officeID && x.Product.ProductCode == filterValue && x.ClosingDate >= DateFromValue && x.ClosingDate <= DateToValue && x.SavingStatus == 0);

            else

                results = DataContext.SavingSummaries.Where(x => (x.IsActive.HasValue ? x.IsActive.Value : true) && x.Posted == true && x.OfficeID == officeID && x.ClosingDate >= DateFromValue && x.ClosingDate <= DateToValue && x.SavingStatus == 0);

            totalCount = results.LongCount();
            var obj = results.OrderBy(x => x.Center.CenterCode).ThenBy(x => x.Member.MemberCode).Skip(startRowIndex).Take(pageSize).Select(s => new DBSavingSummaryDetails()
            {
                SavingSummaryID = s.SavingSummaryID,
                OfficeID = s.OfficeID,
                OfficeCode = s.Office == null ? "" : s.Office.OfficeCode,
                CenterCode = s.Center.CenterCode,
                MemberCode = s.Member.MemberCode,
                MemberName = s.Member.FirstName,
                ProductCode = s.Product.ProductCode,
                NoOfAccount = s.NoOfAccount,
                Deposit = s.Deposit,
                Withdrawal = s.Withdrawal,
                SavingInstallment = s.SavingInstallment,
                InterestRate = s.InterestRate,
                CumInterest = s.CumInterest,
                MonthlyInterest = s.MonthlyInterest,
                Penalty = s.Penalty,
                OpeningDate = s.OpeningDate,
                MaturedDate = s.MaturedDate,
                ClosingDate = s.ClosingDate,
                TransactionDate = s.TransactionDate

            });

            return obj;
        }
    }
}
