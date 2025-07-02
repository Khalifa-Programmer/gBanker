using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IMonthClosingRepository : IRepository<SavingSummary>
    {
        int MonthlyProcess(int? OfficeId,int? OrgID, DateTime? vDate);
        int MonthlyProcessAverageMethod(int? OfficeId,int? OrgID, DateTime? vDate);

    }
   public  class MonthClosingRepository: RepositoryBaseCodeFirst<SavingSummary>, IMonthClosingRepository
    {
       public MonthClosingRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

       public int MonthlyProcess(int? OfficeId, int? OrgID, DateTime? vDate)
       {
          
           var officeIdParameter = new SqlParameter("@OfficeID", OfficeId);
           var orgIdParameter = new SqlParameter("@OrgID", OrgID);
           var dateParameter = new SqlParameter("@MonthEndDate", vDate);
           return DataContext.Database.ExecuteSqlCommand("monthlyProcess @OfficeID,@OrgID,@MonthEndDate", officeIdParameter, orgIdParameter, dateParameter);
       }


       public int MonthlyProcessAverageMethod(int? OfficeId, int? OrgID, DateTime? vDate)
       {
           var officeIdParameter = new SqlParameter("@officeID", OfficeId);
           var orgIdParameter = new SqlParameter("@OrgID", OrgID);
           var dateParameter = new SqlParameter("@ProcessDate", vDate);
           return DataContext.Database.ExecuteSqlCommand("monthlyProcessAverageMethod @officeID,@OrgID,@ProcessDate", officeIdParameter, orgIdParameter, dateParameter);
       }
    }
}
