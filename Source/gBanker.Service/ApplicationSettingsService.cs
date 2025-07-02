using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace gBanker.Service
{
    public interface IApplicationSettingsService : IServiceBase<ApplicationSetting>
    {
        IEnumerable<ValidationResult> IsValidSettings(ApplicationSetting applicationsetting);
        IEnumerable<ValidationResult> IsValidEdit(ApplicationSetting applicationsetting);
        IEnumerable<DBApplicationSettingsDetail> GetApplicationDetailDetail(int? OrgID, int? officeID);
    }
    public class ApplicationSettingsService: IApplicationSettingsService
    {
        private readonly IApplicationSettingRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public ApplicationSettingsService(IApplicationSettingRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
           
        }
      
        public IEnumerable<ApplicationSetting> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.OfficeID);
            return entities;
        }

        public ApplicationSetting GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public ApplicationSetting Create(ApplicationSetting objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ApplicationSetting objectToUpdate)
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
            throw new NotImplementedException();
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

        public IEnumerable<ValidationResult> IsValidSettings(ApplicationSetting applicationsetting)
        {
            var entity = repository.Get(a => a.OfficeID == applicationsetting.OfficeID);

            if (entity != null)
            {
                yield return new ValidationResult("OfficeID", "Duplicate Record");
            }
        }


        public IEnumerable<ValidationResult> IsValidEdit(ApplicationSetting applicationsetting)
        {
            var entity = repository.Get(a => a.OfficeID == applicationsetting.OfficeID);

            if (entity == null)
            {
                yield return new ValidationResult("OfficeID", "Duplicate Record");
            }
        }


        public IEnumerable<DBApplicationSettingsDetail> GetApplicationDetailDetail(int? OrgID, int? officeID)
        {
            return repository.GetApplicationDetailDetail(OrgID,officeID);
        }


        public ApplicationSetting GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<ApplicationSetting> GetMany(Expression<Func<ApplicationSetting, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
