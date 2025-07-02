using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class Inv_RequsitionDetailsRepository : RepositoryBaseCodeFirst<Inv_RequsitionDetails>, IInv_RequsitionDetailsRepository
    {
        public Inv_RequsitionDetailsRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IInv_RequsitionDetailsRepository : IRepository<Inv_RequsitionDetails>
    {
    }
}



