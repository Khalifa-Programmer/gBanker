using gBanker.Core.Common;
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
    public interface ILoanApprovalService : IServiceBase<LoanSummary>
    {
        int getMaxLoanterm(LoanSummary loansummary);
        int getMaxLoantermEdit(LoanSummary loansummary);
        bool IsValidLoan(LoanSummary loansummary, out string msg);
        IEnumerable<ValidationResult> IsValidLoan(LoanSummary loansummary);
        IEnumerable<ValidationResult> IsValidLoanForEdit(LoanSummary loansummary);
    }
    public class LoanApprovalService : ILoanApprovalService
    {
        private readonly ILoanSummaryRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        private readonly IProductRepository productRepository;
        private readonly ILoanCollectionRepository loanTrxRepository;
        private readonly IMemberRepository memberRepository;
        public LoanApprovalService(ILoanSummaryRepository repository, IMemberRepository memberRepository, IProductRepository productRepository, ILoanCollectionRepository loanTrxRepository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.productRepository = productRepository;
            this.loanTrxRepository = loanTrxRepository;
            this.memberRepository = memberRepository;
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
            repository.Add(objectToCreate);
            Save();
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
                if (isActive == true)
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

        public bool IsValidLoan(LoanSummary loansummary, out string msg)
        {
            decimal dailyLoanInstallment;
            decimal dailyIntInstallment;
            decimal LoanSummaryPrincipalLoan;
            decimal LoanSummaryLoanReapid;
            decimal LoanSummaryInterestCharge;
            decimal LoanSummaryInterestPaid;
            decimal summaryresult;
            var validationMsg = "";
            var isValid = true;

            var entityCheck = repository.Get(p => p.OrgID==loansummary.OrgID && p.OfficeID == loansummary.OfficeID && p.MemberID == loansummary.MemberID && p.ProductID == loansummary.ProductID && p.CenterID == loansummary.CenterID && p.LoanTerm == loansummary.LoanTerm);
            if (entityCheck == null)
            {

                var vMaxLoanLerm = repository.GetMany(p => p.OrgID == loansummary.OrgID && p.OfficeID == loansummary.OfficeID && p.MemberID == loansummary.MemberID && p.ProductID == loansummary.ProductID && p.CenterID == loansummary.CenterID && p.LoanStatus == 1).Max();
                if (vMaxLoanLerm == null)
                {
                    isValid = false;
                    validationMsg = "This is no records";
                }
                int vloanTerm = vMaxLoanLerm.LoanTerm;
                var entity = repository.Get(p => p.OrgID == loansummary.OrgID && p.OfficeID == loansummary.OfficeID && p.MemberID == loansummary.MemberID && p.ProductID == loansummary.ProductID && p.CenterID == loansummary.CenterID && p.LoanTerm == vloanTerm && p.LoanStatus == 1);
                if (entity != null)
                {
                    var daiEntity = loanTrxRepository.Get(p => p.OrgID == loansummary.OrgID && p.OfficeID == entity.OfficeID && p.MemberID == entity.MemberID && p.ProductID == entity.ProductID && p.CenterID == entity.CenterID && p.LoanTerm == entity.LoanTerm);
                    if (daiEntity != null)
                    {
                        dailyLoanInstallment = daiEntity.LoanPaid;
                        dailyIntInstallment = daiEntity.IntPaid;
                    }
                    else
                    {
                        dailyLoanInstallment = 0;
                        dailyIntInstallment = 0;
                    }

                    LoanSummaryPrincipalLoan = entity.PrincipalLoan;
                    LoanSummaryLoanReapid = Convert.ToDecimal(entity.LoanRepaid);
                    LoanSummaryInterestPaid = Convert.ToDecimal(entity.IntPaid);
                    LoanSummaryInterestCharge = Convert.ToDecimal(entity.IntCharge);
                    summaryresult = (LoanSummaryPrincipalLoan + LoanSummaryInterestCharge) - (LoanSummaryLoanReapid + LoanSummaryInterestPaid + dailyLoanInstallment + dailyIntInstallment);
                    if (summaryresult > 0)
                    {
                        isValid = false;
                        validationMsg = "This Member has previous loan";
                    }
                }

                if (vloanTerm + 1 != loansummary.LoanTerm)
                {
                    isValid = false;
                    validationMsg = "This is not max Loanterm";
                }

                var product = productRepository.GetById(loansummary.ProductID);

                if (product != null)
                {
                    decimal vinterestRate = Convert.ToDecimal(product.InterestRate);
                    int vduration = Convert.ToInt16(product.Duration);
                    decimal vLoanAmount = Convert.ToDecimal(product.LoanInstallment);
                    decimal vInterestAmount = Convert.ToDecimal(product.InterestInstallment);

                    decimal vLoanInstallment = loansummary.PrincipalLoan * vLoanAmount;
                    decimal vInterestInstallment = loansummary.PrincipalLoan * vInterestAmount;
                    if (vLoanInstallment < loansummary.LoanInstallment)
                    {
                        isValid = false;
                        validationMsg = "Invalid Loan Installment exceeds.";
                    }

                    else if (vInterestInstallment < loansummary.IntInstallment)
                    {
                        isValid = false;
                        validationMsg = "Invalid Interest Installment exceeds.";
                    }
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
                }
            }
            else
                isValid = false;
                msg = validationMsg;
                return isValid;
        }



        public int getMaxLoanterm(LoanSummary loansummary)
        {
            int loanterm;
            var vMaxLoanLerm = repository.MaxLoanTerm(loansummary); // repository.GetMany(p => p.OfficeID == loansummary.OfficeID && p.MemberID == loansummary.MemberID && p.ProductID == loansummary.ProductID && p.CenterID == loansummary.CenterID).Max();
            if (vMaxLoanLerm == 0)
            { 
                loanterm=1;
            }
            else
            {
                loanterm = vMaxLoanLerm + 1;
            }

            return loanterm;
            


        }

        public IEnumerable<ValidationResult> IsValidLoan(LoanSummary loansummary)
        {
            decimal dailyLoanInstallment;
            decimal dailyIntInstallment;
            decimal LoanSummaryPrincipalLoan;
            decimal LoanSummaryLoanReapid;
            decimal LoanSummaryInterestCharge;
            decimal LoanSummaryInterestPaid;
            decimal summaryresult;
            int vloanTerm;



            var memCheck = memberRepository.Get(p => p.OrgID == loansummary.OrgID && p.MemberID == loansummary.MemberID && p.OfficeID == loansummary.OfficeID && p.CenterID == loansummary.CenterID && p.IsActive == true);
            if (memCheck == null)
            {
                yield return new ValidationResult("MemberID", "Pls. select valid Member");
            }
            else
            {
                //var entityCheck = repository.Get(p => p.OfficeID == loansummary.OfficeID && p.MemberID == loansummary.MemberID && p.ProductID == loansummary.ProductID && p.CenterID == loansummary.CenterID && p.LoanTerm == loansummary.LoanTerm && p.IsActive==true);
                var entityCheck = repository.Get(p => p.OrgID == loansummary.OrgID && p.OfficeID == loansummary.OfficeID && p.MemberID == loansummary.MemberID && p.ProductID == loansummary.ProductID && p.LoanTerm == loansummary.LoanTerm && p.IsActive == true);


                var vMaxLoanLerm = getMaxLoanterm(loansummary); // repository.GetMany(p => p.OfficeID == loansummary.OfficeID && p.MemberID == loansummary.MemberID && p.ProductID == loansummary.ProductID && p.CenterID == loansummary.CenterID).Max();

                if (vMaxLoanLerm > 1)
                {
                    vMaxLoanLerm = Convert.ToUInt16(vMaxLoanLerm - 1);
                    var entityLoanCheck = repository.Get(p => p.OrgID == loansummary.OrgID && p.OfficeID == loansummary.OfficeID && p.MemberID == loansummary.MemberID && p.ProductID == loansummary.ProductID && p.LoanTerm == vMaxLoanLerm && p.IsActive == true);
                    // var entityLoanCheck = repository.Get(p => p.OfficeID == loansummary.OfficeID && p.MemberID == loansummary.MemberID && p.ProductID == loansummary.ProductID && p.CenterID == loansummary.CenterID && p.LoanTerm == vMaxLoanLerm && p.IsActive==true);
                    if (entityLoanCheck.LoanStatus == 1)
                    {

                        var daiEntity = loanTrxRepository.GetAll().Where(p => p.OrgID == loansummary.OrgID && p.OfficeID == entityLoanCheck.OfficeID && p.MemberID == entityLoanCheck.MemberID && p.ProductID == entityLoanCheck.ProductID && p.LoanTerm == vMaxLoanLerm);

                        if (daiEntity != null)
                        {
                            var vLoanPaiddata = daiEntity.Sum(s => s.LoanPaid);
                            dailyLoanInstallment = vLoanPaiddata;
                            var vintPaiddata = daiEntity.Sum(s => s.IntPaid);
                            dailyIntInstallment = vintPaiddata;
                        }
                        else
                        {
                            dailyLoanInstallment = 0;
                            dailyIntInstallment = 0;
                        }

                        LoanSummaryPrincipalLoan = entityLoanCheck.PrincipalLoan;
                        LoanSummaryLoanReapid = Convert.ToDecimal(entityLoanCheck.LoanRepaid);
                        LoanSummaryInterestPaid = Convert.ToDecimal(entityLoanCheck.IntPaid);
                        LoanSummaryInterestCharge = Convert.ToDecimal(entityLoanCheck.IntCharge);
                        summaryresult = (LoanSummaryPrincipalLoan + LoanSummaryInterestCharge) - (LoanSummaryLoanReapid + LoanSummaryInterestPaid + dailyLoanInstallment + dailyIntInstallment);
                        //summaryresult = Math.Round((LoanSummaryPrincipalLoan + LoanSummaryInterestCharge) - (LoanSummaryLoanReapid + LoanSummaryInterestPaid + dailyLoanInstallment + dailyIntInstallment));
                        if (Convert.ToDouble(summaryresult) > 0.50)
                        {
                            yield return new ValidationResult("MemberID", "This member has previous loan");
                        }

                        //}

                        var product = productRepository.GetById(loansummary.ProductID);

                        if (product != null)
                        {
                            decimal vinterestRate = Convert.ToDecimal(product.InterestRate);
                            int vduration = Convert.ToInt16(product.Duration);
                            decimal vLoanAmount = Convert.ToDecimal(product.LoanInstallment);
                            decimal vInterestAmount = Convert.ToDecimal(product.InterestInstallment);

                            decimal vLoanInstallment = loansummary.PrincipalLoan * vLoanAmount;
                            decimal vInterestInstallment = loansummary.PrincipalLoan * vInterestAmount;

                            if (product.Duration.HasValue && product.Duration.Value < loansummary.Duration)
                            {
                                yield return new ValidationResult("Duration", "Duration Exceeds");
                            }
                            //mansur
                            else if (product.MaxLimit.HasValue && product.MaxLimit.Value < loansummary.PrincipalLoan)
                            {
                                yield return new ValidationResult("PrincipalLoan", "MaxLimit Exceeds");
                            }

                        }

                    }
                }
                else
                {
                    var product = productRepository.GetById(loansummary.ProductID);

                    if (product != null)
                    {
                        decimal vinterestRate = Convert.ToDecimal(product.InterestRate);
                        int vduration = Convert.ToInt16(product.Duration);
                        decimal vLoanAmount = Convert.ToDecimal(product.LoanInstallment);
                        decimal vInterestAmount = Convert.ToDecimal(product.InterestInstallment);

                        decimal vLoanInstallment = loansummary.PrincipalLoan * vLoanAmount;
                        decimal vInterestInstallment = loansummary.PrincipalLoan * vInterestAmount;

                        if (product.Duration.HasValue && product.Duration.Value < loansummary.Duration)
                        {
                            yield return new ValidationResult("Duration", "Duration Exceeds");
                        }
                        //mansur
                        else if (product.MaxLimit.HasValue && product.MaxLimit.Value < loansummary.PrincipalLoan)
                        {
                            yield return new ValidationResult("PrincipalLoan", "MaxLimit Exceeds");
                        }

                    }
                    else
                    {
                        yield return new ValidationResult("ProductID", "Pls. select valid Product");
                    }
                }

            }


        }


        public IEnumerable<ValidationResult> IsValidLoanForEdit(LoanSummary loansummary)
        {

            var memCheck = memberRepository.Get(p => p.OrgID==loansummary.OrgID && p.MemberID == loansummary.MemberID && p.OfficeID == loansummary.OfficeID && p.IsActive == true);
            if (memCheck == null)
            {
                yield return new ValidationResult("MemberID", "Pls. select valid Member");
            }
            else
            {

               
                var product = productRepository.GetById(loansummary.ProductID);

                if (product != null)
                {
                    decimal vinterestRate = Convert.ToDecimal(product.InterestRate);
                    int vduration = Convert.ToInt16(product.Duration);
                    decimal vLoanAmount = Convert.ToDecimal(product.LoanInstallment);
                    decimal vInterestAmount = Convert.ToDecimal(product.InterestInstallment);

                    decimal vLoanInstallment = loansummary.PrincipalLoan * vLoanAmount;
                    decimal vInterestInstallment = loansummary.PrincipalLoan * vInterestAmount;
                          

                    if (product.Duration.HasValue && product.Duration.Value < loansummary.Duration)
                    {
                        yield return new ValidationResult("Duration", "Duration Exceeds");
                    }
                    
                    else if (product.MaxLimit.HasValue && product.MaxLimit.Value < loansummary.PrincipalLoan)
                    {
                        yield return new ValidationResult("PrincipalLoan", "MaxLimit Exceeds");
                    }

                }
                else
                {
                    yield return new ValidationResult("ProductID", "Pls. select valid Product");
                }

            }

        }


        public int getMaxLoantermEdit(LoanSummary loansummary)
        {

            int loanterm;
            var vMaxLoanLerm = repository.MaxLoanTerm(loansummary); // repository.GetMany(p => p.OfficeID == loansummary.OfficeID && p.MemberID == loansummary.MemberID && p.ProductID == loansummary.ProductID && p.CenterID == loansummary.CenterID).Max();
            if (vMaxLoanLerm == 0)
            {
                loanterm = 1;
            }
            else
            {
                loanterm = vMaxLoanLerm;
            }

            return loanterm;
        }


        public LoanSummary GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<LoanSummary> GetMany(Expression<Func<LoanSummary, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
