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
    public interface IMemberBlackListService : IServiceBase<MemberBlackList>
    { }
    public class MemberBlackListService : IMemberBlackListService
    {
        private readonly IMemberBlackListRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public MemberBlackListService(IMemberBlackListRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<MemberBlackList> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive==true).OrderBy(c => c.MemberCode);
            return entities;
        }

        public MemberBlackList GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public MemberBlackList Create(MemberBlackList objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(MemberBlackList objectToUpdate)
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

        public IEnumerable<MemberBlackList> SearchMemberBlackList()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.MemberCode);
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

        MemberBlackList IServiceBase<MemberBlackList>.GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MemberBlackList> GetMany(Expression<Func<MemberBlackList, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
