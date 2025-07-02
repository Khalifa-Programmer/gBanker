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
    public interface IMemberPassBookStockService : IServiceBase<MemberPassBookStock>
    {
        IEnumerable<getMemberPassBookStock_Result> getPassBookStock(int? officeID);
        IEnumerable<ValidationResult> IsValidPassBookStock(MemberPassBookStock MemberPassBookStock);
    }
    public class MemberPassBookStockService : IMemberPassBookStockService
    {
        private readonly IMemberPassBookStockRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        private readonly IMemberRepository memberRepository;

        public MemberPassBookStockService(IMemberPassBookStockRepository repository, IMemberRepository memberRepository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.memberRepository = memberRepository;
        }



        public IEnumerable<MemberPassBookStock> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.LotNo);
            return entities;
        }

        public MemberPassBookStock GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public MemberPassBookStock Create(MemberPassBookStock objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(MemberPassBookStock objectToUpdate)
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
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.InActiveDate = inactiveDate.HasValue ? inactiveDate : DateTime.Now;
                obj.IsActive = false;
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
                var isActive = obj.IsActive;
                if (isActive == true)
                {
                    return false;
                }
            }

            return true;
        }

        public void Save()
        {
            unitOfWork.Commit();
        }

        public MemberPassBookStock GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<getMemberPassBookStock_Result> getPassBookStock(int? officeID)
        {
            return repository.getPassBookStock(officeID);
        }


        public IEnumerable<ValidationResult> IsValidPassBookStock(MemberPassBookStock MemberPassBookStock)
        {
            if (MemberPassBookStock.LotNo == null)
            {
                yield return new ValidationResult("LotNo", "LotNo Cannt be empty");
            }
            if (MemberPassBookStock.Qty == null)
            {
                yield return new ValidationResult("Qty", "Qty Cannt be empty");
            }
            if (MemberPassBookStock.StartingNo == null)
            {
                yield return new ValidationResult("StartingNo", "StartingNo Cannt be empty");
            }
            var memCheck = repository.Get(p => p.OrgID == MemberPassBookStock.OrgID && p.LotNo == MemberPassBookStock.LotNo && p.OfficeID == MemberPassBookStock.OfficeID && p.IsActive == true);
            if (memCheck != null)
            {
                yield return new ValidationResult("LotNo", "Duplicate LotNo");
            }
        }

        public IEnumerable<MemberPassBookStock> GetMany(Expression<Func<MemberPassBookStock, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
