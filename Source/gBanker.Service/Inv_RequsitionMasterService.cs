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
    public interface IInv_RequsitionMasterService : IServiceBase<Inv_RequsitionMaster>
    {
       // List<Inv_ItemViewModel> GetAllItems<TParamOType>(TParamOType target);
    }
    public class Inv_RequsitionMasterService : IInv_RequsitionMasterService
    {
        private readonly IInv_RequsitionMasterRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public Inv_RequsitionMasterService(IInv_RequsitionMasterRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Inv_RequsitionMaster> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.RequsitionDate);
            return entities;
        }

        public Inv_RequsitionMaster GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Inv_RequsitionMaster Create(Inv_RequsitionMaster objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Inv_RequsitionMaster objectToUpdate)
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
        public bool IsValidInv_Items(Inv_RequsitionMaster Inv_Items)
        {
            var entity = repository.Get(p => p.RequsitionID == Inv_Items.RequsitionID);
            return entity == null ? true : false; ;
        }


        public IEnumerable<Inv_RequsitionMaster> SearchInv_Items()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.RequsitionID);
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
        public Inv_RequsitionMaster GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Inv_RequsitionMaster> GetMany(Expression<Func<Inv_RequsitionMaster, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
    }
}
