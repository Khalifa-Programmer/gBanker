using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class InvStoreItemRepository : RepositoryBaseCodeFirst<InvStoreItem>, IInvStoreItemRepository
    {
        public InvStoreItemRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface IInvStoreItemRepository : IRepository<InvStoreItem>
    {
    }
}



