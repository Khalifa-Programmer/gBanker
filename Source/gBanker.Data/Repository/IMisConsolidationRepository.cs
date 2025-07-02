using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IMISConsolidationProcessRepository : IRepository<MISConsolidationProcess>
    {

    }
    public class MISConsolidationProcessRepository : RepositoryBaseCodeFirst<MISConsolidationProcess>, IMISConsolidationProcessRepository
    {
        public MISConsolidationProcessRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
