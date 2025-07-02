using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface IGroupService : IServiceBase<Group>
    {
        bool IsValidGroup(Group group, out string msg);
        IEnumerable<Group> SearchGroup();
        IEnumerable<Group> GetByOfficeId(int OfficeId);
        IEnumerable<DBGroupDeatil> GetGroupDetail(int OrgID, int? officeID);
        IEnumerable<ValidationResult> IsValidGroup(Group group);
    }
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public GroupService(IGroupRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Group> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.GroupCode);
            return entities;
        }

        public Group GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public Group GetByGroupCode(string GroupCode)
        {
            var entity = repository.Get(p => p.GroupCode == GroupCode);
            return entity;
        }

        public IEnumerable<Group> GetByOfficeId(int OfficeId)
        {
            return repository.GetMany(s => s.OfficeID == OfficeId);
        }

        public Group Create(Group objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Group objectToUpdate)
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
        public bool IsValidOffice(Group group, out string msg)
        {
            var entity = repository.Get(p => p.GroupCode == group.GroupCode);
            msg = "test";
            return entity == null ? true : false; ;
        }


        public IEnumerable<Group> SearchGroup()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.GroupCode);
        }

        //public IEnumerable<DBGroupDetailsModel> GetGroupDetail()
        //{
        //    return repository.get();
        //}

        public bool IsValidGroup(Group group, out string msg)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<DBGroupDeatil> GetGroupDetail(int OrgID, int? officeID)
        {
            return repository.GetGroupDetail(OrgID,officeID);
        }


        public IEnumerable<ValidationResult> IsValidGroup(Group group)
        {
            var entity = repository.Get(p => p.GroupCode == group.GroupCode && p.OfficeID == group.OfficeID && p.OrgID==group.OrgID);

            if (entity != null)


                yield return new ValidationResult("GroupCode", "Duplicate Group.");

           
        }


        public Group GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<Group> GetMany(Expression<Func<Group, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
