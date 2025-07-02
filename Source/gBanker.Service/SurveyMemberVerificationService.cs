using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
//using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface ISurveyMemberVerificationService : IServiceBase<SurveyMemberVerification>
    {
        SurveyMemberVerification GetByGurId(long SurveyId);
    }
    public class SurveyMemberVerificationService : ISurveyMemberVerificationService
    {
        private readonly ISurveyMemberVerificationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public SurveyMemberVerificationService(ISurveyMemberVerificationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<SurveyMemberVerification> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.VarificationNo);
            return entities;
        }

        public SurveyMemberVerification GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public SurveyMemberVerification Create(SurveyMemberVerification objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(SurveyMemberVerification objectToUpdate)
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


        public void conditionalUpdate(SurveyMemberVerification SurveyMemberMaster)
        {
            repository.Update(SurveyMemberMaster);
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


        public SurveyMemberVerification GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }



        public SurveyMemberVerification GetByGurId(long SurveyId)
        {
            var entity = repository.Get(e => e.SMVerificationId == SurveyId && e.IsActive == true);
            return entity;
        }

        public IEnumerable<SurveyMemberVerification> GetMany(Expression<Func<SurveyMemberVerification, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
