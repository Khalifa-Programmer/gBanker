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
    public interface IView_SurveyMemberAssetService : IServiceBase<View_SurveyMemberAsset>
    {
    }
    public class View_SurveyMemberAssetService : IView_SurveyMemberAssetService
    {
        private readonly IView_SurveyMemberAssetRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public View_SurveyMemberAssetService(IView_SurveyMemberAssetRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<View_SurveyMemberAsset> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.RowSl);
            return entities;
        }

        public View_SurveyMemberAsset GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public View_SurveyMemberAsset Create(View_SurveyMemberAsset objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(View_SurveyMemberAsset objectToUpdate)
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


        public void conditionalUpdate(View_SurveyMemberAsset SurveyMemberMaster)
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


        public View_SurveyMemberAsset GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<View_SurveyMemberAsset> GetMany(Expression<Func<View_SurveyMemberAsset, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}

