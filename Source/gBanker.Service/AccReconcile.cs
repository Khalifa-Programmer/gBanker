using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration;
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
    public interface IAccReconcileService : IServiceBase<AccReconcile>
    {
        AccReconcile GetById(long id);
    }
    public class AccReconcileService : IAccReconcileService
    {
        private readonly IAccReconcileRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AccReconcileService(IAccReconcileRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AccReconcile> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.ReffNo);
            return entities;
        }

        public AccReconcile GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public AccReconcile GetById(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public AccReconcile GetByReffNo(string ReffNo)
        {
            var entity = repository.Get(p => p.ReffNo == ReffNo);
            return entity;
        }

      

        public AccReconcile Create(AccReconcile objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AccReconcile objectToUpdate)
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
        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }
        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }
     


        public AccReconcile GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        //public IEnumerable<AccReconcile> GetMany(Expression<Func<AccReconcile, bool>> where)
        //{
        //    throw new NotImplementedException();
        //}
        public IEnumerable<AccReconcile> GetMany(Expression<Func<AccReconcile, bool>> where)
        {
            // throw new NotImplementedException();
            //mahfuz
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

    }
}
