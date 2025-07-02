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
    public interface ILegalInfoService : IServiceBase<LegalInfo>
    {
        IEnumerable<LegalInfo> divcodewisedisinfo(string LegalInfoCode);
    }

    public class LegalInfoService : ILegalInfoService
    {
        private readonly ILegalInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public LegalInfoService(ILegalInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;

        }

        public IEnumerable<LegalInfo> divcodewisedisinfo(string LegalInfoCode)
        {
            var entities = repository.GetAll();
            return entities;
        }

        public LegalInfo Create(LegalInfo objectToCreate)
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

        public IEnumerable<LegalInfo> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.OfficeID);
            return entities;
        }

        public LegalInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public LegalInfo GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }  
        public IEnumerable<LegalInfo> GetMany(Expression<Func<LegalInfo, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {               
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }       

        public void Save()
        {
            unitOfWork.Commit();
        }

        public void Update(LegalInfo objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }
    }
}