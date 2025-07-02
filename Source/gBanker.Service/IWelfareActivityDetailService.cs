using gBanker.Data.CodeFirstMigration;
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
    public interface IWelfareActivityDetailService : IServiceBase<WelfareActivityDetail>
    { }
    public class WelfareActivityDetailService : IWelfareActivityDetailService
    {
        private readonly IWelfareActivityDetailRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public WelfareActivityDetailService(IWelfareActivityDetailRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<WelfareActivityDetail> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.WelfareActivityId);
            return entities;
        }

        public WelfareActivityDetail GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public WelfareActivityDetail Create(WelfareActivityDetail objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(WelfareActivityDetail objectToUpdate)
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
        public bool IsValidWelfareActivityDetail(WelfareActivityDetail WelfareActivityDetail)
        {
            var entity = repository.Get(p => p.WelfareActivityId == WelfareActivityDetail.WelfareActivityId);
            return entity == null ? true : false; ;
        }


        public IEnumerable<WelfareActivityDetail> SearchWelfareActivityDetail()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.WelfareActivityId);
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


        public WelfareActivityDetail GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WelfareActivityDetail> GetMany(Expression<Func<WelfareActivityDetail, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
