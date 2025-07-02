using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IAssetValuerRepository : IRepository<AssetValuer>
    {
    }
    public class AssetValuerRepository : RepositoryBaseCodeFirst<AssetValuer>, IAssetValuerRepository
    {
        public AssetValuerRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
