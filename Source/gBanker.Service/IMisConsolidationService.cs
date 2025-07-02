using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Service
{
    public interface IMISConsolidationProcessService : IServiceBase<MISConsolidationProcess>
    {

    }
    public class MISConsolidationProcessService : IMISConsolidationProcessService
    {
        private readonly IMISConsolidationProcessRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public MISConsolidationProcessService(IMISConsolidationProcessRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<MISConsolidationProcess> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.ConsolidationID);
            return entities;
        }       
        public MISConsolidationProcess GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public MISConsolidationProcess Create(MISConsolidationProcess objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(MISConsolidationProcess objectToUpdate)
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
            unitOfWork.Commit();
        }


        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException(); ;
        }


        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }


        public MISConsolidationProcess Get(Expression<Func<MISConsolidationProcess, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<MISConsolidationProcess> GetMany(Expression<Func<MISConsolidationProcess, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsRunning == true);
            return entities;
        }        
        public MISConsolidationProcess GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
      
    }
}
