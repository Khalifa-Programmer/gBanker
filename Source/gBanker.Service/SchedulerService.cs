using BasicDataAccess;
using gBanker.Data.CodeFirstMigration;
using gBanker.Data.CodeFirstMigration.InfrastructureBase;
using gBanker.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Service
{
    public interface ISchedulerService 
    {
        List<Scheduler> GetAll();
        int DayEndProcess(int? OfficeId, DateTime? vDate);
        int InsertSchedule(string SchedulerName, string Description, DateTime StartTime, int RunEvery, string Frequency, bool? IsActive, DateTime? CreateDate, string CreateUser, string Mode, Int64? SchedulerID);
        int UpdateSchedulerList(Nullable<System.DateTime> date, Nullable<System.DateTime> starttime, Nullable<System.DateTime> endtime, string errordescription, Nullable<int> officeId);
    }
    public class SchedulerService : ISchedulerService        
    {
        public List<Scheduler> GetAll()
        {

            var storeProcedureName = "GetSchedulerList";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetData<Scheduler>(storeProcedureName);
            }
        }


        public int DayEndProcess(int? OfficeId, DateTime? vDate)
        {
            var storeProcedureName = "Prcs_DayEnd";
            using (var gbData = new gBankerDataAccess())
            {
                var obj = new { OfficeId = 67, BusinessDate = Convert.ToDateTime("2015-02-11") };
                return gbData.ExecuteNonQuery<object>(storeProcedureName, obj);
            }
        }
        public int InsertSchedule(string SchedulerName, string Description, DateTime StartTime, int RunEvery, string Frequency, bool? IsActive, DateTime? CreateDate, string CreateUser, string Mode, Int64? SchedulerID)
        {
            var storeProcedureName = "SP_SET_Scheduler";
            using (var gbData = new gBankerDataAccess())
            {
                var obj = new { SchedulerName = SchedulerName, Description = Description, StartTime = StartTime, RunEvery = RunEvery, Frequency = Frequency, IsActive = IsActive, CreateDate = CreateDate, CreateUser = CreateUser, Mode = Mode, SchedulerID = SchedulerID };
                return gbData.ExecuteNonQuery<object>(storeProcedureName, obj);
            }
        }
        //public int UpdateProcess(int? OfficeId, DateTime? vDate, DateTime? starttime, DateTime? Endtime, string? errordescription)
        //{
        //    var storeProcedureName = "UpdateSchedulerList";
        //    using (var gbData = new gBankerDataAccess())
        //    {
        //        var obj = new { Date = Convert.ToDateTime("2015-02-11"), starttime = Convert.ToDateTime("2015-02-11"), Endtime = Convert.ToDateTime("2015-02-11"), errordescription = "", officeId="67" };
        //        return gbData.ExecuteNonQuery<object>(storeProcedureName, obj);
        //    }
        //}


        public int UpdateSchedulerList(DateTime? date, DateTime? starttime, DateTime? endtime, string errordescription, int? officeId)
        {
            var storeProcedureName = "UpdateSchedulerList";
            using (var gbData = new gBankerDataAccess())
            {
                var obj = new { Date = Convert.ToDateTime("2015-02-11"), starttime = Convert.ToDateTime("2015-02-11"), Endtime = Convert.ToDateTime("2015-02-11"), errordescription = "", officeId = "67" };
                return gbData.ExecuteNonQuery<object>(storeProcedureName, obj);
            }
        }
    }
}
