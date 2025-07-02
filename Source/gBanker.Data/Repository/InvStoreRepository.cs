using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class InvStoreRepository : RepositoryBaseCodeFirst<Inv_Store>, IInvStoreRepository
    {
        public InvStoreRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface IInvStoreRepository : IRepository<Inv_Store>
    {
    }
}



