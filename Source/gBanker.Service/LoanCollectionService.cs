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

    public interface ILoanCollectionService : IServiceBase<DailyLoanTrx>
    {
        IEnumerable<getDailyMember_Result> getDailyMember(Nullable<int> officeId, Nullable<int> orgId);
        IEnumerable<getDailyProduct_Result> getDailyProduct(Nullable<int> officeId, Nullable<int> orgId);
        IEnumerable<DailyLoanTrx> GetDailyLoanCollectionByProduct(int OrgId, int OfficeId);
        IEnumerable<DailyLoanTrx> SearchProductDaily(int Prodtype, int OrgID);
        IEnumerable<DailyLoanTrx> GetDailyLoanCollectionByCenterMember(int centerId,int productId,long memberid);
        IEnumerable<DailyLoanTrx> GetDailyLoanCollectionByCenter(int centerId,string filterColumn,string filterValue,string sortColumn, string sortOrder);
        IEnumerable<DailyLoanTrx> GetDailyProshikaLoanCollectionByCenter(int centerId, string filterColumn, string filterValue, string sortColumn, string sortOrder);
        IEnumerable<DailyLoanTrx> GetDailyLoanCollectionByCenter(int centerId);
        IEnumerable<DailyLoanTrx> SaveDailyLoanCollection(IEnumerable<DailyLoanTrx> loanTrxCollection);
        int getMaxLoanterm(DailyLoanTrx loansummary);
        IEnumerable<ValidationResult> IsValidLoan(DailyLoanTrx loanCollection);
        int delVoucher(Nullable<int> officeID, Nullable<System.DateTime> businessDate, int? OrgID);
        int setLoanAndSavingingLessFiftyPercent(int? OrgID, Nullable<int> officeID, Nullable<int> CenterID, Nullable<int> Qtype);
        int UpdateSpecialLOan(Nullable<long> dailyLoanTrxID, Nullable<int> officeId, Nullable<int> centerId, Nullable<long> memberID, Nullable<int> productID, Nullable<int> lOanterm, Nullable<decimal> loanPaid, Nullable<decimal> intPaid, Nullable<decimal> totalPaid, Nullable<int> trxType, Nullable<int> orgID);

        IEnumerable<DailyLoanTrx> GetLoanCollectionDetailPaged(int centerId, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount);
        IQueryable<DailyLoanTrx> GetAllQueryable();
        IQueryable<DailyLoanTrx> GetDailyLoanCollectionByCenterQueryable(int centerId, string filterColumn, string filterValue, string sortColumn, string sortOrder);
        IQueryable<DailyLoanTrx> GetDailyProshikaLoanCollectionByCenterQueryable(int centerId, string filterColumn, string filterValue, string sortColumn, string sortOrder);
    }
    public class LoanCollectionService : ILoanCollectionService
    {
        private readonly ILoanCollectionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        private readonly IMemberRepository memberRepository;
        private readonly ILoanSummaryRepository loanSummaryRepository;

        public LoanCollectionService(ILoanCollectionRepository repository, IUnitOfWorkCodeFirst unitOfWork, IMemberRepository memberRepository, ILoanSummaryRepository loanSummaryRepository)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.memberRepository = memberRepository;
            this.loanSummaryRepository = loanSummaryRepository;
        }
        public IEnumerable<DailyLoanTrx> GetDailyLoanCollectionByCenter(int centerId)
        {
            return repository.GetMany(tr => tr.CenterID == centerId && (tr.TrxType == 10 || tr.TrxType == 11)).OrderBy(tr => tr.MemberID);
        }


        public void Save()
        {
            unitOfWork.Commit();
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


        public IEnumerable<DailyLoanTrx> SaveDailyLoanCollection(IEnumerable<DailyLoanTrx> loanTrxCollection)
        {

            if (loanTrxCollection != null && loanTrxCollection.Count() > 0)
            {
                foreach (var loan in loanTrxCollection)
                {
                    var dbLoan = repository.GetById(loan.DailyLoanTrxID);
                    if (dbLoan != null)
                    {
                        if (dbLoan.OrgID == 54 || dbLoan.OrgID == 6)
                        {
                            if (dbLoan.CollectionType != "B")
                            {
                                //Console.Write(dbLoan.DailyLoanTrxID);
                                dbLoan.LoanPaid = loan.LoanPaid;
                                dbLoan.IntPaid = loan.IntPaid;
                                dbLoan.TotalPaid = loan.TotalPaid;
                                dbLoan.CollectionStatus = loan.CollectionStatus;
                                repository.Update(dbLoan);
                            }
                        }
                        else
                        {
                            dbLoan.LoanPaid = loan.LoanPaid;
                            dbLoan.IntPaid = loan.IntPaid;
                            dbLoan.TotalPaid = loan.TotalPaid;
                            dbLoan.CumIntDue = loan.CumIntDue;
                            dbLoan.CollectionStatus = loan.CollectionStatus;
                            repository.Update(dbLoan);
                        }
                       
                    }
                }
            }
            Save();
            return loanTrxCollection;
        }


        public int getMaxLoanterm(DailyLoanTrx loansummary)
        {
            int loanterm;
            var vMaxLoanLerm = repository.GetAll().Where(s => s.OrgID == loansummary.OrgID && s.OfficeID == loansummary.OfficeID && s.MemberID == loansummary.MemberID && s.ProductID == loansummary.ProductID && s.IsActive == true).FirstOrDefault();

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


        public IEnumerable<ValidationResult> IsValidLoan(DailyLoanTrx loanCollection)
        {

            var vMaxLoanLerm = loanSummaryRepository.GetAll().Where(s => s.OfficeID == loanCollection.OfficeID && s.MemberID == loanCollection.MemberID && s.CenterID == loanCollection.CenterID && s.ProductID == loanCollection.ProductID && s.LoanTerm == loanCollection.LoanTerm && s.LoanStatus == 1 && s.IsActive == true).FirstOrDefault();

            // var vMaxLoanLerm = repository.MaxLoanTerm(loansummary); // repository.GetMany(p => p.OfficeID == loansummary.OfficeID && p.MemberID == loansummary.MemberID && p.ProductID == loansummary.ProductID && p.CenterID == loansummary.CenterID).Max();
            if (vMaxLoanLerm == null)
            {
                yield return new ValidationResult("LoanTerm", "Pls. select valid Member");
            }


            var memCheck = memberRepository.Get(p => p.MemberID == loanCollection.MemberID && p.CenterID == loanCollection.CenterID && p.OfficeID == loanCollection.OfficeID && p.IsActive == true);
            var DupliCheck = repository.Get(d => d.OfficeID == loanCollection.OfficeID && d.MemberID == loanCollection.MemberID && d.ProductID == loanCollection.ProductID && d.LoanTerm == loanCollection.LoanTerm && d.TrxType == 0);
            //var DupliCheck = repository.Get(d => d.OfficeID == loanCollection.OfficeID && d.MemberID == loanCollection.MemberID && d.CenterID == loanCollection.CenterID && d.ProductID == loanCollection.ProductID && d.LoanTerm == loanCollection.LoanTerm && d.TrxType==0);
            if (DupliCheck != null)
            {
                yield return new ValidationResult("LoanTerm", "Pls. select valid Member");
            }
            if (memCheck == null)
            {
                yield return new ValidationResult("LoanTerm", "Pls. select valid Member");
            }
            else
            {
                var entityCheck = repository.Get(p => p.OfficeID == loanCollection.OfficeID && p.MemberID == loanCollection.MemberID && p.ProductID == loanCollection.ProductID && p.LoanTerm == loanCollection.LoanTerm && p.TrxType == 0);
                // var entityCheck = repository.Get(p => p.OfficeID == loanCollection.OfficeID && p.MemberID == loanCollection.MemberID && p.ProductID == loanCollection.ProductID && p.CenterID == loanCollection.CenterID && p.LoanTerm == loanCollection.LoanTerm && p.TrxType == 0);

                if (entityCheck != null)
                {
                    yield return new ValidationResult("LoanTerm", "this member already exists");
                }

            }
        }


        public int delVoucher(int? officeID, DateTime? businessDate, int? OrgID)
        {
            return repository.delVoucher(officeID, businessDate, OrgID);
        }

        public IEnumerable<DailyLoanTrx> GetDailyLoanCollectionByCenter(int centerId, string filterColumn, string filterValue, string sortColumn, string sortOrder)
        {
            IEnumerable<DailyLoanTrx> loantrx = null;
            if (filterColumn == "MemberCode")
                loantrx = repository.GetMany(tr => tr.CenterID == centerId && (tr.TrxType == 10 || tr.TrxType == 11) && tr.MemberCode == filterValue).OrderBy(tr => tr.ProductCode);

            else if (filterColumn == "ProductCode")
                loantrx = repository.GetMany(tr => tr.CenterID == centerId && (tr.TrxType == 10 || tr.TrxType == 11) && tr.ProductCode == filterValue).OrderBy(tr => tr.ProductCode);
            else

                loantrx = repository.GetMany(tr => tr.CenterID == centerId && (tr.TrxType == 10 || tr.TrxType == 11)).OrderBy(tr => tr.ProductCode);

            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                if (sortColumn == "MemberCode")
                    loantrx = loantrx.OrderBy(ord => ord.MemberCode);
                else if (sortColumn == "MemberName")
                    loantrx = loantrx.OrderBy(ord => ord.MemberName);
            }
            else
                loantrx = loantrx.OrderBy(ord => ord.ProductCode);
            return loantrx;
        }


        public IEnumerable<DailyLoanTrx> GetLoanCollectionDetailPaged(int centerId, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount)
        {
            return repository.GetLoanCollectionDetailPaged(centerId, filterColumnName, filterValue, startRowIndex, pageSize, out totalCount);
        }


        public DailyLoanTrx GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }


        public int setLoanAndSavingingLessFiftyPercent(int? OrgID, int? officeID, int? CenterID, int? Qtype)
        {
            return repository.setLoanAndSavingingLessFiftyPercent(OrgID, officeID, CenterID, Qtype);
        }

        public IEnumerable<DailyLoanTrx> GetDailyLoanCollectionByCenterMember(int centerId, int productId, long memberid)
        {
            IEnumerable<DailyLoanTrx> loantrx = null;
            if (productId > 0 && memberid > 0)
            {
                loantrx = repository.GetMany(tr => tr.CenterID == centerId && (tr.TrxType == 10 || tr.TrxType == 11) && tr.ProductID == productId && tr.MemberID == memberid).OrderBy(tr => tr.ProductCode);
            }
            else if (productId == 0 && centerId > 0)
            {
                loantrx = repository.GetMany(tr => tr.CenterID == centerId && (tr.TrxType == 10 || tr.TrxType == 11)).OrderBy(tr => tr.ProductCode);
            }
            else

                loantrx = repository.GetMany(tr => tr.CenterID == centerId && (tr.TrxType == 10 || tr.TrxType == 11) && tr.MemberID == memberid).OrderBy(tr => tr.ProductCode);

            loantrx = loantrx.OrderBy(ord => ord.ProductCode);
            return loantrx;
        }

        public IEnumerable<DailyLoanTrx> SearchProductDaily(int Prodtype, int OrgID)
        {
            return repository.GetMany(g => g.IsActive == true && g.OrgID == OrgID).OrderBy(g => g.ProductCode).Distinct();
        }

        public IEnumerable<DailyLoanTrx> GetDailyLoanCollectionByProduct(int OrgId, int OfficeId)
        {
            return repository.GetDailyLoanCollectionByProduct(OrgId, OfficeId);
        }

        public IEnumerable<getDailyProduct_Result> getDailyProduct(int? officeId, int? orgId)
        {
            return repository.getDailyProduct(orgId, officeId);
        }

        public IEnumerable<getDailyMember_Result> getDailyMember(int? officeId, int? orgId)
        {
            return repository.getDailyMember(officeId, orgId);
        }


        public int UpdateSpecialLOan(long? dailyLoanTrxID, int? officeId, int? centerId, long? memberID, int? productID, int? lOanterm, decimal? loanPaid, decimal? intPaid, decimal? totalPaid, int? trxType, int? orgID)
        {
            return repository.UpdateSpecialLOan(dailyLoanTrxID, officeId, centerId, memberID, productID, lOanterm, loanPaid, intPaid, totalPaid, trxType, orgID);
        }

        public IQueryable<DailyLoanTrx> GetAllQueryable()
        {
            return repository.GetAllQueryable();
        }

        public IQueryable<DailyLoanTrx> GetDailyLoanCollectionByCenterQueryable(int centerId, string filterColumn, string filterValue, string sortColumn, string sortOrder)
        {
            IQueryable<DailyLoanTrx> loantrx = null;
            if (filterColumn == "MemberCode")
                loantrx = repository.GetAllQueryable().Where(tr => tr.CenterID == centerId && (tr.TrxType == 10 || tr.TrxType == 11) && tr.MemberCode == filterValue).OrderBy(tr => tr.MemberCode);

            else if (filterColumn == "ProductCode")
                loantrx = repository.GetAllQueryable().Where(tr => tr.CenterID == centerId && (tr.TrxType == 10 || tr.TrxType == 11) && tr.ProductCode == filterValue).OrderBy(tr => tr.MemberCode);
            else
                loantrx = repository.GetAllQueryable().Where(tr => tr.CenterID == centerId && (tr.TrxType == 10 || tr.TrxType == 11)).OrderBy(tr => tr.MemberCode);

            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                if (sortColumn == "MemberCode")
                    loantrx = loantrx.OrderBy(ord => ord.MemberCode);
                else if (sortColumn == "MemberName")
                    loantrx = loantrx.OrderBy(ord => ord.MemberName);
            }
            else
                loantrx = loantrx.OrderBy(ord => ord.MemberCode);
            return loantrx;
            //IQueryable<DailyLoanTrx> loantrx = null;
            //if (filterColumn == "MemberCode")
            //    loantrx = repository.GetAllQueryable().Where(tr => tr.CenterID == centerId && (tr.TrxType == 10 || tr.TrxType == 11) && tr.MemberCode == filterValue).OrderBy(tr => tr.MemberCode);

            //else if (filterColumn == "ProductCode")
            //    loantrx = repository.GetAllQueryable().Where(tr => tr.CenterID == centerId && (tr.TrxType == 10 || tr.TrxType == 11) && tr.ProductCode == filterValue).OrderBy(tr => tr.MemberCode);
            //else
            //    loantrx = repository.GetAllQueryable().Where(tr => tr.CenterID == centerId && (tr.TrxType == 10 || tr.TrxType == 11)).OrderBy(tr => tr.MemberCode);

            //if (!string.IsNullOrWhiteSpace(sortColumn))
            //{
            //    if (sortColumn == "MemberCode")
            //        loantrx = loantrx.OrderBy(ord => ord.MemberCode);
            //    else if (sortColumn == "MemberName")
            //        loantrx = loantrx.OrderBy(ord => ord.MemberName);
            //}
            //else
            //    loantrx = loantrx.OrderBy(ord => ord.MemberCode);
            //return loantrx;
        }

        public IEnumerable<DailyLoanTrx> GetMany(Expression<Func<DailyLoanTrx, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DailyLoanTrx> GetDailyProshikaLoanCollectionByCenterQueryable(int centerId, string filterColumn, string filterValue, string sortColumn, string sortOrder)
        {
            IQueryable<DailyLoanTrx> loantrx = null;
            if (filterColumn == "MemberCode")
                loantrx = repository.GetAllQueryable().Where(tr => tr.CenterID == centerId && (tr.TrxType == 10 || tr.TrxType == 11) && tr.MemberCode == filterValue).OrderBy(tr => tr.MemberCode);

            else if (filterColumn == "ProductCode")
                loantrx = repository.GetAllQueryable().Where(tr => tr.CenterID == centerId && (tr.TrxType == 10 || tr.TrxType == 11) && tr.ProductCode == filterValue).OrderBy(tr => tr.MemberCode);
            else
                loantrx = repository.GetAllQueryable().Where(tr => tr.CenterID == centerId && (tr.TrxType == 10 || tr.TrxType == 11)).OrderBy(tr => tr.MemberCode);

            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                if (sortColumn == "MemberCode")
                    loantrx = loantrx.OrderBy(ord => ord.MemberCode);
                else if (sortColumn == "MemberName")
                    loantrx = loantrx.OrderBy(ord => ord.MemberName);
            }
            else
                loantrx = loantrx.OrderBy(ord => ord.MemberCode);
            return loantrx;
        }

        public IEnumerable<DailyLoanTrx> GetDailyProshikaLoanCollectionByCenter(int centerId, string filterColumn, string filterValue, string sortColumn, string sortOrder)
        {
            IEnumerable<DailyLoanTrx> loantrx = null;
            if (filterColumn == "MemberCode")
                loantrx = repository.GetMany(tr => tr.CenterID == centerId && (tr.TrxType == 10 || tr.TrxType == 11) && tr.MemberCode == filterValue).OrderBy(tr => tr.ProductCode);

            else if (filterColumn == "ProductCode")
                loantrx = repository.GetMany(tr => tr.CenterID == centerId && (tr.TrxType == 10 || tr.TrxType == 11) && tr.ProductCode == filterValue).OrderBy(tr => tr.ProductCode);
            else

                loantrx = repository.GetMany(tr => tr.CenterID == centerId && (tr.TrxType == 10 || tr.TrxType == 11)).OrderBy(tr => tr.MainProductCode);

            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                if (sortColumn == "MemberCode")
                    loantrx = loantrx.OrderBy(ord => ord.MemberCode);
                else if (sortColumn == "MemberName")
                    loantrx = loantrx.OrderBy(ord => ord.MemberName);
            }
            else
                loantrx = loantrx.OrderBy(ord => ord.ProductCode);
            return loantrx;
        }
    }
}
