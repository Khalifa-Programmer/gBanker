using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace gBanker.Data.Repository
{
    public interface IMobileSyncLogRepository : IRepository<MobileSyncLog>
    {

    }
    public class MobileSyncLogRepository : RepositoryBaseCodeFirst<MobileSyncLog>, IMobileSyncLogRepository
    {
        public MobileSyncLogRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
