using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface ILoanTrxRepository : IRepository<LoanTrx>
    {
        IEnumerable<getTodaysTransaction_Result> GetHistory(int? OrgID, int? officeID, DateTime? tranDateFrom, DateTime? tranDateTo, int? centerIdFrom, long? memberID, string productType, string TransType);

        IEnumerable<getTodaysTransaction_Result> GetHistory(int? OrgID, int? officeID, DateTime? tranDateFrom, DateTime? tranDateTo, int? centerIdFrom, long? memberID, string productType);
        IEnumerable<getWriteOffList_Result> GetwriteOfList(int? OrgID, int? officeID, Nullable<long> memberID, Nullable<System.DateTime> trandate, int? writeoffyear);
        IEnumerable<getWriteOffList_Result> getWriteOffDeclarationList(int? OrgID, int? officeID, Nullable<long> memberID, Nullable<System.DateTime> trandate);
        IEnumerable<Proc_GetZeroBalance_Result> GeZeroBalanceList(int? officeID, int? officeIDTo, Nullable<long> memberID, int? loanStatusType );
        IEnumerable<LoanTrx> GetLoanTrx(int CenterID, long MemberID, int ProductID, int LoanTerm, string Option);
    }
    public class LoanTrxRepository : RepositoryBaseCodeFirst<LoanTrx>, ILoanTrxRepository
    {
        public LoanTrxRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<getTodaysTransaction_Result> GetHistory(int? OrgID,int? officeID, DateTime? tranDateFrom, DateTime? tranDateTo, int? centerIdFrom, long? memberID, string productType)
        {
            var orgIdParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@Office", officeID);
            var dateFrom = new SqlParameter("@TranDateFrom", tranDateFrom);
            var dateTo = new SqlParameter("@TranDateTo", tranDateTo);
            var VarcenterIdFrom = new SqlParameter("@centerIdFrom", centerIdFrom);
            var MemberID = new SqlParameter("@MemberID", memberID);
            var ProductType = new SqlParameter("@ProductType", productType);

            return DataContext.Database.SqlQuery<getTodaysTransaction_Result>("getTodaysTransaction_New @OrgID,@Office,@TranDateFrom,@TranDateTo,@centerIdFrom,@MemberID,@ProductType", orgIdParameter,officeIdParameter, dateFrom, dateTo, VarcenterIdFrom, MemberID, ProductType);

        }


        public IEnumerable<getWriteOffList_Result> GetwriteOfList(int? OrgID, int? officeID, long? memberID, DateTime? trandate, int? writeoffyear)
        {
            var orgIDParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var memberCode = new SqlParameter("@MemberID", memberID);
            var dateTo = new SqlParameter("@Trandate", trandate);
            var vwriteoffyear = new SqlParameter("@writeoffyear", writeoffyear);
            return DataContext.Database.SqlQuery<getWriteOffList_Result>("getWriteOffList @OrgID,@OfficeID,@MemberID,@Trandate,@writeoffyear", orgIDParameter, officeIdParameter, memberCode, dateTo,vwriteoffyear);

        }


        public IEnumerable<getWriteOffList_Result> getWriteOffDeclarationList(int? OrgID, int? officeID, long? memberID, DateTime? trandate)
        {
            var OrgIdParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var memberCode = new SqlParameter("@MemberID", memberID);
            var dateTo = new SqlParameter("@Trandate", trandate);
            return DataContext.Database.SqlQuery<getWriteOffList_Result>("getWriteOffDeclarationList @OrgID,@OfficeID,@MemberID,@Trandate", OrgIdParameter,officeIdParameter, memberCode, dateTo);

        }


        public IEnumerable<Proc_GetZeroBalance_Result> GeZeroBalanceList(int? officeID, int? officeIDTo, long? memberID, int? LoanStatusType)
        {
            var officeIDParameter = new SqlParameter("@OfficeId", officeID);
            var officeIDToParameter = new SqlParameter("@OfficeIdTo", officeIDTo);
            var memberIDParameter = new SqlParameter("@MemberID", memberID);
            var LoanStatusTypeParameter = new SqlParameter("@LoanStatusType", LoanStatusType);

            return DataContext.Database.SqlQuery<Proc_GetZeroBalance_Result>("Proc_GetZeroBalance_New @OfficeId,@OfficeIdTo,@MemberID, @LoanStatusType", officeIDParameter, officeIDToParameter, memberIDParameter, LoanStatusTypeParameter);

        }

        public IEnumerable<LoanTrx> GetLoanTrx(int CenterID, long MemberID, int ProductID, int LoanTerm, string Option)
        {
            var centerIDParameter = new SqlParameter("@CenterID", CenterID);
            var memberIDParameter = new SqlParameter("@MemberID", MemberID);
            var productIDParameter = new SqlParameter("@ProductID", ProductID);
            var loanTermParameter = new SqlParameter("@LoanTerm", LoanTerm);
            var optionParameter = new SqlParameter("@Option", Option);
            return DataContext.Database.SqlQuery<LoanTrx>("Proc_GetLoanTrx  @CenterID, @MemberID, @ProductID, @LoanTerm, @Option", centerIDParameter, memberIDParameter, productIDParameter, loanTermParameter, optionParameter);
        }

        public IEnumerable<getTodaysTransaction_Result> GetHistory(int? OrgID, int? officeID, DateTime? tranDateFrom, DateTime? tranDateTo, int? centerIdFrom, long? memberID, string productType, string TransType)
        {
            var orgIdParameter = new SqlParameter("@OrgID", OrgID);
            var officeIdParameter = new SqlParameter("@Office", officeID);
            var dateFrom = new SqlParameter("@TranDateFrom", tranDateFrom);
            var dateTo = new SqlParameter("@TranDateTo", tranDateTo);
            var VarcenterIdFrom = new SqlParameter("@centerIdFrom", centerIdFrom);
            var MemberID = new SqlParameter("@MemberID", memberID);
            var ProductType = new SqlParameter("@ProductType", productType);
            var transType = new SqlParameter("@TransType", TransType);

            return DataContext.Database.SqlQuery<getTodaysTransaction_Result>("getTodaysTransaction_New @OrgID,@Office,@TranDateFrom,@TranDateTo,@centerIdFrom,@MemberID,@ProductType,@TransType", orgIdParameter, officeIdParameter, dateFrom, dateTo, VarcenterIdFrom, MemberID, ProductType, transType);

        }
    }
}
