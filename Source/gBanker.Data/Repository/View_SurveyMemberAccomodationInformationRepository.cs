using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class View_SurveyMemberAccomodationInformationRepository : RepositoryBaseCodeFirst<View_SurveyMemberAccomodationInformation>, IView_SurveyMemberAccomodationInformationRepository
    {
        public View_SurveyMemberAccomodationInformationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IView_SurveyMemberAccomodationInformationRepository : IRepository<View_SurveyMemberAccomodationInformation>
    {
    }
}



