using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class SurveyKnownMemberRepository : RepositoryBaseCodeFirst<SurveyKnownMember>, ISurveyKnownMemberRepository
    {
        public SurveyKnownMemberRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface ISurveyKnownMemberRepository : IRepository<SurveyKnownMember>
    {
    }
}



