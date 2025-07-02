using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
namespace gBanker.Data.Repository
{
    public interface ILgVillageRepository : IRepository<LgVillage>
    {
    }
    public class LgVillageRepository : RepositoryBaseCodeFirst<LgVillage>, ILgVillageRepository
    {
        public LgVillageRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
