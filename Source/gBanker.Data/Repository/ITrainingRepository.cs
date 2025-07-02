using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface ITrainingRepository : IRepository<Training>
    {
    }
    public class TrainingRepository : RepositoryBaseCodeFirst<Training>, ITrainingRepository
    {
        public TrainingRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
