using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IWeekNoRepository : IRepository<WeekNo>
    {
    }
    public class WeekNoRepository : RepositoryBaseCodeFirst<WeekNo>, IWeekNoRepository
    {
        public WeekNoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
