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
    public interface IMemberLedgerService : IServiceBase<getGetLoanLedgerMemberWise_Result>
    {

        IEnumerable<getGetLoanLedgerMemberWise_Result> getGetLoanLedgerMemberWise(int OrgID, int? OfficeId, int? Memberid);
    }
    public class MemberLedgerService : IMemberLedgerService
    {
        private readonly IMemberLedgerRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public MemberLedgerService(IMemberLedgerRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
           
        }

        public IEnumerable<getGetLoanLedgerMemberWise_Result> getGetLoanLedgerMemberWise(int OrgID, int? OfficeId, int? Memberid)
        {
            return repository.getGetLoanLedgerMemberWise(OrgID,OfficeId, Memberid);
        }

        public IEnumerable<getGetLoanLedgerMemberWise_Result> GetAll()
        {
            throw new NotImplementedException();
        }

        public getGetLoanLedgerMemberWise_Result GetById(int id)
        {
            throw new NotImplementedException();
        }

        public getGetLoanLedgerMemberWise_Result Create(getGetLoanLedgerMemberWise_Result objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Update(getGetLoanLedgerMemberWise_Result objectToUpdate)
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


        public getGetLoanLedgerMemberWise_Result GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<getGetLoanLedgerMemberWise_Result> GetMany(Expression<Func<getGetLoanLedgerMemberWise_Result, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
