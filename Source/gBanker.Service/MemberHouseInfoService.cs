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
    public interface IMemberHouseInfoService : IServiceBase<MemberHouseInfo>
    {
        MemberHouseInfo GetByHouseInfoByMemberId(long MemberID);
        MemberHouseInfo GetByHouseInfoId(Int64 MemberHouseID);
    }
    public class MemberHouseInfoService : IMemberHouseInfoService
    {
        private readonly IMemberHouseInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public MemberHouseInfoService(IMemberHouseInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<MemberHouseInfo> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.MemberID);
            return entities;
        }

        public MemberHouseInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public MemberHouseInfo GetByHouseInfoByMemberId(long MemberID)
        {
            var entity = repository.Get(e => e.MemberID == MemberID && e.IsActive == true);
            return entity;
        }
        public MemberHouseInfo GetByHouseInfoId(Int64 MemberHouseID)
        {
            var entity = repository.Get(e => e.MemberHouseID == MemberHouseID);
            return entity;
        }
        public MemberHouseInfo Create(MemberHouseInfo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(MemberHouseInfo objectToUpdate)
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



        public MemberHouseInfo GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<MemberHouseInfo> GetMany(Expression<Func<MemberHouseInfo, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
