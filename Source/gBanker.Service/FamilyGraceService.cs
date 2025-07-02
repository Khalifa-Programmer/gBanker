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
    public interface IFamilyGraceService : IServiceBase<FamilyGrace>
    {
        IEnumerable<getFamilyGrace_Result> GetFamilyGraceDetail(int? orgID, int? officeID);
        IEnumerable<ValidationResult> IsValidRecord(FamilyGrace familygrace);
        
    }
    public class FamilyGraceService : IFamilyGraceService
    {
        private readonly IFamilyGraceRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        private readonly IMemberRepository memberrepository;
        public FamilyGraceService(IFamilyGraceRepository repository, IUnitOfWorkCodeFirst unitOfWork, IMemberRepository memberrepository)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.memberrepository = memberrepository;
        }
        public IEnumerable<FamilyGrace> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.MemberID);
            return entities;
        }

        public FamilyGrace GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public FamilyGrace Create(FamilyGrace objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(FamilyGrace objectToUpdate)
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
                if (isActive == false)
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

        public IEnumerable<getFamilyGrace_Result> GetFamilyGraceDetail(int? orgID, int? officeID)
        {
            return repository.GetFamilyGraceDetail(orgID,officeID);
        }




        public IEnumerable<ValidationResult> IsValidRecord(FamilyGrace familygrace)
        {
            var entity = memberrepository.Get(p => p.OrgID==familygrace.OrgID && p.MemberID == familygrace.MemberID && p.IsActive == true && p.MemberStatus == "1" && p.OfficeID == familygrace.OfficeID && p.MemberStatus != "4");

            if (entity == null)
            {

                yield return new ValidationResult("Description", "Invalid Member");

            }
            var Memberentity = repository.Get(p => p.OrgID == familygrace.OrgID && p.MemberID == familygrace.MemberID && p.IsActive == true && p.OfficeID == familygrace.OfficeID && p.CenterID == familygrace.CenterID && p.GraceStartDate == familygrace.GraceStartDate && p.GraceEndDate == familygrace.GraceEndDate);

            if (Memberentity != null)
            {

                yield return new ValidationResult("Description", "Duplicate Record");

            }
        }


        public FamilyGrace GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<FamilyGrace> GetMany(Expression<Func<FamilyGrace, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
