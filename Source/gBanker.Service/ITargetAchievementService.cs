using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface ITargetAchievementService : IServiceBase<TargetAchievement>
    { }
    public class TargetAchievementService : ITargetAchievementService
    {
        private readonly ITargetAchievementRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public TargetAchievementService(ITargetAchievementRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<TargetAchievement> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.TargetId);
            return entities;
        }

        public TargetAchievement GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public TargetAchievement Create(TargetAchievement objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(TargetAchievement objectToUpdate)
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
        public bool IsValidTargetAchievement(TargetAchievement TargetAchievement)
        {
            var entity = repository.Get(p => p.TargetId == TargetAchievement.TargetId);
            return entity == null ? true : false; ;
        }


        public IEnumerable<TargetAchievement> SearchTargetAchievement()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.TargetId);
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


        public TargetAchievement GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TargetAchievement> GetMany(Expression<Func<TargetAchievement, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
