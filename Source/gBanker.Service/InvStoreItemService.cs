using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface IInvStoreItemService : IServiceBase<InvStoreItem>
    {
        List<Inv_WarehouseViewModel> GetAllStoreItem<TParamOType>(TParamOType target);
        IEnumerable<InvStoreItem> GetCheck(int warehouseID, int itemID);
    }
    public class InvStoreItemService : IInvStoreItemService
    {
        private readonly IInvStoreItemRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public InvStoreItemService(IInvStoreItemRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<InvStoreItem> GetAll()
        {
            return repository.GetMany(g => g.IsActive == true);
        }
        public List<Inv_WarehouseViewModel> GetAllStoreItem<TParamOType>(TParamOType target)
        {
            return repository.GetSqlResult<Inv_WarehouseViewModel, TParamOType>("sp_StoreItem @officeID,@warehouseID", target).ToList();
        }

        public IEnumerable<InvStoreItem> GetCheck(int warehouseID, int itemID)
        {
            return repository.GetMany(gm => gm.WarehouseID == warehouseID && gm.ItemID==itemID && gm.IsActive == true);
        }
        public InvStoreItem GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public InvStoreItem Create(InvStoreItem objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(InvStoreItem objectToUpdate)
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
        public bool IsValidInv_Items(InvStoreItem Inv_Items)
        {
            var entity = repository.Get(p => p.WarehouseID == Inv_Items.WarehouseID);
            return entity == null ? true : false; ;
        }


        public IEnumerable<InvStoreItem> SearchInv_Items()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.WarehouseID);
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
        public InvStoreItem GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InvStoreItem> GetMany(Expression<Func<InvStoreItem, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
