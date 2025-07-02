using gBanker.Core.Filters;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.DBDetailModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface ISMSSenderRepository : IRepository<SMS_SentLog>
    {
        Task<IEnumerable<SentLogSMSSummaryModel>> GetSentLogSMSSummaryByFilter(BaseSearchFilter filter);
    }
    public class SMSSenderRepository : RepositoryBaseCodeFirst<SMS_SentLog>, ISMSSenderRepository
    {
        public SMSSenderRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {
            
        }

        public async Task<IEnumerable<SentLogSMSSummaryModel>> GetSentLogSMSSummaryByFilter(BaseSearchFilter filter)
        {
            var listing = new List<SentLogSMSSummaryModel>();

            try
            {
                var sqlCommand = $@"[dbo].[SMS_SentLog_GetSMSSummary] {filter.OfficeIds},'{filter.StartDateInString}','{filter.EndDateInString}',{filter.PageNumber},{filter.PageSize}";
                using (var db = new gBankerDbContext())
                {
                    listing = await db.Database.SqlQuery<SentLogSMSSummaryModel>(sqlCommand).ToListAsync();

                    if (listing.Any())
                    {
                        filter.TotalCount = listing[0].TotalCount;
                    }
                }
            }
            catch (Exception ex)
            {
                return new List<SentLogSMSSummaryModel>();
            }

            return listing;
        }
    }
}
