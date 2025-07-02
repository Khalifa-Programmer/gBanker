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
    public interface IInv_ItemsService : IServiceBase<Inv_Items>
    {
        List<Inv_ItemViewModel> GetAllItems<TParamOType>(TParamOType target);
    }
    public class Inv_ItemsService : IInv_ItemsService
    {
        private readonly IInv_ItemsRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public Inv_ItemsService(IInv_ItemsRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Inv_Items> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.ItemID);
            return entities;
        }

        public Inv_Items GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Inv_Items Create(Inv_Items objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Inv_Items objectToUpdate)
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
        public bool IsValidInv_Items(Inv_Items Inv_Items)
        {
            var entity = repository.Get(p => p.ItemID == Inv_Items.ItemID);
            return entity == null ? true : false; ;
        }


        public IEnumerable<Inv_Items> SearchInv_Items()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.ItemID);
        }


        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.IsActive = false;
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }

        
        public List<Inv_ItemViewModel> GetAllItems<TParamOType>(TParamOType target)
        {
            return repository.GetSqlResult<Inv_ItemViewModel, TParamOType>("inv.Inv_sp_Item @itemID", target).ToList();
        }
        
        public bool IsContinued(long id)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                var isActive = obj.IsActive;
                if (isActive == true)
                {
                    return false;
                }
            }

            return true;
        }
        public Inv_Items GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Inv_Items> GetMany(Expression<Func<Inv_Items, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
    }
}
