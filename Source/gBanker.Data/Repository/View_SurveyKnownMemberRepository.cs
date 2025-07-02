using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class View_SurveyKnownMemberRepository : RepositoryBaseCodeFirst<View_SurveyKnownMember>, IView_SurveyKnownMemberRepository
    {
        public View_SurveyKnownMemberRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IView_SurveyKnownMemberRepository : IRepository<View_SurveyKnownMember>
    {
    }
}



