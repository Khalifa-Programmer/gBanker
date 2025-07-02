using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class View_SurveyMemberAssetRepository : RepositoryBaseCodeFirst<View_SurveyMemberAsset>, IView_SurveyMemberAssetRepository
    {
        public View_SurveyMemberAssetRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IView_SurveyMemberAssetRepository : IRepository<View_SurveyMemberAsset>
    {
    }
}




