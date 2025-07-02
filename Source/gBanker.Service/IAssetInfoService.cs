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
    public interface IAssetInfoService : IServiceBase<AssetInfo>
    {

    }
    public class AssetInfoService : IAssetInfoService
    {
        private readonly IAssetInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AssetInfoService(IAssetInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AssetInfo> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.AssetID);
            return entities;
        }

        public AssetInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public AssetInfo Create(AssetInfo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AssetInfo objectToUpdate)
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


        public AssetInfo Get(Expression<Func<AssetInfo, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<AssetInfo> GetMany(Expression<Func<AssetInfo, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }       
        public AssetInfo GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }
        
    }
}
