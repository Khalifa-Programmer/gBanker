using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration;
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
    public interface IDailyTransactionService : IServiceBase<DailyTransaction>
    {
        DailyTransaction AddNewDailyTransaction(DailyTransaction dailyTransaction);
        bool UpdateDailyTransaction(DailyTransaction dailyTransaction);
    }
    public class DailyTransactionService : IDailyTransactionService
    {
        private readonly IDailyTransactionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DailyTransactionService(IDailyTransactionRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<DailyTransaction> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.DailyTransactionId);
            return entities;
        }

        public DailyTransaction GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public DailyTransaction AddNewDailyTransaction(DailyTransaction dailyTransaction)
        {
            return repository.AddNewDailyTransaction(dailyTransaction);
        }
        public bool UpdateDailyTransaction(DailyTransaction dailyTransaction)
        {
            return repository.UpdateDailyTransaction(dailyTransaction);
        }
        public DailyTransaction Create(DailyTransaction objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DailyTransaction objectToUpdate)
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
        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }
        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public DailyTransaction GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<DailyTransaction> GetMany(Expression<Func<DailyTransaction, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
    }
}
