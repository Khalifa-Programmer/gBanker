using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration;
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
    public interface IPNMOrderService : IServiceBase<PNMOrder>
    {
        IEnumerable<ValidationResult> IsValidPNMOrder(PNMOrder pnm);
        PNMOrder GetByLoanSummaryId(long LoanSummaryId);
        //string GetNoteDetail(int note_Id);
    }
    public class PNMOrderService : IPNMOrderService
    {
        private readonly IPNMOrderRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public PNMOrderService(IPNMOrderRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<PNMOrder> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.pnm_order_id);
            return entities;
        }

        public PNMOrder GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public PNMOrder GetByLoanSummaryId(long LoanSummaryId)
        {
            var entity = repository.Get(p => p.loan_disburse_id == LoanSummaryId);
            return entity;
        }

        //public string GetNoteDetail(int note_Id)
        //{
        //    var result = repository.GetById(note_Id);
        //    var note = "";
        //    if (result != null)
        //        note = result.NoteNo.ToString(); //+ " - " + result.NoteName;
        //    return note;

        //}

        public PNMOrder Create(PNMOrder objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(PNMOrder objectToUpdate)
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
        IEnumerable<ValidationResult> IPNMOrderService.IsValidPNMOrder(PNMOrder pnm)
        {
            var entity = repository.Get(p => p.pnm_order_identifier == pnm.pnm_order_identifier);
            if (entity != null)
            {
                yield return new ValidationResult("pnm_order_identifier", "Duplicate Order Identifier.");

            }
        }



        public PNMOrder GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PNMOrder> GetMany(Expression<Func<PNMOrder, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
