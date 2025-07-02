using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.CodeFirstMigration;

namespace gBanker.Data.Repository
{
    public interface IDistrictRepository : IRepository<District>
    {
    }
    public class DistrictRepository : RepositoryBaseCodeFirst<District>, IDistrictRepository
    {
        public DistrictRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
