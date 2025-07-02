using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class SurveyMemberExpenditureRepository : RepositoryBaseCodeFirst<SurveyMemberExpenditure>, ISurveyMemberExpenditureRepository
    {
        public SurveyMemberExpenditureRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface ISurveyMemberExpenditureRepository : IRepository<SurveyMemberExpenditure>
    {
    }
}



