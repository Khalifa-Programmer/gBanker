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
    public interface IDayInitialRepository : IRepository<Prcs_DayInitial_Result>
    {
        int DayInitialProcess(int? OfficeId, DateTime? vDate);
        int DayInitialProcess(int? OfficeId, DateTime? vDate,string CreateUser,DateTime? CreateDate,int OrgID);
       // int ValidateDayInitial(int? OfficeId);
      
    }
    public class DayInitialRepository : RepositoryBaseCodeFirst<Prcs_DayInitial_Result>, IDayInitialRepository
    {
        public DayInitialRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }




        public int DayInitialProcess(int? OfficeId, DateTime? vDate)
        {
            var officeIdParameter = new SqlParameter("@OfficeId", OfficeId);
            var date = new SqlParameter("@BusinessDate", vDate);
            return DataContext.Database.ExecuteSqlCommand("Prcs_DayInitial @OfficeId,@BusinessDate" , officeIdParameter, date);

        }


        public int DayInitialProcess(int? OfficeId, DateTime? vDate, string CreateUser, DateTime? CreateDate,int OrgID)
        {
            var officeIdParameter = new SqlParameter("@OfficeId", OfficeId);
            var date = new SqlParameter("@BusinessDate", vDate);
            var vUser = new SqlParameter("@CreateUser", CreateUser);
            var vdate = new SqlParameter("@CreateDate", CreateDate);
            var vOrgID = new SqlParameter("@OrgID", OrgID);
            return DataContext.Database.ExecuteSqlCommand("Prcs_DayInitial @OfficeId,@BusinessDate,@CreateUser,@CreateDate,@OrgID", officeIdParameter, date, vUser, vdate, vOrgID);
        }
    }
}
