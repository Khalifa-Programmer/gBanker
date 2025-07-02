using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class InstituteTypeRepository : RepositoryBaseCodeFirst<InstituteType>, IInstituteTypeRepository
    {
        public InstituteTypeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IInstituteTypeRepository : IRepository<InstituteType>
    {
    }
}



