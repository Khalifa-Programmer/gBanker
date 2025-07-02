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
    public interface IPNMConfirmService : IServiceBase<PNMConfirm>
    {
        IEnumerable<ValidationResult> IsValidPNMConfirm(PNMConfirm pnm);
        PNMConfirm GetByLoaneeNo(string pnm_order_identifier);
        PNMConfirm GetByPaymentIdentifier(PNMConfirm pnm);
    }
    public class PNMConfirmService : IPNMConfirmService
    {
        private readonly IPNMConfirmRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public PNMConfirmService(IPNMConfirmRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<PNMConfirm> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.pnm_confirm_id);
            return entities;
        }

        public PNMConfirm GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public PNMConfirm GetByLoaneeNo(string pnm_order_identifier)
        {
            var entity = repository.Get(p => p.pnm_order_identifier == pnm_order_identifier);
            return entity;
        }
        public PNMConfirm GetByPaymentIdentifier(PNMConfirm pnm)
        {
            var entity = repository.Get(p => p.pnm_payment_identifier == pnm.pnm_payment_identifier);
            return entity;
        }

        public PNMConfirm Create(PNMConfirm objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(PNMConfirm objectToUpdate)
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
        IEnumerable<ValidationResult> IPNMConfirmService.IsValidPNMConfirm(PNMConfirm pnm)
        {
            var entity = repository.Get(p => p.pnm_payment_identifier == pnm.pnm_payment_identifier);
            if (entity != null)
            {
                yield return new ValidationResult("pnm_order_identifier", "Duplicate Order Identifier.");

            }
        }



        public PNMConfirm GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PNMConfirm> GetMany(Expression<Func<PNMConfirm, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
