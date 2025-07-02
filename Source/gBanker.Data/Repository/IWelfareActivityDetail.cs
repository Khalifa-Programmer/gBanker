using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IWelfareActivityDetailRepository : IRepository<WelfareActivityDetail>
    {
    }
    public class WelfareActivityDetailRepository : RepositoryBaseCodeFirst<WelfareActivityDetail>, IWelfareActivityDetailRepository
    {
        public WelfareActivityDetailRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
