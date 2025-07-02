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
    public interface IAccTrxDetailService : IServiceBase<AccTrxDetail>
    {
        IEnumerable<AccTrxDetail> SaveDailyTrxDetail(IEnumerable<AccTrxDetail> VoucherTrxDetails);
        IEnumerable<AccTrxDetail> GetByTrxMasterId(long id);
        decimal GetCreditByTrxMasterId(long id);
        decimal GetDebitByTrxMasterId(long id);
    }
    public class AccTrxDetailService : IAccTrxDetailService
    {
        private readonly IAccTrxDetailRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AccTrxDetailService(IAccTrxDetailRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AccTrxDetail> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.TrxDetailsID);
            return entities;
        }

        public AccTrxDetail GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public IEnumerable<AccTrxDetail> GetByTrxMasterId(long id)
        {
            var entities = repository.GetAll().Where(m => m.TrxMasterID == id && m.IsActive==true).OrderBy(c => c.TrxDetailsID);
            return entities;
        }
        public decimal GetCreditByTrxMasterId(long id)
        {
            var cr = repository.GetAll().Where(m => m.TrxMasterID == id && m.IsActive == true);
            decimal cr_value=0;
            //cr_value = cr.Sum(x => x.Credit);
            foreach(var cre in cr)
            {
                cr_value = cr_value + Convert.ToDecimal(cre.Credit); 
            }            
            return cr_value;
        }
        public decimal GetDebitByTrxMasterId(long id)
        {
            var dr = repository.GetAll().Where(m => m.TrxMasterID == id && m.IsActive == true);
            decimal dr_value = 0;
            foreach (var dre in dr)
            {
                dr_value = dr_value + Convert.ToDecimal(dre.Debit);
            }
            return dr_value;
        }

        public AccTrxDetail Create(AccTrxDetail objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }
        public IEnumerable<AccTrxDetail> SaveDailyTrxDetail(IEnumerable<AccTrxDetail> VoucherTrxDetails)
        {

            if (VoucherTrxDetails != null && VoucherTrxDetails.Count() > 0)
            {
                foreach (var detail in VoucherTrxDetails)
                {
                    //var dbLoan = repository.GetById(loan.DailyLoanTrxID);
                    //if (dbLoan != null)
                    //{
                    //    dbLoan.LoanPaid = loan.LoanPaid;
                    //    dbLoan.IntPaid = loan.IntPaid;
                    //    dbLoan.TotalPaid = loan.TotalPaid;
                    //    repository.Update(dbLoan);
                    //}
                    Create(detail);
                }
            }
            //Save();
            return VoucherTrxDetails;
        }

        public void Update(AccTrxDetail objectToUpdate)
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



        public AccTrxDetail GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<AccTrxDetail> GetMany(Expression<Func<AccTrxDetail, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
