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
    public interface IAssetOverhaulingService : IServiceBase<AssetOverhauling>
    {

    }
    public class AssetOverhaulingService : IAssetOverhaulingService
    {
        private readonly IAssetOverhaulingRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AssetOverhaulingService(IAssetOverhaulingRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AssetOverhauling> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.AssetOverhaulingID);
            return entities;
        }

        public AssetOverhauling GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public AssetOverhauling Create(AssetOverhauling objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AssetOverhauling objectToUpdate)
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

        public AssetOverhauling GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<AssetOverhauling> GetMany(Expression<Func<AssetOverhauling, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
    }
}
