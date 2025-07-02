using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class View_SurveyMemberFamilyInformationRepository : RepositoryBaseCodeFirst<View_SurveyMemberFamilyInformation>, IView_SurveyMemberFamilyInformationRepository
    {
        public View_SurveyMemberFamilyInformationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IView_SurveyMemberFamilyInformationRepository : IRepository<View_SurveyMemberFamilyInformation>
    {
    }
}


