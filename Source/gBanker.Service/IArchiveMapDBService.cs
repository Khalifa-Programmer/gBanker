using gBanker.Data;
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
    public interface IArchiveDbMappingService : IServiceBase<ArchiveDbMapping>
    {


    }
    public class ArchiveDbMappingService : IArchiveDbMappingService
    {
        private readonly IArchiveDbMappingRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ArchiveDbMappingService(IArchiveDbMappingRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<ArchiveDbMapping> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.OrgId);
            return entities;
        }

        public ArchiveDbMapping GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public ArchiveDbMapping Create(ArchiveDbMapping objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ArchiveDbMapping objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
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

        public ArchiveDbMapping Get(Expression<Func<ArchiveDbMapping, bool>> where)
        {
            var entities = repository.Get(where);
            return entities;
        }
        public IEnumerable<ArchiveDbMapping> GetMany(Expression<Func<ArchiveDbMapping, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }
        public ArchiveDbMapping GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

    }
}
