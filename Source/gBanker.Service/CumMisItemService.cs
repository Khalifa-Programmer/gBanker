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
    public interface ICumMisItemService : IServiceBase<CumMisItem>
    {
      
    }
    public class CumMisItemService : ICumMisItemService
    {
        private readonly ICumMisItemRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public CumMisItemService(ICumMisItemRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public CumMisItem Create(CumMisItem objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Delete(int id)
        {
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }

        public IEnumerable<CumMisItem> GetAll()
        {

            var entities = repository.GetAll().OrderBy(c => c.CumMisItemName);
            return entities;
        }

        public CumMisItem GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public CumMisItem GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CumMisItem> GetMany(Expression<Func<CumMisItem, bool>> where)
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

        public void Update(CumMisItem objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }
    }
}
