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
    public interface IClientTypeService : IServiceBase<ClientType>
    {


    }
    public class ClientTypeService : IClientTypeService
    {
        private readonly IClientTypeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ClientTypeService(IClientTypeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<ClientType> GetAll()
        {
            var entities = repository.GetAll().Where(c => c.IsActive == true).OrderBy(c => c.ClientTypeID);
            return entities;
        }

        public ClientType GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public ClientType Create(ClientType objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ClientType objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
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

        public void Save()
        {
            unitOfWork.Commit();
        }

        public ClientType Get(Expression<Func<ClientType, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ClientType> GetMany(Expression<Func<ClientType, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }      
        public ClientType GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }
        
    }
}
