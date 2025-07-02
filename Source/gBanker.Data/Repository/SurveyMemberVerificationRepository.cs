using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class SurveyMemberVerificationRepository : RepositoryBaseCodeFirst<SurveyMemberVerification>, ISurveyMemberVerificationRepository
    {
        public SurveyMemberVerificationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface ISurveyMemberVerificationRepository : IRepository<SurveyMemberVerification>
    {
    }
}


