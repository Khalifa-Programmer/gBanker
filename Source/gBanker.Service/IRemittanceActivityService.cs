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
    public interface IRemittanceActivityService : IServiceBase<RemittanceActivity>
    { }
    public class RemittanceActivityService : IRemittanceActivityService
    {
        private readonly IRemittanceActivityRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public RemittanceActivityService(IRemittanceActivityRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<RemittanceActivity> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.RemittanceActivityId);
            return entities;
        }

        public RemittanceActivity GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public RemittanceActivity Create(RemittanceActivity objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(RemittanceActivity objectToUpdate)
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
        public bool IsValidRemittanceActivity(RemittanceActivity RemittanceActivity)
        {
            var entity = repository.Get(p => p.RemittanceActivityId == RemittanceActivity.RemittanceActivityId);
            return entity == null ? true : false; ;
        }


        public IEnumerable<RemittanceActivity> SearchRemittanceActivity()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.RemittanceActivityId);
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


        public RemittanceActivity GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RemittanceActivity> GetMany(Expression<Func<RemittanceActivity, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
