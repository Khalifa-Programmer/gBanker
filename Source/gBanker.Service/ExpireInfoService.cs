using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration;
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
    public interface IExpireInfoService : IServiceBase<ExpireInfo>
    {
        int setExpireInfo(Nullable<int> officeID, Nullable<int> centerID, Nullable<long> memberID, string expiryName, string relation, Nullable<System.DateTime> expireDate, string remarks, Nullable<int> orgID, string createUser, Nullable<System.DateTime> createDate, Nullable<int> ExpireStatus,long? cLoanSummaryID, string ApplicantName, string MobileNumber );

        IEnumerable<getExpireInfo_Result> getExpireInfo(Nullable<int> officeId, string filterColumnName, string filterValue);
        IEnumerable<ValidationResult> IsValidExpireInfo(ExpireInfo expireInfo);
    }
    public class ExpireInfoService : IExpireInfoService
    {
        private readonly IExpireInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ExpireInfoService(IExpireInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<ExpireInfo> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.MemberID);
            return entities;
        }

        public ExpireInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public ExpireInfo Create(ExpireInfo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ExpireInfo objectToUpdate)
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

        public ExpireInfo GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<getExpireInfo_Result> getExpireInfo(int? officeId, string filterColumnName, string filterValue)
        {
            return repository.getExpireInfo(officeId,  filterColumnName, filterValue);
        }


       

        public IEnumerable<ValidationResult> IsValidExpireInfo(ExpireInfo expireInfo)
        {
            var expCheck = repository.Get(p => p.MemberID == expireInfo.MemberID && p.CenterID == expireInfo.CenterID && p.OfficeID == expireInfo.OfficeID && p.IsActive == true);
            if (expCheck != null)
            {
                yield return new ValidationResult("MemberID", "Pls. select valid Member");
            }
        }

        public int setExpireInfo(int? officeID, int? centerID, long? memberID, string expiryName, string relation, DateTime? expireDate, string remarks, int? orgID, string createUser, DateTime? createDate, int? ExpireStatus,long? cLoanSummaryID, string ApplicantName, string MobileNumber)
        {
            return repository.setExpireInfo(officeID, centerID, memberID, expiryName, relation, expireDate, remarks, orgID, createUser, createDate, ExpireStatus,cLoanSummaryID, ApplicantName, MobileNumber);
        }

        public IEnumerable<ExpireInfo> GetMany(Expression<Func<ExpireInfo, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
