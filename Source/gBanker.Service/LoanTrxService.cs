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
using System.Data.SqlClient;

namespace gBanker.Service
{
    public interface ILoanTrxService : IServiceBase<LoanTrx>
    {
        IEnumerable<getTodaysTransaction_Result> GetHistory(int? OrgID, int? officeID, DateTime? tranDateFrom, DateTime? tranDateTo, int? centerIdFrom, long? memberID, string productType, string TransType);


        IEnumerable<getTodaysTransaction_Result> GetHistory(int? OrgID, int? officeID, DateTime? tranDateFrom, DateTime? tranDateTo, int? centerIdFrom, long? memberID, string productType);
        IEnumerable<getWriteOffList_Result> GetwriteOfList(int? OrgID, int? officeID, Nullable<long> memberID, Nullable<System.DateTime> trandate, int? writeoffyear);
        IEnumerable<getWriteOffList_Result> getWriteOffDeclarationList(int? OrgID, int? officeID, Nullable<long> memberID, Nullable<System.DateTime> trandate);
        IEnumerable<Proc_GetZeroBalance_Result> GeZeroBalanceList(int? officeID, int? officeIDTo, Nullable<long> memberID, int? loanStatusType);
        IEnumerable<LoanTrx> GetLoanTrxList(int CenterID, long MemberID, int LoanTerm, int ProductID, string Option);
    }
    public class LoanTrxService : ILoanTrxService
    {
        private readonly ILoanTrxRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public object DataContext { get; private set; }

        public LoanTrxService(ILoanTrxRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            
        }
        public IEnumerable<LoanTrx> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.LoanTrxID);
            return entities;
        }

        public LoanTrx GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public LoanTrx Create(LoanTrx objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(LoanTrx objectToUpdate)
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

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.InActiveDate = inactiveDate.HasValue ? inactiveDate : DateTime.Now;
                obj.IsActive = false;
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }

        public bool IsContinued(long id)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                var isActive = obj.IsActive;
                if (isActive == true)
                {
                    return false;
                }
            }

            return true;
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public IEnumerable<getTodaysTransaction_Result> GetHistory(int? OrgID, int? officeID, DateTime? tranDateFrom, DateTime? tranDateTo, int? centerIdFrom, long? memberID, string productType)
        {
            return repository.GetHistory(OrgID,officeID, tranDateFrom, tranDateTo,centerIdFrom, memberID, productType);
        }


        public IEnumerable<getWriteOffList_Result> GetwriteOfList(int? OrgID, int? officeID, long? memberID, DateTime? trandate,int? writeoffyear)
        {
            return repository.GetwriteOfList(OrgID,officeID, memberID, trandate,writeoffyear);
        }


        public IEnumerable<getWriteOffList_Result> getWriteOffDeclarationList(int? OrgID, int? officeID, long? memberID, DateTime? trandate)
        {
            return repository.getWriteOffDeclarationList(OrgID,officeID, memberID, trandate);
        }


        public LoanTrx GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }


        public IEnumerable<Proc_GetZeroBalance_Result> GeZeroBalanceList(int? officeID, int? officeIDTo, long? memberID, int? loanStatusType)
        {
            return repository.GeZeroBalanceList(officeID, officeIDTo, memberID, loanStatusType);
        }

        public IEnumerable<LoanTrx> GetMany(Expression<Func<LoanTrx, bool>> where)
        {
            throw new NotImplementedException();
        }

       
        public IEnumerable<LoanTrx> GetLoanTrxList(int CenterID, long MemberID, int LoanTerm, int ProductID, string Option)
        {
            return repository.GetLoanTrx(CenterID, MemberID, ProductID, LoanTerm, Option);
            // GetLoanTrx(int OfficeID, long MemberID, int ProductID, int LoanTerm)
        }


        public IEnumerable<getTodaysTransaction_Result> GetHistory(int? OrgID, int? officeID, DateTime? tranDateFrom, DateTime? tranDateTo, int? centerIdFrom, long? memberID, string productType, string TransType)
        {
            return repository.GetHistory(OrgID, officeID, tranDateFrom, tranDateTo, centerIdFrom, memberID, productType, TransType);
        }
    }
}
