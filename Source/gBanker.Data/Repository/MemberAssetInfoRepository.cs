using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
namespace gBanker.Data.Repository
{
    public interface IMemberAssetInfoRepository : IRepository<MemberAssetInfo>
    {
    }
    public class MemberAssetInfoRepository : RepositoryBaseCodeFirst<MemberAssetInfo>, IMemberAssetInfoRepository
    {
        public MemberAssetInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
