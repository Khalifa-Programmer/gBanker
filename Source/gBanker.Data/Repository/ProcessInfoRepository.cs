using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IProcessInfoRepository : IRepository<ProcessInfo>
    {
    }
    public class ProcessInfoRepository : RepositoryBaseCodeFirst<ProcessInfo>, IProcessInfoRepository
    {
        public ProcessInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
