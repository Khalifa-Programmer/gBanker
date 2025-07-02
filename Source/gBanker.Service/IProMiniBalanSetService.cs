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
    public interface IProMiniBalanSetService : IServiceBase<ProMiniBalanSet>
    { }
    public class ProMiniBalanSetService : IProMiniBalanSetService
    {
        private readonly IProMiniBalanSetRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ProMiniBalanSetService(IProMiniBalanSetRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<ProMiniBalanSet> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.ProductId);
            return entities;
        }

        public ProMiniBalanSet GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public ProMiniBalanSet Create(ProMiniBalanSet objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ProMiniBalanSet objectToUpdate)
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
        public bool IsValidProMiniBalanSet(ProMiniBalanSet ProMiniBalanSet)
        {
            var entity = repository.Get(p => p.MinimumBalanceID == ProMiniBalanSet.MinimumBalanceID);
            return entity == null ? true : false; ;
        }


        public IEnumerable<ProMiniBalanSet> SearchProMiniBalanSet()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.MinimumBalanceID);
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


        public ProMiniBalanSet GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProMiniBalanSet> GetMany(Expression<Func<ProMiniBalanSet, bool>> where)
        {
            throw new NotImplementedException();
        }

        //public ProMiniBalanSet GetById(int id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
