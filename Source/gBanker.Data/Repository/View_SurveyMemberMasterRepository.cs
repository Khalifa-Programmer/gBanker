using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class View_SurveyMemberMasterRepository : RepositoryBaseCodeFirst<View_SurveyMemberMaster>, IView_SurveyMemberMasterRepository
    {
        public View_SurveyMemberMasterRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IView_SurveyMemberMasterRepository : IRepository<View_SurveyMemberMaster>
    {
    }
}


