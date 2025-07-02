using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IInterbranchTransactionTrackingRepository : IRepository<InterbranchTransactionTracking>
    {
    }
    public class InterbranchTransactionTrackingRepository : RepositoryBaseCodeFirst<InterbranchTransactionTracking>, IInterbranchTransactionTrackingRepository
    {
        public InterbranchTransactionTrackingRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
