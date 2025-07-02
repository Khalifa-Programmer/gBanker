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
    public interface IAssetUserDesignationService : IServiceBase<AssetUserDesignation>
    {

    }
    public class AssetUserDesignationService : IAssetUserDesignationService
    {
        private readonly IAssetUserDesignationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AssetUserDesignationService(IAssetUserDesignationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AssetUserDesignation> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.DesignationID);
            return entities;
        }
        public AssetUserDesignation GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public AssetUserDesignation Create(AssetUserDesignation objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AssetUserDesignation objectToUpdate)
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


        public AssetUserDesignation Get(Expression<Func<AssetUserDesignation, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<AssetUserDesignation> GetMany(Expression<Func<AssetUserDesignation, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
        public AssetUserDesignation GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

    }
}
