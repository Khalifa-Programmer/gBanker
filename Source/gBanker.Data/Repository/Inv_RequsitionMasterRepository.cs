using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class Inv_RequsitionMasterRepository : RepositoryBaseCodeFirst<Inv_RequsitionMaster>, IInv_RequsitionMasterRepository
    {
        public Inv_RequsitionMasterRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IInv_RequsitionMasterRepository : IRepository<Inv_RequsitionMaster>
    {
    }
}



