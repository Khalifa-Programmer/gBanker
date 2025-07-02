using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IMemberLandInfoRepository : IRepository<MemberLandInfo>
    {
    }
    public class MemberLandInfoRepository : RepositoryBaseCodeFirst<MemberLandInfo>, IMemberLandInfoRepository
    {
        public MemberLandInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
