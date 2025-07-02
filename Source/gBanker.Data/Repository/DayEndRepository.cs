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
    public interface IDayEndRepository : IRepository<Prcs_DayEnd_Result>
    {
        int DayEndProcess(int? OfficeId, DateTime? vDate,int? OrgID);
        int PortFOlioYearClosingProcess(int? OfficeID, DateTime? YearEndDate, int? OrgID);
        int PostToLedgerProcess(int? OfficeID, DateTime? TransDate, int? OrgID);
        int PreYearClosingProcess(int? OfficeID, DateTime? YearEndDate);

    }
    public class DayEndRepository: RepositoryBaseCodeFirst<Prcs_DayEnd_Result>, IDayEndRepository
    {
        public DayEndRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }

        public int DayEndProcess(int? OfficeId, DateTime? vDate,int? OrgID)
        {
            var officeIdParameter = new SqlParameter("@OfficeId", OfficeId);
            var date = new SqlParameter("@BusinessDate", vDate);
            var vOrgID = new SqlParameter("@OrgID", OrgID);
            return DataContext.Database.ExecuteSqlCommand("Prcs_DayEnd @OfficeId,@BusinessDate,@OrgID", officeIdParameter, date, vOrgID);
        }


        public int PortFOlioYearClosingProcess(int? OfficeID, DateTime? YearEndDate, int? OrgID)
        {
            var officeIdParameter = new SqlParameter("@OfficeID", OfficeID);
            var date = new SqlParameter("@YearEndDate", YearEndDate);
            var vOrgID = new SqlParameter("@OrgID", OrgID);
            return DataContext.Database.ExecuteSqlCommand("PortFOlioYearClosingProcess @OfficeID,@YearEndDate,@OrgID", officeIdParameter, date, vOrgID);
        }


        public int PostToLedgerProcess(int? OfficeID, DateTime? TransDate, int? OrgID)
        {
            var officeIdParameter = new SqlParameter("@OfficeID", OfficeID);
            var date = new SqlParameter("@TransDate", TransDate);
            var vOrgID = new SqlParameter("@OrgID", OrgID);
            return DataContext.Database.ExecuteSqlCommand("PostToLedgerProcess @OfficeID,@TransDate,@OrgID", officeIdParameter, date, vOrgID);

        }


        public int PreYearClosingProcess(int? OfficeID, DateTime? YearEndDate)
        {
            var officeIdParameter = new SqlParameter("@OfficeID", OfficeID);
            var date = new SqlParameter("@ClosingDate", YearEndDate);
            return DataContext.Database.ExecuteSqlCommand("Prcs_PreclosingProcess @OfficeID,@ClosingDate", officeIdParameter, date);
        }
    }
}
