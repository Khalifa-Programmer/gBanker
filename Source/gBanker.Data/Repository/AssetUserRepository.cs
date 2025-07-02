using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IAssetUserRepository : IRepository<AssetUser>
    {

    }
    public class AssetUserRepository : RepositoryBaseCodeFirst<AssetUser>, IAssetUserRepository
    {
        public AssetUserRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
