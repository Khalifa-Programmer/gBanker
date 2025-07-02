using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
namespace gBanker.Data.Repository
{
    public interface ICountryRepository : IRepository<Country>
    {
    }
    public class CountryRepository : RepositoryBaseCodeFirst<Country>, ICountryRepository
    {
        public CountryRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
