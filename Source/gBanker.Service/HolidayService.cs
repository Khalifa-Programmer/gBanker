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
    public interface IHolidayService : IServiceBase<Holiday>
    {
        int SetHoliDay(Nullable<int> officeID, Nullable<int> orgID, Nullable<System.DateTime> specificDate, string processType, string holidayDescription, string days, string year, string createUser);

        IEnumerable<ValidationResult> IsValidHoliday(Holiday holiday);
        IEnumerable<Holiday> SearchHoliday();
        IEnumerable<DBholidayDetailModel> GetHolidayDetail(int OrgID);
        //Holiday GetByHolidayCode(DateTime businessDate);  
        IEnumerable<Holiday> SaveYearlyHoliday(IEnumerable<Holiday> YearlyHoliday);
    }
    public class HolidayService : IHolidayService
    {
        private readonly IHolidayRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public HolidayService(IHolidayRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Holiday> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.BusinessDate);
            return entities;
        }

        public Holiday GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        //public Holiday GetByHolidayCode(string OfficeCode)
        //{
        //    var entity = repository.Get(p => p.OfficeCode == OfficeCode);
        //    return entity;
        //}

        public Holiday Create(Holiday objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Holiday objectToUpdate)
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
        public IEnumerable<ValidationResult> IsValidHoliday(Holiday holiday)
        {
            var entity = repository.Get(p => p.BusinessDate == holiday.BusinessDate && p.OfficeID == holiday.OfficeID && p.CenterID == holiday.CenterID);
            if (entity != null)
            {
                yield return new ValidationResult("Holiday", "Duplicate Holiday.");
            }
        }


        public IEnumerable<Holiday> SearchHoliday()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.BusinessDate);
        }

        public IEnumerable<DBholidayDetailModel> GetHolidayDetail(int OrgID)
        {
            return repository.GetHolidayDetail(OrgID);
        }

        public IEnumerable<Holiday> SaveYearlyHoliday(IEnumerable<Holiday> YearlyHoliday)
        {

            if (YearlyHoliday != null && YearlyHoliday.Count() > 0)
            {
                foreach (var detail in YearlyHoliday)
                {
                    //var dbLoan = repository.GetById(loan.DailyLoanTrxID);
                    //if (dbLoan != null)
                    //{
                    //    dbLoan.LoanPaid = loan.LoanPaid;
                    //    dbLoan.IntPaid = loan.IntPaid;
                    //    dbLoan.TotalPaid = loan.TotalPaid;
                    //    repository.Update(dbLoan);
                    //}
                    repository.Add(detail);
                    Save();
                }
            }
            //Save();
            return YearlyHoliday;
        }


        public Holiday GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public int SetHoliDay(int? officeID, int? orgID, DateTime? specificDate, string processType, string holidayDescription, string days, string year, string createUser)
        {
            return repository.SetHoliDay(officeID, orgID, specificDate, processType, holidayDescription, days, year, createUser);
        }

        public IEnumerable<Holiday> GetMany(Expression<Func<Holiday, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
