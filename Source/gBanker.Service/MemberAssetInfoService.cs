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
    public interface IMemberAssetInfoService : IServiceBase<MemberAssetInfo>
    {
        MemberAssetInfo GetByAssetInfoByMemberId(long MemberID);
        MemberAssetInfo GetByAssetInfoId(Int64 MemberAssetID);
    }
    public class MemberAssetInfoService : IMemberAssetInfoService
    {
        private readonly IMemberAssetInfoRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public MemberAssetInfoService(IMemberAssetInfoRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<MemberAssetInfo> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.MemberID);
            return entities;
        }

        public MemberAssetInfo GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public MemberAssetInfo GetByAssetInfoByMemberId(long MemberID)
        {
            var entity = repository.Get(e => e.MemberID == MemberID && e.IsActive == true);
            return entity;
        }
        public MemberAssetInfo GetByAssetInfoId(Int64 MemberAssetID)
        {
            var entity = repository.Get(e => e.MemberAssetID == MemberAssetID);
            return entity;
        }

        public MemberAssetInfo Create(MemberAssetInfo objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(MemberAssetInfo objectToUpdate)
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



        public MemberAssetInfo GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<MemberAssetInfo> GetMany(Expression<Func<MemberAssetInfo, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
