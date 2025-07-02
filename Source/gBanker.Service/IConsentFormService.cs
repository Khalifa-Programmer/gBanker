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
    public interface IConsentFormService : IServiceBase<ConsentForm>
    {

    }
    public class ConsentFormService : IConsentFormService
    {
        private readonly IConsentFormRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ConsentFormService(IConsentFormRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<ConsentForm> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.Id);
            return entities;
        }       
        public ConsentForm GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public ConsentForm Create(ConsentForm objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ConsentForm objectToUpdate)
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


        public ConsentForm Get(Expression<Func<ConsentForm, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ConsentForm> GetMany(Expression<Func<ConsentForm, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.StopStatus == 0);
            return entities;
        }
        public ConsentForm GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
      
    }
}
