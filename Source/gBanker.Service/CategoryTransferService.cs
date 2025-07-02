using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface ICategoryTransferService : IServiceBase<TransferHistory>
    {
        IEnumerable<DBCategoryTransferDetails> GetCategoryTransferDetail(int? officeID);
        IEnumerable<getTransferHistory_Result> GetTransferDetail(int OrgID, int? officeID);
        int CateGoryTransfer(Nullable<int> orgID, Nullable<int> officeID, Nullable<int> centerID, Nullable<int> memberID, Nullable<int> memberCategoryID, Nullable<int> productID, Nullable<decimal> savBalance, Nullable<int> newMemberCategoryID, Nullable<System.DateTime> transdate, Nullable<int> newProductID);

    }
    public class CategoryTransferService: ICategoryTransferService
    {
        private readonly ICategoryTransferRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public CategoryTransferService(ICategoryTransferRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;

        }


        public IEnumerable<TransferHistory> GetAll()
        {
            var entities = repository.GetAll();
            return entities;
        }

        public TransferHistory GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public TransferHistory Create(TransferHistory objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(TransferHistory objectToUpdate)
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

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public IEnumerable<getTransferHistory_Result> GetTransferDetail(int OrgID, int? officeID)
        {
            return repository.GetTransferDetail(OrgID,officeID);
        }

        public int CateGoryTransfer(int? orgID, int? officeID, int? centerID, int? memberID, int? memberCategoryID, int? productID, decimal? savBalance, int? newMemberCategoryID, DateTime? transdate, int? newProductID)
        {
            return repository.CateGoryTransfer(orgID,officeID, centerID, memberID, memberCategoryID, productID, savBalance, newMemberCategoryID, transdate, newProductID);
        }

        public IEnumerable<DBCategoryTransferDetails> GetCategoryTransferDetail(int? officeID)
        {
            return repository.GetCategoryTransferDetail(officeID);
        }


        public TransferHistory GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<TransferHistory> GetMany(Expression<Func<TransferHistory, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
