using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IRemittanceActivityRepository : IRepository<RemittanceActivity>
    {
    }
    public class RemittanceActivityRepository : RepositoryBaseCodeFirst<RemittanceActivity>, IRemittanceActivityRepository
    {
        public RemittanceActivityRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
