using gBanker.Data.CodeFirstMigration.Db;
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
    public interface IApproveCellingService : IServiceBase<ApproveCelling>
    {
        IEnumerable<ApproveCelling> GetApproveCellingDetail();
        ApproveCelling GetApproveCellingbyroleAndproductId(int roleID, int? productId);
    }
    public class ApproveCellingService : IApproveCellingService
    {
        private readonly IApproveCellingRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ApproveCellingService(IApproveCellingRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public ApproveCelling Create(ApproveCelling objectToCreate)
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

        public IEnumerable<ApproveCelling> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }

        public ApproveCelling GetApproveCellingbyroleAndproductId(int roleID, int? productId)
        {
            return repository.GetApproveCellingbyroleAndproductId(roleID, productId);
        }


        public IEnumerable<ApproveCelling> GetApproveCellingDetail()
        {
            return repository.GetApproveCellingDetail();
        }

        public ApproveCelling GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public ApproveCelling GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<ApproveCelling> GetMany(Expression<Func<ApproveCelling, bool>> where)
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

        public void Update(ApproveCelling objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }
    }
}
