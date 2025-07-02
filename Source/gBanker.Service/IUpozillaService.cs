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
    public interface IUpozillaService : IServiceBase<Upozilla>
    {
        IEnumerable<Upozilla> upocodewiseuniinfo(string UpozillaCode);
    }

    public class UpozillaService : IUpozillaService
    {
        private readonly IUpozillaRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public UpozillaService(IUpozillaRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;

        }

        public IEnumerable<Upozilla> upocodewiseuniinfo(string UpozillaCode)
        {
            var entities = repository.GetAll().Where(w => w.UpozillaCode == UpozillaCode).OrderBy(o => o.UpozillaName);
            return entities;
        }

        public Upozilla Create(Upozilla objectToCreate)
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

        public IEnumerable<Upozilla> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.UpozillaID);
            return entities;
        }

        public Upozilla GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Upozilla GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }
        public bool IsValidTargetAchievement(Upozilla TargetAchievement)
        {
            var entity = repository.Get(p => p.UpozillaID == TargetAchievement.UpozillaID);
            return entity == null ? true : false; ;
        }


        public IEnumerable<Upozilla> SearchTargetAchievement()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.UpozillaID);
        }

        public IEnumerable<Upozilla> GetMany(Expression<Func<Upozilla, bool>> where)
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

        public void Update(Upozilla objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }        
    }
}

