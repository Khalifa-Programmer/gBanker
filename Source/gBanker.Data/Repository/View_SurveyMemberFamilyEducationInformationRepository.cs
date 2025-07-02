using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class View_SurveyMemberFamilyEducationInformationRepository : RepositoryBaseCodeFirst<View_SurveyMemberFamilyEducationInformation>, IView_SurveyMemberFamilyEducationInformationRepository
    {
        public View_SurveyMemberFamilyEducationInformationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IView_SurveyMemberFamilyEducationInformationRepository : IRepository<View_SurveyMemberFamilyEducationInformation>
    {
    }
}



