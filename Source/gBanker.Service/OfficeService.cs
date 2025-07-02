using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Service
{
    public interface IOfficeService : IServiceBase<Office>
    {
        IEnumerable<ValidationResult> IsValidOffice(Office office);        
        IEnumerable<Office> SearchOffice();
        
        Office GetByOfficeCode(string OfficeCode);
        Office GetByOfficeCode(string OfficeCode,int OfficeLevel);
        IEnumerable<DBOfficeDetailModel> GetOfficeDetail();
        IEnumerable<DBOfficeDetailModel> GetHeadOffice();
        IEnumerable<DBOfficeDetailModel> GetAllZoneOffice(string headofficeCode, int? orgiD);
        IEnumerable<DBOfficeDetailModel> GetAllAreaOfficeForZone(string headofficeCode, string zoneCode, int? orgiD);
        IEnumerable<DBOfficeDetailModel> GetAllBranchesForArea(string headofficeCode, string zoneCode, string areaCode, int? orgiD);
        int GetAllOfficeCount();
        Office GetByOfficeOrgID(int Office_Id, int Org_Id);
        IEnumerable<DBOfficeDetailModel> GetAllZoneOffice1(string headofficeCode, int? orgiD);
    }
    public class OfficeService : IOfficeService
    {
        private readonly IOfficeRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public OfficeService(IOfficeRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Office> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.OfficeCode);
            return entities;
        }

        public Office GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public Office GetByOfficeCode(string OfficeCode)
        {
            var entity = repository.Get(p => p.OfficeCode == OfficeCode);
            return entity;
        }

        public Office Create(Office objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Office objectToUpdate)
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
                if (isActive == true)
                {
                    return false;
                }
            }

            return true;
        }
        public Office GetByOfficeOrgID(int Office_Id, int Org_Id)
        {
            var entity = repository.Get(p => p.OrgID == Org_Id && p.OfficeID == Office_Id);
            return entity;
        }
        //public bool IsValidOffice(Office office, out string msg)
        //{
        //    var entity = repository.Get(p => p.OfficeCode == office.OfficeCode);
        //    msg = "test";
        //    return entity == null ? true : false;
        //}

        IEnumerable<ValidationResult> IOfficeService.IsValidOffice(Office office)
        {
            var entity = repository.Get(p => p.OfficeCode == office.OfficeCode);
            if (entity != null)
            {

                yield return new ValidationResult("OfficeCode", "Duplicate Office.");

            }
        }
        public IEnumerable<Office> SearchOffice()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.OfficeCode);
        }

        public IEnumerable<DBOfficeDetailModel> GetOfficeDetail()
        {
            return repository.GetOfficeDetail();
        }


        public IEnumerable<DBOfficeDetailModel> GetAllZoneOffice(string headofficeCode, int? orgiD)
        {
            return repository.GetAllZoneOffice(headofficeCode,orgiD);
        }

        public IEnumerable<DBOfficeDetailModel> GetAllAreaOfficeForZone(string headofficeCode, string zoneCode, int? orgiD)
        {
           return repository.GetAllAreaOfficeForZone(headofficeCode, zoneCode,orgiD);
        }

        public IEnumerable<DBOfficeDetailModel> GetAllBranchesForArea(string headofficeCode, string zoneCode, string areaCode, int? orgiD)
        {
            return repository.GetAllBranchesForArea(headofficeCode, zoneCode, areaCode,orgiD);
        }


        public int GetAllOfficeCount()
        {
            return repository.GetAllOfficeCount();
        }


        public Office GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }


        public IEnumerable<DBOfficeDetailModel> GetHeadOffice()
        {
            return GetHeadOffice();
        }
        public IEnumerable<DBOfficeDetailModel> GetAllZoneOffice1(string headofficeCode, int? orgiD)
        {
            return repository.GetAllZoneOffice1(headofficeCode, orgiD);
        }

        public IEnumerable<Office> GetMany(Expression<Func<Office, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }

        public Office GetByOfficeCode(string OfficeCode, int OfficeLevel)
        {
            var entity = repository.Get(p => p.OfficeCode == OfficeCode || p.OfficeLevel== OfficeLevel);
            return entity;
        }
    }
}
