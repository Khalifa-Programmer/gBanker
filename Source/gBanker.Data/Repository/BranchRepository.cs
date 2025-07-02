using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;


namespace gBanker.Data.Repository
{
    public interface IBranchRepository : IRepository<Branch>
    {
    }
    public class BranchRepository  : RepositoryBaseCodeFirst<Branch>,IBranchRepository
    {
        public BranchRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
