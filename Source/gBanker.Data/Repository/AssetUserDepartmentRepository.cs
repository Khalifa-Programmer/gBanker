using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IAssetUserDepartmentRepository : IRepository<AssetUserDepartment>
    {

    }
    public class AssetUserDepartmentRepository : RepositoryBaseCodeFirst<AssetUserDepartment>, IAssetUserDepartmentRepository
    {
        public AssetUserDepartmentRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
