using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface ICumMisItemRepository : IRepository<CumMisItem>
    {
      
    }
    public class CumMisItemRepository : RepositoryBaseCodeFirst<CumMisItem>, ICumMisItemRepository
    {
        public CumMisItemRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
