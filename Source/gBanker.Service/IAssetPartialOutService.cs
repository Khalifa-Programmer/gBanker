using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
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
    public interface IAssetPartialOutService : IServiceBase<AssetPartialOut>
    { }
    public class AssetPartialOutService : IAssetPartialOutService
    {
        private readonly IAssetPartialOutRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AssetPartialOutService(IAssetPartialOutRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AssetPartialOut> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.AssetPartialOutID);
            return entities;
        }

        public AssetPartialOut GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public AssetPartialOut Create(AssetPartialOut objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AssetPartialOut objectToUpdate)
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
        public bool IsValidAssetPartialOut(AssetPartialOut AssetPartialOut)
        {
            var entity = repository.Get(p => p.AssetPartialOutID == AssetPartialOut.AssetPartialOutID);
            return entity == null ? true : false; ;
        }


        public IEnumerable<AssetPartialOut> SearchAssetPartialOut()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.AssetPartialOutID);
        }


        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                //obj.InActiveDate = inactiveDate.HasValue ? inactiveDate : DateTime.Now;
                obj.IsActive = false;
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }


        public bool IsContinued(long id)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                var isActive = obj.IsActive;
                if (isActive == true)
                {
                    return false;
                }
            }

            return true;
        }
        public AssetPartialOut GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<AssetPartialOut> GetMany(Expression<Func<AssetPartialOut, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
    }
}
