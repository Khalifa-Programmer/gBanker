using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IParticularRepository : IRepository<Particular>
    {
    }
    public class ParticularRepository : RepositoryBaseCodeFirst<Particular>, IParticularRepository
    {
        public ParticularRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
