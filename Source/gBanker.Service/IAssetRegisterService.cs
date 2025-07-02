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
    public interface IAssetRegisterService : IServiceBase<AssetRegister>
    {

    }
    public class AssetRegisterService : IAssetRegisterService
    {
        private readonly IAssetRegisterRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AssetRegisterService(IAssetRegisterRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AssetRegister> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.AssetRegisterID);
            return entities;
        }

        public AssetRegister GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public AssetRegister Create(AssetRegister objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AssetRegister objectToUpdate)
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


        public AssetRegister Get(Expression<Func<AssetRegister, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<AssetRegister> GetMany(Expression<Func<AssetRegister, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }      
        public AssetRegister GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }        
    }
}
