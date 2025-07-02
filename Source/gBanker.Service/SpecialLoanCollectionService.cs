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
    public interface ISpecialLoanCollectionService : IServiceBase<DailyLoanTrx>
    {
        IEnumerable<Proc_get_MaxLoanTerm_Result> Get_MaxLoanTerm(Nullable<int> orgID, Nullable<int> officeID, Nullable<int> centerId, Nullable<long> memberID, Nullable<int> productID);

        IEnumerable<proc_get_SpecialLoanCollection_Result> GetSpecialLoanCollectionDetail(int? officeID, string vday);
        //IEnumerable<DbSpecialLoanCollectionDetailModel> GetSpecialLoanCollectionDetail();
        int getMaxLoanterm(LoanSummary loansummary);
        IEnumerable<ValidationResult> IsValidLoan(DailyLoanTrx specialCollection);
        IEnumerable<ValidationResult> IsValidLoanSave(DailyLoanTrx specialCollection);
        int SetSLCTxtKeyPress(int? orgID, int? officeID, int? centerID, int? productID, long? memberID, int? loanTerm,  DateTime? collectionDate, int? transType);
        int SpecialCollection(int? OrgID, int? officeID, int? centerID, int? productID, long ? memberID, int? loanTerm, string collectionDay, DateTime? collectionDate, int? qType, int? transType, decimal? loanPaid, decimal? intPaid);
        int delVoucher(Nullable<int> officeID, Nullable<System.DateTime> businessDate,Nullable<int> OrgID);
        int LoanCorrection(int? orgID,int? officeID, int? centerID, int? productID, long? memberID, int? loanTerm, string collectionDay, DateTime? collectionDate, int? qType, int? transType, Nullable<decimal> loanPaid, Nullable<decimal> intPaid);
        IEnumerable<proc_get_SpecialLoanCollection_Result> GetSpecialLoanCollectionDetail(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue);
        IEnumerable<GetSetSLCTxtKeyPress_Result> GetSetSLCTxtKeyPress(int? orgID, int? officeID, int? centerID, int? productID, long? memberID, int? loanTerm, DateTime? collectionDate, int? transType);
        int UpdateSpecialLOan(Nullable<long> dailyLoanTrxID, Nullable<int> officeId, Nullable<int> centerId, Nullable<long> memberID, Nullable<int> productID, Nullable<int> lOanterm, Nullable<decimal> loanPaid, Nullable<decimal> intPaid, Nullable<decimal> totalPaid, Nullable<int> trxType, Nullable<int> orgID);
        IEnumerable<proc_get_SpecialLoanCollection_Result> GetSpecialLoanCollectionDetailReabte(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue);
        IEnumerable<proc_get_SpecialLoanCollection_Result> GetSpecialLoanCollectionDetailEmpWise(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue, int? EmpID);
        IEnumerable<proc_get_SpecialLoanCollection_Result> GetRebateDetails(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue);
        //IEnumerable<proc_get_SpecialLoanCollection_Result> GetSavingReinstate(int? jtStartIndex, int? jtPageSize, string jtSorting, string filterColumn, string filterValue, string DateFromValue, string DateToValue);

    }
    public class SpecialLoanCollectionService: ISpecialLoanCollectionService
    {
        private readonly ISpecialLoanCollectionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        private readonly IMemberRepository memberRepository;
        private readonly ILoanSummaryRepository loanSummaryRepository;

        public SpecialLoanCollectionService(ISpecialLoanCollectionRepository repository, IUnitOfWorkCodeFirst unitOfWork, IMemberRepository memberRepository, ILoanSummaryRepository loanSummaryRepository)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.memberRepository = memberRepository;
            this.loanSummaryRepository = loanSummaryRepository;
        }

       

        public IEnumerable<DailyLoanTrx> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.MemberID);
            return entities;
        }

        public DailyLoanTrx GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public DailyLoanTrx Create(DailyLoanTrx objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(DailyLoanTrx objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(long id)
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
            var vMaxLoanLerm = loanSummaryRepository.GetAll().Where(s => s.OrgID==loansummary.OrgID && s.OfficeID == loansummary.OfficeID && s.MemberID == loansummary.MemberID && s.ProductID == loansummary.ProductID && s.LoanStatus == 1 && s.IsActive == true).FirstOrDefault();

           // var vMaxLoanLerm = loanSummaryRepository.GetAll().Where(s => s.OfficeID == loansummary.OfficeID && s.MemberID == loansummary.MemberID && s.CenterID == loansummary.CenterID && s.ProductID == loansummary.ProductID && s.LoanStatus == 1 && s.IsActive == true).FirstOrDefault();

            if (vMaxLoanLerm != null)
            {
                loanterm = vMaxLoanLerm.LoanTerm;
            }
            else
            {
                loanterm = 0 ;
            }

            return loanterm;

          
               
        }


        public IEnumerable<ValidationResult> IsValidLoan(DailyLoanTrx specialCollection)
        {

          

            //var vMaxLoanLerm = loanSummaryRepository.GetAll().Where(s => s.OrgID==specialCollection.OrgID && s.OfficeID == specialCollection.OfficeID && s.MemberID == specialCollection.MemberID && s.CenterID == specialCollection.CenterID && s.ProductID == specialCollection.ProductID && s.LoanTerm == specialCollection.LoanTerm && s.LoanStatus == 1 && s.IsActive == true).FirstOrDefault();

            //// var vMaxLoanLerm = repository.MaxLoanTerm(loansummary); // repository.GetMany(p => p.OfficeID == loansummary.OfficeID && p.MemberID == loansummary.MemberID && p.ProductID == loansummary.ProductID && p.CenterID == loansummary.CenterID).Max();
            //if (vMaxLoanLerm == null)
            //{
            //    yield return new ValidationResult("LoanTerm", "Pls. select valid Member");
            //}


            var memCheck = memberRepository.Get(p => p.MemberID == specialCollection.MemberID && p.CenterID == specialCollection.CenterID && p.OfficeID == specialCollection.OfficeID && p.IsActive == true);
            if (memCheck == null)
            {
                yield return new ValidationResult("LoanTerm", "Pls. select valid Member");
            }
            else
            {
                var entityCheck = repository.Get(p => p.OfficeID == specialCollection.OfficeID && p.MemberID == specialCollection.MemberID && p.ProductID == specialCollection.ProductID && p.CenterID == specialCollection.CenterID && p.LoanTerm == specialCollection.LoanTerm && p.TrxType==specialCollection.TrxType);

                if (entityCheck == null)
                {
                    yield return new ValidationResult("LoanTerm", "Record Not found for selected member");
                }
        
            }

        }
        public IEnumerable<proc_get_SpecialLoanCollection_Result> GetSpecialLoanCollectionDetail(int? officeID, string vday)
        {
            return repository.GetSpecialLoanCollectionDetail(officeID, vday);
        }

        public int SpecialCollection(int? OrgID,int? officeID, int? centerID, int? productID, long? memberID, int? loanTerm, string collectionDay, DateTime? collectionDate, int? qType, int? transType, decimal? loanPaid, decimal? intPaid)
        {
            return repository.SpecialCollection(OrgID,officeID, centerID, productID, memberID, loanTerm, collectionDay, collectionDate, qType, transType, loanPaid,intPaid);
    
        }


        public int delVoucher(int? officeID, DateTime? businessDate, int? OrgID)
        {
            return repository.delVoucher(officeID, businessDate,OrgID);
        }


        public int LoanCorrection(int? orgID, int? officeID, int? centerID, int? productID, long? memberID, int? loanTerm, string collectionDay, DateTime? collectionDate, int? qType, int? transType, decimal? loanPaid, decimal? intPaid)
        {
            return repository.LoanCorrection(orgID,officeID, centerID, productID, memberID, loanTerm, collectionDay, collectionDate, qType, transType, loanPaid, intPaid);

        }


        public IEnumerable<proc_get_SpecialLoanCollection_Result> GetSpecialLoanCollectionDetail(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue)
        {
            return repository.GetSpecialLoanCollectionDetail(OrgID,officeID, vday, filterColumnName, filterValue);
        }


        public IEnumerable<ValidationResult> IsValidLoanSave(DailyLoanTrx specialCollection)
        {
            var vMaxLoanLerm = loanSummaryRepository.GetAll().Where(s => s.OfficeID == specialCollection.OfficeID && s.MemberID == specialCollection.MemberID && s.CenterID == specialCollection.CenterID && s.ProductID == specialCollection.ProductID && s.LoanTerm == specialCollection.LoanTerm && s.LoanStatus == 1 && s.IsActive == true).FirstOrDefault();

            // var vMaxLoanLerm = repository.MaxLoanTerm(loansummary); // repository.GetMany(p => p.OfficeID == loansummary.OfficeID && p.MemberID == loansummary.MemberID && p.ProductID == loansummary.ProductID && p.CenterID == loansummary.CenterID).Max();
            if (vMaxLoanLerm == null)
            {
                yield return new ValidationResult("LoanTerm", "Pls. select valid Member");
            }


            var memCheck = memberRepository.Get(p => p.MemberID == specialCollection.MemberID && p.CenterID == specialCollection.CenterID && p.OfficeID == specialCollection.OfficeID && p.IsActive == true);
            if (memCheck == null)
            {
                yield return new ValidationResult("LoanTerm", "Pls. select valid Member");
            }
            else
            {
                var entityCheck = loanSummaryRepository.Get(p => p.OfficeID == specialCollection.OfficeID && p.MemberID == specialCollection.MemberID && p.ProductID == specialCollection.ProductID && p.CenterID == specialCollection.CenterID && p.LoanTerm == specialCollection.LoanTerm && p.IsActive == true);

                if (entityCheck == null)
                {
                    yield return new ValidationResult("LoanTerm", "Record Not found for selected member");
                }

            }
        }


        public DailyLoanTrx GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }


        public void Delete(int id)
        {
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }


        public int SetSLCTxtKeyPress(int? orgID, int? officeID, int? centerID, int? productID, long? memberID, int? loanTerm,  DateTime? collectionDate, int? transType)
        {
            return repository.SetSLCTxtKeyPress(orgID, officeID, centerID, productID, memberID, loanTerm, collectionDate, transType);

        }


        public IEnumerable<GetSetSLCTxtKeyPress_Result> GetSetSLCTxtKeyPress(int? orgID, int? officeID, int? centerID, int? productID, long? memberID, int? loanTerm, DateTime? collectionDate, int? transType)
        {
            return repository.GetSetSLCTxtKeyPress(orgID, officeID, centerID, productID, memberID, loanTerm, collectionDate, transType);
        }


        public int UpdateSpecialLOan(long? dailyLoanTrxID, int? officeId, int? centerId, long? memberID, int? productID, int? lOanterm, decimal? loanPaid, decimal? intPaid, decimal? totalPaid, int? trxType, int? orgID)
        {
            return repository.UpdateSpecialLOan(dailyLoanTrxID, officeId, centerId, memberID, productID, lOanterm, loanPaid, intPaid, totalPaid, trxType, orgID);
        }

        public IEnumerable<Proc_get_MaxLoanTerm_Result> Get_MaxLoanTerm(int? orgID, int? officeID, int? centerId, long? memberID, int? productID)
        {
            return repository.Get_MaxLoanTerm(orgID,officeID,centerId, memberID, productID);

        }


        public IEnumerable<proc_get_SpecialLoanCollection_Result> GetSpecialLoanCollectionDetailReabte(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue)
        {
            return repository.GetSpecialLoanCollectionDetailReabte(OrgID, officeID, vday, filterColumnName, filterValue);
        }


        public IEnumerable<proc_get_SpecialLoanCollection_Result> GetSpecialLoanCollectionDetailEmpWise(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue, int? EmpID)
        {
            return repository.GetSpecialLoanCollectionDetailEmpWise(OrgID, officeID, vday, filterColumnName, filterValue, Convert.ToInt16(EmpID));
        }

        public IEnumerable<proc_get_SpecialLoanCollection_Result> GetRebateDetails(int? OrgID, int? officeID, string vday, string filterColumnName, string filterValue)
        {
            return repository.GetSpecialLoanCollectionDetailReabte(OrgID, officeID, vday, filterColumnName, filterValue);
        }

        public IEnumerable<DailyLoanTrx> GetMany(Expression<Func<DailyLoanTrx, bool>> where)
        {
            throw new NotImplementedException();
        }

        //public IEnumerable<proc_get_SpecialLoanCollection_Result> GetSavingReinstate(int? jtStartIndex, int? jtPageSize, string jtSorting, string filterColumn, string filterValue, string DateFromValue, string DateToValue)
        //{
        //    return repository.GetSavingReinstate(jtStartIndex, jtPageSize, jtSorting, filterColumn, filterValue, DateFromValue, DateToValue);
        //}
    }
}
