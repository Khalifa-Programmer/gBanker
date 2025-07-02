using gBanker.Data.CodeFirstMigration.Db;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data
{
    public class UserManagementEntities : IdentityDbContext<ApplicationUser>
    {
        public UserManagementEntities()
            : base("gBankerDbContext", false)
        {

        }
    }
}

