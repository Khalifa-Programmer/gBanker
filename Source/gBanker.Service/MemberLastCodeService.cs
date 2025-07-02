using gBanker.Data.CodeFirstMigration.Db;
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
    public interface IMemberLastCodeService : IServiceBase<MemberLastCode>
    {
        MemberLastCode GetByOffcGroupId(int OrgID, int Office_Id, int center_id);
        MemberLastCode GetByLastCodeId(int last_code_id);
        //MemberCategory Create(MemberCategory objectToCreate);
        //void Update(MemberCategory objectToUpdate);
        //void Delete(int id);
        //void Save();
    }
    public class MemberLastCodeService : IMemberLastCodeService
    {
        private readonly IMemberLastCodeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public MemberLastCodeService(IMemberLastCodeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<MemberLastCode> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.LastCode);
            return entities;
        }

        public MemberLastCode GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public MemberLastCode Create(MemberLastCode objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(MemberLastCode objectToUpdate)
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
        public MemberLastCode GetByOffcGroupId(int OrgID, int Office_Id, int group_id)
        {
            var entity = repository.Get(m => m.OfficeID == Office_Id && m.GroupID == group_id && m.OrgID==OrgID);
            return entity;
        }
        public MemberLastCode GetByLastCodeId(int last_code_id)
        {
            var entity = repository.Get(m => m.LastCodeID == last_code_id);
            return entity;
        }
        //public bool Inactivate(long id, DateTime? inactiveDate)
        //{
        //    var obj = repository.GetById(id);
        //    if (obj != null)
        //    {
        //        obj.InActiveDate = inactiveDate.HasValue ? inactiveDate : DateTime.Now;
        //        obj.IsActive = false;
        //        repository.Update(obj);
        //        Save();
        //        return true;
        //    }
        //    return false;
        //}


        //public bool IsContinued(long id)
        //{
        //    var obj = repository.GetById(id);
        //    if (obj != null)
        //    {
        //        var isActive = obj.IsActive;
        //        if (isActive == true)
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}


        public MemberLastCode GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<MemberLastCode> GetMany(Expression<Func<MemberLastCode, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
