using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface IOrganizationWiseRoleSecurityService : IServiceBase<AspNetSecurityModule>
    {
        IEnumerable<AspNetSecurityModule> GetAllPrentModule(int OrgID);

        IEnumerable<AspNetSecurityModule> GetAllModulesForParent(int parentModuleId, int roleId, int OrgID);
      
    }
    public class OrganizationWiseRoleSecurityService : IOrganizationWiseRoleSecurityService
    {
        private readonly IOrganizationWiseRoleSecurityReposiltory repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;


        public OrganizationWiseRoleSecurityService(IOrganizationWiseRoleSecurityReposiltory repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;           
        }

        public IEnumerable<AspNetSecurityModule> GetAllPrentModule(int OrgID)
        {
            return repository.GetAllPrentModule(OrgID);
        }

        public IEnumerable<AspNetSecurityModule> GetAllModulesForParent(int parentModuleId, int roleId, int OrgID)
        {
            return repository.GetAllModulesForParent(parentModuleId, roleId, OrgID);
        }




        public IEnumerable<AspNetSecurityModule> GetAll()
        {
            return repository.GetAll();
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
            throw new NotImplementedException();
        }


        public AspNetSecurityModule GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<AspNetSecurityModule> GetMany(Expression<Func<AspNetSecurityModule, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
