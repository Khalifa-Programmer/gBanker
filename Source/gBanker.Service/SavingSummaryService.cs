using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using gBanker.Data.CodeFirstMigration;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface ISavingSummaryService : IServiceBase<SavingSummary>
    {
       // int updateSavingInstallment(Nullable<int> office, Nullable<long> memberID, Nullable<int> prodID, Nullable<int> orgID);
        int updateSavingInstallment(Nullable<int> office, Nullable<long> memberID, Nullable<int> prodID, Nullable<int> orgID, Nullable<int> CenterID, Nullable<System.DateTime> TransactionDate, Nullable<System.DateTime> OpeningDate, Nullable<int> EmployeeId, Nullable<int> MemberCategoryID, string CreateUser);
        int Proc_Set_SavingOpeingWhenMemberEligible(Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID, Nullable<int> noOfAccount, Nullable<System.DateTime> transactionDate, Nullable<decimal> interestRate, Nullable<System.DateTime> openingDate, Nullable<int> employeeId, Nullable<int> memberCategoryID, Nullable<int> orgID, string createUser, Nullable<System.DateTime> createDate);
        IEnumerable<Proc_GetSavingBalanceForCate_Result> GetSavingBalanceForCate(Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID);
 
        IEnumerable<Proc_get_MaxNoOfAccount_Result> Get_MaxNoOfAccount(Nullable<int> officeID, Nullable<int> orgID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID);

        int MaxAccountNo(SavingSummary savingsummary);
        IEnumerable<getSavingCloseAccountInfo_Result> GetSavingAccountCloseInfo(int? OrgID, int? officeID, Nullable<System.DateTime> tranDate);
        IEnumerable<getSavingCloseAccountInfo_Result> GetSavingStopInterestInfo(int? OrgID, int? officeID, Nullable<System.DateTime> tranDate);
        IEnumerable<getSavingCloseAccountInfo_Result> GetClaimableStopInterestInfo(int? OrgID, int? officeID, Nullable<System.DateTime> tranDate);
        IEnumerable<DBSavingSummaryDetails> GetSavingSummaryDetailAccountClose(int? OrgID, int? officeID);
        bool IsValidSaving(SavingSummary savingsummary, out string msg);
        IEnumerable<ValidationResult> IsValidSaving(SavingSummary savingsummary);
        bool IsValidSavingForEdit(SavingSummary savingsummary, out string msg);
        IEnumerable<ValidationResult> IsValidSavingForEdit(SavingSummary savingsummary);
        int SetOpeningSavingEntry(int? orgID, int? officeID);
        IEnumerable<DBSavingSummaryDetails> GetSavingSummaryDetail(int? officeID);
        IEnumerable<DBSavingSummaryDetails> GetSavingSummaryDetail(int? officeID, string filterColumnName, string filterValue);
        IEnumerable<DBSavingSummaryDetails> GetSavingSummaryDetailPaged(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount);
        int AccountClose(Nullable<int> OrgID, Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID, Nullable<int> noAccount, Nullable<System.DateTime> tranDate);
        int StopInterestAccount(Nullable<int> OrgID, Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID, Nullable<int> noAccount, Nullable<System.DateTime> tranDate,Nullable<int> Qtype, Nullable<long> SavingSummaryID,Nullable<int> StopOrClaimable,string CreateUser);
        int ClaimableInterestAccount(Nullable<int> OrgID, Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID, Nullable<int> noAccount, Nullable<System.DateTime> tranDate, Nullable<int> Qtype, Nullable<long> SavingSummaryID, Nullable<int> StopOrClaimable, string CreateUser);
        int StartInterestAccount(Nullable<int> OrgID, Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID, Nullable<int> noAccount, Nullable<System.DateTime> tranDate, Nullable<int> Qtype, Nullable<long> SavingSummaryID, Nullable<int> StopOrClaimable, string CreateUser);

        IEnumerable<DBSavingSummaryDetails> GetSavingSummarySavingInterestUpdate(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount);
        SavingSummary GetBySavingSummaryId(long id);
        SavingSummary GetSingleRow(int OfficeID, short ProductID, int CenterID, long MemberID, int NoOfAccount);
        IEnumerable<DBSavingSummaryDetails> GetSavingReinstate(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount, DateTime DateFromValue, DateTime DateToValue);
        int InsertConsentForm(int? OrgID, int? officeID, int? centerID, long? memberID, int? productID, int? noAccount, DateTime? tranDate, int? Qtype, long? SavingSummaryID, int? StopOrClaimable, string CreateUser);
    }
    public class SavingSummaryService : ISavingSummaryService
    {
        private readonly ISavingSummaryRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        private readonly IProductRepository productRepository;
        private readonly IMemberRepository memberRepository;
        public SavingSummaryService(ISavingSummaryRepository repository, IProductRepository productRepository, IUnitOfWorkCodeFirst unitOfWork, IMemberRepository memberRepository)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.productRepository = productRepository;
            this.memberRepository = memberRepository;
            
        }
        public SavingSummaryService(ISavingSummaryRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
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
        public SavingSummary GetBySavingSummaryId(long id)
        {
            var entity = repository.Get(w => w.SavingSummaryID == id);
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
        public void Save()
        {
            unitOfWork.Commit();
        }
        public bool IsValidSaving(SavingSummary savingsummary, out string msg)
        {
            var validationMsg = "";
            var isValid = true;
            var entity = repository.Get(p => p.OfficeID == savingsummary.OfficeID && p.MemberID == savingsummary.MemberID && p.ProductID == savingsummary.ProductID && p.CenterID == savingsummary.CenterID && p.NoOfAccount == savingsummary.NoOfAccount && p.IsActive == true);
            if (entity != null)
            {
                isValid = false;
                validationMsg = "Duplicate data.";
            }


            var product = productRepository.GetById(savingsummary.ProductID);
            if (product != null)
            {
                if (product.InterestRate.HasValue && product.InterestRate.Value < savingsummary.InterestRate)
                {
                    isValid = false;
                    validationMsg = "InterestRate exceeds.";
                }

            }
            else
                isValid = false;
            msg = validationMsg;
            return isValid;
        }
        public bool IsValidSavingForEdit(SavingSummary savingsummary, out string msg)
        {
            var validationMsg = "";
            var isValid = true;

            var product = productRepository.GetById(savingsummary.ProductID);

            if (product != null)
            {
                if (product.InterestRate.HasValue && product.InterestRate.Value < savingsummary.InterestRate)
                {
                    isValid = false;
                    validationMsg = "savingsummary exceeds.";
                }
                //mansur
               
            }
            else
                isValid = false;
            msg = validationMsg;
            return isValid;
        }
        public int SetOpeningSavingEntry(int? orgID, int? officeID)
        {
            return repository.SetOpeningSavingEntry(orgID,officeID);
        }
        public IEnumerable<DBSavingSummaryDetails> GetSavingSummaryDetail(int? officeID)
        {
            return repository.GetSavingSummaryDetail(officeID);
        }
        public IEnumerable<ValidationResult> IsValidSaving(SavingSummary savingsummary)
        {

            var memEntiry = memberRepository.Get(m => m.OfficeID == savingsummary.OfficeID && m.CenterID == savingsummary.CenterID && m.MemberID == savingsummary.MemberID && m.IsActive == true);
            if (memEntiry == null)
            {
                yield return new ValidationResult("OfficeID", "Invalid Member");
            }

            var entity = repository.Get(p => p.OfficeID == savingsummary.OfficeID && p.MemberID == savingsummary.MemberID && p.ProductID == savingsummary.ProductID  && p.NoOfAccount == savingsummary.NoOfAccount && p.IsActive == true);
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
                yield return new ValidationResult("ProductID", "Invalid Product.");
           
        }
        public int AccountClose(int? OrgID,int? officeID, int? centerID, long? memberID, int? productID, int? noAccount, DateTime? tranDate)
        {
            return repository.AccountClose(OrgID, officeID, centerID, memberID, productID, noAccount, tranDate);
        }
        public IEnumerable<ValidationResult> IsValidSavingForEdit(SavingSummary savingsummary)
        {
          
            var product = productRepository.GetById(savingsummary.ProductID);

            if (product != null)
            {
                if (product.InterestRate.HasValue && product.InterestRate.Value < savingsummary.InterestRate)
                {
                    yield return new ValidationResult("InterestRate", "Interest does not match with product interest exceeds.");
                }
               
            }
            
        }
        public IEnumerable<DBSavingSummaryDetails> GetSavingSummaryDetail(int? officeID, string filterColumnName, string filterValue)
        {
            return repository.GetSavingSummaryDetail(officeID, filterColumnName, filterValue);
        }
        public IEnumerable<DBSavingSummaryDetails> GetSavingSummaryDetailPaged(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount)
        {
            return repository.GetSavingSummaryDetailPaged(officeID, filterColumnName, filterValue, startRowIndex, pageSize, out totalCount);
        }
        public IEnumerable<DBSavingSummaryDetails> GetSavingSummaryDetailAccountClose(int? OrgID,int? officeID)
        {
            return repository.GetSavingSummaryDetailAccountClose(OrgID,officeID);
        }
        public SavingSummary GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public IEnumerable<DBSavingSummaryDetails> GetSavingSummarySavingInterestUpdate(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount)
        {
            return repository.GetSavingSummarySavingInterestUpdate(officeID, filterColumnName, filterValue, startRowIndex, pageSize, out totalCount);
        }
        public IEnumerable<getSavingCloseAccountInfo_Result> GetSavingAccountCloseInfo(int? OrgID, int? officeID,DateTime? tranDate)
        {
            return repository.GetSavingAccountCloseInfo(OrgID, officeID,tranDate);
        }
        public int MaxAccountNo(SavingSummary savingsummary)
        {
            return repository.MaxAccountNo(savingsummary);
        }

        public IEnumerable<Proc_get_MaxNoOfAccount_Result> Get_MaxNoOfAccount(int? officeID, int? orgID, int? centerID, long? memberID, int? productID)
        {
            return repository.Get_MaxNoOfAccount(officeID,orgID,centerID,memberID,productID);
        }

        public IEnumerable<Proc_GetSavingBalanceForCate_Result> GetSavingBalanceForCate(int? officeID, int? centerID, long? memberID, int? productID)
        {
            return repository.GetSavingBalanceForCate(officeID, centerID, memberID, productID);
        }



        public int updateSavingInstallment(int? office, long? memberID, int? prodID, int? orgID, int? CenterID, DateTime? TransactionDate, DateTime? OpeningDate, int? EmployeeId, int? MemberCategoryID, string CreateUser)
        {
            return repository.updateSavingInstallment(office, memberID, prodID, orgID, CenterID, TransactionDate, OpeningDate, EmployeeId, MemberCategoryID, CreateUser);
        }


        public int Proc_Set_SavingOpeingWhenMemberEligible(int? officeID, int? centerID, long? memberID, int? productID, int? noOfAccount, DateTime? transactionDate, decimal? interestRate, DateTime? openingDate, int? employeeId, int? memberCategoryID, int? orgID, string createUser, DateTime? createDate)
        {
            return repository.Proc_Set_SavingOpeingWhenMemberEligible(officeID, centerID, memberID, productID, noOfAccount, transactionDate, interestRate, openingDate, employeeId, memberCategoryID, orgID, createUser, createDate);
        }

        public SavingSummary GetSingleRow(int OfficeID, short ProductID, int CenterID, long MemberID, int NoOfAccount)
        {
            var entity = repository.Get(g => g.OfficeID == OfficeID && g.ProductID == ProductID && g.CenterID == CenterID && g.MemberID == MemberID && g.NoOfAccount == NoOfAccount && g.IsActive == true);
            //var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<SavingSummary> GetMany(Expression<Func<SavingSummary, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<getSavingCloseAccountInfo_Result> GetSavingStopInterestInfo(int? OrgID, int? officeID, DateTime? tranDate)
        {
           
            return repository.GetSavingStopInterestInfo(OrgID, officeID, tranDate);
        
    }

        public int StopInterestAccount(int? OrgID, int? officeID, int? centerID, long? memberID, int? productID, int? noAccount, DateTime? tranDate, int? Qtype,long? SavingSummaryID,int? StopOrClaimable,string  CreateUser)
        {
            return repository.StopInterestAccount(OrgID, officeID, centerID, memberID, productID, noAccount, tranDate,Qtype,SavingSummaryID,StopOrClaimable,CreateUser);
        }
        public int InsertConsentForm(int? OrgID, int? officeID, int? centerID, long? memberID, int? productID, int? noAccount, DateTime? tranDate, int? Qtype, long? SavingSummaryID, int? StopOrClaimable, string CreateUser)
        {
            return repository.InsertConsentForm(OrgID, officeID, centerID, memberID, productID, noAccount, tranDate, Qtype, SavingSummaryID, StopOrClaimable, CreateUser);
        }
        public int StartInterestAccount(int? OrgID, int? officeID, int? centerID, long? memberID, int? productID, int? noAccount, DateTime? tranDate, int? Qtype, long? SavingSummaryID, int? StopOrClaimable, string CreateUser)
        {
            return repository.StopInterestAccount(OrgID, officeID, centerID, memberID, productID, noAccount, tranDate, Qtype,SavingSummaryID,StopOrClaimable,CreateUser);

        }

        public IEnumerable<getSavingCloseAccountInfo_Result> GetClaimableStopInterestInfo(int? OrgID, int? officeID, DateTime? tranDate)
        {
            return repository.GetSavingClaimableInterestInfo(OrgID, officeID, tranDate);

        }

        public int ClaimableInterestAccount(int? OrgID, int? officeID, int? centerID, long? memberID, int? productID, int? noAccount, DateTime? tranDate, int? Qtype, long? SavingSummaryID, int? StopOrClaimable, string CreateUser)
        {
            return repository.ClaimableInterestAccount(OrgID, officeID, centerID, memberID, productID, noAccount, tranDate, Qtype, SavingSummaryID, StopOrClaimable, CreateUser);
        }

        public IEnumerable<DBSavingSummaryDetails> GetSavingReinstate(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount, DateTime DateFromValue, DateTime DateToValue)
        {
            return repository.GetSavingReinstate(officeID, filterColumnName, filterValue, startRowIndex, pageSize, out totalCount,DateFromValue,DateToValue);
        }
    }
}
