using gBanker.Core.Common;
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
    public interface ILgVillageService : IServiceBase<LgVillage>
    {
        IEnumerable<LgVillage> GetDivisionByCountry(int country_id);
        IEnumerable<LgVillage> GetDistrictByDivision(string divCode);
        IEnumerable<LgVillage> GetUpozillaByDistrict(string distCode);
        IEnumerable<LgVillage> GetUnionByUpozilla(string UpoCode);
        IEnumerable<LgVillage> GetVillageByUnion(string UniCode);
    }
    public class LgVillageService : ILgVillageService
    {
        private readonly ILgVillageRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public LgVillageService(ILgVillageRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<LgVillage> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.LgVillageID);
            return entities;
        }

        public LgVillage GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }
        public IEnumerable<LgVillage> GetDivisionByCountry(int country_id)
        {
            var entities = repository.GetAll().Where(w => w.CountryID == country_id).OrderBy(o => o.DivisionName);
            return entities;
        }
        public IEnumerable<LgVillage> GetDistrictByDivision(string divCode)
        {
            var entities = repository.GetAll().Where(w => w.DivisionCode == divCode).OrderBy(o => o.DistrictName);
            return entities;
        }
        public IEnumerable<LgVillage> GetUpozillaByDistrict(string distCode)
        {
            var entities = repository.GetAll().Where(w => w.DistrictCode == distCode).OrderBy(o => o.UpozillaName);
            return entities;
        }
        public IEnumerable<LgVillage> GetUnionByUpozilla(string UpoCode)
        {
            var entities = repository.GetAll().Where(w => w.UpozillaCode == UpoCode).OrderBy(o => o.UnionName);
            return entities;
        }
        public IEnumerable<LgVillage> GetVillageByUnion(string UniCode)
        {
            var entities = repository.GetAll().Where(w => w.UnionCode == UniCode).OrderBy(o => o.VillageName);
            return entities;
        }

        public LgVillage Create(LgVillage objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(LgVillage objectToUpdate)
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

        public LgVillage GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LgVillage> GetMany(Expression<Func<LgVillage, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }
    }
}
