using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IStatisticsReportDetailsRepository : IRepository<StatisticsReportDetails>
    {
    }
    public class StatisticsReportDetailsRepository : RepositoryBaseCodeFirst<StatisticsReportDetails>, IStatisticsReportDetailsRepository
    {
        public StatisticsReportDetailsRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}

