using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IAssetGroupInfoRepository : IRepository<AssetGroupInfo>
    {

    }

    public class AssetGroupInfoRepository : RepositoryBaseCodeFirst<AssetGroupInfo>, IAssetGroupInfoRepository
    {
        public AssetGroupInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
