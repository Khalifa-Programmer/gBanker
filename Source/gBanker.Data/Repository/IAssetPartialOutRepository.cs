using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IAssetPartialOutRepository : IRepository<AssetPartialOut>
    {

    }
    public class AssetPartialOutRepository : RepositoryBaseCodeFirst<AssetPartialOut>, IAssetPartialOutRepository
    {
        public AssetPartialOutRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
