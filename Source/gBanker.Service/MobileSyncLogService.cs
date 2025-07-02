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
    public interface IMobileSyncLogService : IServiceBase<MobileSyncLog>
    {

    }
    public class MobileSyncLogService : IMobileSyncLogService
    {
        private readonly IMobileSyncLogRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public MobileSyncLogService(IMobileSyncLogRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public MobileSyncLog Create(MobileSyncLog objectToCreate)
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

        public IEnumerable<MobileSyncLog> GetAll()
        {
            throw new NotImplementedException();
        }

        public MobileSyncLog GetById(int id)
        {
            throw new NotImplementedException();
        }

        public MobileSyncLog GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MobileSyncLog> GetMany(Expression<Func<MobileSyncLog, bool>> where)
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

        public void Update(MobileSyncLog objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }
    }
}
