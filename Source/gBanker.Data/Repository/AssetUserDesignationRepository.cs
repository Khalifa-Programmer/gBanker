using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IAssetUserDesignationRepository : IRepository<AssetUserDesignation>
    {

    }
    public class AssetUserDesignationRepository : RepositoryBaseCodeFirst<AssetUserDesignation>, IAssetUserDesignationRepository
    {
        public AssetUserDesignationRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
