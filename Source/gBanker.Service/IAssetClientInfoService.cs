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
    public interface IAssetClientInfoService : IServiceBase<AssetClientInfo>
    {

    }
    public class AssetClientInfoService : IAssetClientInfoService
    {
        private readonly IAssetClientInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AssetClientInfoService(IAssetClientInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AssetClientInfo> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.AssetClientInfoID);
            return entities;
        }

        public AssetClientInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public AssetClientInfo Create(AssetClientInfo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AssetClientInfo objectToUpdate)
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


        public AssetClientInfo Get(Expression<Func<AssetClientInfo, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<AssetClientInfo> GetMany(Expression<Func<AssetClientInfo, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }        
        public AssetClientInfo GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }
        
    }
}
