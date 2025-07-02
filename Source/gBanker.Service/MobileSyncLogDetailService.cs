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
    public interface IMobileSyncLogDetailService : IServiceBase<MobileSyncLogDetail>
    {

    }
    public class MobileSyncLogDetailService : IMobileSyncLogDetailService
    {
        private readonly IMobileSyncLogDetailRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public MobileSyncLogDetailService(IMobileSyncLogDetailRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public MobileSyncLogDetail Create(MobileSyncLogDetail objectToCreate)
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

        public IEnumerable<MobileSyncLogDetail> GetAll()
        {
            throw new NotImplementedException();
        }

        public MobileSyncLogDetail GetById(int id)
        {
            throw new NotImplementedException();
        }

        public MobileSyncLogDetail GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MobileSyncLogDetail> GetMany(Expression<Func<MobileSyncLogDetail, bool>> where)
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

        public void Update(MobileSyncLogDetail objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }
    }
}
