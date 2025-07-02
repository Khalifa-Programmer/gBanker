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
    public interface ICumAISService : IServiceBase<CumAI>
    {
        IEnumerable<Proc_Get_CUMAIS_Result> GetCumAISInfo(int? OfficeId, string filterColumnName, string filterValue);
    }
    public class CumAISService : ICumAISService
    {
        private readonly ICumAISRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public CumAISService(ICumAISRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<CumAI> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.AccCode);
            return entities;
        }

        public CumAI GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public CumAI Create(CumAI objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(CumAI objectToUpdate)
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

        public CumAI GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<Proc_Get_CUMAIS_Result> GetCumAISInfo(int? OfficeId, string filterColumnName, string filterValue)
        {
            return repository.GetCumAISInfo(OfficeId, filterColumnName, filterValue);
        }

        public IEnumerable<CumAI> GetMany(Expression<Func<CumAI, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
