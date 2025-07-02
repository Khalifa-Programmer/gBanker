using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IAssetRevaluationRepository : IRepository<AssetRevaluation>
    {
    }
    public class AssetRevaluationRepository : RepositoryBaseCodeFirst<AssetRevaluation>, IAssetRevaluationRepository
    {
        public AssetRevaluationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
