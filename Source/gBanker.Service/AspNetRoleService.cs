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

    public interface IAspNetRoleService : IServiceBase<AspNetRole>
    { }

    public class AspNetRoleService : IAspNetRoleService
    {
        private readonly IAspNetRoleRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AspNetRoleService(IAspNetRoleRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AspNetRole> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.Name);
            return entities;
        }

        public AspNetRole GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public AspNetRole Create(AspNetRole objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AspNetRole objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }

        public void Save()
        {
            unitOfWork.Commit();
        }
        
        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }


        public AspNetRole GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AspNetRole> GetMany(Expression<Func<AspNetRole, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
