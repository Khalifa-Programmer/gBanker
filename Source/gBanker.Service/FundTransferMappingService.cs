using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data;
using BasicDataAccess;
using BasicDataAccess.Data;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.DBDetailModels;

namespace gBanker.Service
{
    public interface IFundTransferMappingService : IServiceBase<FundTransferMapping>
    {
        void DeleteById(int id);
    }
    public class FundTransferMappingService : IFundTransferMappingService
    {
        private readonly IFundTransferMappingRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
       

        public FundTransferMappingService(IFundTransferMappingRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
          
        }
       
        public FundTransferMapping GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public FundTransferMapping Create(FundTransferMapping objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(FundTransferMapping objectToUpdate)
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
        public void DeleteById(int id)
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
    
        public IEnumerable<FundTransferMapping> GetMany(Expression<Func<Area, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FundTransferMapping> GetAll()
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

        public FundTransferMapping GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FundTransferMapping> GetMany(Expression<Func<FundTransferMapping, bool>> where)
        {
            throw new NotImplementedException();
        }

        IEnumerable<FundTransferMapping> IServiceBase<FundTransferMapping>.GetAll()
        {
            throw new NotImplementedException();
        }

        FundTransferMapping IServiceBase<FundTransferMapping>.GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }
   
    }
}
