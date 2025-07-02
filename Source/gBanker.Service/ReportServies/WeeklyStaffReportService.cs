using BasicDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gBanker.Service.ReportServies
{
    public interface IWeeklyStaffReportService
    {
        DataSet GetDataStaffwiseWeeklyReport<TParamOType>(TParamOType target) where TParamOType : class;

    }
   public class WeeklyStaffReportService : IWeeklyStaffReportService
    {
        public DataSet GetDataStaffwiseWeeklyReport<TParamOType>(TParamOType target) where TParamOType : class
        {
            var storeProcedureName = "Proc_Rpt_MFI_Weekly_Statement_New";
            using (var gbData = new gBankerDataAccess())
            {
                return gbData.GetDataOnDateset(storeProcedureName, target);
            }
        }

    }
}

