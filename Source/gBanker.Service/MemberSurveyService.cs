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
    public interface ISurveyMemberMasterService : IServiceBase<SurveyMemberMaster>
    {
        IEnumerable<SurveyMemberMaster> GetSurveyCodeWishID(string accountnumber);
    }
    public class SurveyMemberMasterService : ISurveyMemberMasterService
    {
        private readonly ISurveyMemberMasterRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public SurveyMemberMasterService(ISurveyMemberMasterRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<SurveyMemberMaster> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.FirstName);
            return entities;
        }

        public SurveyMemberMaster GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public SurveyMemberMaster Create(SurveyMemberMaster objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(SurveyMemberMaster objectToUpdate)
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
        

        public void conditionalUpdate(SurveyMemberMaster SurveyMemberMaster)
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
        

        public SurveyMemberMaster GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }


        public IEnumerable<SurveyMemberMaster> GetSurveyCodeWishID(string SurveyCode)
        {
            return repository.GetMany(w => w.IsActive == true && w.SurveyCode == SurveyCode).ToList();
        }

        public IEnumerable<SurveyMemberMaster> GetMany(Expression<Func<SurveyMemberMaster, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
