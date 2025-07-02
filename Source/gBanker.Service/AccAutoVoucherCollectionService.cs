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
    public interface IAccAutoVoucherCollectionService : IServiceBase<AutoVoucherCollectionResult>
    {

        int AccAutoVoucherCollectionProcess(int? OfficeId, DateTime? vDate, int? OrgID);
    }
   
    public class AccAutoVoucherCollectionService: IAccAutoVoucherCollectionService
    {
        private readonly IAccAutoVoucherCollectionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public AccAutoVoucherCollectionService(IAccAutoVoucherCollectionRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
           
        }

        public int AccAutoVoucherCollectionProcess(int? OfficeId, DateTime? vDate, int? OrgID)
        {
            return repository.AccAutoVoucherCollectionProcess(OfficeId, vDate,OrgID);
        }

        public IEnumerable<AutoVoucherCollectionResult> GetAll()
        {
            throw new NotImplementedException();
        }

        public AutoVoucherCollectionResult GetById(int id)
        {
            throw new NotImplementedException();
        }

        public AutoVoucherCollectionResult Create(AutoVoucherCollectionResult objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Update(AutoVoucherCollectionResult objectToUpdate)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }


        public AutoVoucherCollectionResult GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AutoVoucherCollectionResult> GetMany(Expression<Func<AutoVoucherCollectionResult, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
