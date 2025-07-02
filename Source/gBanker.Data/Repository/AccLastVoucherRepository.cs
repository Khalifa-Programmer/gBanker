using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IAccLastVoucherRepository : IRepository<AccLastVoucher>
    {
    }
    public class AccLastVoucherRepository : RepositoryBaseCodeFirst<AccLastVoucher>, IAccLastVoucherRepository
    {
        public AccLastVoucherRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
