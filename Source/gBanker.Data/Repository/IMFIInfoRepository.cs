using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IMFIInfoRepository : IRepository<MFIInformation>
    {
    }
    public class MFIInfoRepository : RepositoryBaseCodeFirst<MFIInformation>, IMFIInfoRepository
    {
        public MFIInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
