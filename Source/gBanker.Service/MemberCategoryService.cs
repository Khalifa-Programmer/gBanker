using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration.Db;
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

    public interface IMemberCategoryService: IServiceBase<MemberCategory>  
    {
        //IEnumerable<MemberCategory> GetAll();
        //MemberCategory GetById(int id);
        //MemberCategory Create(MemberCategory objectToCreate);
        //void Update(MemberCategory objectToUpdate);
        //void Delete(int id);
        //void Save();
        IEnumerable<ValidationResult> IsValidMemberCategory(MemberCategory membercategory);
    }
    public class MemberCategoryService : IMemberCategoryService
    {
        private readonly IMemberCategoryRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public MemberCategoryService(IMemberCategoryRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<MemberCategory> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.CategoryName);
            return entities;
        }

        public MemberCategory GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public MemberCategory Create(MemberCategory objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(MemberCategory objectToUpdate)
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
                if (isActive == true)
                {
                    return false;
                }
            }

            return true;
        }

        public IEnumerable<ValidationResult> IsValidMemberCategory(MemberCategory membercategory)
        {
            var entity = repository.Get(p => p.MemberCategoryCode == membercategory.MemberCategoryCode && p.IsActive==true);

            if (entity != null)
            {

                yield return new ValidationResult("MemberCategoryCode", "Duplicate Category.");

            }
        }


        public MemberCategory GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<MemberCategory> GetMany(Expression<Func<MemberCategory, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
