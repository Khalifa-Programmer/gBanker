using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
//using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface IInvestorService : IServiceBase<Investor>
    {
        IEnumerable<Investor> SearchInvestor(int? OrgID);
        void conditionalUpdate(Investor investor);
        //bool IsContinued(int id);
        bool IsValidInvestor(Investor investor, out string msg);
        IEnumerable<ValidationResult> IsValidInvestor(Investor investor);
    }
    public class InvestorService : IInvestorService
    {
        private readonly IInvestorRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public InvestorService(IInvestorRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Investor> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.InvestorName);
            return entities;
        }

        public Investor GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Investor Create(Investor objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Investor objectToUpdate)
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




        public IEnumerable<Investor> SearchInvestor(int? OrgID)
        {
            //return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.InvestorID);
            return repository.GetMany(g => g.IsActive == true && g.OrgID==OrgID).OrderBy(o => o.InvestorCode);
        }


      
        
       
        public void conditionalUpdate(Investor investor)
        {
            repository.Update(investor);
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
                if (isActive == false)
                {
                    return false;
                }
            }

            return true;
        }


        public bool IsValidInvestor(Investor investor, out string msg)
        {
            var validationMsg = "";
            var isValid = true;

            var entity = repository.Get(p => p.InvestorCode == investor.InvestorCode || p.InvestorName == investor.InvestorName);
            if (entity != null)
            {
                isValid = false;
                validationMsg = "Duplicate data.";
            }

            else
                isValid = true;
            msg = validationMsg;
            return isValid;
        }


        public IEnumerable<ValidationResult> IsValidInvestor(Investor investor)
        {
            var entity = repository.Get(p => p.InvestorCode == investor.InvestorCode);

            if (entity != null)
            {

                yield return new ValidationResult("InvestorCode", "Duplicate Investor.");

            }
        }


        public Investor GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<Investor> GetMany(Expression<Func<Investor, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
