using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IPrimaryRegistrationRepository : IRepository<PrimaryRegistration>
    {
    }
    public class PrimaryRegistrationRepository : RepositoryBaseCodeFirst<PrimaryRegistration>, IPrimaryRegistrationRepository
    {
        public PrimaryRegistrationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
