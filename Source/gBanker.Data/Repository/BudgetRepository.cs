using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;


namespace gBanker.Data.Repository
{
    public interface IBudgetRepository : IRepository<Budget>
    {
    }
    public class BudgetRepository : RepositoryBaseCodeFirst<Budget>, IBudgetRepository
    {
        public BudgetRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
