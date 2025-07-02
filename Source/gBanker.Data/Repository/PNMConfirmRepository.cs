using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IPNMConfirmRepository : IRepository<PNMConfirm>
    {
    }
    public class PNMConfirmRepository : RepositoryBaseCodeFirst<PNMConfirm>, IPNMConfirmRepository
    {
        public PNMConfirmRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
