using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IMemberFamilyInfoRepository : IRepository<MemberFamilyInfo>
    {
    }
    public class MemberFamilyInfoRepository : RepositoryBaseCodeFirst<MemberFamilyInfo>, IMemberFamilyInfoRepository
    {
        public MemberFamilyInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
