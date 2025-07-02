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

namespace gBanker.Service
{
    public interface IMRAActivityListService : IServiceBase<MRAActivityList>
    { }
    public class MRAActivityListService : IMRAActivityListService
    {
        private readonly IMRAActivityListRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public MRAActivityListService(IMRAActivityListRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<MRAActivityList> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.ActivityId);
            return entities;
        }

        public MRAActivityList GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public MRAActivityList Create(MRAActivityList objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(MRAActivityList objectToUpdate)
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
        public bool IsValidMRAActivityList(MRAActivityList MRAActivityList)
        {
            var entity = repository.Get(p => p.ActivityId == MRAActivityList.ActivityId);
            return entity == null ? true : false; ;
        }


        public IEnumerable<MRAActivityList> SearchMRAActivityList()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.ActivityId);
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


        public MRAActivityList GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MRAActivityList> GetMany(Expression<Func<MRAActivityList, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
