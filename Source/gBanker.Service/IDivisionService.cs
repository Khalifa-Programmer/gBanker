using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface IDivisionService : IServiceBase<Division>
    {
        IEnumerable<Division> divcodewisedisinfo(string DivisionCode);
    }

    public class DivisionService : IDivisionService
    {
        private readonly IDivisionRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public DivisionService(IDivisionRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;

        }

        public IEnumerable<Division> divcodewisedisinfo(string DivisionCode)
        {
            var entities = repository.GetAll().Where(w => w.DivisionCode == DivisionCode).OrderBy(o => o.DivisionName);
            return entities;
        }

        public Division Create(Division objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Delete(int id)
        {
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }

        public IEnumerable<Division> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.DivisionID);
            return entities;
        }

        public Division GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Division GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }
        public bool IsValidTargetAchievement(Division TargetAchievement)
        {
            var entity = repository.Get(p => p.DivisionID == TargetAchievement.DivisionID);
            return entity == null ? true : false; ;
        }


        public IEnumerable<Division> SearchTargetAchievement()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.DivisionID);
        }

        public IEnumerable<Division> GetMany(Expression<Func<Division, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                //obj.InActiveDate = inactiveDate.HasValue ? inactiveDate : DateTime.Now;
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

        public void Save()
        {
            unitOfWork.Commit();
        }

        public void Update(Division objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }
    }
}