using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IAreaRepository : IRepository<Area>
    {
    }
    public class AreaRepository : RepositoryBaseCodeFirst<Area>, IAreaRepository
    {
        public AreaRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
