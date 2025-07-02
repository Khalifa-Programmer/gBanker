using gBanker.Core.Common;
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
    public interface IFixAssetUpdatesService : IServiceBase<FixAssetUpdates>
    {

    }
    public class FixAssetUpdatesService : IFixAssetUpdatesService
    {
        private readonly IFixAssetUpdatesRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public FixAssetUpdatesService(IFixAssetUpdatesRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<FixAssetUpdates> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.FixAssetUpdateID);
            return entities;
        }

        public FixAssetUpdates GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public FixAssetUpdates Create(FixAssetUpdates objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(FixAssetUpdates objectToUpdate)
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

        public FixAssetUpdates GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<FixAssetUpdates> GetMany(Expression<Func<FixAssetUpdates, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
    }
}
