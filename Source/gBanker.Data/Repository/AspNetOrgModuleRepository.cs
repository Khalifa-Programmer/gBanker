using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IAspNetOrgModuleRepository : IRepository<AspNetOrgModule>
    {
    }
    public class AspNetOrgModuleRepository: RepositoryBaseCodeFirst<AspNetOrgModule>, IAspNetOrgModuleRepository
    {
        public AspNetOrgModuleRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
