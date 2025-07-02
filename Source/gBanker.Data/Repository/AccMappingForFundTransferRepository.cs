using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IAccMappingForFundTransferRepository : IRepository<AccMappingForFundTransfer>
    {
    }
    public class AccMappingForFundTransferRepository : RepositoryBaseCodeFirst<AccMappingForFundTransfer>, IAccMappingForFundTransferRepository
    {
        public AccMappingForFundTransferRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
