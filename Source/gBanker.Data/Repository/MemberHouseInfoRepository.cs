using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IMemberHouseInfoRepository : IRepository<MemberHouseInfo>
    {
    }
    public class MemberHouseInfoRepository : RepositoryBaseCodeFirst<MemberHouseInfo>, IMemberHouseInfoRepository
    {
        public MemberHouseInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
