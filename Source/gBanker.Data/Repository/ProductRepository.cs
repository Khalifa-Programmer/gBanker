using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        //IEnumerable<ProcessDayEnd_Result> ProcessDayEnd(DateTime dayend, string user);
        IEnumerable<Product> GetProductDetailPaged(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long totalCount, int? OrgID);
        IEnumerable<ProductMainCodeModel> GetProductMainCodeList();
        IEnumerable<ProductCodeModel> GetProductCodeList();

    }
    public class ProductRepository : RepositoryBaseCodeFirst<Product>, IProductRepository
    {
        public ProductRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        //public IEnumerable<ProcessDayEnd_Result> ProcessDayEnd(DateTime dayend, string user)
        //{
        //    return DataContext.ProcessDayEnd(dayend, user);
        //}

        public IEnumerable<Product> GetProductDetailPaged(string filterColumnName, string filterValue, int startRowIndex, string jtSorting, int pageSize, out long totalCount, int? OrgID)
        {
            IQueryable<Product> results = null;
            if (filterColumnName == "ProductCode" && !string.IsNullOrWhiteSpace(filterValue))
                results = DataContext.Products.Where(p => p.IsActive == true && p.ProductCode.Contains(filterValue) && p.OrgID == OrgID);
            else
                results = DataContext.Products.Where(p => p.IsActive == true && p.OrgID == OrgID);

            totalCount = results.LongCount();
            var obj = results.OrderBy(x => x.ProductCode).Skip(startRowIndex).Take(pageSize);
            if (!string.IsNullOrWhiteSpace(jtSorting))
            {
                if (jtSorting == "ProductName ASC")
                    obj = results.OrderBy(ord => ord.ProductName).Skip(startRowIndex).Take(pageSize);
                else if (jtSorting == "ProductCode ASC")
                    obj = results.OrderBy(ord => ord.ProductCode).Skip(startRowIndex).Take(pageSize);
                else if (jtSorting == "ProductName DESC")
                    obj = results.OrderByDescending(ord => ord.ProductName).Skip(startRowIndex).Take(pageSize);
                else if (jtSorting == "ProductCode DESC")
                    obj = results.OrderByDescending(ord => ord.ProductCode).Skip(startRowIndex).Take(pageSize);
            }
            else
                obj = results.OrderBy(ord => ord.ProductID).Skip(startRowIndex).Take(pageSize);


            return obj;
        }
        public IEnumerable<ProductMainCodeModel> GetProductMainCodeList()
        {
            try
            {
                var sqlCommand = "select MainProductCode, MainItemName  from Product group by MainProductCode,MainItemName";

                var results = DataContext.Database.SqlQuery<ProductMainCodeModel>(sqlCommand).ToList();

                return results;
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
                var sqlCommand = " SELECT Cast(pt.ProductID as Int) as ProductCode, CONCAT(pt.ProductCode,' ',pt.ProductName) as ProductName FROM Product pt WHERE pt.ProductType = 1 AND LEFT(pt.ProductCode, 1) <> '7'  ";

                var results = DataContext.Database.SqlQuery<ProductCodeModel>(sqlCommand).ToList();

                return results;
            }
            catch (Exception ex)
            {
                return new List<ProductCodeModel>();
            }
        }
    }
}
