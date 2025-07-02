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
    public interface IMemberFamilyInfoService : IServiceBase<MemberFamilyInfo>
    {
        MemberFamilyInfo GetByFamilyInfoId(Int64 memberFamilyID);
        IEnumerable<MemberFamilyInfo> GetByFamilyInfoByMemberId(long MemberID);
    }
    public class MemberFamilyInfoService : IMemberFamilyInfoService
    {
        private readonly IMemberFamilyInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public MemberFamilyInfoService(IMemberFamilyInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<MemberFamilyInfo> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.MemberID);
            return entities;
        }

        public MemberFamilyInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public MemberFamilyInfo GetByFamilyInfoId(Int64 memberFamilyID)
        {
            var entity = repository.Get(e => e.MemberFamilyID == memberFamilyID);
            return entity;
        }
        public IEnumerable<MemberFamilyInfo> GetByFamilyInfoByMemberId(long MemberID)
        {
            var entity = repository.GetMany(e => e.MemberID == MemberID && e.IsActive == true);
            return entity;
        }
        public MemberFamilyInfo Create(MemberFamilyInfo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(MemberFamilyInfo objectToUpdate)
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



        public MemberFamilyInfo GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<MemberFamilyInfo> GetMany(Expression<Func<MemberFamilyInfo, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
