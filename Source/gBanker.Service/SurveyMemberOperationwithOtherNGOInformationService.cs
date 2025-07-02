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
using gBanker.Data.CodeFirstMigration;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface ISurveyMemberOperationwithOtherNGOInformationService : IServiceBase<SurveyMemberOperationwithOtherNGOInformation>
    {
    }
    public class SurveyMemberOperationwithOtherNGOInformationService : ISurveyMemberOperationwithOtherNGOInformationService
    {
        private readonly ISurveyMemberOperationwithOtherNGOInformationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public SurveyMemberOperationwithOtherNGOInformationService(ISurveyMemberOperationwithOtherNGOInformationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<SurveyMemberOperationwithOtherNGOInformation> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.LoanAmount);
            return entities;
        }

        public SurveyMemberOperationwithOtherNGOInformation GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public SurveyMemberOperationwithOtherNGOInformation Create(SurveyMemberOperationwithOtherNGOInformation objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(SurveyMemberOperationwithOtherNGOInformation objectToUpdate)
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


        public void conditionalUpdate(SurveyMemberOperationwithOtherNGOInformation SurveyMemberMaster)
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


        public SurveyMemberOperationwithOtherNGOInformation GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<SurveyMemberOperationwithOtherNGOInformation> GetMany(Expression<Func<SurveyMemberOperationwithOtherNGOInformation, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
