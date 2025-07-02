using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IPageWiseSecurityRepository : IRepository<AspNetOrgModule>
    {
        void  CreateSecurityRole(List<AspNetOrgModule> roleModules);
    }
   public class PageWiseSecurityRepository: RepositoryBaseCodeFirst<AspNetOrgModule>, IPageWiseSecurityRepository
    {
       public PageWiseSecurityRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

       public void CreateSecurityRole(List<AspNetOrgModule> roleModules)
       {
           foreach (var roleModule in roleModules)
           {
               var existingRoleModule = DataContext.AspNetOrgModules.Where(w => w.OrgID == roleModule.OrgID && w.AspNetSecurityModuleId == roleModule.AspNetSecurityModuleId).FirstOrDefault();
               if (roleModule.IsSelectedForRole)
               {
                   if (existingRoleModule == null)
                   {
                       var OrgIDParameter = new SqlParameter("@OrgID", roleModule.OrgID);
                       var varAspNetSecurityModuleId = new SqlParameter("@AspNetSecurityModuleId", roleModule.AspNetSecurityModuleId);
                       var varAspNetOrgModuleID = new SqlParameter("@AspNetOrgModuleID", roleModule.AspNetOrgModuleID);
                       var varQtype = new SqlParameter("@qtype", 1);
                       DataContext.Database.ExecuteSqlCommand("SetAspOrgModule @OrgID,@AspNetSecurityModuleId,@AspNetOrgModuleID,@qtype", OrgIDParameter, varAspNetSecurityModuleId, varAspNetOrgModuleID, varQtype);
                      // Add(roleModule);
                   }
                   else
                   {
                       var OrgIDParameter = new SqlParameter("@OrgID", roleModule.OrgID);
                       var varAspNetSecurityModuleId = new SqlParameter("@AspNetSecurityModuleId", roleModule.AspNetSecurityModuleId);
                       var varAspNetOrgModuleID = new SqlParameter("@AspNetOrgModuleID", roleModule.AspNetOrgModuleID);
                       var varQtype = new SqlParameter("@qtype", 2);
                       DataContext.Database.ExecuteSqlCommand("SetAspOrgModule @OrgID,@AspNetSecurityModuleId,@AspNetOrgModuleID,@qtype", OrgIDParameter, varAspNetSecurityModuleId, varAspNetOrgModuleID, varQtype);

                       //Update(existingRoleModule);
                   }
               }
               else
               {
                   var OrgIDParameter = new SqlParameter("@OrgID", roleModule.OrgID);
                   var varAspNetSecurityModuleId = new SqlParameter("@AspNetSecurityModuleId", roleModule.AspNetSecurityModuleId);
                   var varAspNetOrgModuleID = new SqlParameter("@AspNetOrgModuleID", roleModule.AspNetOrgModuleID);
                   var varQtype = new SqlParameter("@qtype", 3);
                   
                   if (existingRoleModule != null)
                       DataContext.Database.ExecuteSqlCommand("SetAspOrgModule @OrgID,@AspNetSecurityModuleId,@AspNetOrgModuleID,@qtype", OrgIDParameter, varAspNetSecurityModuleId, varAspNetOrgModuleID, varQtype);

                       //Delete(existingRoleModule);
               }
           }
       }
    }
}
