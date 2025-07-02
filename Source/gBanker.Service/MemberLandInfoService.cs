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
    public interface IMemberLandInfoService : IServiceBase<MemberLandInfo>
    {
        MemberLandInfo GetByLandInfoByMemberId(long MemberID);
        MemberLandInfo GetByLandInfoId(Int64 MemberLandID);
    }
    public class MemberLandInfoService : IMemberLandInfoService
    {
        private readonly IMemberLandInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public MemberLandInfoService(IMemberLandInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<MemberLandInfo> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.MemberID);
            return entities;
        }

        public MemberLandInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public MemberLandInfo GetByLandInfoByMemberId(long MemberID)
        {
            var entity = repository.Get(e => e.MemberID == MemberID && e.IsActive == true);
            return entity;
        }
        public MemberLandInfo GetByLandInfoId(Int64 MemberLandID)
        {
            var entity = repository.Get(e => e.MemberLandID == MemberLandID);
            return entity;
        }

        public MemberLandInfo Create(MemberLandInfo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(MemberLandInfo objectToUpdate)
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



        public MemberLandInfo GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<MemberLandInfo> GetMany(Expression<Func<MemberLandInfo, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
