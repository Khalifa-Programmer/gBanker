using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using gBanker.Data.DBDetailModels;

namespace gBanker.Service
{
    public interface IInv_TempStoreService : IServiceBase<Inv_TempStore>
    {
    }
    public class Inv_TempStoreService : IInv_TempStoreService
    {
        private readonly IInv_TempStoreRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public Inv_TempStoreService(IInv_TempStoreRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Inv_TempStore> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }

        public Inv_TempStore GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Inv_TempStore Create(Inv_TempStore objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Inv_TempStore objectToUpdate)
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
        public bool IsValidInv_TempStore(Inv_TempStore Inv_TempStore)
        {
            var entity = repository.Get(p => p.ItemID == Inv_TempStore.ItemID);
            return entity == null ? true : false; ;
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
        
        public bool IsContinued(long id)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                if (true)
                {
                    return false;
                }
            }

            return true;
        }
        public Inv_TempStore GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Inv_TempStore> GetMany(Expression<Func<Inv_TempStore, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }
    }
}
