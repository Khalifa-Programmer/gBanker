using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.CodeFirstMigration;
using System.Data.SqlClient;

namespace gBanker.Data.Repository
{
    public interface IWriteOffHistoryRepository : IRepository<WriteOffHistory>
    {
        int SetOpeningSavingEntry(int? orgID, int? officeID);
    }
    public class WriteOffHistoryRepository : RepositoryBaseCodeFirst<WriteOffHistory>, IWriteOffHistoryRepository
    {
        public WriteOffHistoryRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public int SetOpeningSavingEntry(int? orgID, int? officeID)
        {
            var orgIdParameter = new SqlParameter("@OrgID", orgID);
            var officeIdParameter = new SqlParameter("@OfficeId", officeID);

            return DataContext.Database.ExecuteSqlCommand("WriteOffHistoryInsertToLedger @OrgID,@OfficeId", orgIdParameter, officeIdParameter);

        }
    }
}
