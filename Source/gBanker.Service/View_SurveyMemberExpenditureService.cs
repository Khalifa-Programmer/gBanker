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
    public interface IView_SurveyMemberExpenditureService : IServiceBase<View_SurveyMemberExpenditure>
    {
    }
    public class View_SurveyMemberExpenditureService : IView_SurveyMemberExpenditureService
    {
        private readonly IView_SurveyMemberExpenditureRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public View_SurveyMemberExpenditureService(IView_SurveyMemberExpenditureRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<View_SurveyMemberExpenditure> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.RowSl);
            return entities;
        }

        public View_SurveyMemberExpenditure GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public View_SurveyMemberExpenditure Create(View_SurveyMemberExpenditure objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(View_SurveyMemberExpenditure objectToUpdate)
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


        public void conditionalUpdate(View_SurveyMemberExpenditure SurveyMemberMaster)
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


        public View_SurveyMemberExpenditure GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<View_SurveyMemberExpenditure> GetMany(Expression<Func<View_SurveyMemberExpenditure, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}

