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
    public interface IWriteOffHistoryService : IServiceBase<WriteOffHistory>
    {
        int SetOpeningSavingEntry(int? orgID, int? officeID);
    }

    public class WriteOffHistoryService : IWriteOffHistoryService
    {
        private readonly IWriteOffHistoryRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public WriteOffHistoryService(IWriteOffHistoryRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;

        }
        public WriteOffHistory Create(WriteOffHistory objectToCreate)
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

        public IEnumerable<WriteOffHistory> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.WriteOffHistoryID);
            return entities;
        }

        public WriteOffHistory GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public WriteOffHistory GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public bool IsValidTargetAchievement(WriteOffHistory TargetAchievement)
        {
            var entity = repository.Get(p => p.WriteOffHistoryID == TargetAchievement.WriteOffHistoryID);
            return entity == null ? true : false; ;
        }


        public IEnumerable<WriteOffHistory> SearchTargetAchievement()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.WriteOffHistoryID);
        }

        public IEnumerable<WriteOffHistory> GetMany(Expression<Func<WriteOffHistory, bool>> where)
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

        public void Update(WriteOffHistory objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public int SetOpeningSavingEntry(int? orgID, int? officeID)
        {
            return repository.SetOpeningSavingEntry(orgID, officeID);
        }
    }
}
