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
    public interface ISurveyMemberFamilyInformationService : IServiceBase<SurveyMemberFamilyInformation>
    {
    }
    public class SurveyMemberFamilyInformationService : ISurveyMemberFamilyInformationService
    {
        private readonly ISurveyMemberFamilyInformationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public SurveyMemberFamilyInformationService(ISurveyMemberFamilyInformationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<SurveyMemberFamilyInformation> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.IncomeMonthly);
            return entities;
        }

        public SurveyMemberFamilyInformation GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public SurveyMemberFamilyInformation Create(SurveyMemberFamilyInformation objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(SurveyMemberFamilyInformation objectToUpdate)
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


        public void conditionalUpdate(SurveyMemberFamilyInformation SurveyMemberMaster)
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


        public SurveyMemberFamilyInformation GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<SurveyMemberFamilyInformation> GetMany(Expression<Func<SurveyMemberFamilyInformation, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
