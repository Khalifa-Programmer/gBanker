using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class SurveyMemberAssetRepository : RepositoryBaseCodeFirst<SurveyMemberAsset>, ISurveyMemberAssetRepository
    {
        public SurveyMemberAssetRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface ISurveyMemberAssetRepository : IRepository<SurveyMemberAsset>
    {
    }
}



