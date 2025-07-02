using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IClientTypeRepository : IRepository<ClientType>
    {

    }
    public class ClientTypeRepository : RepositoryBaseCodeFirst<ClientType>, IClientTypeRepository
    {
        public ClientTypeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
