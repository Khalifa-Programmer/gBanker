using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IBatchPostingProcessRepository : IRepository<BatchPostingProcess>
    {

    }
    public class BatchPostingProcessRepository : RepositoryBaseCodeFirst<BatchPostingProcess>, IBatchPostingProcessRepository
    {
        public BatchPostingProcessRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
