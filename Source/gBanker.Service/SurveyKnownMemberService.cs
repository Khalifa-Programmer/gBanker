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
    public interface ISurveyKnownMemberService : IServiceBase<SurveyKnownMember>
    {
    }
    public class SurveyKnownMemberService : ISurveyKnownMemberService
    {
        private readonly ISurveyKnownMemberRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public SurveyKnownMemberService(ISurveyKnownMemberRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<SurveyKnownMember> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.MemberCode);
            return entities;
        }

        public SurveyKnownMember GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public SurveyKnownMember Create(SurveyKnownMember objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(SurveyKnownMember objectToUpdate)
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


        public void conditionalUpdate(SurveyKnownMember SurveyMemberMaster)
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


        public SurveyKnownMember GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<SurveyKnownMember> GetMany(Expression<Func<SurveyKnownMember, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
