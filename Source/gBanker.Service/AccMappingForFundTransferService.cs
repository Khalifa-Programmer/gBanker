using gBanker.Data.CodeFirstMigration;
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
    public interface IAccMappingForFundTransferService : IServiceBase<AccMappingForFundTransfer>
    { 
    }
    public class AccMappingForFundTransferService : IAccMappingForFundTransferService
    {
        private readonly IAccMappingForFundTransferRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AccMappingForFundTransferService(IAccMappingForFundTransferRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AccMappingForFundTransfer> GetAll()
        {
            var entities = repository.GetMany(x=>x.IsActive==true).OrderBy(c => c.ID);
            return entities;
        }

        public AccMappingForFundTransfer GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public AccMappingForFundTransfer Create(AccMappingForFundTransfer objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AccMappingForFundTransfer objectToUpdate)
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

        public AccMappingForFundTransfer GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<AccMappingForFundTransfer> GetMany(Expression<Func<AccMappingForFundTransfer, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
    }
}
