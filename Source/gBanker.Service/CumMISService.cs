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
    public interface ICumMISService : IServiceBase<CumMi>
    {
        IEnumerable<Proc_Get_CUMMIS_Result> GetCumMISInfo(int? OfficeId, string filterColumnName, string filterValue);
    }
    public class CumMISService : ICumMISService
    {
        private readonly ICumMISRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public CumMISService(ICumMISRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<CumMi> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.CenterID);
            return entities;
        }

        public CumMi GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public CumMi Create(CumMi objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(CumMi objectToUpdate)
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

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public CumMi GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<Proc_Get_CUMMIS_Result> GetCumMISInfo(int? OfficeId,  string filterColumnName, string filterValue)
        {
            return repository.GetCumMISInfo(OfficeId, filterColumnName, filterValue);
        }

        public IEnumerable<CumMi> GetMany(Expression<Func<CumMi, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
