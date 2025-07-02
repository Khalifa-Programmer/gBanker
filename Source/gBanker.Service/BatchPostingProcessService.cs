using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Xml;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.Repository;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;

namespace gBanker.Service

{
    public interface IBatchPostingProcessService : IServiceBase<BatchPostingProcess>
    {
        IEnumerable<BatchPostingProcess> GetTransactionDateWise(DateTime TransactionDate);
        BatchPostingProcess getByBatchIdLong(long BatchId);
         int SaveListBatchData(List<BatchPostingProcess> BatchDataList);

    }
    public class BatchPostingProcessService : IBatchPostingProcessService
    {
        private readonly IBatchPostingProcessRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public BatchPostingProcessService(IBatchPostingProcessRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public BatchPostingProcess Create(BatchPostingProcess objectToCreate)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BatchPostingProcess> GetAll()
        {
            var entities = repository.GetMany(g => g.IsRemoved == false);
            return entities;
        }

        public BatchPostingProcess getByBatchIdLong(long BatchId)
        {
            var entity = repository.GetById(BatchId);
            return entity;
        }

        public BatchPostingProcess GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public BatchPostingProcess GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<BatchPostingProcess> GetMany(Expression<Func<BatchPostingProcess, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BatchPostingProcess> GetTransactionDateWise(DateTime TransactionDate)
        {
            var entities = repository.GetAll().Where(b => b.IsRemoved == false && b.TransactionDate == TransactionDate)
                .OrderBy(b => b.TransactionDate);
            return entities;
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

        public int SaveListBatchData(List<BatchPostingProcess> BatchDataList)
        {
            int totalRow = 0;
            try
            {
                foreach (var row in BatchDataList)
                {
                    repository.Add(row);
                    totalRow++;
                }
                Save();
            }
            catch (Exception ex)
            {
                totalRow = totalRow;
                //Log any exception here.
            }
            return totalRow;
        }

        public void Update(BatchPostingProcess objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }


    }
}
