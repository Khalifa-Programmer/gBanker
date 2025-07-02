using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IAccCategoryRepository : IRepository<AccCategory>
    {
    }
    public class AccCategoryRepository : RepositoryBaseCodeFirst<AccCategory>, IAccCategoryRepository
    {
        public AccCategoryRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
