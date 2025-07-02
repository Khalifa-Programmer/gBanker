using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels.OLRSHubs.CommonModels;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Service
{
    public interface IPOProductMappingService : IServiceBase<POProductMapping>
    {
        Task<IEnumerable<POProductMapping>> GetPOProductMappings();
        Task<IEnumerable<LOAN_PRODUCT>> GetLOAN_PRODUCT_List();
        bool ManageProductXLoanMapping(List<POProductMapping> oProductMappings);
        Task<IEnumerable<POLoanCodeWithProductModel>> GetPOLoanCodeWiseProductMappings();
    }
    public class POProductMappingService : IPOProductMappingService
    {
        private readonly IPOProductMappingRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public POProductMappingService(IPOProductMappingRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<POProductMapping>> GetPOProductMappings()
        {
            try
            {
                var listing = await repository.GetPOProductMappings();
                return listing;
            }
            catch
            {
                return new List<POProductMapping>();
            }            
        }

        public async Task<IEnumerable<LOAN_PRODUCT>> GetLOAN_PRODUCT_List()
        {
           
            try
            {
                return await repository.GetLOAN_PRODUCT_List();
            }
            catch (Exception ex)
            {
                return new List<LOAN_PRODUCT>();
            }

        }
        public async Task<IEnumerable<POLoanCodeWithProductModel>> GetPOLoanCodeWiseProductMappings()
        {
            try
            {
                var listing = await repository.GetPOLoanCodeWiseProductMappings();
                return listing;
            }
            catch
            {
                return new List<POLoanCodeWithProductModel>();
            }
        }

        public bool ManageProductXLoanMapping(List<POProductMapping> oProductMappings)
        {
            try
            {
                var isOperationSuccess = repository.ManageProductXLoanMapping(oProductMappings);
                return isOperationSuccess;
            }
            catch
            {
                return false;
            }
        }
        public IEnumerable<POProductMapping> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.Id);
            return entities;
        }

        public POProductMapping GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public POProductMapping Create(POProductMapping objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(POProductMapping objectToUpdate)
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

        public POProductMapping GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<POProductMapping> GetMany(Expression<Func<POProductMapping, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
    }
}
