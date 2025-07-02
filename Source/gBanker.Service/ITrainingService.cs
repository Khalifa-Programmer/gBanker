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
    public interface ITrainingService : IServiceBase<Training>
    { }
    public class TrainingService : ITrainingService
    {
        private readonly ITrainingRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public TrainingService(ITrainingRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Training> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.TrainingID);
            return entities;
        }

        public Training GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Training Create(Training objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Training objectToUpdate)
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
        public bool IsValidTraining(Training Training)
        {
            var entity = repository.Get(p => p.TrainingID == Training.TrainingID);
            return entity == null ? true : false; ;
        }


        public IEnumerable<Training> SearchTraining()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.TrainingID);
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


        public Training GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Training> GetMany(Expression<Func<Training, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
