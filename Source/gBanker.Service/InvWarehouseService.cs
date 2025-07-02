using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using gBanker.Data.DBDetailModels;

namespace gBanker.Service
{
    public interface IInvWarehouseService : IServiceBase<InvWarehouse>
    {
        IEnumerable<InvWarehouse> GetOfficeIDWise(int officeID);
    }
    public class InvWarehouseService : IInvWarehouseService
    {
        private readonly IInvWarehouseRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public InvWarehouseService(IInvWarehouseRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<InvWarehouse> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.OfficeID);
            return entities;
        }
        public IEnumerable<InvWarehouse> GetGridData(int rowLimit, int pageNo, out int totalRow)
        {
            totalRow = repository.GetMany(gm => gm.IsActive == true).Count();
            return repository.GetMany(gm => gm.IsActive == true).Skip(((pageNo - 1) * rowLimit)).Take(rowLimit);
        }
        public IEnumerable<InvWarehouse> GetOfficeIDWise(int officeID)
        {
            return repository.GetMany(gm => gm.OfficeID == officeID && gm.IsActive == true);
        }
        public IEnumerable<InvWarehouse> GetWarehouseName(string name)
        {
            return repository.GetMany(gm => gm.WarehouseName == name && gm.IsActive == true);
        }
        public InvWarehouse GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public InvWarehouse Create(InvWarehouse objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(InvWarehouse objectToUpdate)
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
        public bool IsValidInv_Items(InvWarehouse Inv_Items)
        {
            var entity = repository.Get(p => p.WarehouseID == Inv_Items.WarehouseID);
            return entity == null ? true : false; ;
        }


        public IEnumerable<InvWarehouse> SearchInv_Items()
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
        public InvWarehouse GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InvWarehouse> GetMany(Expression<Func<InvWarehouse, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
    }
}
