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
    public interface ILastAssetCodeInfoService : IServiceBase<LastAssetCodeInfo>
    {

    }
    public class LastAssetCodeInfoService : ILastAssetCodeInfoService
    {
        private readonly ILastAssetCodeInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public LastAssetCodeInfoService(ILastAssetCodeInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<LastAssetCodeInfo> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.Id);
            return entities;
        }

        public LastAssetCodeInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public LastAssetCodeInfo Create(LastAssetCodeInfo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(LastAssetCodeInfo objectToUpdate)
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

        public LastAssetCodeInfo GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LastAssetCodeInfo> GetMany(Expression<Func<LastAssetCodeInfo, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
