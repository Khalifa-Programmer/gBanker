using gBanker.Data.CodeFirstMigration;
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
    public interface IAspNetOrgModuleService : IServiceBase<AspNetOrgModule>
    {
        
    }
    class AspNetOrgModuleService: IAspNetOrgModuleService
    {
        private readonly IAspNetOrgModuleRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AspNetOrgModuleService(IAspNetOrgModuleRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<AspNetOrgModule> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.AspNetOrgModuleID);
            return entities;
        }

        public AspNetOrgModule GetById(int id)
        {
            throw new NotImplementedException();
        }

        public AspNetOrgModule Create(AspNetOrgModule objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Update(AspNetOrgModule objectToUpdate)
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


        public AspNetOrgModule GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AspNetOrgModule> GetMany(Expression<Func<AspNetOrgModule, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
