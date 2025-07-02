using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
   

    public interface IAspNetRoleRepository : IRepository<AspNetRole>
    {
    }
    public class AspNetRoleRepository : RepositoryBaseCodeFirst<AspNetRole>, IAspNetRoleRepository
    {
        public AspNetRoleRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
