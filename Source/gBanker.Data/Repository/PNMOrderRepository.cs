using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IPNMOrderRepository : IRepository<PNMOrder>
    {
    }
    public class PNMOrderRepository : RepositoryBaseCodeFirst<PNMOrder>, IPNMOrderRepository
    {
        public PNMOrderRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
