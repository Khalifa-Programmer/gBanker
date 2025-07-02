using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IOrganizationWiseRoleSecurityReposiltory : IRepository<AspNetSecurityModule>
    {
        IEnumerable<AspNetSecurityModule> GetAllPrentModule(int OrgID);

        IEnumerable<AspNetSecurityModule> GetAllModulesForParent(int parentModuleId, int roleId, int OrgID);
        int GetTotalOrganizationMember();
    }
    public class OrganizationWiseRoleSecurityReposiltory : RepositoryBaseCodeFirst<AspNetSecurityModule>, IOrganizationWiseRoleSecurityReposiltory
    {
       public OrganizationWiseRoleSecurityReposiltory(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }



       public IEnumerable<AspNetSecurityModule> GetAllPrentModule(int OrgID)
       {

           var query = from aom in DataContext.AspNetOrgModules

                       join AspNetSecurityModule in DataContext.AspNetSecurityModules on aom.AspNetSecurityModuleId
                            equals AspNetSecurityModule.AspNetSecurityModuleId

                       where aom.OrgID == OrgID
                       select new AspNetSecurityModule
                       {
                           AspNetSecurityModuleId = AspNetSecurityModule.AspNetSecurityModuleId,
                           SecurityModuleCode = AspNetSecurityModule.SecurityModuleCode,
                           LinkText = AspNetSecurityModule.LinkText,
                           ControllerName = AspNetSecurityModule.ControllerName,
                           ActionName = AspNetSecurityModule.ActionName,
                           ParentModuleId = AspNetSecurityModule.ParentModuleId,
                           SecurityLevelId = AspNetSecurityModule.SecurityLevelId,
                       };

          
               return query;
           
        //   return View(query);

        //   var obj = DataContext.AspNetSecurityModules.Where(w => !w.ParentModuleId.HasValue)
        //.Select(s => new DBAspOrgSecurityModule()
        //{
        //    AspNetSecurityModuleId = s.AspNetSecurityModuleId,
        //    SecurityModuleCode = s.SecurityModuleCode,
        //    LinkText = s.LinkText,
        //    ControllerName = s.ControllerName,
        //    ActionName = s.ActionName,
        //    ParentModuleId = s.ParentModuleId,
        //    SecurityLevelId = s.SecurityLevelId,

        //}).Where(w => w.OrgID == OrgID);

        //   return obj;
       }

       //public IEnumerable<AspNetSecurityModule> GetAllModulesForParent(int parentModuleId, int roleId, int OrgID)
       //{
       //    throw new NotImplementedException();
       //}


       public IEnumerable<AspNetSecurityModule> GetAllModulesForParent(int parentModuleId, int roleId, int OrgID)
       {

           //var obj = DataContext.AspNetSecurityModules.Where(p => p.ParentModuleId.Value == parentModuleId)
           // .Select(s => new DBAspOrgSecurityModule()
           // {
           //     AspNetSecurityModuleId = s.AspNetSecurityModuleId,
           //     SecurityModuleCode = s.SecurityModuleCode,
           //     LinkText = s.LinkText,
           //     ControllerName = s.ControllerName,
           //     ActionName = s.ActionName,
           //     ParentModuleId = s.ParentModuleId,
           //     SecurityLevelId = s.SecurityLevelId,

           // }).Where(w => w.OrgID == OrgID);
           var obj = from aom in DataContext.AspNetOrgModules

                       join AspNetSecurityModule in DataContext.AspNetSecurityModules on aom.AspNetSecurityModuleId
                            equals AspNetSecurityModule.AspNetSecurityModuleId

                       where aom.OrgID == OrgID
                     select new AspNetSecurityModule
                       {
                           AspNetSecurityModuleId = AspNetSecurityModule.AspNetSecurityModuleId,
                           SecurityModuleCode = AspNetSecurityModule.SecurityModuleCode,
                           LinkText = AspNetSecurityModule.LinkText,
                           ControllerName = AspNetSecurityModule.ControllerName,
                           ActionName = AspNetSecurityModule.ActionName,
                           ParentModuleId = AspNetSecurityModule.ParentModuleId,
                           SecurityLevelId = AspNetSecurityModule.SecurityLevelId,
                       };

           var allModles = obj.ToList();
           var query = DataContext.AspNetRoleModules.Where(w => w.RoleId == roleId.ToString());
           var roleModles = query.ToList();
           foreach (var m in allModles)
           {
               var securityExists = roleModles.Where(rm => rm.ModuleId == m.AspNetSecurityModuleId).FirstOrDefault();
               if (securityExists != null)
               {
                   m.RoleId = roleId;
                   m.IsSelectedForRole = true;
                   m.SecurityLevelId = securityExists.SecurityLevelId;
               }
               else
               {
                   m.IsSelectedForRole = false;
                   m.SecurityLevelId = 1;
               }
           }

           return allModles;
       }


       public int GetTotalOrganizationMember()
       {
           return DataContext.AspNetRoleModules.Where(x => x.IsActive == true).Count();
       }
    }
}
