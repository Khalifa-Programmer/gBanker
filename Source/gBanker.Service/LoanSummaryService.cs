using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
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
    /// <summary>
    /// Performs loan summary
    /// </summary>
    public interface ILoanSummaryService : IServiceBase<LoanSummary>
    {
        int setWriteOffList(int? OrgID, int? office, long? memberID, int? centerID, int? productID, int? loanTerm, DateTime? trandate, Nullable<decimal> writeOffLOan, Nullable<decimal> writeOffInterest);

        int delWriteOffList(int? OrgID, int? office, long? memberID, int? centerID, int? productID, int? loanTerm, DateTime? trandate, Nullable<decimal> writeOffLOan, Nullable<decimal> writeOffInterest);
        IEnumerable<LoanSummary> ApprovedLoan();
        bool IsValidLoan(LoanSummary loansummary, out string msg);
        IEnumerable<ValidationResult> IsValidLoan(LoanSummary loansummary);
        bool IsValidLoanForEdit(LoanSummary loansummary, out string msg);
        IEnumerable<ValidationResult> IsValidLoanForEdit(LoanSummary loansummary);
        LoanSummary CreateLoanTrx(LoanSummary objectToCreate);
        int SetOpeningLoanEntry(int? officeID);
        IEnumerable<DBLoanApproveDetailModel> GetLoanApproveDetail(int? orgID, int? officeID);
        IEnumerable<DBLoanApproveDetailModel> GetLoanSummaryDetail(int? officeID);
        IEnumerable<DBLoanApproveDetailModel> GetLoanSummaryDetail(int? officeID, string filterColumnName, string filterValue);
        IEnumerable<DBLoanApproveDetailModel> GetLoanApproveDetail(int? orgID, int? officeID, string filterColumnName, string filterValue);
        IEnumerable<DBLoanApproveDetailModel> GetLoanApproveDetailPaged(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, string jtSorting, out long totalCount, DateTime? transactiondate, int OrgID);

        IEnumerable<DBLoanApproveDetailModel> GetLoanApproveDetailPaged(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, string jtSorting, out long totalCount, DateTime? transactiondate, int OrgID, int empid);
        LoanSummary GetByMemProdId(int MemId, int ProdId);

        IEnumerable<DBLoanApproveDetailModel> GetLoanSummaryDetailPaged(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount);
        int Proc_Set_RepaymentSchedule(Nullable<long> loanSummaryID, Nullable<int> officeID, Nullable<long> memberID, Nullable<short> productID, Nullable<int> centerID, Nullable<byte> memberCategoryID, Nullable<int> loanTerm, Nullable<int> duration, Nullable<System.DateTime> installmentStartDate, string createUser, Nullable<System.DateTime> createDate);
        //  int updateDisburseCharge(int? loanSummaryID, int? officeID, int? centerID, int? memberId, int? productID, int? loanterm, decimal? principal, Nullable<System.DateTime> installmentStartDate, Nullable<System.DateTime> disburseDate);
        LoanSummary GetByLasLoanByMemberId(long MemId);
        string GetNewLoanNo(long memId, string memCode);
        int InsuranceDailyDisburse(int? officeID, int? centerID, Nullable<long> memberID, Nullable<int> productID, Nullable<int> loanTerm, Nullable<System.DateTime> disburseDate, Nullable<decimal> principalLoan, Nullable<int> employeeId, Nullable<int> memberCategoryID, Nullable<int> orgID);
        LoanSummary GetByLoanSummaryId(long id);

        LoanSummary GetSingleRow(int OfficeID, short ProductID, int CenterID, long MemberID, int LoanTerm);
        LoanSummary GetByWriteoffinfowithmemberinfo(long memberID, DateTime DD, decimal PL);

    }
    public class LoanSummaryService : ILoanSummaryService
    {
        private readonly ILoanSummaryRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        private readonly IProductRepository productRepository;
        private readonly ILoanTrxRepository loanTrxRepository;
        private readonly IMemberRepository memberRepository;
        public LoanSummaryService(ILoanSummaryRepository repository, IProductRepository productRepository, ILoanTrxRepository loanTrxRepository, IUnitOfWorkCodeFirst unitOfWork,IMemberRepository memberRepository)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.productRepository = productRepository;
            this.loanTrxRepository = loanTrxRepository;
            this.memberRepository = memberRepository;
        }
        public bool IsValidLoan(LoanSummary loansummary, out string msg)
        {
            var validationMsg = "";
            var isValid = true;
            var entity = repository.Get(p => p.OfficeID == loansummary.OfficeID && p.MemberID == loansummary.MemberID && p.ProductID == loansummary.ProductID && p.CenterID == loansummary.CenterID && p.LoanTerm == loansummary.LoanTerm && p.IsActive==true);
            if (entity != null)
            {
                isValid = false;
                validationMsg = "Duplicate data.";
            }


            var product = productRepository.GetById(loansummary.ProductID);
            if (product != null)
            {
                if (product.Duration.HasValue && product.Duration.Value < loansummary.Duration)
                {
                    isValid = false;
                    validationMsg = "Duration exceeds.";
                }
                //mansur
                else if (product.MaxLimit.HasValue && product.MaxLimit.Value < loansummary.PrincipalLoan)
                {
                    isValid = false;
                    validationMsg = "MaxLimt exceeds.";
                }
                else if (product.InterestRate.HasValue && product.InterestRate.Value != loansummary.InterestRate)
                {
                    isValid = false;
                    validationMsg = "Interest does not match with product interest exceeds.";
                }
            }
            else
                isValid = false;
            msg = validationMsg;
            return isValid;
        }
        public IEnumerable<LoanSummary> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.MemberID);
            return entities;
        }
        public LoanSummary GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public LoanSummary GetByLoanSummaryId(long id)
        {
            var entity = repository.Get(w => w.LoanSummaryID == id);
            return entity;
        }
        
        public LoanSummary GetByWriteoffinfowithmemberinfo(long memberID,DateTime DD, decimal PL)
        {
            var entity = repository.Get(w => w.MemberID == memberID && w.DisburseDate == DD && w.PrincipalLoan == PL);
            return entity;
        }

        public LoanSummary GetByMemProdId(int MemId, int ProdId)
        {
            var entity = repository.Get(g => g.MemberID == MemId && g.ProductID == ProdId);            
            //var entity = repository.GetById(id);
            return entity;
        }
        public LoanSummary GetByLasLoanByMemberId(long MemId)
        {
            //var entity = repository.Get(g => g.MemberID == MemId && g.IsActive == true && g.LoanStatus==1).OrderByDescending(o => o.LoanSummaryID).First();
            var entity = repository.GetAll().Where(g => g.MemberID == MemId && g.IsActive == true && g.LoanStatus == 1).OrderByDescending(o => o.LoanSummaryID).FirstOrDefault();
            //var entity = repository.GetById(id);
            return entity;
        }
        public string GetNewLoanNo(long memId, string memCode)
        {
            var entity = GetByLasLoanByMemberId(memId);
            var LoanNo = "";
            if (entity != null)
            {
                LoanNo = (Convert.ToInt64(entity.LoanNo) + 1).ToString();
            }
            else
            {
                LoanNo = memCode + "1";
            }
            return LoanNo;

        }
        public LoanSummary Create(LoanSummary objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }
        public LoanSummary CreateLoanTrx(LoanSummary objectToCreate)
        {


            var loanSumma = repository.GetMany(p => p.IsActive == true);

            foreach (var item in loanSumma)
            {


                var loanTrx = new LoanTrx();
                loanTrx.TrxDate = Convert.ToDateTime(item.InstallmentDate);
                loanTrx.LoanSummaryID = item.LoanSummaryID;
                loanTrx.OfficeID = item.OfficeID;
                loanTrx.MemberID = item.MemberID;
                loanTrx.ProductID = item.ProductID;
                loanTrx.CenterID = item.CenterID;
                loanTrx.MemberCategoryID = Convert.ToByte(item.MemberCategoryID);
                loanTrx.LoanTerm = item.LoanTerm;
                loanTrx.LoanTerm = item.LoanTerm;

                loanTrx.InstallmentDate = Convert.ToDateTime(item.InstallmentDate);
                loanTrx.PrincipalLoan = item.PrincipalLoan;
                loanTrx.LoanDue = Convert.ToDecimal(item.LoanInstallment) * Convert.ToInt16(item.InstallmentNo);
                loanTrx.LoanPaid = Convert.ToDecimal(item.LoanRepaid);

                loanTrx.IntCharge = Convert.ToDecimal(item.IntCharge);
                loanTrx.IntDue = Convert.ToDecimal(item.IntInstallment) * Convert.ToInt16(item.InstallmentNo);
                loanTrx.IntPaid = Convert.ToDecimal(item.IntPaid);
                loanTrx.TrxType = 1;
                loanTrx.InstallmentNo = Convert.ToInt16(item.InstallmentNo);
                loanTrx.CreateDate = System.DateTime.Now;
                loanTrx.CreateUser = "sa";
                loanTrxRepository.Add(loanTrx);
                Save();



            }


            return objectToCreate;
        }
        public void Update(LoanSummary objectToUpdate)
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

        public bool IsValidLoanForEdit(LoanSummary loansummary, out string msg)
        {
            var validationMsg = "";
            var isValid = true;

            var product = productRepository.GetById(loansummary.ProductID);

            if (product != null)
            {
                if (product.Duration.HasValue && product.Duration.Value < loansummary.Duration)
                {
                    isValid = false;
                    validationMsg = "Duration exceeds.";
                }
                //mansur
                else if (product.MaxLimit.HasValue && product.MaxLimit.Value < loansummary.PrincipalLoan)
                {
                    isValid = false;
                    validationMsg = "MaxLimt exceeds.";
                }
                else if (product.InterestRate.HasValue && product.InterestRate.Value != loansummary.InterestRate)
                {
                    isValid = false;
                    validationMsg = "Interest does not match with product interest exceeds.";
                }
            }
            else
                isValid = false;
            msg = validationMsg;
            return isValid;
        }

        public IEnumerable<LoanSummary> ApprovedLoan()
        {
            return repository.GetMany(g => g.IsActive == true && g.DisburseDate == null).OrderBy(g => g.MemberID);
        }


        public int SetOpeningLoanEntry(int? officeID)
        {
            return repository.SetOpeningLoanEntry(officeID);
        }


        public IEnumerable<ValidationResult> IsValidLoan(LoanSummary loansummary)
        {
            var memEntiry = memberRepository.Get(m => m.OfficeID == loansummary.OfficeID && m.CenterID == loansummary.CenterID && m.MemberID == loansummary.MemberID && m.IsActive == true);
            if (memEntiry == null)
            {
                yield return new ValidationResult("OfficeID", "Invalid Member");
            }

            var entity = repository.Get(p => p.OfficeID == loansummary.OfficeID && p.MemberID == loansummary.MemberID && p.ProductID == loansummary.ProductID  && p.LoanTerm == loansummary.LoanTerm && p.IsActive == true);
            if (entity != null)
            {
                yield return new ValidationResult("OfficeID", "Duplicate Record");
            }
            var product = productRepository.GetById(loansummary.ProductID);
            if (product != null)
            {
                if (product.Duration.HasValue && product.Duration.Value < loansummary.Duration)               
                    yield return new ValidationResult("Duration", "Duration exceeds.");               
                //mansur
                else if (product.MaxLimit.HasValue && product.MaxLimit.Value < loansummary.PrincipalLoan)
                    yield return new ValidationResult("PrincipalLoan", "MaxLimt exceeds.");               
                else if (product.InterestRate.HasValue && product.InterestRate.Value != loansummary.InterestRate)
                    yield return new ValidationResult("InterestRate", "Interest does not match with product interest exceeds.");                
            }          
           
        }


        public IEnumerable<ValidationResult> IsValidLoanForEdit(LoanSummary loansummary)
        {

            var loanSum = repository.GetAll().Where(l => l.LoanSummaryID == loansummary.LoanSummaryID && l.Posted == false);
            if (loanSum != null)
            {
                var product = productRepository.GetById(loansummary.ProductID);

                if (product != null)
                {
                    if (product.Duration.HasValue && product.Duration.Value < loansummary.Duration)
                        yield return new ValidationResult("Duration", "Duration exceeds.");
                    //mansur
                    else if (product.MaxLimit.HasValue && product.MaxLimit.Value < loansummary.PrincipalLoan)
                        yield return new ValidationResult("PrincipalLoan", "MaxLimt exceeds.");
                    else if (product.InterestRate.HasValue && product.InterestRate.Value != loansummary.InterestRate)
                        yield return new ValidationResult("InterestRate", "Interest does not match with product interest exceeds.");
                }
            }
            else

                yield return new ValidationResult("Duration", "Already Posted");

        }


        public IEnumerable<DBLoanApproveDetailModel> GetLoanApproveDetail(int? orgID, int? officeID)
        {
            return repository.GetLoanApproveDetail(orgID,officeID);
        }


        public IEnumerable<DBLoanApproveDetailModel> GetLoanSummaryDetail(int? officeID)
        {
            return repository.GetLoanSummaryDetail(officeID);
        }


        public IEnumerable<DBLoanApproveDetailModel> GetLoanSummaryDetail(int? officeID, string filterColumnName, string filterValue)
        {
            return repository.GetLoanSummaryDetail(officeID,filterColumnName, filterValue);
        }


        public IEnumerable<DBLoanApproveDetailModel> GetLoanApproveDetail(int? orgID, int? officeID, string filterColumnName, string filterValue)
        {
            return repository.GetLoanApproveDetail(orgID,officeID, filterColumnName, filterValue);
        }


        public IEnumerable<DBLoanApproveDetailModel> GetLoanSummaryDetailPaged(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount)
        {
            return repository.GetLoanSummaryDetailPaged(officeID, filterColumnName, filterValue, startRowIndex, pageSize, out totalCount);
        }


        public IEnumerable<DBLoanApproveDetailModel> GetLoanApproveDetailPaged(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, string jtSorting, out long totalCount, DateTime? transactiondate, int OrgID)
        {
            return repository.GetLoanApproveDetailPaged(officeID, filterColumnName, filterValue, startRowIndex, pageSize,jtSorting, out totalCount,transactiondate,OrgID);
        }


        public int Proc_Set_RepaymentSchedule(long? loanSummaryID, int? officeID, long? memberID, short? productID, int? centerID, byte? memberCategoryID, int? loanTerm, int? duration, DateTime? installmentStartDate, string createUser, DateTime? createDate)
        {
            return repository.Proc_Set_RepaymentSchedule(loanSummaryID,officeID,memberID,productID,centerID,memberCategoryID,loanTerm,duration,installmentStartDate,createUser,createDate);
        }



        public int setWriteOffList(int? OrgID, int? office, long? memberID, int? centerID, int? productID, int? loanTerm, DateTime? trandate, decimal? writeOffLOan, decimal? writeOffInterest)
        {
            return repository.setWriteOffList(OrgID,office, memberID, centerID, productID, loanTerm, trandate, writeOffLOan, writeOffInterest);
        }


        public int delWriteOffList(int? OrgID, int? office, long? memberID, int? centerID, int? productID, int? loanTerm, DateTime? trandate, decimal? writeOffLOan, decimal? writeOffInterest)
        {
            return repository.delWriteOffList(OrgID,office, memberID, centerID, productID, loanTerm, trandate, writeOffLOan, writeOffInterest);
        }


        public LoanSummary GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }


        public int InsuranceDailyDisburse(int? officeID, int? centerID, long? memberID, int? productID, int? loanTerm, DateTime? disburseDate, decimal? principalLoan, int? employeeId, int? memberCategoryID, int? orgID)
        {
           return repository.InsuranceDailyDisburse(officeID,centerID,memberID,productID,loanTerm,disburseDate,principalLoan,employeeId,memberCategoryID,orgID);
        }


        public IEnumerable<DBLoanApproveDetailModel> GetLoanApproveDetailPaged(int? officeID, string filterColumnName, string filterValue, int startRowIndex, int pageSize, string jtSorting, out long totalCount, DateTime? transactiondate, int OrgID, int empid)
        {
            return repository.GetLoanApproveDetailPaged(officeID, filterColumnName, filterValue, startRowIndex, pageSize, jtSorting, out totalCount, transactiondate, OrgID,empid);
        }

        public IEnumerable<LoanSummary> GetMany(Expression<Func<LoanSummary, bool>> where)
        {
            throw new NotImplementedException();
        }

        public LoanSummary GetSingleRow(int OfficeID, short ProductID, int CenterID, long MemberID, int LoanTerm)
        {
            var entity = repository.Get(g => g.OfficeID == OfficeID && g.ProductID == ProductID && g.CenterID == CenterID && g.MemberID == MemberID && g.LoanTerm == LoanTerm);
            //var entity = repository.GetById(id);
            return entity;
        }
    }
}
