using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public class Inv_CategoryOrSubCategoryRepository : RepositoryBaseCodeFirst<Inv_CategoryOrSubCategory>, IInv_CategoryOrSubCategoryRepository
    {
        public Inv_CategoryOrSubCategoryRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface IInv_CategoryOrSubCategoryRepository : IRepository<Inv_CategoryOrSubCategory>
    {
    }
}



