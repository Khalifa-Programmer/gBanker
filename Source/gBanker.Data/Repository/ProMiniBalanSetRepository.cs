using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.CodeFirstMigration;

namespace gBanker.Data.Repository
{
    public interface IProMiniBalanSetRepository : IRepository<ProMiniBalanSet>
    {
    }
    public class ProMiniBalanSetRepository : RepositoryBaseCodeFirst<ProMiniBalanSet>, IProMiniBalanSetRepository
    {
        public ProMiniBalanSetRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
