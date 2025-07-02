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
    public interface IInv_VendorService : IServiceBase<Inv_Vendor>
    {
       // List<Inv_ItemViewModel> GetAllItems<TParamOType>(TParamOType target);
    }
    public class Inv_VendorService : IInv_VendorService
    {
        private readonly IInv_VendorRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public Inv_VendorService(IInv_VendorRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Inv_Vendor> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.VendorID);
            return entities;
        }

        public Inv_Vendor GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Inv_Vendor Create(Inv_Vendor objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Inv_Vendor objectToUpdate)
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
        public bool IsValidInv_Items(Inv_Vendor Inv_Items)
        {
            var entity = repository.Get(p => p.VendorID == Inv_Items.VendorID);
            return entity == null ? true : false; ;
        }


        public IEnumerable<Inv_Vendor> SearchInv_Items()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.VendorID);
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

        
        //public List<Inv_ItemViewModel> GetAllItems<TParamOType>(TParamOType target)
        //{
        //    return repository.GetSqlResult<Inv_ItemViewModel, TParamOType>("Inv_sp_Item @itemID", target).ToList();
        //}
        
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
        public Inv_Vendor GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Inv_Vendor> GetMany(Expression<Func<Inv_Vendor, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
    }
}
