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
    public interface IMiscellaneouService : IServiceBase<Miscellaneou>
    {
        IEnumerable<Proc_get_Miscellaneou_Result> GetMiscellaneou(int? officeID, DateTime? TrxDate);
    }
    public class MiscellaneouService: IMiscellaneouService
    {
        private readonly IMiscellaneouRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;


        public MiscellaneouService(IMiscellaneouRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
           
        }

        public IEnumerable<Miscellaneou> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.CenterID);
            return entities;
        }

        public Miscellaneou GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Miscellaneou Create(Miscellaneou objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Miscellaneou objectToUpdate)
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

        public void Save()
        {
            unitOfWork.Commit();
        }

        public Miscellaneou GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<Proc_get_Miscellaneou_Result> GetMiscellaneou(int? officeID, DateTime? TrxDate)
        {
            return repository.GetMiscellaneou(officeID, TrxDate);
        }

        public IEnumerable<Miscellaneou> GetMany(Expression<Func<Miscellaneou, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
