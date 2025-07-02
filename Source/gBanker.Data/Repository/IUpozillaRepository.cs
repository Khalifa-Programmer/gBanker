using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.CodeFirstMigration;

namespace gBanker.Data.Repository
{
    public interface IUpozillaRepository : IRepository<Upozilla>
    {
    }
    public class UpozillaRepository : RepositoryBaseCodeFirst<Upozilla>, IUpozillaRepository
    {
        public UpozillaRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}

