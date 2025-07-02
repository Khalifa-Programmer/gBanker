using gBanker.Data.CodeFirstMigration.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration.InfrastructureBase
{
   public interface IDatabaseFactoryCodeFirst
    {
       gBankerDbContext Get();
    }
}
