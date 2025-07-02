using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface ISavingTrxService : IServiceBase<SavingTrx>
    {
        // IEnumerable<ValidationResult> IsValidOffice(Office office);

        IEnumerable<SavingTrx> GetSavingTrxList(int CenterID, long MemberID, int LoanTerm, int ProductID, string Option);

        IEnumerable<SavingTrx> GetSavingImatureList(int CenterID, long MemberID, int LoanTerm, int ProductID, string Option, int SaveYesNO);



    }
    public class SavingTrxService : ISavingTrxService
    {
        private readonly ISavingTrxRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public SavingTrxService(ISavingTrxRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<SavingTrx> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == 1).OrderBy(c => c.SavingTrxID);
            return entities;
        }

        public SavingTrx GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }


        public SavingTrx Create(SavingTrx objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(SavingTrx objectToUpdate)
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
                obj.IsActive = 0;
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
                if (isActive == 1)
                {
                    return false;
                }
            }

            return true;
        }





        public IEnumerable<SavingTrx> GetSavingTrxList(int CenterID, long MemberID, int NoOfAccount, int ProductID, string Option)
        {
            return repository.GetSavingTrx(CenterID, MemberID, ProductID, NoOfAccount, Option);
            // GetLoanTrx(int OfficeID, long MemberID, int ProductID, int LoanTerm)
        }
        public IEnumerable<SavingTrx> GetSavingImatureList(int CenterID, long MemberID, int NoOfAccount, int ProductID, string Option, int SaveYesNO)
        {
            return repository.GetSavingImatureList(CenterID, MemberID, ProductID, NoOfAccount, Option, SaveYesNO);
            // GetLoanTrx(int OfficeID, long MemberID, int ProductID, int LoanTerm)
        }







        public SavingTrx GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<SavingTrx> GetMany(Expression<Func<SavingTrx, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
    
}
