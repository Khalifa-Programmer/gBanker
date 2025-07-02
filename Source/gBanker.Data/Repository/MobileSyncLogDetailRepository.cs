using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace gBanker.Data.Repository
{
    public interface IMobileSyncLogDetailRepository : IRepository<MobileSyncLogDetail>
    {

    }
    public class MobileSyncLogDetailRepository : RepositoryBaseCodeFirst<MobileSyncLogDetail>, IMobileSyncLogDetailRepository
    {
        public MobileSyncLogDetailRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
