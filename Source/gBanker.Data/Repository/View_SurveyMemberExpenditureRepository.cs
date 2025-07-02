using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class View_SurveyMemberExpenditureRepository : RepositoryBaseCodeFirst<View_SurveyMemberExpenditure>, IView_SurveyMemberExpenditureRepository
    {
        public View_SurveyMemberExpenditureRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IView_SurveyMemberExpenditureRepository : IRepository<View_SurveyMemberExpenditure>
    {
    }
}





