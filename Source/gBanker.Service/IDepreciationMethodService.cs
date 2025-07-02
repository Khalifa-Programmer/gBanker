using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Service
{
    public interface IDepreciationMethodService : IServiceBase<DepreciationMethod>
    {

    }
    public class DepreciationMethodService : IDepreciationMethodService
    {
        private readonly IDepreciationMethodRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DepreciationMethodService(IDepreciationMethodRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<DepreciationMethod> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.Id);
            return entities;
        }

        public DepreciationMethod GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public DepreciationMethod Create(DepreciationMethod objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DepreciationMethod objectToUpdate)
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
            throw new NotImplementedException(); ;
        }


        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }


        public DepreciationMethod Get(Expression<Func<DepreciationMethod, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<DepreciationMethod> GetMany(Expression<Func<DepreciationMethod, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public DepreciationMethod GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }
    }
}
