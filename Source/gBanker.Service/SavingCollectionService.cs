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
    public interface ISavingCollectionService : IServiceBase<DailySavingTrx>
    {
        IEnumerable<getDailySavingProduct_Result> getDailySavingProduct(Nullable<int> officeId, Nullable<int> orgId);
        IEnumerable<DailySavingTrx> GetDailySavingCollectionByCenterMember(int centerId, int productId, long memberid);
        IEnumerable<DailySavingTrx> GetDailySavingBankInterestCollectionByCenter(int centerID, string filterColumnName, string filterValue);
        IEnumerable<DailySavingTrx> GetDailySavingCollectionByCenter( int centerID, string filterColumnName, string filterValue);
        IEnumerable<DailySavingTrx> GetDailySavingCollectionByCenterForMonthly(int centerID, string filterColumnName, string filterValue);
        IEnumerable<DailySavingTrx> GetDailySavingCollectionByCenter(int centerId);
        IQueryable<DailySavingTrx> GetDailySavingCollectionByCenterQueryableForMonthly(int centerID, string filterColumnName, string filterValue);
        IEnumerable<DailySavingTrx> SaveDailysavingCollection(IEnumerable<DailySavingTrx> savingTrxCollection);
        IEnumerable<ValidationResult> IsValidLoan(DailySavingTrx loanCollection);
        int delVoucher(Nullable<int> officeID, Nullable<System.DateTime> businessDate, int? OrgID);
        IQueryable<DailySavingTrx> GetDailySavingCollectionByCenterQueryable(int centerID, string filterColumnName, string filterValue);
        IEnumerable<DailySavingTrx> SaveMonthlyInterestCollection(IEnumerable<DailySavingTrx> savingTrxCollection);
        IQueryable<DailySavingTrx> GetDailySavingBankInterestCollectionByCenterQueryable(int centerID, string filterColumnName, string filterValue);

    }
    public class SavingCollectionService : ISavingCollectionService
    {
        private readonly ISavingCollectionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        private readonly IMemberRepository memberRepository;
        private readonly ISavingSummaryRepository savingsummaryRepository;

        public SavingCollectionService(ISavingCollectionRepository repository, IUnitOfWorkCodeFirst unitOfWork, IMemberRepository memberRepository, ISavingSummaryRepository savingsummaryRepository)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.memberRepository = memberRepository;
            this.savingsummaryRepository = savingsummaryRepository;
        }

        public IEnumerable<DailySavingTrx> GetDailySavingCollectionByCenter( int centerID, string filterColumnName, string filterValue)
        {
            
            if (filterColumnName == "MemberCode")
                return repository.GetMany(tr => tr.CenterID == centerID && (tr.TransType == 10 || tr.TransType == 11) && tr.MemberCode == filterValue).OrderBy(tr => tr.ProductCode);
            
            else if (filterColumnName == "ProductCode")
                return repository.GetMany(tr => tr.CenterID == centerID && (tr.TransType == 10 || tr.TransType == 11) && tr.ProductCode == filterValue).OrderBy(tr => tr.ProductCode);


            else

                return repository.GetMany(tr => tr.CenterID == centerID && (tr.TransType == 10 || tr.TransType == 11)).OrderBy(tr => tr.ProductCode);
           
        }
        public IEnumerable<DailySavingTrx> GetDailySavingCollectionByCenterForMonthly(int centerID, string filterColumnName, string filterValue)
        {

            if (filterColumnName == "MemberCode")
                return repository.GetMany(tr => tr.CenterID == centerID && (tr.TransType == 20 ) && (tr.PaymentFrequency == "M") && tr.MemberCode == filterValue).OrderBy(tr => tr.ProductCode);

            else if (filterColumnName == "ProductCode")
                return repository.GetMany(tr => tr.CenterID == centerID && (tr.TransType == 20 ) && (tr.PaymentFrequency == "M") && tr.ProductCode == filterValue).OrderBy(tr => tr.ProductCode);


            else

                return repository.GetMany(tr => tr.CenterID == centerID && (tr.TransType == 20 ) && (tr.PaymentFrequency == "M")).OrderBy(tr => tr.ProductCode);

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


        public IEnumerable<DailySavingTrx> SaveDailysavingCollection(IEnumerable<DailySavingTrx> savingTrxCollection)
        {

            if (savingTrxCollection != null && savingTrxCollection.Count() > 0)
            {
                foreach (var saving in savingTrxCollection)
                {
                    var dbsaving = repository.GetById(saving.DailySavingTrxID);
                    if (dbsaving != null)
                    {
                        if (dbsaving.OrgID == 54 || dbsaving.OrgID==6)
                        {
                            if (dbsaving.CollectionType != "B")
                            {
                                dbsaving.Balance = saving.Balance;
                                dbsaving.SavingInstallment = saving.SavingInstallment;
                                dbsaving.Withdrawal = saving.Withdrawal;
                                dbsaving.Penalty = saving.Penalty;
                                repository.Update(dbsaving);
                            }
                        }
                        else
                        {
                            dbsaving.Balance = saving.Balance;
                            dbsaving.SavingInstallment = saving.SavingInstallment;
                            dbsaving.Withdrawal = saving.Withdrawal;
                            dbsaving.Penalty = saving.Penalty;
                            repository.Update(dbsaving);
                        }
                            
                    }
                }
            }
            Save();
            return savingTrxCollection;
        }


        public IEnumerable<ValidationResult> IsValidLoan(DailySavingTrx loanCollection)
        {

            var vMaxLoanLerm = savingsummaryRepository.GetAll().Where(s => s.OrgID==loanCollection.OrgID && s.OfficeID == loanCollection.OfficeID && s.MemberID == loanCollection.MemberID && s.ProductID == loanCollection.ProductID && s.NoOfAccount == loanCollection.NoOfAccount && s.SavingStatus == 1 && s.IsActive==true).FirstOrDefault();

           // var vMaxLoanLerm = savingsummaryRepository.GetAll().Where(s => s.OfficeID == loanCollection.OfficeID && s.MemberID == loanCollection.MemberID && s.CenterID == loanCollection.CenterID && s.ProductID == loanCollection.ProductID && s.NoOfAccount == loanCollection.NoOfAccount && s.SavingStatus == 1 && s.IsActive == true).FirstOrDefault();

        
            if (vMaxLoanLerm == null)
            {
                yield return new ValidationResult("LoanTerm", "Pls. select valid Member");
            }


            var memCheck = memberRepository.Get(p => p.OrgID==loanCollection.OrgID && p.MemberID == loanCollection.MemberID && p.CenterID==loanCollection.CenterID && p.OfficeID == loanCollection.OfficeID && p.IsActive == true);
            if (memCheck == null)
            {
                yield return new ValidationResult("NoOfAccount", "Pls. select valid Member");
            }
            else
            {
                var entityCheck = repository.Get(p => p.OrgID == loanCollection.OrgID &&  p.OfficeID == loanCollection.OfficeID && p.MemberID == loanCollection.MemberID && p.ProductID == loanCollection.ProductID && p.NoOfAccount == loanCollection.NoOfAccount && p.TransType == 0);

              //  var entityCheck = repository.Get(p => p.OfficeID == loanCollection.OfficeID && p.MemberID == loanCollection.MemberID && p.ProductID == loanCollection.ProductID && p.CenterID == loanCollection.CenterID && p.NoOfAccount == loanCollection.NoOfAccount && p.TransType == 0 );

                if (entityCheck != null)
                {
                    yield return new ValidationResult("NoOfAccount", "this member already exists");
                }

            }
        }


        public int delVoucher(int? officeID, DateTime? businessDate, int? OrgID)
        {
            return repository.delVoucher(officeID, businessDate,OrgID);
        }

        public IEnumerable<DailySavingTrx> GetDailySavingCollectionByCenter(int centerId)
        {
            return repository.GetMany(tr => tr.CenterID == centerId && (tr.TransType == 10 || tr.TransType == 11)).OrderBy(tr => tr.MemberID);
        }


        public DailySavingTrx GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<DailySavingTrx> GetDailySavingCollectionByCenterMember(int centerId, int productId, long memberid)
        {

            IEnumerable<DailySavingTrx> savingtrx = null;
            if (productId > 0 && memberid > 0)
            {
                savingtrx = repository.GetMany(tr => tr.CenterID == centerId && (tr.TransType == 10 || tr.TransType == 11) && tr.ProductID == productId && tr.MemberID == memberid).OrderBy(tr => tr.ProductCode);
            }
            else if (productId == 0 && centerId > 0)
            {
                savingtrx = repository.GetMany(tr => tr.CenterID == centerId && (tr.TransType == 10 || tr.TransType == 11)).OrderBy(tr => tr.ProductCode);
            }
            else

                savingtrx = repository.GetMany(tr => tr.CenterID == centerId && (tr.TransType == 10 || tr.TransType == 11) && tr.MemberID == memberid).OrderBy(tr => tr.ProductCode);

            savingtrx = savingtrx.OrderBy(ord => ord.ProductCode);
            return savingtrx;
        }

        public IEnumerable<getDailySavingProduct_Result> getDailySavingProduct(int? officeId, int? orgId)
        {
            return repository.getDailySavingProduct(orgId, officeId);
        }


        public IQueryable<DailySavingTrx> GetDailySavingCollectionByCenterQueryable(int centerID, string filterColumnName, string filterValue)
        {
            IQueryable<DailySavingTrx> savingtrx = null;

            if (filterColumnName == "MemberCode")
                savingtrx=repository.GetAllQueryable().Where(tr => tr.CenterID == centerID && (tr.TransType == 10 || tr.TransType == 11) && tr.MemberCode == filterValue).OrderBy(tr => tr.ProductCode);

            else if (filterColumnName == "ProductCode")
                savingtrx=repository.GetAllQueryable().Where(tr => tr.CenterID == centerID && (tr.TransType == 10 || tr.TransType == 11) && tr.ProductCode == filterValue).OrderBy(tr => tr.ProductCode);


            else

                savingtrx = repository.GetAllQueryable().Where(tr => tr.CenterID == centerID && (tr.TransType == 10 || tr.TransType == 11)).OrderBy(tr => tr.ProductCode);
            return savingtrx;
        }
        public IQueryable<DailySavingTrx> GetDailySavingCollectionByCenterQueryableForMonthly(int centerID, string filterColumnName, string filterValue)
        {
            IQueryable<DailySavingTrx> savingtrx = null;

            if (filterColumnName == "MemberCode")
                savingtrx = repository.GetAllQueryable().Where(tr => tr.CenterID == centerID && (tr.TransType == 20) && (tr.PaymentFrequency == "M") && tr.MemberCode == filterValue).OrderBy(tr => tr.ProductCode);

            else if (filterColumnName == "ProductCode")
                savingtrx = repository.GetAllQueryable().Where(tr => tr.CenterID == centerID && (tr.TransType == 20) && (tr.PaymentFrequency == "M") && tr.ProductCode == filterValue).OrderBy(tr => tr.ProductCode);


            else

                savingtrx = repository.GetAllQueryable().Where(tr => tr.CenterID == centerID && (tr.TransType == 20) && (tr.PaymentFrequency == "M")).OrderBy(tr => tr.ProductCode);
            return savingtrx;
        }


        public IEnumerable<DailySavingTrx> GetDailySavingBankInterestCollectionByCenter(int centerID, string filterColumnName, string filterValue)
        {

            if (filterColumnName == "MemberCode")
                return repository.GetMany(tr => tr.CenterID == centerID && (tr.TransType == 22 || tr.TransType == 10 || tr.TransType == 11) && tr.MemberCode == filterValue).OrderBy(tr => tr.ProductCode);
            else if (filterColumnName == "ProductCode")
                return repository.GetMany(tr => tr.CenterID == centerID && (tr.TransType == 22 || tr.TransType == 10 || tr.TransType == 11) && tr.ProductCode == filterValue).OrderBy(tr => tr.ProductCode);
            else
                return repository.GetMany(tr => tr.CenterID == centerID && (tr.TransType == 22 || tr.TransType == 10 || tr.TransType == 11)).OrderBy(tr => tr.ProductCode);


            //if (filterColumnName == "MemberCode")
            //    return repository.GetMany(tr => tr.CenterID == centerID && ( tr.TransType == 21 ) && tr.MemberCode == filterValue).OrderBy(tr => tr.ProductCode);

            //else if (filterColumnName == "ProductCode")
            //    return repository.GetMany(tr => tr.CenterID == centerID && ( tr.TransType == 21) && tr.ProductCode == filterValue).OrderBy(tr => tr.ProductCode);


            //else

            //    return repository.GetMany(tr => tr.CenterID == centerID && ( tr.TransType == 21 )).OrderBy(tr => tr.ProductCode);

        }

        public IEnumerable<DailySavingTrx> SaveMonthlyInterestCollection(IEnumerable<DailySavingTrx> savingTrxCollection)
        {

            if (savingTrxCollection != null && savingTrxCollection.Count() > 0)
            {
                foreach (var saving in savingTrxCollection)
                {
                    var dbsaving = repository.GetById(saving.DailySavingTrxID);
                    if (dbsaving != null)
                    {
                        dbsaving.Balance = saving.Balance;
                        dbsaving.SavingInstallment = saving.SavingInstallment;
                        dbsaving.Withdrawal = saving.Withdrawal;
                        dbsaving.Penalty = saving.Penalty;
                        dbsaving.MonthlyInterest = saving.MonthlyInterest;
                        repository.Update(dbsaving);
                    }
                }
            }
            Save();
            return savingTrxCollection;
        }

        public IQueryable<DailySavingTrx> GetDailySavingBankInterestCollectionByCenterQueryable(int centerID, string filterColumnName, string filterValue)
        {
            IQueryable<DailySavingTrx> savingtrx = null;

            if (filterColumnName == "MemberCode")
                savingtrx = repository.GetAllQueryable().Where(tr => tr.CenterID == centerID && (tr.TransType == 10 || tr.TransType == 11 || tr.TransType == 22) && tr.MemberCode == filterValue).OrderBy(tr => tr.ProductCode);

            else if (filterColumnName == "ProductCode")
                savingtrx = repository.GetAllQueryable().Where(tr => tr.CenterID == centerID && (tr.TransType == 10 || tr.TransType == 11 || tr.TransType == 22) && tr.ProductCode == filterValue).OrderBy(tr => tr.ProductCode);


            else

                savingtrx = repository.GetAllQueryable().Where(tr => tr.CenterID == centerID && (tr.TransType == 10 || tr.TransType == 11 || tr.TransType == 22)).OrderBy(tr => tr.ProductCode);
            return savingtrx;
        }

        public IEnumerable<DailySavingTrx> GetMany(Expression<Func<DailySavingTrx, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
