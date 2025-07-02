using gBanker.Core.Filters;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IOLRSAccChartMappingRepository : IRepository<OLRSAccChartMapping>
    {
        Task<IEnumerable<AccChart>> GetAccChartListByLevel(AccChartSearchFilter filter);
        Task<IEnumerable<AccChart>> GetAccChartList(AccChartSearchFilter filter);
        Task<IEnumerable<PO_A_ACC_HEADModel>> GetPOAccChartList();
        Task<IEnumerable<OLRSAccChartMapping>> GetOLRSAccChartMappingList();
        bool CheckAccChartMappingDuplicacy(OLRSAccChartMapping param);
        Task<bool> AddAccChartMapping(OLRSAccChartMapping accChartMapping);        
    }
    public class OLRSAccChartMappingRepository : RepositoryBaseCodeFirst<OLRSAccChartMapping>, IOLRSAccChartMappingRepository
    {
        public OLRSAccChartMappingRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public async Task<IEnumerable<AccChart>> GetAccChartList(AccChartSearchFilter filter)
        {
            var listing = new List<AccChart>();
            try
            {
                var sqlCommand = $@"[dbo].[AccChart_GetAccChartsByLevelAndCode] '{filter.AccChartLevel}','{filter.AccCode}'";

                listing = await DataContext.Database.SqlQuery<AccChart>(sqlCommand).ToListAsync();

                return listing;
            }
            catch (Exception ex)
            {
                return new List<AccChart>();
            }
        }

        public async Task<IEnumerable<AccChart>> GetAccChartListByLevel(AccChartSearchFilter filter)
        {
            var listing = new List<AccChart>();
            try
            {
                var sqlCommand = $@"[dbo].[AccChart_GetAccChartsByLevel] '{filter.AccChartLevel}'";

                listing = await DataContext.Database.SqlQuery<AccChart>(sqlCommand).ToListAsync();

                return listing;
            }
            catch (Exception ex)
            {
                return new List<AccChart>();
            }
        }

        public async Task<IEnumerable<PO_A_ACC_HEADModel>> GetPOAccChartList()
        {
            var listing = new List<PO_A_ACC_HEADModel>();
            try
            {
                var sqlCommand = $@"[pksf].[AccChart_GetPO_A_ACC_HEAD]";

                listing = await DataContext.Database.SqlQuery<PO_A_ACC_HEADModel>(sqlCommand).ToListAsync();

                return listing;
            }
            catch (Exception ex)
            {
                return new List<PO_A_ACC_HEADModel>();
            }
        }

        public async Task<IEnumerable<OLRSAccChartMapping>> GetOLRSAccChartMappingList()
        {
            var listing = new List<OLRSAccChartMapping>();
            try
            {
                IQueryable<OLRSAccChartMapping> olrsAccChartMappings = DataContext.OLRSAccChartMappings.Where(f => f.IsActive);
                listing = await olrsAccChartMappings.ToListAsync();

                return listing;
            }
            catch (Exception ex)
            {
                return new List<OLRSAccChartMapping>();
            }
        }
        public bool CheckAccChartMappingDuplicacy(OLRSAccChartMapping param)
        {
            try
            {
                var existingMappings = DataContext.OLRSAccChartMappings.Where(f => f.POCode == param.POCode && f.AccCode == param.AccCode && f.AccCodeOLRS == param.AccCodeOLRS);
                if (existingMappings.Any())
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> AddAccChartMapping(OLRSAccChartMapping accChartMapping)
        {
            try
            {
                var existingMappings = DataContext.OLRSAccChartMappings.Where(f =>f.POCode==accChartMapping.POCode && f.AccCode == accChartMapping.AccCode && f.AccCodeOLRS == accChartMapping.AccCodeOLRS);
                DataContext.OLRSAccChartMappings.RemoveRange(existingMappings);

                DataContext.OLRSAccChartMappings.Add(accChartMapping);
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
