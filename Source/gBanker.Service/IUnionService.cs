using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using gBanker.Core.Filters;

namespace gBanker.Service
{
    public interface IUnionService : IServiceBase<Union>
    {

    }

    public class UnionService : IUnionService
    {
        private readonly IUnionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public UnionService(IUnionRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;

        }
        public Union Create(Union objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Delete(int id)
        {
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }

        public IEnumerable<Union> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.UnionID);
            return entities;
        }

        public Union GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Union GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }
        public bool IsValidTargetAchievement(Union TargetAchievement)
        {
            var entity = repository.Get(p => p.UnionID == TargetAchievement.UnionID);
            return entity == null ? true : false; ;
        }
        public IEnumerable<Union> SearchTargetAchievement()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.UnionID);
        }
        public IEnumerable<Union> GetMany(Expression<Func<Union, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
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
        public void Save()
        {
            unitOfWork.Commit();
        }
        public void Update(Union objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }        
    }
}
