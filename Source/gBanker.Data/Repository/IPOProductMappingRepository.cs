using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels.OLRSHubs.CommonModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace gBanker.Data.Repository
{
    public interface IPOProductMappingRepository : IRepository<POProductMapping>
    {
        Task<IEnumerable<POProductMapping>> GetPOProductMappings();
        Task<IEnumerable<LOAN_PRODUCT>> GetLOAN_PRODUCT_List();
        bool ManageProductXLoanMapping(List<POProductMapping> oProductMappings);
        Task<IEnumerable<POLoanCodeWithProductModel>> GetPOLoanCodeWiseProductMappings();        
    }
    public class POProductMappingRepository : RepositoryBaseCodeFirst<POProductMapping>, IPOProductMappingRepository
    {
        public POProductMappingRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        public async Task<IEnumerable<POProductMapping>> GetPOProductMappings()
        {
            var listing = new List<POProductMapping>();
            try
            {
                IQueryable<POProductMapping> query = DataContext.POProductMappings;
                listing = await query.ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<POProductMapping>();
            }

            return listing;
        }
        public async Task<IEnumerable<LOAN_PRODUCT>> GetLOAN_PRODUCT_List()
        {
            var listing = new List<LOAN_PRODUCT>();
            try
            {
                IQueryable<LOAN_PRODUCT> query = DataContext.LOAN_PRODUCTs;
                listing = await query.ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<LOAN_PRODUCT>();
            }

            return listing;
        }
        public async Task<IEnumerable<POLoanCodeWithProductModel>> GetPOLoanCodeWiseProductMappings()
        {
            var listing = new List<POLoanCodeWithProductModel>();
            try
            {
                var sqlCommand = $@"pksf.GetLoanCodeWiseProduct";
                listing = await DataContext.Database.SqlQuery<POLoanCodeWithProductModel>(sqlCommand).ToListAsync();
                return listing;
            }
            catch (Exception ex)
            {
                return new List<POLoanCodeWithProductModel>();
            }
        }

        public bool ManageProductXLoanMapping(List<POProductMapping> oProductMappings)
        {
            bool isOperationSuccess = true;

            using (var ts = new TransactionScope())
            {
                try
                {
                    //let's remove old items
                    var oldProductMappings = DataContext.POProductMappings.ToList();
                    DataContext.POProductMappings.RemoveRange(oldProductMappings);
                    DataContext.SaveChanges();

                    //let's add new list
                    DataContext.POProductMappings.AddRange(oProductMappings);
                    DataContext.SaveChanges();

                    var loanCodes = oProductMappings.GroupBy(g=>new { g.LoanCode, g.AssociatedLoanCode }).Select(s => s.First());
                    foreach (var item in loanCodes)
                    {
                        var sqlCommand = $@"[pksf].[POLoanCode_Update_OLRS_Associated_Loan_Code] '{item.LoanCode}','{item.AssociatedLoanCode}'";
                        DataContext.Database.ExecuteSqlCommand(sqlCommand);
                    }
                }
                catch (Exception ex)
                {
                    isOperationSuccess = false;
                }

                if (isOperationSuccess)
                    ts.Complete();

                ts.Dispose();
            }

            return isOperationSuccess;
        }
    }
}
