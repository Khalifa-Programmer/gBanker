using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IStatisticsReportRepository : IRepository<StatisticsReport>
    {
    }
    public class StatisticsReportRepository : RepositoryBaseCodeFirst<StatisticsReport>, IStatisticsReportRepository
    {
        public StatisticsReportRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
