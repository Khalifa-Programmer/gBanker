using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class SurveyMemberOperationwithOtherNGOInformationRepository : RepositoryBaseCodeFirst<SurveyMemberOperationwithOtherNGOInformation>, ISurveyMemberOperationwithOtherNGOInformationRepository
    {
        public SurveyMemberOperationwithOtherNGOInformationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface ISurveyMemberOperationwithOtherNGOInformationRepository : IRepository<SurveyMemberOperationwithOtherNGOInformation>
    {
    }
}



