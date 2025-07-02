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
    public interface IApplicationLogService : IServiceBase<ApplicationLog>
    {
        IEnumerable<ApplicationLog> GetApplicationLogPaged(string organizationId, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount);
        IQueryable<ApplicationLog> GetAllQueryable();
    }
    public class ApplicationLogService : IApplicationLogService
    {
        private readonly IApplicationLogRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ApplicationLogService(IApplicationLogRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<ApplicationLog> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }

        public ApplicationLog GetById(int id)
        {
            //throw new NotImplementedException();
            var entity = repository.GetById(id);
            return entity;
        }

        public ApplicationLog Create(ApplicationLog objectToCreate)
        {
            //throw new NotImplementedException();
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ApplicationLog objectToUpdate)
        {
            //throw new NotImplementedException();
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            //throw new NotImplementedException();
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

        public IEnumerable<ApplicationLog> GetApplicationLogPaged(string organizationId, string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount)
        {
            return repository.GetApplicationLogPaged(organizationId, filterColumnName, filterValue, startRowIndex, pageSize, out totalCount);
        }


        public ApplicationLog GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ApplicationLog> GetAllQueryable()
        {
            return repository.GetAllQueryable();
        }

        public IEnumerable<ApplicationLog> GetMany(Expression<Func<ApplicationLog, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
