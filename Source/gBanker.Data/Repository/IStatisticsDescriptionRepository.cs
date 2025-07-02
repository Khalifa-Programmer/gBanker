using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IStatisticsDescriptionRepository : IRepository<StatisticsDescription>
    {
    }
    public class StatisticsDescriptionRepository : RepositoryBaseCodeFirst<StatisticsDescription>, IStatisticsDescriptionRepository
    {
        public StatisticsDescriptionRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
