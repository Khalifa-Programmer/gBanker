using gBanker.Data.CodeFirstMigration.Db;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration.InfrastructureBase
{
   public class DatabaseFactoryCodeFirst: Disposable,IDatabaseFactoryCodeFirst
    {
       private gBankerDbContext dataContext;
       public gBankerDbContext Get()
        {
            return dataContext ?? (dataContext = new gBankerDbContext());
        }
        protected override void DisposeCore()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }
}
