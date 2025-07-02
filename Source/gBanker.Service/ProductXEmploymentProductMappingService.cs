using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data;
using BasicDataAccess;
using BasicDataAccess.Data;

namespace gBanker.Service
{
    public interface IProductXEmploymentProductMappingService : IServiceBase<ProductXEmploymentProductMapping>
    {
      List<ProductXEmploymentProductMapping> GetEmploymentMappingList();
        void DeleteById(int id);
    }
    public class ProductXEmploymentProductMappingService : IProductXEmploymentProductMappingService
    {
        private readonly IProductXEmploymentProductMappingRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
       

        public ProductXEmploymentProductMappingService(IProductXEmploymentProductMappingRepository repository, IUnitOfWorkCodeFirst unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
          
        }
       
        public ProductXEmploymentProductMapping GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public ProductXEmploymentProductMapping Create(ProductXEmploymentProductMapping objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(ProductXEmploymentProductMapping objectToUpdate)
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
        public void DeleteById(int id)
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
    
        public IEnumerable<Area> GetMany(Expression<Func<Area, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductXEmploymentProductMapping> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            throw new NotImplementedException();
        }

        public bool IsContinued(long id)
        {
            throw new NotImplementedException();
        }

        public ProductXEmploymentProductMapping GetByIdLong(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductXEmploymentProductMapping> GetMany(Expression<Func<ProductXEmploymentProductMapping, bool>> where)
        {
            throw new NotImplementedException();
        }
        public List<ProductXEmploymentProductMapping> GetEmploymentMappingList()
        {
            try
            {
                var list = repository.GetAll().OrderBy(f=>f.DisplayOrder).ToList();
                return list;
            }
            catch (Exception ex)
            {
                return new List<ProductXEmploymentProductMapping>();
            }
        }
    }
}
