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
    public interface IInterbranchTransactionTrackingService : IServiceBase<InterbranchTransactionTracking>
    { }
    public class InterbranchTransactionTrackingService : IInterbranchTransactionTrackingService
    {
        private readonly IInterbranchTransactionTrackingRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public InterbranchTransactionTrackingService(IInterbranchTransactionTrackingRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<InterbranchTransactionTracking> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive==true).OrderBy(c => c.SummaryID);
            return entities;
        }

        public InterbranchTransactionTracking GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public InterbranchTransactionTracking Create(InterbranchTransactionTracking objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(InterbranchTransactionTracking objectToUpdate)
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



        public InterbranchTransactionTracking GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InterbranchTransactionTracking> GetMany(Expression<Func<InterbranchTransactionTracking, bool>> where)
        {
            throw new NotImplementedException();
        }

        InterbranchTransactionTracking IServiceBase<InterbranchTransactionTracking>.GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }
    }
}
