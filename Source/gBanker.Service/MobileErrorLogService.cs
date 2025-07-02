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
    public interface IMobileErrorLogService : IServiceBase<MobileErrorLog>
    {
        
    }
    public class MobileErrorLogService : IMobileErrorLogService
    {
        private readonly IMobileErrorLogRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public MobileErrorLogService(IMobileErrorLogRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public MobileErrorLog Create(MobileErrorLog objectToCreate)
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

        public IEnumerable<MobileErrorLog> GetAll()
        {
            throw new NotImplementedException();
        }

        public MobileErrorLog GetById(int id)
        {
            throw new NotImplementedException();
        }

        public MobileErrorLog GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MobileErrorLog> GetMany(Expression<Func<MobileErrorLog, bool>> where)
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

        public void Update(MobileErrorLog objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }
    }
}
