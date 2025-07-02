using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class Inv_ItemPriceDetailsRepository : RepositoryBaseCodeFirst<Inv_ItemPriceDetails>, IInv_ItemPriceDetailsRepository
    {
        public Inv_ItemPriceDetailsRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IInv_ItemPriceDetailsRepository : IRepository<Inv_ItemPriceDetails>
    {
    }
}



