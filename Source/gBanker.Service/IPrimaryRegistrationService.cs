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
    public interface IPrimaryRegistrationService : IServiceBase<PrimaryRegistration>
    { }
    public class PrimaryRegistrationService : IPrimaryRegistrationService
    {
        private readonly IPrimaryRegistrationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public PrimaryRegistrationService(IPrimaryRegistrationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<PrimaryRegistration> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.PrimaryRegistrationID);
            return entities;
        }

        public PrimaryRegistration GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public PrimaryRegistration Create(PrimaryRegistration objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(PrimaryRegistration objectToUpdate)
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
        public bool IsValidPrimaryRegistration(PrimaryRegistration PrimaryRegistration)
        {
            var entity = repository.Get(p => p.PrimaryRegistrationID == PrimaryRegistration.PrimaryRegistrationID);
            return entity == null ? true : false; ;
        }


        public IEnumerable<PrimaryRegistration> SearchPrimaryRegistration()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.PrimaryRegistrationID);
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


        public PrimaryRegistration GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PrimaryRegistration> GetMany(Expression<Func<PrimaryRegistration, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
