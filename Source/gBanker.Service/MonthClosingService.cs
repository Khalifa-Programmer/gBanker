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
    public interface IMonthClosingService : IServiceBase<SavingSummary>
    {

        int MonthlyProcess(int? OfficeId, int? OrgID, DateTime? vDate);
        int MonthlyProcessAverageMethod(int? OfficeId, int? OrgID, DateTime? vDate);
        //Bappa

    }
    public class MonthClosingService: IMonthClosingService
    {
        private readonly IMonthClosingRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public MonthClosingService(IMonthClosingRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
           
        }

        public int MonthlyProcess(int? OfficeId, int? OrgID, DateTime? vDate)
        {
            return repository.MonthlyProcess(OfficeId, OrgID, vDate);
        }

        public int MonthlyProcessAverageMethod(int? OfficeId, int? OrgID, DateTime? vDate)
        {
            return repository.MonthlyProcessAverageMethod(OfficeId,OrgID, vDate);
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


        public SavingSummary GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<SavingSummary> GetMany(Expression<Func<SavingSummary, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
