using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{

    public interface ISecurityRepository : IRepository<AspNetRoleModule>
    {
        IEnumerable<AspNetSecurityModule> GetAllPrentModule();
        IEnumerable<AspNetSecurityModule> GetAllModulesForParent(int parentModuleId, int roleId);
        void CreateSecurityRole(List<AspNetRoleModule> roleModules);
        IEnumerable<AspNetSecurityModule> GeAllRoleModules(int roleId);
        IEnumerable<AspNetSecurityModule> GetAllPrentOrgModule(int OrgID);
        IEnumerable<AspNetSecurityModule> GetAllModulesORgForParent(int parentModuleId, int roleId, int OrgID);
        IEnumerable<AspNetSecurityModule> GetAllModulesForParentOrganizationWise(int parentModuleId, int roleId, int OrgID);
        IEnumerable<AspNetSecurityModule> GetAllPrentModuleOrganizationWise(int? OrgID);
        IEnumerable<AspNetSecurityModule> GetAllModules();
    }
    public class SecurityRepository : RepositoryBaseCodeFirst<AspNetRoleModule>, ISecurityRepository
    {
        public SecurityRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public IEnumerable<AspNetSecurityModule> GetAllPrentModule()
        {
            var moduels = DataContext.AspNetSecurityModules.Where(w => !w.ParentModuleId.HasValue);
            return moduels;
        }
        public IEnumerable<AspNetSecurityModule> GetAllModulesForParent(int parentModuleId, int roleId)
        {

            var allModles = DataContext.AspNetSecurityModules.Where(p => p.ParentModuleId.Value == parentModuleId).ToList();
            var allModuleIds = allModles.Select(s => s.AspNetSecurityModuleId).ToArray();
            var thirdLevelModules = DataContext.AspNetSecurityModules.Where(w => allModuleIds.Contains(w.ParentModuleId.Value) && w.MenuLevel == 3).ToList();
            allModles.AddRange(thirdLevelModules);
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


        public void CreateSecurityRole(List<AspNetRoleModule> roleModules)
        {
            foreach (var roleModule in roleModules)
            {
                var existingRoleModule = DataContext.AspNetRoleModules.Where(w => w.RoleId == roleModule.RoleId && w.ModuleId == roleModule.ModuleId).FirstOrDefault();
                if (roleModule.IsSelectedForRole)
                {
                    if (existingRoleModule == null)
                    {
                        roleModule.CreateDate = DateTime.Now;
                        Add(roleModule);
                    }
                    else
                    {
                        existingRoleModule.SecurityLevelId = roleModule.SecurityLevelId;
                        Update(existingRoleModule);
                    }
                }
                else
                {
                    if (existingRoleModule != null)
                        Delete(existingRoleModule);
                }
            }

        }


        public IEnumerable<AspNetSecurityModule> GeAllRoleModules(int roleId)
        {
            var allModles = DataContext.AspNetSecurityModules.Where(p => p.ParentModuleId.HasValue).ToList();
            var query = DataContext.AspNetRoleModules.Where(w => w.RoleId == roleId.ToString());
            var roleModles = query.ToList();
            var newList = new List<AspNetSecurityModule>();
            foreach (var m in allModles)
            {
                var securityExists = roleModles.Where(rm => rm.ModuleId == m.AspNetSecurityModuleId).FirstOrDefault();
                if (securityExists != null)
                {
                    m.RoleId = roleId;
                    m.IsSelectedForRole = true;
                    m.SecurityLevelId = securityExists.SecurityLevelId;
                    newList.Add(m);
                }
                
            }

            return newList;
        }




        public IEnumerable<AspNetSecurityModule> GetAllModulesORgForParent(int parentModuleId, int roleId, int OrgID)
        {
            //var obj = from aom in DataContext.AspNetOrgModules

            //          join AspNetSecurityModule in DataContext.AspNetSecurityModules on aom.AspNetSecurityModuleId
            //               equals AspNetSecurityModule.AspNetSecurityModuleId

            //          where aom.OrgID == OrgID
            //          select new AspNetSecurityModule
            //          {
            //              AspNetSecurityModuleId = AspNetSecurityModule.AspNetSecurityModuleId,
            //              SecurityModuleCode = AspNetSecurityModule.SecurityModuleCode,
            //              LinkText = AspNetSecurityModule.LinkText,
            //              ControllerName = AspNetSecurityModule.ControllerName,
            //              ActionName = AspNetSecurityModule.ActionName,
            //              ParentModuleId = AspNetSecurityModule.ParentModuleId,
            //              SecurityLevelId = AspNetSecurityModule.SecurityLevelId,
            //          };
            //var balance = (from a in DataContext.AspNetOrgModules
            //               join c in DataContext.AspNetSecurityModules on a.AspNetSecurityModuleId equals c.AspNetSecurityModuleId
            //               where c.OrgID == OrgID && !c.ParentModuleId.HasValue
            //               select c);
            //var allModles = balance.ToList();
            //var query = DataContext.AspNetRoleModules.Where(w => w.RoleId == roleId.ToString());
            //var roleModles = query.ToList();
            //foreach (var m in allModles)
            //{
            //    var securityExists = roleModles.Where(rm => rm.ModuleId == m.AspNetSecurityModuleId).FirstOrDefault();
            //    if (securityExists != null)
            //    {
            //        m.RoleId = roleId;
            //        m.IsSelectedForRole = true;
            //        m.SecurityLevelId = securityExists.SecurityLevelId;
            //    }
            //    else
            //    {
            //        m.IsSelectedForRole = false;
            //        m.SecurityLevelId = 1;
            //    }
            //}

            //return allModles;

            var queryJoin = from aspOrg in DataContext.AspNetOrgModules
                            where aspOrg.OrgID == OrgID

                            join asps in DataContext.AspNetSecurityModules
                                on aspOrg.AspNetSecurityModuleId equals asps.AspNetSecurityModuleId
                            where asps.ParentModuleId == parentModuleId
                            select asps;
            var allModles = queryJoin.ToList();
            //var allModles = DataContext.AspNetSecurityModules.Where(p => p.ParentModuleId.Value == parentModuleId).ToList();
            var allModuleIds = allModles.Select(s => s.AspNetSecurityModuleId).ToArray();
            var thirdLevelModules = DataContext.AspNetSecurityModules.Where(w => allModuleIds.Contains(w.ParentModuleId.Value) && w.MenuLevel == 3).ToList();
            allModles.AddRange(thirdLevelModules);
            var query = DataContext.AspNetRoleModules.Where(w => w.RoleId == roleId.ToString() );
           // var query = DataContext.AspNetOrgModules.Where(w => w.OrgID == roleId);
           
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


        public IEnumerable<AspNetSecurityModule> GetAllPrentOrgModule(int OrgID)
        {
            //var query = from aom in DataContext.AspNetOrgModules

            //            join AspNetSecurityModule in DataContext.AspNetSecurityModules on aom.AspNetSecurityModuleId
            //                 equals AspNetSecurityModule.AspNetSecurityModuleId

            //            where aom.OrgID == OrgID
            //            select new AspNetSecurityModule
            //            {
            //                AspNetSecurityModuleId = AspNetSecurityModule.AspNetSecurityModuleId,
            //                SecurityModuleCode = AspNetSecurityModule.SecurityModuleCode,
            //                LinkText = AspNetSecurityModule.LinkText,
            //                ControllerName = AspNetSecurityModule.ControllerName,
            //                ActionName = AspNetSecurityModule.ActionName,
            //                ParentModuleId = AspNetSecurityModule.ParentModuleId,
            //                SecurityLevelId = AspNetSecurityModule.SecurityLevelId,
            //            };


            //return query;
            var query = from aspOrg in DataContext.AspNetOrgModules
                        where aspOrg.OrgID== OrgID

                        join asps in DataContext.AspNetSecurityModules
                            on aspOrg.AspNetSecurityModuleId equals asps.AspNetSecurityModuleId
                      //  where !asps.ParentModuleId.HasValue
                        select asps;
            //var moduels = DataContext.AspNetSecurityModules.Where(w => !w.ParentModuleId.HasValue);
            return query;
        }



        public IEnumerable<AspNetSecurityModule> GetAllModulesForParentOrganizationWise(int parentModuleId, int roleId, int OrgID)
        {
            var allModles = DataContext.AspNetSecurityModules.Where(p => p.ParentModuleId.Value == parentModuleId).ToList();
            var query = DataContext.AspNetOrgModules.Where(w => w.OrgID == roleId);
            var roleModles = query.ToList();
            foreach (var m in allModles)
            {
                var securityExists = roleModles.Where(rm => rm.AspNetSecurityModuleId == m.AspNetSecurityModuleId).FirstOrDefault();
                if (securityExists != null)
                {
                    m.RoleId = roleId;
                    m.IsSelectedForRole = true;
                    m.SecurityLevelId = securityExists.AspNetSecurityModuleId;
                }
                else
                {
                    m.IsSelectedForRole = false;
                    m.SecurityLevelId = 1;
                }
            }

            return allModles;
            //var queryJoin = from aspOrg in DataContext.AspNetOrgModules
            //                where aspOrg.OrgID == OrgID

            //                join asps in DataContext.AspNetSecurityModules
            //                    on aspOrg.AspNetSecurityModuleId equals asps.AspNetSecurityModuleId
            //                where asps.ParentModuleId == parentModuleId
            //                select asps;
            //var allModles = queryJoin.ToList();
            ////var allModles = DataContext.AspNetSecurityModules.Where(p => p.ParentModuleId.Value == parentModuleId).ToList();
            //var allModuleIds = allModles.Select(s => s.AspNetSecurityModuleId).ToArray();
            //var thirdLevelModules = DataContext.AspNetSecurityModules.Where(w => allModuleIds.Contains(w.ParentModuleId.Value) && w.MenuLevel == 3).ToList();
            //allModles.AddRange(thirdLevelModules);
            //var query = DataContext.AspNetRoleModules.Where(w => w.RoleId == roleId.ToString());
            ////var query = DataContext.AspNetOrgModules.Where(w => w.OrgID == roleId);

            //var roleModles = query.ToList();
            //foreach (var m in allModles)
            //{
            //    var securityExists = roleModles.Where(rm => rm.ModuleId == m.AspNetSecurityModuleId).FirstOrDefault();
            //    if (securityExists != null)
            //    {
            //        m.RoleId = roleId;
            //        m.IsSelectedForRole = true;
            //        m.SecurityLevelId = securityExists.SecurityLevelId;
            //    }
            //    else
            //    {
            //        m.IsSelectedForRole = false;
            //        m.SecurityLevelId = 1;
            //    }
            //}

            //return allModles;
        }





        public IEnumerable<AspNetSecurityModule> GetAllPrentModuleOrganizationWise(int? OrgID)
        {

            var moduels = DataContext.AspNetSecurityModules.Where(w => !w.ParentModuleId.HasValue);
            return moduels;
        }

        public IEnumerable<AspNetSecurityModule> GetAllModules()
        {
            var allModles = DataContext.AspNetSecurityModules.Where(p => p.ParentModuleId.HasValue && p.IsActive.HasValue && p.IsActive.Value).ToList();
            return allModles;
        }
    }
}
