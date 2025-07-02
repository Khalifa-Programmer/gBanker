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

namespace gBanker.Service
{
    public interface IInv_ItemPriceDetailsService : IServiceBase<Inv_ItemPriceDetails>
    { }
    public class Inv_ItemPriceDetailsService : IInv_ItemPriceDetailsService
    {
        private readonly IInv_ItemPriceDetailsRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public Inv_ItemPriceDetailsService(IInv_ItemPriceDetailsRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Inv_ItemPriceDetails> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.ItemPriceSetID);
            return entities;
        }

        public Inv_ItemPriceDetails GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Inv_ItemPriceDetails Create(Inv_ItemPriceDetails objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Inv_ItemPriceDetails objectToUpdate)
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
        public bool IsValidInv_ItemPriceDetails(Inv_ItemPriceDetails Inv_ItemPriceDetails)
        {
            var entity = repository.Get(p => p.ItemPriceSetID == Inv_ItemPriceDetails.ItemPriceSetID);
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
                var isActive = false;
                if (isActive == true)
                {
                    return false;
                }
            }

            return true;
        }
        public Inv_ItemPriceDetails GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Inv_ItemPriceDetails> GetMany(Expression<Func<Inv_ItemPriceDetails, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
