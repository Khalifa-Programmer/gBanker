using gBanker.Data;
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
    public interface IAssetValuerService : IServiceBase<AssetValuer>
    {


    }
    public class AssetValuerService : IAssetValuerService
    {
        private readonly IAssetValuerRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AssetValuerService(IAssetValuerRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<AssetValuer> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.ValuerID);
            return entities;
        }

        public AssetValuer GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public AssetValuer Create(AssetValuer objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AssetValuer objectToUpdate)
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

        public AssetValuer Get(Expression<Func<AssetValuer, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<AssetValuer> GetMany(Expression<Func<AssetValuer, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
        public AssetValuer GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

    }
}
