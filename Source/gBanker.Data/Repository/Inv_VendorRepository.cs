using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class Inv_VendorRepository : RepositoryBaseCodeFirst<Inv_Vendor>, IInv_VendorRepository
    {
        public Inv_VendorRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IInv_VendorRepository : IRepository<Inv_Vendor>
    {
    }
}



