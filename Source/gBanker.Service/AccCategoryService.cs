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
    public interface IAccCategoryService : IServiceBase<AccCategory>
    { 
    }
    public class AccCategoryService : IAccCategoryService
    {
        private readonly IAccCategoryRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AccCategoryService(IAccCategoryRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AccCategory> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.CategoryName);
            return entities;
        }

        public AccCategory GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public AccCategory Create(AccCategory objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AccCategory objectToUpdate)
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
            //throw new NotImplementedException();
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



        public AccCategory GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<AccCategory> GetMany(Expression<Func<AccCategory, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
