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
    public interface ICreditScoreService : IServiceBase<CreditScore>
    {
        int usp_rpt_credit_score(int? officeID, Nullable<System.DateTime> start_date, Nullable<System.DateTime> end_date);
        IEnumerable<GetCreditScore_Result> GetCreditScore(int? officeID);
    }
    public class CreditScoreService : ICreditScoreService
    {
        private readonly ICreditScoreRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public CreditScoreService(ICreditScoreRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<CreditScore> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.MemberID);
            return entities;
        }

        public CreditScore GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public CreditScore Create(CreditScore objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Update(CreditScore objectToUpdate)
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

        public int usp_rpt_credit_score(int? officeID, DateTime? start_date, DateTime? end_date)
        {
            return repository.usp_rpt_credit_score(officeID, start_date, end_date);
        }


        public IEnumerable<GetCreditScore_Result> GetCreditScore(int? officeID)
        {
            return repository.GetCreditScore(officeID);
        }


        public CreditScore GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CreditScore> GetMany(Expression<Func<CreditScore, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
