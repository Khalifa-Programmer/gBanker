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
    public interface IAccTrxMasterService : IServiceBase<AccTrxMaster>
    {
        IEnumerable<Proc_Get_AccountDetails_Result> Proc_Get_AccountDetails(Nullable<int> orgID, Nullable<int> officeID, DateTime? TrxDate);
        IEnumerable<AccTrxMaster> GetByTrxDt_VType(string vType, DateTime trxDt, int offc_id);
        AccTrxMaster GetByTrxmasterId(Int64 masterId);
    }
    public class AccTrxMasterService : IAccTrxMasterService
    {
        private readonly IAccTrxMasterRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public AccTrxMasterService(IAccTrxMasterRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<AccTrxMaster> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.VoucherNo);
            return entities;
        }

        public AccTrxMaster GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public AccTrxMaster GetByTrxmasterId(Int64 masterId)
        {
            var entity = repository.Get(e => e.TrxMasterID == masterId);
            return entity;
        }

        public IEnumerable<AccTrxMaster> GetByTrxDt_VType(string vType, DateTime trxDt, int offc_id)
        {
            var entities = repository.GetAll().Where(m => m.OfficeID == offc_id && m.TrxDate == trxDt && m.VoucherType == vType && m.IsYearlyClosing == false && m.IsActive==true);
            return entities;
        }

        public AccTrxMaster Create(AccTrxMaster objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(AccTrxMaster objectToUpdate)
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



        public AccTrxMaster GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }



        public IEnumerable<Proc_Get_AccountDetails_Result> Proc_Get_AccountDetails(int? orgID, int? officeID, DateTime? TrxDate)
        {
            return  repository.Proc_Get_AccountDetails(orgID,officeID,TrxDate);
        }

        public IEnumerable<AccTrxMaster> GetMany(Expression<Func<AccTrxMaster, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
