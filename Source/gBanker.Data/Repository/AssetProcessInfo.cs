using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IAssetProcessInfoRepository : IRepository<AssetProcessInfo>
    {

    }
    public class AssetProcessInfoRepository : RepositoryBaseCodeFirst<AssetProcessInfo>, IAssetProcessInfoRepository
    {
        public AssetProcessInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
