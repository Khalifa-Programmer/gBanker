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
    
    public interface ILoanCorrectionService : IServiceBase<LoanCorrectionTrx>
    {
      
        //IEnumerable<DbSpecialLoanCollectionDetailModel> GetSpecialLoanCollectionDetail();
        int getMaxLoanterm(LoanSummary loansummary);
        IEnumerable<proc_get_LoanCorrection_Result> GetLoanCorrectionDetail(int? orgID, int? officeID, DateTime? collectionDate);
        int SpecialCollection(int? officeID, int? centerID, int? productID, int? memberID, int? loanTerm, string collectionDay, DateTime? collectionDate, int? qType, int? transType, decimal? loanPaid, decimal? intPaid);
        int delVoucher(Nullable<int> officeID, Nullable<System.DateTime> businessDate);
        int LoanCorrection(int? orgID, int? officeID, int? centerID, int? productID, int? memberID, int? loanTerm, string collectionDay, DateTime? collectionDate, int? qType, int? transType, Nullable<decimal> loanPaid, Nullable<decimal> intPaid);
        IEnumerable<ValidationResult> IsValidLoan(LoanCorrectionTrx specialCollection);
    }
    public class LoanCorrectionService : ILoanCorrectionService
    {
        private readonly ILoanCorrectionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        private readonly IMemberRepository memberRepository;
        private readonly ILoanSummaryRepository loanSummaryRepository;
        private readonly ISpecialLoanCollectionRepository specialLoanCollectionRepository;
        public LoanCorrectionService(ILoanCorrectionRepository repository, IUnitOfWorkCodeFirst unitOfWork, IMemberRepository memberRepository, ILoanSummaryRepository loanSummaryRepository, ISpecialLoanCollectionRepository specialLoanCollectionRepository)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.memberRepository = memberRepository;
            this.loanSummaryRepository = loanSummaryRepository;
            this.specialLoanCollectionRepository = specialLoanCollectionRepository;
        }



        public IEnumerable<LoanCorrectionTrx> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.MemberID);
            return entities;
        }

        public LoanCorrectionTrx GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public LoanCorrectionTrx Create(LoanCorrectionTrx objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(LoanCorrectionTrx objectToUpdate)
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



        public int getMaxLoanterm(LoanSummary loansummary)
        {

            int loanterm;
            var vMaxLoanLerm = loanSummaryRepository.GetAll().Where(s => s.OfficeID == loansummary.OfficeID && s.MemberID == loansummary.MemberID && s.CenterID == loansummary.CenterID && s.ProductID == loansummary.ProductID && s.LoanStatus == 1 && s.IsActive == true).FirstOrDefault();

            // var vMaxLoanLerm = repository.MaxLoanTerm(loansummary); // repository.GetMany(p => p.OfficeID == loansummary.OfficeID && p.MemberID == loansummary.MemberID && p.ProductID == loansummary.ProductID && p.CenterID == loansummary.CenterID).Max();
            if (vMaxLoanLerm != null)
            {
                loanterm = vMaxLoanLerm.LoanTerm;
            }
            else
            {
                loanterm = 0;
            }

            return loanterm;

        }


     
       

        public int SpecialCollection(int? officeID, int? centerID, int? productID, int? memberID, int? loanTerm, string collectionDay, DateTime? collectionDate, int? qType, int? transType, decimal? loanPaid, decimal? intPaid)
        {
            return repository.SpecialCollection(officeID, centerID, productID, memberID, loanTerm, collectionDay, collectionDate, qType, transType, loanPaid, intPaid);

        }


        public int delVoucher(int? officeID, DateTime? businessDate)
        {
            return repository.delVoucher(officeID, businessDate);
        }


        public int LoanCorrection(int? orgID, int? officeID, int? centerID, int? productID, int? memberID, int? loanTerm, string collectionDay, DateTime? collectionDate, int? qType, int? transType, decimal? loanPaid, decimal? intPaid)
        {
            return repository.LoanCorrection(orgID,officeID, centerID, productID, memberID, loanTerm, collectionDay, collectionDate, qType, transType, loanPaid, intPaid);

        }


        public IEnumerable<proc_get_LoanCorrection_Result> GetLoanCorrectionDetail(int? orgID, int? officeID, DateTime? collectionDate)
        {
            return repository.GetLoanCorrectionDetail(orgID,officeID, collectionDate);
        }


        public IEnumerable<ValidationResult> IsValidLoan(LoanCorrectionTrx specialCollection)
        {
            var vMaxLoanLerm = loanSummaryRepository.GetAll().Where(s => s.OfficeID == specialCollection.OfficeID && s.MemberID == specialCollection.MemberID && s.CenterID == specialCollection.CenterID && s.ProductID == specialCollection.ProductID && s.LoanTerm == specialCollection.LoanTerm && s.LoanStatus == 1 && s.IsActive == true).FirstOrDefault();

            // var vMaxLoanLerm = repository.MaxLoanTerm(loansummary); // repository.GetMany(p => p.OfficeID == loansummary.OfficeID && p.MemberID == loansummary.MemberID && p.ProductID == loansummary.ProductID && p.CenterID == loansummary.CenterID).Max();
            if (vMaxLoanLerm == null)
            {
                yield return new ValidationResult("LoanTerm", "Pls. select valid Member");
            }


            var memCheck = memberRepository.Get(p => p.MemberID == specialCollection.MemberID && p.OfficeID == specialCollection.OfficeID && p.IsActive == true);
            if (memCheck == null)
            {
                yield return new ValidationResult("LoanTerm", "Pls. select valid Member");
            }
            else
            {
                var entityCheck = repository.Get(p => p.OfficeID == specialCollection.OfficeID && p.MemberID == specialCollection.MemberID && p.ProductID == specialCollection.ProductID && p.CenterID == specialCollection.CenterID && p.LoanTerm == specialCollection.LoanTerm && p.TrxType == specialCollection.TrxType);

                if (entityCheck == null)
                {
                    yield return new ValidationResult("LoanTerm", "this member already exists");
                }

            }
        }


        public LoanCorrectionTrx GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<LoanCorrectionTrx> GetMany(Expression<Func<LoanCorrectionTrx, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
