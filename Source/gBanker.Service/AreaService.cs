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
    public interface IAreaService : IServiceBase<Area>
    { }
    public class AreaService : IAreaService
    {
        private readonly IAreaRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AreaService(IAreaRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Area> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive==true).OrderBy(c => c.AreaCode);
            return entities;
        }

        public Area GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Area Create(Area objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Area objectToUpdate)
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
        public bool IsValidArea(Area area)
        {
            var entity = repository.Get(p => p.AreaCode == area.AreaCode);
            return entity == null ? true : false; ;
        }


        public IEnumerable<Area> SearchArea()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.AreaCode);
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


        public Area GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Area> GetMany(Expression<Func<Area, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
