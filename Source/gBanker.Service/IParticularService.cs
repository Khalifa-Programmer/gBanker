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
    public interface IParticularService : IServiceBase<Particular>
    { }
    public class ParticularService : IParticularService
    {
        private readonly IParticularRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ParticularService(IParticularRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Particular> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.ParticularId);
            return entities;
        }

        public Particular GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Particular Create(Particular objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Particular objectToUpdate)
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
        public bool IsValidParticular(Particular Particular)
        {
            var entity = repository.Get(p => p.ParticularId == Particular.ParticularId);
            return entity == null ? true : false; ;
        }


        public IEnumerable<Particular> SearchParticular()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.ParticularId);
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
        public Particular GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Particular> GetMany(Expression<Func<Particular, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
