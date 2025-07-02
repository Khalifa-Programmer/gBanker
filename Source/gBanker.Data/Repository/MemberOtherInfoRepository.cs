using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IMemberOtherInfoRepository : IRepository<MemberOtherInfo>
    {
    }
    public class MemberOtherInfoRepository : RepositoryBaseCodeFirst<MemberOtherInfo>, IMemberOtherInfoRepository
    {
        public MemberOtherInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}

