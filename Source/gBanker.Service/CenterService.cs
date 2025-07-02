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
    public interface ICenterService : IServiceBase<Center>
    {
        IEnumerable<Center> SearchCenter();
        IEnumerable<Center> SearchOffCenter(string colday, int offcId,int? OrgID);
        IEnumerable<Center> SearchOffCenter(string colday, int offcId, int? OrgID, short? EmpID,string EmpType);
        IEnumerable<Center> SearchOffCenterBuro(string colday, int offcId, int? OrgID, short? EmpID, string EmpType);
        IEnumerable<Center> GetByOfficeId(int offcId,int OrgID);
        IEnumerable<DBCenterDetailModel> GetCenterDetail(int? OrgID);
        IEnumerable<DBCenterDetailModel> GetNonCenterMeetingCenterDetail(string colday, int OfficeId);
        IEnumerable<Center> SearchSpecialCenter(string colday, int offcId);
        IEnumerable<ValidationResult> IsValidCenter(Center center);
        IEnumerable<Center> GetByOfficeId(int offcId, int OrgID,short? empID);
    }
    public class CenterService : ICenterService
    {
        private readonly ICenterRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public CenterService(ICenterRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Center> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.CenterName);
            return entities;
        }

        public Center GetById(int id)
        {
            //throw new NotImplementedException();
            var entity = repository.GetById(id);
            return entity;
        }
        public IEnumerable<Center> GetByOfficeId(int offcId, int OrgID)
        {
            return repository.GetMany(s => s.OfficeID == offcId && s.IsActive==true && s.OrgID==OrgID && s.CenterStatus!=0);
        }

        public Center Create(Center objectToCreate)
        {
            //throw new NotImplementedException();
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Center objectToUpdate)
        {
            //throw new NotImplementedException();
            repository.Update(objectToUpdate);
            Save();
        }

        public void Delete(int id)
        {
            //throw new NotImplementedException();
            var entity = repository.GetById(id);
            repository.Delete(entity);
            Save();
        }

        public void Save()
        {
            //throw new NotImplementedException();
            unitOfWork.Commit();
        }
        //public bool IsValidCenter(Center area)
        //{
        //    var entity = repository.Get(p => p.CenterName == area.CenterName);
        //    return entity == null ? true : false; ;
        //}

        IEnumerable<ValidationResult> ICenterService.IsValidCenter(Center center)
        {
            var entity = repository.Get(p => p.CenterCode == center.CenterCode && p.OfficeID == center.OfficeID && p.CenterTypeID==center.CenterTypeID);
            if (entity != null)
            {

                yield return new ValidationResult("CenterCode", "Duplicate Center.");

            }
        }
        public IEnumerable<Center> SearchCenter()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.CenterName);
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
                if (isActive == true)
                {
                    return false;
                }
            }

            return true;
        }
        public IEnumerable<DBCenterDetailModel> GetCenterDetail(int? OrgID)
        {
            return repository.GetCenterDetail(OrgID);
        }





        public IEnumerable<DBCenterDetailModel> GetNonCenterMeetingCenterDetail(string colday, int OfficeId)
        {
            return repository.GetNonCenterMeetingCenterDetail(colday, OfficeId);
        }


        public IEnumerable<Center> SearchOffCenter(string colday, int offcId, int? OrgID)
        {
            return repository.GetMany(g => g.IsActive == true && g.CollectionDay == colday && g.OfficeID == offcId && g.OrgID == OrgID && g.CenterStatus != 0).OrderBy(g => g.CenterCode);
        }



        public IEnumerable<Center> SearchSpecialCenter(string colday, int offcId)
        {
            return repository.GetMany(g => g.IsActive == true && g.CollectionDay != colday && g.OfficeID == offcId).OrderBy(g => g.CenterName);
        }


        public Center GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }


        public IEnumerable<Center> SearchOffCenter(string colday, int offcId, int? OrgID, short? EmpID, string EmpType)
        {
            if (EmpType == "FO")
            {
                return repository.GetMany(g => g.IsActive == true && g.CollectionDay == colday && g.OfficeID == offcId && g.OrgID == OrgID && g.CenterStatus != 0 && g.EmployeeId==EmpID).OrderBy(g => g.CenterCode);
            }
            else
                return repository.GetMany(g => g.IsActive == true && g.CollectionDay == colday && g.OfficeID == offcId && g.OrgID == OrgID && g.CenterStatus != 0).OrderBy(g => g.CenterCode);
        }


        public IEnumerable<Center> GetByOfficeId(int offcId, int OrgID, short? EmpID)
        {
            
                return repository.GetMany(s => s.OfficeID == offcId && s.IsActive == true && s.OrgID == OrgID && s.CenterStatus != 0 && s.EmployeeId == EmpID);
        }

        public IEnumerable<Center> GetMany(Expression<Func<Center, bool>> where)
        {
            return repository.GetMany(where);
        }

        public IEnumerable<Center> SearchOffCenterBuro(string colday, int offcId, int? OrgID, short? EmpID, string EmpType)
        {
            if (EmpType == "FO")
            {
                return repository.GetMany(g => g.IsActive == true  && g.OfficeID == offcId && g.OrgID == OrgID && g.CenterStatus != 0 && g.EmployeeId == EmpID ).OrderBy(g => g.CenterCode);
            }
            else
                return repository.GetMany(g => g.IsActive == true  && g.OfficeID == offcId && g.OrgID == OrgID && g.CenterStatus != 0).OrderBy(g => g.CenterCode);

        }
    }
}
