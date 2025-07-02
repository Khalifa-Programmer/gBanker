using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface ITodaysSummaryRepository : IRepository<Proc_GetRptTodaySummary_Result>
    {
        int GetTodaysSummary(int? qtype, int? officeID, DateTime? vdate);

    }
    public class TodaysSummaryRepository: RepositoryBaseCodeFirst<Proc_GetRptTodaySummary_Result>, ITodaysSummaryRepository
    {
        public TodaysSummaryRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }



        public int GetTodaysSummary(int? qtype, int? officeID, DateTime? vdate)
        {
            var qtypeParameter = new SqlParameter("@QType", qtype);
            var officeIdParameter = new SqlParameter("@OfficeID", officeID);
            var dateParameter = new SqlParameter("@TransactionDate", vdate);
            return DataContext.Database.ExecuteSqlCommand("Proc_GetRptTodaySummary @QType,@OfficeID,@TransactionDate", qtypeParameter, officeIdParameter, dateParameter);
        }
    }
}
