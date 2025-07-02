using BasicDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Service.ReportServies
{
    public interface ITodaysSummaryReportService
    {
        DataSet GetDataProductInfo<TParamOType>(TParamOType target) where TParamOType : class;
    }
    public class TodaysSummaryReportService : ITodaysSummaryReportService
    {
        public DataSet GetDataProductInfo<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_GetRptTodaySummary";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }
    }
}
