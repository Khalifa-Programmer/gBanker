using gBanker.Core.Common;
using gBanker.Core.Filters;
using gBanker.Data.CodeFirstMigration;
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
    public interface IPOLoanCodeService : IServiceBase<POLoanCode>
    {
        Task<IEnumerable<POLoanCode>> GetPOLoanCodes();
        Task<IEnumerable<POLoanCodeRelatedAccCodeModel>> GetPOAccCodes(BaseSearchFilter filter);
        Task<bool> GetPOLoanCodes(List<POLoanCode> pOLoanCodes);
        Task<bool> Manage_IMP_COST_LN_SC(IMP_COST_LN_SC_INSERT_Model model);
    }
    public class POLoanCodeService : IPOLoanCodeService
    {
        private readonly IPOLoanCodeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public POLoanCodeService(IPOLoanCodeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<POLoanCode>> GetPOLoanCodes()
        {
            try
            {
                return await repository.GetPOLoanCodes();
            }
            catch
            {
                return new List<POLoanCode>();
            }

        }

        public async Task<IEnumerable<POLoanCodeRelatedAccCodeModel>> GetPOAccCodes(BaseSearchFilter filter)
        {
            var listing = new List<POLoanCodeRelatedAccCodeModel>();
            try
            {
                return await repository.GetPOAccCodes(filter);
            }
            catch
            {
                return new List<POLoanCodeRelatedAccCodeModel>();
            }
        }

        public async Task<bool> Manage_IMP_COST_LN_SC(IMP_COST_LN_SC_INSERT_Model model)
        {
            try
            {                 
                 return await repository.Manage_IMP_COST_LN_SC(model);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> GetPOLoanCodes(List<POLoanCode> pOLoanCodes)
        {           
            try
            {
                return await repository.GetPOLoanCodes(pOLoanCodes);
            }
            catch
            {
                return false;
            }            
        }

        public IEnumerable<POLoanCode> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.Id);
            return entities;
        }

        public POLoanCode GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public POLoanCode Create(POLoanCode objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(POLoanCode objectToUpdate)
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

        public POLoanCode GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<POLoanCode> GetMany(Expression<Func<POLoanCode, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
    }
}
