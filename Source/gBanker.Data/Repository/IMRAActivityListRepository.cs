using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IMRAActivityListRepository : IRepository<MRAActivityList>
    {
    }
    public class MRAActivityListRepository : RepositoryBaseCodeFirst<MRAActivityList>, IMRAActivityListRepository
    {
        public MRAActivityListRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
