using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface ISmsLogService : IServiceBase<SmsLog>
    {
        IEnumerable<ValidationResult> IsValidSmsLog(SmsLog note);
        IEnumerable<SmsLog> GetByOrgID(int orgId);
        SmsLog GetLoanCollectionSms(int org_id, long mem_id, DateTime trx_dt);
    }
    public class SmsLogService : ISmsLogService
    {
        private readonly ISmsLogRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public SmsLogService(ISmsLogRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<SmsLog> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.SmsLogID);
            return entities;
        }

        public SmsLog GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public IEnumerable<SmsLog> GetByOrgID(int orgId)
        {
            var entity = repository.GetMany(p => p.OrgID == orgId);
            return entity;
        }

        public SmsLog GetLoanCollectionSms(int org_id, long mem_id, DateTime trx_dt )
        {
            var entity = repository.GetAll().Where(w => w.OrgID == org_id && w.MemberID == mem_id && w.DateSent == trx_dt && w.SmsType == "C");
            //var result = repository.GetMany(g => g.OrgID == org_id && g.MemberID == mem_id && g.DateSent == trx_dt && g.SmsType=="C").Last();            
            //var result = null;
            SmsLog result = null;
            if(entity.Count() > 0)
            {
                result = entity.Last();
            }
            return result;            
        }

        public SmsLog Create(SmsLog objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(SmsLog objectToUpdate)
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
            throw new NotImplementedException();
        }
        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }
        IEnumerable<ValidationResult> ISmsLogService.IsValidSmsLog(SmsLog note)
        {
            var entity = repository.Get(p => p.SmsLogID == note.SmsLogID);
            if (entity != null)
            {
                yield return new ValidationResult("SmsID", "Duplicate SMS Credential.");

            }
        }



        public SmsLog GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SmsLog> GetMany(Expression<Func<SmsLog, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
