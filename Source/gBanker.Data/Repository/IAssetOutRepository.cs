using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IAssetOutRepository : IRepository<AssetOut>
    {

    }
    public class AssetOutRepository : RepositoryBaseCodeFirst<AssetOut>, IAssetOutRepository
    {
        public AssetOutRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
