using gBanker.Data.CodeFirstMigration.Db;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
//using gBanker.Data.Db;
using gBanker.Data.DBDetailModels;
//using gBanker.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Data.Repository
{
    public interface IHolidayRepository : IRepository<Holiday>
    {
        int SetHoliDay(Nullable<int> officeID, Nullable<int> orgID, Nullable<System.DateTime> specificDate, string processType, string holidayDescription, string days, string year, string createUser);
        IEnumerable<DBholidayDetailModel> GetHolidayDetail(int OrgID);
    }
    public class HolidayRepository : RepositoryBaseCodeFirst<Holiday>, IHolidayRepository
    {
        public HolidayRepository(IDatabaseFactoryCodeFirst databaseFactory)
            : base(databaseFactory)
        {

        }
        public IEnumerable<DBholidayDetailModel> GetHolidayDetail(int OrgID)
        {
            var obj = DataContext.Holidays.Where(x => x.IsActive == true && x.OrgID==OrgID)
                .Select(s => new DBholidayDetailModel()
                {
                    HolidayID = s.HolidayID,
                    BusinessDate = s.BusinessDate,
                    OfficeID = s.OfficeID,
                    OfficeName = s.Office.OfficeName,
                    CenterID = s.CenterID,
                    CenterName = s.Center.CenterName,
                    Description = s.Description,
                    HolidayType = s.HolidayType,
                    IsActive = s.IsActive,
                    InActiveDate = s.InActiveDate
                });

            return obj;
        }

        public int SetHoliDay(int? OfficeID, int? OrgID, DateTime? SpecificDate, string processType, string holidayDescription, string days, string year, string createUser)
        {
            var officeIdParameter = new SqlParameter("@OfficeID", OfficeID);
            var OrgIDParameter = new SqlParameter("@OrgID", OrgID);
            var SpecificDateParameter = new SqlParameter("@SpecificDate", SpecificDate);
            var ProcessTypeParameter = new SqlParameter("@ProcessType", processType);
            var HolidayDescriptionParameter = new SqlParameter("@HolidayDescription", holidayDescription);
            var DaysParameter = new SqlParameter("@Days", days);
            var YearParameter = new SqlParameter("@year", year);
            var createUserParameter = new SqlParameter("@createUser", createUser);


            return DataContext.Database.ExecuteSqlCommand("SP_Holiday_Process @OfficeID,@OrgID,@SpecificDate,@ProcessType,@HolidayDescription,@Days,@year,@createUser", officeIdParameter, OrgIDParameter,SpecificDateParameter,ProcessTypeParameter,HolidayDescriptionParameter,DaysParameter,YearParameter,createUserParameter);
        }
    }
}
