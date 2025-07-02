using gBanker.Core.Common;
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
    public interface ISpecialSavingCollectionService : IServiceBase<DailySavingTrx>
    {
        int setUpdateSavingBalance(Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID, Nullable<int> noOfAccount, Nullable<decimal> monthlyInt);
        int setDailySavingTrx(Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID, Nullable<int> noOfAccount, Nullable<decimal> dailySavinInstallment, Nullable<decimal> withDrawal, Nullable<System.DateTime> lcl_BusinessDate, string createUser, Nullable<System.DateTime> createDate, Nullable<int> transType);

        IEnumerable<proc_get_SpecialSavingCollection_Result> GetSpecialsavingCollectionDetail(int? officeID, string vday);
        IEnumerable<ValidationResult> IsValidLoan(DailySavingTrx specialSavingCollection);
        IEnumerable<ValidationResult> IsValidLoanSave(DailySavingTrx specialSavingCollection);
        IEnumerable<ValidationResult> IsValidLoanSaveBankInterestUpdate(DailySavingTrx specialSavingCollection);
        int delVoucher(Nullable<int> officeID, Nullable<System.DateTime> businessDate, int? OrgID);
        IEnumerable<proc_get_SpecialSavingCollection_Result> GetSpecialsavingCollectionDetail(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue);
        IEnumerable<proc_get_SpecialSavingCollection_Result> GetsavingCollectionDetailForBankInterest(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue);
        IEnumerable<getSavingLastBalance_Result> getSavingLastBalance(Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID, Nullable<int> noOfAccount, Nullable<decimal> dailySavinInstallment, Nullable<decimal> withDrawal, Nullable<System.DateTime> lcl_BusinessDate, Nullable<int> transType);
        IEnumerable<Proc_get_SavingLastBalance> Proc_getSavingLastBalance(Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, Nullable<int> productID, Nullable<int> noOfAccount, Nullable<decimal> dailySavinInstallment, Nullable<decimal> withDrawal, Nullable<System.DateTime> lcl_BusinessDate, Nullable<int> transType);
        IEnumerable<proc_get_SpecialSavingCollection_Result> GetSpecialsavingCollectionDetailEmpWise(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue, short? EmpID);
        IEnumerable<proc_get_SpecialSavingCollection_Result> GetSpecialsavingCollectionDetailEmpWiseAdjustment(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue, short? EmpID);
        IEnumerable<proc_get_SpecialSavingCollection_Result> GetSpecialsavingCollectionDetailEmpWise_ForDormant(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue, short? EmpID);


    }
    public class SpecialSavingCollectionService: ISpecialSavingCollectionService
    {
        private readonly ISpecialSavingCollectionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        private readonly IMemberRepository memberRepository;
        private readonly ISavingSummaryRepository savingSummaryRepository;

        public SpecialSavingCollectionService(ISpecialSavingCollectionRepository repository, IUnitOfWorkCodeFirst unitOfWork, IMemberRepository memberRepository, ISavingSummaryRepository savingSummaryRepository)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.memberRepository = memberRepository;
            this.savingSummaryRepository = savingSummaryRepository;
        }

        public IEnumerable<proc_get_SpecialSavingCollection_Result> GetSpecialsavingCollectionDetailEmpWise_ForDormant(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue, short? EmpID)
        {
            return repository.GetSpecialsavingCollectionDetailEmpWise_ForDormant(OrgID, officeID, vday, filterColumnName, filterValue, Convert.ToInt16(EmpID));
        }

        public IEnumerable<DailySavingTrx> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.MemberID);
            return entities;
        }

        public DailySavingTrx GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public DailySavingTrx Create(DailySavingTrx objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DailySavingTrx objectToUpdate)
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
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public IEnumerable<proc_get_SpecialSavingCollection_Result> GetSpecialsavingCollectionDetail(int? officeID, string vday)
        {
            return repository.GetSpecialsavingCollectionDetail(officeID, vday);
        }


        public IEnumerable<ValidationResult> IsValidLoan(DailySavingTrx specialSavingCollection)
        {

            
            
            var memCheck = memberRepository.Get(p => p.OrgID==specialSavingCollection.OrgID && p.MemberID == specialSavingCollection.MemberID && p.CenterID==specialSavingCollection.CenterID && p.OfficeID == specialSavingCollection.OfficeID && p.IsActive==true);
            if (memCheck == null)
            {
                yield return new ValidationResult("NoOfAccount", "Pls. select valid Member");
            }
            else
            {
                var entityCheck = repository.Get(p => p.OrgID == specialSavingCollection.OrgID && p.OfficeID == specialSavingCollection.OfficeID && p.MemberID == specialSavingCollection.MemberID && p.ProductID == specialSavingCollection.ProductID && p.CenterID == specialSavingCollection.CenterID && p.NoOfAccount == specialSavingCollection.NoOfAccount && p.TransType == specialSavingCollection.TransType);

                if (entityCheck == null)
                {
                    yield return new ValidationResult("NoOfAccount", "Record Not found for selected member");
                }

            }
        }


        public int delVoucher(int? officeID, DateTime? businessDate, int? OrgID)
        {
            return repository.delVoucher(officeID, businessDate,OrgID);
        }


        public IEnumerable<proc_get_SpecialSavingCollection_Result> GetSpecialsavingCollectionDetail(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue)
        {
            return repository.GetSpecialsavingCollectionDetail(OrgID,officeID, vday, filterColumnName, filterValue);
        }


        public IEnumerable<ValidationResult> IsValidLoanSave(DailySavingTrx specialSavingCollection)
        {
            var memCheck = memberRepository.Get(p => p.OrgID==specialSavingCollection.OrgID && p.MemberID == specialSavingCollection.MemberID && p.CenterID == specialSavingCollection.CenterID && p.OfficeID == specialSavingCollection.OfficeID && p.IsActive == true);
            if (memCheck == null)
            {
                yield return new ValidationResult("NoOfAccount", "Pls. select valid Member");
            }
            else
            {
                var entityCheck = savingSummaryRepository.Get(p => p.OrgID == specialSavingCollection.OrgID && p.OfficeID == specialSavingCollection.OfficeID && p.MemberID == specialSavingCollection.MemberID && p.ProductID == specialSavingCollection.ProductID && p.CenterID == specialSavingCollection.CenterID && p.NoOfAccount == specialSavingCollection.NoOfAccount && p.IsActive == true);

                if (entityCheck == null)
                {
                    yield return new ValidationResult("NoOfAccount", "Record Not found for selected member");
                }
                var entityDailyCheck = repository.Get(p => p.OrgID == specialSavingCollection.OrgID && p.OfficeID == specialSavingCollection.OfficeID && p.MemberID == specialSavingCollection.MemberID && p.ProductID == specialSavingCollection.ProductID && p.CenterID == specialSavingCollection.CenterID && p.NoOfAccount == specialSavingCollection.NoOfAccount && p.TransType == specialSavingCollection.TransType);

                if (entityDailyCheck != null)
                {
                    yield return new ValidationResult("NoOfAccount", "Record Already Exists");
                }
            }
        }


        public DailySavingTrx GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }


        public IEnumerable<proc_get_SpecialSavingCollection_Result> GetsavingCollectionDetailForBankInterest(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue)
        {
            return repository.GetSpecialsavingCollectionDetail(OrgID, officeID, vday, filterColumnName, filterValue);
        }

        public int setDailySavingTrx(int? officeID, int? centerID, long? memberID, int? productID, int? noOfAccount, decimal? dailySavinInstallment, decimal? withDrawal, DateTime? lcl_BusinessDate, string createUser, DateTime? createDate, int? transType)
        {
            return repository.setDailySavingTrx(officeID,centerID,memberID,productID,noOfAccount,dailySavinInstallment,withDrawal,lcl_BusinessDate,createUser,createDate,transType);
        }


        public IEnumerable<getSavingLastBalance_Result> getSavingLastBalance(int? officeID, int? centerID, long? memberID, int? productID, int? noOfAccount, decimal? dailySavinInstallment, decimal? withDrawal, DateTime? lcl_BusinessDate, int? transType)
        {
            return repository.getSavingLastBalance(officeID, centerID, memberID, productID, noOfAccount, dailySavinInstallment, withDrawal, lcl_BusinessDate, transType);
        }

        public int setUpdateSavingBalance(int? officeID, int? centerID, long? memberID, int? productID, int? noOfAccount, decimal? monthlyInt)
        {
            return repository.setUpdateSavingBalance(officeID, centerID, memberID, productID, noOfAccount, monthlyInt);
        }


        public IEnumerable<Proc_get_SavingLastBalance> Proc_getSavingLastBalance(int? officeID, int? centerID, long? memberID, int? productID, int? noOfAccount, decimal? dailySavinInstallment, decimal? withDrawal, DateTime? lcl_BusinessDate, int? transType)
        {
            return repository.Proc_getSavingLastBalance(officeID, centerID, memberID, productID, noOfAccount, dailySavinInstallment, withDrawal, lcl_BusinessDate, transType);

        }


        public IEnumerable<proc_get_SpecialSavingCollection_Result> GetSpecialsavingCollectionDetailEmpWise(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue, short? EmpID)
        {
            return repository.GetSpecialsavingCollectionDetailEmpWise(OrgID, officeID, vday, filterColumnName, filterValue,Convert.ToInt16(EmpID));
        }

        public IEnumerable<DailySavingTrx> GetMany(Expression<Func<DailySavingTrx, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ValidationResult> IsValidLoanSaveBankInterestUpdate(DailySavingTrx specialSavingCollection)
        {
            var memCheck = memberRepository.Get(p => p.OrgID == specialSavingCollection.OrgID && p.MemberID == specialSavingCollection.MemberID && p.CenterID == specialSavingCollection.CenterID && p.OfficeID == specialSavingCollection.OfficeID && p.IsActive == true);
            if (memCheck == null)
            {
                yield return new ValidationResult("NoOfAccount", "Pls. select valid Member");
            }
            else
            {
                var entityCheck = savingSummaryRepository.Get(p => p.OrgID == specialSavingCollection.OrgID && p.OfficeID == specialSavingCollection.OfficeID && p.MemberID == specialSavingCollection.MemberID && p.ProductID == specialSavingCollection.ProductID && p.CenterID == specialSavingCollection.CenterID && p.NoOfAccount == specialSavingCollection.NoOfAccount && p.IsActive == true);

                if (entityCheck == null)
                {
                    yield return new ValidationResult("NoOfAccount", "Record Not found for selected member");
                }
               
               
            }
        }

        public IEnumerable<proc_get_SpecialSavingCollection_Result> GetSpecialsavingCollectionDetailEmpWiseAdjustment(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue, short? EmpID)
        {
            return repository.GetSpecialsavingCollectionDetailEmpWiseAdjustment(OrgID, officeID, vday, filterColumnName, filterValue, Convert.ToInt16(EmpID));
        }
    }
}
