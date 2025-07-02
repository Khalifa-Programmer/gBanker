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
    public interface IAssetProcessInfoService : IServiceBase<AssetProcessInfo>
    {


    }
    public class AssetProcessInfoService : IAssetProcessInfoService
    {
        private readonly IAssetProcessInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AssetProcessInfoService(IAssetProcessInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }       
        public IEnumerable<AssetProcessInfo> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.ProcessID);
            return entities;
        }

        public AssetProcessInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public AssetProcessInfo Create(AssetProcessInfo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AssetProcessInfo objectToUpdate)
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

        public AssetProcessInfo Get(Expression<Func<AssetProcessInfo, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        //public IEnumerable<AssetProcessInfo> GetMany(Expression<Func<AssetProcessInfo, bool>> where)
        //{
        //    var entities = repository.GetMany(where).Where(b => b.IsActive == true);
        //    return entities;
        //}
        public AssetProcessInfo GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AssetProcessInfo> GetMany(Expression<Func<AssetProcessInfo, bool>> where)
        {
            var entities = repository.GetMany(where);//.Where(b => b.IsActive == true);
            return entities;
        }
    }
}
