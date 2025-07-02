using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IWorkingAreaLogRepository : IRepository<WorkingAreaLog>
    {
       
    }
     public class WorkingAreaLogRepository: RepositoryBaseCodeFirst<WorkingAreaLog>, IWorkingAreaLogRepository
    {
         public WorkingAreaLogRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
    }
}
