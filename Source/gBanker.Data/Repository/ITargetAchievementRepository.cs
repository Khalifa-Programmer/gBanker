using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface ITargetAchievementRepository : IRepository<TargetAchievement>
    {
    }
    public class TargetAchievementRepository : RepositoryBaseCodeFirst<TargetAchievement>, ITargetAchievementRepository
    {
        public TargetAchievementRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
