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
    public interface IDayEndService : IServiceBase<Prcs_DayEnd_Result>
    {

        int DayEndProcess(int? OfficeId, DateTime? vDate,int? OrgID);
        int PortFOlioYearClosingProcess(int? OfficeID, DateTime? YearEndDate, int? OrgID);
        int PostToLedgerProcess(int? OfficeID, DateTime? TransDate, int? OrgID);
        int PreYearClosingProcess(int? OfficeID, DateTime? YearEndDate);
    }
    public class DayEndService : IDayEndService
    {
        private readonly IDayEndRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public DayEndService(IDayEndRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
           
        }
        public IEnumerable<Prcs_DayInitial_Result> GetAll()
        {
            throw new NotImplementedException();
        }
        public Prcs_DayInitial_Result GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Prcs_DayInitial_Result Create(Prcs_DayInitial_Result objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Update(Prcs_DayInitial_Result objectToUpdate)
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

        public int DayEndProcess(int? OfficeId, DateTime? vDate, int? OrgID)
        {
            return repository.DayEndProcess(OfficeId, vDate,OrgID);
        }

        IEnumerable<Prcs_DayEnd_Result> IServiceBase<Prcs_DayEnd_Result>.GetAll()
        {
            throw new NotImplementedException();
        }

        Prcs_DayEnd_Result IServiceBase<Prcs_DayEnd_Result>.GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Prcs_DayEnd_Result Create(Prcs_DayEnd_Result objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Update(Prcs_DayEnd_Result objectToUpdate)
        {
            throw new NotImplementedException();
        }


        public Prcs_DayEnd_Result GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }


        public int PortFOlioYearClosingProcess(int? OfficeID, DateTime? YearEndDate, int? OrgID)
        {
            return repository.PortFOlioYearClosingProcess(OfficeID, YearEndDate,OrgID);
        }


        public int PostToLedgerProcess(int? OfficeID, DateTime? TransDate, int? OrgID)
        {
            return repository.PostToLedgerProcess(OfficeID, TransDate, OrgID);
        }


        public int PreYearClosingProcess(int? OfficeID, DateTime? YearEndDate)
        {
            return repository.PreYearClosingProcess(OfficeID, YearEndDate);
        }

        public IEnumerable<Prcs_DayEnd_Result> GetMany(Expression<Func<Prcs_DayEnd_Result, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
