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
    public interface IMemberOtherInfoService : IServiceBase<MemberOtherInfo>
    {
        MemberOtherInfo GetByOtherInfoID(Int64 memberOtherInfoID);
        IEnumerable<MemberOtherInfo> GetByOtherInfoByMemberId(long MemId);
    }
    public class MemberOtherInfoService : IMemberOtherInfoService
    {
        private readonly IMemberOtherInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public MemberOtherInfoService(IMemberOtherInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<MemberOtherInfo> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.MemberID);
            return entities;
        }

        public MemberOtherInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public MemberOtherInfo GetByOtherInfoID(Int64 memberOtherInfoID)
        {
            var entity = repository.Get(e => e.MemberOtherInfoID == memberOtherInfoID);
            return entity;
        }
        public IEnumerable<MemberOtherInfo> GetByOtherInfoByMemberId(long MemId)
        {
            var entity = repository.GetMany(e => e.MemberID == MemId && e.IsActive == true);
            return entity;
        }
        public MemberOtherInfo Create(MemberOtherInfo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(MemberOtherInfo objectToUpdate)
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



        public MemberOtherInfo GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<MemberOtherInfo> GetMany(Expression<Func<MemberOtherInfo, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}

