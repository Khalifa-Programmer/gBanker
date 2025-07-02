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
    public interface ISavingCorrectionService : IServiceBase<SavingCorrectionTrx>
    {
        IEnumerable<getSavingsCorrection_Result> GetSavingCorrectionDetail(int? OrgID, int? officeID, DateTime? collectionDate);
        IEnumerable<proc_get_SpecialSavingCollection_Result> GetSpecialsavingCollectionDetail(int? officeID, string vday);
        IEnumerable<ValidationResult> IsValidLoan(SavingCorrectionTrx specialSavingCollection);
        int delVoucher(Nullable<int> officeID, Nullable<System.DateTime> businessDate);
        int savingCorrection(int? OrgID, int? officeID, int? centerID, long? memberID, int? productID, int? NoAccount, int? transType, Nullable<decimal> SavingInstallment, Nullable<decimal> withdrawal, Nullable<decimal> Penalty, DateTime? Transdate);

    }
    public class SavingCorrectionService : ISavingCorrectionService
    {
        private readonly ISpecialSavingCollectionRepository specialSavingCollectionrepository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        private readonly IMemberRepository memberRepository;
        private readonly ISavingSummaryRepository savingSummaryRepository;
        private readonly ISavingCorrectionRepository repository;
        public SavingCorrectionService(ISavingCorrectionRepository repository, IUnitOfWorkCodeFirst unitOfWork, IMemberRepository memberRepository, ISavingSummaryRepository savingSummaryRepository, ISpecialSavingCollectionRepository specialSavingCollectionrepository)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.memberRepository = memberRepository;
            this.savingSummaryRepository = savingSummaryRepository;
            this.specialSavingCollectionrepository = specialSavingCollectionrepository;
        }

        public IEnumerable<SavingCorrectionTrx> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.MemberID);
            return entities;
        }

        public SavingCorrectionTrx GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public SavingCorrectionTrx Create(SavingCorrectionTrx objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(SavingCorrectionTrx objectToUpdate)
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


        public IEnumerable<ValidationResult> IsValidLoan(SavingCorrectionTrx specialSavingCollection)
        {

            var vMaxLoanLerm = savingSummaryRepository.GetAll().Where(s => s.OfficeID == specialSavingCollection.OfficeID && s.MemberID == specialSavingCollection.MemberID && s.CenterID == specialSavingCollection.CenterID && s.ProductID == specialSavingCollection.ProductID && s.NoOfAccount == specialSavingCollection.NoOfAccount && s.SavingStatus == 1 && s.IsActive==true).FirstOrDefault();

            // var vMaxLoanLerm = repository.MaxLoanTerm(loansummary); // repository.GetMany(p => p.OfficeID == loansummary.OfficeID && p.MemberID == loansummary.MemberID && p.ProductID == loansummary.ProductID && p.CenterID == loansummary.CenterID).Max();
            if (vMaxLoanLerm == null)
            {
                yield return new ValidationResult("LoanTerm", "Pls. select valid Member");
            }
            
            var memCheck = memberRepository.Get(p => p.MemberID == specialSavingCollection.MemberID && p.OfficeID == specialSavingCollection.OfficeID && p.IsActive==true);
            if (memCheck == null)
            {
                yield return new ValidationResult("NoOfAccount", "Pls. select valid Member");
            }
            else
            {
                var entityCheck = repository.Get(p => p.OfficeID == specialSavingCollection.OfficeID && p.MemberID == specialSavingCollection.MemberID && p.ProductID == specialSavingCollection.ProductID && p.CenterID == specialSavingCollection.CenterID && p.NoOfAccount == specialSavingCollection.NoOfAccount && p.TransType == specialSavingCollection.TransType);

                if (entityCheck == null)
                {
                    yield return new ValidationResult("NoOfAccount", "this member already exists");
                }

            }
        }


        public int delVoucher(int? officeID, DateTime? businessDate)
        {
            return repository.delVoucher(officeID, businessDate);
        }


        public int savingCorrection(int? OrgID, int? officeID, int? centerID, long? memberID, int? productID, int? NoAccount, int? transType, decimal? SavingInstallment, decimal? withdrawal, decimal? Penalty, DateTime? Transdate)
        {
            return repository.savingCorrection(OrgID,officeID, centerID, memberID, productID, NoAccount, transType, SavingInstallment, withdrawal, Penalty, Transdate);

        }

        public IEnumerable<getSavingsCorrection_Result> GetSavingCorrectionDetail(int? OrgID, int? officeID, DateTime? collectionDate)
        {
            return repository.GetSavingCorrectionDetail(OrgID,officeID, collectionDate);
        }


        public SavingCorrectionTrx GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<SavingCorrectionTrx> GetMany(Expression<Func<SavingCorrectionTrx, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
