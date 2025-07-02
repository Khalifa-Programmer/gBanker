using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IAssetInfoRepository : IRepository<AssetInfo>
    {

    }
    public class AssetInfoRepository : RepositoryBaseCodeFirst<AssetInfo>, IAssetInfoRepository
    {
        public AssetInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
