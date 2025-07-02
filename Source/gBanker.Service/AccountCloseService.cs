using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
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
    public interface IAccountCloseService : IServiceBase<SavingSummary>
    {
        IEnumerable<get_lastDayendDate_Result> getLastDayEndDate(Nullable<int> officeID);
        IEnumerable<ValidationResult> IsValidAccountClose(SavingSummary savingsummary);
        int AccountClose(Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID, Nullable<int> noAccount, Nullable<System.DateTime> tranDate);
    }
    public class AccountCloseService: IAccountCloseService
    {
        private readonly IAccountCloseRepository repository;
        private readonly ISavingSummaryRepository savingsummaryrepository;
        private readonly ILoanSummaryRepository loansummaryrepository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public AccountCloseService(IAccountCloseRepository repository, IUnitOfWorkCodeFirst unitOfWork, ISavingSummaryRepository savingsummaryrepository, ILoanSummaryRepository loansummaryrepository)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.savingsummaryrepository = savingsummaryrepository;
            this.loansummaryrepository = loansummaryrepository;
        }

        public IEnumerable<SavingSummary> GetAll()
        {
            throw new NotImplementedException();
        }

        public SavingSummary GetById(int id)
        {
            throw new NotImplementedException();
        }

        public SavingSummary Create(SavingSummary objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Update(SavingSummary objectToUpdate)
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

        public IEnumerable<ValidationResult> IsValidAccountClose(SavingSummary savingsummary)
        {
            var loanentityCheck = loansummaryrepository.Get(p => p.OfficeID == savingsummary.OfficeID && p.MemberID == savingsummary.MemberID && p.CenterID == savingsummary.CenterID);
            if (loanentityCheck == null)
            { 
                                
            }
            return null;
        }


        public int AccountClose(int? officeID, int? centerID, long? memberID, int? productID, int? noAccount, DateTime? tranDate)
        {
            return repository.AccountClose(officeID, centerID, memberID, productID, noAccount, tranDate);
        }


        public SavingSummary GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<get_lastDayendDate_Result> getLastDayEndDate(int? officeID)
        {
            return repository.getLastDayEndDate(officeID);
        }

        public IEnumerable<SavingSummary> GetMany(Expression<Func<SavingSummary, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
