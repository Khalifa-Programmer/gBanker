using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.CodeFirstMigration;

namespace gBanker.Data.Repository
{
    public interface ITargetAchievementBuroRepository : IRepository<TargetAchievementBuro>
    {
    }
    public class TargetAchievementBuroRepository : RepositoryBaseCodeFirst<TargetAchievementBuro>, ITargetAchievementBuroRepository
    {
        public TargetAchievementBuroRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
