using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface ISmsLogRepository : IRepository<SmsLog>
    {
    }
    public class SmsLogRepository : RepositoryBaseCodeFirst<SmsLog>, ISmsLogRepository
    {
        public SmsLogRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
