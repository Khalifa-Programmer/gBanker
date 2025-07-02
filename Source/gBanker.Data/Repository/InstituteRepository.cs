using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class InstituteRepository : RepositoryBaseCodeFirst<Institute>, IInstituteRepository
    {
        public InstituteRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IInstituteRepository : IRepository<Institute>
    {
    }
}




