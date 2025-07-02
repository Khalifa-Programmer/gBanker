using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class Inv_TempStoreRepository : RepositoryBaseCodeFirst<Inv_TempStore>, IInv_TempStoreRepository
    {
        public Inv_TempStoreRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IInv_TempStoreRepository : IRepository<Inv_TempStore>
    {
    }
}



