using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IAccReconcileRepository : IRepository<AccReconcile>
    {
    }
    public class AccReconcileRepository : RepositoryBaseCodeFirst<AccReconcile>, IAccReconcileRepository
    {
        public AccReconcileRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
