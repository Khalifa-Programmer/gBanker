using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.CodeFirstMigration.InfrastructureBase
{
   public interface IUnitOfWorkCodeFirst
    {
       void Commit();
    }
}
