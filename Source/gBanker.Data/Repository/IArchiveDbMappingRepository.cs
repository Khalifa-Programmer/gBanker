using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IArchiveDbMappingRepository : IRepository<ArchiveDbMapping>
    {

    }
    public class ArchiveDbMappingRepository : RepositoryBaseCodeFirst<ArchiveDbMapping>, IArchiveDbMappingRepository
    {
        public ArchiveDbMappingRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
