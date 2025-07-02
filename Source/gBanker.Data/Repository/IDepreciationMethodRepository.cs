using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gBanker.Data.Repository
{
    public interface IDepreciationMethodRepository : IRepository<DepreciationMethod>
    {
        
    }
    public class DepreciationMethodRepository : RepositoryBaseCodeFirst<DepreciationMethod>, IDepreciationMethodRepository
    {
        public DepreciationMethodRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
