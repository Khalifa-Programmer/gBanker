using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface IInv_RequsitionDetailsService : IServiceBase<Inv_RequsitionDetails>
    {
       // List<Inv_ItemViewModel> GetAllItems<TParamOType>(TParamOType target);
    }
    public class Inv_RequsitionDetailsService : IInv_RequsitionDetailsService
    {
        private readonly IInv_RequsitionDetailsRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public Inv_RequsitionDetailsService(IInv_RequsitionDetailsRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Inv_RequsitionDetails> GetAll()
        {
            var entities = repository.GetMany(g => g.AprovedStatus == g.AprovedStatus).OrderBy(c => c.ID);
            return entities;
        }

        public Inv_RequsitionDetails GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Inv_RequsitionDetails Create(Inv_RequsitionDetails objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Inv_RequsitionDetails objectToUpdate)
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
        public bool IsValidInv_Items(Inv_RequsitionDetails Inv_Items)
        {
            var entity = repository.Get(p => p.ID == Inv_Items.ID);
            return entity == null ? true : false; ;
        }


        public IEnumerable<Inv_RequsitionDetails> SearchInv_Items()
        {
            return repository.GetMany(g => g.AprovedStatus == g.AprovedStatus).OrderBy(g => g.ID);
        }


        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.AprovedStatus = "";
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
                var isActive = false;
                if (isActive == true)
                    return false;
            }

            return true;
        }
        public Inv_RequsitionDetails GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Inv_RequsitionDetails> GetMany(Expression<Func<Inv_RequsitionDetails, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.AprovedStatus == b.AprovedStatus);
            return entities;
        }
    }
}
