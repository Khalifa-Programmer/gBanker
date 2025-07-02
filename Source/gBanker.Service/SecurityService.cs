using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gBanker.Service
{
   
    public interface ISecurityService : IServiceBase<AspNetSecurityModule>
    {
        IEnumerable<AspNetSecurityModule> GetAllPrentModule();
        IEnumerable<AspNetSecurityModule> GetAllModulesForParent(int parentModuleId, int roleId);
        void CreateSecurityRole(List<AspNetRoleModule> roleModules);
        IEnumerable<AspNetSecurityModule> GeAllRoleModules(int roleId);
        IEnumerable<AspNetSecurityModule> GetAllPrentOrgModule(int? OrgID);
        IEnumerable<AspNetSecurityModule> GetAllModulesORgForParent(int parentModuleId, int roleId,int? OrgID);
        IEnumerable<AspNetSecurityModule> GetAllModulesForParentOrganizationWise(int parentModuleId, int roleId, int OrgID);
        IEnumerable<AspNetSecurityModule> GetAllPrentModuleOrganizationWise(int? OrgID);
        IEnumerable<AspNetSecurityModule> GetAllModules();
    }
    public class SecurityService : ISecurityService
    {
        private readonly ISecurityRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;


        public SecurityService(ISecurityRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;           
        }


        public IEnumerable<AspNetSecurityModule> GetAllPrentModule()
        {
            return repository.GetAllPrentModule();
        }

        public IEnumerable<AspNetSecurityModule> GetAllModulesForParent(int parentModuleId, int roleId)
        {
            return repository.GetAllModulesForParent(parentModuleId, roleId);
        }

        public IEnumerable<AspNetSecurityModule> GetAll()
        {
            throw new NotImplementedException();
        }

        public AspNetSecurityModule GetById(int id)
        {
            throw new NotImplementedException();
        }

        public AspNetSecurityModule Create(AspNetSecurityModule objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Update(AspNetSecurityModule objectToUpdate)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            unitOfWork.Commit();
        }


        public void CreateSecurityRole(List<AspNetRoleModule> roleModules)
        {
            repository.CreateSecurityRole(roleModules);
            Save();
        }


        public IEnumerable<AspNetSecurityModule> GeAllRoleModules(int roleId)
        {
            return repository.GeAllRoleModules(roleId);
        }


        public IEnumerable<AspNetSecurityModule> GetAllPrentOrgModule(int? OrgID)
        {
            return repository.GetAllPrentOrgModule( Convert.ToInt16( OrgID));
        }

        public IEnumerable<AspNetSecurityModule> GetAllModulesORgForParent(int parentModuleId, int roleId, int? OrgID)
        {
            return repository.GetAllModulesORgForParent(parentModuleId, roleId, Convert.ToInt16(OrgID));
        }


        public IEnumerable<AspNetSecurityModule> GetAllModulesForParentOrganizationWise(int parentModuleId, int roleId, int OrgID)
        {
            return repository.GetAllModulesForParentOrganizationWise(parentModuleId, roleId, Convert.ToInt16(OrgID));
        }


        public IEnumerable<AspNetSecurityModule> GetAllPrentModuleOrganizationWise(int? OrgID)
        {
            return repository.GetAllPrentModuleOrganizationWise(Convert.ToInt16(OrgID));
        }


        public AspNetSecurityModule GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AspNetSecurityModule> GetAllModules()
        {
            return repository.GetAllModules();
        }

        public IEnumerable<AspNetSecurityModule> GetMany(Expression<Func<AspNetSecurityModule, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
