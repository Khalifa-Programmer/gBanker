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
    public interface IStatisticsDescriptionService : IServiceBase<StatisticsDescription>
    { }
    public class StatisticsDescriptionService : IStatisticsDescriptionService
    {
        private readonly IStatisticsDescriptionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public StatisticsDescriptionService(IStatisticsDescriptionRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<StatisticsDescription> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.StatisticsDescriptionID);
            return entities;
        }

        public StatisticsDescription GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public StatisticsDescription Create(StatisticsDescription objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(StatisticsDescription objectToUpdate)
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
        public bool IsValidStatisticsDescription(StatisticsDescription StatisticsDescription)
        {
            var entity = repository.Get(p => p.StatisticsDescriptionID == StatisticsDescription.StatisticsDescriptionID);
            return entity == null ? true : false; ;
        }


        public IEnumerable<StatisticsDescription> SearchStatisticsDescription()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.StatisticsDescriptionID);
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
        public StatisticsDescription GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StatisticsDescription> GetMany(Expression<Func<StatisticsDescription, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
