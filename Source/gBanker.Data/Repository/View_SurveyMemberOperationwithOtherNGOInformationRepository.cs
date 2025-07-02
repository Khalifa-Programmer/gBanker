using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class View_SurveyMemberOperationwithOtherNGOInformationRepository : RepositoryBaseCodeFirst<View_SurveyMemberOperationwithOtherNGOInformation>, IView_SurveyMemberOperationwithOtherNGOInformationRepository
    {
        public View_SurveyMemberOperationwithOtherNGOInformationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IView_SurveyMemberOperationwithOtherNGOInformationRepository : IRepository<View_SurveyMemberOperationwithOtherNGOInformation>
    {
    }
}




