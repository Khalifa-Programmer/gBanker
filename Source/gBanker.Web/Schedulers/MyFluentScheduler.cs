using FluentScheduler;
using gBanker.Data.CodeFirstMigration;
using gBanker.Service;
using gBanker.Service.ReportServies;
using gBanker.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Data;

namespace gBanker.Web.Schedulers
{
    public class MyFluentScheduler : Registry
    {
        //private readonly IGroupwiseReportService groupwiseReportService; //Khalid

        public string StartWorkTime;
        public string CompleteWorkTime;
        public string MonthClosingDate;

        private static string processStarted = "N";
        //public string CompleteWorkProcess;

         
         public MyFluentScheduler()
         {
             //IGroupwiseReportService groupwiseReportService = new 
             #region variable
             

            
             //this.CompleteWorkProcess = "N";
            //Schedule(() =>
            //{
            //    RunScheduler(DateTime.Now.ToShortDateString());
            //}).ToRunEvery(30).Seconds();

             Schedule(() => // Run Process
            {
               // RunProcess();
            }).ToRunEvery(30).Seconds();
            
            Schedule(() => //Get Settings
            {
               // getData();
            }).ToRunEvery(30).Seconds();

             #endregion

         }
        private static void RunScheduler(string msg)
        {
            try
            {
               
                ISchedulerService schedulerService = new SchedulerService();
                if (schedulerService != null)
                {
                    foreach (var item in schedulerService.GetAll())
                    {
                        if (NeedRun(item))
                        {
                            if (item.SchedulerName == "DayEnd")
                            {
                               // var dayEndSchedululer = DependencyResolver.Current.GetService<IDayEndService>();
                                schedulerService.DayEndProcess(67, Convert.ToDateTime("2015-02-11"));

                            }
                            schedulerService.UpdateSchedulerList(Convert.ToDateTime("2015-02-11"), Convert.ToDateTime("2015-02-11"), Convert.ToDateTime("2015-02-11"), "sadsa", 67);
                        }
                        
                    }
                }
                //var date = DateTime.Now.ToString("MMddyyyy");
                //var logPath = string.Format(@"D:\LogFiles\{0}\Fluent{0}_{1}.txt", "ABC", date);
                //if (!Directory.Exists(@"D:\LogFiles\" + "ABC"))
                //    Directory.CreateDirectory(@"D:\LogFiles\" + "ABC");
                //using (StreamWriter sr = File.AppendText(logPath))
                //{
                //    sr.WriteLine(string.Format("Log Time: {0} - Msg: {1}", DateTime.Now, msg));

                //}
            }
            catch (Exception ex)
            {

            }
        }

        public void RunProcess()
        {
            try
            { //if it is Time Then Start Process

                DateTime d = DateTime.Now;
                string TimeNow = d.Hour.ToString("00") + ":" + d.Minute.ToString("00");// +":" + d.Second.ToString();
                var paramss = new { id = 1 };
                //if (TimeNow == StartWorkTime && processStarted == "N") //Start  work process
                if (TimeNow == StartWorkTime ) //Start  work process
                {
                    var result = "OK";

                    GroupwiseReportService nwGroupwiseReportService = new GroupwiseReportService();
                    //Get Office List
                   
                    var List = nwGroupwiseReportService.GetDataUltimateReleaseReport(paramss, "GetOfficeList");

                    List<ProcessConfigViewModel> OfficeList = new List<ProcessConfigViewModel>();
                    OfficeList = List.Tables[0].AsEnumerable()
                   .Select(row => new ProcessConfigViewModel
                   {
                       OfficeID = row.Field<int>("OfficeID"),
                       ClosingDate = row.Field<DateTime>("ClosingDate"),
                       OrgID = row.Field<int>("OrgID")

                   }).ToList();

                    foreach (var office in OfficeList)
                    {
                        var param = new
                        {
                            OfficeId = office.OfficeID,  //OfficeId
                            BusinessDate = office.ClosingDate,
                            CreateUser = "sa",
                            CreateDate = DateTime.Now,
                            OrgID = office.OrgID

                        };
                        var value = nwGroupwiseReportService.GetDataUltimateReleaseReport(param, "Prcs_DayInitial");
                    
                    }
                     
                    Thread.Sleep(70000); //Delay 70 seconds
                   // processStarted = "Y";
                  
                } // End of Start Work Process

                if (TimeNow == CompleteWorkTime) //Complete  work process
                {
                    var result = "OK";

                    GroupwiseReportService nwGroupwiseReportService = new GroupwiseReportService();


                    var List = nwGroupwiseReportService.GetDataUltimateReleaseReport(paramss, "GetOfficeList");

                    List<ProcessConfigViewModel> OfficeList = new List<ProcessConfigViewModel>();
                    OfficeList = List.Tables[0].AsEnumerable()
                   .Select(row => new ProcessConfigViewModel
                   {
                       OfficeID = row.Field<int>("OfficeID"),
                       ClosingDate = row.Field<DateTime>("ClosingDate"),
                       OrgID = row.Field<int>("OrgID")

                   }).ToList();

                    foreach (var office in OfficeList)
                    {
                        var param = new
                        {
                            OfficeId = office.OfficeID,  //OfficeId
                            BusinessDate = office.ClosingDate,

                            OrgID = office.OrgID

                        };
                        var value = nwGroupwiseReportService.GetDataUltimateReleaseReport(param, "Prcs_DayEnd");

                    }
                     
                   // var param = new
                   // {
                   //     OfficeId = 4,
                   //     BusinessDate = DateTime.Now,
                   //     CreateUser = "sa",
                   //     CreateDate = DateTime.Now,
                   //     OrgID = 4

                   // };// Not Yet Finalize

                   //// var List = nwGroupwiseReportService.GetDataUltimateReleaseReport(param, "Prcs_DayInitial");

                    Thread.Sleep(60000); //Delay 60 seconds

                } // End of Complete Work Process







               
            }
            catch (Exception ex)
            {

            }
        }

        public  void getData()
        {
          List<ProcessConfigViewModel> List_ViewModel = new List<ProcessConfigViewModel>();
        //Start Work Time
           GroupwiseReportService nwGroupwiseReportService = new GroupwiseReportService();
            var param = new { @Processid = 1 };
            var List = nwGroupwiseReportService.GetDataUltimateReleaseReport(param, "GetCurrentSetting");

            if (List.Tables[0].Rows.Count > 0)
            {
                var time = List.Tables[0].Rows[0]["Time"].ToString(); // List..[0]["Time"].toString;
                var kActive = List.Tables[0].Rows[0]["IsActive"].ToString();
                if (time != "" && Convert.ToBoolean( kActive) == true)
                {

                    StartWorkTime = time;
                }
            }
            else
            {
                StartWorkTime = "00.00";
            }
        //End StartWorkTime
        //CompleteWorkTime
            GroupwiseReportService nwGroupwiseReportService2 = new GroupwiseReportService();
            var param2 = new { @Processid = 2 };
            var List2 = nwGroupwiseReportService2.GetDataUltimateReleaseReport(param2, "GetCurrentSetting");
            if (List2.Tables[0].Rows.Count > 0)
            {
                var time2 = List2.Tables[0].Rows[0]["Time"].ToString(); // List..[0]["Time"].toString;
                var kActive2 = List2.Tables[0].Rows[0]["IsActive"].ToString();
                if (time2 != "" && Convert.ToBoolean(kActive2) == true)
                {
                    CompleteWorkTime = time2;
                }
            }
            else
            {
                CompleteWorkTime = "00.00";
            }
        //End Work Time        



        
        
        }//End GetData




        private static bool NeedRun(Scheduler scheduler)
        {
            var needRun = false;
            var getScheduleHour = scheduler.StartTime.Value.Hour;
            var getScheduleMinute = scheduler.StartTime.Value.Minute;
            var getLastRun = scheduler.LastRun;
            var getRunevery = Convert.ToDouble(scheduler.RunEvery);
            var startTime = scheduler.StartTime;
            var scheduleruntime =DateTime.Today.AddHours( startTime.Value.Hour).AddMinutes(startTime.Value.Minute);

            switch (scheduler.Frequency)
            {
                case "D":
                    //TODO: Apply logic   

                    if (getLastRun == null)
                    {
                        needRun = DateTime.Now >= scheduleruntime;
                        break;
                    }
                    if (getLastRun != null)
                    {
                        var nextScheduleTime = getLastRun.Value.AddDays(getRunevery).AddHours(getScheduleHour).AddMinutes(getScheduleMinute);
                        if (System.DateTime.Now >= Convert.ToDateTime(nextScheduleTime))
                        {
                            needRun = true;
                            break;
                        }
                        else
                            needRun = false;
                        break;
                    }
                    else
                        needRun = false;
                    
                    break;
                case "M":
                    //TODO: Apply logic
                    
                    if (getLastRun == null)
                    {
                        needRun = DateTime.Now >= scheduleruntime;
                        break;
                    }
                    if (getLastRun != null)
                    {
                        var nextScheduleTime = getLastRun.Value.AddDays(getRunevery).AddHours(getScheduleHour).AddMinutes(getScheduleMinute);
                        if (System.DateTime.Now >= Convert.ToDateTime(nextScheduleTime))
                        {
                            needRun = true;
                            break;
                        }
                        else
                            needRun = false;
                        break;
                    }
                    else
                        needRun = false;

                    break;
                case "Y":
                     if (getLastRun == null)
                    {
                        needRun = DateTime.Now >= scheduleruntime;
                        break;
                    }
                    if (getLastRun != null)
                    {
                        var nextScheduleTime = getLastRun.Value.AddDays(getRunevery).AddHours(getScheduleHour).AddMinutes(getScheduleMinute);
                        if (System.DateTime.Now >= Convert.ToDateTime(nextScheduleTime))
                        {
                            needRun = true;
                            break;
                        }
                        else
                            needRun = false;
                        break;
                    }
                    else
                        needRun = false;

                    break;
                default:
                    break;
            }

            return needRun;
        }
       

    }
}