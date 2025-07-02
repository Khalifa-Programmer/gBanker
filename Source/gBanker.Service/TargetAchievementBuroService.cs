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

namespace gBanker.Service
{
    public interface ITargetAchievementBuroService : IServiceBase<TargetAchievementBuro>
    {
        
    }

    public class TargetAchievementBuroService : ITargetAchievementBuroService
    {
        private readonly ITargetAchievementBuroRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public TargetAchievementBuroService(ITargetAchievementBuroRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
           
        }
        public TargetAchievementBuro Create(TargetAchievementBuro objectToCreate)
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

        public IEnumerable<TargetAchievementBuro> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.TargetId);
            return entities;
        }

        public TargetAchievementBuro GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public TargetAchievementBuro GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }
        public bool IsValidTargetAchievement(TargetAchievementBuro TargetAchievement)
        {
            var entity = repository.Get(p => p.TargetId == TargetAchievement.TargetId);
            return entity == null ? true : false; ;
        }


        public IEnumerable<TargetAchievementBuro> SearchTargetAchievement()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.TargetId);
        }

        public IEnumerable<TargetAchievementBuro> GetMany(Expression<Func<TargetAchievementBuro, bool>> where)
        {
            throw new NotImplementedException();
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

        public void Update(TargetAchievementBuro objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }
    }
}
