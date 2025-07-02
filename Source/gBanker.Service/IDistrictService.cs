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
    public interface IDistrictService : IServiceBase<District>
    {
        IEnumerable<District> discodewiseupoinfo(string DistrictCode);
    }

    public class DistrictService : IDistrictService
    {
        private readonly IDistrictRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public DistrictService(IDistrictRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;

        }

        public IEnumerable<District> discodewiseupoinfo(string DistrictCode)
        {
            var entities = repository.GetAll().Where(w => w.DistrictCode == DistrictCode).OrderBy(o => o.DistrictName);
            return entities;
        }

        public District Create(District objectToCreate)
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

        public IEnumerable<District> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.DistrictID);
            return entities;
        }

        public District GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public District GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }
        public bool IsValidTargetAchievement(District TargetAchievement)
        {
            var entity = repository.Get(p => p.DistrictID == TargetAchievement.DistrictID);
            return entity == null ? true : false; ;
        }


        public IEnumerable<District> SearchTargetAchievement()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.DistrictID);
        }

        public IEnumerable<District> GetMany(Expression<Func<District, bool>> where)
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

        public void Update(District objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }
    }
}
