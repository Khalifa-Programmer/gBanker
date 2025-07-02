using gBanker.Core.Common;
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
    public interface IPurposeService : IServiceBase<Purpose>
    {
        IEnumerable<Purpose> SearchPurpose(int OrgID);
        void conditionalUpdate(Purpose purpose);
        bool IsContinued(int id);
        IEnumerable<ValidationResult> IsValidPurpose(Purpose purpose);
        IEnumerable<Purpose> GetPurposeDetailPaged(string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount, int? OrgID);
    }
    public class PurposeService : IPurposeService
    {
        private readonly IPurposeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public PurposeService(IPurposeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Purpose> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.PurposeName);
            return entities;
        }

        public Purpose GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Purpose Create(Purpose objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Purpose objectToUpdate)
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




        public IEnumerable<Purpose> SearchPurpose(int OrgID)
        {
            return repository.GetMany(g => g.IsActive == true && g.OrgID == OrgID).OrderBy(g => g.PurposeID);
        }





        public void conditionalUpdate(Purpose purpose)
        {
            repository.Update(purpose);
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


        public bool IsContinued(int id)
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


        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<ValidationResult> IsValidPurpose(Purpose purpose)
        {

            if (purpose.PurposeCode == ""|| purpose.PurposeName=="")
            {
                yield return new ValidationResult("PurposeCode", "Purpose can not be empty");
            }
            var entity = repository.Get(p => p.PurposeCode == purpose.PurposeCode && p.IsActive==true);

            if (entity != null)
            {

                yield return new ValidationResult("PurposeCode", "Duplicate Purpose.");

            }
        }


        public IEnumerable<Purpose> GetPurposeDetailPaged(string filterColumnName, string filterValue, int startRowIndex, int pageSize, out long totalCount, int? OrgID)
        {
            return repository.GetPurposeDetailPaged(filterColumnName, filterValue, startRowIndex, pageSize, out totalCount,OrgID);
        }


        public Purpose GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<Purpose> GetMany(Expression<Func<Purpose, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}