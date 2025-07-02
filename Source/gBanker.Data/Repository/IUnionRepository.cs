using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.CodeFirstMigration;

namespace gBanker.Data.Repository
{
    public interface IUnionRepository : IRepository<Union>
    {
    }
    public class UnionRepository : RepositoryBaseCodeFirst<Union>, IUnionRepository
    {
        public UnionRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
