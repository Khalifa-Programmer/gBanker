using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration;
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
    public interface IMemberNomineeService : IServiceBase<MemberNominee>
    {
        //int SetOpeningSavingEntry(int? orgID, int? officeID);
        MemberNominee GetByGurId(int MemberNomineeId);
    }

    public class MemberNomineeService : IMemberNomineeService
    {
        private readonly IMemberNomineeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        public MemberNomineeService(IMemberNomineeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;

        }

        public MemberNominee GetByGurId(int MemberNomineeId)
        {
            var entity = repository.Get(e => e.MemberNomineeId == MemberNomineeId && e.IsActive == true);
            return entity;
        }

        public MemberNominee Create(MemberNominee objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Delete(int id)
        {
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }

        public IEnumerable<MemberNominee> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.MemberNomineeId);
            return entities;
        }

        public MemberNominee GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public MemberNominee GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public bool IsValidTargetAchievement(MemberNominee TargetAchievement)
        {
            var entity = repository.Get(p => p.MemberNomineeId == TargetAchievement.MemberNomineeId);
            return entity == null ? true : false; ;
        }


        public IEnumerable<MemberNominee> SearchTargetAchievement()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.MemberNomineeId);
        }

        public IEnumerable<MemberNominee> GetMany(Expression<Func<MemberNominee, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
            //throw new NotImplementedException();
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                //obj.InActiveDate = inactiveDate.HasValue ? inactiveDate : DateTime.Now;
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

        public void Update(MemberNominee objectToUpdate)
        {
            repository.Update(objectToUpdate);
            Save();
        }

        //public int SetOpeningSavingEntry(int? orgID, int? officeID)
        //{
        //    return repository.SetOpeningSavingEntry(orgID, officeID);
        //}
    }
}
