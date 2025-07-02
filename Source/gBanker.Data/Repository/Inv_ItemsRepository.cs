using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class Inv_ItemsRepository : RepositoryBaseCodeFirst<Inv_Items>, IInv_ItemsRepository
    {
        public Inv_ItemsRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IInv_ItemsRepository : IRepository<Inv_Items>
    {
    }
}



