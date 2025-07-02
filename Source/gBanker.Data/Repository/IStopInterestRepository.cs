using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IStopInterestRepository : IRepository<StopInterest>
    {
    }
    public class StopInterestRepository : RepositoryBaseCodeFirst<StopInterest>, IStopInterestRepository
    {
        public StopInterestRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
