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
    public interface ISavingCollectionRepository : IRepository<DailySavingTrx>
    {
        IEnumerable<getDailySavingProduct_Result> getDailySavingProduct(Nullable<int> officeId, Nullable<int> orgId);

        // IEnumerable<DailyLoanTrx> GetDailyLoanCollectionByCenter(int centerId);
        int delVoucher(Nullable<int> officeID, Nullable<System.DateTime> businessDate, Nullable<int> OrgID);
      

    }

    public class SavingCollectionRepository: RepositoryBaseCodeFirst<DailySavingTrx>, ISavingCollectionRepository
    {
        public SavingCollectionRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public int delVoucher(int? officeID, DateTime? businessDate, int? OrgID)
        {
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var date = new SqlParameter("@BusinessDate", businessDate);
            var orgIdParameter = new SqlParameter("@OrgID", OrgID);
            return DataContext.Database.ExecuteSqlCommand("delVoucher @OfficeID,@BusinessDate,@OrgID", officeIdParameter, date, orgIdParameter);
        }


        public IEnumerable<getDailySavingProduct_Result> getDailySavingProduct(int? officeId, int? orgId)
        {
            var orgIdParameter = new SqlParameter("@OrgId", orgId);
            var officeIdParameter = new SqlParameter("@OfficeId", officeId);

            return DataContext.Database.SqlQuery<getDailySavingProduct_Result>("getDailySavingProduct @OfficeId,@OrgId", officeIdParameter, orgIdParameter);
  
        }
    }
}
