using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class MemberSurveyRepository : RepositoryBaseCodeFirst<SurveyMemberMaster>, ISurveyMemberMasterRepository
    {
        public MemberSurveyRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface ISurveyMemberMasterRepository : IRepository<SurveyMemberMaster>
    {
    }
}
