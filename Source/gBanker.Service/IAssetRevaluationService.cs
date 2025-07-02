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
    public interface IAssetRevaluationService : IServiceBase<AssetRevaluation>
    { }
    public class AssetRevaluationService : IAssetRevaluationService
    {
        private readonly IAssetRevaluationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AssetRevaluationService(IAssetRevaluationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AssetRevaluation> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.AssetRevaluationID);
            return entities;
        }

        public AssetRevaluation GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public AssetRevaluation Create(AssetRevaluation objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AssetRevaluation objectToUpdate)
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
        public bool IsValidAssetRevaluation(AssetRevaluation AssetRevaluation)
        {
            var entity = repository.Get(p => p.AssetRevaluationID == AssetRevaluation.AssetRevaluationID);
            return entity == null ? true : false; ;
        }


        public IEnumerable<AssetRevaluation> SearchAssetRevaluation()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.AssetRevaluationID);
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
        public AssetRevaluation GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<AssetRevaluation> GetMany(Expression<Func<AssetRevaluation, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
    }
}
