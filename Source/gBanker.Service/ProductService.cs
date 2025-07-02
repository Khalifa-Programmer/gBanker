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
using gBanker.Data.DBDetailModels;

namespace gBanker.Service
{
    public interface IProductService : IServiceBase<Product>
    {
        //bool IsValidProduct(Product product);
        IEnumerable<ProductMainCodeModel> GetProductMainCodeList();
        IEnumerable<ValidationResult> IsValidProduct(Product product);
        IEnumerable<Product> SearchProduct(int Prodtype,int OrgID,string ItemType);
        IEnumerable<Product> SearchProductForLoanEligible(int Prodtype, int OrgID, string ItemType,int ProductID);
        IEnumerable<Product> CategoryProduct(int Prodtype, int OrgID);
        IEnumerable<DailyLoanTrx> SearchProductDaily(int Prodtype, int OrgID);
        IEnumerable<Product> SearchAllProduct(int OrgID);
        IEnumerable<Product> GetProductDetailPaged(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long totalCount, int? OrgID);
        IEnumerable<ProductCodeModel> GetProductCodeList();
        //IEnumerable<ProcessDayEnd_Result> ProcessDayEnd(DateTime dayend, string user);
        // bool IsContinued(int id);
    }
    public  class ProductService : IProductService
    {
        private readonly IProductRepository repository;
        private readonly IUnitOfWorkCodeFirst unitOfWork;
        private readonly ILoanCollectionRepository LoanCollectionrepository;

        public ProductService(IProductRepository repository, IUnitOfWorkCodeFirst unitOfWork, ILoanCollectionRepository LoanCollectionrepository)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.LoanCollectionrepository = LoanCollectionrepository;
        }

        public IEnumerable<ProductMainCodeModel> GetProductMainCodeList()
        {
            try
            {               
                return repository.GetProductMainCodeList();
            }
            catch (Exception ex)
            {
                return new List<ProductMainCodeModel>();
            }
        }
        public IEnumerable<ProductCodeModel> GetProductCodeList()
        {
            try
            {
                return repository.GetProductCodeList();
            }
            catch (Exception ex)
            {
                return new List<ProductCodeModel>();
            }
        }
        public IEnumerable<Product> GetAll()
        {
            var entities = repository.GetAll().OrderBy(c => c.ProductName);
            return entities;
        }

        public Product GetById(int id)
        {
            var entity = repository.GetById(id);
            return entity;
        }

        public Product Create(Product objectToCreate)
        {
            repository.Add(objectToCreate);
            Save();
            return objectToCreate;
        }

        public void Update(Product objectToUpdate)
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
        //public bool IsValidProduct(Product product)
        //{
        //    var entity = repository.Get(p => p.ProductCode == product.ProductCode || p.ProductName==product.ProductName);
        //    return entity == null ? true : false; ;
        //}

        public void Save()
        {
            unitOfWork.Commit();
        }




        public IEnumerable<Product> SearchProduct(int Prodtype, int OrgID, string ItemType)
        {
            if (ItemType == "S")
            {
                  return repository.GetMany(g => g.IsActive == true && g.ProductType == Prodtype && g.OrgID==OrgID).OrderBy(g => g.ProductCode);
            }
            else if (ItemType=="L")
            {
                return repository.GetMany(g => g.IsActive == true && g.ProductType != Prodtype && g.OrgID == OrgID).OrderBy(g => g.ProductCode);
            }
            if (ItemType == "B")
            {
                return repository.GetMany(g => g.IsActive == true && g.ProductType == Prodtype && g.OrgID == OrgID).OrderBy(g => g.ProductCode);
            }
            if (ItemType == "M")
            {
                return repository.GetMany(g => g.IsActive == true && g.ProductType == Prodtype && g.OrgID == OrgID).OrderBy(g => g.ProductCode);
            }
            else
                return repository.GetMany(g => g.IsActive == true &&  g.OrgID == OrgID).OrderBy(g => g.ProductCode);
        }


        public bool Inactivate(long id, DateTime? inactiveDate)
        {
            var obj = repository.GetById(id);
            if (obj != null)
            {
                obj.InActiveDate = inactiveDate.HasValue? inactiveDate:DateTime.Now;
                obj.IsActive = false;
                repository.Update(obj);
                Save();
                return true;
            }
            return false;
        }
        //public IEnumerable<ProcessDayEnd_Result> ProcessDayEnd(DateTime dayend, string user)
        //{
        //    return repository.ProcessDayEnd(dayend, user);
        //}

       

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


        IEnumerable<ValidationResult> IProductService.IsValidProduct(Product product)
        {
            var entity = repository.Get(p => p.ProductCode == product.ProductCode && p.IsActive==true);

            if (entity != null)
            {

                yield return new ValidationResult("ProductCode", "Duplicate Product.");
              
            }
        }


        public IEnumerable<Product> SearchAllProduct(int OrgID)
        {
            return repository.GetMany(g => g.IsActive == true && g.OrgID==OrgID).OrderBy(g => g.ProductType);
        }


        public IEnumerable<Product> GetProductDetailPaged(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long totalCount,int? OrgID)
        {
            return repository.GetProductDetailPaged(filterColumnName, filterValue, startRowIndex, jtSorting,pageSize, out totalCount,OrgID);
        }


        public Product GetByIdLong(long id)
        {
            var entity = repository.GetById(id);
            return entity;
        }


        public IEnumerable<DailyLoanTrx> SearchProductDaily(int Prodtype, int OrgID)
        {
            return LoanCollectionrepository.GetMany(g => g.IsActive == true && g.OrgID == OrgID).OrderBy(g => g.ProductCode).Distinct();
        }


        public IEnumerable<Product> CategoryProduct(int Prodtype, int OrgID)
        {
            return repository.GetMany(g => g.IsActive == true && g.ProductType == Prodtype && g.OrgID == OrgID && g.MainProductCode=="21.00").OrderBy(g => g.ProductCode);
        }

        public IEnumerable<Product> SearchProductForLoanEligible(int Prodtype, int OrgID, string ItemType, int ProductID)
        {
            if (ItemType == "S")
            {
                return repository.GetMany(g => g.IsActive == true && g.ProductType == Prodtype && g.OrgID == OrgID && g.ProductID==ProductID).OrderBy(g => g.ProductCode);
            }
            else if (ItemType == "L")
            {
                return repository.GetMany(g => g.IsActive == true && g.ProductType != Prodtype && g.OrgID == OrgID && g.ProductID == ProductID).OrderBy(g => g.ProductCode);
            }
            if (ItemType == "B")
            {
                return repository.GetMany(g => g.IsActive == true && g.ProductType == Prodtype && g.OrgID == OrgID && g.ProductID == ProductID).OrderBy(g => g.ProductCode);
            }
            if (ItemType == "M")
            {
                return repository.GetMany(g => g.IsActive == true && g.ProductType == Prodtype && g.OrgID == OrgID && g.ProductID == ProductID).OrderBy(g => g.ProductCode);
            }
            else
                return repository.GetMany(g => g.IsActive == true && g.OrgID == OrgID && g.ProductID == ProductID).OrderBy(g => g.ProductCode);

        }
        public IEnumerable<Product> GetMany(Expression<Func<Product, bool>> where)
        {
            var entities = repository.GetMany(where).Where(b => b.IsActive == true);
            return entities;
        }       
    }
}
