using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.DBDetailModels;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gBanker.Data.CodeFirstMigration;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface ILoanDisburseService : IServiceBase<LoanSummary>
    {
        IEnumerable<LoanSummary> DisburseLoan();
        bool IsValidLoan(LoanSummary loansummary, out string msg);
        IEnumerable<DBLoanApproveDetailModel> GetLoanApproveDetail(int OrgID,int? officeID);
        IEnumerable<Proc_get_LoanDisburse_Result> GetLoanDisburse(int? OfficeId, DateTime? vDate);
        IEnumerable<Proc_get_LoanDisburse_Result> GetLoanDisburse(int? OrgID,int? OfficeId, DateTime? vDate, string filterColumnName, string filterValue);
        IEnumerable<Proc_get_LoanDisburse_Result> GetLoanDisburseSms(int? OrgID, int? OfficeId, DateTime? vDate, string filterColumnName, string filterValue);
        IEnumerable<Proc_get_LoanDisburse_Result> GetPartialLoanDisburse(int? OrgID, int? OfficeId, DateTime? vDate, string filterColumnName, string filterValue);
        IEnumerable<Proc_get_LoanDisburse_Result> GetFirstLoanDisburse(int? OrgID, int? OfficeId, DateTime? vDate, string filterColumnName, string filterValue);
        IEnumerable<Proc_get_LoanDisburse_Result> GetPartialLoanDisburseCCLoan(int? OrgID, int? OfficeId, DateTime? vDate, string filterColumnName, string filterValue, string filterCCLoan);
    }
    public class LoanDisburseService : ILoanDisburseService
    {
         
        private readonly ILoanSummaryRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        private readonly IProductRepository productRepository;
        private readonly ILoanTrxRepository loanTrxRepository;
       
        public LoanDisburseService(ILoanSummaryRepository repository, IProductRepository productRepository, ILoanTrxRepository loanTrxRepository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.productRepository = productRepository;
            this.loanTrxRepository = loanTrxRepository;
        }
        public IEnumerable<LoanSummary> DisburseLoan()
        {
            return repository.GetMany(g => g.IsActive == true || g.DisburseDate == null);
        }

        public bool IsValidLoan(LoanSummary loansummary, out string msg)
        {
           
            var validationMsg = "";
            var isValid = true;

                 var product = productRepository.GetById(loansummary.ProductID);

                if (product != null)
                {
                    decimal vinterestRate = Convert.ToDecimal(product.InterestRate);
                    int vduration = Convert.ToInt16(product.Duration);
                    decimal vLoanAmount = Convert.ToDecimal(product.LoanInstallment);
                    decimal vInterestAmount = Convert.ToDecimal(product.InterestInstallment);

                    decimal vLoanInstallment = loansummary.PrincipalLoan * vLoanAmount;
                    decimal vInterestInstallment = loansummary.PrincipalLoan * vInterestAmount;
                    //if (vLoanInstallment < loansummary.LoanInstallment)
                    //{
                    //    isValid = false;
                    //    validationMsg = "Invalid Loan Installment exceeds.";
                    //}

                    //else if (vInterestInstallment < loansummary.IntInstallment)
                    //{
                    //    isValid = false;
                    //    validationMsg = "Invalid Interest Installment exceeds.";
                    //}
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
                    //else if (product.InterestRate.HasValue && product.InterestRate.Value != loansummary.InterestRate)
                    //{
                    //    isValid = false;
                    //    validationMsg = "Interest does not match with product interest exceeds.";
                    //}
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

        public LoanSummary Create(LoanSummary objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Update(LoanSummary objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

       

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
        public IEnumerable<DBLoanApproveDetailModel> GetLoanApproveDetail(int OrgID, int? officeID)
        {
            return repository.GetLoanApproveDetail(OrgID, officeID);
        }
        public IEnumerable<Proc_get_LoanDisburse_Result> GetLoanDisburse(int? OfficeId, DateTime? vDate)
        {
            return repository.GetLoanDisburse(OfficeId, vDate);
            // return repository.GetLoanDisburse(OfficeId, vDate);
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









        public IEnumerable<Proc_get_LoanDisburse_Result> GetLoanDisburse(int? OrgID,int? OfficeId, DateTime? vDate, string filterColumnName, string filterValue)
        {
            return repository.GetLoanDisburse(OrgID,OfficeId, vDate, filterColumnName, filterValue);
        }
        public IEnumerable<Proc_get_LoanDisburse_Result> GetLoanDisburseSms(int? OrgID, int? OfficeId, DateTime? vDate, string filterColumnName, string filterValue)
        {
            return repository.GetLoanDisburseSms(OrgID,OfficeId, vDate, filterColumnName, filterValue);
        }


        public LoanSummary GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }


        public IEnumerable<Proc_get_LoanDisburse_Result> GetPartialLoanDisburse(int? OrgID, int? OfficeId, DateTime? vDate, string filterColumnName, string filterValue)
        {
            return repository.GetPartialLoanDisburse(OrgID, OfficeId, vDate, filterColumnName, filterValue);
        }
        public IEnumerable<Proc_get_LoanDisburse_Result> GetPartialLoanDisburseCCLoan(int? OrgID, int? OfficeId, DateTime? vDate,  string filterColumnName, string filterValue, string filterCCLoan)
        {
            return repository.GetPartialLoanDisburseCCLoan(OrgID, OfficeId, vDate,  filterColumnName, filterValue, filterCCLoan);
        }

        public IEnumerable<Proc_get_LoanDisburse_Result> GetFirstLoanDisburse(int? OrgID, int? OfficeId, DateTime? vDate, string filterColumnName, string filterValue)
        {
            return repository.GetFirstLoanDisburse(OrgID, OfficeId, vDate, filterColumnName, filterValue);
        }

        public IEnumerable<LoanSummary> GetMany(Expression<Func<LoanSummary, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
