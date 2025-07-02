using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;


namespace gBanker.Data.Repository
{
    public interface ILegalInfoRepository : IRepository<LegalInfo>
    {
    }
    public class LegalInfoRepository : RepositoryBaseCodeFirst<LegalInfo>, ILegalInfoRepository
    {
        public LegalInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
