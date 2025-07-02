using gBanker.Core.Filters;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using gBanker.Data.DBDetailModels.OLRSHubs.CommonModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IIndicatorRepository : IRepository<Indicator>
    {
        Task<IEnumerable<Indicator>> GetIndicators();
        Task<IEnumerable<POIndicatorRelatedAccCodeModel>> GetPOAccCodes(BaseSearchFilter filter);
        Task<bool> UpdateIndicators(List<Indicator> indicators);

        Task<bool> AddManualProgramData(ProgramDataManualDataModel model);

        Task<bool> AddFinancialData(FinancialDataModel model);
        Task<bool> AddBasicData(BasicDataModel model);
        Task<IEnumerable<Indicator>> GetIndicatorList(BaseSearchFilter filter);
        Task<bool> AddIndicator(Indicator indicator);
        Task<IEnumerable<Indicator>> GetIndicatorListByIsManual(IndicatorSearchFilter filter);
        Task<IEnumerable<Indicator>> GetIndicatorsByFD();
        Task<IEnumerable<POIndicatorRelatedAccCodeModel>> GetPOAccCodesByFilter(BaseSearchFilter filter);
    }
    public class IndicatorRepository : RepositoryBaseCodeFirst<Indicator>, IIndicatorRepository
    {
        public IndicatorRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public async Task<IEnumerable<Indicator>> GetIndicators()
        {
            var listing = new List<Indicator>();
            try
            {
                IQueryable<Indicator> query = DataContext.Indicators.OrderBy(f => f.IndicatorCode);

                listing = await query.ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<Indicator>();
            }

            return listing;
        }
        public async Task<IEnumerable<Indicator>> GetIndicatorsByFD()
        {
            var listing = new List<Indicator>();
            try
            {
                IQueryable<Indicator> query = DataContext.Indicators.Where(f => f.AssociatedTable == "PRA_MN_RPT_TAB_XL_FD" && f.IsManual == false).OrderBy(f => f.IndicatorCode);

                listing = await query.ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<Indicator>();
            }

            return listing;
        }
        public async Task<IEnumerable<POIndicatorRelatedAccCodeModel>> GetPOAccCodes(BaseSearchFilter filter)
        {
            var listing = new List<POIndicatorRelatedAccCodeModel>();
            try
            {
                string sqlCommand = $@"[pksf].[AccChart_GetAccChartByFilter] '{filter.ReportName}','{filter.ReportType}'";
                listing = await DataContext.Database.SqlQuery<POIndicatorRelatedAccCodeModel>(sqlCommand).ToListAsync();
            }
            catch
            {
                return new List<POIndicatorRelatedAccCodeModel>();
            }

            return listing;
        }
        public async Task<IEnumerable<POIndicatorRelatedAccCodeModel>> GetPOAccCodesByFilter(BaseSearchFilter filter)
        {
            var listing = new List<POIndicatorRelatedAccCodeModel>();
            try
            {
                string sqlCommand = $@"[pksf].[AccChart_GetIndicatorAccChartByFilter]";
                listing = await DataContext.Database.SqlQuery<POIndicatorRelatedAccCodeModel>(sqlCommand).ToListAsync();
            }
            catch
            {
                return new List<POIndicatorRelatedAccCodeModel>();
            }

            return listing;
        }
        public async Task<bool> UpdateIndicators(List<Indicator> indicators)
        {
            var isUpdated = true;
            try
            {
                foreach (var item in indicators)
                {
                    var updatePOLoanCode = await DataContext.Indicators.FirstOrDefaultAsync(f => f.IndicatorCode == item.IndicatorCode);

                    if (updatePOLoanCode == null)
                        continue;
                    updatePOLoanCode.AssociatedAccCodeFD = item.AssociatedAccCodeFD;
                    updatePOLoanCode.UpdateUser = item.UpdateUser;
                    updatePOLoanCode.UpdateDate = item.UpdateDate;

                    await DataContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return isUpdated;
        }

        public async Task<IEnumerable<Indicator>> GetIndicatorList(BaseSearchFilter filter)
        {
            var listing = new List<Indicator>();
            try
            {

                //IQueryable<Indicator> indicators = DataContext.Indicators.Where(f=>f.AssociatedTable== filter.AssociatedTable && f.IsManual == true);
                IQueryable<Indicator> indicators = DataContext.Indicators
                    .Where(f => f.IsManual == true
                        && (filter.AssociatedTable == null || filter.AssociatedTable == "" || f.AssociatedTable == filter.AssociatedTable)
                    );
                listing = await indicators.ToListAsync();

                return listing;
            }
            catch (Exception ex)
            {
                return new List<Indicator>();
            }
        }
        public async Task<IEnumerable<Indicator>> GetIndicatorListByIsManual(IndicatorSearchFilter filter)
        {
            var listing = new List<Indicator>();
            try
            {
                IQueryable<Indicator> indicators = DataContext.Indicators.Where(f => f.AssociatedTable == filter.AssociatedTable && f.IsManual == true);
                listing = await indicators.OrderBy(o=>o.IndicatorCode).ToListAsync();

                return listing;
            }
            catch (Exception ex)
            {
                return new List<Indicator>();
            }
        }
        public async Task<bool> AddFinancialData(FinancialDataModel model)
        {
            try
            {
                var sqlCommand = $@"[pksf].[PRA_MN_RPT_TAB_XL_FD_InsertFinancialData] 
                    '{model.POCode}',
                    '{model.Ind_code}',
                    '{model.MNYR}',
                    '{model.FD_PKSF_FUND}',
                    {model.CreatedBy}
                ";

                var isAdded = await DataContext.Database.SqlQuery<bool>(sqlCommand).FirstOrDefaultAsync();

                return isAdded;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddBasicData(BasicDataModel model)
        {
            try
            {
                var sqlCommand = $@"[pksf].[PRA_MN_RPT_TAB_XL_BD_AddBasicData] 
                    '{model.po_code}',
                    '{model.ind_code}',
                    '{model.mnyr}',
                    '{model.m_f_flag}',
                    '{model.bd_pksf_fund}',
                    '{model.bd_non_pksf_fund}',
                    {model.created_by}
                    ";

                var isAdded = await DataContext.Database.SqlQuery<bool>(sqlCommand).FirstOrDefaultAsync();

                return isAdded;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddManualProgramData(ProgramDataManualDataModel model)
        {
            try
            {
                var sqlCommand = $@"[pksf].[PRA_MN_RPT_TAB_XL_PD_InsertManualData] 
                    '{model.OrganizationCode}',
                    '{model.MNYR}',
                    '{model.IndCode}',
                    '{model.LoanCode}',
                    '{model.M_F_FLAG}',
                    {model.Amount},
                    '{model.InsertUser}'
                ";

                var isAdded = await DataContext.Database.SqlQuery<bool>(sqlCommand).FirstOrDefaultAsync();

                return isAdded;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> AddIndicator(Indicator indicator)
        {
            try
            {
                DataContext.Indicators.Add(indicator);
                await DataContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
