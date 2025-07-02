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
    public interface IFamilyMemberSameHouseholdService : IServiceBase<FamilyMemberSameHousehold>
    {
        IEnumerable<FamilyMemberSameHousehold> GetActiveRecords();
    }
    public class FamilyMemberSameHouseholdService : IFamilyMemberSameHouseholdService
    {
        private readonly IFamilyMemberSameHouseholdRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public FamilyMemberSameHouseholdService(IFamilyMemberSameHouseholdRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<FamilyMemberSameHousehold> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.FamilyMemberId);
            return entities;
        }

        public FamilyMemberSameHousehold GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public FamilyMemberSameHousehold Create(FamilyMemberSameHousehold objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(FamilyMemberSameHousehold objectToUpdate)
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
        
        public FamilyMemberSameHousehold GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FamilyMemberSameHousehold> GetActiveRecords()
        {
            return repository.GetAll();
        }

        public IEnumerable<FamilyMemberSameHousehold> GetMany(Expression<Func<FamilyMemberSameHousehold, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
