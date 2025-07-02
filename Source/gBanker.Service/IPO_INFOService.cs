using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace gBanker.Service
{
    public interface IPO_INFOService : IServiceBase<PO_INFO>
    {
        Task<IEnumerable<PO_INFO>> GetPO_INFOCodes();
        Task<PO_INFO_MAPPING> Get_PO_INFO_MAPPING(string mfi_PO_CODE);
        Task<bool> Manage_PO_INFO_MAPPING(string mfi_PO_CODE, string pksf_PO_CODE);


    }
    public class PO_INFOService : IPO_INFOService
    {
        private readonly IPO_INFORepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public PO_INFOService(IPO_INFORepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<PO_INFO_MAPPING> Get_PO_INFO_MAPPING(string mfi_PO_CODE)
        {
            try
            {
                return await repository.Get_PO_INFO_MAPPING(mfi_PO_CODE);
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Manage_PO_INFO_MAPPING(string mfi_PO_CODE,string pksf_PO_CODE)
        {
            try
            {
                return await repository.Manage_PO_INFO_MAPPING(mfi_PO_CODE, pksf_PO_CODE);
            }
            catch
            {
                return false;
            }
        }

        public PO_INFO Create(PO_INFO objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PO_INFO> GetAll()
        {
            throw new NotImplementedException();
        }

        public PO_INFO GetById(int id)
        {
            throw new NotImplementedException();
        }

        public PO_INFO GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PO_INFO> GetMany(Expression<Func<PO_INFO, bool>> where)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PO_INFO>> GetPO_INFOCodes()
        {
            try
            {
                return await repository.GetPO_INFOCodes();
            }
            catch
            {
                return new List<PO_INFO>();
            }

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
            throw new NotImplementedException();
        }

        public void Update(PO_INFO objectToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
