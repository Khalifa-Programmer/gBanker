using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IProjectInfoRepository : IRepository<ProjectInfo>
    {

    }
    public class ProjectInfoRepository : RepositoryBaseCodeFirst<ProjectInfo>, IProjectInfoRepository
    {
        public ProjectInfoRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
