using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Data.Repository
{
    public interface IDashBoardRepository : IRepository<DashBoard>
    {
    }
    public class DashBoardRepository : RepositoryBaseCodeFirst<DashBoard>, IDashBoardRepository
    {
        public DashBoardRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
