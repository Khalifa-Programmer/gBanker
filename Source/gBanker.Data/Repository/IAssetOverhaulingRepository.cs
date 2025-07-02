using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IAssetOverhaulingRepository : IRepository<AssetOverhauling>
    {

    }

    public class AssetOverhaulingRepository : RepositoryBaseCodeFirst<AssetOverhauling>, IAssetOverhaulingRepository
    {
        public AssetOverhaulingRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
