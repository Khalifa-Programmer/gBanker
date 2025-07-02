using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;


namespace gBanker.Data.Repository
{
    public interface ISmsConfigRepository : IRepository<SmsConfig>
    {
    }
    public class SmsConfigRepository : RepositoryBaseCodeFirst<SmsConfig>, ISmsConfigRepository
    {
        public SmsConfigRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
