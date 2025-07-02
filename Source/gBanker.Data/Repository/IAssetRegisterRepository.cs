using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IAssetRegisterRepository : IRepository<AssetRegister>
    {

    }
    public class AssetRegisterRepository : RepositoryBaseCodeFirst<AssetRegister>, IAssetRegisterRepository
    {
        public AssetRegisterRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }


    }
}
