using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface ITodaysSummaryService : IServiceBase<Proc_GetRptTodaySummary_Result>
    {
        int GetTodaysSummary(int? qtype, int? officeID, DateTime? vdate);
       
    }
    public class TodaysSummaryService: ITodaysSummaryService
    {
        private readonly ITodaysSummaryRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public TodaysSummaryService(ITodaysSummaryRepository repository,  IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            
        }

       

        public IEnumerable<Proc_GetRptTodaySummary_Result> GetAll()
        {
            throw new NotImplementedException();
        }

        public Proc_GetRptTodaySummary_Result GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Proc_GetRptTodaySummary_Result Create(Proc_GetRptTodaySummary_Result objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Update(Proc_GetRptTodaySummary_Result objectToUpdate)
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

        public int GetTodaysSummary(int? qtype, int? officeID, DateTime? vdate)
        {
            return repository.GetTodaysSummary(1, officeID, vdate);
        }


        public Proc_GetRptTodaySummary_Result GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Proc_GetRptTodaySummary_Result> GetMany(Expression<Func<Proc_GetRptTodaySummary_Result, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
