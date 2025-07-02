using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IMobileErrorLogRepository : IRepository<MobileErrorLog>
    {
       
    }
    public class MobileErrorLogRepository : RepositoryBaseCodeFirst<MobileErrorLog>, IMobileErrorLogRepository
    {
        public MobileErrorLogRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
