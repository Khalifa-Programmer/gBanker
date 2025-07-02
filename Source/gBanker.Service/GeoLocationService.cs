using gBanker.Core.Common;
using gBanker.Data.CodeFirstMigration.Db;
//using gBanker.Data.CodeFirstMigration.Db;
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
    public interface IGeoLocationService : IServiceBase<GeoLocation>
    {
        bool IsValidGeoLocation(GeoLocation geoLocation, out string msg);
        IEnumerable<GeoLocation> SearchGeoLocation();
    }
    public class GeoLocationService : IGeoLocationService
    {
        private readonly IGeoLocationRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public GeoLocationService(IGeoLocationRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<GeoLocation> GetAll()
        {
            var entities = repository.GetMany(g => g.IsActive == true).OrderBy(c => c.LocationName);
            return entities;
        }

        public GeoLocation GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public GeoLocation Create(GeoLocation objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(GeoLocation objectToUpdate)
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
        public bool IsValidGeoLocation(GeoLocation geoLocation, out string msg)
        {
            var entity = repository.Get(p => p.LocationName == geoLocation.LocationName);
            msg = "test";
            return entity == null ? true : false; ;
        }


        public IEnumerable<GeoLocation> SearchGeoLocation()
        {
            return repository.GetMany(g => g.IsActive == true).OrderBy(g => g.LocationName);
        }


        public GeoLocation GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public IEnumerable<GeoLocation> GetMany(Expression<Func<GeoLocation, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
