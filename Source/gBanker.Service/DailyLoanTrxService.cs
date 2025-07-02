using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface IDailyLoanTrxService : IServiceBase<DailyLoanTrx>
    {
        //IEnumerable<DailyLoanTrx> Get_LoanSummaryInfo(Nullable<int> orgID, Nullable<int> officeID, Nullable<int> centerId, Nullable<long> memberID, Nullable<int> productID, Nullable<int> loanterm);
        IEnumerable<DailyLoanTrx> GetDailyLoanTrxDetailPaged(string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount, int? OrgID, int? OfficeID);
        DailyLoanTrx IsDataExistByDate(DateTime date);
    }
    public class DailyLoanTrxService : IDailyLoanTrxService
    {
        private readonly IDailyLoanTrxRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public DailyLoanTrxService(IDailyLoanTrxRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<DailyLoanTrx> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.MemberID);
            return entities;
        }

        public DailyLoanTrx GetById(int id)
        {
            //throw new NotImplementedException();
            var entity = repository.GetById(id);
            return entity;
        }

        public DailyLoanTrx Create(DailyLoanTrx objectToCreate)
        {
            //throw new NotImplementedException();
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DailyLoanTrx objectToUpdate)
        {
            //throw new NotImplementedException();
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            //throw new NotImplementedException();
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
        public IEnumerable<DailyLoanTrx> GetDailyLoanTrxDetailPaged(string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount, int? OrgID, int? OfficeID)
        {
            return repository.GetDailyLoanTrxDetailPaged(filterColumnName, filterValue, startRowIndex, pageSize, out totalCount, OrgID, OfficeID);
        }

        public DailyLoanTrx GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<DailyLoanTrx> GetMany(Expression<Func<DailyLoanTrx, bool>> where)
        {
            throw new NotImplementedException();
        }
        //public IEnumerable<DailyLoanTrx> Get_LoanSummaryInfo(int? officeID, int? centerId, long? memberID, int? productID, int? loanterm)
        //{
        //    return repository.GetAll();

        //}
        public DailyLoanTrx IsDataExistByDate(DateTime date)
        {
            var entity = repository.IsDataExistByDate(date);
            return entity;
        }
        
    }
}