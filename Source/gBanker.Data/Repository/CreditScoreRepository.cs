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
    public interface ICreditScoreRepository : IRepository<CreditScore>
    {
        int usp_rpt_credit_score(int? officeID, Nullable<System.DateTime> start_date, Nullable<System.DateTime> end_date);
        IEnumerable<GetCreditScore_Result> GetCreditScore(int? officeID);
    }
    public class CreditScoreRepository : RepositoryBaseCodeFirst<CreditScore>, ICreditScoreRepository
    {
        public CreditScoreRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public int usp_rpt_credit_score(int? officeID, DateTime? start_date, DateTime? end_date)
        {
            var officeIdParameter = new SqlParameter("@officeID", officeID);
            var startdate = new SqlParameter("@start_date", start_date);
            var enddate = new SqlParameter("@end_date", end_date);
            return DataContext.Database.ExecuteSqlCommand("usp_rpt_credit_score @officeID,@start_date,@end_date", officeIdParameter, startdate, enddate);
        }


        public IEnumerable<GetCreditScore_Result> GetCreditScore(int? officeID)
        {
            var officeIdParameter = new SqlParameter("@officeId", officeID);

            return DataContext.Database.SqlQuery<GetCreditScore_Result>("GetCreditScore @officeId", officeIdParameter);
        }
    }
}
