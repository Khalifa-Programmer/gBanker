using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class Inv_RequisitionConsulateDetailsRepository : RepositoryBaseCodeFirst<Inv_RequisitionConsulateDetails>, IInv_RequisitionConsulateDetailsRepository
    {
        public Inv_RequisitionConsulateDetailsRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IInv_RequisitionConsulateDetailsRepository : IRepository<Inv_RequisitionConsulateDetails>
    {
    }
}



