using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface IInvTrxMasterService : IServiceBase<Inv_TrxMaster>
    {
        IEnumerable<Proc_Get_AccountDetails_Result> Proc_Get_AccountDetails(Nullable<int> orgID, Nullable<int> officeID, DateTime? TrxDate);
        Inv_TrxMaster GetByTrxmasterId(Int64 masterId);
    }
    public class InvTrxMasterService : IInvTrxMasterService
    {
        private readonly IInvTrxMasterRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public InvTrxMasterService(IInvTrxMasterRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Inv_TrxMaster> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.VoucherNo);
            return entities;
        }

        public Inv_TrxMaster GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public Inv_TrxMaster GetByTrxmasterId(Int64 masterId)
        {
            var entity = repository.Get(e => e.TrxMasterID == masterId);
            return entity;
        }

        

        public Inv_TrxMaster Create(Inv_TrxMaster objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Inv_TrxMaster objectToUpdate)
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
            throw new NotImplementedException();
        }
        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }



        public Inv_TrxMaster GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }



        public IEnumerable<Proc_Get_AccountDetails_Result> Proc_Get_AccountDetails(int? orgID, int? officeID, DateTime? TrxDate)
        {
            return  repository.Proc_Get_AccountDetails(orgID,officeID,TrxDate);
        }

        public IEnumerable<Inv_TrxMaster> GetMany(Expression<Func<Inv_TrxMaster, bool>> where)
        {
           return repository.GetMany(where);
        }
    }
}
