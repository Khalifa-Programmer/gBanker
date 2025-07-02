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
    public interface IAccountCloseRepository : IRepository<SavingSummary>
    {


        IEnumerable<get_lastDayendDate_Result> getLastDayEndDate(Nullable<int> officeID);
        int AccountClose(Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID, Nullable<int> noAccount, Nullable<System.DateTime> tranDate);
    }
    public class AccountCloseRepository : RepositoryBaseCodeFirst<SavingSummary>, IAccountCloseRepository
    {
       public AccountCloseRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

       public int AccountClose(int? officeID, int? centerID, long? memberID, int? productID, int? noAccount, DateTime? tranDate)
       {
           var officeIdParameter = new SqlParameter("@OfficeID", officeID);
           var centerParameter = new SqlParameter("@CenterID", centerID);
           var memberParameter = new SqlParameter("@MemberID", memberID);
           var productParameter = new SqlParameter("@ProductID", productID);
           var noAccountParameter = new SqlParameter("@NoAccount", noAccount);
           var dateParameter = new SqlParameter("@TranDate", tranDate);
           return DataContext.Database.ExecuteSqlCommand("AccountClose @OfficeID,@CenterID,@MemberID,@ProductID,@NoAccount,@TranDate", officeIdParameter, centerParameter, memberParameter, productParameter, noAccountParameter,dateParameter);
       }

       public IEnumerable<get_lastDayendDate_Result> getLastDayEndDate(int? officeID)
       {
           var officeIdParameter = new SqlParameter("@OfficeID", officeID);
           return DataContext.Database.SqlQuery<get_lastDayendDate_Result>("get_LastDayEndDate @OfficeID", officeIdParameter);


       }
    }
}
