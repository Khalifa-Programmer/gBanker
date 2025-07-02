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
    public interface IStopInterestService : IServiceBase<StopInterest>
    {

    }
    public class StopInterestService : IStopInterestService
    {
        private readonly IStopInterestRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public StopInterestService(IStopInterestRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<StopInterest> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.StopInterestID);
            return entities;
        }       
        public StopInterest GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public StopInterest Create(StopInterest objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(StopInterest objectToUpdate)
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
            unitOfWork.Commit();
        }


        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException(); ;
        }


        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }


        public StopInterest Get(Expression<Func<StopInterest, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<StopInterest> GetMany(Expression<Func<StopInterest, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }        
        public StopInterest GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
      
    }
}
