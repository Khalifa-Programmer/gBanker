using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration;
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
    public interface ISavingsAccountOpeningService : IServiceBase<SavingSummary>
    {
        IEnumerable<DBSavingSummaryDetails> GetSavingSummaryDetail(int? OrgID, int? officeID);
        IEnumerable<ValidationResult> IsValidSaving(SavingSummary savingsummary);
        IEnumerable<ValidationResult> IsValidSavingEdit(SavingSummary savingsummary);
        IEnumerable<ValidationResult> IsValidSavingDelete(SavingSummary savingsummary);
        
    }
    public class SavingsAccountOpeningService: ISavingsAccountOpeningService
    {
        private readonly ISavingsAccountOpeningRepository repository;
        private readonly ISavingTrxRepository savingTrxrepository;
        private readonly IMemberRepository memberrepository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        private readonly IProductRepository productRepository;
        private readonly IDailySavingTrxRepository DailySavingTrxRepository;
        public SavingsAccountOpeningService(ISavingsAccountOpeningRepository repository, IUnitOfWorkCodeFirst unitOfWork, IMemberRepository memberrepository, IProductRepository productRepository,ISavingTrxRepository savingTrxrepository,IDailySavingTrxRepository DailySavingTrxRepository)
        {
            this.repository = repository;
            this.memberrepository = memberrepository;
            this.productRepository = productRepository;
            this.savingTrxrepository = savingTrxrepository;
            this.unitOfWork = unitOfWork;
            this.DailySavingTrxRepository = DailySavingTrxRepository;

        }

        public IEnumerable<SavingSummary> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.MemberID);
            return entities;
        }

        public SavingSummary GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public SavingSummary Create(SavingSummary objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(SavingSummary objectToUpdate)
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

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.InActiveDate = inactiveDate.HasValue ? inactiveDate : DateTime.Now;
                obj.IsActive = true;
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

        public void Save()
        {
            unitOfWork.Commit();
        }

        public IEnumerable<DBSavingSummaryDetails> GetSavingSummaryDetail(int? OrgID, int? officeID)
        {
            return repository.GetSavingSummaryDetail(OrgID,officeID);
        }


        public IEnumerable<ValidationResult> IsValidSaving(SavingSummary savingsummary)
        {
           // var entity = repository.Get(p => p.OfficeID == savingsummary.OfficeID && p.MemberID == savingsummary.MemberID && p.ProductID == savingsummary.ProductID && p.CenterID == savingsummary.CenterID && p.NoOfAccount == savingsummary.NoOfAccount && p.IsActive == true && p.SavingStatus == 1);
            var entity = repository.Get(p => p.OrgID==savingsummary.OrgID && p.OfficeID == savingsummary.OfficeID && p.MemberID == savingsummary.MemberID && p.ProductID == savingsummary.ProductID  && p.NoOfAccount == savingsummary.NoOfAccount && p.IsActive == true && p.SavingStatus == 1);
           
            
            if (entity != null)
            {
                yield return new ValidationResult("OfficeID", "Duplicate Record");
            }

            entity = repository.Get(p => p.OrgID == savingsummary.OrgID && p.OfficeID == savingsummary.OfficeID && p.MemberID == savingsummary.MemberID && p.ProductID == savingsummary.ProductID && p.NoOfAccount==savingsummary.NoOfAccount && p.IsActive == true && p.SavingStatus == 1);
            if (entity != null)
            {
               
                
                yield return new ValidationResult("OfficeID", "Duplicate Record");
            }

            var product = productRepository.GetById(savingsummary.ProductID);
            if (product != null)
            {

                if (product.InterestRate.HasValue && product.InterestRate.Value != savingsummary.InterestRate)
                    yield return new ValidationResult("InterestRate", "Interest does not match with product interest exceeds.");
            }
            else
                yield return new ValidationResult("ProductID", "Invalid Product Id");
        }







        public IEnumerable<ValidationResult> IsValidSavingEdit(SavingSummary savingsummary)
        {
            var entity = repository.Get(p =>p.OrgID==savingsummary.OrgID && p.OfficeID == savingsummary.OfficeID && p.MemberID == savingsummary.MemberID && p.ProductID == savingsummary.ProductID && p.NoOfAccount == savingsummary.NoOfAccount && p.IsActive == true && p.SavingStatus == 1);

           // var entity = repository.Get(p => p.OfficeID == savingsummary.OfficeID && p.MemberID == savingsummary.MemberID && p.ProductID == savingsummary.ProductID && p.CenterID == savingsummary.CenterID && p.NoOfAccount == savingsummary.NoOfAccount && p.IsActive == true && p.SavingStatus == 1);
            if (entity != null)
            {
                //var entityTrx = savingTrxrepository.Get(p => p.OfficeID == savingsummary.OfficeID && p.MemberID == savingsummary.MemberID && p.ProductID == savingsummary.ProductID && p.NoOfAccount == savingsummary.NoOfAccount);
                var entityTrx = savingTrxrepository.Get(p => p.OrgID == savingsummary.OrgID && p.OfficeID == savingsummary.OfficeID && p.MemberID == savingsummary.MemberID && p.ProductID == savingsummary.ProductID && p.NoOfAccount == savingsummary.NoOfAccount && p.IsActive == 1);
            
                if (entityTrx != null)
                {
                    yield return new ValidationResult("NoOfAccount", "Invalid Record");
                }
                else
                {
                    var product = productRepository.GetById(savingsummary.ProductID);
                    if (product != null)
                    {

                        if (product.InterestRate.HasValue && product.InterestRate.Value != savingsummary.InterestRate)
                            yield return new ValidationResult("InterestRate", "Interest does not match with product interest exceeds.");
                    }        
                }
                 
            }
            else

                yield return new ValidationResult("NoOfAccount", "Invalid Record");
           
        }


        public IEnumerable<ValidationResult> IsValidSavingDelete(SavingSummary savingsummary)
        {
            var entityTrx = savingTrxrepository.Get(p => p.OrgID == savingsummary.OrgID && p.OfficeID == savingsummary.OfficeID && p.MemberID == savingsummary.MemberID && p.ProductID == savingsummary.ProductID && p.CenterID == savingsummary.CenterID && p.NoOfAccount == savingsummary.NoOfAccount);
            if (entityTrx != null)
            {
                yield return new ValidationResult("NoOfAccount", "Invalid Record for delete");
            }

            var entityDaTrx = DailySavingTrxRepository.Get(p => p.OrgID == savingsummary.OrgID && p.OfficeID == savingsummary.OfficeID && p.MemberID == savingsummary.MemberID && p.ProductID == savingsummary.ProductID && p.CenterID == savingsummary.CenterID && p.NoOfAccount == savingsummary.NoOfAccount);
            if (entityDaTrx != null)
            {
                yield return new ValidationResult("NoOfAccount", "Invalid Record for delete");
            }



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
