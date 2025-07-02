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
    public interface IMemberPassBookRegisterService : IServiceBase<MemberPassBookRegister>
    {
        IEnumerable<ValidationResult> IsValidPassBookCreate(MemberPassBookRegister MemberPassBookRegister);
        IEnumerable<getPassBookRegister_Result> getPassBookRegister(int? officeID, int? orgID);
        IEnumerable<ValidationResult> IsValidPassBook(MemberPassBookRegister MemberPassBookRegister);
    }
    public class MemberPassBookRegisterService : IMemberPassBookRegisterService
    {
        private readonly IMemberPassBookRegisterRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        private readonly IMemberRepository memberRepository;
        private readonly IMemberPassBookStockRepository MemberPassBookStockrepository;

        public MemberPassBookRegisterService(IMemberPassBookRegisterRepository repository, IMemberRepository memberRepository, IUnitOfWorkCodeFirst unitOfWork, IMemberPassBookStockRepository MemberPassBookStockrepository)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.memberRepository = memberRepository;
            this.MemberPassBookStockrepository = MemberPassBookStockrepository;
        }

        public IEnumerable<MemberPassBookRegister> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.MemberPassBookNO);
            return entities;
        }

        public MemberPassBookRegister GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public MemberPassBookRegister Create(MemberPassBookRegister objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(MemberPassBookRegister objectToUpdate)
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

        public MemberPassBookRegister GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<getPassBookRegister_Result> getPassBookRegister(int? officeID, int? orgID)
        {
            return repository.getPassBookRegister(officeID, orgID);
        }


        public IEnumerable<ValidationResult> IsValidPassBook(MemberPassBookRegister MemberPassBookRegister)
        {

            if (MemberPassBookRegister.MemberPassBookNO == null )
            {
                yield return new ValidationResult("MemberID", "PassBook No Cann't be empty");
            }


            var memStock = MemberPassBookStockrepository.Get(p => p.OrgID == MemberPassBookRegister.OrgID && p.LotNo == MemberPassBookRegister.LotNo && p.OfficeID == MemberPassBookRegister.OfficeID && p.IsActive == true);
            if (memStock != null)
            {
                if(MemberPassBookRegister.MemberPassBookNO<memStock.StartingNo || MemberPassBookRegister.MemberPassBookNO>memStock.LastIssue)
                {
                    yield return new ValidationResult("MemberID", "Duplicate Passbook");
                }
            }
            else
                yield return new ValidationResult("LotNo", "Invalid Lot");


            var memCheckDup = repository.Get(p => p.OrgID == MemberPassBookRegister.OrgID && p.OfficeID == MemberPassBookRegister.OfficeID && p.MemberPassBookNO == MemberPassBookRegister.MemberPassBookNO && p.MemberID!= MemberPassBookRegister.MemberID && p.IsActive == true);
            if (memCheckDup != null)
            {
                yield return new ValidationResult("MemberID", "Duplicate Passbook");
            }

            var memCheck = repository.Get(p => p.OrgID == MemberPassBookRegister.OrgID && p.MemberID != MemberPassBookRegister.MemberID && p.OfficeID == MemberPassBookRegister.OfficeID && p.MemberPassBookNO == MemberPassBookRegister.MemberPassBookNO && p.IsActive == true);
            if (memCheck != null)
            {
                yield return new ValidationResult("MemberID", "Duplicate Passbook");
            }

        }
        public IEnumerable<ValidationResult> IsValidPassBookCreate(MemberPassBookRegister MemberPassBookRegister)
        {

            if (Convert.ToInt64(MemberPassBookRegister.MemberPassBookNO) == null)
            {
                yield return new ValidationResult("MemberID", "PassBook No Cann't be empty");
            }


            var memStock = MemberPassBookStockrepository.Get(p => p.OrgID == MemberPassBookRegister.OrgID && p.LotNo == MemberPassBookRegister.LotNo && p.OfficeID == MemberPassBookRegister.OfficeID && p.IsActive == true);
            if (memStock != null)
            {
                if (MemberPassBookRegister.MemberPassBookNO < memStock.StartingNo || MemberPassBookRegister.MemberPassBookNO > memStock.LastIssue)
                {
                    yield return new ValidationResult("MemberID", "Duplicate Passbook");
                }
            }
            else
                yield return new ValidationResult("LotNo", "Invalid Lot");


            var memCheckDup = repository.Get(p => p.OrgID == MemberPassBookRegister.OrgID && p.OfficeID == MemberPassBookRegister.OfficeID && p.MemberPassBookNO == MemberPassBookRegister.MemberPassBookNO && p.MemberID == MemberPassBookRegister.MemberID && p.IsActive == true);
            if (memCheckDup != null)
            {
                yield return new ValidationResult("MemberID", "Duplicate Passbook");
            }

            var memCheck = repository.Get(p => p.OrgID == MemberPassBookRegister.OrgID && p.MemberID != MemberPassBookRegister.MemberID && p.OfficeID == MemberPassBookRegister.OfficeID && p.MemberPassBookNO == MemberPassBookRegister.MemberPassBookNO && p.IsActive == true);
            if (memCheck != null)
            {
                yield return new ValidationResult("MemberID", "Duplicate Passbook");
            }

        }

        public IEnumerable<MemberPassBookRegister> GetMany(Expression<Func<MemberPassBookRegister, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
