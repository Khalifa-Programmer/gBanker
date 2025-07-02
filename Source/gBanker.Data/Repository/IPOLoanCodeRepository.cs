using gBanker.Core.Filters;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels.OLRSHubs.CommonModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IPOLoanCodeRepository : IRepository<POLoanCode>
    {
        Task<IEnumerable<POLoanCode>> GetPOLoanCodes();

        Task<IEnumerable<POLoanCodeRelatedAccCodeModel>> GetPOAccCodes(BaseSearchFilter filter);


        Task<bool> GetPOLoanCodes(List<POLoanCode> pOLoanCodes);

        Task<bool> Manage_IMP_COST_LN_SC(IMP_COST_LN_SC_INSERT_Model model);

    }
    public class POLoanCodeRepository : RepositoryBaseCodeFirst<POLoanCode>, IPOLoanCodeRepository
    {
        public POLoanCodeRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public async Task<IEnumerable<POLoanCode>> GetPOLoanCodes()
        {
            var listing = new List<POLoanCode>();
            try
            {
                IQueryable<POLoanCode> query = DataContext.POLoanCodes.OrderBy(f=>f.Id);

                listing = await query.ToListAsync();
            }
            catch
            {
                return new List<POLoanCode>();
            }

            return listing;
        }

        public async Task<IEnumerable<POLoanCodeRelatedAccCodeModel>> GetPOAccCodes(BaseSearchFilter filter)
        {
            var listing = new List<POLoanCodeRelatedAccCodeModel>();
            try
            {
                string sqlCommand = $@"[pksf].[AccChart_GetAccChartByFilter] '{filter.ReportName}','{filter.ReportType}'";
                listing = await DataContext.Database.SqlQuery<POLoanCodeRelatedAccCodeModel>(sqlCommand).ToListAsync();
            }
            catch
            {
                return new List<POLoanCodeRelatedAccCodeModel>();
            }

            return listing;
        }

        public async Task<bool> Manage_IMP_COST_LN_SC(IMP_COST_LN_SC_INSERT_Model model)
        {
           
            try
            {
                string sqlCommand = $@"[pksf].[PRA_MN_IMP_COST_LN_SC_ManageServiceCharge] '{model.PO_CODE}','{model.MNYR}',{model.Sc_Rate},'{model.LoanCode}',{model.LoanServiceAmount},'{model.INS_USER}'";
                await DataContext.Database.ExecuteSqlCommandAsync(sqlCommand);
            }
            catch
            {
                return false;
            }

            return true;
        }


        public async Task<bool> GetPOLoanCodes(List<POLoanCode> pOLoanCodes)
        {
            var isUpdated = true;
            try
            {
                foreach (var item in pOLoanCodes)
                {
                    var updatePOLoanCode = await DataContext.POLoanCodes.FirstOrDefaultAsync(f => f.LoanCode==item.LoanCode);

                    if (updatePOLoanCode == null)
                        continue;
                    updatePOLoanCode.AssociatedAccCodeFA = item.AssociatedAccCodeFA;
                    updatePOLoanCode.AssociatedAccCodeSCP = item.AssociatedAccCodeSCP;
                    updatePOLoanCode.UpdateUser = item.UpdateUser;
                    updatePOLoanCode.UpdateDate = item.UpdateDate;

                    await DataContext.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                return false;
            }

            return isUpdated;
        }

    }
}
