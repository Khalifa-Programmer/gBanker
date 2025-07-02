using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class SurveyMemberFamilyInformationRepository : RepositoryBaseCodeFirst<SurveyMemberFamilyInformation>, ISurveyMemberFamilyInformationRepository
    {
        public SurveyMemberFamilyInformationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface ISurveyMemberFamilyInformationRepository : IRepository<SurveyMemberFamilyInformation>
    {
    }
}


