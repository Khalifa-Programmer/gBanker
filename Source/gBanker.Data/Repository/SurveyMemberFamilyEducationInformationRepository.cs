using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class SurveyMemberFamilyEducationInformationRepository : RepositoryBaseCodeFirst<SurveyMemberFamilyEducationInformation>, ISurveyMemberFamilyEducationInformationRepository
    {
        public SurveyMemberFamilyEducationInformationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface ISurveyMemberFamilyEducationInformationRepository : IRepository<SurveyMemberFamilyEducationInformation>
    {
    }
}




