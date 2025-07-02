using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface IInv_RequisitionConsulateMasterService : IServiceBase<Inv_RequisitionConsulateMaster>
    {
    }
    public class Inv_RequisitionConsulateMasterService : IInv_RequisitionConsulateMasterService
    {
        private readonly IInv_RequisitionConsulateMasterRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public Inv_RequisitionConsulateMasterService(IInv_RequisitionConsulateMasterRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Inv_RequisitionConsulateMaster> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.SenderDate);
            return entities;
        }

        public Inv_RequisitionConsulateMaster GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Inv_RequisitionConsulateMaster Create(Inv_RequisitionConsulateMaster objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Inv_RequisitionConsulateMaster objectToUpdate)
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
                var isActive = true;
                if (isActive == true)
                {
                    return false;
                }
            }

            return true;
        }
        public Inv_RequisitionConsulateMaster GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Inv_RequisitionConsulateMaster> GetMany(Expression<Func<Inv_RequisitionConsulateMaster, bool>> where)
        {
            var entities = repository.GetMany(where);
            return entities;
        }
    }
}
