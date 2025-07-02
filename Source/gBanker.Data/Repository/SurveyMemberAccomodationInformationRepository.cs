using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class SurveyMemberAccomodationInformationRepository : RepositoryBaseCodeFirst<SurveyMemberAccomodationInformation>, ISurveyMemberAccomodationInformationRepository
    {
        public SurveyMemberAccomodationInformationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface ISurveyMemberAccomodationInformationRepository : IRepository<SurveyMemberAccomodationInformation>
    {
    }
}



