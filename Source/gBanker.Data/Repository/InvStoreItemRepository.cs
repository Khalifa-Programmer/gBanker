using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class InvWarehouseRepository : RepositoryBaseCodeFirst<InvWarehouse>, IInvWarehouseRepository
    {
        public InvWarehouseRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface IInvWarehouseRepository : IRepository<InvWarehouse>
    {
    }
}



