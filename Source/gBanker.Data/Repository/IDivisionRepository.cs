using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.CodeFirstMigration;

namespace gBanker.Data.Repository
{
    public interface IDivisionRepository : IRepository<Division>
    {
    }
    public class DivisionRepository : RepositoryBaseCodeFirst<Division>, IDivisionRepository
    {
        public DivisionRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}

