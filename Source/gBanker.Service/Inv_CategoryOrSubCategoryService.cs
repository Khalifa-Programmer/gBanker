using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using gBanker.Data.DBDetailModels;

namespace gBanker.Service
{
    public interface IInv_CategoryOrSubCategoryService : IServiceBase<Inv_CategoryOrSubCategory>
    {
        List<Inv_SubcategoryViewModel> GetAllSubCategory<TParamOType>(TParamOType target);
    }
    public class Inv_CategoryOrSubCategoryService : IInv_CategoryOrSubCategoryService
    {
        private readonly IInv_CategoryOrSubCategoryRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public Inv_CategoryOrSubCategoryService(IInv_CategoryOrSubCategoryRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Inv_CategoryOrSubCategory> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.CategorySubCategoryID);
            return entities;
        }

        public Inv_CategoryOrSubCategory GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Inv_CategoryOrSubCategory Create(Inv_CategoryOrSubCategory objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Inv_CategoryOrSubCategory objectToUpdate)
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
            //throw new NotImplementedException();
            unitOfWork.Commit();
        }
        public bool IsValidInv_CategoryOrSubCategory(Inv_CategoryOrSubCategory Inv_CategoryOrSubCategory)
        {
            var entity = repository.Get(p => p.CategorySubCategoryID == Inv_CategoryOrSubCategory.CategorySubCategoryID);
            return entity == null ? true : false; ;
        }

        
        public IEnumerable<Inv_CategoryOrSubCategory> SearchInv_CategoryOrSubCategory()
        {
            return repository.GetMany(g => g.ParentCategoryID == 0 && g.IsActive==true);
        }

        public List<Inv_SubcategoryViewModel> GetAllSubCategory<TParamOType>(TParamOType target)
        {
            return repository.GetSqlResult<Inv_SubcategoryViewModel, TParamOType>("inv.Inv_sp_Subcategory @isactive", target).ToList();
        }
        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.IsActive = false;
                obj.UpdateDate = inactiveDate;
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
                var isActive = true;
                if (isActive == true)
                {
                    return false;
                }
            }

            return true;
        }
        public Inv_CategoryOrSubCategory GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Inv_CategoryOrSubCategory> GetMany(Expression<Func<Inv_CategoryOrSubCategory, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
