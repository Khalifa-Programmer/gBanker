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
    public interface ICountryService : IServiceBase<Country>
    {
        IEnumerable<Country> GetActiveRecords();
    }
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;

        public CountryService(ICountryRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<Country> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.CountryName);
            return entities;
        }

        public Country GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Country Create(Country objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Country objectToUpdate)
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
        
        public Country GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Country> GetActiveRecords()
        {
            return repository.GetAll();
        }

        public IEnumerable<Country> GetMany(Expression<Func<Country, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
