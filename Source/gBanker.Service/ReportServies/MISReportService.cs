using BasicDataAccess;
using BasicDataAccess.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Service.ReportServies
{
    public interface IMISReportService
    {
        DataSet GetDataWeeklyMonitoringReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataMonthlyStatisticalReportProcess<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataMonthlyStatisticalReportProcessDateWise<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataMonthlyStatisticalReport<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataMonthlyProjectStatementReportProcess<TParamOType>(TParamOType target) where TParamOType : class;
        DataSet GetDataMonthlyProjectStatementReport<TParamOType>(TParamOType target) where TParamOType : class;
    }
    public class MISReportService : IMISReportService
    {
        public DataSet GetDataWeeklyMonitoringReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_WeeklyMonitoringReport";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataMonthlyStatisticalReportProcess<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Get_MonthlyStatisticalReportProcess";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataMonthlyStatisticalReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "fnc_Get_MonthlyStatisticalReport";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataMonthlyProjectStatementReportProcess<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Get_MonthlyProjectStatementReport";
            using (var gbData = new gBankerReportDataAccess())
           // using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

        public DataSet GetDataMonthlyProjectStatementReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "fnc_Get_MonthlyProjectStatementReport";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }


        public DataSet GetDataMonthlyStatisticalReportProcessDateWise<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "GetSetMonthlyStatisticalReportProcess";
            using (var gbData = new gBankerReportDataAccess())
            //using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
    }
}
