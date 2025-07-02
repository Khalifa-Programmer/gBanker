using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Service
{
    public interface IPKSFFundLoanService : IServiceBase<PKSFFundLoan>
    {

    }
    public class PKSFFundLoanService : IPKSFFundLoanService
    {
        private readonly IPKSFFundLoanRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public PKSFFundLoanService(IPKSFFundLoanRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<PKSFFundLoan> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.FundLoanID);
            return entities;
        }
        public PKSFFundLoan GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public PKSFFundLoan Create(PKSFFundLoan objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(PKSFFundLoan objectToUpdate)
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
            unitOfWork.Commit();
        }


        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException(); ;
        }


        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }


        public PKSFFundLoan Get(Expression<Func<PKSFFundLoan, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<PKSFFundLoan> GetMany(Expression<Func<PKSFFundLoan, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }
        public PKSFFundLoan GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

    }
}
