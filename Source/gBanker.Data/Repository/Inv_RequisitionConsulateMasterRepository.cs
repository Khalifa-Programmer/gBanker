using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class Inv_RequisitionConsulateMasterRepository : RepositoryBaseCodeFirst<Inv_RequisitionConsulateMaster>, IInv_RequisitionConsulateMasterRepository
    {
        public Inv_RequisitionConsulateMasterRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IInv_RequisitionConsulateMasterRepository : IRepository<Inv_RequisitionConsulateMaster>
    {
    }
}



