using gBanker.Core.Common;
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
    public interface IWorkingAreaLogService : IServiceBase<WorkingAreaLog>
    {
        IEnumerable<ValidationResult> IsValidSaving(WorkingAreaLog WorkingAreaLog);
    }
    public class WorkingAreaLogService: IWorkingAreaLogService
    {
        private readonly IWorkingAreaLogRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public WorkingAreaLogService(IWorkingAreaLogRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<WorkingAreaLog> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.MainProductCode);
            return entities;
        }

        public WorkingAreaLog GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public WorkingAreaLog Create(WorkingAreaLog objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(WorkingAreaLog objectToUpdate)
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

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.InActiveDate = inactiveDate.HasValue ? inactiveDate : DateTime.Now;
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

        public WorkingAreaLog GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<ValidationResult> IsValidSaving(WorkingAreaLog WorkingAreaLog)
        {
            var product = repository.GetById(WorkingAreaLog.WorkingAreaLogID);
          if (WorkingAreaLog.WorkingArea == "")
                yield return new ValidationResult("ProductID", "Invalid Product Id");
        }

        public IEnumerable<WorkingAreaLog> GetMany(Expression<Func<WorkingAreaLog, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
