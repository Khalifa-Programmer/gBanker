using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IMemberBlackListRepository : IRepository<MemberBlackList>
    {
    }
    public class MemberBlackListRepository : RepositoryBaseCodeFirst<MemberBlackList>, IMemberBlackListRepository
    {
        public MemberBlackListRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
