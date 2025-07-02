using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface IInv_RequisitionConsulateDetailsService : IServiceBase<Inv_RequisitionConsulateDetails>
    {
    }
    public class Inv_RequisitionConsulateDetailsService : IInv_RequisitionConsulateDetailsService
    {
        private readonly IInv_RequisitionConsulateDetailsRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public Inv_RequisitionConsulateDetailsService(IInv_RequisitionConsulateDetailsRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Inv_RequisitionConsulateDetails> GetAll()
        {
            var entities = repository.GetMany(g => g.AprovedStatus == g.AprovedStatus).OrderBy(c => c.ConsulateDetailID);
            return entities;
        }

        public Inv_RequisitionConsulateDetails GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Inv_RequisitionConsulateDetails Create(Inv_RequisitionConsulateDetails objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Inv_RequisitionConsulateDetails objectToUpdate)
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
        public bool IsValidInv_Items(Inv_RequisitionConsulateDetails Inv_Items)
        {
            var entity = repository.Get(p => p.ConsulateDetailID == Inv_Items.ConsulateDetailID);
            return entity == null ? true : false; ;
        }


        public IEnumerable<Inv_RequisitionConsulateDetails> SearchInv_Items()
        {
            return repository.GetMany(g => g.AprovedStatus == g.AprovedStatus).OrderBy(g => g.ConsulateDetailID);
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
        public Inv_RequisitionConsulateDetails GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Inv_RequisitionConsulateDetails> GetMany(Expression<Func<Inv_RequisitionConsulateDetails, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.AprovedStatus == b.AprovedStatus);
            return entities;
        }
    }
}
