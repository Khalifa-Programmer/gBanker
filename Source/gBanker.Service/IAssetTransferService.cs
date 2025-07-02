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
    public interface IAssetTransferService : IServiceBase<AssetTransfer>
    {


    }
    public class AssetTransferService : IAssetTransferService
    {
        private readonly IAssetTransferRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AssetTransferService(IAssetTransferRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<AssetTransfer> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.TransferID);
            return entities;
        }

        public AssetTransfer GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public AssetTransfer Create(AssetTransfer objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AssetTransfer objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
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

        public AssetTransfer Get(Expression<Func<AssetTransfer, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<AssetTransfer> GetMany(Expression<Func<AssetTransfer, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
        public AssetTransfer GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

    }
}
