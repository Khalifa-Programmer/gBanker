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
    public interface IAssetUserDepartmentService : IServiceBase<AssetUserDepartment>
    {

    }
    public class AssetUserDepartmentService : IAssetUserDepartmentService
    {
        private readonly IAssetUserDepartmentRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AssetUserDepartmentService(IAssetUserDepartmentRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AssetUserDepartment> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.DepartmentID);
            return entities;
        }
        public AssetUserDepartment GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public AssetUserDepartment Create(AssetUserDepartment objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AssetUserDepartment objectToUpdate)
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


        public AssetUserDepartment Get(Expression<Func<AssetUserDepartment, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<AssetUserDepartment> GetMany(Expression<Func<AssetUserDepartment, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
        public AssetUserDepartment GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

    }
}
