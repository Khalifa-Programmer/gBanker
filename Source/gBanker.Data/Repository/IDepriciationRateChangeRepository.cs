using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IDepriciationRateChangeRepository : IRepository<DepriciationRateChange>
    {
    }
    public class DepriciationRateChangeRepository : RepositoryBaseCodeFirst<DepriciationRateChange>, IDepriciationRateChangeRepository
    {
        public DepriciationRateChangeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
