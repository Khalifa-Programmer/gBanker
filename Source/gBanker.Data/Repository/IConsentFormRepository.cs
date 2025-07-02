using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IConsentFormRepository : IRepository<ConsentForm>
    {

    }
    public class ConsentFormRepository : RepositoryBaseCodeFirst<ConsentForm>, IConsentFormRepository
    {
        public ConsentFormRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
